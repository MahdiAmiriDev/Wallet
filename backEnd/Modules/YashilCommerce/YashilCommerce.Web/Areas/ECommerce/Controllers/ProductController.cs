using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Core.Services;
using System.Collections.Generic;
using Yashil.Infrastructure.Caching;
using Yashil.Core.CustomViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Common.Core.Classes;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Interfaces;
using YashilUserManagement.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using System.Linq.Dynamic.Core;
using System.Linq;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class ProductController : YashilMvcBaseController<object>
    {
        // GET: /<controller>/
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IMapper _mapper;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ICompanyService _companyService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAddressService _addressService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IProductTagsService _productTagsService;
        private readonly ICategoryService _categoryService;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ProductController(ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<object> logger, IPermissionService permissionService,
            IProductSalesPlanningService productSalesPlanningService, ICompanyService companyService,
            IStaticCacheManager staticCacheManager, StoreConfiguration storeConfiguration,
            IAddressService addressService, IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal, IProductTagsService productTagsService, ICategoryService categoryService)
            : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _companyService = companyService;
            _staticCacheManager = staticCacheManager;
            _storeConfiguration = storeConfiguration;
            _addressService = addressService;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _productTagsService = productTagsService;
            _categoryService = categoryService;
        }
        /// <summary>
        /// Get productSalesPlanning  By id and View Product Detail
        /// </summary>
        /// <param name="pSpId">شناسه پلن فروش محصول</param>
        /// <returns></returns>
        public async Task<ActionResult> Index(int? pSpId)
        {
            if (pSpId == null || pSpId <= 0)
                return View("Error");

            var productSalesPlanning = _productSalesPlanningService.FindById(pSpId.Value);

            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var customerClubData = new Dictionary<string, object> { { "customerClubId", userAdditionalUserProp.CustomerClubId } };
            var productSalePlanningPresentationViewModel =
                await productSalesPlanning.ProjectTo<ProductSalePlanningPresentationViewModel>
                    (_mapper.ConfigurationProvider, customerClubData).FirstOrDefaultAsync();

            if (productSalePlanningPresentationViewModel != null)
            {
                productSalePlanningPresentationViewModel.IsResumeFillOutEnough =
                    userAdditionalUserProp.IsResumeFillOutEnough;

                productSalePlanningPresentationViewModel.FullAddress =
                    _addressService
                        .GetFullAddress(productSalePlanningPresentationViewModel.FullAddress, productSalePlanningPresentationViewModel.IsEncryptedAddress);

                (decimal discountPercentage, decimal? CalcedPrice) calcPriceInformation =
                    await _productSalesPlanningService.GetCalcPriceInformationAsync(
                    productSalePlanningPresentationViewModel.Price,
                    productSalePlanningPresentationViewModel.DiscountPercentage,
                    productSalePlanningPresentationViewModel.SaleConfigurationId,
                    userAdditionalUserProp.CustomerClubId);


                productSalePlanningPresentationViewModel.CalcedPrice = calcPriceInformation.CalcedPrice;
                productSalePlanningPresentationViewModel.DiscountPercentage = calcPriceInformation.discountPercentage;



                var cityViewModel = new List<CityTitleOnlyViewModel>();

                //در صورت نداشتن نمایندگی باید شهر نمایش داده شود و نمایندگی انخاب گردد
                if (!productSalePlanningPresentationViewModel.RepresentationId.HasValue)
                {
                    var companyId = productSalePlanningPresentationViewModel.CompanyId;

                    var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<City>.GetCityByCompanyIdCacheKey, companyId);
                    cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;
                    cityViewModel = await _staticCacheManager.GetAsync(cacheKey, () =>
                    {
                        var cityByCompany = _companyService.GetCityByCompanyId(companyId);
                        return cityByCompany.ProjectTo<CityTitleOnlyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                    });
                }

                var isProductSalesPlanningActive = await _productSalesPlanningService
                    .CheckExistsProductSalePlanningInApplicationAsync(pSpId.Value, _applicationConfiguration.ApplicationId, true);

                var productPresentationViewModel = new ProductPresentationViewModel()
                {

                    ProductSalePlanningPresentationViewModel = productSalePlanningPresentationViewModel,
                    CityListModelViewModel = cityViewModel,
                    CourseBased = _storeConfiguration.CourseBased,
                    SubsidyBased = userAdditionalUserProp.SupportSubsidy
                                   && productSalePlanningPresentationViewModel.MaxSataSubsidy.HasValue
                                   && productSalePlanningPresentationViewModel.MaxSataSubsidy > 0,
                    EmploymentSubsidyDto = await _productSalesPlanningService.CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(pSpId.Value)
                    ,
                    RelatedTagIds = await _productTagsService.GetByProductSalesPlanningId(pSpId.Value)
                    .Select(x => x.TagType).ToListAsync(),

                    ProductQuickPromoDetail = await _categoryService.GetCategoryWithParents(productSalePlanningPresentationViewModel.CategoryId)
                        .Where(x => x.ParentId != null && x.Parent.ParentId == null)
                        .Select(c => c.ProductQuickPromoDetail)
                        .FirstOrDefaultAsync()
                        ,
                    IsProductSalesPlanningActive = isProductSalesPlanningActive
                };
                return View(productPresentationViewModel);
            }

            return View("Error");
        }

        public ActionResult CategoryProducts(int? categoryId, int? company, int? cityId, int? representationId, int? pageIndex, int? categoryGroupId)
        {
            List<int> companies = new List<int>();
            if (company.HasValue)
            {
                companies.Add(company.Value);
            }
            return View(new ProductFilter
            {
                CategoryId = categoryId,
                Company = companies,
                CityId = cityId,
                RepresentationId = representationId,
                PageIndex = pageIndex,
                CategoryGroupId = categoryGroupId
            });
        }
    }
}
