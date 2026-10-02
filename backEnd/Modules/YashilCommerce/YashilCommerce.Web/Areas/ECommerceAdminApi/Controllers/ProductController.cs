using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;
using YashilUserManagement.Core.Dto;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductController : BaseController<Product, int, ProductListViewModel, ProductEditModel,
        ProductSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IOrderItemService _orderItemService;
        private readonly IYashilLogger<Product> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IRepresentationService _representationService;
        private readonly IAppConfigService _appConfigService;
        private readonly IActivityLogService _activityLogService;
        private readonly IStaticCacheManager _staticCacheManager;


        public ProductController(IProductSalesPlanningService productSalesPlanningService,
            IProductService productService, IMapper mapper, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration,
            StoreConfiguration storeConfiguration,
            IYashilLogger<Product> logger,
            IPermissionService permissionService,
            IOrderItemService orderItemService,
            IProductCategoryService productCategoryService,
            IAppDocumentService appDocumentService,
            IRepresentationService representationService,
            IAppConfigService appConfigService,
            IActivityLogService activityLogService,
            IStaticCacheManager staticCacheManager) : base(
            productService,
            mapper, logger, permissionService)
        {
            _mapper = mapper;
            _userPrincipal = userPrincipal;
            _logger = logger;
            _productCategoryService = productCategoryService;
            _appDocumentService = appDocumentService;
            _representationService = representationService;
            _appConfigService = appConfigService;
            _productService = productService;
            _productSalesPlanningService = productSalesPlanningService;
            _orderItemService = orderItemService;
            _applicationConfiguration = applicationConfiguration;
            _storeConfiguration = storeConfiguration;
            _activityLogService = activityLogService;
            _staticCacheManager = staticCacheManager;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, string title,
            string code, int? categoryId, int? companyId, int? appEntityId, int? productTemplateId, int? giftCardTypeId,
            int? downloadActivationType, int? recurringCyclePeriodType, int? taxCategoryId, int? basePriceUnitType,
            int? basePriceBaseUnitType, bool? showArchives)
        {
            await CheckUserHasAccessForCompanyAsync(companyId);

            var products = _productService.GetByCustomFilter(title, code, categoryId, companyId, appEntityId,
                productTemplateId, giftCardTypeId, downloadActivationType, recurringCyclePeriodType, taxCategoryId,
                basePriceUnitType, basePriceBaseUnitType, showArchives);

            return await DataSourceLoader.LoadAsync(
                products.ProjectTo<ProductListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این api
        /// جهت دریافت لیست دور های مربوط به یک مرکز کاربرد دارد
        /// مدل بازگشتی نام دوره و شناسه دوره را بازمیگرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        [HttpGet("GetProductList")]
        [YashilAuthorization("GetByCustomFilterForList")]
        public async Task<List<ProductSimpleViewModel>> GetProductListAsync(int companyId)
        {
            await CheckUserHasAccessForCompanyAsync(companyId);

            var productQuery = _productService.GetByCustomFilter(null, null, null, companyId, null,
                null, null, null, null, null,
                null, null, false);

            return await productQuery.ProjectTo<ProductSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }



        protected override async Task CustomMapBeforeInsertAsync(ProductEditModel editModel, Product entity)
        {
            await _productService.CustomMapBeforeInsertAsync(entity, editModel.Categories, editModel.TaxCategoryId,
                editModel.IsTaxExempt, editModel.SalesModelType, editModel.MinPrice, editModel.MaxPrice,
                editModel.AppEntityId);
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        /// <summary>
        /// set sales planing model in inserting new simple or grouped product
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        // private void SetDependentProductSalePlanning(ProductEditModel editModel, Product entity)
        // {
        //     // check if the sales model for sale in all representation is selected or not
        //     if (editModel.SalesModelType == (int)SalesModelTypes.SalesAtAllRepresentations)
        //     {
        //         // if min and max product are equal the sales planing price must be equal to them
        //         // otherwise it should be 0 and the representation will edit it
        //         var price = editModel.MaxPrice == editModel.MinPrice ? editModel.MaxPrice : 0;
        //         // create new sales planing instance
        //         var productSalesPlanning = _productService.CreateProductSalesPlanningObject(null, Convert.ToInt32(price));
        //         // add sales planning instance to te company
        //         entity.ProductSalesPlanning.Add(productSalesPlanning);
        //         // if the product is group all product in the group should have just one sales planing
        //         // check the product in group has sales planing or not
        //         // if not add a sales planing to them
        //     }
        // }

        // ProductSalesPlanning CreateProductSalesPlanningObject(int? productId, int price)
        // {
        //     var psp = new ProductSalesPlanning
        //     {
        //         CreateBy = _userPrincipal.Id,
        //         CreatorOrganizationId = _userPrincipal.OrganizationId,
        //         CreationDate = DateTime.Now,
        //         ApplicationId = _userPrincipal.ApplicationId,
        //         Price = Convert.ToInt32(price),
        //         RunType = (int)RunTypeEnum.Class
        //     };
        //     if (productId.HasValue)
        //     {
        //         psp.ProductId = productId.Value;
        //     }
        //
        //     return psp;
        // }

        protected override async Task CustomMapBeforeUpdateAsync(ProductEditModel editModel, Product entity)
        {
            await _productService.SetEntityDefaultPropsAsync(entity, editModel.TaxCategoryId, editModel.IsTaxExempt);

            UpdateProductCategories(editModel, entity);
            if (entity.Course != null)
            {
                entity.Course.ModifyBy = _userPrincipal.Id;
                entity.Course.ModificationDate = DateTime.Now;
            }

            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        /// <summary>
        /// هنگام بروزرسانی محصول باید دسته بندی های قبلی حذف شدند باید دسته بندی های جدید اضافه گردند
        /// برای اینکار از این متد استفاده میشود
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        private void UpdateProductCategories(ProductEditModel editModel, Product entity)
        {
            _productService.SetProductDefaultCategory(entity, editModel.Categories);

            var productCategories = new List<ProductCategory>();
            foreach (var categoryId in editModel.Categories)
            {
                productCategories.Add(new ProductCategory
                {
                    CreateBy = _userPrincipal.Id,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CategoryId = categoryId,
                    ProductId = entity.Id
                });
            }

            _productCategoryService.Update(productCategories, entity.Id);
        }

        [HttpGet("GetRepresentationProductsByCategoryId")]
        public async Task<LoadResult> GetRepresentationProductsByCategoryId(CustomDataSourceLoadOptions loadOptions,
            int? companyId, int? representationId, int? categoryId, bool hierarchical = true)
        {
            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);
            var products =
                _productService.GetRepresentationProductsByCategoryId(companyId, representationId, categoryId,
                    hierarchical);
            return await DataSourceLoader.LoadAsync(
                products.ProjectTo<ProductWithCourseSimpleListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(ProductEditModel editModel)
        {
            var product = await _productService.GetAsync(editModel.Id, true);


            await CheckUserHasAccessForCompanyAsync(editModel.CompanyId);

            if (product.CompanyId != editModel.CompanyId)
            {
                var hasAnyProductSalesPlanning =
                    await _productSalesPlanningService.CheckProductHasAnyProductSalesPlanningAsync(editModel.Id);
                if (hasAnyProductSalesPlanning)
                {
                    return
                        "کاربر گرامی با توجه به اینکه برای این محصول برنامه ریزی فروش وجود دارد نمی توانید مرکز آن را تغییر دهید.";
                }
            }

            var isProductInOrderItem = _orderItemService.IsProductInOrderItems(product.Id);
            if (isProductInOrderItem && product.Title != editModel.Title)
            {
                return "کاربر گرامی این محصول خریداری شده است، شما مجاز به تغییر نام این محصول نمی باشید.";
            }

            // اگر کد محصولات اپلیکیشن ضروری باشد و کد محصول پر شده توسط کاربر مقدار نداشته باشد
            if (_storeConfiguration.RequireProductCode && string.IsNullOrEmpty(editModel.Code))
            {
                return "کاربر گرامی کد محصول باید مقدار داشته باشد.";
            }

            // اگر کد بسته مقدار داشت و مقدار آن تغییر کرده بود تکراری بودن آن را بررسی می کند
            if (!string.IsNullOrEmpty(editModel.Code) && product.Code != editModel.Code)
            {
                // بررسی می کند که کد محصول وارد شده تکراری است یا خیر 
                var isProductCodeDuplicate =
                    _productService.IsProductCodeDuplicate(editModel.Code, _applicationConfiguration.ApplicationId);
                if (isProductCodeDuplicate)
                {
                    return "کاربر گرامی کد محصول وارد شده تکراری می باشد، لطفا کد محصول را تغییر دهید.";
                }
            }

            //کد دسته بندی گروه اشتغال
            var employmentCategoryGroupId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCategoryGroupEmploymentCategoryGroupId);
            //کد دسته بندی گروه سایرین
            var skillCategoryGroupId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCategoryGroupSkillCategoryGroupId);

            if (editModel.CategoryGroupId != employmentCategoryGroupId &&
                editModel.CategoryGroupId != skillCategoryGroupId)
                editModel.ProductType = (int)ProductTypeEnum.Other;

            var isLmsModeActive =
                await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode);

            if (!TryValidProductGroupValue(editModel.ProductGroupId, editModel.ProductGroupIdIncludeTax, isLmsModeActive, out string message))
                return message;

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(ProductEditModel editModel)
        {
            var result = _productService.ValidateProductCodeBeforeInsert(editModel.Code);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            //کد دسته بندی گروه اشتغال
            var employmentCategoryGroupId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCategoryGroupEmploymentCategoryGroupId);
            //کد دسته بندی گروه سایرین
            var skillCategoryGroupId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCategoryGroupSkillCategoryGroupId);

            if (editModel.CategoryGroupId != employmentCategoryGroupId &&
                editModel.CategoryGroupId != skillCategoryGroupId)
                editModel.ProductType = (int)ProductTypeEnum.Other;

            var isLmsModeActive =
                await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode);


            if (!TryValidProductGroupValue(editModel.ProductGroupId, editModel.ProductGroupIdIncludeTax, isLmsModeActive, out string message))
                return message;


            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        /// <summary>
        /// جهت دریافت لیست فایل های بارگذاری شده  صفحات توضیحات محصولات
        /// در منو بارگذاری صفحات توضیحات محصول استفاده میشود
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedProductDetailPagesFile")]
        [YashilAuthorization(ActionName = "UploadedProductDetailPages")]
        public async Task<LoadResult> GetUploadedProductDetailPagesFileAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var transactionFiles = _appDocumentService.GetObjectDocumentsByUserId(
                Enum.GetName(AppEntityEnum.Product), _userPrincipal.Id,
                (int)DocumentTypeEnum.UploadProductDetailPages);

            return await DataSourceLoader.LoadAsync(
                transactionFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// جهت بارگذاری صفحات html
        /// توضیحات محصول
        /// در منو بارگذاری صفحات توضیحات محصول استفاده میشود
        /// </summary>
        /// <param name="productDetailPagesFile"></param>
        /// <returns></returns>
        [HttpPost("UploadedProductDetailPagesFile")]
        [YashilAuthorization(ActionName = "UploadedProductDetailPages")]
        public async Task<ResultStatus<int>> UploadedProductDetailPagesFile(IFormFile productDetailPagesFile)
        {
            var res = await _productService.SaveProductDetailPagesFileAndExtractToPath(productDetailPagesFile);
            if (string.IsNullOrEmpty(res))
            {
                return new ResultStatus<int>(true, "کاربر گرامی بارگذاری با موفقیت انجام شد");
            }

            return new ResultStatus<int>(false, res);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var product = await _productService.GetAsync(id, true);
            await CheckUserHasAccessForCompanyAsync(product.CompanyId);
            var allowedToDeleteProduct = await _productService.IsAllowedDeleteProductAsync(id);
            if (!allowedToDeleteProduct.isAllowed)
            {
                return allowedToDeleteProduct.message;
            }

            return await base.CustomValidateBeforeDeleteAsync(id);
        }

        /// <summary>
        ///  نمایش اطلاعات دوره انتخاب شده برای دانشجوی مورد نظر  
        ///  این متد در کامپوننت product-sales-planning-info
        ///  استفاده میشود
        ///  اطلاعاتی که به نمایش گذاشته میشود عبارت اند از نام مرکز نام نمایندگی و مدت دوره ، زمان تشکیل کلاس ، تاریخ شروع ، تاریخ پایان ، مدرس ، نوع برگزاری ، روش اجرای دوره ،جنسیت ، عنوان تنظیم فروش و نام محصول میباشد
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductInfo")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<ProductInfoViewModel> GetProductInfo(int productId)
        {


            var productInfo = _productService.FindById(productId);

            var res = await productInfo.ProjectTo<ProductInfoViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (res != null)
            {
                bool userHasAccessForProduct =
                    await _representationService.CheckUserAccessForCompanyAsync(res.CompanyId);
                if (!userHasAccessForProduct)
                {
                    _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                        nameof(Factor), "Get Error", null);
                    throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(OrderItem)}");
                }
            }

            return res;
        }

        [NonAction]
        private async Task CheckUserHasAccessForCompanyAsync(int? companyId)
        {
            bool userHasAccessForCompany =
                await _representationService.CheckUserAccessForCompanyAsync(companyId, _userPrincipal.Id);

            if (!userHasAccessForCompany)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(Factor), "Get Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }
        }

        [NonAction]
        private async Task CheckUserHasAccessForCompanyAndRepresentation(int? companyId, int? representationId)
        {
            bool userHasAccessForCompanyAndRepresentation =
                await _representationService.CheckUserAccessForCompanyAndRepresentationAsync(companyId,
                    representationId, _userPrincipal.Id);

            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(Factor), "Get Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }
        }

        [HttpPost("ArchiveProduct")]
        [YashilAuthorization("ArchiveEntity")]
        public async Task<ResultStatus<int>> ArchiveProductAsync(ProductSimpleEditModel psp)
        {
            string successMessage = "دوره انتخاب شده با موفقیت آرشیو شد";
            string errorMessage = "متاسفانه خطایی در هنگام پردازش رخ داده است. دوباره سعی کنید";
            try
            {
                var result = await _productService.Archive(psp.Id);
                if (result == -1)
                {
                    return new ResultStatus<int>(false, errorMessage);
                }

                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "آرشیو دوره",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = nameof(ArchiveProductAsync),
                    ObjectId = psp.Id,
                    PreviousRecordContent = JsonConvert.SerializeObject(result),
                    ResourceTitle = nameof(result),
                    UserId = _userPrincipal.Id
                });

                RemoveCategoryCache();

                return new ResultStatus<int>(true, successMessage);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "بروز خطا به هنگام آرشیو دوره",
                    e);
                return new ResultStatus<int>(false, errorMessage);
            }
        }



        [HttpPost("RestoreFromArchiveProduct")]
        [YashilAuthorization("ArchiveEntity")]
        public async Task<ResultStatus<int>> RestoreFromArchiveProductAsync(ProductSimpleEditModel psp)
        {
            string successMessage = "دوره انتخاب شده با موفقیت از آرشیو خارج شد";
            string errorMessage = "متاسفانه خطایی در هنگام پردازش رخ داده است. دوباره سعی کنید";
            try
            {
                var result = await _productService.RestoreFromArchiveAsync(psp.Id);
                if (result == -1)
                {
                    return new ResultStatus<int>(false, errorMessage);
                }

                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "آرشیو دوره",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = nameof(RestoreFromArchiveProductAsync),
                    ObjectId = psp.Id,
                    PreviousRecordContent = JsonConvert.SerializeObject(result),
                    ResourceTitle = nameof(result),
                    UserId = _userPrincipal.Id
                });

                RemoveCategoryCache();

                return new ResultStatus<int>(true, successMessage);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "بروز خطا به هنگام آرشیو دوره",
                    e);
                return new ResultStatus<int>(false, errorMessage);
            }
        }

        /// <summary>
        /// پاک کردن کش مربوط به دوره ها و دسته بندی در صفحه هوم
        /// </summary>
        private async void RemoveCategoryCache()
        {
            await _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<ProductSalesPlanningStudent>
                                        .GetLandingHomePagePublicProductSalesPlanningKey);
            await _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<Category>.GetAllByCompanyAndCity,
                null, null, null, null, null);
        }

        /// <summary>
        /// بررسی می کند که آیا
        /// ProductGroup
        /// دارای مقدار معتبر است یا نه
        /// </summary>
        /// <param name="productGroupId"></param>
        /// <param name="productGroupIdIncludeTax"></param>
        /// <param name="isLmsModeActive"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        private bool TryValidProductGroupValue(int? productGroupId, int? productGroupIdIncludeTax, bool isLmsModeActive, out string message)
        {
            message = "کاربر گرامی مقادیر شناسه گروه محصول و گروه محصول شامل مالیات باید دارای مقدار باشد.";
            return (isLmsModeActive || (productGroupId.HasValue && productGroupId.Value != 0 &&
                                        productGroupIdIncludeTax.HasValue && productGroupIdIncludeTax.Value != 0));
        }

        /// <summary>
        /// دریافت تعداد دوره های تعریف شده در سامانه
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductCount")]
        [YashilAuthorization("GetProductCount")]
        public async Task<IActionResult> GetProductCountAsync()
        {

            return Ok(new
            {
                ProductCount = await _productService.GetProductCountBasedOnCurrentUserCompanyAndRepresentationAsync()
            });
        }
    }
}