using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.SharedKernel;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Enums;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class UploadedCertificateService : GenericService<UploadedCertificate, int>, IUploadedCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadedCertificateRepository _uploadedCertificateRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IYashilLogger<UploadedCertificate> _logger;
        private readonly ISqlBulkCopyExcelToDb _sqlBulkCopyExcelToDb;

        public UploadedCertificateService(IUnitOfWork unitOfWork, IUploadedCertificateRepository uploadedCertificateRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IAppDocumentService appDocumentService, IYashilLogger<UploadedCertificate> logger, ISqlBulkCopyExcelToDb sqlBulkCopyExcelToDb)
            : base(unitOfWork, uploadedCertificateRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _uploadedCertificateRepository = uploadedCertificateRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _appDocumentService = appDocumentService;
            _logger = logger;
            _sqlBulkCopyExcelToDb = sqlBulkCopyExcelToDb;
        }

        /// <summary>
        /// این متد جهت حذف کردن تمامی
        /// رکورد های مربوط به جدول
        /// UploadedCertificate
        /// که توسط کاربری که شناسه آن ارسال شده است کاربرد دارد.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task DeleteByUserIdAsync(int userId)
        {
            return _uploadedCertificateRepository.DeleteByUserIdAsync(userId);
        }

        public async Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempCertificateFile)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var addAppDocumentResult = await _appDocumentService.SaveDocumentAsync(null, 
                    null, null, null, null,
                    (int)AppEntityEnum.UploadedCertificate, (int)DocumentTypeEnum.UploadedCertificate,
                    null, (int)AppEntityEnum.Certificate, tempCertificateFile);

                if (!addAppDocumentResult.isSuccess)
                    return new ResultStatus<int>(false, addAppDocumentResult.message);

                var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);

                string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, filePath);

                var wrokbook = new Workbook(fileLocation);

                if (wrokbook.Worksheets[0].Cells.Rows.Count <= 1)
                    new ResultStatus<int>(false,
                        "کاربر گرامی فایل اکسل گواهی نامه ها نمی تواند خالی باشد لطفا فایل اکسل را مقدار دهی نمایید و مجددا تلاش کنید.");

                var uploadedCertificateColumns = SampleCertificateFileInfo.Instance.
                    SampleCertificateFileColumns.Where(x => x.Value.AllowUpload)
                    .Select(s => s.Key).ToArray();


                var isSuccess = await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync
                ("shp.UploadedCertificate", filePath,
                    0, uploadedCertificateColumns,
                    addAppDocumentResult.appDocument.Id, 0,true);

                if (!isSuccess)
                    return new ResultStatus<int>(false, "بروز خطا در بارگذاری فایل اکسل کاربران lms");


                await transaction.CommitAsync();
                return new ResultStatus<int>(true, string.Empty);

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                _logger.InsertLog(LogLevel.Error, (int)Events.SaveException, nameof(UploadedCertificate),
                    "بروز خطا به هنگام بارگذاری فایل اکسل گواهی نامه ها", e);

                return new ResultStatus<int>(false, "بروز خطا در بارگذاری فایل اکسل گواهی نامه ها");
            }
        }

        public Task<int> GetInvalidUploadedCertificateCountAsync(int userId)
        {
            return _uploadedCertificateRepository.GetInvalidUploadedCertificateCountAsync(userId);
        }

        /// <summary>
        /// این متد داده ستون های که نیاز است
        /// رمزنگاری شوند را
        /// hash
        /// می کند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> HashRequiredColumnsAsync(int userId)
        {
            try
            {
                var uploadedCertificateQuery = GetAll(false).Where(x => x.CreateBy == userId);

                foreach (var uploadedCertificate in uploadedCertificateQuery)
                {
                    if (!string.IsNullOrEmpty(uploadedCertificate.Name))
                        uploadedCertificate.Name = CryptographyHelper.AesEncrypt(uploadedCertificate.Name);
                    if (!string.IsNullOrEmpty(uploadedCertificate.LastName))
                        uploadedCertificate.LastName = CryptographyHelper.AesEncrypt(uploadedCertificate.LastName);
                    if (!string.IsNullOrEmpty(uploadedCertificate.FatherName))
                        uploadedCertificate.FatherName = CryptographyHelper.AesEncrypt(uploadedCertificate.FatherName);
                    if (!string.IsNullOrEmpty(uploadedCertificate.LatinName))
                        uploadedCertificate.LatinName = CryptographyHelper.AesEncrypt(uploadedCertificate.LatinName);
                    if (!string.IsNullOrEmpty(uploadedCertificate.LatinLastName))
                        uploadedCertificate.LatinLastName = CryptographyHelper.AesEncrypt(uploadedCertificate.LatinLastName);
                    if (!string.IsNullOrEmpty(uploadedCertificate.NationalCode))
                        uploadedCertificate.NationalCode = CryptographyHelper.AesEncrypt(uploadedCertificate.NationalCode);
                    if (!string.IsNullOrEmpty(uploadedCertificate.IdentityNumber))
                        uploadedCertificate.IdentityNumber = CryptographyHelper.AesEncrypt(uploadedCertificate.IdentityNumber);
                    if (!string.IsNullOrEmpty(uploadedCertificate.MobileNumber))
                        uploadedCertificate.MobileNumber = CryptographyHelper.AesEncrypt(uploadedCertificate.MobileNumber);
                    if (!string.IsNullOrEmpty(uploadedCertificate.Tell))
                        uploadedCertificate.Tell = CryptographyHelper.AesEncrypt(uploadedCertificate.Tell);

                    await UpdateAsync(uploadedCertificate, uploadedCertificate.Id);
                }

                await SaveChangeAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }

        public Task<int> AddCertificateFromUploadedCertificateAsync(int userId)
        {
            return _uploadedCertificateRepository.AddCertificateFromUploadedCertificate(userId);
        }

        public Task<bool> HasUserUploadAnyCertificateAsync(int userId)
        {
            return GetAll(true).AnyAsync(uc => uc.CreateBy == userId);
        }

        public MemoryStream CopySampleCertificateFileToWoorkbook()
        {
            var sampleCertificateFileInfo = SampleCertificateFileInfo.Instance.SampleCertificateFileColumns;

            var sampleUserFileWorkBook = CreateWorkbook(sampleCertificateFileInfo.Select(x => x.Value.Name).ToList());

            var ms = new MemoryStream();
            sampleUserFileWorkBook.Save(ms, SaveFormat.Xlsx);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }


        public Workbook CreateWorkbook(List<string> certificateColumnNames)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "نمونه فایل اکسل گواهینامه";
            worksheet.DisplayRightToLeft = true;
            // فرمت سلول های اکسل را Text می کند
            Style formatStyle = worksheet.Cells.GetCellStyle(0, 0);
            // 49 = TextFormat
            formatStyle.Number = 49;
            worksheet.Cells.ApplyStyle(formatStyle, new StyleFlag { All = true });


            foreach (var name in certificateColumnNames)
            {
                SetStyleForTextWrapperColumns(workbook, name, 30);
            }

            var style = new CellsFactory().CreateStyle();

            style.BackgroundColor = System.Drawing.Color.Yellow;
            style.ForegroundColor = System.Drawing.Color.Yellow;
            style.Pattern = BackgroundType.Solid;
            style.IsTextWrapped = true;
            style.Font.IsBold = true;
            style.VerticalAlignment = TextAlignmentType.Center;
            style.HorizontalAlignment = TextAlignmentType.Center;

            var i = 0;
            foreach (var columnName in certificateColumnNames)
            {
                worksheet.Cells[0, i].PutValue(columnName);
                worksheet.Cells[0, i].SetStyle(style, new StyleFlag { All = true });
                i++;
            }

            worksheet.FreezePanes(1, 0, 1, 0);
            return workbook;

        }

        /// <summary>
        /// ست کردن استایل text wrap و طول مناسب
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="columnName"></param>
        /// <param name="width"></param>
        private void SetStyleForTextWrapperColumns(Workbook workbook, string columnName, int width)
        {
            var column = workbook.Worksheets[0].Cells.Columns[SampleCertificateFileInfo.Instance.GetIndexOfSampleUserFileItemColumn(columnName)];

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

        public IQueryable<UploadedCertificate> GetByUserId(int userId, bool showInvalidItem)
        {
            return _uploadedCertificateRepository.GetByUserId(userId, showInvalidItem);
        }
    }

    public class SampleCertificateFileInfo
    {
        /// <summary>
        /// ستون های اکسل گواهینامه 
        /// </summary>
        public List<KeyValuePair<string, ColumnDetail>> SampleCertificateFileColumns { get; }

        private static SampleCertificateFileInfo _sampleUserFileInfo;

        public static SampleCertificateFileInfo Instance =>
            _sampleUserFileInfo ??= new SampleCertificateFileInfo();
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


        private SampleCertificateFileInfo()
        {
            SampleCertificateFileColumns =
                new List<KeyValuePair<string, ColumnDetail>>
                {
                new(SampleCertificateFileColumnName.IssueDateInt.ToString(),new ColumnDetail {Name = "تاریخ صدور",AllowUpload = true}),
                new(SampleCertificateFileColumnName.Number.ToString(),new ColumnDetail {Name = "شماره پیگیری",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CityTitle.ToString(),new ColumnDetail {Name = "عنوان شهر",AllowUpload = true}),
                new(SampleCertificateFileColumnName.RepresentationTitle.ToString(),new ColumnDetail {Name = "عنوان نمایندگی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.TeacherName.ToString(),new ColumnDetail {Name = "نام مربی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CourseCode.ToString(),new ColumnDetail {Name = "کد دوره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CourseTitle.ToString(),new ColumnDetail {Name = "عنوان دوره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CourseType.ToString(),new ColumnDetail {Name = "نوع دوره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CourseDuration.ToString(),new ColumnDetail {Name = "مدت زمان دوره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.SexTitle.ToString(),new ColumnDetail {Name = "عنوان جنسیت",AllowUpload = true}),
                new(SampleCertificateFileColumnName.Name.ToString(),new ColumnDetail {Name = "نام",AllowUpload = true}),
                new(SampleCertificateFileColumnName.LastName.ToString(),new ColumnDetail {Name = "نام خانوادگی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.FatherName.ToString(),new ColumnDetail {Name = "نام پدر",AllowUpload = true}),
                new(SampleCertificateFileColumnName.LatinName.ToString(),new ColumnDetail {Name = "نام لاتین",AllowUpload = true}),
                new(SampleCertificateFileColumnName.LatinLastName.ToString(),new ColumnDetail {Name = "نام خانوادگی لاتین",AllowUpload = true}),
                new(SampleCertificateFileColumnName.NationalCode.ToString(),new ColumnDetail {Name = "کد ملی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.IdentityNumber.ToString(),new ColumnDetail {Name = "شماره شناسنامه",AllowUpload = true}),
                new(SampleCertificateFileColumnName.PlaceOfIssue.ToString(),new ColumnDetail {Name = "محل صدور",AllowUpload = true}),
                new(SampleCertificateFileColumnName.Score.ToString(),new ColumnDetail {Name = "نمره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.MobileNumber.ToString(),new ColumnDetail {Name = "شماره موبایل",AllowUpload = true}),
                new(SampleCertificateFileColumnName.Tell.ToString(),new ColumnDetail {Name = "تلفن",AllowUpload = true}),
                new(SampleCertificateFileColumnName.Email.ToString(),new ColumnDetail {Name = "ایمیل",AllowUpload = true}),
                new(SampleCertificateFileColumnName.DateOfBirthInt.ToString(),new ColumnDetail {Name = "تاریخ تولد",AllowUpload = true}),
                new(SampleCertificateFileColumnName.LastEvidence.ToString(),new ColumnDetail {Name = "آخرین مدرک تحصیلی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.FieldOfStudy.ToString(),new ColumnDetail {Name = "رشته تحصیلی",AllowUpload = true}),
                new(SampleCertificateFileColumnName.CoursePrice.ToString(),new ColumnDetail {Name = "مبلغ دوره",AllowUpload = true}),
                new(SampleCertificateFileColumnName.TeacherMobile.ToString(),new ColumnDetail {Name = "شماره مربی",AllowUpload = true}),

                };

        }

        /// <summary>
        /// نام ستون های سوالات چهار گزینه ای
        /// </summary>
        public enum SampleCertificateFileColumnName
        {
            /// <summary>
            /// تاریخ صدور
            /// </summary>
            IssueDateInt,
            /// <summary>
            /// شماره گوهینامه
            /// </summary>
            Number,
            /// <summary>
            /// عنوان شهر
            /// </summary>
            CityTitle,
            /// <summary>
            /// عنوان نمایندگی
            /// </summary>
            RepresentationTitle,
            /// <summary>
            /// نام مربی
            /// </summary>
            TeacherName,
            /// <summary>
            /// عنوان دوره
            /// </summary>
            CourseCode,
            /// <summary>
            /// عنوان دوره
            /// </summary>
            CourseTitle,
            /// <summary>
            ///نوع دوره
            /// </summary>
            CourseType,
            /// <summary>
            ///مدت زمان دوره
            /// </summary>
            CourseDuration,
            /// <summary>
            /// عنوان جنسیت
            /// </summary>
            SexTitle,
            /// <summary>
            /// نام
            /// </summary>
            Name,
            /// <summary>
            /// نام خانوادگی
            /// </summary>
            LastName,
            /// <summary>
            /// نام پدر
            /// </summary>
            FatherName,
            /// <summary>
            /// نام لاتین
            /// </summary>
            LatinName,
            /// <summary>
            /// نام خانوادگی به لاتین
            /// </summary>
            LatinLastName,
            /// <summary>
            ///کد ملی
            /// </summary>
            NationalCode,
            /// <summary>
            /// شماره شناسنامه
            /// </summary>
            IdentityNumber,
            /// <summary>
            /// محل صدور
            /// </summary>
            PlaceOfIssue,
            /// <summary>
            /// نمره
            /// </summary>
            Score,
            /// <summary>
            /// شماره موبایل
            /// </summary>
            MobileNumber,
            /// <summary>
            /// تلفن
            /// </summary>
            Tell,
            /// <summary>
            /// ایمیل
            /// </summary>
            Email,
            /// <summary>
            /// تاریخ تولد
            /// </summary>
            DateOfBirthInt,
            /// <summary>
            /// آخرین مدرک
            /// </summary>
            LastEvidence,
            /// <summary>
            /// رشته تحصیلی
            /// </summary>
            FieldOfStudy,
            /// <summary>
            /// هزینه دوره
            /// </summary>
            CoursePrice,
            /// <summary>
            /// شماره موبایل مربی
            /// </summary>
            TeacherMobile,

        }


        /// <summary>
        /// Ctor
        /// </summary>

        public int GetIndexOfSampleUserFileItemColumn(string columnName)
        {
            var columnInfo = SampleCertificateFileColumns.FirstOrDefault(x => x.Value.Name == columnName);
            var index =  SampleCertificateFileColumns.IndexOf(columnInfo);
            return index;
        }

    }
}