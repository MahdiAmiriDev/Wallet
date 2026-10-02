using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Web.Infrastructure.Classes;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YashilDms.Core.Services;
using Yashil.Core.Enums;
using MimeKit;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilUserManagement.Core.Services;
using YashilCommerce.Core.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UploadedProductItemController : BaseController<UploadedProductItem, int, UploadedProductItemListViewModel, UploadedProductItemEditModel, UploadedProductItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedProductItemService _uploadedProductItemService;
        private readonly IYashilLogger<UploadedProductItem> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IProductService _productService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IAppConfigService _appConfigService;
        private readonly IPermissionService _permissionService;
        private readonly IConfiguration _configuration;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        public UploadedProductItemController(IUploadedProductItemService uploadedProductItemService,
            ApplicationConfiguration applicationConfiguration, IAppDocumentService appDocumentService,
        IMapper mapper, IYashilLogger<UploadedProductItem> logger, IProductService productService,
        IPermissionService permissionService, IUserPrincipal userPrincipal, ITaxCategoryService taxCategoryService, IAppConfigService appConfigService, IConfiguration configuration, IProductSalesPlanningService productSalesPlanningService) :
            base(uploadedProductItemService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _uploadedProductItemService = uploadedProductItemService;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
            _userPrincipal = userPrincipal;
            _taxCategoryService = taxCategoryService;
            _appConfigService = appConfigService;
            _configuration = configuration;
            _productSalesPlanningService = productSalesPlanningService;
            _productService = productService;
            _permissionService = permissionService;
            _appDocumentService = appDocumentService;
        }


        /// <summary>
        /// وقتی کاربر فایا اکسل را بار گذاری می کند:
        /// ابتدا رکورد های قبلی مربوط به کاربر لاگین کرده را از جدول 
        /// Temp UploadProductItem
        /// پاک میکند. 
        /// سپس فایل ارسالی از سمت کاربر را به سرویس درج فایل از طرق اکسل می فرستد
        /// درصورتی که بارگذاری فایل اکسل با موفقیت انجام شود و خطایی در آیتم های مربوط به قیمت نباشد
        /// جداول محصول و برنامه ریزی فروش را به روز رسانی میکند
        /// </summary>
        /// <param name="tempProductFile">فایل ارسالی ار سمت کاربر</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost("uploadProductFile/{companyId}")]
        [YashilAuthorization("UploadProductFile")]
        public async Task<ResultStatus<int>> UploadProductFileAsync(IFormFile tempProductFile, int companyId)
        {
            try
            {
                var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();
                bool isLms = _userPrincipal.ApplicationId == lmsApplicationId;

                if (!isLms)
                {
                    var defaultTaxExemptTaxCategoryId =
                        await _appConfigService.GetIntValueAsync
                            (ApplicationConfigKey.YashilDefaultTaxExemptTaxCategoryId);


                    if (defaultTaxExemptTaxCategoryId <= 0)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کاربر گرامی دسته بندی مالیاتی معاف از مالیات در تنظیمات سایت وجود ندارد."
                        };

                    }
                    var isExistsTaxExemptTaxCategory = await _taxCategoryService
                        .FindById(defaultTaxExemptTaxCategoryId).AnyAsync();


                    if (!isExistsTaxExemptTaxCategory)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کاربر گرامی دسته بندی مالیاتی معاف از مالیات وجود ندارد."
                        };
                    }
                    var defaultCourseTaxCategoryId =
                        await _appConfigService.GetIntValueAsync
                            (ApplicationConfigKey.YashilDefaultCourseTaxCategoryId);
                    if (defaultCourseTaxCategoryId <= 0)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کاربر گرامی دسته بندی مالیاتی دارای مالیات ارزش افزوده در تنظیمات سایت وجود ندارد."
                        };

                    }
                    var isExistsCourseTaxCategory = await _taxCategoryService
                        .FindById(defaultCourseTaxCategoryId).AnyAsync();

                    if (!isExistsCourseTaxCategory)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کاربر گرامی دسته بندی مالیاتی دارای مالیات ارزش افزوده وجود ندارد."
                        };
                    }
                }



                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                await _uploadedProductItemService.DeleteByUserIdAsync(_userPrincipal.Id);

                var res = await _uploadedProductItemService
                    .BulkInsertFromExcelAsync(tempProductFile, companyId);


                if (res.IsSuccess)
                {
                    int invalidUploadedProductItem;

                    if (isLms)
                    {

                        invalidUploadedProductItem = await _uploadedProductItemService
                            .GetInvalidUploadedSimpleProductItemsCountAsync(_userPrincipal.Id, companyId);
                    }
                    else
                    {
                        // موارد مربوط به قیمت محصول بارگذاری شده که نامعتبر می باشند را بررسی میکند
                        invalidUploadedProductItem = await _uploadedProductItemService
                           .GetInvalidUploadedProductItemsCountAsync(
                               _userPrincipal.Id,
                               companyId);
                    }



                    if (invalidUploadedProductItem > 0)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "فایل بارگذاری شده دارای خطا می باشد، لطفا پس از رفع ایرادات فایل، مجددا آن را بارگذاری نمایید."
                        };
                    }
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        Message = "فایل اکسل محصولات با موفقیت بارگذاری شد."
                    };
                }

                return res;

            }
            catch
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                    nameof(RefundDetail), "Upload ProductItem Bulk Insert Error", null);
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }

        /// <summary>
        /// اطلاعات کامل محصولات مربوط به مرکز را در قالب اکسل در سمت کاربر دانلود می کند 
        /// کاربر از این فایل به عنوان نمونه اکسل محصولات برای بارگذاری محصولات از طریق اکسل استفاده می کند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        [HttpGet("ExportCompanyProductsToExcel")]
        [YashilAuthorization("ExportCompanyProductsToExcel")]
        public async Task<FileResult> ExportCompanyProductsToExcelAsync(int companyId, int? categoryGroupId)
        {
            if (companyId == 0)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ExportCompanyProductsToExcelAsync),
                    "برای دریافت اکسل محصولات شناسه مرکز باید مقدار داشته باشد.", new Exception());

                throw new Exception("Invalid companyId");
            }


            try
            {
                var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();

                var isLms = _userPrincipal.ApplicationId == lmsApplicationId;

                if (isLms)
                {
                    var simpleProductDomainModel = await _productService
                        .GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(companyId);

                    if (simpleProductDomainModel == null)
                        return null;

                    var simpleProductDetailViewModel =
                        _mapper.Map<List<ProductDetailViewModel>>(simpleProductDomainModel);


                    var memoryStream =
                        await _uploadedProductItemService.CopyProductListToWorkBook(simpleProductDetailViewModel,
                            companyId, null);

                    return File(memoryStream, MimeTypes.GetMimeType("productsExcel.xlsx"), "productsExcel.xlsx");

                }

                if (!categoryGroupId.HasValue || categoryGroupId.Value == 0)
                    throw new Exception("Invalid categoryGroupId");


                var productDetails = _productService.GetCompanyConfirmedAndPublishedProducts(companyId);

                if (productDetails == null)
                    return null;

                var productDetailList = _mapper.Map<List<ProductDetailViewModel>>(productDetails);

                var ms = await _uploadedProductItemService.CopyProductListToWorkBook(productDetailList, companyId,
                    categoryGroupId);

                return File(ms, MimeTypes.GetMimeType("productsExcel.xlsx"), "productsExcel.xlsx");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException,
                    nameof(ExportCompanyProductsToExcelAsync),
                    "بروز خطا به هنگام دریافت اکسل محصولات", e);

                throw;
            }




        }

        /// <summary>
        /// دریافت اطلاعات فایل اکسل ذخیره شده - برای نمایش دادن در لیست
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="showInvalidItem"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedProductItemsAsync")]
        [YashilAuthorization("GetUploadedProductItems")]
        public async Task<LoadResult> GetUploadedProductItemsAsync(CustomDataSourceLoadOptions loadOptions, bool showInvalidItem)
        {
            var uploadedProductItems = _uploadedProductItemService.GetByUserId(_userPrincipal.Id, showInvalidItem);

            var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();

            var isLms = _userPrincipal.ApplicationId == lmsApplicationId;

            if (isLms)
                return await DataSourceLoader
                    .LoadAsync(uploadedProductItems.ProjectTo<UploadedProductItemSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);


            return await DataSourceLoader.LoadAsync(uploadedProductItems.ProjectTo<UploadedProductItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }


        /// <summary>
        /// این متد برای ذخیره و یا به روز رسانی اطلاعات محصولات بارگذاری شده از جدول 
        /// UploadedProductItem
        /// به جدول های 
        /// product  - ProductSalePlaning - DependentProductSale - ProductCategory
        /// میباشد
        /// وقتی کاربر دکمه ثبت نهایی را می زند
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddDataFromUploadedProductItem")]
        [YashilAuthorization("AddDataFromUploadedProductItem")]
        public async Task<ResultStatus<int>> AddDataFromUploadedProductItemAsync(int? companyId)
        {
            if (companyId == null)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "مرکز انتخابی وجود ندارد"
                };
            }
            var existsAnyInvalidItem = _uploadedProductItemService.ExistsAnyInvalidItemByUserId(_userPrincipal.Id);


            if (existsAnyInvalidItem)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت نهایی، آیتم های بارگذاری شده دارای خطا می باشد"
                };
            }


            var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();

            int? res = null;

            bool isLms = _userPrincipal.ApplicationId == lmsApplicationId;

            if (isLms)
            {
                res = await _uploadedProductItemService.AddDataFromSimpleUploadedProductItemAsync(
                    _userPrincipal.ApplicationId != 0
                        ? _userPrincipal.ApplicationId
                        : _applicationConfiguration.ApplicationId, _userPrincipal.Id, companyId.Value);
            }
            else
            {
                res = await _productService.AddDataFromUploadedProductItemAsync(
                    _userPrincipal.ApplicationId != 0 ?
                        _userPrincipal.ApplicationId
                        : _applicationConfiguration.ApplicationId, _userPrincipal.Id, companyId.Value);
            }



            if (res is > 0)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "با موفقیت بارگذاری شد"
                };
            }

            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در ثبت نهایی"
            };
        }


        /// <summary>
        /// فایل بارگذاری شده محصولات را بر می گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        ///
        ///
        /// 
        [HttpGet("GetTransactionFiles")]
        [YashilAuthorization(ActionName = "GetTransactionFiles")]
        public async Task<LoadResult> GetTransactionFilesAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var transactionFiles = _appDocumentService.GetObjectDocumentsByUserId(Enum.GetName(AppEntityEnum.UploadedProductItem), _userPrincipal.Id,
                (int)DocumentTypeEnum.UploadProductItem);

            return await DataSourceLoader.LoadAsync(transactionFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند این متد مقدار
        ///  true
        ///  بر می گرداند در غیر اینصورت مقدار
        ///  false
        ///  بر می گرداند
        /// </summary>
        /// <returns></returns>
        [HttpPost("CheckExistsProductsSoldAndRenamedInUploadedProductsAsync")]
        [YashilAuthorization(ActionName = "CheckExistsProductsSoldAndRenamedInUploadedProducts")]
        public async Task<bool> CheckExistsProductsSoldAndRenamedInUploadedProductsAsync(int companyId)
        {
            var res = await _uploadedProductItemService.CheckExistsProductsSoldAndRenamedInUploadedProductsAsync
                (_userPrincipal.Id, _applicationConfiguration.ApplicationId, companyId);
            return res;
        }
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند .
        ///  آن محصولات را بر می  گرداند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductsSoldAndRenamedInUploadedProductsAsync")]
        [YashilAuthorization(ActionName = "GetProductsSoldAndRenamedInUploadedProducts")]
        public async Task<LoadResult> GetProductsSoldAndRenamedInUploadedProductsAsync(CustomDataSourceLoadOptions loadOptions, int companyId)
        {
            var uploadedProductItems = _uploadedProductItemService.GetProductsSoldAndRenamedInUploadedProducts
                (_userPrincipal.Id, _applicationConfiguration.ApplicationId, companyId);
            return await DataSourceLoader.LoadAsync(uploadedProductItems.ProjectTo<UploadProductListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }

}
