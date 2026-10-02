using System;
using System.Collections.Generic;
using System.Drawing;
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
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class UploadedVoucherService : GenericService<UploadedVoucher, int>, IUploadedVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadedVoucherRepository _uploadVoucherRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ISqlBulkCopyExcelToDb _iSqlBulkCopyExcelToDb;
        private readonly IYashilLogger<UploadedVoucher> _logger;
        private readonly ICompanyService _companyService;
        private readonly IVocherTypeService _voucherTypeService;
        public UploadedVoucherService(IUnitOfWork unitOfWork, IUploadedVoucherRepository uploadVoucherRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IAppDocumentService appDocumentService,
            IAdditionalUserPropService additionalUserPropService, ISqlBulkCopyExcelToDb iSqlBulkCopyExcelToDb,
            IVocherTypeService voucherTypeService,
            IYashilLogger<UploadedVoucher> logger, ICompanyService companyService)
            : base(unitOfWork, uploadVoucherRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _uploadVoucherRepository = uploadVoucherRepository;
            _userPrincipal = userPrincipal;
            _appDocumentService = appDocumentService;
            _additionalUserPropService = additionalUserPropService;
            _iSqlBulkCopyExcelToDb = iSqlBulkCopyExcelToDb;
            _logger = logger;
            _companyService = companyService;
            _voucherTypeService = voucherTypeService;
        }

        public IQueryable<UploadedVoucher> GetByCustomFilter(int? appDocumentId)
        {
            return _uploadVoucherRepository.GetByCustomFilter(appDocumentId);
        }
        /// <summary>
        /// حذف کوپن های کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        public void DeleteByUserId(int userId)
        {
            _uploadVoucherRepository.DeleteByUserId(userId);
        }
        /// <summary>
        /// بارگذاری کوپن از طریق فایل اکسل در جدول UploadVoucher
        /// </summary>
        /// <param name="tempVoucherFile">فایل ارسالی ار سمت کاربر</param>
        /// <param name="companyId">سناسه مرکز</param>
        /// /// <param name="voucherTypeId">نوع کوپن</param>
        /// <returns></returns>
        public async Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempVoucherFile, int companyId, int voucherTypeId)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            // ستون هایی را بارگذاری می کنیم که اجازه آپلود داشته باشند.
            var voucherColumns = UploadVoucherInfo.Instance.VoucherColumns.Select(x => x.Key).ToArray();
            try
            {
                var addAppDocumentResult = await AddAppDocumentAsync(tempVoucherFile, DocumentTypeEnum.UploadVoucher);
                var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);
                // فایل بارگذاری شده را باز میکنیم و اطلاعات مرکز آن را میخوانیم
                string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, filePath);
                var workbook = new Workbook(fileLocation);
                var worksheet = workbook.Worksheets[0];
                // خواندن مقدار آیدی مرکز
                var companyIdCellValue = worksheet.Cells[UploadVoucherInfo.CompanyIdPosition].Value;
                if (companyIdCellValue == null)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی فایل اکسل دارای مقادیر صحیحی نمی باشد، لطفا مقادیر هدر اکسل را تغییر ندهید"
                    };
                }
                else
                {
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
                }
                // خواندن مقدار آیدی نوع کوپن
                var voucherTypeIdCellValue = worksheet.Cells[UploadVoucherInfo.VoucherTypeIdPosition].Value;
                if (voucherTypeIdCellValue == null)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی فایل اکسل دارای مقادیر صحیحی نمی باشد، لطفا مقادیر هدر اکسل را تغییر ندهید"
                    };
                }
                else
                {
                    // تبدیل مقدار عددی نوع کوپن
                    var canConvertVoucherTypeIdToInt = Int32.TryParse(voucherTypeIdCellValue.ToString(), out int voucherTypeIdInt);
                    if (!canConvertVoucherTypeIdToInt)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message =
                                "کاربر گرامی فایل اکسل دارای مقادیر صحیحی نمی باشد، لطفا مقادیر هدر اکسل را تغییر ندهید"
                        };
                    }
                    if (voucherTypeIdInt != voucherTypeId)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کاربر گرامی فایل بارگذاری شده مربوط به نوع کوپن انتخابی نمی باشد"
                        };
                    }
                }

                var res = await _iSqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.UploadedVoucher", filePath,
                    0, voucherColumns,
                    addAppDocumentResult.appDocument.Id);
                await transaction.CommitAsync();
                if (res)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                    };
                }

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل کوپن"
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "shp.UploadedVoucher",
                    $"بروز خطا در فایل بارگذاری فایل اکسل کوپن ", e);
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل کوپن"
                };
            }
        }
        /// <summary>
        /// نمونه فایل کد کوپن را در قالب
        /// MemoryStream
        /// بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        public async Task<MemoryStream> CopySampleVoucherCodeFileToWorkBook(int companyId, int voucherTypeId)
        {
            var voucherColumns = UploadVoucherInfo.Instance.VoucherColumns;
            // CreateWorkBook 
            var voucherWorkBook = await CreateWorkBookAsync(voucherColumns.Select(x => x.Value.Name).ToList(), companyId, voucherTypeId);

            var voucherCodeMemoryStream = new MemoryStream();
            voucherWorkBook.Save(voucherCodeMemoryStream, SaveFormat.Xlsx);
            voucherCodeMemoryStream.Seek(0, SeekOrigin.Begin);


            return voucherCodeMemoryStream;
        }
        /// <summary>
        /// اطلاعات مربوط به مرکز و نوع کوپن  را در هدر  نمونه اکسل کوپن قرار می دهد
        /// </summary>
        /// <param name="workSheet">شیت اکسل</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        private async Task InsertHeaderInfoIntoWorkSheetAsync(Worksheet workSheet, int companyId, int voucherTypeId)
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
            workSheet.Cells[UploadVoucherInfo.CompanyIdPosition].PutValue(companyId);
            workSheet.Cells[UploadVoucherInfo.CompanyTitlePosition].PutValue($"نام مرکز: {companyTitle} - ");
            workSheet.Cells.Merge(0, 1, 1, 2);
            workSheet.Cells[UploadVoucherInfo.VoucherTypeIdPosition].PutValue(voucherTypeId);
            var voucherType = (await _voucherTypeService.GetAsync(voucherTypeId, true));
            if (voucherType != null)
            {
                workSheet.Cells[UploadVoucherInfo.VoucherTypeTitlePosition].PutValue(
                    $"نوع کوپن : {voucherType.Title}");
            }
            else
            {
                throw new Exception("نوع ووچر یافت نشد");
            }

            var companyIdPositionStyle = workSheet.Cells[UploadVoucherInfo.CompanyIdPosition].GetStyle();
            companyIdPositionStyle.Font.Color = Color.CornflowerBlue;
            workSheet.Cells[UploadVoucherInfo.CompanyIdPosition].SetStyle(companyIdPositionStyle);

            var voucherTypeIdPositionStyle = workSheet.Cells[UploadVoucherInfo.VoucherTypeIdPosition].GetStyle();
            voucherTypeIdPositionStyle.Font.Color = Color.CornflowerBlue;
            workSheet.Cells[UploadVoucherInfo.VoucherTypeIdPosition].SetStyle(voucherTypeIdPositionStyle);
        }
        /// <summary>
        ///  ساخت WorkBook
        /// </summary>
        /// <param name="customColumnNames">نام ستون های دلخواه برای نمایش </param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<Workbook> CreateWorkBookAsync(List<string> customColumnNames, int companyId, int voucherTypeId)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "کد کوپن";
            worksheet.DisplayRightToLeft = true;

            await InsertHeaderInfoIntoWorkSheetAsync(worksheet, companyId, voucherTypeId);
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
            return workbook;
        }
        /// <summary>
        /// درج سند برای اکسل کد کوپن
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="documentTypeEnum"></param>
        /// <returns></returns>
        private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile, DocumentTypeEnum documentTypeEnum)
        {
            return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.UploadedVoucher, (int)documentTypeEnum,
                null, (int)AppEntityEnum.UploadedVoucher, formFile);
        }
        /// <summary>
        /// دریافت کد کوپن های بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public IQueryable<UploadedVoucher> GetByUserId(int userId)
        {
            return _uploadVoucherRepository.GetByUserId(userId);
        }
        /// <summary>
        /// کد کوپن بارگذاری شده توسط کاربر را بعد از بارگذاری موفق رمز می کند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="voucherUrlPrefix">آدرس ارسال کد کوپن</param>
        /// <returns></returns>
        public async Task<bool> EncryptVoucherCode(int userId)
        {
            var uploadedVouchers = GetByUserId(userId);
            foreach (var uploadedVoucher in uploadedVouchers)
            {
                uploadedVoucher.Code = CryptographyHelper.AesEncrypt($"{uploadedVoucher.Code}");
            }
            try
            {
                await SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "shp.UploadedVoucher",
                    $"بروز خطا در رمز کردن مقدار ووچر ", e);
                return false;
            }
        }
    }
}

public class UploadVoucherInfo
{
    public enum VoucherColumnName
    {
        Code
    }
    private static UploadVoucherInfo _uploadVoucherInfoInstance;
    public static UploadVoucherInfo Instance =>
        _uploadVoucherInfoInstance ??= new UploadVoucherInfo();
    public class VoucherColumnDetail
    {
        /// <summary>
        /// نام ستون
        /// </summary>
        public string Name { get; set; }

    }
    public List<KeyValuePair<string, VoucherColumnDetail>> VoucherColumns { get; }
    private UploadVoucherInfo()
    {
        VoucherColumns = new List<KeyValuePair<string, VoucherColumnDetail>>
        {
            new(VoucherColumnName.Code.ToString(), new VoucherColumnDetail() { Name = "کد کوپن" })
        };

    }
    public int GetIndexOfUploadVoucherColumn(VoucherColumnName columnName)
    {
        return VoucherColumns.IndexOf(VoucherColumns.FirstOrDefault(x => x.Key == columnName.ToString()));
    }

    public static string CompanyIdPosition = "A1";
    public static string CompanyTitlePosition = "B1";
    public static string VoucherTypeIdPosition = "D1";
    public static string VoucherTypeTitlePosition = "E1";

}
