using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class RecipientService : GenericService<Recipient, int>, IRecipientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecipientRepository _recipientRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAddressService _addressService;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ICommonBaseDataService _commonBaseDataService;
        private readonly IAppConfigService _appConfigService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IYashilLogger<Recipient> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public RecipientService(IUnitOfWork unitOfWork,
            IRecipientRepository recipientRepository, IUserPrincipal userPrincipal,
            IAddressService addressService,
            ICommonBaseDataService commonBaseDataService, IEventPublisher eventPublisher, StoreConfiguration storeConfiguration, IAppConfigService appConfigService, IAdditionalUserPropService additionalUserPropService, IYashilLogger<Recipient> logger, ApplicationConfiguration applicationConfiguration)
            : base(unitOfWork, recipientRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _recipientRepository = recipientRepository;
            _userPrincipal = userPrincipal;
            _addressService = addressService;
            _commonBaseDataService = commonBaseDataService;
            _storeConfiguration = storeConfiguration;
            _appConfigService = appConfigService;
            _additionalUserPropService = additionalUserPropService;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        public IQueryable<Recipient> GetByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null)
        {
            return _recipientRepository.GetByCustomFilter(userId, addressId, membershipType, nationalCode);
        }

        public int GetCountByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null)
        {
            return _recipientRepository.GetCountByCustomFilter(userId, addressId, membershipType, nationalCode);
        }

        public async Task<bool> AddRecipientAsync(Recipient recipient, bool saveAfterInsert)
        {
            var recipientCount = GetCountByCustomFilter(
                _userPrincipal.Id,
                null,
                null);
            if (recipientCount < _storeConfiguration.MaximumRecipientNumber)
            {
                await AddAsync(recipient, saveAfterInsert);
                return true;
            }
            return false;
        }

        public Address CloneRecipientAddress(int recipientId)
        {
            return _recipientRepository.CloneRecipientAddress(recipientId);
        }

        public IQueryable<Recipient> GetUserRecipients(int userId)
        {
            return _recipientRepository.GetUserRecipients(userId);
        }

        public IQueryable<Recipient> GetIncludeAddress(int recipientId)
        {
            return _recipientRepository.GetIncludeAddress(recipientId);
        }

        public IQueryable<Recipient> GetUserRecipient(int recipientId, bool readOnly)
        {
            return _recipientRepository.GetUserRecipient(recipientId, readOnly);
        }

        public async Task<bool> UpdateSubsidyStatusTypeAsync(int? recipientId, int? subsidyType, string nationalCode, int? subsidyStatusType)
        {
            var recipient = _recipientRepository.Get(recipientId);
            if (recipient != null)
            {
                recipient.SubsidyStatusType = subsidyStatusType;
                recipient.NationalCodeForSubsidyApproval = nationalCode;
                recipient.SubsidyType = subsidyType;
                // اگر نوع یارانه ساتایی بود نوع عضویت به ساتا تغییر داده شود
                // اگر نوع یارانه وزارتی بود نوع عضویت به وزارتی تغییر داده شود
                // در غیر حالت های بالا نوع عضویت Null میباشد
                recipient.MembershipType = subsidyType == (int)SubsidyTypesEnum.Sata ?
                    (int)MembershipTypesEnum.Sata : subsidyType == (int)SubsidyTypesEnum.Vezarat ?
                        (int)MembershipTypesEnum.Vezarat : int.Parse(null!);
                var res = await SaveChangeAsync();
                if (res > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public IQueryable<Recipient> GetUserRecipientsByApplicationId(int userId, int applicationId)
        {
            return _recipientRepository.GetUserRecipientsByApplicationId(userId, applicationId);
        }

        public IQueryable<CommonBaseData> GetMembershipTypes(bool hekmatBased, bool subsidyBased)
        {
            var query = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.MembershipType.ToString());
            if (hekmatBased || subsidyBased)
            {
                query = query.Where(x => x.Id != (int)MembershipTypesEnum.Others);
            }
            else
            {
                query = query.Where(x => x.Id == (int)MembershipTypesEnum.Others);
            }
            return query;
        }

        public IQueryable<CommonBaseData> GetGenderTypes()
        {
            var query = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.Gender.ToString());
            return query;
        }

        public async Task<(bool isSuccess, string message, int? recipientId)> AddOrUpdateRecipientAsync(
            Recipient recipient, Address address, bool hasCardNumber)
        {
            // دریافت اطلاعات باشگاه مشتریان کاربر لاگین شده
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            if (!recipient.BirthDate.HasValue)
            {
                return (false, "کاربر گرامی لطفا تاریخ تولد خود را انتخاب کنید.", null);
            }
            if (!recipient.GenderType.HasValue)
            {
                return (false, "کاربر گرامی لطفا جنسیت را انتخاب کنید.", null);
            }
            // بررسی مقدار کد ملی
            if (!recipient.NationalCode.IsValidNationalCode())
            {
                return (false, "فرمت کد ملی صحیح نمی باشد.", null);
            }
            // بررسی مقدار شماره موبایل
            if (!recipient.MobileNumber.IsValidMobileNumber())
            {
                return (false, "فرمت شماره موبایل صحیح نمی باشد.", null);
            }

            // بررسی حکمت بیس بودن یا پشتیبانی از یارانه
            if (userAdditionalUserProp.SupportSubsidy || userAdditionalUserProp.HekmatBased)
            {
                // بررسی مقدار شماره کارت
                if (string.IsNullOrEmpty(recipient.CardNumber))
                {
                    return (false, "لطفا شماره کارت را وارد نمایید", null);
                }
                // بررسی معتبر بودن شماره کارت
                if (!recipient.CardNumber.IsValidValidateHekmatCard())
                {
                    return (false, "شماره کارت وارد شده صحیح نمی باشد", null);
                }
            }
            else if (hasCardNumber && (string.IsNullOrEmpty(recipient.CardNumber) ||
                                       !recipient.CardNumber.IsValidValidateHekmatCard()))
            {
                return (false, "کاربر گرامی لطفا شماره کارت حکمت 16 رقمی خود را بدرستی وارد نمایید.", null);
            }

            //اگر کاربر مبتنی بر یارانه باشد یا حکمتی باشد یا شماره کارت حکت وارد کرده باشد 
            //بررسی می کنیم که مقدار نسبت خریدار با تحویل گیرنده را نیز وارد کرده باشد.
            //اگر مقدار وارد نکرده باشد  یا برابر 0 باشد غیر قابل قبول است
            //اگر کاربر غیر حکمتی باشد عدم وارد کردن این فیلد مشکلی ندارد
            if ((userAdditionalUserProp.SupportSubsidy || userAdditionalUserProp.HekmatBased ||
                 !string.IsNullOrEmpty(recipient.CardNumber)) &&
                (!recipient.RelationshipWithBuyerType.HasValue || recipient.RelationshipWithBuyerType.Value == 0))
            {
                return (false, "کاربر گرامی، انتخاب نسبت خریدار با صاحب کارت حکمت اجباری می باشد.", null);
            }


            // بررسی شهر
            if (address.CityId == null)
            {
                return (false, "کاربر گرامی لطفا شهر را وارد نمایید.", null);
            }

            //بررسی مقادیر اجباری فرم در صورتی که آدرس بیس باشد
            if (userAdditionalUserProp.AddressBased)
            {
                // بررسی ادرس
                if (string.IsNullOrEmpty(address.FullAddress))
                {
                    return (false, "کاربر گرامی لطفا آدرس را وارد نمایید.", null);
                }

                // بررسی مقدار نام و نام خانوادگی
                if (string.IsNullOrEmpty(recipient.Name) || string.IsNullOrEmpty(recipient.LastName))
                    return (false, "کاربر گرامی لطفا نام و نام خانوادگی را وارد کنید.", null);

                // رمزنگاری اطلاعات آدرس
                address.FullAddress = CryptographyHelper.AesEncrypt(address.FullAddress);
                address.IsEncrypted = true;
            }

            // دریافت آیدی باشگاه مشتریان عمومی
            var publicCustomerClubId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCustomerClubPublicCustomerClubId);

            // اگر در حالت بروزرسانی بودیم ماالکیت recipient و آدرس ان باید بررسی شود

            RecipientSimpleDto recipientInfoInDb = null;
            if (recipient.Id != 0)
            {
                recipientInfoInDb = await GetUserRecipient(recipient.Id, true).Select(x => new RecipientSimpleDto
                {
                    Id = x.Id,
                    AddressId = x.AddressId,
                    Name = x.Name,
                    LastName = x.LastName
                }).FirstOrDefaultAsync();

                // بررسی مالکیت recipient
                // که برای کاربر جاری باشد
                if (recipientInfoInDb == null)
                {
                    return (false, "بروز خطا در دریافت اطلاعات", null);
                }
                // بررسی ولید بودن آدرسی که کاربر میخواهد بروزرسانی کند
                if (userAdditionalUserProp.AddressBased && recipientInfoInDb.AddressId.HasValue && recipientInfoInDb.AddressId.Value != recipient.AddressId)
                {
                    return (false, "بروز خطا در دریافت اطلاعات", null);
                }
            }
            var transaction = await _unitOfWork.BeginTransactionAsync();
            // عملیات درج یا بروزرسانی
            try
            {
                //بررسی نیاز به تغییر عضویت باشگاه مشتری
                //و بروزرسانی آن به نوع حکمتی
                if (userAdditionalUserProp.CustomerClubId == publicCustomerClubId && !string.IsNullOrEmpty(recipient.CardNumber))
                {
                    await _additionalUserPropService.UpdateCurrentUserCustomerClubToHekmatAsync();
                }

                // درج یا بروزرسانی آدرس
                if (address.CityId.HasValue)
                {
                    // رمزنگاری مقدار نام و نام خانوادگی
                    if (!string.IsNullOrEmpty(recipient.Name))
                        recipient.Name = CryptographyHelper.AesEncrypt(recipient.Name);
                    if (!string.IsNullOrEmpty(recipient.LastName))
                        recipient.LastName = CryptographyHelper.AesEncrypt(recipient.LastName);

                    if (recipient.AddressId.HasValue)
                    {
                        await _addressService.UpdateAsync(address, address.Id, true);
                    }
                    else
                    {
                        var insertedAddress = await _addressService.AddAsync(address, true);
                        // مقدار دهی ادرس برای recipient
                        recipient.AddressId = insertedAddress.Id;
                    }
                }
                else
                {
                    if (recipientInfoInDb != null)
                    {
                        recipient.Name = recipientInfoInDb.Name;
                        recipient.LastName = recipientInfoInDb.LastName;
                        recipient.AddressId = recipientInfoInDb.AddressId;
                    }

                }
                // درج یا بروزرسانی recipient
                if (recipient.Id != 0)
                {
                    await UpdateAsync(recipient, recipient.Id, true);
                }
                else
                {
                    recipient.ApplicationId = _applicationConfiguration.ApplicationId;
                    var insertedRecipient = await AddAsync(recipient, true);
                    recipient.Id = insertedRecipient.Id;
                }
                await transaction.CommitAsync();
                return (true, string.Empty, recipient.Id);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InsertException, Events.InsertException.ToString()), nameof(recipient), "public Page AddOrUpdate recipient Error", e);
                return (false, "بروز خطا در انجام عملیات", null);
            }
        }
        }
    }

    public class RecipientSimpleDto
    {
        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }

