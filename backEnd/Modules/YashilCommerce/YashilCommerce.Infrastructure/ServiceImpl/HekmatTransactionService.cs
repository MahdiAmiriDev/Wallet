using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class HekmatTransactionService : GenericService<HekmatTransaction, int>, IHekmatTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatTransactionRepository _hekmatTransactionRepository;
        private readonly IAppDocumentService _appDocumentService;
        private readonly ISqlBulkCopyExcelToDb _iSqlBulkCopyExcelToDb;
        private readonly IYashilLogger<HekmatTransaction> _logger;

        public HekmatTransactionService(IUnitOfWork unitOfWork, IHekmatTransactionRepository hekmatTransactionRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IAppDocumentService appDocumentService, ISqlBulkCopyExcelToDb iSqlBulkCopyExcelToDb, IYashilLogger<HekmatTransaction> logger)
            : base(unitOfWork, hekmatTransactionRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _hekmatTransactionRepository = hekmatTransactionRepository;
            _appDocumentService = appDocumentService;
            _iSqlBulkCopyExcelToDb = iSqlBulkCopyExcelToDb;
            _logger = logger;
        }
        /// <summary>
        /// فایل تراکنش هایی را که منتظر تعیین وضعیت می باشند را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<HekmatTransactionCustomModel> GetHekmatTransactions()
        {
            return _hekmatTransactionRepository.GetHekmatTransactions();
        }
        /// <summary>
        /// دریافت فایل تراکنش های حکمت بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        ///  <param name="readOnly">فقط خواندنی</param>
        /// <returns></returns>
        public IQueryable<HekmatTransaction> GetByUserId(int userId, bool readOnly)
        {
            return GetAll(readOnly).Where(x => x.CreateBy == userId && !x.Done);
        }
        /// <summary>
        /// به روز رسانی نتیجه تراکنش های حکمت در دیتا بیس درگاه حکمت
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public Task<string> UpdateHekmatTransactionAsync(int userId)
        {
            return _hekmatTransactionRepository.UpdateHekmatTransactionsAsync(userId);
        }
        /// <summary>
        /// نتیجه تراکنش های حکمت کاربر را که به طور کامل انجام نشده است حذف می کند 
        /// </summary>
        /// <param name="userId"></param>
        public Task<int> DeleteByUserIdAsync(int userId)
        {
            return _hekmatTransactionRepository.DeleteByUserIdAsync(userId);
        }
        /// <summary>
        /// بارگذاری فایل نتیجه تراکنش های حکمت
        /// </summary>
        /// <param name="hekmatTransactionFile">فایل نتیجه تراکنش حکمت</param>
        /// <returns></returns>
        public async Task<string> BulkInsertFromCsvAsync(IFormFile hekmatTransactionFile)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            // ستون هایی را بارگذاری می کنیم که اجازه آپلود داشته باشند.
            var hekmatTransactionColumns = HekmatTransactionItemInfo.Instance.HekmatTransactionColumns
                .Where(x => x.Value.AllowUpload)
                .Select(x => x.Key).ToArray();
            try
            {
                var addAppDocumentResult = await AddAppDocumentAsync(hekmatTransactionFile, DocumentTypeEnum.HekmatTransaction);
                var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);

                var res = await _iSqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatTransaction", filePath,
                    0, hekmatTransactionColumns,
                    addAppDocumentResult.appDocument.Id, 0);
                await transaction.CommitAsync();
                if (res)
                {
                    return string.Empty;
                }


                return "فایل بارگذاری شده دارای خطا می باشد، لطفا پس از رفع ایرادات فایل، مجددا آن را بارگذاری نمایید";


            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "shp.HekmatTransaction",
                    $"بروز خطا در  فایل نتیجه تراکنش های حکمت ", e);
                return "بروز خطا در بارگذاری فایل اکسل نتیجه تراکنش های حکمت";
            }
        }
        /// <summary>
        /// درج سند فایل تراکنش های حکمت
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="documentTypeEnum"></param>
        /// <returns></returns>
        private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile, DocumentTypeEnum documentTypeEnum)
        {
            return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.HekmatTransaction, (int)documentTypeEnum,
                null, (int)AppEntityEnum.HekmatTransaction, formFile);
        }
        /// <summary>
        /// کد ملی و شماره موبایل و شماره تلفن تراکنش های حکمت را به حالت رمز در می آورد و در 
        /// ستون های جدا ریخته و ستون های کد ملی و شماره موبایل و تلفن را به عدد 1 به روز رسانی می کتد
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        public Task EncryptHekmatTransactionsAsync(string key)
        {
            return _hekmatTransactionRepository.EncryptHekmatTransactionsAsync(key);
        }
    }
    /// <summary>
    /// فایل csv
    /// تراکنش های حکمت
    /// بعد از آنکه  توسط نرم افزار مودم سمت حکمت ثبت می شوند.
    ///  نتیجه تراکنش ها در فایلی با فرمت csv
    ///  دریافت می شود
    /// و برای بارگذاری نتیجه تراکنش های حکمت در دیتا بیس، فایل در فرم بارگذاری نتیجه تراکنش، بارگذاری می شود
    /// و در جدول
    /// HekmatTransaction
    /// درج می شوند و به کاربر نمایش داده می شود
    /// و کاربر با زدن دکمه ثبت نهایی نتیجه تراکنش ها را در دیتا بیس
    /// DargaheHekmatDB
    /// جدول
    /// DargaheHekmat
    /// به روز رسانی می کند.
    /// این کلاس ستون های فایل csv
    /// دریافتی از نرم افزار مودم سمت حکمت را با نام ستون های جدول
    /// HekmatTransaction
    /// مپ می کند تا بتوان در حدول
    /// HekmatTransaction
    /// نتیجه تراکنش را بارگذاری کرد
    /// </summary>
    public class HekmatTransactionItemInfo
    {
        public enum HekmatTransactionColumnName
        {
            TransactionDate,
            InstallmentTrackingCode,
            CardNumber,
            NationalCode,
            InstallmentCount,
            Price,
            Status,
            ErrorCode,
            HekmatToken,
            TransactionResult,
            IsLearn
        }

        private static HekmatTransactionItemInfo _hekmatTransactionItemInfoInstance;
        public static char NumberGroupSeparator = ',';

        public static HekmatTransactionItemInfo Instance =>
            _hekmatTransactionItemInfoInstance ??= new HekmatTransactionItemInfo();
        /// <summary>
        /// جزئیات هر ستون
        /// </summary>
        public class HekmatTransactionColumnDetail
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
        public List<KeyValuePair<string, HekmatTransactionColumnDetail>> HekmatTransactionColumns { get; }

        private HekmatTransactionItemInfo()
        {
            HekmatTransactionColumns =
                new List<KeyValuePair<string, HekmatTransactionColumnDetail>>
                {

                new(HekmatTransactionColumnName.TransactionDate.ToString(),new HekmatTransactionColumnDetail(){Name = "TransactionDate",AllowUpload = true}),
                new(HekmatTransactionColumnName.InstallmentTrackingCode.ToString(),new HekmatTransactionColumnDetail(){Name = "HekmatID",AllowUpload = true}),
                new(HekmatTransactionColumnName.CardNumber.ToString(), new HekmatTransactionColumnDetail(){Name = "HekmatCart",AllowUpload = true}),
                new(HekmatTransactionColumnName.NationalCode.ToString(),new HekmatTransactionColumnDetail(){Name = "Nid",AllowUpload = true} ),
                new(HekmatTransactionColumnName.InstallmentCount.ToString(), new HekmatTransactionColumnDetail(){Name = "TedadGhest",AllowUpload = true}),
                new(HekmatTransactionColumnName.Price.ToString(), new HekmatTransactionColumnDetail(){Name = "Price",AllowUpload = true}),
                new(HekmatTransactionColumnName.Status.ToString(),new HekmatTransactionColumnDetail(){Name =  "Status",AllowUpload = true}),
                new(HekmatTransactionColumnName.ErrorCode.ToString(), new HekmatTransactionColumnDetail(){Name =  "ErrorCode",AllowUpload = true}),
                new(HekmatTransactionColumnName.HekmatToken.ToString(), new HekmatTransactionColumnDetail(){Name =  "Token",AllowUpload = true}),
                new(HekmatTransactionColumnName.TransactionResult.ToString(), new HekmatTransactionColumnDetail(){Name =  "Message",AllowUpload = true}),

                };
        }

        public int GetIndexOfHekmatTransactionItemColumn(HekmatTransactionColumnName columnName)
        {
            return HekmatTransactionColumns.IndexOf(HekmatTransactionColumns.FirstOrDefault(x => x.Key == columnName.ToString()));
        }

    }

}
