using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class HekmatPaymentGatewayTransactionController : BaseController<HekmatPaymentGatewayTransaction, int, HekmatPaymentGatewayTransactionListViewModel, HekmatPaymentGatewayTransactionEditModel, HekmatPaymentGatewayTransactionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHekmatPaymentGatewayTransactionService _hekmatPaymentGatewayTransactionService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatPaymentGatewayTransactionController(IHekmatPaymentGatewayTransactionService hekmatPaymentGatewayTransactionService,
        IMapper mapper,
        IYashilLogger<HekmatPaymentGatewayTransaction> logger,
        IPermissionService permissionService, ApplicationConfiguration applicationConfiguration, IUserPrincipal userPrincipal) : base(hekmatPaymentGatewayTransactionService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _hekmatPaymentGatewayTransactionService = hekmatPaymentGatewayTransactionService;
            _applicationConfiguration = applicationConfiguration;
            _userPrincipal = userPrincipal;
        }
        /// <summary>
        /// دریافت تراکنش های حکمت که در وضعیت ارسال به حکمت می باشند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTransactionsForSendToHekmatAsync")]
        [YashilAuthorization("GetTransactionsForSendToHekmat")]

        public async Task<List<HekmatPaymentGatewayTransactionCustomViewModel>> GetTransactionsForSendToHekmatAsync()
        {
            //تراکنش ها برای ارسال به حکمت دریافت میکند
            var hekmatPaymentGatewayTransaction = await _hekmatPaymentGatewayTransactionService.
                GetTransactionsForSendToHekmatAsync(_userPrincipal.ApplicationId);
            var hekmatPaymentGatewayTransactions = await hekmatPaymentGatewayTransaction
                .ProjectTo<HekmatPaymentGatewayTransactionCustomViewModel>(_mapper.ConfigurationProvider).ToListAsync();


            return hekmatPaymentGatewayTransactions;

        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? merchantId, int? yashilHekmatTerminalId, int? fromDate, int? toDate, int? statusCode)
        {
            var hekmatPaymentGatewayTransactions = _hekmatPaymentGatewayTransactionService.GetByCustomFilter(merchantId, yashilHekmatTerminalId, fromDate, toDate, statusCode, null);
            return await DataSourceLoader.LoadAsync(
                hekmatPaymentGatewayTransactions.ProjectTo<HekmatPaymentGatewayTransactionListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد جهت رمزنگاری کد ملی و شماره
        /// کارت تراکنش های حکمت کاربرد دارد
        /// </summary>
        /// <param name="hekmatTransactions"></param>
        [NonAction]
        private void EncryptCardNumberAndNationalCode(List<HekmatPaymentGatewayTransactionCustomViewModel> hekmatTransactions)
        {
            try
            {
                hekmatTransactions.Where(x =>
                        !string.IsNullOrEmpty(x.HekmatCardNumber) && !string.IsNullOrEmpty(x.NationalCode))
                    .ToList().ForEach(x =>
                    {
                        x.HekmatCardNumber = CryptographyHelper.AesEncrypt(x.HekmatCardNumber);
                        x.NationalCode = CryptographyHelper.AesEncrypt(x.NationalCode);
                    });
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}