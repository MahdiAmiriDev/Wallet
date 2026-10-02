using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using YashilCommerce.Core.Classes;
using YashilUserManagement.Core.Services;
using Yashil.Core.CustomDtos.Representation;
using YashilTms.Core.Repositories;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class RepresentationService : GenericService<Representation, int>, IRepresentationService
    {

        private readonly IRepresentationRepository _representationRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserService _userService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepresentationCategoryService _representationCategoryService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly IFactorRepository _factorRepository;
        private readonly IProductSalesPlanningRepository _productSalesPlanningRepository;
        private readonly ISaleConfigurationRepository _saleConfigurationRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IRepresentationTeacherRepository _representationTeacherRepository;

        public RepresentationService(
            IUnitOfWork unitOfWork,
            IRepresentationRepository representationRepository,
            IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher,
            IProductSalesPlanningService salesPlanningService,
            IAdditionalUserPropService additionalUserPropService
            , IUserService userService, ICategoryRepository categoryRepository,
            IRepresentationCategoryService representationCategoryService,
            ApplicationConfiguration applicationConfiguration, IAppConfigService appConfigService, IFactorRepository factorRepository, IProductSalesPlanningRepository productSalesPlanningRepository, ISaleConfigurationRepository saleConfigurationRepository, IShoppingCartItemRepository shoppingCartItemRepository, IRepresentationTeacherRepository representationTeacherRepository)
            : base(unitOfWork, representationRepository,
                userPrincipal, eventPublisher)
        {

            _representationRepository = representationRepository;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
            _userService = userService;
            _categoryRepository = categoryRepository;
            _representationCategoryService = representationCategoryService;
            _applicationConfiguration = applicationConfiguration;
            _appConfigService = appConfigService;
            _factorRepository = factorRepository;
            _productSalesPlanningRepository = productSalesPlanningRepository;
            _saleConfigurationRepository = saleConfigurationRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _representationTeacherRepository = representationTeacherRepository;
        }

        /// <summary>
        /// Get Representation By City Id
        /// </summary>
        /// <returns></returns>
        public IQueryable<Representation> GetByCityId(int cityId)
        {
            return _representationRepository.GetByCityId(cityId);
        }

        public Representation GetUserRepresentation(int userPrincipalId)
        {
            return _representationRepository.GetUserRepresentation(userPrincipalId);
        }

        public IQueryable<Representation> GetByCompanyMainCategory(int companyId)
        {
            return _representationRepository.GetByCompanyMainCategory(companyId);
        }
        public IQueryable<Representation> GetByCustomFilter(int? addressId, int? companyId, int? founderId, int? licenseType, int? ownershipType, bool showConfirmRepresentation = false)
        {
            return _representationRepository.GetByCustomFilter(addressId, companyId, founderId, licenseType, ownershipType, showConfirmRepresentation);
        }

        public async Task<IQueryable<Representation>> GetCompanyRepresentationByFilterAsync(
            int applicationId,
            List<int> company,
            int? cityId,
            int? categoryId,
            int? categoryGroupId)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var isEmploymentCategoryGroup = false;
            if (categoryGroupId.HasValue)
            {
                //گروه دسته بندی اشتغال
                var employmentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId, _applicationConfiguration.ApplicationId);
                // در صورتی که گروه دسته بندی اشتغالی باشد 
                isEmploymentCategoryGroup = employmentCategoryGroupId == categoryGroupId;
            }
            return _representationRepository.GetCompanyRepresentationByFilter(
                userAdditionalUserProp.CustomerClubId,
                applicationId,
                company,
                cityId,
                categoryId,
                categoryGroupId,
                isEmploymentCategoryGroup);
        }

        public string GetTitle(int id)
        {
            return _representationRepository.GetTitle(id);
        }

        public IQueryable<Representation> GetByCityIdAndCompanyId(int? cityId, int? companyId, bool readOnly = true)
        {
            return _representationRepository.GetByCityIdAndCompanyId(cityId, companyId);
        }
        /// <summary>
        /// دریافت نمایندگی بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">برنامه ریزی دوره</param>
        /// <returns></returns>
        public Task<Representation> GetByProductSalePlanningAsync(int productSalePlanningId)
        {
            return _representationRepository.GetByProductSalePlanningASync(productSalePlanningId);
        }
        public async Task<bool> CheckUserAccessForCompanyAndRepresentationAsync(int? companyId, int? representationId, int? userId = null)
        {
            var userCompanyAndRepresentation = await GetUserAdditionalAsync(userId);

            //اگر کاربر مرکز نداشت 
            //ادمین بوده و همه چیز درست است
            if (!userCompanyAndRepresentation.CompanyId.HasValue)
            {
                return true;
            }

            //کاربر مرکز ارسال نکرده است و ادمین هم نیست
            //اگر مرکز نداشت ولی نمایندگی داشت شناسه مرکز را از نمایندگی ارسالی می خوانیم
            if (!companyId.HasValue && representationId.HasValue)
            {
                var representation = await GetAsync(representationId, true);
                companyId = representation.CompanyId;
            }

            //اگر کاربر ادمین نبود و مرکز هم ارسال نکرده بود 
            //خطا می دهد به دلیل این که کاربر مرکز دارد و باید شناسه مرکز را ارسال کند
            if (!companyId.HasValue)
                return false;

            //اگر مرکز ارسالی و کاربر یکی نباشد خطا داریم
            if (userCompanyAndRepresentation.CompanyId != companyId)
            {
                return false;
            }

            //اگر کاربر مرکز دارد ولی نمایندگی ندارد درست است
            if (!userCompanyAndRepresentation.RepresentationId.HasValue)
            {
                return true;
            }

            //کاربر نمایندگی دارد و ارسال کرده و نمایندگی ارسالی یکی است.
            if (representationId.HasValue && userCompanyAndRepresentation.RepresentationId == representationId)
            {
                return true;
            }

            return false;
        }

        public Task<bool> IsExistRepresentationForCompanyAsync(int companyId)
        {
            return _representationRepository.GetByCompanyId(companyId).AnyAsync();
        }

        public IQueryable<Representation> GetByCompanyId(int companyId, bool readOnly)
        {
            return _representationRepository.GetByCompanyId(companyId);
        }

        public async Task AddCategoriesForRepresentationAsync(int representationId)
        {
            var allowAssignCategoryForCompanyAndRepresentation = await _appConfigService.GetBooleanValueAsync(
                ApplicationConfigKey.SiteConfigurationAssignCategoryForCompanyAndRepresentationAfterInsert
                , _applicationConfiguration.ApplicationId);
            if (allowAssignCategoryForCompanyAndRepresentation)
            {
                // دریافت دسته بندی هایی که ادمین تعریف کرده است
                // یعنی دسته بندی هایی که
                // CompanyId = null می باشد
                var categories =
                    _categoryRepository.GetByCustomFilter(null, null, null, false);
                foreach (var category in categories)
                {
                    await _representationCategoryService.AddAsync(new RepresentationCategory
                    {
                        RepresentationId = representationId,
                        CategoryId = category.Id,
                        ApplicationId = _userPrincipal.ApplicationId,
                        CreateBy = _userPrincipal.Id,
                        CreatorOrganizationId = _userPrincipal.OrganizationId
                    });
                }

                await _representationCategoryService.SaveChangeAsync();
            }
        }

        public async Task<bool> CheckUserAccessForCompanyAsync(int? companyId, int? userId = null)
        {
            var userCompanyAndRepresentation = await GetUserAdditionalAsync(userId);

            if (!userCompanyAndRepresentation.CompanyId.HasValue)
            {
                return true;
            }

            if (userCompanyAndRepresentation.CompanyId == companyId)
            {
                return true;
            }


            return false;
        }

        async Task<(int? CompanyId, int? RepresentationId)> GetUserAdditionalAsync(int? userId)
        {
            if (!userId.HasValue)
            {
                if (_userPrincipal != null)
                {
                    userId = _userPrincipal.Id;
                }
            }

            var userInfo = _userService.GetCurrentUserInfoAsync();


            if (userId != null && userInfo != null)
            {
                var userApplicationId = userInfo.Value.Result.ApplicationId;

                var res = _additionalUserPropService
                    .GetByUserIdAndApplicationId(userId.Value, userApplicationId);

                if (res != null)
                {
                    return (res.CompanyId, res.RepresentationId);
                }
                else
                {
                    throw new Exception("User AdditionalUserProp Is Null");
                }
            }

            await Task.CompletedTask;
            return (null, null);
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر به جاری به شناسه مرکز ارسالی دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="sentRepresentationId">شناسه نمایندگی ارسالی</param>
        /// <returns>bool</returns>
        public async Task<bool> HasCurrentUserAccessToSentRepresentationIdAsync(int sentRepresentationId)
        {
            var userCompanyInfo = await _additionalUserPropService
                .GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId)
                .Select(aup => new
                {
                    CompanyId = aup.CompanyId,
                    CompanyCodePath = aup.Company.CodePath,
                    RepresentationId = aup.RepresentationId,
                    RepresentationCompanyId = aup.RepresentationId.HasValue ? aup.Representation.Company.CodePath : null,

                }).FirstOrDefaultAsync();

            if (userCompanyInfo == null)
                return false;

            if (!userCompanyInfo.CompanyId.HasValue)
                return true;

            if (userCompanyInfo.RepresentationId.HasValue && userCompanyInfo.RepresentationId.Value == sentRepresentationId)
                return true;

            if (userCompanyInfo.CompanyCodePath.Contains(userCompanyInfo.RepresentationCompanyId.ToString()))
                return true;

            return false;
        }

        /// <summary>
        ///  دریافت لیست نمایندگی ها بر اساس پارامترهای ارسالی
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public IQueryable<Representation> GetRepresentationByParams(List<int> companyIds)
        {
            return _representationRepository.GetRepresentationByParams(companyIds);
        }

        /// <summary>
        ///  id دریافت جزییات نمایندگی بر اساس 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<Representation> GetCompanyDetails(int id)
        {
            return _representationRepository.GetCompanyDetails(id);
        }

        public Task<bool> IsHubRepresentationAsync(int representationId)
        {
            return GetAll(true)
                .AnyAsync(x => x.Id == representationId &&
                               x.IsProvinceHub.HasValue &&
                               x.IsProvinceHub.Value);
        }
        /// <summary>
        /// بررسی می کند که آیا امکان حذف نمایندگی مورد نظر وجود دارد یا نه
        /// تمام وابستگی های مربوط به نمایندگی بررسی می شود
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool isAllowed, string message)> AllowDeleteRepresntationAsync(int id)
        {
            var isRepresentaionCategoryExist = await _representationCategoryService.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isRepresentaionCategoryExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف دسته بندی نمایندگی، امکان حذف وجود ندارد.");
            }

            var isFactorExist = await _factorRepository.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isFactorExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف فاکتور، امکان حذف وجود ندارد.");
            }

            var isProductSalesPlanningExist = await _productSalesPlanningRepository.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isProductSalesPlanningExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف دوره، امکان حذف وجود ندارد.");
            }

            var isSalesConfigRepositoryExist = await _saleConfigurationRepository.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isSalesConfigRepositoryExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف تنظیمات فروش، امکان حذف وجود ندارد.");
            }

            var isShoppingCartItemExist = await _shoppingCartItemRepository.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isShoppingCartItemExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف کارت خرید، امکان حذف وجود ندارد.");
            }

            var isRepresentationTeacherExist = await _representationTeacherRepository.GetAll(true).AnyAsync(x => x.RepresentationId == id);
            if (isRepresentationTeacherExist)
            {
                return (false, "کاربر گرامی، با توجه به تعریف مربی نمایندگی، امکان حذف وجود ندارد.");
            }

            return (true, "");
        }

        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteRepresentationCategoryByIdAsync(int id)
        {
             await _representationCategoryService.DeleteByRepresentationIdAsync(id);
        }

        public IQueryable<Representation> GetByProductSalesPlanningCompany(int productSalesPlanningId)
        {
            return _representationRepository.GetByProductSalesPlanningCompany(productSalesPlanningId);
        }
    }
}