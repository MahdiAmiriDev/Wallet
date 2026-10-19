using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class HybridPaymentRequestController : BaseController<HybridPaymentRequest, int,
        HybridPaymentRequestListViewModel, HybridPaymentRequestEditModel, HybridPaymentRequestSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHybridPaymentRequestService _hybridPaymentRequestService;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public HybridPaymentRequestController(IHybridPaymentRequestService hybridPaymentRequestService,
            IMapper mapper,
            IYashilLogger<HybridPaymentRequest> logger,
            IPermissionService permissionService, IPaymentGatewayService paymentGatewayService) : base(
            hybridPaymentRequestService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _hybridPaymentRequestService = hybridPaymentRequestService;
            _paymentGatewayService = paymentGatewayService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? merchantId,
            int? yashilHekmatTerminalId, int? hybridPaymentRequestStatus)
        {
            var hybridPaymentRequests = _hybridPaymentRequestService.GetByCustomFilter(merchantId,
                yashilHekmatTerminalId, hybridPaymentRequestStatus, null, null);
            return await DataSourceLoader.LoadAsync(
                hybridPaymentRequests.ProjectTo<HybridPaymentRequestListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? merchantId,
            int? yashilHekmatTerminalId, int? hybridPaymentRequestStatus)
        {
            var hybridPaymentRequests = _hybridPaymentRequestService.GetByCustomFilter(merchantId,
                yashilHekmatTerminalId, hybridPaymentRequestStatus, null, null);
            return await DataSourceLoader.LoadAsync(
                hybridPaymentRequests.ProjectTo<HybridPaymentRequestSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// متد محاسبه قیمت میزان گرداخت نقدی و اقساطی
        /// این متد زمانی صدا زده میشود که کاربر برای اولین بار به صفحه پرداخت ترکیبی رفته است و پذیرنده درصدی را مشخص نکرده
        /// پس کاربرگس از مشخص نمودن درصد گرداخت نقدی و زدن محاسبه قیمت این متد صدا زده میشود
        /// </summary>
        /// <param name="calculatePriceForPayRequestViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("CalculatePriceForPayAsync")]

        public async Task<CalculatePriceForPayResultViewModel> CalculatePriceForPayAsync(CalculatePriceForPayRequestViewModel calculatePriceForPayRequestViewModel)
        {
            var resultModel = new CalculatePriceForPayResultViewModel
            {
                Succeed = false
            };
            // بررسی معتبر بودن توکن
            (resultModel.Succeed, resultModel.Message, _, var ipgToken, var terminalInfo) =
                await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(calculatePriceForPayRequestViewModel
                    .PaymentRequestToken);
            if (!resultModel.Succeed || ipgToken == null || terminalInfo == null)
            {
                return resultModel;
            }
            // بررسی میکنیم که توکن که توسط پذیرنده ارسال شده باشد خود دارای درصد پرداخت نقدی نباشد
            // اگر بود باید خطا دهیم
            // زیرا تنها در حالتی اجازه صدا زدن این متد را داریم که درصد پرداخت نقدی مشخص نباشد 
            if (ipgToken.CashPaymentPercentage.HasValue)
            {
                resultModel.Succeed = false;
                resultModel.Message = "کاربر گرامی شما نمی توانید درصد پرداخت نقدی را تغییر دهید.";
                return resultModel;
            }
            // مقدار دهی پارامتر ها
            resultModel.CashPaymentPercentage = calculatePriceForPayRequestViewModel.CashPaymentPercentage;
            resultModel.InstituteWage = terminalInfo.InstituteWage;
            resultModel.SupplierWage = terminalInfo.SupplierWage;
            resultModel.CashInstituteWage = terminalInfo.CashInstituteWage;
            resultModel.CashSupplierWage = terminalInfo.CashSupplierWage;
            resultModel.BasePrice = ipgToken.Amount;
            resultModel.InstallmentCount = ipgToken.InstallmentCount;
            resultModel.Tax = !ipgToken.IsTaxExempt ? 9 : 0;
            resultModel.ShippingAmount = ipgToken.ShippingAmount;
            // محاسبه قیمت نقدی و اقساطی
            resultModel.CashPaymentPrice = _hybridPaymentRequestService
                .CalculateCashPrice(resultModel.BasePrice,
                 resultModel.CashPaymentPercentage,
                 resultModel.CashInstituteWage,
                 resultModel.Tax,
                 resultModel.ShippingAmount, ipgToken.UseFormula);
            resultModel.HekmatPaymentPrice = _hybridPaymentRequestService
                .CalculateHekmatPrice(resultModel.BasePrice,
                resultModel.CashPaymentPercentage,
                resultModel.InstallmentCount,
                resultModel.InstituteWage,
                resultModel.SupplierWage,
                resultModel.Tax,
                resultModel.ShippingAmount, ipgToken.UseFormula);


            // مبلغ کل اقساطی باید از مبلغ تنظیم شده در appConfig
            // بیشتر باشد
            (var isValidHekmatPrice, var hekmatPriceValidateMessage) =
                await _paymentGatewayService.ValidateHekmatPriceAsync(resultModel.HekmatPaymentPrice);
            resultModel.IsValidHekmatPrice = isValidHekmatPrice;
            if (!isValidHekmatPrice)
            {
                resultModel.Succeed = true;
                resultModel.Message = hekmatPriceValidateMessage;
            }

            return resultModel;
        }

    }
}