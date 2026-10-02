using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.SharedKernel.Logging;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using YashilCommerce.Core.Model;
using System.Collections;
using System.Collections.Generic;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class AdditionalUserPropService : GenericService<AdditionalUserProp, int>, IAdditionalUserPropService
    {
        private readonly IAppConfigService _appConfigService;
        private readonly IMemoryCache _cache;
        private readonly IWebHelper _webHelper;
        private readonly IAdditionalUserPropRepository _additionalUserPropRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IUserService _userService;
        private readonly IYashilLogger<AdditionalUserProp> _logger;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICustomerClubService _customerClubService;

        public AdditionalUserPropService(
            IUnitOfWork unitOfWork,
            IAdditionalUserPropRepository additionalUserPropRepository,
            IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher,
            IUserService userService, IYashilLogger<AdditionalUserProp> logger,
            IWebHelper webHelper,
            IStaticCacheManager staticCacheManager,
            ICustomerClubService customerClubService, IMemoryCache cache, IAppConfigService appConfigService)
            : base(unitOfWork, additionalUserPropRepository, userPrincipal, eventPublisher)
        {
            _additionalUserPropRepository = additionalUserPropRepository;
            _userPrincipal = userPrincipal;
            _userService = userService;
            _logger = logger;
            _webHelper = webHelper;
            _staticCacheManager = staticCacheManager;
            _customerClubService = customerClubService;
            _cache = cache;
            _appConfigService = appConfigService;
        }

        /// <summary>
        /// چک میکند کاربری با شناسه باشگاه و کد باشگاه وجود دارد یا نه
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <param name="customerClubCode"></param>
        /// <returns></returns>
        public bool CheckExistenceByFilter(int? customerClubId, string customerClubCode)
        {
            return _additionalUserPropRepository.CheckExistenceByFilter(customerClubId, customerClubCode);
        }

        public IQueryable<User> GetAllUsersByFilter(
            int? companyId,
            int? representationId,
            int? representationCategoryId,
            int? personId,
            List<int> itemsGroupId = null,
            bool considerCurrentUser = true)
        {

            var additionalUserProp = GetByUserId(_userPrincipal.Id);

            var selectedCompanyId = considerCurrentUser && additionalUserProp != null
                ? additionalUserProp.CompanyId
                : companyId;
            var selectedRepresentationId = considerCurrentUser && additionalUserProp != null
                ? additionalUserProp.RepresentationId
                : representationId;
            var selectedRepresentationCategoryId = considerCurrentUser && additionalUserProp != null
                ? additionalUserProp.RepresentationCategoryId
                : representationCategoryId;

            var query = _userService
                .GetAllByApplicationId(_userPrincipal.ApplicationId, true)
                .Where(x => x.OrganizationId == _userPrincipal.OrganizationId);

            if (selectedCompanyId.HasValue)
            {
                query = query.Where(x => x.AdditionalUserPropUser.CompanyId == selectedCompanyId.Value);
            }

            if (selectedRepresentationId.HasValue)
            {
                query = query.Where(x => x.AdditionalUserPropUser.RepresentationId == selectedRepresentationId.Value);
            }

            if (selectedRepresentationCategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.AdditionalUserPropUser.RepresentationCategoryId == selectedRepresentationCategoryId.Value);
            }

            if (itemsGroupId != null && itemsGroupId.Any())
            {
                query = query.Where(x => x.AdditionalUserPropUser.AdditionalUserPropGroupId.HasValue &&
                  itemsGroupId.Contains(x.AdditionalUserPropUser.AdditionalUserPropGroupId.Value));
            }

            if (personId.HasValue)
            {
                query = query.Where(x => x.PersonId == personId);
            }

            return query;

        }

        public AdditionalUserProp GetByUserIdAndApplicationId(int userId, int applicationId)
        {
            return _additionalUserPropRepository.GetByUserIdAndApplicationId(userId, applicationId);
        }

        public IQueryable<AdditionalUserProp> FindByUserId(int id)
        {
            return _additionalUserPropRepository.FindByUserId(id);
        }

        public AdditionalUserProp GetByUserId(int userId, bool readOnly = false)
        {
            return _additionalUserPropRepository.GetByUserId(userId, readOnly);
        }

        public IQueryable<AdditionalUserProp> GetByCustomFilter(int? userId, int? applicationId)
        {
            return _additionalUserPropRepository.GetByCustomFilter(userId, applicationId);
        }

        public Task<bool> IsExistAdditionalUserPropForCompanyAsync(int companyId)
        {
            return _additionalUserPropRepository.GetByCompanyId(companyId).AnyAsync();
        }

        /// <summary>
        /// بروزرسانی مقدار
        /// ParticipatedInHumaxTest
        /// به مقدار
        /// True
        /// برای کاربرانی که در آزمون استعداد سنجی شرکت کرده اند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateParticipatedInHumaxTestValueAsync(int userId)
        {
            var additionalUserProp = await FindByUserId(userId).FirstOrDefaultAsync();

            if (additionalUserProp == null)
                return false;

            if (!additionalUserProp.ParticipatedInHumaxTest)
            {
                try
                {
                    additionalUserProp.ParticipatedInHumaxTest = true;
                    await UpdateAsync(additionalUserProp, additionalUserProp.Id, true);
                    await RefreshAdditionalUserPropAsync(_userPrincipal.Id);
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(AdditionalUserProp), "بروز خطا به هنگام  بروزرسانی مقدار ParticipatedInHumaxTest", e);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ویژگی های مربوط کاربر جاری را
        /// با توجه به دامنه آدرس سایت
        /// بر می گرداند
        /// ویژگی ها مانند باشگاه مشتریان کاربر جاری در دامنه سایت
        /// و حکمتی باشد کاربر جاری و ...
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AdditionalUserPropsDto> GetUserAdditionalUserPropAsync(int userId)
        {
            var domainName = _webHelper.GetDomainName();
            object cacheKeyParam;
            if (userId == 0)
            {
                cacheKeyParam = domainName;
            }
            else
            {
                cacheKeyParam = userId;
            }
            var cacheKey = _staticCacheManager
                .PrepareKeyForDefaultCache(YashilModelCacheDefaults<AdditionalUserProp>
                    .AdditionalUserPropsModelKey, cacheKeyParam);
            cacheKey.CacheTime = (int)CacheTime.OneHour;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                int userCustomerClubId;
                if (userId != 0)
                {
                    var additionalUserProp = GetByUserId(userId);
                    if (additionalUserProp != null)
                    {
                        if (additionalUserProp.CustomerClubId.HasValue)
                        {
                            userCustomerClubId = additionalUserProp.CustomerClubId.Value;
                        }
                        else
                        {
                            userCustomerClubId = await GetPublicCustomerClubId();
                        }
                    }
                    else
                    {
                        throw new Exception($"User With Id:{userId} does not have additionalUserProp!");
                    }
                }
                else
                {
                    userCustomerClubId = await GetDefaultCustomerClubId(domainName);
                }



                var additionalUserPropsDto =
                 await GetUserAdditionalUserPropByCustomerClubIdAsync(userCustomerClubId);

                return additionalUserPropsDto;
            });
        }
        /// <summary>
        /// ویژگی های مربوط به باشگاه مشتریان  را بر می گرداند
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        private async Task<AdditionalUserPropsDto> GetUserAdditionalUserPropByCustomerClubIdAsync(int customerClubId)
        {
            var additionalUserPropsDto = await _customerClubService
                .FindById(customerClubId, true)
                .Select(x => new AdditionalUserPropsDto
                {
                    CustomerClubId = x.Id,
                    SupportSubsidy = x.SupportSubsidy,
                    AddressBased = x.AddressBased,
                    HekmatBased = x.CustomerClubPaymentMethodType
                        .Any(ccp =>
                            ccp.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                            ),
                    InstallmentBased = x.CustomerClubPaymentMethodType
                        .Any(ccp =>
                            ccp.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                            || ccp.PaymentMethodType == (int)PaymentMethodTypesEnum.Samin
                        ),
                    RegistrationModeType = x.RegistrationModeType,
                    IsResumeFillOutEnough = x.AdditionalUserProp.Where(aup => aup.UserId == _userPrincipal.Id)
                        .Select(aup => aup.IsResumeFillOutEnough).FirstOrDefault(),
                    HasUserPurchasedEmploymentExam = x.AdditionalUserProp.Where(aup => aup.UserId == _userPrincipal.Id)
                        .Select(aup => aup.PurchasedEmploymentExam).FirstOrDefault(),
                    ParticipatedInhumaxTest = x.AdditionalUserProp.Where(aup => aup.UserId == _userPrincipal.Id)
                        .Select(aup => aup.ParticipatedInHumaxTest).FirstOrDefault(),
                }).FirstOrDefaultAsync();

            return additionalUserPropsDto;
        }

        /// <summary>
        /// پاک کردن ویژگی کاربر از حافظه
        /// </summary>
        /// <param name="userId"></param>
        public void TryRemoveUserAdditionalUserProp(int userId)
        {
            _cache.Remove(userId);
        }

        public Task RefreshAdditionalUserPropAsync(int userId)
        {
            var cacheKey =
                _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<AdditionalUserProp>.AdditionalUserPropsModelKey, userId);
            return _staticCacheManager.RemoveAsync(cacheKey);
        }
        /// <summary>
        /// دریافت شناسه باشگاه مشتریان پیش فرض با توجه به دامین سایت
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetDefaultCustomerClubId(string domainName)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<AppConfig>.DefaultCustomerClubId, domainName);
            cacheKey.CacheTime = (int)CacheTime.Default;

            var appConfigKey = $"YashilCommerce.CustomerClub.{domainName}.DefaultCustomerClubId";
            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            await _appConfigService.GetIntValueAsync(appConfigKey));

        }

        /// <summary>
        /// شناسه باشگاه مشتریان عمومی را بر میگرداند
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetPublicCustomerClubId()
        {
            return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubPublicCustomerClubId);
        }
        /// <summary>
        /// بررسی می کند که آیا باشگاه مشتریان کاربر جاری حکمتی می باشد یا خیر 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckIsHekmatCurrentUserCustomerClub()
        {
            var hekmatCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                .YashilCommerceCustomerClubsHekmatCustomerClubId);
            var additionalUserPropAsync = await GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return additionalUserPropAsync.CustomerClubId == hekmatCustomerClubId;
        }
        /// <summary>
        /// این متد کار
        /// بروزرسانی مقدار
        /// HasUserPurchasedEmploymentExam
        /// به
        /// true
        /// را انجام می دهد و در صورتی فراخوانی می شود که کاربر آزمون استعدادیابی شغلی را خریده باشد 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateHasUserPurchasedEmploymentExamAsync(int userId)
        {
            var additionalUserProp = await FindByUserId(userId).FirstOrDefaultAsync();

            if (additionalUserProp == null)
                return false;

            if (!additionalUserProp.PurchasedEmploymentExam.HasValue || !additionalUserProp.PurchasedEmploymentExam.Value)
            {
                try
                {
                    additionalUserProp.PurchasedEmploymentExam = true;
                    await UpdateAsync(additionalUserProp, additionalUserProp.Id, true);
                    await RefreshAdditionalUserPropAsync(_userPrincipal.Id);
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(AdditionalUserProp), "بروز خطا به هنگام  بروزرسانی مقدار HasUserPurchasedEmploymentExam", e);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// این متد عملیات بروزرسانی مقدار
        /// IsResumeFillOutEnough  = true
        /// را انجام می دهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateIsResumeFillOutEnoughAsync(int userId)
        {
            var additionalUserProp = await FindByUserId(userId).FirstOrDefaultAsync();

            if (additionalUserProp == null)
                return false;

            if (!additionalUserProp.IsResumeFillOutEnough)
            {
                additionalUserProp.IsResumeFillOutEnough = true;

                try
                {
                    await UpdateAsync(additionalUserProp, additionalUserProp.Id, true);
                    await RefreshAdditionalUserPropAsync(_userPrincipal.Id);
                    return true;
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(AdditionalUserProp), "بروز خطا به هنگام  بروزرسانی مقدار IsUserFillOutResumeEnough", e);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// این متد عملیات تبدیل رشته رزومه ارسالی کاربر به مدل رزومه را انجام می دهد
        /// </summary>
        /// <param name="resume">رشته رزومه کاربر</param>
        /// <returns>UserResume | Null</returns>
        public Resume DeserializeResumeStringToResumeModel(string resume)
        {
            try
            {
                var userResume = JsonConvert.DeserializeObject<Resume>(resume);

                if (userResume != null)
                    return userResume;

                return null;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InvalidModel), nameof(AdditionalUserProp), $"failed to parse user resume userId: {_userPrincipal.Id}", e);
                return null;
            }
        }

        /// <summary>
        /// این متد عملیات مربوط به
        /// تبدیل مدل رزومه کاربر به رشته جی سون
        /// را انجام می دهد
        /// </summary>
        /// <param name="resume">رزومه کاربر</param>
        /// <returns>string</returns>
        public string SerializeResumeToJson(Resume resume)
        {

            try
            {
                return JsonConvert.SerializeObject(resume);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InvalidModel, "SerializeFailed"), nameof(AdditionalUserProp), $"failed to parse user resume userId: {_userPrincipal.Id}", e);
                return string.Empty;
            }
        }

        /// <summary>
        /// بررسی می کند که آیا کاربر به مقدار کافی رزومه را پر کرده است یا نه؟
        /// </summary>
        /// <param name="userResume">رزومه کاربر</param>
        /// <returns>bool</returns>
        public bool HasUserFillOutResumeEnough(Resume userResume)
        {
            return (userResume != null &&
                    userResume.Information != null &&
                    !string.IsNullOrEmpty(userResume.Information.NationalId) &&
                    !string.IsNullOrEmpty(userResume.Information.BirthYear) &&
                    !string.IsNullOrEmpty(userResume.Information.Mobile) &&
                    !string.IsNullOrEmpty(userResume.Information.Province));
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر رزومه را به اندازه ی کافی پر کرده است یا نه؟
        /// همچنین بررسی می کند که سن کاربر برای شرکت در آزمون استعداد یابی در باز مجاز باشد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>(bool isSuccess, string message)</returns>
        public async Task<(bool isSuccess, string message)> ValidateUserResumeAndUserAgeAsync(int userId)
        {
            var userMustFillResumeMessage = "کاربر گرامی، جهت بهره مندی از یارانه اشتغال و شرکت در آزمون هدایت شغلی، لطفا رزومه خود را بررسی و تکمیل نمایید.";
            var resumeString = await FindByUserId(userId)
                .Select(user => user.Resume).FirstOrDefaultAsync();

            if (resumeString == null)
                return (false, userMustFillResumeMessage);

            Resume userResume;

            try
            {
                userResume = JsonConvert.DeserializeObject<Resume>(resumeString);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InvalidModel, "SendSmsFailed"), nameof(User), $"failed to parse user resume userId: {_userPrincipal.Id}", e);
                return (false, userMustFillResumeMessage);
            }

            if (HasUserFillOutResumeEnough(userResume))
            {
                var todayShamsiYear = DateFunction.TodayShamsiYearAndMonthInt().ToString().Substring(0, 4);

                if (int.TryParse(userResume?.Information?.BirthYear, out var birthYearInt))
                {
                    var userAge = int.Parse(todayShamsiYear) - birthYearInt;

                    if ((userAge >= 17 && userAge <= 40))
                        return (true, string.Empty);

                    return (false, "کاربر گرامی سن شما در بازه مجاز جهت شرکت در آزمون استعدادیابی نمی باشد!");
                }
                else
                {
                    return (false, "کاربر گرامی مقدار انتخاب شده برای سال تولد صحیح نمی باشد!");
                }
            }

            return (false, " کاربر گرامی لطفا مقادیر کدملی، سال تولد، شماره موبایل و استان محل سکونت رزومه خود را کامل پرنمایید");
        }

        public Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId)
        {
            return _additionalUserPropRepository.CheckExistRepresentationCategoryByCategoryIdAsync(categoryId);
        }

        /// <summary>
        /// دریافت رزومه کاربر بر اساس شناسه کاربری
        /// در صورتی که کاربر رزومه را پر نکرده باشد مقدار 
        /// null
        /// بر میگرداند همجنین اگر در هنگام 
        /// deserialize
        /// کردن resume به مدل
        /// خطای رخ دهد نیز مقدار null 
        /// بر میگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Resume || null</returns>
        public async Task<Resume> GetResumeByUserIdAsync(int userId)
        {
            var resumeString = await FindByUserId(userId).Select(additionalUserProp => additionalUserProp.Resume).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(resumeString))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Resume>(CryptographyHelper.AesDecrypt(resumeString));
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InvalidModel, "Error occurred in deserializing resume string to resume Model"),
                    nameof(Resume), $"failed to parse user resume userId: {_userPrincipal.Id}", e);
            }

            return null;
        }

        public Task<bool> ExistsAnyAdditionalUserPropServiceByItemGroupIdAsync(int itemGroupId)
        {
            return _additionalUserPropRepository.ExistsAnyAdditionalUserPropServiceByItemGroupId(itemGroupId);
        }

        public IQueryable<AdditionalUserProp> GetUsersAssignToGroup(int groupId)
        {
            return _additionalUserPropRepository.GetUsersAssignToGroup(groupId);
        }

        public IQueryable<AdditionalUserProp> GetUsersForNewGroup(bool readOnly = false)
        {
            return _additionalUserPropRepository.GetUsersForNewGroup(readOnly);
        }




        /// <summary>
        /// این متد بررسی می کند که آیا کاربر رزومه را پر کرده است یا نه 
        /// اگر پر نکرده باشد مقدار 0 بر می گرداند یعنی در حالتی که روزمه کاربر مقدار 
        /// null داشته باشد
        /// در غیر این صورت محاسبه می کند که چند در صد از رزومه را پر کرده است و سپس خروجی را به صورت عدد می دهد
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public async Task<int> GetResumeCompletionPercentageByUserIdAsync(int userId)
        {
            var resume = await GetResumeByUserIdAsync(userId);

            if (resume == null)
                return default;

            int rate = default(int);

            if (resume.Information != null)
            {
                var countOfNullOrEmptyValueOfInformation = GetCountOfNullOrEmptyValueOfObject(resume.Information);
                if (countOfNullOrEmptyValueOfInformation >= 0 && countOfNullOrEmptyValueOfInformation <= 3)
                    rate += 15;
            }

            if (resume.CareerProfile != null)
            {
                var countOfNullOrEmptyValueOfCareerProfile = GetCountOfNullOrEmptyValueOfObject(resume.CareerProfile.JobPreferences);
                if (countOfNullOrEmptyValueOfCareerProfile >= 0 && countOfNullOrEmptyValueOfCareerProfile <= 3)
                    rate += 15;
            }
            if (resume.Education != null && resume.Education.Count > 0)
                rate += 15;

            if (resume.CareerProfile != null && (resume.CareerProfile.Jobs != null || resume.CareerProfile.JobPreferences != null)
                                             && (resume?.CareerProfile?.Jobs?.Count > 0 || resume?.CareerProfile?.JobPreferences != null))
                rate += 15;

            if (resume.Certificates != null && resume.Certificates.Count > 0)
                rate += 20;

            if (resume.Languages != null && resume.Languages.Count > 0)
                rate += 20;

            return rate;
        }

        /// <summary>
        /// این متد کار شمارش تعداد پروپرتی های که دارای مقدار null Or Empty 
        /// هستند را برای یک مدل انجام می دهد
        /// این تد که به صورت جنریک است
        /// یک مدل از ورودی دریافت می کند و پروپرتی های آن را بررسی می کند
        /// اگر از نوع لیست باشد 
        /// بررسی میکند که آیا لیست مقداری دارد یا نه
        /// اگر نداشته باشد به عددی خروجی 
        /// یکی اضافه می کند و اگر پروپرتی از نوع int or string
        /// باشد بررسی می کند که مقدارش
        /// null or empty
        /// نباشد و اگر برعکس بود 
        /// یک عدد به خروجی اضافه می کند
        /// این کار را به ازای تمامی پروپرتی های مدل انجام می دهد و نتیجه خروجی را به صورت عددی بر می گرداند
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="sourceModel"></param>
        /// <returns></returns>
        private int GetCountOfNullOrEmptyValueOfObject<TModel>(TModel sourceModel)
        {
            int countOfNullOrEmptyValue = default(int);

            if (sourceModel != null)
            {
                foreach (var property in sourceModel.GetType().GetProperties())
                {
                    var valueOfProperty = property?.GetValue(sourceModel);
                    if (valueOfProperty is IList && ((IList)valueOfProperty).Count == 0)
                    {
                        ++countOfNullOrEmptyValue;
                    }
                    else if (valueOfProperty == null || string.IsNullOrEmpty(valueOfProperty.ToString()))
                    {
                        ++countOfNullOrEmptyValue;
                    }

                }
            }

            return countOfNullOrEmptyValue;
        }

        public Task<IQueryable<AdditionalUserProp>> GetAdditionalUserPropNotAssignedToGroupAsync(
            int groupId,
            int? companyId,
            int? representationId,
            int? categoryId,
            int? userGroupId)
        {
            return _additionalUserPropRepository.GetAdditionalUserPropNotAssignedToGroupAsync(groupId,
                companyId, representationId, categoryId, userGroupId);
        }
        public IQueryable<AdditionalUserProp> GetAdditionalUsersPropAssignedToGroup(int itemGroupId)
        {
            return _additionalUserPropRepository.GetAdditionalUsersPropAssignedToGroup(itemGroupId);
        }
        public async Task<bool> AssignSelectedItemsToGroupAsync(List<int> additionalUserPropIds, int groupId, bool assign)
        {
            await _additionalUserPropRepository.AssignSelectedItemsToGroupAsync(additionalUserPropIds, groupId, assign);
            try
            {
                await SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)Events.UpdateException, Events.UpdateException.ToString()),
                    nameof(additionalUserPropIds),
                    e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// این متد جهت بروزرسانی باشگاه مشتریان کاربر جاری به
        /// نوع حکمت کارت کاربرد دارد
        /// در صورت بروز خطا Exception
        /// می دهد که باید توسط متد فراخواننده مدیریت شود
        /// </summary>
        /// <param name="hekamtCardNumber">شماره کارت حکمت</param>
        /// <returns></returns>
        public async Task UpdateCurrentUserCustomerClubToHekmatAsync(string hekamtCardNumber = null)
        {
            var hekmatCustomerClubId = await _appConfigService
                .GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubsHekmatCustomerClubId);
            var additionalUserProp = GetByUserId(_userPrincipal.Id);

            if (additionalUserProp.CustomerClubId != hekmatCustomerClubId)
                additionalUserProp.CustomerClubId = hekmatCustomerClubId;

            if (!string.IsNullOrEmpty(hekamtCardNumber))
                additionalUserProp.CardNumber = hekamtCardNumber;

            await UpdateAsync(additionalUserProp, additionalUserProp.Id, true);
            await RefreshAdditionalUserPropAsync(_userPrincipal.Id);
        }
    }
}
