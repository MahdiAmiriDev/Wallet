using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Dtos;
using Yashil.Common.Infrastructure.PagedList;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;
using YashilLight.Core.Services;
using YashilUserManagement.Core.Dto;
using YashilUserManagement.Core.Services;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductSalesPlanningController : BaseController<ProductSalesPlanning, int,
        ProductSalesPlanningBaseListViewModel, ProductSalesPlanningEditModel, ProductSalesPlanningSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IProductService _productService;
        private readonly IYashilLogger<ProductSalesPlanning> _logger;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly IRepresentationService _representationService;
        private readonly ICompanyService _companyService;
        private readonly ICategoryService _categoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IOrderService _orderService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly IProductSalesPlanningStudentService _productSalesPlanningStudentService;
        private readonly IUserRoleService _userRoleService;

        public ProductSalesPlanningController(
            IProductSalesPlanningService productSalesPlanningService,
            IMapper mapper,
            IProductService productService,
            IYashilLogger<ProductSalesPlanning> logger,
            ISaleConfigurationService saleConfigurationService,
            IPermissionService permissionService,
            IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration,
            IAppConfigService appConfigService,
            IRepresentationService representationService, ICompanyService companyService,
            ICategoryService categoryService, IAdditionalUserPropService additionalUserPropService,
            IOrderService orderService, IProductSalesPlanningItemGroupService productSalesPlanningItemGroupService,
            IStaticCacheManager staticCacheManager, IAppDocumentService appDocumentService,
            IActivityLogService activityLogService, IConfiguration configuration, IProductSalesPlanningStudentService productSalesPlanningStudentService, IUserRoleService userRoleService) : base(
            productSalesPlanningService, mapper, logger,
            permissionService)
        {
            _mapper = mapper;
            _productService = productService;
            _logger = logger;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _appConfigService = appConfigService;
            _representationService = representationService;
            _companyService = companyService;
            _categoryService = categoryService;
            _additionalUserPropService = additionalUserPropService;
            _orderService = orderService;
            _staticCacheManager = staticCacheManager;
            _productSalesPlanningService = productSalesPlanningService;
            _saleConfigurationService = saleConfigurationService;
            _appDocumentService = appDocumentService;
            _activityLogService = activityLogService;
            _configuration = configuration;
            _productSalesPlanningStudentService = productSalesPlanningStudentService;
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="productId"></param>
        /// <param name="representationId"></param>
        /// <param name="manageInventoryMethodType"></param>
        /// <param name="productAvailabilityRangeType"></param>
        /// <param name="deliveryDateType"></param>
        /// <param name="published">
        /// null:در نظر نگیر
        /// 0:false
        /// 1:true
        /// 2:آنهایی که خالی است
        /// </param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? productId,
                int? representationId, int? manageInventoryMethodType, int? productAvailabilityRangeType,
                int? deliveryDateType, int? published, int? confirmed)
        {
            var productSalesPlannings = _productSalesPlanningService.GetByCustomFilter(productId, representationId,
                manageInventoryMethodType, productAvailabilityRangeType, deliveryDateType, published, confirmed);
            return await DataSourceLoader.LoadAsync(
                productSalesPlannings.ProjectTo<ProductSalesPlanningBaseListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// این متد جهت دریافت لیست برنامه ریزی های دوره مربوط به یک مرکز
        /// کاربرد دارد اگر کاربر به مرکز دسترسی نداشته باشد
        /// Exception
        /// می دهد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningList")]
        [YashilAuthorization("GetByCustomFilterForList")]
        public async Task<List<ProductSalesPlanningBaseListViewModel>> GetProductSalesPlanningListAsync(int companyId, int representationId)
        {
            if (companyId <= 0)
                throw new ArgumentOutOfRangeException(nameof(companyId));

            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);

            var isHubRepresentation = await _representationService.IsHubRepresentationAsync(representationId);

            var productSalesPlannings = await _productSalesPlanningService
                .GetByRepresentationFilterAsync(
                    _userPrincipal.ApplicationId,
                    companyId, null,
                    representationId,
                    false,
                    true,
                    null,
                    null,
                    null,
                    null,
                    isHubRepresentation);

            return await productSalesPlannings
                .ProjectTo<ProductSalesPlanningBaseListViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        protected override async Task CustomMapBeforeUpdateAsync(ProductSalesPlanningEditModel editModel,
            ProductSalesPlanning entity)
        {
            await SetDefaultPropsAsync(editModel, entity);
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        protected override async Task CustomMapBeforeInsertAsync(ProductSalesPlanningEditModel editModel,
            ProductSalesPlanning entity)
        {
            await SetDefaultPropsAsync(editModel, entity);
            //در صورتی که کد محصول نال باشد، یعنی محصول باید تعریف شود
            if (!editModel.ProductId.HasValue || editModel.ProductId == 0)
            {
                await _productService.CustomMapBeforeInsertAsync(
                    entity.Product,
                    editModel.Product.Categories,
                    editModel.Product.TaxCategoryId,
                    editModel.Product.IsTaxExempt,
                    editModel.Product.SalesModelType,
                    editModel.Product.MinPrice,
                    editModel.Product.MaxPrice,
                    editModel.Product.AppEntityId);

                // set Default Product property
                entity.Product.CreateBy = _userPrincipal.Id;
                entity.Product.CreatorOrganizationId = _userPrincipal.OrganizationId;
                entity.Product.CreationDate = DateTime.Now;
                entity.Product.ApplicationId = _userPrincipal.ApplicationId;
            }
            // اگر محصول را انتخاب کرده باشد یعنی محصول قبلا تعریف شده باشد، دیگر نیازی به تعریف محصول نیست
            else
            {
                entity.Product = null;
            }

            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        private async Task SetDefaultPropsAsync(ProductSalesPlanningEditModel editModel, ProductSalesPlanning entity)
        {


            await SetDefaultPropsAsync(editModel.StartDate, editModel.EndDate, editModel.ProductId.Value, entity);
        }

        private async Task SetDefaultPropsAsync(ProductSalesPlanningBaseEditModel editModel,
            ProductSalesPlanning entity)
        {
            await SetDefaultPropsAsync(editModel.StartDate, editModel.EndDate, editModel.ProductId.Value, entity);
        }

        private async Task SetDefaultPropsAsync(int startDate, int? endDate, int? productId,
            ProductSalesPlanning entity)
        {
            //   هست یا نه LMS بررسی اینکه سامانه در حالت
            if (!await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode))
            {

                /*
                در صورتی که کاربر دسترسی انتشار محصول را نداشته باشد، بعد از ویرایش
                این محصول به وضعیت در انتظار تایید می رود و در غیر این صورت تایید 
                می شود
                */
                if (!_productSalesPlanningService.CanCurrentUserConfirmProductSalesPlanning())
                    entity.Confirmed = null;
            }
            else
            {
                if (!_productSalesPlanningService.CanCurrentUserConfirmProductSalesPlanning())
                {
                    //  می شود تا در انتظار تایید بالادستی باشد null درصورتی که نیاز به تایید ادمین یا بالادستی بود این مقدار
                    // در غیر اینصورت تایید می شود
                    entity.Confirmed = Convert.ToBoolean(CryptographyHelper.AesDecrypt(await _appConfigService.GetStringValueByApplicationIdAsync(ApplicationConfigKey
                     .GetAdminApprovalRequiredForProductSalesPlanning, _userPrincipal.ApplicationId))) ? null : true;
                }

            }

            entity.StartDateString = startDate != 0 ? DateFunction.GetDateFromIntReverse(startDate) : null;

            entity.EndDateString = endDate.HasValue ? DateFunction.GetDateFromIntReverse(endDate) : null;

            var productSalesModeTypeId = await _productService.GetSalesModeTypeIdAsync(productId.Value);

            if (productSalesModeTypeId == (int)SalesModelTypeEnum.SalesInAllRepresentation)
                entity.RepresentationId = null;
        }

        private async Task<(string message, int saleConfigurationId)>
            CheckProductCompanyOrRepresentationHasAnyActiveSalesConfiguration(int productId, int? representationId)
        {
            var productCompanyId = _productService.GetProductCompanyId(productId);

            var saleConfiguration = await _saleConfigurationService.GetActiveSaleConfigurationAsync(
                productCompanyId, representationId);


            if (saleConfiguration != null)
                return (string.Empty, saleConfiguration.Id);


            return (
                "کاربر گرامی برای نمایندگی یا مرکز شما تنظیم فروش ثبت نشده است. شما نمی توانید برنامه ریزی فروش ثبت کنید",
                0);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(ProductSalesPlanningEditModel editModel)
        {


            (string message, int saleConfigurationId) =
                await CheckProductCompanyOrRepresentationHasAnyActiveSalesConfiguration(editModel.ProductId.Value
                    , editModel.RepresentationId);

            if (!string.IsNullOrEmpty(message))
                return message;

            editModel.SaleConfigurationId = saleConfigurationId;


            // اگر product Id نداشته باشیم
            // یعنی کاربر در حالت تعریف دوره نیز هست که باید کد نیز بررسی شود
            if (editModel.ProductId == 0)
            {
                var result = _productService.ValidateProductCodeBeforeInsert(editModel.Product.Code);

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            if (editModel.Product != null)
            {
                var productSaleModeTypeValidate =
                    ValidateProductSaleModeType(editModel.Product.SalesModelType, editModel.RepresentationId);
                if (!productSaleModeTypeValidate.isvalid)
                {
                    return productSaleModeTypeValidate.message;
                }
            }

            await CheckUserHasAccessForCompanyAndRepresentation(editModel.Product?.CompanyId,
                editModel.RepresentationId);


            await RemovePublicProductSalesPlanningCacheAsync(editModel.IsPublic, null, false);

            return await base.CustomValidateBeforeInsertAsync(editModel);

        }

        /// <summary>
        /// این متد بررسی می کند که اگر در حالتی باشیم که فروش دوره به صورت
        /// فروش در همه نمایندگی ها نباشد یعنی
        /// هر دوره باید توسط نمایدگی مشخصی فروخته شود باید شناسه نمایدگی نیز مقدار داشته باشد در
        /// غیر این صورت پیام خطا به کاربر برمیگردانیم
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        private (bool isvalid, string message) ValidateProductSaleModeType(int salesModelType, int? RepresentationId)
        {
            if (salesModelType != (int)SalesModelTypeEnum.SalesInAllRepresentation && !RepresentationId.HasValue)
            {
                return (false, "کاربر گرامی، دانشکده یا نمایندگی را باید انتخاب نمایید. ");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="categoryId">دسته بندی</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="representationId">نمایندگی</param>
        /// <param name="showOnlyActiveProductSalesPlannings">نمایس برنامه ریزی دوره فعال</param>
        /// <param name="visibleIndividually"></param>
        /// <param name="doGetProductSalePlanningByRepresentation">برنامه ریزی دوره را ابتدا بر اساس نمایندگی دریافت می کند و سپس سایر فیلتر ها را اعمال می کند</param>
        /// <param name="orderId">شناسه سفارش در صورتی که این پارامتر مقدار داشته باشد باشگاه مشتریان را از این سفارش  با این شناسه دریافت می کنیم</param>
        /// <param name="orderId">شناسه سفارش در صورتی که این پارامتر مقدار داشته باشد باشگاه مشتریان را از این سفارش  با این شناسه دریافت می کنیم</param>
        /// <param name="showArchives">نمایش برنامه ریزی دوره هایی که آرشیو شده اند</param>
        /// <returns></returns>
        [HttpGet("GetByCategoryIdAndCompanyId")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetByCategoryId(CustomDataSourceLoadOptions loadOptions,
            int? categoryId,
            int? companyId,
            int? representationId,
            bool showOnlyActiveProductSalesPlannings,
            bool? visibleIndividually,
            bool doGetProductSalePlanningByRepresentation,
            int? orderId,
            bool? showArchives)
        {

            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);
            int? orderCustomerClubId = null;
            // در صورتی که شناسه سفارش مقدار داشته باشد
            // باشگاه مشتریان کاربر سفارش  را برای دریافت برنامه ریزی دوره  به متد  نهایی ارسال می کنیم
            if (orderId != null)
            {
                orderCustomerClubId = await _orderService
                    .FindById(orderId.Value, _applicationConfiguration.ApplicationId)
                    .Select(o => o.User.AdditionalUserPropUser.CustomerClubId)
                    .FirstOrDefaultAsync();
            }

            var isCurrentUserAdminOrLocalAdmin = await _userRoleService.
               IsUserHasRoleAsync(_userPrincipal.Id, _userPrincipal.ApplicationId, RoleEnum.Admin, RoleEnum.LocalAdmin);

            var productSalesPlannings = await _productSalesPlanningService
                .GetByRepresentationFilterAsync(
                    _userPrincipal.ApplicationId,
                    companyId, categoryId,
                    representationId,
                    showOnlyActiveProductSalesPlannings,
                    doGetProductSalePlanningByRepresentation,
                    visibleIndividually,
                    orderCustomerClubId,
                    showArchives);
            return await DataSourceLoader.LoadAsync(
                productSalesPlannings.ProjectTo<ProductSalesPlanningListViewModel>(_mapper.ConfigurationProvider,
                  new Dictionary<string, object>
                        {{"isAdmin" ,isCurrentUserAdminOrLocalAdmin}}
                ),
                loadOptions);
        }

        [HttpGet("GetTeacherProductSalesPlanningByFilter")]
        [YashilAuthorization(ActionName = "GetTeacherProductSalesPlanningByFilter")]
        public async Task<LoadResult> GetTeacherProductSalesPlanningByFilter(CustomDataSourceLoadOptions loadOptions,
            int? companyId, int? categoryId, int? representationId, bool showOnlyActiveProductSalesPlannings,
            bool? visibleIndividually, bool hierarchical = true)
        {
            var productSalesPlannings = await _productSalesPlanningService.GetTeacherProductSalesPlanningsAsync(
                _userPrincipal.ApplicationId, _userPrincipal.Id, companyId, categoryId, representationId, hierarchical,
                showOnlyActiveProductSalesPlannings, visibleIndividually);
            return await DataSourceLoader.LoadAsync(
                productSalesPlannings.ProjectTo<ProductSalesPlanningListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// برای فرم ویرایش برنامه ربزی دوره، درصد تخفیفی که در تنظیم قرار داد ثبت شده را میخواهیم نمایش بدهیم
        /// کاربر وقتی دوره ای را برنامه ریزی میکند اگر درصد تخفیف برنامه ریزی دوره را بخواهد مقدار دهی کند یا تغییر بدهد
        /// درصد تخفیف ثبت شده در برنامه ریزی دوره برای کاربر نمایش داده بشود
        /// باید متد زیر را تغییر بدهیم 
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>

        protected override async Task<ProductSalesPlanningEditModel> GetEntityAndMapToEditModelForEdit(
            int productSalesPlanningId)
        {
            var productSalesPlanning = _productSalesPlanningService.FindById(productSalesPlanningId);
            var entityForEdit = await productSalesPlanning
                .ProjectTo<ProductSalesPlanningEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            var hekmatCustomerClubId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCustomerClubsHekmatCustomerClubId);

            if (hekmatCustomerClubId == 0)
            {
                throw new Exception(
                    "hekmatCustomerClubId is null in method: ProductSalesPlanningController.GetEntityAndMapToEditModelForEdit");
            }

            if (entityForEdit != null && entityForEdit.SaleConfigurationId.HasValue)
            {
                #region Check Discount Percentage

                entityForEdit.HasFinalDiscountPercentageChanged = await _productSalesPlanningService
                    .HasDiscountPercentageChangedComparedToTheContractAsync(
                        productSalesPlanningId);

                #endregion

            }

            return entityForEdit;
        }

        private async Task<decimal?> GetSaleConfigurationDiscountPercentage
            (int productSalesPlanningId, int customerClubId)
        {
            var saleConfiguration = await _saleConfigurationService
                .GetSaleConfigurationAttributeAsync(productSalesPlanningId, customerClubId);
            if (saleConfiguration.DiscountPercentage.HasValue)
            {
                return saleConfiguration.DiscountPercentage.Value;
            }

            return null;
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(ProductSalesPlanningEditModel editModel)
        {
            var validateSaleConfigurationResult =
                ValidateProductSalesPlanningSaleConfiguration(editModel.SaleConfigurationId);

            if (!string.IsNullOrEmpty(validateSaleConfigurationResult))
                return validateSaleConfigurationResult;

            var pspCompanyIdAndRepresentationId =
                await _productSalesPlanningService.GetCompanyAndRepresentationIdAsync(editModel.Id);



            if (editModel.Product != null)
            {
                var productSaleModeTypeValidate =
                    ValidateProductSaleModeType(editModel.Product.SalesModelType, editModel.RepresentationId);
                if (!productSaleModeTypeValidate.isvalid)
                {
                    return productSaleModeTypeValidate.message;
                }
            }

            await CheckUserHasAccessForCompanyAndRepresentation(pspCompanyIdAndRepresentationId.companyId,
                pspCompanyIdAndRepresentationId.represenationId);

            await RemovePublicProductSalesPlanningCacheAsync(editModel.IsPublic, editModel.Id, true);

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        /// <summary>
        /// این متد کار بررسی
        /// SaleConfiguration
        /// برای یک برنامه ریزی دوره انجام می دهد
        /// </summary>
        /// <param name="salesConfigurationId">شناسه تنظیم فروش</param>
        /// <returns></returns>
        private string ValidateProductSalesPlanningSaleConfiguration(int? salesConfigurationId)
        {
            if (salesConfigurationId == 0 || salesConfigurationId == null)
                return
                    "کاربر برای نمایندگی یا مرکز شما تنظیم فروش ثبت نشده است. شما نمی توانید برنامه ریزی فروش ثبت کنید";

            var saleConfiguration = _saleConfigurationService.Get(salesConfigurationId, true);

            if (saleConfiguration == null || !saleConfiguration.IsActive || saleConfiguration.Deleted)
                return
                    "کاربر گرامی به دلیل اینکه قرار داد فعال برای مرکز یا نمایندگی وجود ندارد، مجاز به ویرایش نمی باشید.";

            return string.Empty;
        }

        /// <summary>
        /// این متد برای حذف کش مربوط به ای پی ای دریافت دور های که به صورت عمومی
        ///  در سامانه ال ام اس تعریف شده اند کاربرد دارد
        /// به صورتی که اگر دوره ای
        /// به هنگام درج مقدار فیلد
        /// IsPublic = true
        /// باشد کش را پاک می کنیم
        /// یا در حالت بروزرسانی اگر مقدار این فیلد تغییر کرده باشد نیز کش را پاک می کنیم
        /// </summary>
        /// <param name="editModelIsPublic">مقدار isPublic  در EditModel -- productSalesPlanning isPublic </param>
        /// <param name="editModelId">شناسه EditModel -- productSalesPlanningId</param>
        /// <param name="isUpdateMode">آیا در حالت بروزرسانی هستیم</param>
        /// <returns></returns>
        private async Task RemovePublicProductSalesPlanningCacheAsync(bool? editModelIsPublic, int? editModelId,
            bool isUpdateMode)
        {
            try
            {

                if (isUpdateMode)
                {
                    var pspIsPublic = await _productSalesPlanningService.GetAll(true)
                        .Where(x => x.Id == editModelId).Select(x => x.IsPublic)
                        .FirstOrDefaultAsync();

                    if (editModelIsPublic.HasValue && editModelIsPublic.Value != pspIsPublic)
                        await _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<ProductSalesPlanningStudent>
                            .GetLandingHomePagePublicProductSalesPlanningKey);
                }
                else
                {
                    if (editModelIsPublic.HasValue && editModelIsPublic.Value)
                        await _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<ProductSalesPlanningStudent>
                            .GetLandingHomePagePublicProductSalesPlanningKey);
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "RemoveCacheError"),
                    nameof(ProductSalesPlanning), "Remove cache error", null);
                throw e;
            }
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var psp = await _productSalesPlanningService.GetCompanyAndRepresentationIdAsync(id);

            await CheckUserHasAccessForCompanyAndRepresentation(psp.companyId, psp.represenationId);

            return await base.CustomValidateBeforeDeleteAsync(id);
        }

        /// <summary>
        ///  نمایش لیست دوره ها در قسمت انتخاب دوره  
        ///  این متد در قسمت ویرایش فرم دانشجویان دوره  کاربرد دارد
        ///  اطلاعاتی که به نمایش گذاشته میشود عبارت اند از نام مرکز نام نمایندگی و مدت دوره ، زمان تشکیل کلاس ، تاریخ شروع ، تاریخ پایان ، مدرس ، نوع برگزاری ، روش اجرای دوره ،جنسیت ، عنوان تنظیم فروش و نام محصول میباشد
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWithProductInfoForSelect")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetWithProductInfoForSelect(CustomDataSourceLoadOptions loadOptions)
        {

            var productSalesPlannings = _productSalesPlanningService.GetAll(true);
            return await DataSourceLoader.LoadAsync(
                productSalesPlannings.ProjectTo<ProductSalesPlanningInfoListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        ///  نمایش اطلاعات دوره انتخاب شده برای دانشجوی مورد نظر  
        ///  این متد در کامپوننت product-sales-planning-info
        ///  استفاده میشود
        ///  اطلاعاتی که به نمایش گذاشته میشود عبارت اند از نام مرکز نام نمایندگی و مدت دوره ، زمان تشکیل کلاس ، تاریخ شروع ، تاریخ پایان ، مدرس ، نوع برگزاری ، روش اجرای دوره ،جنسیت ، عنوان تنظیم فروش و نام محصول میباشد
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningInfo")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<ProductSalesPlanningInfoViewModel> GetProductSalesPlanningInfo(int productSalesPlanningId)
        {
            var productSalesPlanningInfo = _productSalesPlanningService.FindById(productSalesPlanningId);
            return await productSalesPlanningInfo
                .ProjectTo<ProductSalesPlanningInfoViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        }

        [HttpGet("GetProductSalePlanningSimpleListByFilter")]
        [AllowAnonymous]
        public async Task<GetProductSalePlanningSimpleListByFilterViewModel> GetProductSalePlanningSimpleListByFilter(
            string title, int applicationGroupId, int? categoryId, int? cityId, int? companyId, int? representationId,
            int pageSize, int pageIndex)
        {
            var company = companyId.HasValue ? new List<int> { companyId.Value } : new List<int>();
            var allCategoryProducts = await _productSalesPlanningService.SearchProductSalesPlanningsAsync(title,
                applicationGroupId, null, categoryId, company, representationId, cityId, null, true, true);

            var productSalesPlannings = allCategoryProducts.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var productSalesPlanningCustomSimpleListViewModels = await productSalesPlannings
                .ProjectTo<ProductSalesPlanningCustomSimpleListViewModel>(_mapper.ConfigurationProvider).ToListAsync();


            var page = new Page
            {
                Index = pageIndex,
                Size = allCategoryProducts.Count()
            };

            var res = new GetProductSalePlanningSimpleListByFilterViewModel
            {
                productSalesPlannings = productSalesPlanningCustomSimpleListViewModels,
                Page = page
            };
            return res;
        }

        [HttpGet("FindProductSalesPlanningsCompanies")]
        [AllowAnonymous]
        public async Task<List<CompanyCustomSimpleViewModel>> FindProductSalesPlanningsCompanies(string title,
            int? categoryId, int? cityId)
        {
            var companies =
                await _productSalesPlanningService.FindProductSalesPlanningsCompaniesAsync(title, null, categoryId,
                    null, cityId, true, true);
            return await companies.ProjectTo<CompanyCustomSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();

        }

        /// <summary>
        /// به این دلیل این متد را بازنویسی می کنیم :
        /// در صورتی که توضیحات مربوط به برنامه ریزی محصول تغییر کند 
        /// وضعیت برنامه ریزی محصول به ثبت اولیه می رود تا تایید کننده برنامه ریزی محصول 
        /// توضیحات را بررسی و تایید کند
        /// </summary>
        /// <param name="descEditModel"></param>
        /// <returns></returns>
        [YashilAuthorization(ActionName = "UpdateEntityDescription")]
        public async override Task<IActionResult> UpdateEntityDescription(DescriptionEditModel<int> descEditModel)
        {
            var productSalesPlanning = _productSalesPlanningService.Get(descEditModel.Id, false);
            //   هست یا نه LMS بررسی اینکه سامانه در حالت
            if (!await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode))
            {
                /*
                 در صورتی که کاربر دسترسی انتشار محصول را نداشته باشد، بعد از ویرایش
                 این محصول به وضعیت در انتظار تایید می رود و در غیر این صورت تایید 
                 می شود
                 */
                if (!_productSalesPlanningService.CanCurrentUserConfirmProductSalesPlanning())
                {
                    productSalesPlanning.Confirmed = null;
                }
            }
            else
            {
                if (!_productSalesPlanningService.CanCurrentUserConfirmProductSalesPlanning())
                {
                    //  می شود تا در انتظار تایید بالادستی باشد null درصورتی که نیاز به تایید ادمین یا بالادستی بود این مقدار
                    // در غیر اینصورت تایید می شود
                    productSalesPlanning.Confirmed = Convert.ToBoolean(CryptographyHelper.AesDecrypt(await _appConfigService.GetStringValueByApplicationIdAsync(ApplicationConfigKey
                     .GetAdminApprovalRequiredForProductSalesPlanning, _userPrincipal.ApplicationId))) ? null : true;
                }
            }

            return await base.UpdateEntityDescription(descEditModel);

        }

        /// <summary>
        /// دریافت عکس دوره
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningPicture")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client,
            VaryByQueryKeys = new string[] { "modificationDate" })]
        public async Task<IActionResult> GetProductSalesPlanningPicture(int? productSalesPlanningId)
        {
            if (!productSalesPlanningId.HasValue || productSalesPlanningId == 0)
            {
                return NotFound();
            }

            var appDocument =
                await _productSalesPlanningService.GetProductSalesPlanningPicture(productSalesPlanningId.Value,
                    _applicationConfiguration.ApplicationId);
            if (appDocument?.DocumentFile != null)
            {
                return File(appDocument.DocumentFile, appDocument.ContentType, appDocument.OrginalName);
            }

            return File("images/DefaultLms.png", "image/png");
        }

        private async Task CheckUserHasAccessForCompanyAndRepresentation(int? companyId, int? representationId)
        {
            bool userHasAccessForCompanyAndRepresentation = await _representationService
                .CheckUserAccessForCompanyAndRepresentationAsync(companyId, representationId, _userPrincipal.Id);

            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(ProductSalesPlanning), "Get Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(ProductSalesPlanning)}");
            }
        }

        /// <summary>
        /// برنامه ریزی محصولات فروش رفته که فاکتور نشده اند را بر می گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId">مرکز </param>
        /// <param name="representationId">نمایندگی</param>
        /// <returns></returns>
        [HttpGet("GetUnFactoredProductSalesPlanning")]
        [YashilAuthorization()]
        public async Task<LoadResult> GetUnFactoredProductSalesPlannings(CustomDataSourceLoadOptions loadOptions,
            int? companyId,
            int? representationId
        )
        {
            // دریافت اطلاعات کاربر لاگین شده
            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);

            var productSalesPlannings = await _productSalesPlanningService
                .GetUnFactoredProductSalesPlanningsAsync(
                    _userPrincipal.ApplicationId, companyId, representationId);
            return await DataSourceLoader.LoadAsync(
                productSalesPlannings.ProjectTo<ProductSalesPlanningListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// این متد عملیات جستجو یک رشته کلید را در بین
        /// عنوان برنامه ریزی  های دروه
        /// عنوان مراکز و عنوان دسته بندی ها را انجام می دهد
        /// </summary>
        /// <param name="searchKey">کلید جستجو</param>
        /// <returns>SearchByKeyViewModel</returns>
        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<SearchByKeyViewModel> Search(string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey) || searchKey.Length < 2)
                return null;

            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            SearchByKeyViewModel searchByKeyViewModel = new SearchByKeyViewModel();

            var productSalesPlannings = await _productSalesPlanningService
                .SearchInProductSalesPlanningTitleAsync(searchKey);

            searchByKeyViewModel.SearchInProductSalesPlanningResult =
                await productSalesPlannings
                    .Select(psp => new ProductSalesPlanningSearchViewModel
                    {
                        ProductSalesPlanningTitle = psp.ProductSalesPlanningTitle,
                        ProductSalesPlanningId = psp.ProductSalesPlanningId,
                        RepresentationTitle = psp.RepresentationTitle,
                        RepresentationId = psp.RepresentationId,
                        CompanyId = psp.CompanyId,
                        CompanyTitle = psp.CompanyTitle
                    }).ToListAsync();
            searchByKeyViewModel.SearchInCompanyResult = await (await
                    _companyService.SearchInCompanyTitleAsync(searchKey, additionalUserProp.CustomerClubId))
                .Select(s => new CompanySearchViewModel
                {
                    Title = s.Title,
                    Id = s.Id
                }).ToListAsync();
            searchByKeyViewModel.SearchInCategoryResult = _categoryService
                .SearchInCategoryTitle(searchKey, additionalUserProp.CustomerClubId)
                .Select(category => new CategorySearchViewModel
                {
                    Title = category.Title,
                    Id = category.Id,
                    IsLevelOneCategory = category.IsLevelOneCategory
                }).ToList();



            return searchByKeyViewModel;
        }

        /// <summary>
        /// این api
        /// جهت درج  برنامه ریزی دوره جدید در سامانه کاربرد دارد
        /// علت استفاده از این متد برای درج برنامه ریزی دوره به حای متد
        /// BaseController
        /// استفاده از EditModel
        /// کاستوم برای جلوگیری از OverBinding
        /// است
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost("AddProductSalesPlanning")]
        [YashilAuthorization("PostEntity")]
        public async Task<ResultStatus<int>> AddProductSalesPlanningAsync(ProductSalesPlanningBaseEditModel editModel)
        {

            (string message, int saleConfigurationId) =
                await CheckProductCompanyOrRepresentationHasAnyActiveSalesConfiguration(editModel.ProductId.Value
                    , editModel.RepresentationId);


            if (!string.IsNullOrEmpty(message))
                return new ResultStatus<int>(false, message);


            ProductSalesPlanning entity = _mapper.Map<ProductSalesPlanning>(editModel);
            entity.SaleConfigurationId = saleConfigurationId;

            await SetDefaultPropsAsync(editModel, entity);


            if (!editModel.ProductId.HasValue)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException,
                    nameof(ProductSalesPlanning),
                    "Error in insert new productSalesPlanning editModel.ProductId Not HasValue", null);

                return new ResultStatus<int>(false,
                    "کاربر گرامی به هنگام انجام عملیات خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }


            var validatePoints = await ValidatePointsValues(editModel);
            if (!validatePoints.isValid)
            {
                return new ResultStatus<int>(false, validatePoints.message);
            }


            await ValidateAssignProductSalesPlanningToAllRepresentationValueAsync(editModel);

            var product = await _productService.FindById(editModel.ProductId.Value)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                var productSaleModeTypeValidate =
                    ValidateProductSaleModeType(product.SalesModelType, editModel.RepresentationId);
                if (!productSaleModeTypeValidate.isvalid)
                {
                    return new ResultStatus<int>(false, productSaleModeTypeValidate.message);
                }
            }

            await CheckUserHasAccessForCompanyAndRepresentation(product.CompanyId,
                editModel.RepresentationId);

            var insertedPsp = new ProductSalesPlanning();
            try
            {

                insertedPsp = await _productSalesPlanningService.AddAsync(entity, true);

                await _productSalesPlanningService
                    .UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync
                        (insertedPsp.Id);

                await ClearCacheAsync(product?.CompanyId);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException,
                    nameof(ProductSalesPlanning), "Error in insert new productSalesPlanning", e);
                return new ResultStatus<int>(false,
                    "کاربر گرامی به هنگام انجام عملیات خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }

            return new ResultStatus<int>
            {
                EntityId = insertedPsp.Id,
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد."
            };
        }


        //اگر نمایندگی مقدار نداشت و تیک گزینه فروش محصول در تمامی نمایدگی ها خورده باشد
        //باید product
        //انتخابی بروزرسانی شده و نوع فروش آن به فروش در تمامی نمایدگی تغییر پیدا کند
        [NonAction]
        private Task ValidateAssignProductSalesPlanningToAllRepresentationValueAsync(ProductSalesPlanningBaseEditModel editModel)
        {

            if (editModel.ProductId.HasValue && !editModel.RepresentationId.HasValue &&
                editModel.AssignProductSalesPlanningToAllRepresentation.HasValue &&
                editModel.AssignProductSalesPlanningToAllRepresentation.Value)
                return _productService.UpdateProductSalesModeTypeAsync(editModel.ProductId.Value,
                    SalesModelTypeEnum.SalesInAllRepresentation);

            return Task.CompletedTask;
        }

        /// <summary>
        /// این api
        /// جهت بروزرسانی که برنامه ریزی دوره در سامانه کاربرد دارد
        /// علت استفاده از این متد برای بروزرسانی برنامه ریزی دوره به حای متد
        /// BaseController
        /// استفاده از EditModel
        /// کاستوم برای جلوگیری از OverBinding
        /// است
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost("UpdateProductSalesPlanning")]
        [YashilAuthorization("UpdateEntity")]
        public async Task<ResultStatus<int>> UpdateProductSalesPlanningAsync(
            ProductSalesPlanningBaseEditModel editModel)
        {
            (string message, int saleConfigurationId) =
                await CheckProductCompanyOrRepresentationHasAnyActiveSalesConfiguration(editModel.ProductId.Value
                    , editModel.RepresentationId);

            if (!string.IsNullOrEmpty(message))
                return new ResultStatus<int>(false, message);

            var validateSalesConfigurationResult = ValidateProductSalesPlanningSaleConfiguration(saleConfigurationId);

            if (!string.IsNullOrEmpty(validateSalesConfigurationResult))
                return new ResultStatus<int>(false, validateSalesConfigurationResult);

            var pspCompanyIdAndRepresentationId =
                await _productSalesPlanningService.GetCompanyAndRepresentationIdAsync(editModel.Id);


            if (!editModel.ProductId.HasValue)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException,
                    nameof(ProductSalesPlanning),
                    "Error in insert new productSalesPlanning editModel.ProductId Not HasValue", null);

                return new ResultStatus<int>(false,
                    "کاربر گرامی به هنگام انجام عملیات خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }

            await ValidateAssignProductSalesPlanningToAllRepresentationValueAsync(editModel);

            var product = await _productService.FindById(editModel.ProductId.Value)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                var productSaleModeTypeValidate =
                    ValidateProductSaleModeType(product.SalesModelType, editModel.RepresentationId);
                if (!productSaleModeTypeValidate.isvalid)
                {
                    return new ResultStatus<int>(false, productSaleModeTypeValidate.message);
                }
            }


            var validatePoints = await ValidatePointsValues(editModel);
            if (!validatePoints.isValid)
            {
                return new ResultStatus<int>(false, validatePoints.message);
            }

            await CheckUserHasAccessForCompanyAndRepresentation(pspCompanyIdAndRepresentationId.companyId,
                pspCompanyIdAndRepresentationId.represenationId);


            ProductSalesPlanning productSalesPlanning =
                _mapper.Map<ProductSalesPlanningBaseEditModel, ProductSalesPlanning>(editModel);

            ProductSalesPlanningEditModel productSalesPlanningEditModel =
                _mapper.Map<ProductSalesPlanningBaseEditModel, ProductSalesPlanningEditModel>(editModel);

            await SetDefaultPropsAsync(editModel, productSalesPlanning);

            try
            {
                ModifyPropertiesDescriptor modifyProps =
                    _productSalesPlanningService.GetPropertiesForApplyOrIgnoreUpdateProductSalesPlanning(
                        productSalesPlanning, editModel);

                await base.UpdateAsync(productSalesPlanning, productSalesPlanningEditModel, productSalesPlanning.Id,
                    modifyProps.Properties, modifyProps.UpdateSelectedFieldsOrIgnoreTheme);

                await _staticCacheManager.RemoveAsync(
                    YashilModelCacheDefaults<ProductSalesPlanning>.PublicProductSalesPlanningCacheKey,
                    productSalesPlanning.Id);

                await _productSalesPlanningService
                    .UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync
                        (editModel.Id);

                await ClearCacheAsync(product?.CompanyId);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException,
                    nameof(ProductSalesPlanning), "Error in update productSalesPlanning", e);

                return new ResultStatus<int>(false,
                    "کاربر گرامی به هنگام انجام عملیات خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }

            return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");
        }

        /// <summary>
        /// این متد برای دریافت برنامه ریزی دوره کاربرد دارد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetProductSalesPlaning")]
        [YashilAuthorization("GetEntities")]
        public async Task<ProductSalesPlanningBaseEditModel> GetProductSalesPlaningAsync(int id)
        {
            var userHasAccessToPsp =
                await _productSalesPlanningService.CheckUserHasAccessToProductSalesPlanning(id, _userPrincipal.Id);

            if (!userHasAccessToPsp)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(ProductSalesPlanning), "Get Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(ProductSalesPlanning)}");
            }

            return await _productSalesPlanningService.FindById(id, _userPrincipal.ApplicationId)
                .ProjectTo<ProductSalesPlanningBaseEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }


        [HttpGet("UpdateConfirmStateAfterUploadFile")]
        [YashilAuthorization("UpdateEntity")]
        public async Task<ResultStatus<int>> UpdateConfirmStateAfterUploadFile(int appDocumentId, int pspId)
        {
            var psp = await _productSalesPlanningService.GetAsync(pspId, false);

            if (psp == null)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "بروز خطا به هنگام دریافت برنامه ریزی دوره جهت بروزرسانی وضعیت تایید دوره پس از بارگذاری سند",
                    null);
                return new ResultStatus<int>(false,
                    "کاربر گرامی، به هنگام دریافت برنامه ریزی دوره خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }

            if (!_productSalesPlanningService.CanCurrentUserConfirmProductSalesPlanning())
            {
                psp.Confirmed = null;
                try
                {
                    await _productSalesPlanningService.UpdateAsync(psp, psp.Id, true);
                    return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(ProductSalesPlanning),
                        "بروز خطا به هنگام بروزرسانی وضعیت تایید دوره پس از بارگذاری فایل برای برنامه ریزی دوره", ex);

                    var appDocument = await _appDocumentService.GetAsync(appDocumentId, false);

                    await _appDocumentService.DeleteAsync(appDocumentId, true);

                    return new ResultStatus<int>(false,
                        "کاربر گرامی، به هنگام انجام عملیات خطایی رخ داده است لطفا با مدیر سیستم تماس بگیرید.");

                }
            }

            return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");
        }

        /// <summary>
        /// این متد جهت دریافت برانامه ریزی دور های فعال بر اساس شناسه مرکز و نمایندگی کاربرد دارد
        /// برای مواقعی که می خواهیم
        /// selectOption
        /// انجام دهیم
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApproval")]
        [YashilAuthorization("GetForSelect")]
        public async Task<LoadResult> GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApprovalAsync(CustomDataSourceLoadOptions loadOptions,
            int? companyId = null, int? representationId = null)
        {
            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);

            if (companyId.HasValue && companyId.Value == 0)
                companyId = null;

            if (!representationId.HasValue || representationId.Value == 0)
                representationId = null;

            var productSalesPlanningQuery = await _productSalesPlanningService
                .GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApprovalAsync
                    (companyId, _userPrincipal.ApplicationId, representationId);

            return await DataSourceLoader.LoadAsync(productSalesPlanningQuery
                .ProjectTo<ProductSalesPlanningSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetProductSalesPlanningWithRequireSupplierApprovalBeforePayment")]
        [YashilAuthorization("GetForSelect")]
        public async Task<LoadResult> GetProductSalesPlanningWithRequireSupplierApprovalBeforePaymentAsync(CustomDataSourceLoadOptions loadOptions,
            int? companyId = null, int? representationId = null)
        {
            await CheckUserHasAccessForCompanyAndRepresentation(companyId, representationId);

            if (companyId.HasValue && companyId.Value == 0)
                companyId = null;

            if (!representationId.HasValue || representationId.Value == 0)
                representationId = null;

            var productSalesPlanningQuery = await _productSalesPlanningService
                .GetProductSalesPlanningWithRequireSupplierApprovalBeforePaymentAsync
                    (companyId, _userPrincipal.ApplicationId, representationId);

            return await DataSourceLoader.LoadAsync(productSalesPlanningQuery
                .ProjectTo<ProductSalesPlanningSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این تابع بررسی می کند که آیا تعیین سطح مهارت فعال است یا نه و در صورتیکه  فعال باشد
        /// بررسی می کند که آیا مقادر امتیازات به درستی وارد شده است یا نه
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>

        private async Task<(bool isValid, string message)> ValidatePointsValues(ProductSalesPlanningBaseEditModel editModel)
        {
            const string nullOrZeroMessage =
                "کاربر گرامی، برای استفاده از تعیین سطح مهارت دروس دانشجویان، مقادیر امتیازات را وارد کنید.";

            const string pointsExceedLimitMessage =
                "کاربر گرامی، مجموع امتیازات داده شده نباید بیشتر از 100 باشد.";

            try
            {
                var isLmsMode = await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode);
                if (!isLmsMode) return (true, string.Empty);


                var configValue = await _appConfigService.GetStringValueAsync(
                    YashilLight.Core.Classes.ApplicationConfigKey.UsingAbilityToDetermineSkillLevelOfStudentsCourses);

                var isValidationEnabled = Convert.ToBoolean(
                    CryptographyHelper.AesDecrypt(configValue));


                if (!isValidationEnabled)
                {
                    return (true, string.Empty);
                }

                // Check for null or zero values
                if (editModel.ClassPoint is null or <= 0 &&
                    editModel.ExamPoint is null or <= 0 &&
                    editModel.HomeWorkPoint is null or <= 0)
                {
                    return (false, nullOrZeroMessage);
                }


                // Calculate and validate sum
                var totalPoints = editModel.ClassPoint +
                                  editModel.ExamPoint +
                                  editModel.HomeWorkPoint;

                if (totalPoints > 100)
                {
                    return (false, pointsExceedLimitMessage);
                }

                return (true, string.Empty);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "بروز خطا به هنگام بررسی مقادیر ثبت شده توسط کاربر",
                    e);

                return (false, "کاربر گرامی، خطایی در اعتبارسنجی مقادیر امتیازات رخ داده است. لطفا با ادمین تماس حاصل فرمایید.");
            }
        }


        [HttpPost("ArchiveProductSalesPlanning")]
        [YashilAuthorization("ArchiveEntity")]
        public async Task<ResultStatus<int>> ArchiveProductSalesPlanningAsync(ProductSalesPlanningSimpleEditModel psp)
        {
            string successMessage = "برنامه ریزی دوره انتخاب شده با موفقیت آرشیو شد";
            string errorMessage = "متاسفانه خطایی در هنگام پردازش رخ داده است. دوباره سعی کنید";
            try
            {
                var result = await _productSalesPlanningService.Archive(psp.Id);
                if (result == -1)
                {
                    return new ResultStatus<int>(false, errorMessage);
                }

                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "آرشیو برنامه ریزی دوره",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = nameof(ArchiveProductSalesPlanningAsync),
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
                    "بروز خطا به هنگام آرشیو برنامه",
                    e);
                return new ResultStatus<int>(false, errorMessage);
            }
        }

        [HttpPost("RestoreProductSalesPlanningFromArchive")]
        [YashilAuthorization("ArchiveEntity")]
        public async Task<ResultStatus<int>> RestoreProductSalesPlanningFromArchiveAsync(ProductSalesPlanningSimpleEditModel psp)
        {
            string successMessage = "برنامه ریزی دوره انتخاب شده با موفقیت از آرشیو خارج شد";
            string errorMessage = "متاسفانه خطایی در هنگام پردازش رخ داده است. دوباره سعی کنید";
            try
            {
                var result = await _productSalesPlanningService.RestoreFromArchive(psp.Id);
                if (result == -1)
                {
                    return new ResultStatus<int>(false, errorMessage);
                }

                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "خروج از آرشیو برنامه ریزی دوره",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = nameof(ArchiveProductSalesPlanningAsync),
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
                    "بروز خطا به هنگام خروج از آرشیو برنامه",
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
        /// این متد جهت حذف کردن کش مربوط به برنامه ریزی دوره کاربرد دارد
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private async Task ClearCacheAsync(int? companyId = null)
        {
            if (_userPrincipal.ApplicationId == _configuration.GetSection("ApplicationsId:Lms").Get<int>())
            {
                var tasks = new List<Task>();

                tasks?.Add(_staticCacheManager
                    .RemoveAsync(YashilModelCacheDefaults<ProductSalesPlanningStudent>
                        .GetLandingHomePagePublicProductSalesPlanningKey));


                if (companyId.HasValue)
                    tasks?.Add(_staticCacheManager.RemoveAsync(YashilModelCacheDefaults<ProductSalesPlanningStudent>
                        .GetProductSalesPlanningConsiderPublicFilteredByCompanyIdKey, companyId.Value));

                await Task.WhenAll(tasks);

            }
        }


        /// <summary>
        /// دریافت برنامه ریزی دوره های در حال اجرا یا اجرا شده
        /// یا اجرا شده
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("GetCompanyProductSalesPlanningComprehensiveReport")]
        [YashilAuthorization("GetCompanyProductSalesPlanningComprehensiveReport")]
        public async Task<LoadResult> GetCompanyProductSalesPlanningComprehensiveReportAsync(CustomDataSourceLoadOptions loadOptions,
            int? companyId = null)
        {
            if (companyId is < 0)
                return new LoadResult()
                {
                    data = Enumerable.Empty<OngoingProductSalesPlanningViewModel>()
                };

            await CheckUserHasAccessForCompanyAndRepresentation(companyId, null);

            return await DataSourceLoader.LoadAsync(
                _productSalesPlanningService.GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(companyId)
                    .GroupBy(x => new OngoingProductSalesPlanningGroupKey
                    {
                        CompanyId = x.Product.CompanyId,
                        RepresentationId = x.RepresentationId
                    })
                    .ProjectTo<OngoingProductSalesPlanningViewModel>(_mapper.ConfigurationProvider)
                , loadOptions);

        }

        /// <summary>
        /// دریافت گزارش جامع مربوط به برنامه ریزی دوره های یک مرکز و نمایندگی
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningComprehensiveReport")]
        [YashilAuthorization("GetProductSalesPlanningComprehensiveReport")]
        public async Task<LoadResult> GetProductSalesPlanningComprehensiveReportAsync(CustomDataSourceLoadOptions loadOptions, int? companyId, int? representationId)
        {
            if (companyId is null or < 0 || (representationId is null or < 0))
                return new LoadResult()
                {
                    data = Enumerable.Empty<CompanyProductSalesPlanningComprehensiveReportViewModel>()
                };

            await CheckUserHasAccessForCompanyAndRepresentation(companyId, null);

            return await DataSourceLoader.LoadAsync(
                _productSalesPlanningService.GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(companyId, representationId)
                    .GroupBy(x => x.Id)
                    .ProjectTo<CompanyProductSalesPlanningComprehensiveReportViewModel>(_mapper.ConfigurationProvider)
                , loadOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetproductSalesPlanningsAssignedToproductSalesPlanningStudentAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetProductsAssignedToStudentAsync(
          CustomDataSourceLoadOptions loadOptions,
          int id,
          int? companyId,
          int? representationId)
        {
            var list = _productSalesPlanningStudentService.GetAssignedProductToStudent(id, companyId, representationId);
            return await DataSourceLoader.LoadAsync(list.ProjectTo<ProductSalesPlanningAssignedViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetproductSalesPlanningsNotAssignedToProductSalesPlanningStudentAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetProductSalesPlanningsNotAssignedToProductSalesPlanningStudentAsync(
         CustomDataSourceLoadOptions loadOptions,
         int id,
         int? companyId,
         int? representationId)
        {
            var list = await _productSalesPlanningStudentService.GetNotAssignedProductToStudentAsync(id, companyId, representationId);
            return await DataSourceLoader.LoadAsync(list.ProjectTo<ProductSalesPlanningAssignedViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد در فرم تخصیص دانشجو به چند دوره کاربرد دارد
        /// این متد برای تخصیص دانشجو به چند دوره و خارج کردن  دانشجو از دوره ها کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="assignableListEditModel"></param>
        /// <returns></returns>
        [HttpPost("AssignSelectedItemsToProductSalesPlanningStudent")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<bool> AssignSelectedItemsToProductSalesPlanningStudent(AssignableListEditModel assignableListEditModel)
        {
            return await _productSalesPlanningStudentService.
                AssignSelectedProductSalesPlanningToStudentAsync(
                new List<int>(assignableListEditModel.SelectedItems),
                assignableListEditModel.GroupId,
                assignableListEditModel.Assign);
        }
        /// <summary>
        /// دریافت فایل پیوست شده در بخش پیوست های دوره
        /// </summary>
        /// <param name="mediaCode"></param>
        /// <returns></returns>
        [HttpGet("GetAttachedMediaFileByCode")]
        [YashilAuthorization("GetAttachedMediaFile")]
        public FileContentResult GetAttachedMediaFileByCode(string mediaCode)
        {
            var appDoc = _appDocumentService.GetFileByCode(Guid.Parse(mediaCode), (int)DocumentTypeEnum.ProductSalesPlanningVideo, true);

            if (appDoc?.DocumentFile != null && appDoc?.ContentType != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType, appDoc.OrginalName, true);
            }
            return null;
        }


        /// <summary>
        /// دریافت آخرین برنامه ریزی دوره های تعریف شده در سامانه در صورتی که کاربر ادمین باشد
        /// و اگر مربی باشد برنامه ریزی دوره های مربوط به خود را میبیند
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetLatestProductSalesPlaningInfoForShowInAnalystDashboard")]
        [YashilAuthorization("GetLatestProductSalesPlaningInfoForShowInAnalystDashboard")]
        public async Task<List<LastestProductSalesPlaningForShowInAnalyticsDashboard>> GetLatestProductSalesPlaningForShowInAnalystDashboardAsync()
        {
            var isTeacher = await _userRoleService.CheckRoleAssignedToUserAsync(_userPrincipal.Id, (int)RoleEnum.Teacher,
                true, null, _userPrincipal.ApplicationId);

            if (isTeacher)
            {
                return await _productSalesPlanningService.GetTeacherLatestActiveProductSalesPlanning(_userPrincipal.Id, null)
                    .ProjectTo<LastestProductSalesPlaningForShowInAnalyticsDashboard>(_mapper.ConfigurationProvider,
                        new Dictionary<string, object> { { "isTeacher", true } }).ToListAsync();
            }

            var additionalUserProp = await _additionalUserPropService
                .GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId)
                .Select(aup => new { aup.RepresentationId, aup.CompanyId })
                .FirstOrDefaultAsync();


            return await _productSalesPlanningService.GetLatestActiveProductSalesPlanning(additionalUserProp?.CompanyId, additionalUserProp?.RepresentationId, null).ProjectTo<LastestProductSalesPlaningForShowInAnalyticsDashboard>(_mapper.ConfigurationProvider,
                    new Dictionary<string, object> { { "isTeacher", false } })
                .ToListAsync();
        }

        /// <summary>
        /// دریافت تعداد برنامه ریزی دوره های فعال بر اساس
        /// representation
        /// در صورتی که کاربر مربی باشد و اگر ادمین باشد
        /// بر اساس مرکز اگر تخصیص داده شده باشد تعداد دوره های فعال را برمیگرداند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroupedActiveProductSalesPlanningCountForShowInAnalystDashboard")]
        [YashilAuthorization("GetGroupedActiveProductSalesPlanningCountForShowInAnalystDashboard")]
        public async Task<List<ActiveProductSalesPlanningCountGroupedDto>> GetActiveProductSalesPlanningCountGroupByCompanyAsync()
        {
            var isTeacher = await _userRoleService.CheckRoleAssignedToUserAsync(_userPrincipal.Id, (int)RoleEnum.Teacher,
                true, null, _userPrincipal.ApplicationId);

            if (isTeacher)
            {
                return await _productSalesPlanningService.GetTeacherActiveProductSalesPlanningCountGroupByRepresentationAsync(
                    _userPrincipal.Id);
            }

            var additionalUserProp = await _additionalUserPropService
                .GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId)
                .Select(aup => new { aup.RepresentationId, aup.CompanyId })
                .FirstOrDefaultAsync();

            return await _productSalesPlanningService
                .GetActiveProductSalesPlanningCountGroupByCompanyAsync(additionalUserProp?.CompanyId);

        }
        /// <summary>
        /// دریافت لیست دوره های تخصیص شده به دانشجو
        /// </summary>
        /// <param name="studentId">شناسه دانشجو</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <returns></returns>

        [HttpGet("GetStudentsProductSalesPlanningListByParams")]
        [YashilAuthorization("GetStudentsProductSalesPlanningListByParams")]
        public async Task<LoadResult> GetStudentsProductSalesPlanningListByParamsAsync(CustomDataSourceLoadOptions loadOptions, int studentId, int? companyId, int? representationId)
        {
            bool userHasAccessForCompanyAndRepresentation = await _representationService
                             .CheckUserAccessForCompanyAndRepresentationAsync(companyId, representationId, _userPrincipal.Id);

            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(ProductSalesPlanning), "Get Error", null);
                throw new Exception(
                    $"{Events.AccessDeniedGetEntities} : {nameof(GetStudentsProductSalesPlanningListByParamsAsync)}");
            }

            var productSalesPlanning = _productSalesPlanningStudentService.GetAssignedProductToStudent(studentId, companyId, representationId);
            return await DataSourceLoader.LoadAsync(productSalesPlanning.ProjectTo<StudentProductSalesPlanningViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}