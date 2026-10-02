using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Core.Enums;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    /// <summary>
    /// پرداخت از روی پایگاه داده محلی
    /// پرداخت هایی که اطلاعات مربوط به اعتبار سنجی آنها در پایگاه داده محلی و در جدول مربوط به اطلاعات باشگاه مشتریان است
    /// برای رکوردهایی که فیلد IsWalletPay
    /// </summary>
    public class WalletPaymentController : YashilMvcBaseController<object>
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerClubCardService _customerClubCardService;
        private readonly IPaymentService _paymentService;

        public WalletPaymentController(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
             IYashilLogger<object> logger, IPaymentService paymentService,
            IPermissionService permissionService,
            IOrderService orderService,
            ICustomerClubCardService customerClubCardService)
            : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _orderService = orderService;
            _customerClubCardService = customerClubCardService;
            _paymentService = paymentService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        public IActionResult Index(PayWithWalletParam payWithWalletParam)
        {
            if (!payWithWalletParam.OId.HasValue)
            {
                return RedirectToAction("Index", "Error");
            }
            ModelState.Clear();
            return View(payWithWalletParam);

        }

        /// <summary>
        ///  نتیجه پرداخت از نوع پایگاه داده محلی
        ///  بعد از وارد کردن کد مربوط به اعتبار سنجی وارد این اکشن می شود
        /// </summary>
        /// <param name="oId">Order Id</param>
        /// <returns></returns>
        public IActionResult VerifyPaymentTransaction(int? paymentId)
        {
            var walletPaymentVerifyViewModel = new VerifyPaymentViewModel()
            {
                PaymentMethodType = PaymentMethodTypesEnum.Wallet
            };
            if (!paymentId.HasValue)
            {
                return null;
            }
            if (paymentId.HasValue)
            {
                var payment = _paymentService.Get(paymentId.Value, true);
                if (payment != null)
                {
                    var order = _orderService.Get(payment.OrderId, true);

                    var paymentStatusType = payment.PaymentStatusType;
                    walletPaymentVerifyViewModel.OrderNumber = order.Title;
                    walletPaymentVerifyViewModel.ResNum = _orderService.GetReservationNumber(payment.OrderId, payment.Id);
                    if (paymentStatusType == (int)PaymentStatusTypesEnum.Paid && order.CustomerClubCardId.HasValue)
                    {
                        walletPaymentVerifyViewModel.Message = WalletStatusMessage.SuccessfulVerify;
                        walletPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                    }
                    else if (paymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful)
                    {
                        walletPaymentVerifyViewModel.Message = WalletStatusMessage.Invalidity;
                        walletPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    }
                    else if (paymentStatusType == (int)PaymentStatusTypesEnum.Cancel)
                    {
                        walletPaymentVerifyViewModel.Message = WalletStatusMessage.CancelByUser;
                        walletPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                    }
                    else
                    {
                        walletPaymentVerifyViewModel.Message = WalletStatusMessage.ErrorMessage;
                        walletPaymentVerifyViewModel.PaymentStatusType = (PaymentStatusTypesEnum)paymentStatusType;
                    }
                    return View(walletPaymentVerifyViewModel);
                }
            }
            return null;

        }

    }
}
