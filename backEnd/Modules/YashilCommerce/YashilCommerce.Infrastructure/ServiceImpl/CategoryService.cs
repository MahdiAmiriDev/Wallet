using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CategoryService : GenericService<Category, int>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductService _productService;
        private readonly ICompanyMainCategoryService _companyMainCategoryService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IRepresentationCategoryService _representationCategoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppConfigService _appConfigService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ICompanyService _companyService;
        private readonly IRepresentationService _representationService;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            IProductSalesPlanningService salesPlanningService,
            IAdditionalUserPropService additionalUserPropService,
            IProductService productService,
            ICompanyMainCategoryService companyMainCategoryService,
            IRepresentationCategoryService representationCategoryService, IAppConfigService appConfigService,
            ApplicationConfiguration applicationConfiguration, ICompanyService companyService,
            IRepresentationService representationService)
            : base(unitOfWork, categoryRepository, userPrincipal, eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _productSalesPlanningService = salesPlanningService;
            _additionalUserPropService = additionalUserPropService;
            _productService = productService;
            _companyMainCategoryService = companyMainCategoryService;
            _representationCategoryService = representationCategoryService;
            _appConfigService = appConfigService;
            _applicationConfiguration = applicationConfiguration;
            _companyService = companyService;
            _representationService = representationService;
            _userPrincipal = userPrincipal;
        }

        public override async Task AfterSaveChangesAsync(int commitAsync)
        {
            await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync();
            await UpdateCodePathAsync();
        }

        public IQueryable<Category> GetByCompanyMainCategory(int companyId)
        {
            return _categoryRepository.GetByCompanyMainCategory(companyId);
        }

        public IQueryable<Category> GetByCompanyMainCategoryId(int companyMainCategoryId)
        {
            return _categoryRepository.GetByCompanyMainCategoryId(companyMainCategoryId);
        }

        public IQueryable<Category> GetRepresentationCourseCategories(int representationId)
        {
            return _categoryRepository.GetRepresentationCategories(representationId);
        }

        public IQueryable<Category> GetByCustomFilter(int? parentId, int? companyId, int? representationId, bool loadLevelOneNodes = false)
        {
            return _categoryRepository.GetByCustomFilter(parentId, companyId, representationId, loadLevelOneNodes);
        }

        public IQueryable<Category> GetApplicationCategories(int applicationId, int? companyId)
        {
            return _categoryRepository.GetApplicationCategories(applicationId, companyId);
        }

        public async Task<IEnumerable<CategoryCustomModel>> GetUsedCategoriesAsync(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            string categoryTitle,
            int applicationId,
            bool loadLevelOneNodes,
            int? categoryGroupId = null)
        {
            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return GetUsedCategoriesByFilters
            (parentId,
                searchTitle,
                companyId,
                representationId,
                cityId,
                categoryTitle,
                applicationId,
                loadLevelOneNodes,
                additionalUserProp.CustomerClubId,
                categoryGroupId);
        }

        public IEnumerable<CategoryCustomModel> GetUsedCategoriesByFilters(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            string categoryTitle,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId = null)
        {

            return _categoryRepository.GetUsedCategories(
                parentId,
                searchTitle,
                companyId,
                representationId,
                cityId,
                applicationId,
                loadLevelOneNodes,
                customerClubId,
                categoryTitle,
                categoryGroupId);
        }
        public IEnumerable<CategoryCustomModel> GetRootUsedCategories(
            int? parentId,
            int? companyId,
            int? representationId,
            int? cityId,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId = null)
        {
            return _categoryRepository.GetRootUsedCategories(parentId, companyId,
                representationId, cityId, applicationId, loadLevelOneNodes,
                customerClubId, categoryGroupId);
        }
        public IQueryable<Category> GetCategoryWithParents(int categoryId)
        {

            return _categoryRepository.GetCategoryWithParents(categoryId);

        }

        public string GetTitle(int id)
        {
            return _categoryRepository.GetTitle(id);
        }
        /// <summary>
        /// پایین ترین دسته محصول یک مرکز را به همراه دسته پدر آن بر می گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<LowLevelCategoryCustomModel> GetLowLevelCategoryWithParent(int companyId)
        {
            return _categoryRepository.GetLowLevelCategoryWithParent(companyId);
        }

        public IQueryable<Category> GetRepresentationMainCategories(int representationId)
        {
            return _categoryRepository.GetRepresentationMainCategories(representationId);
        }

        // public IQueryable<Category> GetByMediaCustomFilter()
        // {
        //     return _categoryRepository.GetByMediaCustomFilter();
        // }

        public bool CheckAllMainCategoriesHasUnitOfMeasurementAndCode()
        {
            return _categoryRepository.CheckAllMainCategoriesHasUnitOfMeasurementAndCode();
        }

        public Task UpdateCodePathAsync()
        {
            return _categoryRepository.UpdateCodePathAsync();
        }

        public Task<bool> IsExistCategoryForCompanyAsync(int companyId)
        {
            return _categoryRepository.GetByCompanyId(companyId).AnyAsync();
        }

        public async Task<(bool allowDelete, string message)>
            AllowDeleteCategoryAsync(int categoryId)
        {
            var categoryIsParent = await CategoryIsParentAsync(categoryId);
            if (categoryIsParent)
            {
                return (false, "کاربر گرامی، برای این رکورد زیر مجوعه تعریف شده است، امکان حذف وجود ندارد.");
            }

            var existProductForCategory = await _productService
                .ExistProductByCategoryIdAsync(categoryId);
            if (existProductForCategory)
            {
                return (false, "کاربر گرامی، برای این رکورد دوره تعریف شده است، امکان حذف وجود ندارد.");
            }
            var allowAssignCategoryForCompanyAndRepresentation =
                await _appConfigService.GetBooleanValueAsync(
                ApplicationConfigKey.SiteConfigurationAssignCategoryForCompanyAndRepresentationAfterInsert);
            // اگر تنظیم درج دسته بندی مرکز و نمایندگی به صورت اتومات نباشد کاربر
            // باید از طریق فرم های دسته بندی مرکز و نمایندگی ،ارتباط با دسته بندی را حذف کند
            // تا بتواند دسته بندی را حذف کند پس پیام عدم حذف را نمایش می دهیم
            if (!allowAssignCategoryForCompanyAndRepresentation)
            {
                bool existCompanyMainCategory = await _companyMainCategoryService
                    .CheckExistCompanyMainCategoryByCategoryIdAsync(categoryId);
                if (existCompanyMainCategory)
                {
                    return (false,
                        "کاربر گرامی، این رکورد در فرم های دیگر استفاده شده است، امکان حذف وجود ندارد.");
                }
                bool existRepresentationCategory = await _representationCategoryService
                    .CheckExistRepresentationCategoryByCategoryIdAsync(categoryId);
                if (existRepresentationCategory)
                {
                    return (false,
                        "کاربر گرامی، این رکورد فرم های دیگر استفاده شده است، امکان حذف وجود ندارد.");
                }

            }
            var existRepresentationCategoryForAdditionalUser =
                await _additionalUserPropService.CheckExistRepresentationCategoryByCategoryIdAsync(categoryId);
            if (existRepresentationCategoryForAdditionalUser)
            {
                return (false,
                    "کاربر گرامی، این رکورد به کاربران اختصاص داده شده است، امکان حذف وجود ندارد.");
            }
            return (true, string.Empty);

        }

        public IQueryable<Category> FindById(int id)
        {
            return _categoryRepository.FindById(id);
        }
        public Task<bool> IsRootCategoryAsync(int categoryId)
        {
            return FindById(categoryId)
                .AnyAsync(x => !x.Parent.ParentId.HasValue);
        }

        public Task<(IQueryable<Category> categories, int? currentUserCompanyId)> GetCurrentUserCategories()
        {
            return _categoryRepository.GetCurrentUserCategoriesAsync();
        }

        public async Task AssignToCompanyAndRepresentationAsync(int categoryId)
        {
            var allowAssignCategoryForCompanyAndRepresentation = await _appConfigService.GetBooleanValueAsync(
                ApplicationConfigKey.SiteConfigurationAssignCategoryForCompanyAndRepresentationAfterInsert
                , _applicationConfiguration.ApplicationId);
            if (allowAssignCategoryForCompanyAndRepresentation)
            {
                await AssignCategoryToAllCompaniesAsync(categoryId);
            }

        }
        /// <summary>
        /// درج دسته بندی مرکز و
        /// برای مراکز
        /// درج دسته بندی نمایندگی
        /// برای نمایندگی
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private async Task AssignCategoryToAllCompaniesAsync(int categoryId)
        {
            // در صورتی که کاربر دارای مرکز باشد
            // مرکز کاربر  بر می گرداند
            // در غیر اینصورت کل مراکز را بر می گرداند
            var companiesIds = await _companyService.GetCompanyIds();
            var categoryIsRoot = await IsRootCategoryAsync(categoryId);
            // در صورتی که دسته بندی در
            // Level one
            // باشد آن را برای مرکز درج می کنیم
            if (categoryIsRoot)
            {
                foreach (var companyId in companiesIds)
                {
                    await _companyMainCategoryService.AddAsync(new CompanyMainCategory
                    {
                        CompanyId = companyId,
                        CategoryId = categoryId,
                        ApplicationId = _userPrincipal.ApplicationId,
                        CreateBy = _userPrincipal.Id,
                        CreatorOrganizationId = _userPrincipal.OrganizationId
                    });
                }
            }

            // نمایندگی های مربوط به مراکز را بر می گرداند
            var representations =
                await _representationService.GetAll().Where(x => companiesIds.Contains(x.CompanyId)).ToListAsync();
            foreach (var representation in representations)
            {
                await _representationCategoryService.AddAsync(new RepresentationCategory
                {
                    RepresentationId = representation.Id,
                    CategoryId = categoryId,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CreateBy = _userPrincipal.Id,
                    CreatorOrganizationId = _userPrincipal.OrganizationId
                });
            }
            await SaveChangeAsync();
        }
        private Task<bool> CategoryIsParentAsync(int categoryId)
        {
            return _categoryRepository.CategoryIsParentAsync(categoryId);
        }

        /// <summary>
        /// این متد جهت جستجو در بین عنوان دسته بندی ها بر اساس کلید داده شده
        /// را انجام می دهد
        /// </summary>
        /// <param name="searchKey">کلید جستجو</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <returns></returns>
        public IEnumerable<CategoryCustomModel> SearchInCategoryTitle(string searchKey, int customerClubId)
        {
            return _categoryRepository.GetUsedCategories(null, null, null, null, null,
                _applicationConfiguration.ApplicationId,
                false, customerClubId, searchKey, null);
        }
        /// <summary>
        /// دسته بندی را از دسته های مرکز دسته بندی و های نمایندگی حذف می کنیم  
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task RemoveCategoryFromCompanyRepresentationCategoryAsync(int categoryId)
        {
            await _representationCategoryService.DeleteByCategoryIdAsync(categoryId);
            await _companyMainCategoryService.DeleteByCategoryIdAsync(categoryId);
        }
        public override async Task DeleteAsync(object id, bool saveAfterDelete = false, bool publishEvent = true, bool logical = false)
        {
            var allowAssignCategoryForCompanyAndRepresentation =
                await _appConfigService.GetBooleanValueAsync(
                    ApplicationConfigKey.SiteConfigurationAssignCategoryForCompanyAndRepresentationAfterInsert);
            //  اگر تنظیم درج دسته بندی مرکز و نمایندگی به صورت اتومات باشد 
            // با فراخوانی سرویس زیر دسته بندی را از دسته بندی مرکز و نمایندگی حذف می کنیم 
            if (allowAssignCategoryForCompanyAndRepresentation)
            {
                await RemoveCategoryFromCompanyRepresentationCategoryAsync((int)id);
            }
            await base.DeleteAsync(id, saveAfterDelete, publishEvent, logical);
        }

        /// <summary>
        /// دریافت
        /// codePath
        /// مربوط به یک Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<string> GetProductCategoryCodePathAsync(int productId)
        {
            return _productService.GetProductMainCategoryView()
                .Join(GetAll(true),
                    pmcv => pmcv.CategoryId,
                    category => category.Id,
                    (productMainCategoryView, category) =>
                        new
                        {
                            category,
                            productMainCategoryView

                        })
                .Where(x =>
                    x.productMainCategoryView.ProductId == productId)
                .Select(s => s.category.CodePath)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Category> GetCategoryTreeByCodePath(string codePath)
            => _categoryRepository.GetCategoryTreeByCodePath(codePath);


        /// <summary>
        /// دریافت برترین دسته بندی ها 
        /// براساس تعداد دوره و ترتیب نمایش
        /// </summary>
        /// <param name="topCount">تعداد آیتم های قابل نمایش</param>
        /// <returns></returns>
        public async Task<List<TopCategoryDto>> GetTopCategoriesAsync(int topCount = 5)
        {
            return await _categoryRepository.GetAllByApplicationId(13)
                .Select(x => new TopCategoryDto
                {
                    DisplayOrder = x.DisplayOrder,
                    Title = x.Title,
                    Count = x.ProductCategory.Count()
                })
                 .OrderBy(x => x.DisplayOrder)
                 .ThenByDescending(x => x.Count)
                 .Take(topCount).ToListAsync();
        }

    }
}
