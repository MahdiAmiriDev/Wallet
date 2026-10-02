using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.ControllersExtenders;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.UserManagement;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ControllersExtenders
{
    public class UserControllerExtender : IUserControllerExtender
    {
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ICustomerClubService _customerClubService;
        private readonly IAppConfigService _appConfigService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IYashilLogger<AdditionalUserProp> _logger;


        public UserControllerExtender(
            IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration,
            ICustomerClubService customerClubService, StoreConfiguration storeConfiguration, IAppConfigService appConfigService, IAdditionalUserPropService additionalUserPropService, IUserService userService, IConfiguration configuration, IYashilLogger<AdditionalUserProp> logger)
        {
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _customerClubService = customerClubService;
            _storeConfiguration = storeConfiguration;
            _appConfigService = appConfigService;
            _additionalUserPropService = additionalUserPropService;
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// عملیاتی که قبل از آپدیت کردن یوزر اتفاق می افتد
        /// </summary>
        /// <param name="editModelKeyValues"></param>
        /// <param name="insertedUserId"></param>
        public async Task BeforeUpdateAsync(Dictionary<string, string> editModelKeyValues, int insertedUserId)
        {
            var additionalUserProp = _additionalUserPropService.GetByUserId(insertedUserId) ??
                                     new AdditionalUserProp { UserId = insertedUserId };

            await SetAdditionalPropValueAsync(additionalUserProp, editModelKeyValues);
            if (additionalUserProp.Id != 0)
            {
                await _additionalUserPropService.UpdateAsync(additionalUserProp, additionalUserProp.Id);
            }
            else
            {
                await _additionalUserPropService.AddAsync(additionalUserProp);
            }
        }

        /// <summary>
        /// ست کردن مقادیر AdditionalUserProp
        /// اطلاعاتی همچون companyId و representationId
        /// </summary>
        /// <param name="additionalUserProp"></param>
        /// <param name="editModelKeyValues"></param>
        private async Task SetAdditionalPropValueAsync(AdditionalUserProp additionalUserProp,
            Dictionary<string, string> editModelKeyValues)
        {
            if (editModelKeyValues == null)
            {
                return;
            }

            // دریافت اطلاعات ارسالی
            editModelKeyValues.TryGetValue("companyId", out var companyIdStr);
            editModelKeyValues.TryGetValue("representationId", out var representationIdStr);
            editModelKeyValues.TryGetValue("representationCategoryId", out var representationCategoryIdStr);
            editModelKeyValues.TryGetValue("cardNumber", out var cardNumber);



            if (_configuration.GetSection("ApplicationsId:Lms").Get<int>() == _applicationConfiguration.ApplicationId)
            {
                editModelKeyValues.TryGetValue("maximumUploadCapacity", out string maximumUploadCapacity);

                var defaultMaximumUserUploadCapacity = await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilUserManagementUserDefaultMaximumAllowedCapacityMegaByte);

                if (string.IsNullOrEmpty(maximumUploadCapacity) || maximumUploadCapacity == "null")
                {

                    if (defaultMaximumUserUploadCapacity == 0)
                    {
                        var exception = new Exception("Invalid config value key: YashilUserManagementUserDefaultMaximumAllowedCapacityMegaByte");

                        _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(AdditionalUserProp),
                            "مقدار پیش فرض مربوط به حداکثر ظرفیت آپلود کاربر در تنظیمات سامانه ست نشده است", exception);

                        throw exception;
                    }

                    additionalUserProp.MaximumUploadCapacity = defaultMaximumUserUploadCapacity;
                }
                else
                {
                    if (!int.TryParse(maximumUploadCapacity, out int insertedCapacity))
                    {
                        var exception = new ArgumentException("Invalid inserted maximumUploadCapacity value", nameof(maximumUploadCapacity));

                        _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(AdditionalUserProp),
                            "مقدار وارد شده برای حداکثر ظرفیت آپلود کاربر نامعتبر بوده و قابل تبدیل به int نمی باشد", exception);

                        throw exception;
                    }

                    if (insertedCapacity > defaultMaximumUserUploadCapacity)
                    {
                        var exception = new ArgumentException(
                            "Invalid maximumUploadCapacity value out of default maximum value range",
                            nameof(maximumUploadCapacity));

                        _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(AdditionalUserProp),
                            "مقدار حداکثر ظرفیت شده وارد شده از مقدار حداکثر ظرفیت تنظیم شده برای هر کاربر بیشتر می باشد.", exception);

                        throw exception;
                    }

                    additionalUserProp.MaximumUploadCapacity = insertedCapacity;
                }
            }



            /*
             * اگر اطلاعات مرکز ارسالی مقدار نداشته باشد
             * دو حالت پیش می آید
             * یا یک کاربر لاگین شده در حال ساخت کاربر جدید است یا کاربر خود در حال ثبت نام است
             * در حالتی که کاربر لاگین شده در حال ثبت نام کاربر جدید است
             * باید مرکز و نمایندگی کاربر لاگین شده را چک کنیم
             * اگر کاربر لاگین شده دارای مرکز و نمایندگی بود باید یوزر در حال ساخت نیز همان مرکز و نمایندگی را بگیرد و
             * اگر کاربر لاگین شده دارای مرکز و نمایندگی نبود
             * آنگاه میتواند یوزری که در حال ساخت آن است نیز بدون مرکز یا نمایندگی باشد
             *
             */
            // دریافت مرکز و نمایندگی کاربر لاگین شده


            // کاربر لاگین شده در حالت ساخت یوزر جدید است
            if (_userPrincipal != null)
            {

                var currentAdditionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
                if (currentAdditionalUserProp != null)
                {
                    // اگر کاربر لاگین شده مرکز داشته باشد یوزر در حالت ساخت تنها میتواند مرکز یا نمایندگی کاربر لاگین شده را داشته باشد
                    // و در صورتی که کاربر لاگین شده مرکز و نمایندگی نداشت آنگا یوزر در حال ساخت نیز میتواند مرکز و نمایندگی نداشته باشد
                    if (currentAdditionalUserProp.CompanyId.HasValue)
                    {
                        additionalUserProp.CompanyId = currentAdditionalUserProp.CompanyId;
                    }
                    else //کاربر لاگین شده می خواهد یک کاربر جدید بسازد و کمپانی ندارد
                    {

                        if (!string.IsNullOrEmpty(companyIdStr) &&
                            companyIdStr != "0" &&
                            companyIdStr != "null" &&
                            int.TryParse(companyIdStr, out var companyId))
                        {
                            additionalUserProp.CompanyId = companyId;

                        }
                        else
                        {
                            // اگر یوزر لاگین شده مرکز نداشته باشد باید نمایندگی برای یوزر در حال ساخت هم خالی شود
                            additionalUserProp.CompanyId = null;
                            additionalUserProp.RepresentationId = null;
                        }
                    }

                    if (currentAdditionalUserProp.RepresentationId.HasValue)
                    {

                        additionalUserProp.RepresentationId = currentAdditionalUserProp.RepresentationId;
                    }
                    else //کاربر لاگین شده می خواهد یک کاربر جدید بسازد و نمایندگی ندارد
                    {

                        // اگر به این شروط برسد باید حتما مرکز مقدار داشته باشد

                        if (
                            additionalUserProp.CompanyId.HasValue &&
                            !string.IsNullOrEmpty(representationIdStr) &&
                            representationIdStr != "0" &&
                            representationIdStr != "null" &&
                            int.TryParse(representationIdStr, out var representationId))

                            additionalUserProp.RepresentationId = representationId;

                        else
                            additionalUserProp.RepresentationId = null;
                    }
                    if (currentAdditionalUserProp.RepresentationCategoryId.HasValue)
                    {

                        additionalUserProp.RepresentationCategoryId = currentAdditionalUserProp.RepresentationCategoryId;
                    }
                    else //کاربر لاگین شده می خواهد یک کاربر جدید بسازد و دسته بندی نمایندگی ندارد
                    {

                        // اگر به این شروط برسد باید حتما مرکز و نمایندگی مقدار داشته باشد

                        if (
                            additionalUserProp.CompanyId.HasValue &&
                            additionalUserProp.RepresentationId.HasValue &&
                            !string.IsNullOrEmpty(representationCategoryIdStr) &&
                            representationCategoryIdStr != "0" &&
                            representationCategoryIdStr != "null" &&
                            int.TryParse(representationCategoryIdStr, out var representationCategoryIdId))

                            additionalUserProp.RepresentationCategoryId = representationCategoryIdId;

                        else
                            additionalUserProp.RepresentationCategoryId = null;
                    }

                }
            }
            else
            {
                // اگر یوزر لاگین شده مرکز نداشته باشد باید نمایندگی برای یوزر در حال ساخت هم خالی شود
                additionalUserProp.CompanyId = null;
                additionalUserProp.RepresentationId = null;
                additionalUserProp.RepresentationCategoryId = null;
            }


        }
        /// <summary>
        /// دریافت اطلاعات اضافی کاربر از additionalUserProp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetCustomProps(int userId)
        {
            var additionalInfos = new Dictionary<string, string>();
            var additionalUserProp = _additionalUserPropService.GetByUserId(userId);
            if (additionalUserProp != null)
            {
                if (additionalUserProp.CompanyId.HasValue)
                {
                    additionalInfos.Add("companyId", additionalUserProp.CompanyId.ToString());
                }

                if (additionalUserProp.RepresentationId.HasValue)
                {
                    additionalInfos.Add("representationId", additionalUserProp.RepresentationId.ToString());
                }
                if (additionalUserProp.RepresentationCategoryId.HasValue)
                {
                    additionalInfos.Add("representationCategoryId", additionalUserProp.RepresentationCategoryId.ToString());
                }

                if (additionalUserProp.MaximumUploadCapacity.HasValue)
                    additionalInfos.Add("maximumUploadCapacity", additionalUserProp.MaximumUploadCapacity.ToString());
            }
            return additionalInfos;
        }

        /// <summary>
        /// عملیاتی که قبل از insert کردن یوزر اتفاق می افتد
        /// </summary>
        /// <param name="user"></param>
        /// <param name="additionalInfoList"></param>
        /// <param name="userId"></param>
        public async Task BeforeInsert(User user, Dictionary<string, string> additionalInfoList, int userId)
        {
            var additionalUserProp = new AdditionalUserProp { User = user };
            if (_userPrincipal != null && _userPrincipal.Id != 0)
            {
                additionalUserProp.CreateBy = _userPrincipal.Id;
                additionalUserProp.CreatorOrganizationId = _userPrincipal.OrganizationId;
                additionalUserProp.ApplicationId = _userPrincipal.ApplicationId;
            }
            else
            {
                additionalUserProp.CreateBy = _applicationConfiguration.SysUserId;
                additionalUserProp.CreatorOrganizationId = _applicationConfiguration.PublicUsersOrganizationId;
                additionalUserProp.ApplicationId = _applicationConfiguration.ApplicationId;
            }
            CustomerClubVerifyParameter customerClubVerifyParameter = GetCustomerClubVerifyParameter(additionalInfoList);
            if (_storeConfiguration.LimitedToCustomerClub)
            {
                if (customerClubVerifyParameter.CustomerClubId != null)
                {
                    var customerClub = _customerClubService.Get(customerClubVerifyParameter.CustomerClubId.Value, true);
                    // اگر باشگاه دارای کد باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    if (customerClub.HasCustomCode)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.CardNumber;
                    }
                    // در غیر اینصورت اگر باشگاه دارای کد نباشد
                    //  ولی باشگاه دارای کد ملی باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    else if (customerClub.HasNationalCode)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.NationalCode;
                    }
                    // در غیر اینصورت اگر باشگاه دارای کد و کد ملی نباشد
                    //    ولی باشگاه دارای شمارخ موبایل باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    else if (customerClub.HasMobileNumber)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.MobileNumber;
                    }
                }

                additionalUserProp.CustomerClubId = customerClubVerifyParameter.CustomerClubId;


                if (!string.IsNullOrEmpty(customerClubVerifyParameter.NationalCode))
                {
                    user.PersonNavigation.NationalCode = customerClubVerifyParameter.NationalCode;
                }
            }
            else
            {
                if (customerClubVerifyParameter.HasCardNumber)
                {
                    var hekmatCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubsHekmatCustomerClubId);
                    additionalUserProp.CustomerClubId = hekmatCustomerClubId;
                    additionalUserProp.CardNumber = customerClubVerifyParameter.CardNumber;
                }
                else
                {
                    var publicCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubPublicCustomerClubId);
                    additionalUserProp.CustomerClubId = publicCustomerClubId;
                }
            }
            await SetAdditionalPropValueAsync(additionalUserProp, additionalInfoList);
            await _additionalUserPropService.AddAsync(additionalUserProp);
        }

        /// <summary>
        /// اعتبار سنجی داده های ورودی قبل از اینزرت کردن یوزر
        /// </summary>
        /// <param name="additionalInfoList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CustomValidateBeforeInsertAsync(Dictionary<string, string> additionalInfoList)
        {
            return await CustomValidateAsync(additionalInfoList);
        }
        /// <summary>
        /// اعتبار سنجی داده های ورودی قبل از بروزرسانی کاربر
        /// </summary>
        /// <param name="additionalInfoList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CustomValidateBeforeUpdate(Dictionary<string, string> additionalInfoList, int id)
        {
            return true;
        }


        /// <summary>
        /// عملیاتی که قبل از لاگین کردن اتفاق می افتد
        /// </summary>
        /// <param name="additionalInfoList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> BeforeLogin(Dictionary<string, string> additionalInfoList, int userId)
        {
            // دریافت اطلاعات اضافی کاربر
            var additionalUserProp = _additionalUserPropService.GetByUserId(userId);
            // اگر کاربر دارای اطلاعات اضافی باشد و باشگاه مشتریان نداشته باشد
            // به این معناست که کاربر در پروژه های بدون باشگاه مشتریان ثبت نام کرده است
            // لذا برای استفاده از این پروژه که محدود به باشگاه مشتریان است
            // باید عضو باشگاه مشتریان شود.


            if (additionalUserProp != null && additionalUserProp.CustomerClubId == null)
            {
                // بررسی اطلاعات باشگاه مشتریان
                var validateMessage = await CustomValidateAsync(additionalInfoList);
                // اگر اطلاعات ورودی باشگاه مشتریان صحیح باشد
                // ست کردن اطلاعات باشگاه مشتریان
                if (!string.IsNullOrWhiteSpace(validateMessage))
                {
                    // اطلاعات وارد شده باشگاه مشتریان کاربر دارای اعتبار نمیباشد
                    return validateMessage;
                }

                // دریافت اطلاعات باگشاه مشتریان وارد شده توسط کاربر و ست کردن آن در اطلاعات اضافی کاربر
                CustomerClubVerifyParameter customerClubVerifyParameter = GetCustomerClubVerifyParameter(additionalInfoList);
                if (_storeConfiguration.LimitedToCustomerClub)
                {

                    var customerClub =
                        _customerClubService.Get(customerClubVerifyParameter.CustomerClubId.Value, true);
                    // اگر باشگاه دارای کد باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    if (customerClub.HasCustomCode)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.CardNumber;
                    }
                    // در غیر اینصورت اگر باشگاه دارای کد نباشد
                    //  ولی باشگاه دارای کد ملی باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    else if (customerClub.HasNationalCode)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.NationalCode;
                    }
                    // در غیر اینصورت اگر باشگاه دارای کد و کد ملی نباشد
                    //    ولی باشگاه دارای شمارخ موبایل باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                    else if (customerClub.HasMobileNumber)
                    {
                        additionalUserProp.CustomerClubCode = customerClubVerifyParameter.MobileNumber;
                    }
                    else
                    {
                        return "اطلاعات باشگاه مشتریان نامعتبر است";
                    }
                    // ست کردن اطلاعات باشگاه مشتریان
                    additionalUserProp.CustomerClubId = customerClubVerifyParameter.CustomerClubId;
                }
                else
                {
                    if (customerClubVerifyParameter.HasCardNumber)
                    {
                        var hekmatCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubsHekmatCustomerClubId);
                        additionalUserProp.CustomerClubId = hekmatCustomerClubId;
                    }
                    else
                    {
                        var publicCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubPublicCustomerClubId);
                        additionalUserProp.CustomerClubId = publicCustomerClubId;
                    }
                }
                await _additionalUserPropService.UpdateAsync(additionalUserProp, additionalUserProp.Id, true);
                return string.Empty;

            }
            // اگر کاربر وجود داشته باشد اما اطلاعات اضافی برای آن وجود نداشته باشد خطایی رخ داده است
            return "بروز خطا در انجام عملیات";

        }
        /// <summary>
        /// متد اعتبار سنجی ورودی ها هنگام ثبت نام
        /// و یا هنگامی که کاربر دارای اطلاعات کافی جهت ورود نداشته باشد و اکنون باید اطلاعات ثبت نامی خود راتکمیل کنید
        /// </summary>
        /// <param name="additionalInfoList"></param>
        /// <returns></returns>
        private async Task<string> CustomValidateAsync(Dictionary<string, string> additionalInfoList)
        {
            //در این حالت کاربر از سایت فروشگاه اقدام به  ثبت نام کرده است
            //یعنی کاربر عمومی
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                CustomerClubVerifyParameter customerClubVerifyParameter = GetCustomerClubVerifyParameter(additionalInfoList);

                if (_storeConfiguration.LimitedToCustomerClub)
                {
                    if (customerClubVerifyParameter.CustomerClubId.HasValue)
                    {
                        // اگر باشگاه دارای شماره کارت باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                        if (customerClubVerifyParameter.CardNumber != null)
                        {
                            var isExistenceCustomerClub = _additionalUserPropService.CheckExistenceByFilter(customerClubVerifyParameter.CustomerClubId, customerClubVerifyParameter.CardNumber);
                            if (isExistenceCustomerClub)
                            {
                                return "کاربر گرامی قبلا با این مشخصات باشگاه مشتریان، در سامانه ثبت نام انجام شده است";
                            }
                        }
                        // در غیر اینصورت اگر باشگاه دارای شماره کارت نباشد
                        //  ولی باشگاه دارای کد ملی باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                        else if (customerClubVerifyParameter.NationalCode != null)
                        {
                            var isExistenceCustomerClub = _additionalUserPropService.CheckExistenceByFilter(customerClubVerifyParameter.CustomerClubId, customerClubVerifyParameter.NationalCode);
                            if (isExistenceCustomerClub)
                            {
                                return "کاربر گرامی قبلا با این مشخصات باشگاه مشتریان، در سامانه ثبت نام انجام شده است";
                            }
                        }
                        // در غیر اینصورت اگر باشگاه دارای شماره کارت و کد ملی نباشد
                        //    ولی باشگاه دارای شمارخ موبایل باشد آن را به عنوان کد باشگاه مشتریان در نظر بگیرد
                        else if (customerClubVerifyParameter.MobileNumber != null)
                        {
                            var isExistenceCustomerClub = _additionalUserPropService.CheckExistenceByFilter(customerClubVerifyParameter.CustomerClubId, customerClubVerifyParameter.MobileNumber);
                            if (isExistenceCustomerClub)
                            {
                                return "کاربر گرامی قبلا با این مشخصات باشگاه مشتریان، در سامانه ثبت نام انجام شده است";
                            }
                        }
                        var verifyCustomer = await _customerClubService.VerifyCustomerAsync(customerClubVerifyParameter);

                        if (verifyCustomer)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return "کاربر گرامی عضویت شما در باشگاه مشتریان انتخاب شده تایید نشد";
                        }
                    }
                    else
                    {
                        return "کاربر گرامی برای ثبت نام در این سایت، شما باید عضو یکی از باشگاه های مشتریان طرف قرارداد باشید";
                    }
                }
                else
                {
                    if (customerClubVerifyParameter.HasCardNumber
                        && (string.IsNullOrEmpty(customerClubVerifyParameter.CardNumber) || !customerClubVerifyParameter.CardNumber.IsValidCardNumber()))
                    {
                        return "کاربر گرامی، لطفا شماره کارت 16 رقمی حکمت را بصورت صحیح وارد نمایید";
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// صفحه ثبت نام کاربران اختصاصی برای این ماژول
        /// </summary>
        /// <returns></returns>
        public ActionRouteInfo GetCustomRegisterViewInfo()
        {
            if (_storeConfiguration.LimitedToCustomerClub)
            {
                return new ActionRouteInfo() { ActionName = "CustomerClubRegister" };
            }
            return null;
        }

        /// <summary>
        /// پارامتر ها را از دیکشنری به 
        /// ViewModel تبدیل می کند 
        /// </summary>
        /// <param name="additionalInfoList"></param>
        /// <returns></returns>
        public CustomerClubVerifyParameter GetCustomerClubVerifyParameter(Dictionary<string, string> additionalInfoList)
        {
            CustomerClubVerifyParameter customerClubVerifyParameter = new CustomerClubVerifyParameter();
            additionalInfoList.TryGetValue("customerClubId", out var customerClubIdStr);
            if (!string.IsNullOrEmpty(customerClubIdStr) && int.TryParse(customerClubIdStr, out var customerClubId))
            {
                customerClubVerifyParameter.CustomerClubId = customerClubId;
            }
            additionalInfoList.TryGetValue("cardNumber", out var cardNumber);
            if (!string.IsNullOrEmpty(cardNumber))
            {
                customerClubVerifyParameter.CardNumber = cardNumber;
            }

            additionalInfoList.TryGetValue("hasCardNumber", out var hasCardNumberStr);
            if (!string.IsNullOrEmpty(hasCardNumberStr) && bool.TryParse(hasCardNumberStr, out var hasCardNumber))
            {
                customerClubVerifyParameter.HasCardNumber = hasCardNumber;
            }
            additionalInfoList.TryGetValue("nationalCode", out var nationalCode);
            if (!string.IsNullOrEmpty(nationalCode))
            {
                customerClubVerifyParameter.NationalCode = nationalCode;
            }
            additionalInfoList.TryGetValue("mobileNumber", out var mobileNumber);
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                customerClubVerifyParameter.MobileNumber = mobileNumber;
            }
            return customerClubVerifyParameter;
        }

        /// <summary>
        /// بعد از لاگین کردن کاربر این متد فراخوانی می شود
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        public async Task<string> AfterLogin(int userId)
        {
            await _userService.CheckNeedToAddActivityLogForUserLoginLogoutEventAndDoAsync(true, userId);

            string result = string.Empty;

            await Task.CompletedTask;
            return result;
        }

        public IQueryable<User> GetAll()
        {
            return _additionalUserPropService.GetAllUsersByFilter(null, null, null, null);
        }


        public IQueryable<User> GetForSelectByFilter(int? companyId, int? representationId, int? representationCategoryId, int? personId, bool considerCurrentUser = true)

        {
            return _additionalUserPropService.GetAllUsersByFilter(companyId, representationId, representationCategoryId, personId, null, considerCurrentUser);
        }
    }
}