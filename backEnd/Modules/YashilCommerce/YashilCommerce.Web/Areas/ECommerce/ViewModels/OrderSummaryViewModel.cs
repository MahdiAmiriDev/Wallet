using System.Collections.Generic;
using Yashil.Common.Core.Attributes;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class OrderSummaryViewModel
    {
        /// <summary>
        /// لیست پرداخت های موفق سفارش
        /// </summary>
        public List<SuccessFulPaymentListViewModel> OrderSuccessFulPaymentListSummary { get; internal set; }
        public OrderAddressViewModel OrderAddressViewModel { get; internal set; }
        public List<OrderItemDetailPresentationViewModel> OrderItemDetailPresentationViewModels { get; internal set; }
        public OrderSummaryForPayViewModel OrderSummaryForPayViewModel { get; internal set; }
        /// <summary>
        ///  پیام بالای صفحه سفارش 
        /// </summary>
        [StringValidation] public string CustomMessage { get; internal set; }
        /// <summary>
        /// منتظر نتیجه پرداخت درگاه حکمت
        /// </summary>
        public bool IsWaitingResponseFromHekmatPaymentGateway { get; internal set; }
        public bool OrderIsComplete { get; internal set; }
        public int OrderStatusType { get; internal set; }

        [StringValidation] public string OrderStatusTypeTitle { get; internal set; }
        public int? SubsidyStatusType { get; internal set; }
        [StringValidation] public string SubsidyStatusTypeTitle { get; internal set; }
        [StringValidation] public string Title { get; set; }
        public int? OrderId { get; set; }
        public bool SubsidyBased { get; set; }
        /// <summary>
        /// نتیجه تراکنش پرداخت
        /// </summary>
        [StringValidation] public string AuthorizationTransactionResult { get; set; }

        /// <summary>
        /// متن راهنمایی برای سفارشات
        /// </summary>
        public string GuidanceText { get; set; }
        /// <summary>
        /// آیا کاربر جاری رزئمه به اندازه کافی پر کرده است
        /// </summary>
        public bool IsResumeFillOutEnough { get; set; }


        /// <summary>
        /// پیام های خطا مربوط به حداقل مبلغ خرید
        /// </summary>
        public List<string> MinimumPurchaseAmountErrorMessages { get; set; }
    }
    public class OrderSummaryForPayViewModel
    {
        public OrderSummaryForPayViewModel()
        {
            AllowContinueShopping = true;
            LimitToHekmatPayment = false;
        }
        public int? OrderId;
        public int OrderItemsCount { get; set; }
        public decimal? SumTotal { get; set; }
        public decimal? SumTotalForPay { get; set; }
        public decimal? SumPrice { get; set; }
        /// <summary>
        /// جمع تخفیف
        /// </summary>
        public decimal? SumDiscount { get; set; }
        public decimal? SumSubsidy { get; set; }
        /// <summary>
        /// سقف یارانه وزرات
        /// </summary>
        public decimal? SumMaxSubsidyVezarat { get; set; }
        /// <summary>
        /// سقف یارانه ساتا
        /// </summary>
        public decimal? SumMaxSubsidySata { get; set; }
        /// <summary>
        /// درصد کارمزد
        /// </summary>
        public decimal? SumWage { get; set; }
        /// <summary>
        /// مبتنی بر یارانه
        /// </summary>
        public bool SubsidyBased { get; set; }
        /// <summary>
        /// اقساطی
        /// </summary>
        public bool AllowedToPayInInstallment { get; set; }
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// اقساط
        /// </summary>
        public decimal? InstallmentAmount { get; set; }
        /// <summary>
        /// نمایش درگاه های پرداخت
        /// </summary>
        public bool AllowSelectPaymentGateway { get; set; }
        /// <summary>
        /// تمامی آیتم های سفارش دارای مقدار می باشند
        /// </summary>
        public bool AllItemsHasValue { get; set; }
        public int RecipientId { get; set; }
        /// <summary>
        /// نیاز به تایید یارانه دارد
        /// </summary>
        public bool RequiresSubsidyApproval { get; set; }
        /// <summary>
        /// نمایش دکمه ادامه فرایند خرید
        /// </summary>
        public bool AllowContinueShopping { get; internal set; }
        /// <summary>
        /// دلیل عدم امکان پرداخت
        /// </summary>
        [StringValidation] public string ReasonForNotBeingAllowedPay { get; internal set; }
        public int? SubsidyStatusType { get; internal set; }
        [StringValidation] public string SubsidyStatusTypeTitle { get; internal set; }
        /// <summary>
        /// فقط درگاه حکمت کارت نمایش داده شود-بدلیل اینکه هنوز یارانه تایید نشده است
        /// </summary>
        public bool LimitToHekmatPayment { get; internal set; }
        /// <summary>
        /// در صورتی که تحویل گیرنده گزینه سایر را انتخاب کند و عضو باشگاه مشتریانی باشد که پرداخت آن لوکالی نباشند
        /// باید درگاه های پرداخت اینترنتی را ببیند و پرداخت کند
        /// </summary>
        public bool LimitedToBankInternetPaymentGateway { get; set; }
        public int PaymentStatusType { get; internal set; }
        [StringValidation] public string PaymentStatusTypeTitle { get; internal set; }
        public int OrderStatusType { get; internal set; }
        [StringValidation] public string OrderStatusTypeTitle { get; internal set; }
        public bool OrderIsComplete { get; internal set; }
        public decimal SumShippingCharge { get; internal set; }
        public decimal? SumTax { get; internal set; }
        /// <summary>
        /// شناسه کاربر سفارش
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// نوع فروش
        /// </summary>
        public int? SaleType { get; set; }
        /// <summary>
        ///  می تواند یارانه استفاده کند
        /// </summary>
        public bool SubsidyCanBeUsed { get; set; }

        /// <summary>
        /// نیاز به تایید تامین کننده قبل از پرداخت کاربر
        /// در صورتی که این پروپرتی مقدار
        /// True
        /// داشته باشد کاربر در صفحه پیش فاکتور دکمه خرید را مشاهده نمی کند
        /// و دکمه ای را جهت تایید تامین کننده مشاهده می کند
        /// تا بعد از تایید تامین کننده بتواند پرداخت کند
        /// </summary>
        public bool RequireSupplierApprovalBeforePayment { get; set; }
        /// <summary>
        /// متن دکمه پیش ثبت نام در صفحه پیش فاکتور
        /// وقتی دوره ای نایز به تایید تامین کننده قبل از پرداخت است
        /// در صفحه پیش فاکتور به جای متن دکمه ثبت نهایی و  پرداخت
        /// متنی از کانفیگ خوانده می شود و برای دکمه قرار میگیرد
        /// </summary>
        public string ButtonTextRequireSupplierApprovalBeforePayment { get; set; }

    }
    public class OrderAddressViewModel
    {
        public OrderAddressViewModel()
        {
            AllowEditAddress = true;
        }
        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string FullAddress { get; set; }
        [StringValidation] public string CardNumber { get; set; }
        [StringValidation] public string NationalCode { get; set; }
        public bool AddressBased { get; set; }
        public int MembershipType { get; set; }
        public bool AllowEditAddress { get; set; }
        public int? SubsidyStatusType { get; set; }
        [StringValidation] public string SubsidyStatusTypeTitle { get; set; }
        [StringValidation] public string MobileNumber { get; set; }
        /// <summary>
        /// کد ملی برای بررسی یارانه ساتا -در صورتی که این کد مساوی مقدار ستون کد ملی باشد،
        /// یعنی وضعیت تایید یارانه درست است و در غیر این صورت باید از اول استعلام گرفته شود
        /// </summary>
        [StringValidation] public string NationalCodeForSubsidyApproval { get; set; }
        public int? SubsidyType { get; set; }
        public bool? IsEncrypted { get; set; }


    }
}
