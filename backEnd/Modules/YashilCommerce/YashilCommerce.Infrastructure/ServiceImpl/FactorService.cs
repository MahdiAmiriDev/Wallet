using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using Yashil.Common.SharedKernel.Logging;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorService : GenericService<Factor, int>, IFactorService
    {
        private readonly IFactorRepository _factorRepository;
        private readonly IFactorVerificationConfigurationService _factorVerificationConfigurationService;
        private readonly IFactorVerificationConfigurationRoleService _factorVerificationConfigurationRoleService;
        private readonly IFactorVerificationService _factorVerificationService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IFactorItemService _factorItemService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IRepresentationService _representationService;
        private readonly IYashilLogger<Factor> _logger;

        public FactorService(
            IUnitOfWork unitOfWork,
            IFactorRepository factorRepository,
            IUserPrincipal userPrincipal,
            IFactorVerificationConfigurationService factorVerificationConfigurationService,
            IFactorVerificationConfigurationRoleService factorVerificationConfigurationRoleService,
            IFactorVerificationService factorVerificationService,
            IAppDocumentService appDocumentService,
            IFactorItemService factorItemService,
            IEventPublisher eventPublisher,
            ISaleConfigurationService saleConfigurationService,
            StoreConfiguration storeConfiguration,


            IRepresentationService representationService, IYashilLogger<Factor> logger)
            : base(unitOfWork, factorRepository, userPrincipal, eventPublisher)
        {
            _factorRepository = factorRepository;
            _userPrincipal = userPrincipal;
            _factorVerificationConfigurationService = factorVerificationConfigurationService;
            _factorVerificationConfigurationRoleService = factorVerificationConfigurationRoleService;
            _factorVerificationService = factorVerificationService;
            _appDocumentService = appDocumentService;
            _factorItemService = factorItemService;
            _saleConfigurationService = saleConfigurationService;
            _storeConfiguration = storeConfiguration;
            _representationService = representationService;
            _logger = logger;
        }

        public IQueryable<Factor> GetByCustomFilterForDelete(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType)
        {
            return _factorRepository.GetByCustomFilterForDelete(taxCategoryId, companyId, representationId, factorReviewStatusType);

        }

        public IQueryable<Factor> GetByCustomFilter(int? taxCategoryId, int? companyId, int? representationId, int? factorReviewStatusType)
        {
            return _factorRepository.GetByCustomFilter(taxCategoryId, companyId, representationId, factorReviewStatusType);
        }

        public bool CheckExistenceUnconfirmedFactor(int companyId, int? representationId)
        {
            return _factorRepository.CheckExistenceUnconfirmedFactor(companyId, representationId);
        }

        public async Task<bool> CreateFactorAsync(Factor factor, int[] productSalePlanningIds, bool saveAfterAdd = false)
        {
            var prefixFactorNumber = _storeConfiguration.FactorNumberPrefix;
            return await _factorRepository.CreateFactorAsync(factor, productSalePlanningIds, prefixFactorNumber, saveAfterAdd);
        }


        public IQueryable<Factor> GetByCustomFilterForFactorGroupList(int factorGroupId, int? taxCategoryId, int? companyId,
            int? representationId, int? factorReviewStatusType)
        {
            return _factorRepository.GetByCustomFilterForFactorGroupList(factorGroupId, taxCategoryId, companyId, representationId,
                factorReviewStatusType);
        }

        public IQueryable<Factor> GetFactorsAssignedToFactorGroupAsync(int factorGroupId)
        {
            return _factorRepository.GetFactorsAssignedToFactorGroupAsync(factorGroupId);
        }

        public IQueryable<Factor> GetFactorsNotAssignedToFactorGroupAsync(int? categoryGroupId)
        {
            return _factorRepository.GetFactorsNotAssignedToFactorGroupAsync(categoryGroupId);
        }

        public async Task<bool> AssignSelectedItemsToFactorGroup(List<int> selectedFactors, int groupId, bool assign)
        {
            await _factorRepository.AssignSelectedItemsToFactorGroup(selectedFactors, groupId, assign);
            await SaveChangeAsync();
            return true;
        }

        public IQueryable<FactorGroupItemReport> GetFactorGroupItemList(int factorGroupId)
        {
            return _factorRepository.GetFactorGroupItemList(factorGroupId);
        }

        public async Task<string> FinalFactorRegistration(int factorId,
            bool saveAfterAddOrUpdate = false)
        {
            // بررسی وجود فاکتور
            var factor = Get(factorId);
            if (factor != null)
            {
                //بررسی وضعیت فاکتور
                if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin)
                {
                    return @"کاربر گرامی این فاکتور در وضعیت منتظر تایید توسط مدیر سایت می باشد.";
                }
                if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Processing)
                {
                    return @"کاربر گرامی این فاکتور در حال پردازش میباشد.";
                }
                if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed)
                {
                    return @"کاربر گرامی این فاکتور تایید شده میباشد.";
                }
                // بررسی آپلود شدن فایل فاکتور مهر شده
                if (!_appDocumentService.ObjectHasDocument(AppEntityEnum.Factor.ToString(), factor.Id, (int)DocumentCategoryEnum.FactorDocument, (int)DocumentTypeEnum.FactorDocument))
                {
                    return @"کاربر گرامی لطفا گزارش فاکتور مهر شده را آپلود نمایید.";
                }
                //بررسی وجود تنظیم بررسی فاکتور فعال
                if (!_factorVerificationConfigurationService.CheckExistenceActiveFactorVerification())
                {
                    return @"کاربر گرامی تنظیم بررسی تایید فاکتوری وجود ندارد لطفا با ادمین تماس بگیرید.";
                }


                int maxFactorVerificationTryNumber = await _factorVerificationService.GetMaxFactorVerificationTryNumberAsync(factor.Id);

                int currentTryNumber = 1;
                //
                if (maxFactorVerificationTryNumber > 0)
                {
                    //در این حالت این فاکتور قبلا بررسی شده است و رد شده است و باید رکورد های بررسی قبلی این فاکتور آرشیو شود
                    await _factorVerificationService.ArchiveOldFactorVerificationsAsync(factor.Id, saveAfterAddOrUpdate);
                    currentTryNumber = maxFactorVerificationTryNumber + 1;
                }

                // سپس مجددا بررسی های فاکتور برای این فاکتور ایجاد میشود
                // دریافت اطلاعات تنظیم بررسی فاکتور و نقش های آن
                var factorVerificationConfiguration = _factorVerificationConfigurationService.GetActiveFactorVerificationConfiguration();
                var factorVerificationConfigurationRoles =await _factorVerificationConfigurationRoleService.GetFactorVerificationConfigurationRoles(factorVerificationConfiguration.Id).ToListAsync();
                // شماره آخرین باری که فاکتور جهت بررسی توسط تامین کننده ارسال شده است
                // اگر برای اولین بار باشد مقدار صفر برمیگرداند


                // ثبت در بررسی فاکتور بازای هر نقش تنظیم فاکتور
                foreach (var factorVerificationConfigurationRole in factorVerificationConfigurationRoles)
                {
                    await _factorVerificationService.AddAsync(new FactorVerification()
                    {
                        FactorId = factor.Id,
                        FactorConfirmationType = (int)FactorConfirmationTypeEnum.Initial,
                        FactorVerificationConfigurationRoleId = factorVerificationConfigurationRole.Id,
                        IsHistory = false,
                        // هنگام ثبت بررسی های فاکتور به گردش قبلی یکی اضافه میکنیم
                        TryNumber = currentTryNumber

                    });
                }
                // تغییر وضعیت فاکتور به در حال بررسی
                factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.Processing;
                // عملیات بروزرسانی
                var result = await UpdateAsync(factor, factor.Id, saveAfterAddOrUpdate);
                if (result != null)
                {
                    return null;
                }
                return @"خطا در ثبت نهایی فاکتور جهت بررسی";


            }

            return @"کاربر گرامی چنین فاکتوری وجود ندارد";

        }

        public async Task<FactorVerification> ConfirmFactorVerification(int factorVerificationId
            , bool saveAfterAddOrUpdate = false)
        {
            var factorVerification = await _factorVerificationService.GetAsync(factorVerificationId, false);
            if (factorVerification != null)
            {
                // بررسی میشود که بررسی فاکتور اجازه بروزرسانی دارد یا خیر
                var allowUpdateFactorVerification = _factorVerificationService.AllowUpdateFactorConfirmationType(
                    factorVerification);
                if (allowUpdateFactorVerification)
                {
                    // در صورت اجازه بروزرسانی داشتن تایید میشود
                    factorVerification.FactorConfirmationType = (int)FactorConfirmationTypeEnum.Confirmed;
                    factorVerification.ValidationDate = DateTime.Now;
                    factorVerification.VerifyBy = _userPrincipal.Id;
                    var factor = Get(factorVerification.FactorId);
                    if (factor != null)
                    {
                        // اگر آخرین بررسی فاکتور باشد باید فاکتور نیز به حالت تایید شده در بیاید
                        if (!_factorVerificationService.CheckExistenceSubsequentRecord(
                            factorVerification.FactorVerificationConfigurationRole.VerificationOrder,
                            factorVerification.FactorId))
                        {
                            factor.ConfirmBy = _userPrincipal.Id;
                            factor.ConfirmDate = DateTime.Now;
                            factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.Confirmed;
                        }
                        // در غیر این صورت فاکتور به حالت در حال بررسی در می آید
                        else
                        {
                            factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.Processing;
                        }

                        await UpdateAsync(factor, factor.Id);
                        return await _factorVerificationService.UpdateAsync(factorVerification, factorVerification.Id, saveAfterAddOrUpdate);
                    }
                    return null;
                }
            }
            return null;

        }



        public async Task<string> RejectFactorVerification(FactorVerification factorVerification,
            bool saveAfterAddOrUpdate = false)
        {
            if (factorVerification != null)
            {
                if (string.IsNullOrEmpty(factorVerification.Description))
                {
                    return "کاربر گرامی لطفا برای رد کردن فاکتور توضیحات را وارد کنید";
                }
                // بررسی میشود که بررسی فاکتور اجازه بروزرسانی دارد یا خیر
                var allowUpdateFactorVerification = _factorVerificationService.AllowUpdateFactorConfirmationType(factorVerification);
                if (allowUpdateFactorVerification)
                {
                    // در صورت اجازه بروزرسانی داشتن رد میشود
                    factorVerification.FactorConfirmationType = (int)FactorConfirmationTypeEnum.Rejected;
                    factorVerification.ValidationDate = DateTime.Now;
                    factorVerification.VerifyBy = _userPrincipal.Id;
                    var factor = Get(factorVerification.FactorId);
                    // فاکتور نیز به حالت رد شده تغییر وضعیت میدهد
                    factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.Rejected;
                    await UpdateAsync(factor, factor.Id);
                    var result = await _factorVerificationService.UpdateAsync(factorVerification, factorVerification.Id, saveAfterAddOrUpdate);
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "خطا در رد تراکنش";
                    }
                }

                return "کاربر گرامی شما اجازه رد فاکتور را ندارید";
            }
            return "خطا در تایید تراکنش";
        }

        public IQueryable<Factor> GetSupervisorFactorList(int? companyId, int? representationId, int? taxCategoryId, int? factorReviewStatusType)
        {
            return _factorRepository.GetSupervisorFactorList(companyId, representationId, taxCategoryId, factorReviewStatusType);
        }

        public Factor GetFactorIfAccess(int factorId, bool readOnly)
        {
            return _factorRepository.GetUserFactor(factorId, readOnly);
        }

        public IQueryable<Factor> GetGroupedFactors(int? companyId, int? representationId)
        {
            return _factorRepository.GetGroupedFactors(companyId, representationId);
        }

        public bool CheckExistenceFactorForSaleConfiguration(int saleConfigurationId)
        {
            return _factorRepository.CheckExistenceFactorForSaleConfiguration(saleConfigurationId);
        }

        public string ValidateFactorYashilScheduler()
        {
            /*
             * دریافت قرارداد فعال در صورت وجود داشتن
             * و درصورت وجود نداشتن دریافت آخرین قرارداد موجود بر اساس تاریخ
             */
            //var contract = _contractService.GetActiveContractUser(_userPrincipal.Id) ?? _contractService.GetLastUserContractByDate(_userPrincipal.Id);
            // بررسی وجود داشتن قرارداد

            var saleConfiguration = _saleConfigurationService.GetUserActiveSaleConfiguration(_userPrincipal.Id) ?? _saleConfigurationService.GetLastSaleConfigurationByDate(_userPrincipal.Id);
            if (saleConfiguration == null)
            {
                return "کاربر گرامی قرارداد یا تنظیم فروش برای شما وجود ندارد";
            }
            // بررسی وجود داشتن زمانبندی صدور فاکتور برای قرارداد موجود
            var factorYashilScheduler = saleConfiguration.YashilScheduler;
            if (factorYashilScheduler == null)
            {
                return "کاربر گرامی زمانبندی صدور فاکتوری برای شما تنظیم نشده است لطفا با مدیر سایت تماس بگیرید";
            }
            /*
             * ابتدا چک میشود که ایا اکنون(یعنی تاریخ فعلی) در بازه شروع و پایان زمانبندی فاکتور هست یا خیر
             * در صورتی وجود نداشته باشد به کاربر پبغام مناسب نمایش داده میشود
             */
            var currentDate = DateTime.Now;
            // تبدیل تاریخ شروع زمانبندی صدور فاکتور به میلادی و بررسی آن با تاریخ فعلی
            var starDate = DateFunction.ShamsiToMiladi(DateFunction.GetDateFromIntReverse(factorYashilScheduler.StartDate));
            if (starDate > currentDate)
            {
                return String.Format("کاربر گرامی شما از تاریخ {0} به بعد قادر به ثبت فاکتور خواهید بود", DateFunction.GetDateFromIntReverse(factorYashilScheduler.StartDate));
            }
            // بررسی وجود داشتن تاریخ پایان
            if (factorYashilScheduler.EndDate != null)
            {
                // در صورتی که تاریخ پایان وجود داشته باشد تبدیل آن به میلادی و مقایسه ان با تاریخ فعلی
                var endDate = DateFunction.ShamsiToMiladi(DateFunction.GetDateFromIntReverse(factorYashilScheduler.EndDate));
                if (endDate < currentDate)
                {
                    return String.Format("کاربر گرامی در تاریخ {0} زمانبندی صدور فاکتور شما به پایان رسیده است لطفا با مدیر سیستم تماس بگیرید", DateFunction.GetDateFromIntReverse(factorYashilScheduler.EndDate));
                }
            }

            //آیا در بازه زمانی مجاز در ماه جاری هستیم؟
            var isAllowedEra = IsUserGeneratingInvoiceInAllowedEra(factorYashilScheduler.FromEra, factorYashilScheduler.ToEra);

            if (!isAllowedEra)
                return $" کاربر گرامی بازه زمانی مجاز برای صدور فاکتور از روز {factorYashilScheduler.FromEra} ام تا روز {factorYashilScheduler.ToEra} ام ماه جاری می باشد.";


            //تاریخ  روز جاری ماه و سال بدون روز
            var todayShamsiStringWithoutDay = DateFunction.TodayShamsiInt().ToString().Substring(0, 6);

            string fromEra = string.Empty;

            if (factorYashilScheduler.FromEra < 10)
                fromEra = "0" + factorYashilScheduler.FromEra;

            fromEra = !string.IsNullOrEmpty(fromEra) ? fromEra : factorYashilScheduler.FromEra.ToString();

            //تاریخ شروع بازه در ماه جاری به صورت شمسی و رشته
            var startEraDateInCurrentMonthString = int.Parse(todayShamsiStringWithoutDay + fromEra);

            string toEra = string.Empty;

            if (factorYashilScheduler.ToEra < 10)
                toEra = "0" + factorYashilScheduler.FromEra;

            toEra = !string.IsNullOrEmpty(toEra) ? toEra : factorYashilScheduler.ToEra.ToString();

            //تاریخ پایان بازه در ماه جاری به صورت شمسی و رشته
            var endEraDateInCurrentMonthString = int.Parse(todayShamsiStringWithoutDay + toEra);

            //تعداد فاکتور های صادر شده در بازه
            var userFactorRequestCountInEra = GetUserFactorRequestCountInEra(DateFunction.ShamsiIntToMiladi(startEraDateInCurrentMonthString),
                DateFunction.ShamsiIntToMiladi(endEraDateInCurrentMonthString));

            if (userFactorRequestCountInEra >= factorYashilScheduler.RepeatNumber)
                return String.Format("کاربر گرامی شما در این بازه از حداکثر تعداد درخواست های خود استفاده کرده اید. لطفا از روز {0} ام تا روز {1} ام ماه بعد برای ثبت درخواست جدید مراجعه کنید.", factorYashilScheduler.FromEra, factorYashilScheduler.ToEra);


            return null;
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر در بازه زمانی مجاز در ماه جاری اقدام به صدور فاکتور کرده است
        /// </summary>
        /// <param name="fromEra">شروع باز</param>
        /// <param name="toEra">پایان بازه</param>
        /// <returns></returns>
        private bool IsUserGeneratingInvoiceInAllowedEra(int fromEra, int toEra)
        {

            var todayShamsiString = DateFunction.TodayShamsiInt().ToString();

            var dayOfMonthString = todayShamsiString.Substring(todayShamsiString.Length - 2);

            if (dayOfMonthString[0] == '0')
                dayOfMonthString = dayOfMonthString.Substring(1);

            var dayOfMonthInt = int.Parse(dayOfMonthString);

            if (!(fromEra <= dayOfMonthInt && toEra >= dayOfMonthInt))
                return false;

            return true;
        }

        /// <summary>
        /// بازنویسی دیلیت فاکتور
        /// ابتدا آیتمهای فاکتور را پاک میکند
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveAfterDelete"></param>
        /// <param name="publishEvent"></param>
        public override async Task DeleteAsync(Factor entity, bool saveAfterDelete = false, bool publishEvent = true)
        {
            var factorItems = _factorItemService.GetFactorItems(entity.Id);
            foreach (var factorItem in factorItems)
            {
                await _factorItemService.DeleteAsync(factorItem);
            }
            var factorVerifications = _factorVerificationService.GetFactorVerificationsByFactorId(entity.Id);
            foreach (var factorVerification in factorVerifications)
            {
                await _factorVerificationService.DeleteAsync(factorVerification);
            }
            await base.DeleteAsync(entity, saveAfterDelete);
        }
        /// <summary>
        /// بازنویسی دیلیت فاکتور
        /// ابتدا آیتمهای فاکتور را پاک میکند
        /// </summary>
        public override async Task DeleteAsync(object id, bool saveAfterDelete = false, bool publishEvent = true, bool logical = false)
        {
            var factorItems = _factorItemService.GetFactorItems((int)id);
            foreach (var factorItem in factorItems)
            {
                await _factorItemService.DeleteAsync(factorItem, false, false);
            }
            var factorVerifications = _factorVerificationService.GetFactorVerificationsByFactorId((int)id);
            foreach (var factorVerification in factorVerifications)
            {
                await _factorVerificationService.DeleteAsync(factorVerification);
            }
            await base.DeleteAsync(id, saveAfterDelete);
        }

        public int? GetUserFactorRequestCountInEra(DateTime startDate, DateTime endDate)
        {
            return _factorRepository.GetUserFactorRequestCountInEra(startDate, endDate);
        }

        public Task<bool> CheckExistAnyFactorByPersonBankAccountIdAsync(int personBankaccountId)
        {
            return _factorRepository.CheckExistAnyFactorByPersonBankAccountIdAsync(personBankaccountId);
        }

        /// <summary>
        /// مقایسه پرداخت های حکمت مربوط به کد فاکتور با دیتای  تراکنش های تایید شده حکمت که از طریق سی دی از شرکت اتکا گرفته میشود
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="showContrast"></param>
        /// <returns></returns>
        public IQueryable<PaymentComparisonWithHekmatApprovedTransactionDomainModel> GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(int factorId, bool showContrast)
        {
            return _factorRepository.GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(factorId, showContrast);
        }

        public IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView> GetFactorItemPaymentFullInfoViewList(int factorGroupId)
        {
            return _factorRepository.GetFactorItemPaymentFullInfoViewList(factorGroupId);
        }

        public FactorStatisticDomainModel GetFactorStatistic(int factorId)
        {
            return _factorRepository.GetFactorStatistic(factorId);
        }

        public bool CheckExistenceFactorForCompanyAndRepresentation(int companyId, int? representationId)
        {
            return _factorRepository.CheckExistenceFactorForCompanyAndRepresentation(companyId, representationId);
        }

        public async Task<bool> CheckUserAccessForFactorId(int factorId)
        {
            if (factorId == 0)
            {
                return true;
            }
            var factor = await _factorRepository.GetAsync(factorId);
            if (factor != null)
            {
                return await _representationService.CheckUserAccessForCompanyAndRepresentationAsync(
                    factor.CompanyId, factor.RepresentationId);
            }
            return false;

        }

        public async Task<(string message, bool isSuccess)> ValidateAssignSelectedItemsToFactorGroup(
            List<int> factorIds, int factorGroupId)
        {
            //بررسی می کند که آیا گروه دسته بندی متفاوت در فاکتور ها انتخابی کاربر وجود دارد یا خیر


            //بنا به درخواست مالی این محدودیت برداشته شد و کاربر می تواند از گروه
            //دسته بندی های مختلفی فاکتور های اختصاص نشده را انتخاب کند

            //var isExistDifferentCategoryGroup =
            //    await _factorRepository.IsExistDifferentCategoryGroupInSelectedFactorsAsync(factorIds);
            //if (isExistDifferentCategoryGroup)
            //{
            //    return ("کاربر گرامی گروه دسته بندی فاکتور های انتخابی یکسان نمی باشد.", false);
            //}


            //  شناسه گروه دسته بندی فاکتور های اساین شده را دریافت می کنیم 
            // نکته : فاکتور های اساین شده فقط یک شناسه گروه دسته بندی دارند  
            var assignedFactorCategoryGroupId = await
                GetFactorsAssignedToFactorGroupAsync(factorGroupId)
                    .Select(x => x.CategoryGroupId)
                    .FirstOrDefaultAsync();
            // هیچ فاکتوری اساین نشده است
            if (assignedFactorCategoryGroupId == 0)
            {
                return (string.Empty, true);
            }

            //  شناسه گروه دسته بندی فاکتور های انتخاب شده توسط کاربر را دریافت می کنیم 
            // نکته : فاکتور های انتخاب شده توسط کاربر قبلا بررسی شد
            // که فقط یک شناسه گروه دسته بندی داشته باشند 
            // پس در این خط کد ما فقط یک شناسه گروه دسته بندی داریم 

            // بنا بر درخواست مالی این محدودیت برداشته شد

            //var selectedFactorCategoryGroupId = await
            //    FindById(factorIds[0], true)
            //        .Select(x => x.CategoryGroupId)
            //        .FirstOrDefaultAsync();

            ////  بررسی می کند که آیا گروه دسته بندی محصول فاکتور های انتخابی
            //// با گروه دسته بندی محصول فاکتور های اساین شده   
            //// متفاوت می باشد یا خیر 
            //if (selectedFactorCategoryGroupId != assignedFactorCategoryGroupId)
            //{
            //    return (
            //        "کاربر گرامی گروه دسته بندی فاکتور های انتخابی با گروه دسته بندی فاکتور هایی که در کادر سمت چت انتخاب شده است، یکسان نمی باشد.",
            //        false);
            //}

            return (string.Empty, true);
        }

        /// <summary>
        /// دریافت فاکتور بر اساس شناسه فاکتور
        /// </summary>
        /// <param name="id">شناسه فاکتور</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        public IQueryable<Factor> FindById(int id, bool readOnly = false)
        {
            return _factorRepository.FindById(id, readOnly);
        }

        public IQueryable<Factor> GetByFactorGroupId(int factorGroupId)
        {
            return _factorRepository.GetByFactorGroupId(factorGroupId);
        }

        public async Task<(bool IsSucces, string Message)> ChangeFactorStatusToProcessingStatusAsync(int id)
        {
            var checkedFactor = await CheckValidateFactorForChangeToInitialStatusAsync(id);
            if (!checkedFactor.IsSucces)
            {
                return (false, checkedFactor.Message);
            }

            var updatedFactorVerification = await _factorVerificationService.ChangeFactorLastVerifierConfirmTypeToInitialAsync(id);
            if (updatedFactorVerification != null)
            {
                var factor = await GetAsync(id, false);
                factor.ConfirmBy = null;
                factor.ConfirmDate = null;
                factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.Processing;
                await UpdateAsync(factor, factor.Id);
                await SaveChangeAsync();
                return (true, string.Empty);
            }
            _logger.InsertLog(LogLevel.Error,
                (int)Events.UpdateException,
                nameof(Factor),
                "ChangeFactorLastVerifierConfirmTypeToInitialAsync هیچ رکوردی را تغییر نداده است"
                , null
                );
            return (false, string.Empty);
        }

        public IQueryable<Factor> GetConvertedFactor(int? factorGroupId)
        {
            var query = GetAll(true)
                .Where(x => x.FactorGroupId.HasValue);

            if (factorGroupId is > 0)
                query = query.Where(x => x.FactorGroupId.Value == factorGroupId);

            return query;
        }

        /// <summary>
        ///  بررسی می کند آیا فاکتور برای تغییر وضعیت به وضعیت در حال بررسی معتیر می باشد یا خیر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<(bool IsSucces, string Message)> CheckValidateFactorForChangeToInitialStatusAsync(int id)
        {
            var factorInfo = await
                FindById(id)
                    .Select(f => new
                    {
                        IsGrouped = f.FactorGroupId.HasValue,
                        IsConfirm = f.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed
                    })
                    .FirstOrDefaultAsync();
            if (factorInfo == null)
            {
                return (false, "کاربر گرامی فاکتوری با شناسه ارسالی یافت نشد.");
            }
            // فاکتور باید گروه بندی نشده باشد تا بتوان آن را به وضعیت در حال بررسی تغییر داد
            // پس اگر گروه بندی شده باشد پیام زیر به کاربر نمایش داده می شود
            if (factorInfo.IsGrouped)
            {
                return (false, "کاربر گرامی فاکتور انتخابی گروه بندی شده است و امکان تغییر به وضعیت در حال بررسی وجود ندارد.");
            }
            // فاکتور باید در وضعیت تایید باشد تا بتوان آن را به وضعیت در حال بررسی تغییر داد
            // پس اگر در وضعیت تایید باشد پیام زیر به کاربر نمایش داده می شود
            if (!factorInfo.IsConfirm)
            {
                return (false, "کاربر گرامی فاکتور انتخابی در وضعیت تایید نهایی نمی باشد و امکان تغییر به وضعیت در حال بررسی وجود ندارد..");
            }
            return (true, string.Empty);
        }

        public Task<int> UpdateFactorInstallmentCountFromSaleConfiguration(int factorId)
        {
            return GetAll(true)
                .Where(x => x.Id == factorId &&
                            x.FactorInstallmentCount != x.SaleConfiguration.SaleConfigurationAttribute
                            .Where(s => s.SaleConfigurationId == x.SaleConfigurationId &&
                            s.CustomerClubId == x.CustomerClubId)
                            .Select(s => s.FactorInstallmentCount)
                            .FirstOrDefault())

                .Select(x => new
                {
                    Factor = x,
                    SaleConfigurationInstallmentCount = x.SaleConfiguration.SaleConfigurationAttribute
                    .Where(s => s.SaleConfigurationId == x.SaleConfigurationId &&
                            s.CustomerClubId == x.CustomerClubId)
                            .Select(s => s.FactorInstallmentCount)
                            .FirstOrDefault()
                })
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(u => u.Factor.FactorInstallmentCount, b => b.SaleConfigurationInstallmentCount));
        }
    }

}
