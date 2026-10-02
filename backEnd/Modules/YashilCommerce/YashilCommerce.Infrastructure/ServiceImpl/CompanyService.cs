using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using YashilUserManagement.Core.Services;


namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CompanyService : GenericService<Company, int>, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IAppConfigService _appConfigService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IProductService _productService;
        private readonly IRepresentationService _representationService;
        private readonly ICompanyMainCategoryService _companyMainCategoryService;
        private readonly IVocherTypeService _vocherTypeService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IAppDocumentService _appDocumentService;


        public CompanyService(IUnitOfWork unitOfWork, ICompanyRepository companyRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher, IAdditionalUserPropService additionalUserPropService, IAppConfigService appConfigService,
            ISaleConfigurationService saleConfigurationService, IProductService productService,
            IRepresentationService representationService,
            ICompanyMainCategoryService companyMainCategoryService, IVocherTypeService vocherTypeService, ApplicationConfiguration applicationConfiguration, ICategoryRepository categoryRepository, IStaticCacheManager staticCacheManager, IProductSalesPlanningService productSalesPlanningService, IAppDocumentService appDocumentService)
            : base(unitOfWork, companyRepository, userPrincipal, eventPublisher)
        {
            _companyRepository = companyRepository;
            _userPrincipal = userPrincipal;
            _appConfigService = appConfigService;
            _saleConfigurationService = saleConfigurationService;
            _productService = productService;
            _representationService = representationService;
            _companyMainCategoryService = companyMainCategoryService;
            _vocherTypeService = vocherTypeService;
            _additionalUserPropService = additionalUserPropService;
            _applicationConfiguration = applicationConfiguration;
            _categoryRepository = categoryRepository;
            _staticCacheManager = staticCacheManager;
            _productSalesPlanningService = productSalesPlanningService;
            _appDocumentService = appDocumentService;
        }

        public async Task<IQueryable<Company>> GetApplicationCompaniesAsync(
            string searchTitle,
            int applicationId,
            int customerClubId,
            int? cityId,
            int? categoryId,
            int? categoryGroupId,
            bool orderByCompanyDisplayOrder = true)
        {
            var isEmploymentCategoryGroup = false;
            if (categoryGroupId.HasValue)
            {
                isEmploymentCategoryGroup = await CheckIsEmploymentCategoryGroupAsync(categoryGroupId.Value);
            }

            return _companyRepository.GetApplicationCompanies(
                searchTitle,
                applicationId,
                customerClubId,
                cityId,
                categoryId,
                categoryGroupId,
                isEmploymentCategoryGroup,
                orderByCompanyDisplayOrder
                );
        }

        public IQueryable<City> GetCityByCompanyId(int? companyId)
        {
            return _companyRepository.GetCityByCompanyId(companyId);
        }

        public async Task<IQueryable<City>> GetCityByFilterAsync(int? companyId, int? categoryId, int? categoryGroupId)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var isEmploymentCategoryGroup = false;
            if (categoryGroupId.HasValue)
            {
                isEmploymentCategoryGroup = await CheckIsEmploymentCategoryGroupAsync(categoryGroupId.Value);
            }
            return _companyRepository.GetCityByFilter(
                userAdditionalUserProp.CustomerClubId,
                companyId,
                categoryId,
                categoryGroupId,
                isEmploymentCategoryGroup);
        }

        /// <summary>
        /// بررسی می کند آیا گروه دسته بندی ارسالی در پارامتر
        /// گروه دسته بندی اشتغالی است یا خیر 
        /// </summary>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <returns></returns>
        private async Task<bool> CheckIsEmploymentCategoryGroupAsync(int categoryGroupId)
        {
            //گروه دسته بندی اشتغال
            var employmentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId, _applicationConfiguration.ApplicationId);
            // در صورتی که گروه دسته بندی اشتغالی باشد 
            return employmentCategoryGroupId == categoryGroupId;

        }
        public string GetTitle(int id)
        {
            return _companyRepository.GetTitle(id);
        }

        public Company GetUserCompany(int userId)
        {
            return _companyRepository.GetUserCompany(userId);
        }
        public IQueryable<Company> GetCompaniesByCityId(int? cityId)
        {
            return _companyRepository.GetCompaniesByCityId(cityId);
        }

        public IQueryable<Company> GetCompaniesHaveVoucherType(bool readOnly = true)
        {
            return _companyRepository.GetCompaniesHaveVoucherType(readOnly);
        }

        public IQueryable<Company> GetAllActiveCompanies(bool readOnly = true, int? applicationGroupId = null, int? applicationId = null)
        {
            return _companyRepository.GetAllActiveCompanies(readOnly, applicationGroupId, applicationId);
        }
        /// <summary>
        /// دریافت مرکز بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        public Task<Company> GetByProductSalePlanningAsync(int productSalePlanningId)
        {
            return _companyRepository.GetByProductSalePlanningAsync(productSalePlanningId);
        }

        /// <summary>
        /// بعد از درج کمپنی برسی می کند که آیا کانفیگ افزودن تنظیم فروش به هنگام افزودن کمپانی 
        /// true
        /// است یا نه در صورت (ترو) بودن تنظیم فروش جدید به جدول تنظیم فروش افزودن می شود
        /// </summary>
        /// <param name="t"></param>
        /// <param name="saveAfterAdd"></param>
        /// <param name="publishEvent"></param>
        /// <returns></returns>
        public override async Task<Company> AddAsync(Company t, bool saveAfterAdd = false, bool publishEvent = true)
        {
            var company = await base.AddAsync(t, saveAfterAdd, publishEvent);
            var doInsertDefaultSaleConfigurationByCreatingNewCompany = await _appConfigService.
                GetBooleanValueAsync(ApplicationConfigKey.YashilCommerceCompanyInsertDefaultSaleConfigurationByCreatingNewCompany);
            if (doInsertDefaultSaleConfigurationByCreatingNewCompany)
            {
                var saleConfiguration = new SaleConfiguration
                {
                    CompanyId = company.Id,
                    IsActive = true,
                    FromDate = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    ToDate = DateFunction.ToShamsiInt(DateTime.Now.AddYears(10).Year, DateTime.Now.AddYears(10).Month, DateTime.Now.AddYears(10).Day),
                    Title = t.Title,
                    AllowSetTaxInProduct = true,
                    IsTaxExempt = false,
                    CreateBy = _userPrincipal.Id
                };
                await _saleConfigurationService.AddAsync(saleConfiguration, true);
            }

            return company;
        }

        public override async Task DeleteAsync(object id, bool saveAfterDelete = false, bool publishEvent = true, bool logical = false)
        {
            var defaultActiveCompanySaleConfiguration =
               await GetDefaultActiveCompanySaleConfigurationAsync((int)id);

            if (defaultActiveCompanySaleConfiguration.IsExist &&
                defaultActiveCompanySaleConfiguration.saleConfiguration != null)
            {
                await _saleConfigurationService.DeleteAsync(defaultActiveCompanySaleConfiguration.saleConfiguration);
            }
            await base.DeleteAsync(id, saveAfterDelete, publishEvent, logical);
        }
        /// <summary>
        /// بررسی می کند که آیا در تنظیمات برنامه
        /// "هنگام درج مرکز تنظیم فروش پیش فرض درج شود"
        /// برابر  True
        /// می باشد
        /// و تنظیم فروش فعال برای آن مرکز وجود دارد
        /// آن تنظیم فروش را بر می گرداند
        /// در غیر اینصورت خروجی
        /// Null
        /// می شود
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        private async Task<(bool IsExist, SaleConfiguration saleConfiguration)>
            GetDefaultActiveCompanySaleConfigurationAsync(int companyId)
        {
            // آیا در تنظیمات برنامه تنظیم "هنگام درج مرکز تنظیم فروش پیش فرض درج شود" مقدار
            // True دارد
            var doInsertDefaultSaleConfigurationByCreatingNewCompany = await _appConfigService.
                GetBooleanValueAsync(ApplicationConfigKey.
                    YashilCommerceCompanyInsertDefaultSaleConfigurationByCreatingNewCompany);

            // تمظیم فروش  فعال مرکز را بر می گرداند 
            var saleConfiguration = await _saleConfigurationService
                .GetActiveSaleConfigurationAsync(companyId, null);

            return (doInsertDefaultSaleConfigurationByCreatingNewCompany, saleConfiguration);
        }

        public async Task<(bool isAllowed, string message)> IsAllowedDeleteCompanyAsync(int companyId)
        {
            // آیا محصولی برای مرکز تعریف شده است 
            var isExistProduct = await _productService.IsExistProductForCompanyAsync(companyId);
            if (isExistProduct)
            {
                return (false, "کاربر گرامی، با توجه به تعریف دوره امکان حذف وجود ندارد.");
            }
            // آیا مرکز تنظیم فروش فعال دارد
            var defaultActiveCompanySaleConfiguration =
                await GetDefaultActiveCompanySaleConfigurationAsync(companyId);
            if (!defaultActiveCompanySaleConfiguration.IsExist
                && defaultActiveCompanySaleConfiguration.saleConfiguration != null)
            {
                return (false, "کاربر گرامی، تنظیم فروش فعال برای این رکورد تعریف شده و امکان حذف وجود ندارد.");
            }
            var isExistRepresentation = await _representationService.IsExistRepresentationForCompanyAsync(companyId);
            if (isExistRepresentation)
            {
                return (false, "کاربر گرامی، این رکورد در فرم های دیگر استفاده شده است و امکان حذف وجود ندارد.");
            }
            var isExistCategory = await _categoryRepository.GetByCompanyId(companyId).AnyAsync();
            //await _categoryService.IsExistCategoryForCompanyAsync(companyId);
            if (isExistCategory)
            {
                return (false, "کاربر گرامی، این رکورد در فرم های دیگر استفاده شده است و امکان حذف وجود ندارد.");
            }
            var isExistCompanyMainCategory = await _companyMainCategoryService.IsExistCompanyMainCategoryAsync(companyId);
            if (isExistCompanyMainCategory)
            {
                return (false, "کاربر گرامی، این رکورد در فرم های دیگر استفاده شده است و امکان حذف وجود ندارد.");
            }


            var isExistVoucherType = await _vocherTypeService.IsExistVoucherTypeForCompanyAsync(companyId);
            if (isExistVoucherType)
            {
                return (false, "کاربر گرامی، نوع ووچر برای این رکورد تعریف شده و امکان حذف وجود ندارد.");
            }
            var isExistAdditionalUserProp = await _additionalUserPropService.IsExistAdditionalUserPropForCompanyAsync(companyId);
            if (isExistAdditionalUserProp)
            {
                return (false, "کاربر گرامی، این رکورد به کاربری اختصاص داده شده است و امکان حذف وجود ندارد.");
            }
            return (true, string.Empty);

        }
        /// <summary>
        /// حذف بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task DeleteCompanyMainCategoryByCompanyIdAsync(int companyId)
        {
            await _companyMainCategoryService.DeleteByCompanyIdAsync(companyId);
        }

        public IQueryable<Company> FindById(int id, bool readOnly = true)
        {
            return _companyRepository.FindById(id, readOnly);
        }
        public Task UpdateCodePathAsync()
        {
            return _companyRepository.UpdateCodePathAsync();
        }

        public Task<bool> CheckCompanyRepresentationsHasProductSalesPlanning(int companyId)
        {
            var todayInt = DateFunction.TodayShamsiInt();

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Company>.
                    CompanyRepresentationsHasProductSalesPlanning,
                companyId
            );
            cacheKey.CacheTime = (int)CacheTime.Default;
            return _staticCacheManager.GetAsync(cacheKey, () =>
            {
                return _companyRepository.GetAll(true, _applicationConfiguration.ApplicationId)
                    .Where(x => x.Id == companyId)
                    .AnyAsync(x =>
                        x.Representation.Any(r =>
                            r.ProductSalesPlanning.Any(psp =>
                                psp.Confirmed.Value &&
                                psp.Published &&
                                (!psp.EndDate.HasValue || psp.EndDate >= todayInt) &&
                                psp.RepresentationId.HasValue)));
            });
        }
        /// <summary>
        /// دریافت تمامی کمپانی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Company> GetAllCompaniesByApplicationId(int applicationId, bool readOnly = false)
        {
            return _companyRepository.GetAllCompanies(applicationId, readOnly);
        }
        /// <summary>
        /// این سرویس عملیات بروزرسانی فید CanUserSeeOtherCompany مربوط به یک مرکز را انجام می دهد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="canUserOtherCompany"></param>
        /// <returns></returns>
        public async Task<Company> UpdateCanUserSeeOtherCompanyAsync(int companyId, bool canUserOtherCompany)
        {
            var company = await FindById(companyId, false).SingleOrDefaultAsync();

            if (company == null)
                return null;

            company.CanUserSeeOtherCompany = canUserOtherCompany;

            return await UpdateAsync(company, companyId, true);
        }

        /// <summary>
        /// دریافت مراکز به همراه فرزندان
        /// </summary>
        /// <param name="codePath"> codePath مرکز</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        public IQueryable<Company> GetCompaniesWithChildren(string codePath, int applicationId)
        {
            return _companyRepository.GetCompaniesWithChildren(codePath, applicationId);
        }

        public async Task<(IQueryable<Company> companies, string currentUserCompanyCodePath)>
            GetCurrentUserCompanyWithChildrenAsync
            (bool possibleSeeOtherCompanies)
        {

            int applicationId = _applicationConfiguration.ApplicationId;
            var currentUserCompanyCodePath = string.Empty;
            var currentUserCompanyInfo = await GetCurrentUserCompanyInfo();
            if (!string.IsNullOrEmpty(currentUserCompanyInfo.CodePath))
            {
                currentUserCompanyCodePath =
                    currentUserCompanyInfo.CanUserSeeOtherCompany && possibleSeeOtherCompanies
                        ? null
                        : currentUserCompanyInfo.CodePath;
            }
            var companies = GetCompaniesWithChildren(currentUserCompanyCodePath, applicationId);
            return (companies, currentUserCompanyCodePath);
        }
        /// <summary>
        /// دریافت اطلاعات مرکز کاربر جاری
        /// </summary>
        /// <returns></returns>
        public async Task<(string CodePath, bool CanUserSeeOtherCompany)> GetCurrentUserCompanyInfo()
        {
            var currentUserCompanyId = await _additionalUserPropService
                .FindByUserId(_userPrincipal.Id)
                .Select(x => x.CompanyId)
                .FirstOrDefaultAsync();
            if (currentUserCompanyId.HasValue)
            {
                var currentUserCompanyInfo = await FindById(currentUserCompanyId.Value)
                    .Select(x => new { x.CodePath, x.CanUserSeeOtherCompany })
                    .FirstOrDefaultAsync();
                if (currentUserCompanyInfo != null)
                    return new(currentUserCompanyInfo.CodePath, currentUserCompanyInfo.CanUserSeeOtherCompany);
            }
            return new(string.Empty, false);
        }
        public Task<bool> CheckCompanyParentAsync(int parentId, string currentUserCompanyCodePath)
        {
            var parentCompany = _companyRepository.FindParentCompany(parentId);
            if (!string.IsNullOrEmpty(currentUserCompanyCodePath))
            {
                return parentCompany.AnyAsync(x => EF.Functions.Like(x.CodePath, $"%{currentUserCompanyCodePath}%"));
            }
            return parentCompany.AnyAsync();
        }

        public async Task AddCategoriesForCompanyAsync(int companyId)
        {
            var allowAssignCategoryForCompanyAndRepresentation = await _appConfigService.GetBooleanValueAsync(
                ApplicationConfigKey.SiteConfigurationAssignCategoryForCompanyAndRepresentationAfterInsert
                , _applicationConfiguration.ApplicationId);
            if (allowAssignCategoryForCompanyAndRepresentation)
            {
                // دریافت دسته بندی های Level One
                var categories =
                    _categoryRepository.GetAll().Where(x =>
                        !x.Parent.ParentId.HasValue && x.ApplicationId == _userPrincipal.ApplicationId);
                foreach (var category in categories)
                {
                    await _companyMainCategoryService.AddAsync(new CompanyMainCategory
                    {
                        CompanyId = companyId,
                        CategoryId = category.Id,
                        ApplicationId = _userPrincipal.ApplicationId,
                        CreateBy = _userPrincipal.Id,
                        CreatorOrganizationId = _userPrincipal.OrganizationId
                    });
                }

                await _companyMainCategoryService.SaveChangeAsync();
            }

        }

        public async Task<List<int>> GetCompanyIds()
        {
            return await _companyRepository.GetAll()
                .Select(x => x.Id)
                 .ToListAsync();
        }

        /// <summary>
        /// این متد عملیات حستجو براساس کلید در بین عنوان مراکز را انجام می دهد
        /// نکته مرکز باید جزو مراکز فعال باشد
        /// </summary>
        /// <param name="searchKey">کلید جستجو</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        public async Task<IQueryable<CompanySearchDto>> SearchInCompanyTitleAsync(string searchKey, int customerClubId)
        {
            return (await GetApplicationCompaniesAsync(null, _applicationConfiguration.ApplicationId,
                    customerClubId, null, null, null))
                  .OrderBy(c => c.CreationDate)
                  .Where(company => company.Title.Contains(searchKey))
                  .Select(company => new CompanySearchDto
                  {
                      Title = company.Title,
                      Id = company.Id
                  }).AsNoTracking().Take(5);
        }

        /// <summary>
        /// این متد برای دریافت اطلاعات یک مرکز شامل این که
        /// آیا برنامه ریزی دوره فعال دارد یا نه
        /// آیا گزینه نمایش در صفحه اصلی مرکز فعال است
        /// آیا تصویر حالت موبایل و دسکتاپ دارد یا نه
        /// آیا قرارداد فعال دارد یا نه
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <returns>CompanyInformationDto</returns>
        public async Task<CompanyInformationDto> GetCompanyInformation(int companyId, int customerClubId)
        {
            var companyInformation = await _companyRepository.GetAll(true, _applicationConfiguration.ApplicationId)
                .Where(p => p.Id == companyId)
                 .Select(x => new CompanyInformationDto
                 {
                     HasActiveProductSalesPlanning = x.Product.Any(p => p.ProductSalesPlanning.AsQueryable()
                         .Any(_productSalesPlanningService
                             .GetProductSalePlanningIsActiveFilter(_applicationConfiguration.ApplicationId,
                                 customerClubId, null, false))),

                     ShowInMainPage = x.ShowInMainPage,

                     HasActiveContract = ((x.SaleConfiguration.Any(s => s.IsActive) || x.Representation.
                             Any(r => r.SaleConfiguration.Any(rs => rs.IsActive)) &&
                             x.Confirmed && x.IsActive))

                 }).FirstOrDefaultAsync();

            var companiesWithDesktopAndMobilePicture = await GetActiveCompaniesIfHasPictureAsync(customerClubId,
                _companyRepository.GetAll(true, _applicationConfiguration.ApplicationId)
                    .Where(c => c.Id == companyId));

            if (companyInformation == null)
                return null;

            companyInformation.HasDesktopAndMobileModeImage = companiesWithDesktopAndMobilePicture
                .Any(company => company.Id == companyId);

            return companyInformation;
        }

        /// <summary>
        /// دریافت اطلاعات مراکز فعالی که دارای تصویر برای حالت موبایل و دسکتاپ باشند
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanySimpleDto>> GetActiveCompaniesIfHasPictureAsync()
        {
            var additionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Company>.ActiveCompaniesIfHasPictureKey, additionalUserProp.CustomerClubId);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
             await GetActiveCompaniesIfHasPictureAsync(additionalUserProp.CustomerClubId));
        }

        /// <summary>
        /// ابن متد مراکز فعالی را که دارای تصویر برای حالت دسکتاپ و موبایل باشند را بر میگرداند
        /// </summary>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<List<CompanySimpleDto>> GetActiveCompaniesIfHasPictureAsync(int customerClubId, IQueryable<Company> query = null)
        {
            var activeCompanyQuery = query ?? await GetApplicationCompaniesAsync
                (null, _applicationConfiguration.ApplicationId, customerClubId, null, null, null, false);

            return await activeCompanyQuery.Join(
                    _appDocumentService.GetAllByApplicationId(true, _applicationConfiguration.ApplicationId),
                    company => company.Id,
                    appDoc => appDoc.ObjectId,
                    (company, appDoc) => new { company, appDoc }
                ).Where(joined => joined.appDoc.DocTypeId == (int)DocumentTypeEnum.CompanyMobileLogo ||
                                  joined.appDoc.DocTypeId == (int)DocumentTypeEnum.CompanyDesktopLogo)
                .GroupBy(joined => new
                {
                    joined.company.Id,
                    joined.company.ModificationDate,
                    joined.company.ShortName,
                    joined.company.Title,
                    joined.company.DisplayOrder
                }).Where(group => group.Select(c => c.appDoc.DocTypeId).Count() == 2)
                .Select(x => new CompanySimpleDto
                {
                    Id = x.Key.Id,
                    ModificationDate = x.Key.ModificationDate.Value.ToString("yyyyMMddhhmmss"),
                    ShortName = x.Key.ShortName,
                    Title = x.Key.Title,
                    DisplayOrder = x.Key.DisplayOrder
                }).OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر به شناسه مرکز ارسال شده
        /// دسترسی دارد یا نه ؟
        /// </summary>
        /// <param name="sentCompanyId">شناسه مرکز ارسال شده توسط کاربر</param>
        /// <returns>bool</returns>
        public async Task<bool> HasCurrentUserAccessToSentCompanyIdAsync(int sentCompanyId)
        {

            var userCompanyInfo = await _additionalUserPropService
                .GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId)
                .Select(aup => new
                {
                    CompanyId = aup.CompanyId,
                    CompanyCodePath = aup.Company.CodePath

                }).FirstOrDefaultAsync();

            if (userCompanyInfo == null)
                return false;

            if (!userCompanyInfo.CompanyId.HasValue)
                return true;

            if(sentCompanyId == userCompanyInfo.CompanyId.Value) 
                return true;

            if(userCompanyInfo.CompanyCodePath.Contains(sentCompanyId.ToString()))
                return true;

            return false;
        }
    }
}

