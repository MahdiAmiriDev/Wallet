using System;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Enums;
using YashilDms.Core.Services;
using YashilUserManagement.Core.Services;
using Yashil.Core.Configurations;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class HekmatTransactionController : BaseController<HekmatTransaction, int, HekmatTransactionListViewModel, HekmatTransactionEditModel, HekmatTransactionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHekmatTransactionService _hekmatTransactionService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<HekmatTransaction> _logger;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IAppConfigService _appConfigService;
        private readonly IDataBaseConfiguration _dataBaseConfiguration;
        private readonly IPermissionService _permissionService;

        public HekmatTransactionController(IHekmatTransactionService hekmatTransactionService,
        IMapper mapper,
        IYashilLogger<HekmatTransaction> logger,
        IAppConfigService appConfigService,
        IDataBaseConfiguration dataBaseConfiguration,
        IPermissionService permissionService, IUserPrincipal userPrincipal, IAppDocumentService appDocumentService) : base(hekmatTransactionService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _logger = logger;
            _userPrincipal = userPrincipal;
            _appDocumentService = appDocumentService;
            _hekmatTransactionService = hekmatTransactionService;
            _appConfigService = appConfigService;
            _dataBaseConfiguration = dataBaseConfiguration;
            _permissionService = permissionService;
        }
        /// <summary>
        /// دریافت تراکنش های حکمت که در وضعیت ارسال به حکمت می باشند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTransactions")]
        [YashilAuthorization(ActionName = "TransactionsForSendToHekmat")]
        public async Task<List<HekmatTransactionListCustomViewModel>> GetTransactionsForSendToHekmatAsync()
        {
            var hekmatTransactions = await _hekmatTransactionService.GetHekmatTransactions().ToListAsync();
            return _mapper.Map<List<HekmatTransactionListCustomViewModel>>(hekmatTransactions);

        }
        /// <summary>
        /// دریافت فایل تراکنش های حکمت بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetHekmatTransactionItem")]
        [YashilAuthorization(ActionName = "HekmatTransactionItem")]
        public async Task<LoadResult> GetHekmatTransactionItem(CustomDataSourceLoadOptions loadOptions)
        {
            var hekmatTransactions = _hekmatTransactionService.GetByUserId(_userPrincipal.Id, true);
            return await DataSourceLoader.LoadAsync(hekmatTransactions.ProjectTo<UploadedHekmatTransactionListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// بعد از بارگذاری فایل نتیجه تراکنش های حکمت در سایت، وقتی کاربر دکمه
        /// ثبت نهایی را می زند این متد صدا شده می شود
        /// و نتیجه تراکنش های حکمت به روز رسانی می شود
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateHekmatTransaction")]
        [YashilAuthorization(ActionName = "UpdateHekmatTransaction")]
        public async Task<ResultStatus<int>> UpdateHekmatTransaction()
        {
            var res = await _hekmatTransactionService.UpdateHekmatTransactionAsync(_userPrincipal.Id);
            if (string.IsNullOrEmpty(res))
            {
                var encryptedKey = _dataBaseConfiguration.EncryptedKey;
                //بعد از اینکه به روز رسانی نتیجه تراکنش های حکمت با موفقیت انجام شد متد زیر جهت رمز کردن داده ها فراخوانی می شود
                await _hekmatTransactionService.EncryptHekmatTransactionsAsync(encryptedKey);
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "کاربر گرامی، تراکنش های حکمت با موفقیت در سایت به روز رسانی شد"
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = res
            };
        }
        /// <summary>
        /// ابتدا رکورد های قبلی مربوط به کاربر لاگین کرده را از جدول 
        /// HekmatTransaction
        ///  در صورتی که انجام نشده است را پاک میکند. 
        /// سپس فایل ارسالی از سمت کاربر را به سرویس درج فایل از طرق اکسل می فرستد
        /// درصورتی که بارگذاری فایل اکسل با موفقیت انجام شود و خطایی رخ ندهد
        /// اطلاعات فایل تراکنش های حکمت در جدول
        /// HekmatTransaction
        /// درج می شود 
        /// </summary>
        /// <param name="hekmatTransactionFile">فایل ارسالی ار سمت کاربر</param>
        /// <returns></returns>
        [HttpPost("UploadHekmatTransactionFileAsync")]
        [YashilAuthorization(ActionName = "UploadHekmatTransactionFile")]
        public async Task<ResultStatus<int>> UploadHekmatTransactionFileAsync(IFormFile hekmatTransactionFile)
        {
            try
            {
                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                await _hekmatTransactionService.DeleteByUserIdAsync(_userPrincipal.Id);
                var msg = await _hekmatTransactionService.BulkInsertFromCsvAsync(hekmatTransactionFile);
                if (string.IsNullOrEmpty(msg))
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        Message = "فایل اکسل محصولات با موفقیت بارگذاری شد"
                    };
                }

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = msg
                };

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
        ///فایل های بارگذاری شده  نتیجه تراکنش حکمت را بر می گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetTransactionFiles")]
        [YashilAuthorization(ActionName = "HekmatTransactionFile")]
        public async Task<LoadResult> GetTransactionFilesAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var transactionFiles = _appDocumentService.GetObjectDocumentsByUserId(Enum.GetName(AppEntityEnum.HekmatTransaction), _userPrincipal.Id,
                (int)DocumentTypeEnum.HekmatTransaction);
            return await DataSourceLoader.LoadAsync(transactionFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}