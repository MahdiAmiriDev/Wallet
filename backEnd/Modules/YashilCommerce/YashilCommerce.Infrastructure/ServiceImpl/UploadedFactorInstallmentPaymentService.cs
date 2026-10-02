using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.RepositoryImpl;
using YashilDms.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class UploadedFactorInstallmentPaymentService : GenericService<UploadedFactorInstallmentPayment, int>, IUploadedFactorInstallmentPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadedFactorInstallmentPaymentRepository _uploadedFactorInstallmentPaymentRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ISqlBulkCopyExcelToDb _iSqlBulkCopyExcelToDb;
        private readonly IYashilLogger<UploadedFactorInstallmentPayment> _logger;
        private readonly IFactorPaymentInstallmentService _factorPaymentInstallmentService;


        public UploadedFactorInstallmentPaymentService(IUnitOfWork unitOfWork,
            IUploadedFactorInstallmentPaymentRepository uploadedFactorInstallmentPaymentRepository,
            IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher,
            IAppDocumentService appDocumentService,
            IAdditionalUserPropService additionalUserPropService,
            ISqlBulkCopyExcelToDb iSqlBulkCopyExcelToDb,
            IYashilLogger<UploadedFactorInstallmentPayment> logger,
            IFactorPaymentInstallmentService factorPaymentInstallmentService)
            : base(unitOfWork, uploadedFactorInstallmentPaymentRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _uploadedFactorInstallmentPaymentRepository = uploadedFactorInstallmentPaymentRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _appDocumentService = appDocumentService;
            _additionalUserPropService = additionalUserPropService;
            _iSqlBulkCopyExcelToDb = iSqlBulkCopyExcelToDb;
            _logger = logger;
            _factorPaymentInstallmentService = factorPaymentInstallmentService;
        }

        public IQueryable<UploadedFactorInstallmentPayment> GetByCustomFilter(int? YearMonth, int? userId)
        {
            return _uploadedFactorInstallmentPaymentRepository.GetByCustomFilter(YearMonth, userId);
        }
        public async Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempFile, int yearMonth)
        {
            var uploadedFactorInstallmentPaymentColumns =
                UploadedFactorInstallmentPaymentInfo.Instance.Columns
                    .Select(x => x.Key)
                    .ToArray();

            try
            {
                // 1. ذخیره فایل
                var addAppDocumentResult =
                    await AddAppDocumentAsync(
                        tempFile,
                        DocumentTypeEnum.UploadedFactorInstallmentPayment);

                if (!addAppDocumentResult.isSuccess)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = addAppDocumentResult.message
                    };
                }

                var filePath =
                    _appDocumentService.GetFilePath(
                        addAppDocumentResult.appDocument.Id);

                string fileLocation =
                    Path.Combine(GlobalConfiguration.WebRootPath, filePath);

                var workbook = new Workbook(fileLocation);
                var worksheet = workbook.Worksheets[0];

                // 2. اعبتارسنجی اولیه فایل
                var basicValidaton = BaseValidateFile(worksheet, 1, yearMonth);
                if (!basicValidaton.isSuccess)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = basicValidaton.message
                    };
                }

                // دریافت سال و ماه از سلول اول فایل
                var yearMonthCellValue =
                    worksheet.Cells[
                        UploadedFactorInstallmentPaymentInfo.YearMonthHeaderPosition
                    ].Value;


                if (!int.TryParse(yearMonthCellValue?.ToString(), out var fileYearMonth))
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "ماه و سال وارد شده صحیح نمی‌باشد"
                    };
                }

                // 3. حذف دیتای کلی قبلی بر اساس سال و ماه که ثبت نهایی نشده است
                _uploadedFactorInstallmentPaymentRepository.DeleteByYearMonth(_userPrincipal.Id, fileYearMonth);

                // 4. به جدول Excel وارد کردن اطلاعات
                var res =
                    await _iSqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync(
                        "shp.UploadedFactorInstallmentPayment",
                        filePath,
                        0,
                        uploadedFactorInstallmentPaymentColumns,
                        addAppDocumentResult.appDocument.Id,
                        skippedRowCount: 0,
                        addCreationDate: true,
                        customColumns: new Dictionary<string, object>
                        {
                           {
                               "YearMonth",
                               yearMonthCellValue
                           }
                        },
                        headerRowIndex: 1);

                if (!res)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "بروز خطا در بارگذاری فایل اکسل پرداخت‌های اقساطی"
                    };
                }


                // 5. اعتبارسنجی اطلاعات آپلود شده

                var hasError =
                    await _uploadedFactorInstallmentPaymentRepository
                        .ValidateUploadedFactorInstallmentPayment(
                            _userPrincipal.Id,
                           yearMonth);

                if (hasError)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کابر گرامی اطلاعات فایل دارای خطا می باشد لطفا قبل از ثبت نهایی آنرا رفع کنید"
                    };
                }

                return new ResultStatus<int>
                {
                    IsSuccess = true
                };

            }
            catch (Exception e)
            {
                _logger.InsertLog(
                    LogLevel.Error,
                    (int)Events.InsertException,
                    "shp.UploadedFactorInstallmentPayment",
                    "بروز خطا در بارگذاری فایل اکسل پرداخت‌های اقساطی",
                    e);

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل پرداخت‌های اقساطی"
                };
            }
        }

        /// <summary>
        ///  بررسی فایل و اعتبارسنجی اولیه قبل از آپلود در جدول
        ///  UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="headerRowIndex"></param>
        /// <returns></returns>
        private (bool isSuccess, string message) BaseValidateFile(Worksheet worksheet, int headerRowIndex, int yearMonth)
        {
            // بررسی هدر ماه و سال
            var yearMonthCellValue = worksheet.Cells[UploadedFactorInstallmentPaymentInfo.YearMonthHeaderPosition].Value;
            if (yearMonthCellValue == null)
            {
                return (
                    false,
                    "کاربر گرامی، فایل اکسل دارای مقادیر صحیحی نمی‌باشد. لطفاً مقادیر هدر اکسل را تغییر ندهید."
                );
            }

            if (!int.TryParse(yearMonthCellValue.ToString()?.Trim(), out int cellYearMonth) || cellYearMonth != yearMonth)
            {
                return (false, "کاربر گرامی، فایل اکسل، مربوط به ماه مورد نظر نیست.");
            }

            var range = worksheet.Cells.MaxDisplayRange;

            int firstDataRow = headerRowIndex + 1;
            int lastRow = range.FirstRow + range.RowCount - 1;
            int lastColumn = range.FirstColumn + range.ColumnCount - 1;

            // بررسی وجود حداقل یک ردیف داده
            if (firstDataRow > lastRow)
            {
                return (
                    false,
                    "فایل اکسل فاقد اطلاعات برای بارگذاری است."
                );
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// درج سند برای اکسل آپلودشدهٔ پرداخت‌های اقساطی
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="documentTypeEnum"></param>
        /// <returns></returns>
        private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile, DocumentTypeEnum documentTypeEnum)
        {
            return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.UploadedFactorInstallmentPayment, (int)documentTypeEnum,
                null, (int)AppEntityEnum.UploadedFactorInstallmentPayment, formFile);
        }


        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        public async Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId)
        {
            return await _factorPaymentInstallmentService.UpdateFactorInstallmentPaymentFromUploadedFileAsync(userId);
        }

        /// <summary>
        /// دریافت لیست شناسه مرجع پرداخت
        /// براساس تاریخ ثبت اکسل در جدول
        /// UploadedFactorInstallmentPayment
        /// گروه بندی می شود
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public async Task<List<UploadedFactorInstallmentPaymentReferenceGroupDto>> GetFactorPaymentInstallmentReferenceGroupAsync(int yearMonth)
        {
            var data = await _uploadedFactorInstallmentPaymentRepository
                .GetFactorPaymentInstallmentReferenceGroup(_userPrincipal.Id, yearMonth)
                .ToListAsync();

            return data
                .GroupBy(x => new
                {
                    x.CreationDate.Date,
                    x.CreationDate.Hour,
                    x.CreationDate.Minute
                })
                .Select(x =>
                {
                    var creationDate = x
                        .OrderBy(y => y.CreationDate)
                        .First()
                        .CreationDate;

                    return new UploadedFactorInstallmentPaymentReferenceGroupDto
                    {
                        Id = x.Min(y => y.Id),
                        YearMonth = yearMonth,
                        CreationDate = creationDate,
                        CreationPersianDateTime =
                            DateFunction.ToShamsiString(
                                creationDate.Year,
                                creationDate.Month,
                                creationDate.Day)
                            + " "
                            + creationDate.ToString("HH:mm"),
                        PaymentReferenceId = x.Select(x => x.PaymentReferenceId).FirstOrDefault(),
                        PaymentReferencePersianDateTime = x.Select(x => x.PaymentReferencePersianDateTime).FirstOrDefault()
                    };
                })
                .OrderBy(x => x.CreationDate)
                .ToList();
        }

        public IQueryable<UploadedFactorInstallmentPaymentReferenceDto> GetFactorPaymentInstallmentReferenceAsync(DateTime creationDate)
        {
            return _uploadedFactorInstallmentPaymentRepository.GetFactorPaymentInstallmentReference(_userPrincipal.Id, creationDate);
        }


        public class UploadedFactorInstallmentPaymentInfo
        {
            public enum ColumnName
            {
                Id,
                Code,
                CompanyOrRepresentationTitle,
                FactorCode,
                NumberOfInstallmentsPaid,
                SumAmountPaymentToSupplier,
                InstallmentAmount,
                RefundAmount,
                PaidAmount,
                RemainingPaymentToSupplier,
                TotalPaymentToSupplier,
                InstallmentNumber,
                NumberOfInstallmentsRemaining,
                AccountNumber,
                ShebaNumber,
                BankAccountOwner,
                BankName,
                YearMonth,
                FactorPaymentInstallmentId,
            }
            private static UploadedFactorInstallmentPaymentInfo _uploadedFactorInstallmentPaymentInfoInstance;
            public static UploadedFactorInstallmentPaymentInfo Instance =>
                _uploadedFactorInstallmentPaymentInfoInstance ??= new UploadedFactorInstallmentPaymentInfo();
            public List<KeyValuePair<string, ColumnDetail>> Columns { get; }
            public class ColumnDetail
            {
                /// <summary>
                /// نام ستون
                /// </summary>
                public string Name { get; set; }

            }
            UploadedFactorInstallmentPaymentInfo()
            {
                Columns = new List<KeyValuePair<string, ColumnDetail>>
            {
                new(ColumnName.FactorPaymentInstallmentId.ToString(), new ColumnDetail() { Name = "شناسه" }),
                new(ColumnName.Code.ToString(), new ColumnDetail() { Name = "کد آموزشگاه" }),
                new(ColumnName.CompanyOrRepresentationTitle.ToString(), new ColumnDetail() { Name = "موسسه" }),
                new(ColumnName.FactorCode.ToString(), new ColumnDetail() { Name = "شماره فاکتور" }),
                new(ColumnName.NumberOfInstallmentsPaid.ToString(), new ColumnDetail() { Name = "تعداد اقساط پرداختی" }),
                new(ColumnName.SumAmountPaymentToSupplier.ToString(), new ColumnDetail() { Name = "مبلغ فاکتور" }),
                new(ColumnName.InstallmentAmount.ToString(), new ColumnDetail() { Name = "مبلغ یک قسط" }),
                new(ColumnName.RefundAmount.ToString(), new ColumnDetail() { Name = "عودت وجه" }),
                new(ColumnName.PaidAmount.ToString(), new ColumnDetail() { Name = "میلغ پرداختی" }),
                new(ColumnName.RemainingPaymentToSupplier.ToString(), new ColumnDetail() { Name = "مانده مبلغ" }),
                new(ColumnName.TotalPaymentToSupplier.ToString(), new ColumnDetail() { Name = "مجموع پرداختی" }),
                new(ColumnName.InstallmentNumber.ToString(), new ColumnDetail() { Name = "شماره قسط" }),
                new(ColumnName.NumberOfInstallmentsRemaining.ToString(), new ColumnDetail() { Name = "مانده قسط" }),
                new(ColumnName.AccountNumber.ToString(), new ColumnDetail() { Name = "شماره حساب" }),
                new(ColumnName.ShebaNumber.ToString(), new ColumnDetail() { Name = "شماره شبا" }),
                new(ColumnName.BankAccountOwner.ToString(), new ColumnDetail() { Name = "نام صاحب حساب" }),
                new(ColumnName.BankName.ToString(), new ColumnDetail() { Name = "نام بانک" }),
                new(ColumnName.YearMonth.ToString(), new ColumnDetail() { Name = "تاریخ قسط" }),
            };
            }
            public static string YearMonthHeaderPosition = "A1";
        }
    }
}
