using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using Aspose.Cells;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilUserManagement.Core.Services;
using Yashil.Common.Infrastructure.AppEvents;
using YashilCommerce.Infrastructure.RepositoryImpl;

namespace YashilCommerce.Infrastructure.ServiceImpl;

public class UploadedProductItemService : GenericService<UploadedProductItem, int>, IUploadedProductItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadedProductItemRepository _uploadedProductItemRepository;
    private readonly IAppDocumentService _appDocumentService;
    private readonly IYashilLogger<object> _logger;
    private readonly ISqlBulkCopyExcelToDb _iSqlBulkCopyExcelToDb;
    private readonly ICompanyService _companyService;
    private readonly ISaleConfigurationService _saleConfigurationService;
    private readonly IAdditionalUserPropService _additionalUserPropService;
    private readonly IUserPrincipal _userPrincipal;
    private readonly IVocherTypeService _vocherTypeService;
    private readonly ICategoryService _categoryService;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IAppConfigService _appConfigService;
    private readonly IProductGroupService _productGroupService;
    private readonly IConfiguration _configuration;
    private readonly int _lmsApplicationId;
    private readonly IProductService _productService;
    private readonly IProductSalesPlanningService _productSalesPlanningService;
    private readonly IEventPublisher _eventPublisher;

    public UploadedProductItemService(IUnitOfWork unitOfWork,
        IUploadedProductItemRepository uploadedProductItemRepository,
        IAppDocumentService appDocumentService, IYashilLogger<object> logger,
        ISqlBulkCopyExcelToDb sqlBulkCopyExcelToDb,
        IVocherTypeService vocherTypeService,
        IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
        ICompanyService companyService,
        ISaleConfigurationService saleConfigurationService,
        IAdditionalUserPropService additionalUserPropService,
        ICategoryService categoryService,
        ApplicationConfiguration applicationConfiguration,
        IAppConfigService appConfigService, IProductGroupService productGroupService, IConfiguration configuration, IProductService productService, IProductSalesPlanningService productSalesPlanningService)
        : base(unitOfWork, uploadedProductItemRepository, userPrincipal, eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _uploadedProductItemRepository = uploadedProductItemRepository;
        _appDocumentService = appDocumentService;
        _logger = logger;
        _iSqlBulkCopyExcelToDb = sqlBulkCopyExcelToDb;
        _userPrincipal = userPrincipal;
        _eventPublisher = eventPublisher;
        _companyService = companyService;
        _saleConfigurationService = saleConfigurationService;
        _additionalUserPropService = additionalUserPropService;
        _categoryService = categoryService;
        _applicationConfiguration = applicationConfiguration;
        _appConfigService = appConfigService;
        _productGroupService = productGroupService;
        _configuration = configuration;
        _productService = productService;
        _productSalesPlanningService = productSalesPlanningService;
        _vocherTypeService = vocherTypeService;
        _lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();
    }

    /// <summary>
    /// فایل ارسال شده کاربر را در مسیر مستند ریخته 
    /// و نام مسیر و شماره اندیس شیت فایل اکسل و نام جدول برای درج دیتا به
    /// همراه نام ستون های جدول را به متد درج دیتا به صورت 
    /// Bulk
    /// ارسال میکند
    /// </summary>
    /// <param name="productTransactionFile">فایل ارسال شده</param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public async Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile productTransactionFile, int companyId)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync();


        var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();

        var uploadedProductItemInfoInstance =
            new UploadedProductItemInfo(_userPrincipal.ApplicationId, lmsApplicationId);

        // ستون هایی را بارگذاری می کنیم که اجازه آپلود داشته باشند.
        var productColumns = uploadedProductItemInfoInstance.ProductColumns
            .Where(x => x.Value.AllowUpload)
            .Select(x => x.Key).ToArray();

        try
        {
            var addAppDocumentResult = await AddAppDocumentAsync(productTransactionFile, DocumentTypeEnum.UploadProductItem);

            var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);

            // فایل بارگذاری شده را باز میکنیم و اطلاعات مرکز آن را میخوانیم
            string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, filePath);

            var workbook = new Workbook(fileLocation);

            var worksheet = workbook.Worksheets[0];

            // خواندن مقدار آیدی مرکز
            var companyIdCellValue = worksheet.Cells[UploadedProductItemInfo.CompanyIdPosition].Value;

            if (companyIdCellValue == null)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فایل مقادیر وارد شده در فایل اکسل صحیح نیست، لطفا مقادیر هدر اکسل را تغییر ندهید"
                };
            }


            var productColumnsCount = uploadedProductItemInfoInstance.ProductColumns
                .Count(x => x.Value.AllowUpload);

            var productNotAllowUploadColumnsCount = uploadedProductItemInfoInstance.ProductColumns
                .Count(x => !x.Value.AllowUpload);

            var rng = worksheet.Cells.MaxDisplayRange;

            var excelDataArray = worksheet.Cells.ExportArray(1, rng.FirstColumn, 1, rng.ColumnCount);

            if (productColumnsCount != excelDataArray.Length - productNotAllowUploadColumnsCount)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی تعداد ستون های فایل اکسل بارگذاری شده اشتباه می باشد، لطفا از فایل نمونه استفاده کنید. "
                };
            }
            // تبدیل مقدار عددی مرکز
            var canConvertToInt = Int32.TryParse(companyIdCellValue.ToString(), out int companyIdInt);

            if (!canConvertToInt)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message =
                        "کاربر گرامی فایل اکسل دارای مقادیر صحیحی نمی باشد، لطفا مقادیر هدر اکسل را تغییر ندهید"
                };
            }
            if (companyIdInt != companyId)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فایل بارگذاری شده مربوط به مرکز انتخابی نمی باشد"
                };
            }
            // بررسی دسترسی کاربر لاگین شده به آیدی مرکز موجود در فایل اکسل
            var additionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);

            if (additionalUserProp.CompanyId != null && additionalUserProp.CompanyId != companyIdInt)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی شما دسترسی لازم جهت بارگذاری فایل برای این مرکز را ندارید"
                };
            }





            var res = await _iSqlBulkCopyExcelToDb
                .BulkInsertFromExcelOrCsvAsync("shp.UploadedProductItem",
                    filePath,
                0,
                    productColumns,
                    addAppDocumentResult.appDocument.Id);


            await transaction.CommitAsync();

            if (res)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = true
                };
            }

            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا در بارگذاری فایل اکسل محصولات"
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "shp.UploadedProductItem",
                $"بروز خطا در فایل بارگذاری و تعریف فروش محصولات ", e);
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا در بارگذاری فایل اکسل محصولات"
            };
        }
    }

    public Task<int> GetInvalidUploadedProductItemsCountAsync(int userId, int companyId)
    {
        return _uploadedProductItemRepository.GetInvalidUploadedProductItemsCountAsync(userId,
            _userPrincipal.ApplicationId != 0 ?
        _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId,
            UploadedProductItemInfo.NumberGroupSeparator, companyId);
    }

    /// <summary>
    /// رکورد های قبلی بارگذاری شده توسط کاربر را حذف میکند
    /// </summary>
    /// <param name="userId"></param>
    public Task DeleteByUserIdAsync(int userId)
    {
        return _uploadedProductItemRepository.DeleteByUserIdAsync(userId);
    }

    ///// <summary>
    ///// رکورد های مربوط به کاربر را بر می گرداند
    ///// </summary>
    ///// <param name="userId">کاربر درج کننده</param>
    ///// <returns></returns>
    public IQueryable<UploadedProductItem> GetByUserId(int userId, bool showInvalidItem)
    {
        return _uploadedProductItemRepository.GetByUserId(userId, showInvalidItem);
    }
    /// <summary>
    /// بررسی میکند که کاربر آیتم نامعتبر دارد یا خیر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <returns></returns>
    public bool ExistsAnyInvalidItemByUserId(int userId)
    {
        return GetByUserId(userId, true).Any();
    }

    /// <summary>
    /// درج سند برای اکسل محصولات
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="documentTypeEnum"></param>
    /// <returns></returns>
    private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile, DocumentTypeEnum documentTypeEnum)
    {
        return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.UploadedProductItem, (int)documentTypeEnum,
            null, (int)AppEntityEnum.UploadedProductItem, formFile);
    }
    /// <summary>
    /// اگر بخواهیم نام ستونی در خروجی اکسل را شخصی سازی کنیم و
    /// از نام ستون پیش فرض استفاده نکند ،
    /// این متد را استفاده می کنیم
    /// </summary>
    /// <param name="productColumns"></param>
    /// <param name="columnName"></param>
    /// <param name="value"></param>
    private void SetColumnCustomName(
        List<KeyValuePair<string, UploadedProductItemInfo.ProductColumnDetail>> productColumns,
        UploadedProductItemInfo.ProductItemColumnName columnName,
        string value)
    {
        productColumns[UploadedProductItemInfo.Instance
            .GetIndexOfProductItemColumn(columnName)].Value.Name = value;
    }

    /// <summary>
    /// سورس لیست محصولات  را در 
    /// MemoryStream 
    /// ریخته و بر می گرداند
    /// </summary>
    /// <returns></returns>
    public async Task<MemoryStream> CopyProductListToWorkBook(List<ProductDetailViewModel> source, int companyId, int? categoryGroupId)
    {
        var productCount = source.Count == 0 ? 1 : source.Count;

        var uploadedProductItemInfoInstance = new UploadedProductItemInfo(_userPrincipal.ApplicationId, _lmsApplicationId);

        var productColumns = uploadedProductItemInfoInstance.ProductColumns;


        bool isLmsApplication = _lmsApplicationId == _userPrincipal.ApplicationId;

        if (!isLmsApplication)
        {
            SetColumnCustomName(productColumns, UploadedProductItemInfo.ProductItemColumnName.BookBasePrice,
                await _appConfigService
                    .GetStringValueAsync(
                        ApplicationConfigKey.YashilCommerceProductsTypeWithOutTaxBasePriceColumnNameForExportExcel));

            SetColumnCustomName(productColumns, UploadedProductItemInfo.ProductItemColumnName.TestBasePrice,
                await _appConfigService
                    .GetStringValueAsync(
                        ApplicationConfigKey.YashilCommerceProductsTypeWithTaxBasePriceColumnNameForExportExcel));

            SetColumnCustomName(productColumns, UploadedProductItemInfo.ProductItemColumnName.BookPriceWithDiscount,
                await _appConfigService
                    .GetStringValueAsync(
                        ApplicationConfigKey.YashilCommerceProductsTypeWithOutTaxPriceWithDiscountColumnNameForExportExcel));

            SetColumnCustomName(productColumns, UploadedProductItemInfo.ProductItemColumnName.TestPriceWithDiscount,
                await _appConfigService
                    .GetStringValueAsync(
                        ApplicationConfigKey.YashilCommerceProductsTypeWithTaxPriceWithDiscountColumnNameForExportExcel));
        }


        // CreateWorkBook 
        var productWorkBook = await CreateWorkBookAsync(productColumns
            .Select(x => x.Value.Name).ToList(), companyId);


        var productMainSheet = productWorkBook.Worksheets[0];


        if (!isLmsApplication)
        {
            #region Create Excel Data Validation

            var validationRangeFirstColumn = 1;
            // ساخت لیست آیتم انتخابی برای نوع بسته که در اکسل قرار میگیرد و کاربر فقط میتواند انتخاب کند
            // مقادیر این لیست در شیت اکسل 
            //validationSource
            //  در سطر اول و ستون اول قرار میگیرد  
            var productTypeList = new List<string>
        {
            await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceProductsTypeWithOutTax),
            await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceProductsTypeWithTax),
            await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceProductsTypeIsGroup)
        };

            CreateDataValidation(
                productWorkBook,
                productTypeList,
                UploadedProductItemInfo.ProductItemColumnName.Type.ToString(),
                1500,
                UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                    .Type),
                ++validationRangeFirstColumn
            );
            // ساخت لیست آیتم انتخابی برای پایه تحصیلی / رشته تحصیلی (دسته اصلی / زیر دسته) که در اکسل قرار میگیرد و کاربر فقط میتواند انتخاب کند
            // مقادیر این لیست در شیت اکسل 
            // validationSource
            //  در سطر اول و ستون دوم قرار میگیرد  
            var categoryList = await _categoryService.GetLowLevelCategoryWithParent(companyId)
                .OrderBy(x => x.ParentTitle).ThenBy(x => x.Title)
                .Select(x => $"{x.ParentTitle}/{x.Title}#{x.Id}").Distinct().ToListAsync();

            if (categoryList.Count > 0)
            {
                CreateDataValidation(
                    productWorkBook,
                    categoryList,
                    UploadedProductItemInfo.ProductItemColumnName.CategoryTitle.ToString(),
                    1500,
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName.CategoryTitle),
                    ++validationRangeFirstColumn
                );
            }
            // در صورتی که محصولات مرکز دارای ووچر باشند 
            // انواع ووچر های مرکز را در لیست نوع ووچر نمایش می دهد تا کاربر از این لیست نوع ووچر را انتخاب کند 
            var voucherTypeTitleList = await _vocherTypeService.GetVocherTypesByCompanyId(companyId)
                .Select(x => x.Title).ToListAsync();
            if (voucherTypeTitleList.Count > 0)
            {
                CreateDataValidation(
                    productWorkBook,
                    voucherTypeTitleList,
                    UploadedProductItemInfo.ProductItemColumnName.VocherTypeTitle.ToString(),
                    1500,
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.VocherTypeTitle),
                    ++validationRangeFirstColumn
                );
            }

            // ساخت لیست آیتم انتخابی برای نوع انتشار محصول که در اکسل قرار میگیرد و کاربر فقط میتواند انتخاب کند
            // مقادیر این لیست در شیت اکسل 
            // validationSource
            //  در سطر اول و ستون چهارم قرار میگیرد  
            var productPublishType = new List<string>
                 {
                     UploadedProductPublisheType.Published,
                     UploadedProductPublisheType.Unpublished
                 };


            CreateDataValidation(
                productWorkBook,
                productPublishType,
                UploadedProductItemInfo.ProductItemColumnName.Published.ToString(),
                1500,
                UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                    .Published),
                ++validationRangeFirstColumn
            );

            // وضعیت نمایش زیر نویس برنامه ریزی دوره
            var productSalePlanningDisplaySubTitleType = new List<string>
                 {
                     DisplaySubTitleType.Display,
                     DisplaySubTitleType.NoDisplay
                 };

            CreateDataValidation(
                productWorkBook,
                productSalePlanningDisplaySubTitleType,
                UploadedProductItemInfo.ProductItemColumnName.ProductSalePlanningDisplaySubTitle.ToString(),
                1500,
                UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                    .ProductSalePlanningDisplaySubTitle),
                ++validationRangeFirstColumn
            );



            #region ProductGroup data validation

            if (categoryGroupId.HasValue)
            {
                var productGroupList = await _productGroupService.GetByCategoryGroupId(categoryGroupId.Value)
                    .Select(x => string.Concat(x.Title, "#", x.Id)).ToListAsync();

                if (productGroupList.Count > 0)
                {
                    CreateDataValidation(productWorkBook, productGroupList,
                        UploadedProductItemInfo.ProductItemColumnName.ProductGroupIdForBook.ToString(), 1500,
                        UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                            .ProductGroupIdForBook), ++validationRangeFirstColumn);

                    CreateDataValidation(productWorkBook, productGroupList,
                        UploadedProductItemInfo.ProductItemColumnName.ProductGroupIdForExam.ToString(), 1500,
                        UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                            .ProductGroupIdForExam), ++validationRangeFirstColumn);

                }
            }

            #endregion

            #endregion
        }
        else
        {
            int validationRangeFirstColumn = 1;

            var categoryList = await _categoryService.GetLowLevelCategoryWithParent(companyId)
                .OrderBy(x => x.ParentTitle).ThenBy(x => x.Title)
                .Select(x => $"{x.ParentTitle}/{x.Title}#{x.Id}").Distinct().ToListAsync();

            if (categoryList?.Count > 0)
            {
                CreateDataValidation(
                    productWorkBook,
                    categoryList,
                    UploadedProductItemInfo.ProductItemColumnName.CategoryTitle.ToString(),
                    1500,
                    uploadedProductItemInfoInstance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName.CategoryTitle),
                    ++validationRangeFirstColumn
                );
            }
            var productPublishType = new List<string>
            {
                UploadedProductPublisheType.Published,
                UploadedProductPublisheType.Unpublished
            };


            CreateDataValidation(
                productWorkBook,
                productPublishType,
                UploadedProductItemInfo.ProductItemColumnName.Published.ToString(),
                1500,
                uploadedProductItemInfoInstance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                    .Published), ++validationRangeFirstColumn
            );

            var productList =
                await _productService.GetCompanyProductListForFillInUploadedProductItemXlsValidationAsync(companyId);

            if (productList?.Count > 0)
            {
                CreateDataValidation(
                    productWorkBook,
                    productList,
                    UploadedProductItemInfo.ProductItemColumnName.Title.ToString(),
                    1500,
                    uploadedProductItemInfoInstance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                        .Title), ++validationRangeFirstColumn
                );
            }

            var representationTitleList = await _productSalesPlanningService
                .GetProductSalesPlanningRepresentationTitleListForFillInUploadedProductItemXlsValidationAsync(
                    companyId);

            if (representationTitleList?.Count > 0)
            {
                CreateDataValidation(
                    productWorkBook,
                    representationTitleList,
                    UploadedProductItemInfo.ProductItemColumnName.Representation.ToString(),
                    1500,
                    uploadedProductItemInfoInstance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                        .Representation), ++validationRangeFirstColumn
                );
            }


        }




        var productColumnKeys = productColumns.Select(x => x.Key).ToArray();
        //پر کردن داده های شیت اکسل با داده های ورودی  

        productMainSheet.Cells.ImportCustomObjects(
            source,
            productColumnKeys,
            false,
            2,
            0,
            productCount,
            true,
            "yyyy/mm/dd",
            true);

        if (_lmsApplicationId != _userPrincipal.ApplicationId)
        {
            //اضافه کردن ستون محاسباتی قیمت کتاب با تخفیف و قیمت آزمون با تخفیف
            await AddPriceWithDiscountFormulaToColumn(productMainSheet, companyId, UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                .ProductItemColumnName.BookBasePrice), UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                .ProductItemColumnName.BookPriceWithDiscount), productCount);


            await AddPriceWithDiscountFormulaToColumn(productMainSheet, companyId, UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                .ProductItemColumnName.TestBasePrice), UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                .ProductItemColumnName.TestPriceWithDiscount), productCount);
        }


        var ms = new MemoryStream();
        productWorkBook.Save(ms, SaveFormat.Xlsx);
        ms.Seek(0, SeekOrigin.Begin);


        return ms;
    }

    private async Task InsertHeaderInfoIntoWorkSheet(Worksheet workSheet, int companyId)
    {
        var row = workSheet.Cells.Rows[0];

        var style = workSheet.Workbook.CreateStyle();
        style.Font.IsBold = true;
        style.BackgroundColor = Color.CornflowerBlue;
        style.ForegroundColor = Color.CornflowerBlue;
        style.HorizontalAlignment = TextAlignmentType.Center;
        style.VerticalAlignment = TextAlignmentType.Center;
        style.Pattern = BackgroundType.Solid;
        style.IsLocked = true;
        var styleFlag = new StyleFlag
        {
            All = true,
        };
        row.ApplyStyle(style, styleFlag);
        workSheet.Cells.SetRowHeight(0, 25);
        var companyTitle = _companyService.GetTitle(companyId);
        workSheet.Cells[UploadedProductItemInfo.CompanyIdPosition].PutValue(companyId);
        workSheet.Cells[UploadedProductItemInfo.CompanyTitlePosition].PutValue($"نام مرکز: {companyTitle}");
        workSheet.Cells.Merge(0, 1, 1, 2);

        bool isLmsApplication = _lmsApplicationId == _userPrincipal.ApplicationId;

        //در صورتی که اپلیکیشن آموزش مجازی آویسا نبود اطلاعات تنظیم فروش و درصد تخفیف
        //اضافه می شوند
        if (!isLmsApplication)
        {
            var activeSaleConfiguration = await _saleConfigurationService.GetActiveSaleConfigurationAsync(companyId, null);
            if (activeSaleConfiguration == null)
            {
                workSheet.Cells[UploadedProductItemInfo.SaleConfigurationToDatePosition]
                    .PutValue($"قرارداد فعالی برای مرکز {companyTitle} وجود ندارد");
            }
            else
            {

                workSheet.Cells[UploadedProductItemInfo.SaleConfigurationToDatePosition].PutValue(
                    $"تاریخ اتمام قرارداد: {DateFunction.GetDateStringFromInt(activeSaleConfiguration.ToDate)}");

                //workSheet.Cells[UploadedProductItemInfo.DiscountPercentagePosition]
                //    .PutValue($"{Decimal.ToInt32(activeSaleConfiguration.DiscountPercentage)}% :درصد تخفیف");

                // TODO Fix This Remove From ProductSalePlanning ****** activeSaleConfiguration.DiscountPercentage
                workSheet.Cells[UploadedProductItemInfo.DiscountPercentagePosition]
                    .PutValue($"درصد تخفیف");
            }
        }


        var companyIdPositionStyle = workSheet.Cells[UploadedProductItemInfo.CompanyIdPosition].GetStyle();
        companyIdPositionStyle.Font.Color = Color.CornflowerBlue;
        workSheet.Cells[UploadedProductItemInfo.CompanyIdPosition].SetStyle(companyIdPositionStyle);
    }


    /// <summary>
    ///  ساخت WorkBook
    /// </summary>
    /// <param name="customColumnNames">نام ستون های دلخواه برای نمایش </param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public async Task<Workbook> CreateWorkBookAsync(List<string> customColumnNames, int companyId)
    {
        Workbook workbook = new Workbook();
        Worksheet worksheet = workbook.Worksheets[0];
        worksheet.Name = "محصولات";
        worksheet.DisplayRightToLeft = true;

        bool isLmsApplication = _lmsApplicationId == _userPrincipal.ApplicationId;

        if (!isLmsApplication)
        {
            #region ست کردن استایل برروی ستون های قیمت
            // ستون هایی که می خواهیم بر روی آن استایل اعمال شود را مشخص می کنیم
            var testBasePriceColumn = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.TestBasePrice)];
            var bookBasePriceColumn = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.BookBasePrice)];
            var deliveryCostColumn = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.DeliveryCost)];
            var bookPriceWithDiscountColumn = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.BookPriceWithDiscount)];
            var testPriceWithDiscount = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.TestPriceWithDiscount)];
            // استایل بر روی همه مقادیر ستون اعمال می شود.
            var styleFlag = new StyleFlag
            {
                //Set All Styles
                All = true
            };
            // استایل را ایجاد میکنیم
            var separatorStyle = workbook.CreateStyle();
            // در اینجا مشخص میکنیم که سه رقم سه رقم اعداد از هم جدا شوند
            separatorStyle.Custom = "#,##0";
            // تعیین جدا کننده
            workbook.Settings.NumberGroupSeparator = UploadedProductItemInfo.NumberGroupSeparator;
            // استایل ایجاد شده را  بر روی ستون های مشخص شده اعمال میکنیم
            testBasePriceColumn.ApplyStyle(separatorStyle, styleFlag);
            bookBasePriceColumn.ApplyStyle(separatorStyle, styleFlag);
            deliveryCostColumn.ApplyStyle(separatorStyle, styleFlag);
            bookPriceWithDiscountColumn.ApplyStyle(separatorStyle, styleFlag);
            testPriceWithDiscount.ApplyStyle(separatorStyle, styleFlag);

            #endregion
        }

        await InsertHeaderInfoIntoWorkSheet(worksheet, companyId);

        if (!isLmsApplication)
        {
            var descriptionColumn = workbook.Worksheets[0].Cells
                .Columns[
                    UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
                        .ProductItemColumnName.Description)];
            descriptionColumn.Width = 50;
            descriptionColumn.Style.IsTextWrapped = true;
            var descriptionColumnStyle = workbook.CreateStyle();
            descriptionColumnStyle.IsTextWrapped = true;
            var descriptionColumnStyleFlag = new StyleFlag
            {
                WrapText = true,
            };
            descriptionColumn.ApplyStyle(descriptionColumnStyle, descriptionColumnStyleFlag);
        }

        var style = new CellsFactory().CreateStyle();

        style.BackgroundColor = Color.Yellow;
        style.ForegroundColor = Color.Yellow;
        style.Pattern = BackgroundType.Solid;
        style.IsTextWrapped = true;
        style.Font.IsBold = true;
        style.VerticalAlignment = TextAlignmentType.Center;
        style.HorizontalAlignment = TextAlignmentType.Center;

        var i = 0;

        foreach (var customColumnName in customColumnNames)
        {
            worksheet.Cells[1, i].PutValue(customColumnName);
            i++;
        }

        worksheet.Cells.Rows[1].ApplyStyle(style, new StyleFlag() { All = true });
        worksheet.FreezePanes(2, 0, 2, 0);

        // اندازه ستون ها را باندازه فضای محتوایی که در خود دارند ست میکنیم
        //worksheet.AutoFitColumns(UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
        //    .ProductItemColumnName.Type), UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(
        //    UploadedProductItemInfo
        //        .ProductItemColumnName.Title));
        //worksheet.AutoFitColumns(UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo
        //    .ProductItemColumnName.CategoryTitle), UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(
        //    UploadedProductItemInfo
        //        .ProductItemColumnName.TestPriceWithDiscount));
        return workbook;
    }



    /// <summary>
    /// اضافه کردن فرمول به ستون های قیمت کتاب با تخفیف و قیمت آزمون با تخفیف
    /// این ستون ها قیمت را بر اساس درصد تخفیف و قیمت پایه آزمون و کتاب محاسبه می کند
    /// و این فرمول را در ستون های قیمت کتاب با تخفیف
    /// و قیمت آزمون با تخفیف قرار می دهد
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="companyId"></param>
    /// <param name="sourceColumnIndex"></param>
    /// <param name="destinationColumnIndex"></param>
    /// <param name="rangeRowNumber"></param>
    /// <returns></returns>
    private async Task AddPriceWithDiscountFormulaToColumn(Worksheet worksheet, int companyId, int sourceColumnIndex, int destinationColumnIndex, int rangeRowNumber)
    {

        var hekmatCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubsHekmatCustomerClubId);
        // دریافت تنظیمات فروش فعال و درصد سود
        var activeSaleConfigurationAttributeDto = await _saleConfigurationService.GetSaleConfigurationAttributeByFilterAsync(hekmatCustomerClubId, companyId, null);

        var discountPercentage = decimal.Zero;

        if (activeSaleConfigurationAttributeDto != null &&
            activeSaleConfigurationAttributeDto.SaleConfigurationAttributeId.HasValue &&
            activeSaleConfigurationAttributeDto.DiscountPercentage.HasValue
            )
        {
            discountPercentage = activeSaleConfigurationAttributeDto.DiscountPercentage.Value;
        }

        //دریافت مقدار سلول درصد تخفیف
        var rowDiscountPercentageCell = worksheet.Cells[2,
            UploadedProductItemInfo.Instance.GetIndexOfProductItemColumn(UploadedProductItemInfo.ProductItemColumnName
                .TbaoCustomerClubDiscountPercentage)];

        // دریافت سلولی که قیمت پایه در آن است
        var firstsSourceBasePriceCell = worksheet.Cells[2,
            sourceColumnIndex];
        // محاسبه فرمول
        var priceWithDiscountFormula =
            $"=IF(ISBLANK({rowDiscountPercentageCell.Name}),{(100 - discountPercentage) / 100}*{firstsSourceBasePriceCell.Name},((100-{rowDiscountPercentageCell.Name})/100)*{firstsSourceBasePriceCell.Name})";
        // دریافت سلولی که میخواهیم محاسبات را در آن انجام دهیم
        var firstCellPriceWithDiscount = worksheet.Cells[2, destinationColumnIndex];
        // اعمال محاسبات در کل ستونی که میخواهیم در آن محاسبات صورت گیرد
        firstCellPriceWithDiscount.SetSharedFormula(priceWithDiscountFormula, rangeRowNumber, 1);
    }


    /// <summary>
    /// برای ساخت  لیست انتخابی در شیت اصلی اکسل از این متد استفاده میشود
    /// ابتدا یک شیت جدید برای اکسل تعریف میکنیم تا سورس لیست انتخابی در آن قرار گیرید
    /// سپس رنج لیست انتخابی را با محل قرار گیری آن در شیت جدید تعریف می کنیم و رنج را با سورس لیست انتخابی مقدار دهی میکنیم
    /// سپس محل قرار گیری لیست انتخابی در شیت اصلی اکسل را مشخص می کنیم
    /// </summary>
    /// <param name="mainWorkbook">اکسل اصلی</param>
    /// <param name="dataValidationSource">سورس مربوط به لیست انتخابی</param>
    /// <param name="rangeName">نام رنج مربوط به لیست انتخابی</param>
    /// <param name="validationRangeTotalColumn"></param>
    /// <param name="startRowInMainWorkbook">شروع سطر مربوط محل قرار گیری رنج در شیت اصلی</param>
    /// <param name="endRowInMainWorkbook">پایان سطر مربوط محل قرار گیری رنج در شیت اصلی</param>
    /// <param name="startColumnInMainWorkbook">شروع ستون مربوط محل قرار گیری رنج در شیت اصلی</param>
    /// <param name="endColumnInMainWorkbook">پایان ستون مربوط محل قرار گیری رنج در شیت اصلی</param>
    /// <param name="validationRangeFirstColumn"></param>
    /// <param name="validationRangeFirstRow"></param>
    public void CreateDataValidation(Workbook mainWorkbook,
        List<string> dataValidationSource,
        string rangeName,
        int endRowInMainWorkbook,
        int startColumnInMainWorkbook,
        int validationRangeFirstColumn,
        int validationRangeFirstRow = 1,
        int validationRangeTotalColumn = 1,
        int startRowInMainWorkbook = 1,
        int? endColumnInMainWorkbook = null)
    {
        endColumnInMainWorkbook ??= startColumnInMainWorkbook;
        int totalRow = dataValidationSource.Count;

        var mainWorksheet = mainWorkbook.Worksheets[0];
        //ساخت شیت اکسل برای آیتم های لیست باکس در شیت اصلی محصولات مرکز
        var sheetNumber = mainWorkbook.Worksheets.Add();
        Worksheet validationSourceSheet = mainWorkbook.Worksheets[sheetNumber];
        validationSourceSheet.IsVisible = false;
        validationSourceSheet.Protect(ProtectionType.All, "aspose", null);
        // ساخت رنج برای لیست انتخابی
        var range = validationSourceSheet.Cells.CreateRange(validationRangeFirstRow, validationRangeFirstColumn,
            totalRow, validationRangeTotalColumn);
        range.Name = rangeName;
        for (var i = 0; i < dataValidationSource.Count; i++)
        {
            range[i, 0].PutValue(dataValidationSource[i]);
        }

        // قرار دادن لیست انتخابی در شیت اصلی اکسل
        ValidationCollection dataValidation = mainWorksheet.Validations;
        SetDataValidationInMainSheet(
            dataValidation,
            startRowInMainWorkbook,
            endRowInMainWorkbook,
            startColumnInMainWorkbook,
            endColumnInMainWorkbook.Value,
            rangeName);
    }

    /// <summary>
    /// تنظیمات مربوط به لیست آیتم
    /// </summary>
    /// <param name="validations">سورس </param>
    /// <param name="endColumnInMainWorkbook"></param>
    /// <param name="formula1">نام rang که به در فرمول قرار میگیرد</param>
    /// <param name="startRowInMainWorkbook"></param>
    /// <param name="endRowInMainWorkbook"></param>
    /// <param name="startColumnInMainWorkbook"></param>
    public void SetDataValidationInMainSheet(
        ValidationCollection validations,
        int startRowInMainWorkbook,
        int endRowInMainWorkbook,
        int startColumnInMainWorkbook,
        int endColumnInMainWorkbook,
        string formula1)
    {
        // Create Cell Area
        var ca = new CellArea
        {
            StartRow = startRowInMainWorkbook,
            EndRow = endRowInMainWorkbook,
            StartColumn = startColumnInMainWorkbook,
            EndColumn = endColumnInMainWorkbook
        };
        // Create a new validation to the validations list.
        var validation = validations[validations.Add(ca)];

        // Set the validation type.
        validation.Type = ValidationType.List;

        // Set the operator.
        validation.Operator = OperatorType.None;

        // Set the in cell drop down.
        validation.InCellDropDown = true;

        // Set the formula1.
        validation.Formula1 = $"={formula1}";

        // Enable it to show error.
        validation.ShowError = true;

        // Set the alert type severity level.
        validation.AlertStyle = ValidationAlertType.Stop;

        // Set the error title.
        validation.ErrorTitle = "خطا";

        // Set the error message.
        validation.ErrorMessage = "کاربر گرامی لطفا از آیتم های لیست شده یک مورد را انتخاب نمایید";


        // Specify the validation area.
        CellArea area;
        area.StartRow = startRowInMainWorkbook;
        area.EndRow = endRowInMainWorkbook;
        area.StartColumn = startColumnInMainWorkbook;
        area.EndColumn = endColumnInMainWorkbook;

        // Add the validation area.
        validation.AddArea(area);
    }
    /// <summary>
    /// دیتای فایل اکسل بارگذاری شده کاربر که دارای خطا می باشد به همراه سند آن را حذف می کند  
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="deleteInvalidItem">حذف آیتم نامعتبر</param>
    public async void DeleteUploadedProductItemWithAppDocument(int userId, bool deleteInvalidItem)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync();
        var uploadedProductItem = GetByUserId(userId, deleteInvalidItem).FirstOrDefault();
        try
        {
            if (uploadedProductItem != null)
            {
                var appDocumentId = uploadedProductItem.AppDocumentId;

                await DeleteByUserIdAsync(userId);
                await SaveChangeAsync();
                await _appDocumentService.DeleteAsync(appDocumentId);
                await _appDocumentService.SaveChangeAsync();
            }
            await transaction.CommitAsync();

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "shp.UploadedProductItem",
                $"بروز خطا در حذف دیتای فایل اکسل بارگذاری شده و سند آن ", ex);
        }


    }
    /// <summary>
    ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
    ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاذی جدید تغییر می کند این متد مقدار
    ///  true
    ///  بر می گرداند در غیر اینصورت مقدار
    ///  false
    ///  بر می گرداند
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="applicationId">شناسه برنامه</param>
    /// <param name="companyId">شناسه مرکز</param>
    public Task<bool> CheckExistsProductsSoldAndRenamedInUploadedProductsAsync(int userId, int applicationId, int companyId)
    {
        return _uploadedProductItemRepository.CheckExistsProductsSoldAndRenamedInUploadedProductsAsync(userId, applicationId, companyId);
    }
    /// <summary>
    ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
    ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند .
    ///  آن محصولات را بر می  گرداند
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="applicationId">شناسه برنامه</param>
    /// <param name="companyId">شناسه مرکز</param>
    /// <returns></returns>
    public IQueryable<UploadedProductItem> GetProductsSoldAndRenamedInUploadedProducts(int userId, int applicationId, int companyId)
    {
        return _uploadedProductItemRepository.GetProductsSoldAndRenamedInUploadedProducts(userId, applicationId, companyId);
    }

    /// <summary>
    ///دریافت تعداد ایتم های برنامه ریزی دوره نامعتبر که
    /// از طریق اکسل بارگذاری شده اند مربوط به سامانه ی
    /// Lms
    /// </summary>
    /// <returns></returns>
    public Task<int> GetInvalidUploadedPspItemsCountAsync(int userId, int companyId)
    {
        return _uploadedProductItemRepository.GetInvalidUploadedSimpleProductItemsCountAsync(userId, companyId);
    }

    public Task<int> AddDataFromUploadedProductItemAsync(int applicationId, int userId, int companyId)
    {
        var task = _uploadedProductItemRepository.AddDataFromSimpleUploadedProductItemAsync(applicationId, userId, companyId);

        //using Fire and forger approach
        _ = _eventPublisher.EntityInsertedAsync<ProductSalesPlanning, int>(null);

        return task;
    }

    public Task<int> GetInvalidUploadedSimpleProductItemsCountAsync(int userId, int companyId)
    {
        return _uploadedProductItemRepository.GetInvalidUploadedSimpleProductItemsCountAsync(userId, companyId);
    }

    public Task<int> AddDataFromSimpleUploadedProductItemAsync(int applicationId, int userId, int companyId)
    {
        return _uploadedProductItemRepository.AddDataFromSimpleUploadedProductItemAsync(applicationId, userId, companyId);
    }
}

public class UploadedProductItemInfo
{
    public enum ProductItemColumnName
    {
        Type,
        Code,
        Title,
        Description,
        CategoryTitle,
        BookBasePrice,
        TestBasePrice,
        DeliveryCost,
        TbaoCustomerClubDiscountPercentage,
        OtherCustomerClubDiscountPercentage,
        Published,
        ProductSalePlanningDisplaySubTitle,
        BookPriceWithDiscount,
        TestPriceWithDiscount,
        VocherTypeTitle,
        ProductGroupIdForBook,
        ProductGroupIdForExam,
        FullDescription,
        StartDate,
        EndDate,
        MaxCapacity,
        Representation,
        StudentUserName,
        TeacherUserName
    }

    private static UploadedProductItemInfo _uploadedProductItemInfoInstance;

    public static char NumberGroupSeparator = ',';



    public static UploadedProductItemInfo Instance =>
        _uploadedProductItemInfoInstance ??= new UploadedProductItemInfo(null, null);
    /// <summary>
    /// جزئیات هر ستون
    /// </summary>
    public class ProductColumnDetail
    {
        /// <summary>
        /// نام ستون
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// اجازه بارگذاری 
        /// </summary>
        public bool AllowUpload { get; set; }
    }
    public List<KeyValuePair<string, ProductColumnDetail>> ProductColumns { get; }

    public UploadedProductItemInfo(int? currentUserApplicationId, int? lmsApplicationId)
    {

        ProductColumns = (currentUserApplicationId.HasValue && lmsApplicationId.HasValue &&
                          currentUserApplicationId == lmsApplicationId)
            ? new List<KeyValuePair<string, ProductColumnDetail>>
            {
                new(ProductItemColumnName.Code.ToString(),
                    new ProductColumnDetail { Name = "کد ", AllowUpload = true }),
                new(ProductItemColumnName.Title.ToString(),
                    new ProductColumnDetail { Name = "عنوان ", AllowUpload = true }),
                new(ProductItemColumnName.CategoryTitle.ToString(),
                    new ProductColumnDetail { Name = "عنوان دسته اصلی", AllowUpload = true }),
                new(ProductItemColumnName.Published.ToString(),
                    new ProductColumnDetail { Name = "وضعیت انتشار", AllowUpload = true }),
                new(ProductItemColumnName.Representation.ToString(),
                    new ProductColumnDetail { Name = "گروه علمی", AllowUpload = true }),
                new(ProductItemColumnName.StudentUserName.ToString(),
                    new ProductColumnDetail { Name = "نام کاربری دانشجو", AllowUpload = true }),
                new(ProductItemColumnName.TeacherUserName.ToString(),
                    new ProductColumnDetail { Name = "نام کاربری استاد", AllowUpload = true }),
                new(ProductItemColumnName.FullDescription.ToString(),
                    new ProductColumnDetail { Name = "توضیحات کامل", AllowUpload = true }),
                new(ProductItemColumnName.StartDate.ToString(),
                    new ProductColumnDetail { Name = "تاریخ شروع", AllowUpload = true }),
                new(ProductItemColumnName.EndDate.ToString(),
                    new ProductColumnDetail { Name = "تاریخ پایان", AllowUpload = true }),
                new(ProductItemColumnName.MaxCapacity.ToString(),
                    new ProductColumnDetail { Name = "حداکثر ظرفیت", AllowUpload = true }),
            }
            : new List<KeyValuePair<string, ProductColumnDetail>>
            {


                new(ProductItemColumnName.Type.ToString(),
                    new ProductColumnDetail { Name = "نوع بسته", AllowUpload = true }),
                new(ProductItemColumnName.Code.ToString(),
                    new ProductColumnDetail { Name = "کد بسته", AllowUpload = true }),
                new(ProductItemColumnName.Title.ToString(),
                    new ProductColumnDetail { Name = "عنوان بسته", AllowUpload = true }),
                new(ProductItemColumnName.Description.ToString(),
                    new ProductColumnDetail { Name = "شرح مختصر", AllowUpload = true }),
                new(ProductItemColumnName.CategoryTitle.ToString(),
                    new ProductColumnDetail { Name = "عنوان دسته اصلی - عنوان زیر دسته", AllowUpload = true }),
                new(ProductItemColumnName.BookBasePrice.ToString(),
                    new ProductColumnDetail { Name = "قیمت پایه کتاب ریال", AllowUpload = true }),
                new(ProductItemColumnName.TestBasePrice.ToString(),
                    new ProductColumnDetail { Name = "قیمت پایه آزمون ریال", AllowUpload = true }),
                new(ProductItemColumnName.DeliveryCost.ToString(),
                    new ProductColumnDetail { Name = "هزینه ارسال ریال", AllowUpload = true }),
                new(ProductItemColumnName.TbaoCustomerClubDiscountPercentage.ToString(),
                    new ProductColumnDetail { Name = "درصد تخفیف طرح باقرالعلوم", AllowUpload = true }),
                new(ProductItemColumnName.OtherCustomerClubDiscountPercentage.ToString(),
                    new ProductColumnDetail { Name = "درصد تخفیف سایر باشگاه ها", AllowUpload = true }),
                new(ProductItemColumnName.Published.ToString(),
                    new ProductColumnDetail { Name = "وضعیت انتشار", AllowUpload = true }),
                new(ProductItemColumnName.ProductSalePlanningDisplaySubTitle.ToString(),
                    new ProductColumnDetail { Name = "وضعیت نمایش زیر نویس", AllowUpload = true }),
                new(ProductItemColumnName.VocherTypeTitle.ToString(),
                    new ProductColumnDetail { Name = "نوع ووچر", AllowUpload = true }),
                new(ProductItemColumnName.ProductGroupIdForBook.ToString(),
                    new ProductColumnDetail
                        { Name = "عنوان گروه محصول برای فاکتور و کانورت برای کتاب", AllowUpload = true }),
                new(ProductItemColumnName.ProductGroupIdForExam.ToString(),
                    new ProductColumnDetail
                        { Name = "عنوان گروه محصول برای فاکتور و کانورت برای آزمون", AllowUpload = true }),
                new(ProductItemColumnName.BookPriceWithDiscount.ToString(),
                    new ProductColumnDetail { Name = "قیمت کتاب با تخفیف ریال", AllowUpload = false }),
                new(ProductItemColumnName.TestPriceWithDiscount.ToString(),
                    new ProductColumnDetail { Name = "قیمت آزمون با تخفیف ریال", AllowUpload = false })
            };
    }

    public int GetIndexOfProductItemColumn(ProductItemColumnName columnName)
    {
        return ProductColumns.IndexOf(ProductColumns.FirstOrDefault(x => x.Key == columnName.ToString()));
    }

    public static string CompanyIdPosition = "A1";
    public static string CompanyTitlePosition = "B1";
    public static string SaleConfigurationToDatePosition = "D1";
    public static string DiscountPercentagePosition = "E1";
}