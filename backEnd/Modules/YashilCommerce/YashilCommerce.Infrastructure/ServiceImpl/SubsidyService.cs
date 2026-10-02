using System;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using Yashil.Core.Configurations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Interfaces;
using YashilCommerce.Core.Repositories;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.DTO;
using Yashil.Common.SharedKernel.Logging;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Caching;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class SubsidyService : GenericService<Subsidy, int>, ISubsidyService
    {
        private readonly ISubsidyRepository _subsidyRepository;
        private readonly ISataSubsidyApprovalConfiguration _sataSubsidyApprovalConfiguration;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IYashilLogger<Subsidy> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public SubsidyService(IUnitOfWork unitOfWork, ISubsidyRepository subsidyRepository, IUserPrincipal userPrincipal,
            ISataSubsidyApprovalConfiguration sataSubsidyApprovalConfiguration, IEventPublisher eventPublisher, IProductSalesPlanningService salesPlanningService, IYashilLogger<Subsidy> logger, ApplicationConfiguration applicationConfiguration, IStaticCacheManager staticCacheManager)
            : base(unitOfWork, subsidyRepository, userPrincipal, eventPublisher)
        {
            _subsidyRepository = subsidyRepository;
            _sataSubsidyApprovalConfiguration = sataSubsidyApprovalConfiguration;
            _productSalesPlanningService = salesPlanningService;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        public Task<Subsidy> GetApplicationCurrentSubsidyAsync(int applicationId)
        {
            return _subsidyRepository.GetApplicationCurrentSubsidyAsync(applicationId);
        }
        public Subsidy GetCurrentSubsidy()
        {
            return _subsidyRepository.GetCurrentSubsidy();
        }

        /// <summary>
        /// دریافت توکن بعد از لاگین در سرویس ساتا 
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateTokenForSataServiceAsync()
        {
            var token = string.Empty;
            var url = _sataSubsidyApprovalConfiguration.Url;
            var username = CryptographyHelper.Base64Encode(_sataSubsidyApprovalConfiguration.UserName);
            var password = CryptographyHelper.Base64Encode(_sataSubsidyApprovalConfiguration.Password);
            var client = new RestClient(url + "api/token/login");
            var request = new RestRequest()
            {
                Method = Method.Post
            };
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "username=" + username + "&password=" + password, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            var responseStatus = response.StatusCode;
            if (responseStatus == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<SataSubsidyApprovalLoginParams>(response.Content);
                token = result.Message.Data.Access_token;
            }
            return token;
        }

        /// <summary>
        /// وب سرویس استعلام یارانه ساتا یعنی آیا باید به کد ملی ارسالی یارانه تخصیص داده شود یانه
        /// </summary>
        /// <param name="nationalCode">کد ملی خریدار</param>
        /// <param name="identificationCode"></param>
        /// <returns>در صورتی که موفق به بررسی سوبسید ساتا شود مقدار درست و در غیر این صورت نادرست بر می گرداند</returns>
        public async Task<SataSubsidyApprovalResult> TrySataSubsidyApprovalAsync(string nationalCode, string identificationCode = null)
        {
            var sataSubsidyApprovalResult = new SataSubsidyApprovalResult();
            if (string.IsNullOrEmpty(_sataSubsidyApprovalConfiguration.Url))
            {
                throw new Exception("مسیر وب سرویس ساتا برای چک کردن یارانه در فایل تنظیمات ست نشده است");
            }
            try
            {
                sataSubsidyApprovalResult.CanCheck = false;
                sataSubsidyApprovalResult.Result = false;
                var token = await CreateTokenForSataServiceAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    var client = new RestClient(_sataSubsidyApprovalConfiguration.Url + "api/common/checkFamilyMembership");
                    var request = new RestRequest()
                    {
                        Method = Method.Post
                    };
                    request.RequestFormat = DataFormat.Json;
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");
                    var sataCheckFamilyMembershipParam = new SataCheckFamilyMembershipParam
                    {
                        nationalCode = CryptographyHelper.Base64Encode(nationalCode),
                        identificationCode = identificationCode == null ? string.Empty : CryptographyHelper.Base64Encode(identificationCode)
                    };
                    var param = JsonConvert.SerializeObject(sataCheckFamilyMembershipParam);
                    request.AddStringBody(param, ContentType.Json);
                    //request.AddParameter("application/json", param, ParameterType.RequestBody,true);
                    var response = await client.ExecuteAsync(request);
                    var sataCheckFamilyMembershipRoot =
                        JsonConvert.DeserializeObject<SataCheckFamilyMembershipRoot>(response.Content);
                    //در صورتی که 
                    // Status برابر با صفر باشد یعنی استعلام  با موفقیت انجام شده است
                    // اگر Status مخالف صفر باشد یعنی استعلام با موفقیت انجام نشده است 
                    if (sataCheckFamilyMembershipRoot != null && sataCheckFamilyMembershipRoot.Status == 0)
                    {
                        //Data  نتیجه استعلام یارانه ساتا بودن فیلد 
                        // می باشد اگر True
                        // باشد یعنی کد ملی یارانه ساتا می تواند دریافت کند در غیر اینصورت یارانه ساتا نمی تواند دریافت کند 
                        sataSubsidyApprovalResult.Result = sataCheckFamilyMembershipRoot.Message.Data;
                        sataSubsidyApprovalResult.CanCheck = true;
                    }
                }
                else
                {
                    sataSubsidyApprovalResult.CanCheck = false;
                }


            }
            catch (Exception e)
            {
                sataSubsidyApprovalResult.Result = false;
                sataSubsidyApprovalResult.CanCheck = false;
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.GetEntityException,
                    "Subsidy",
                    $"خطا در فراخوانی سرویس ساتا : {e.Message}",
                    null);

            }

            return sataSubsidyApprovalResult;

        }

        public override async Task AfterSaveChangesAsync(int commitAsync)
        {
            await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync();
        }
        /// <summary>
        /// تنظیم یارانه قبلی برنامه را غیر فعال می کند 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public async Task DeactivateOldSubsidyByApplicationIdAsync(int applicationId)
        {
            await _subsidyRepository.DeactivateOldSubsidyByApplicationIdAsync(applicationId);
        }

        public IQueryable<Subsidy> GetByCustomFilter(int? categoryGroupId, int? subsidyGroupId)
        {
            return _subsidyRepository.GetByCustomFilter(categoryGroupId, subsidyGroupId);
        }

        public Task<bool> CheckExistCategoryGroupActiveSubsidyAsync(int categoryGroupId)
        {
            return GetByCustomFilter(categoryGroupId, null)
                .AnyAsync(x => x.IsActive);
        }

        public IQueryable<Subsidy> FindById(int id, bool readOnly = true)
        {
            return _subsidyRepository.FindById(id, readOnly);
        }

        public Task UpdateSubsidyStatusBySubsidyConsumption()
        {
            return _subsidyRepository.UpdateSubsidyStatusBySubsidyConsumption(_applicationConfiguration.ApplicationId);
        }

        public Task<Subsidy> GetCategoryGroupActiveSubsidyInfoAsync(int employmentCategoryGroupId)
        {
            return _subsidyRepository.GetByCategoryGroupId(employmentCategoryGroupId)
                .Where(x => x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
