using System;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using System.Collections.Generic;
using System.Drawing;
using Aspose.Cells;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Enums;
using Yashil.Core;
using YashilDms.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl;

public class HekmatPaymentGatewayTransactionBulkRequestService : GenericService<HekmatPaymentGatewayTransactionBulkRequest, int>, IHekmatPaymentGatewayTransactionBulkRequestService
{
    private readonly IHekmatPaymentGatewayTransactionBulkRequestRepository _hekmatPaymentGatewayTransactionBulkRequestRepository;

    private readonly IHekmatPaymentGatewayTransactionBulkRequestDetailService _hekmatPaymentGatewayTransactionBulkRequestDetailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppDocumentService _appDocumentService;
    private readonly ISqlBulkCopyExcelToDb _sqlBulkCopyExcelToDb;
    private readonly IUserPrincipal _userPrincipal;
    private readonly IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestService> _logger;

    public HekmatPaymentGatewayTransactionBulkRequestService(IUnitOfWork unitOfWork,
        IHekmatPaymentGatewayTransactionBulkRequestRepository hekmatPaymentGatewayTransactionBulkRequestRepository,
        IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
        IAppDocumentService appDocumentService, ISqlBulkCopyExcelToDb sqlBulkCopyExcelToDb,
        IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestService> logger,
        IHekmatPaymentGatewayTransactionBulkRequestDetailService hekmatPaymentGatewayTransactionBulkRequestDetailService)
        : base(unitOfWork, hekmatPaymentGatewayTransactionBulkRequestRepository, userPrincipal, eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _hekmatPaymentGatewayTransactionBulkRequestRepository = hekmatPaymentGatewayTransactionBulkRequestRepository;
        _userPrincipal = userPrincipal;
        _appDocumentService = appDocumentService;
        _sqlBulkCopyExcelToDb = sqlBulkCopyExcelToDb;
        _logger = logger;
        _hekmatPaymentGatewayTransactionBulkRequestDetailService = hekmatPaymentGatewayTransactionBulkRequestDetailService;
    }


    public IQueryable<HekmatPaymentGatewayTransactionBulkRequest> GetByCustomFilter(int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId)
    {
        return _hekmatPaymentGatewayTransactionBulkRequestRepository.GetByCustomFilter(parentId, hekmatPaymentGatewayTransactionBulkRequestCategoryId);
    }
    /// <summary>
    /// ست کردن استایل text wrap و طول مناسب
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="columnName"></param>
    /// <param name="width"></param>
    private void SetStyleForTextWrapperColumns(Workbook workbook,
        SampleTransactionFileInfo.SampleTransactionFileColumnName columnName,
        int width
    )
    {
        var column = workbook.Worksheets[0].Cells
            .Columns[
                SampleTransactionFileInfo.Instance.GetIndexOfSampleTransactionFileItemColumn
                    (columnName)];

        column.Width = width;
        column.Style.IsTextWrapped = true;
        var columnStyle = workbook.CreateStyle();
        columnStyle.IsTextWrapped = true;
        columnStyle.Number = 49;
        var descriptionColumnStyleFlag = new StyleFlag
        {
            WrapText = true
        };
        column.ApplyStyle(columnStyle, descriptionColumnStyleFlag);
    }

    /// <summary>
    /// ساخت WorkBook
    /// </summary>
    /// <param name="questionColumnsNames">نام ستون</param>
    /// <returns></returns>
    public Workbook CreateWorkBook(List<string> questionColumnsNames)
    {
        Workbook workbook = new Workbook();
        Worksheet worksheet = workbook.Worksheets[0];
        worksheet.Name = "نمونه فایل اکسل تراکنش ها حکمت";
        worksheet.DisplayRightToLeft = true;
        // فرمت سلول های اکسل را Text می کند
        Style formatStyle = worksheet.Cells.GetCellStyle(0, 0);
        // 49 = TextFormat
        formatStyle.Number = 49;
        worksheet.Cells.ApplyStyle(formatStyle, new StyleFlag { All = true });

        #region ست کردن استایل برای ستون 
        SetStyleForTextWrapperColumns(workbook, SampleTransactionFileInfo.SampleTransactionFileColumnName.CardNumber, 50);
        SetStyleForTextWrapperColumns(workbook, SampleTransactionFileInfo.SampleTransactionFileColumnName.NationalCode, 30);
        SetStyleForTextWrapperColumns(workbook, SampleTransactionFileInfo.SampleTransactionFileColumnName.InstallmentCount, 20);
        SetStyleForTextWrapperColumns(workbook, SampleTransactionFileInfo.SampleTransactionFileColumnName.Price, 20);
        #endregion

        var style = new CellsFactory().CreateStyle();
        style.BackgroundColor = Color.Yellow;
        style.ForegroundColor = Color.Yellow;
        style.Pattern = BackgroundType.Solid;
        style.IsTextWrapped = true;
        style.Font.IsBold = true;
        style.VerticalAlignment = TextAlignmentType.Center;
        style.HorizontalAlignment = TextAlignmentType.Center;

        var i = 0;
        foreach (var customColumnName in questionColumnsNames)
        {
            worksheet.Cells[0, i].PutValue(customColumnName);
            worksheet.Cells[0, i].SetStyle(style, new StyleFlag { All = true });
            i++;
        }

        worksheet.FreezePanes(1, 0, 1, 0);
        return workbook;
    }
    /// <summary>
    /// ساخت نمومه تراکنش فایل
    /// </summary>
    /// <returns></returns>
    public MemoryStream SampleTransactionFileToWorkBook()
    {
        var sampleUserFileInfo =
             SampleTransactionFileInfo.Instance.SampleTransactionFileColumns;

        var sampleUserFileWorkBook = CreateWorkBook
        (sampleUserFileInfo.
            Select(x => x.Value.Name).ToList());


        var ms = new MemoryStream();
        sampleUserFileWorkBook.Save(ms, SaveFormat.Xlsx);
        ms.Seek(0, SeekOrigin.Begin);

        return ms;
    }

    public async Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile transactionFile, int hekmatTransactionBulkRequestCategoryId)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var addAppDocumentResult = await AddAppDocumentAsync(transactionFile,
                DocumentTypeEnum.HekmatPaymentGatewayTransactionBulkRequest);

            if (!addAppDocumentResult.isSuccess)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = addAppDocumentResult.message
                };
            }
            var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);
            // فایل بارگذاری شده را باز میکنیم و اطلاعات مرکز آن را میخوانیم
            string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, filePath);
            var workbook = new Workbook(fileLocation);
            var worksheet = workbook.Worksheets[0];
            if (worksheet.Cells.Rows.Count <= 1)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فایل اکسل خالی می باشد لطفا فایل اکسل را مقدار دهی نمایید و مجددا بارگذاری کنید."
                };
            }

            //مقادیر ستون نام کاربری را trim() می کنیم
            var countOfRows = worksheet.Cells.Rows.Count;
            for (int row = 1; row <= countOfRows; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    worksheet.Cells[row, 1].Value = worksheet.Cells[row, 1].Value.ToString()?.Trim().ToLower().Replace(" ", "");
                }
            }
            // دریافت ستون ها 
            var uploadedTransactionColumns = SampleTransactionFileInfo.Instance.SampleTransactionFileColumns
                .Where(x => x.Value.AllowUpload)
                .Select(x => x.Key).ToArray();


            var hekmatPaymentGatewayTransactionBulkRequest = await AddAsync(
                new HekmatPaymentGatewayTransactionBulkRequest
                {
                    Title = transactionFile.FileName,
                    HekmatPaymentGatewayTransactionBulkRequestCategoryId = hekmatTransactionBulkRequestCategoryId,
                    CreateBy = _userPrincipal.Id,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    AccessLevelId = (int)AccessLevelEnum.Normal

                },true);
            var customColumn = new Dictionary<string, object>()
            {
                { "HekmatPaymentGatewayTransactionBulkRequestId",hekmatPaymentGatewayTransactionBulkRequest.Id}
            };
            // درج در جدول UploadedQuestion
            var res = await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync
            ("pay.HekmatPaymentGatewayTransactionBulkRequestDetail", filePath,
                0, uploadedTransactionColumns,
                addAppDocumentResult.appDocument.Id, 0, true, customColumn);

            if (res)
            {
                await transaction.CommitAsync();
                return new ResultStatus<int>
                {
                    EntityId = hekmatPaymentGatewayTransactionBulkRequest.Id ,
                    IsSuccess = true
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا در بارگذاری فایل اکسل   lms"
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "pay.HekmatPaymentGatewayTransactionBulkRequestDetail",
                $"بروز خطا در فایل بارگذاری lms ", e);
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا در بارگذاری فایل اکسل  lms"
            };
        }

    }

    public Task<int> GetInvalidUploadedHekmatTransactionBulkCountAsync(int userId)
    {
        return _hekmatPaymentGatewayTransactionBulkRequestRepository
            .GetInvalidUploadedHekmatTransactionBulkCountAsync(userId);
    }

    public async Task DeleteStatusNotDeterminedBulkRequestWithDetailAsync(int userId)
    {

        var hekmatBulkRequestId = await
            _hekmatPaymentGatewayTransactionBulkRequestDetailService
                .GetTransactionBulkRequestDetailStatusNotDeterminedByUserId(userId)
                .Select(x => x.HekmatPaymentGatewayTransactionBulkRequestId)
                .FirstOrDefaultAsync();

        await _hekmatPaymentGatewayTransactionBulkRequestDetailService.DeleteStatusNotDeterminedAsync(userId);

        await _hekmatPaymentGatewayTransactionBulkRequestRepository
            .DeleteByHekmatPaymentGatewayTransactionBulkRequestAsync(hekmatBulkRequestId);
    }

    public Task<int> AddHekmatPaymentGatewayTransactionFromBulkRequestAsync(
        int hekmatPaymentGatewayTransactionBulkRequestId,
        int userId)
    {
        return _hekmatPaymentGatewayTransactionBulkRequestRepository.AddHekmatPaymentGatewayTransactionFromBulkRequestAsync
         (hekmatPaymentGatewayTransactionBulkRequestId,
             userId);
    }

    private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile,
        DocumentTypeEnum documentTypeEnum)
    {
        return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null,
            (int)AppEntityEnum.HekmatPaymentGatewayTransactionBulkRequest, (int)documentTypeEnum,
            null, (int)AppEntityEnum.HekmatPaymentGatewayTransactionBulkRequest, formFile);
    }
}

public class SampleTransactionFileInfo
{
    /// <summary>
    /// نام ستون های سوالات چهار گزینه ای
    /// </summary>
    public enum SampleTransactionFileColumnName
    {
        /// <summary>
        /// شماره کارت حکمت
        /// </summary>
        CardNumber,
        /// <summary>
        /// کد ملی
        /// </summary>
        NationalCode,
        /// <summary>
        /// مبلغ به ریال
        /// </summary>
        Price,
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        InstallmentCount,

    }
    private static SampleTransactionFileInfo _sampleTransactionFileInfo;
    public static SampleTransactionFileInfo Instance =>
        _sampleTransactionFileInfo ??= new SampleTransactionFileInfo();
    /// <summary>
    /// جزئیات هر ستون
    /// </summary>
    public class ColumnDetail
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
    /// <summary>
    /// ستون های اکسل کاربران 
    /// </summary>
    public List<KeyValuePair<string, ColumnDetail>> SampleTransactionFileColumns { get; }


    private SampleTransactionFileInfo()
    {
        SampleTransactionFileColumns =
            new List<KeyValuePair<string, ColumnDetail>>
            {
                new(SampleTransactionFileColumnName.CardNumber.ToString(),new ColumnDetail {Name = "شماره کارت حکمت",AllowUpload = true}),
                new(SampleTransactionFileColumnName.NationalCode.ToString(),new ColumnDetail {Name = "کد ملی",AllowUpload = true}),
                new(SampleTransactionFileColumnName.Price.ToString(),new ColumnDetail {Name = "مبلغ به ریال",AllowUpload = true}),
                new(SampleTransactionFileColumnName.InstallmentCount.ToString(),new ColumnDetail {Name = "تعداد اقساط",AllowUpload = true}),


            };

    }
    public int GetIndexOfSampleTransactionFileItemColumn(SampleTransactionFileColumnName columnName)
    {
        return SampleTransactionFileColumns.IndexOf(SampleTransactionFileColumns.FirstOrDefault(x => x.Key == columnName.ToString()));
    }

}