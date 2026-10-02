using System.Collections.Generic;
using JetBrains.Annotations;
using Yashil.Common.Core.Attributes;
using Yashil.Core.CustomViewModels;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class OrderSimplePresentationViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// شماره سفارش
        /// </summary>
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string ShippingAddressTitle { get; set; }
        /// <summary>
        ///         /// کد ملی
        ///</summary>
        [StringValidation] public string? NationalCode { get; set; }
        [StringValidation] public string RecipientTitle { get; set; }
        public int PaymentStatusType { get; set; }
        public int OrderStatusType { get; set; }
        [StringValidation] public string PaymentStatusTypeTitle { get; set; }
        /// <summary>
        /// نام سیستم روش پرداخت- حکمت-ثمین و شتاب-
        ///</summary>
        public int? PaymentMethodType { get; set; }

        [StringValidation] public string OrderStatusTypeTitle { get; set; }
        public decimal? Total { get; set; }
        public int CreationDateInt { get; set; }
        public bool AllowPay { get; set; }
        //The order is allowed to be canceled
        public bool AllowCancel { get; set; }
        /// <summary>
        /// این فیلد توسط مدیر سیستم مقدار دهی می شود
        /// </summary>
        public bool AllowRefund { get; set; }
        /// <summary>
        /// این فیلد توسط مدیر سیستم مقدار دهی می شود
        /// </summary>
        public bool AllowEdit { get; set; }
        /// <summary>
        /// این فیلد محاسبه می شود
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// این فیلد محاسبه می شود
        /// </summary>
        public bool Refundable { get; set; }
        /// <summary>
        /// این فیلد محاسبه می شود
        /// </summary>
        public bool Cancelable { get; set; }

        public bool AllowCancelEdit { get; set; }

        public int SaleType { get; set; }
        public int SubsidyStatusType { get; internal set; }
        [StringValidation] public string SubsidyStatusTypeTitle { get; internal set; }
        public decimal? TotalInclSubsidy { get; set; }
        public decimal? TotalExclSubsidy { get; set; }
        /// <summary>
        /// شماره سفارش
        /// </summary>
//        [StringValidation] public string OrderNumber { get; set; }
        /// <summary>
        /// سفارش تغییر داده شده است یا خیر
        /// </summary>
        public bool IsModified { get; set; }
        /// <summary>
        /// شناسه سفارش تغییر داده شده
        /// </summary>
        public int? LastOrderId { get; set; }
        public bool InstallmentBased { get; set; }
        public bool SubsidyBased { get; internal set; }
        [StringValidation] public string CustomMessage { get; set; }
        /// <summary>
        /// آیا محصول قابل دانلود است
        /// </summary>
        public bool IsDownloadAbleProduct { get; set; }
        /// <summary>
        /// لینک دانلود برنامه ریزی دوره
        /// </summary>
        public string? ProductSalesPlanningDownloadUrl { get; set; }

    }

    public class OrderDetailPresentationViewModel
    {
        public int Id { get; set; }
        public AddressListViewModel addressListViewModel { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string RecipientTitle { get; set; }
        //public int PaymentStatusType { get; set; }

        public int OrderStatusType { get; set; }

        [StringValidation] public string OrderStatusTypeTitle { get; set; }

        //[StringValidation] public string PaymentStatusTypeTitle { get; set; }

        public decimal? Total { get; set; }

        public int CreationDateInt { get; set; }

        public bool AllowPay { get; set; }
        public int SaleType { get; internal set; }

        [StringValidation] public string ShippingStatusTypeTitle { get; internal set; }

        [StringValidation] public string SubsidyTypeTitle { get; internal set; }
        [StringValidation] public string ShippingAddressFullAddress { get; set; }
        [StringValidation] public string ShippingAddressEmail { get; set; }
        [StringValidation] public string ShippingAddressMobile { get; set; }
        [StringValidation] public string ShippingAddressTelephone { get; set; }
        public int? SubsidyStatusType { get; set; }
        [StringValidation] public string SubsidyStatusTypeTitle { get; set; }
        public decimal? TotalInclSubsidy { get; set; }
        public decimal? TotalExclSubsidy { get; set; }

        public decimal? PricePerInstallmentExclSubsidy { get; set; }

        public decimal? PricePerInstallmentInclSubsidy { get; set; }

        public int? InstallmentCountInclSubsidy { get; set; }

        public int? InstallmentCountExclSubsidy { get; set; }
        [StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        public int? CardType { get; set; }

        [StringValidation] public string MembershipTypeTitle { get; set; }
        public int MembershipType { get; set; }

        [StringValidation] public string CityTitle { get; set; }


        /// <summary>
        /// شماره سفارش
        /// </summary>
        [StringValidation] public string OrderNumber { get; set; }
        /// <summary>
        /// مالیات سفارش شامل یارانه
        ///</summary>
        public decimal? TaxInclSubsidy { get; set; }
        /// <summary>
        /// مالیات سفارش بدون یارانه
        ///</summary>
        public decimal? TaxExclSubsidy { get; set; }

        /// <summary>
        /// شناسه کاربر سفارش
        /// UserId in Order Table
        /// </summary>
        public int UserId { get; set; }
    }

    public class OrderItemSimplePresentationViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [StringValidation] public string ProductCode { get; set; }

        [StringValidation] public string OrderCode { get; set; }

        [StringValidation] public string ProductTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public int Quantity { get; set; }
        [StringValidation] public string VoucherString { get; set; }
        /// <summary>
        /// قالب نوع ووچر در صورتی
        /// دارای مقدار 1 باشد
        /// یعنی ووچر از دارای لینک آدرس می باشد
        /// در غیر اینصورت ووچر دارای لینک نمی باشد و مقدار آن نمایش داده می شود 
        /// </summary>
        public int? VoucherTemplateId { get; set; }
        /// <summary>
        /// لینک آدرس برای ووچر که به هر مرکز در صورت نیاز اختصاص داده می شود
        /// </summary>
        [StringValidation] public string VoucherUrlPrefix { get; set; }

        public bool Deleted { get; set; }
        /// <summary>
        /// کلید api
        /// مرکز برای استفاده از سرویس
        /// APi
        /// مرکز 
        /// </summary>
        public string CompanyApiLocalKey { get; set; }
        /// <summary>
        /// آیا مرکز از
        /// APi
        /// پشتیبانی می کند
        /// </summary>
        public bool CompanyApiSupported { get; set; }
        /// <summary>
        /// سفارش نیاز به ارجاع به سایت مرکز دارد یا نه؟
        /// </summary>
        public bool NeedRedirectToCompanySiteForReceive { get; set; }

    }

    public class OrderItemDetailPresentationViewModel
    {
        public OrderItemDetailPresentationViewModel()
        {
            OrderItemChildDetailList = new List<OrderItemChildDetailPresentationViewModel>();
        }
        public int Id { get; set; }
        public int ShoppingCartItemId { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string ProductCode { get; set; }

        [StringValidation] public string OrderCode { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal? DiscountAmountInclTax { get; set; }

        public decimal? DiscountAmountExclTax { get; set; }

        public decimal PriceIncDiscount { get; set; }

        public decimal WagePercentage { get; set; }

        public decimal WageAmountIncTax { get; set; }

        public decimal WageAmountExcTax { get; set; }

        public decimal? SubsidyPrice { get; set; }
        /// <summary>
        /// سقف یارانه ساتا برای این آیتم
        /// </summary>
        public decimal? MaxSataSubsidyPrice { get; set; }
        /// <summary>
        ///  سقف یارانه وزارت برای این آیتم
        /// </summary>
        public decimal? MaxVezaratSubsidyPrice { get; set; }

        public decimal TaxPrice { get; set; }

        public decimal? PriceTotal { get; set; }
        public int InstallmentCount { get; set; }


        public decimal PricePerInstallment { get; set; }

        public decimal? RefundedAmount { get; set; }

        public decimal OriginalProductCost { get; set; }

        public decimal? Total { get; set; }
        public decimal? TotalInclSubsidy { get; set; }
        public decimal? TotalExclSubsidy { get; set; }
        public decimal? ProfitPrice { get; set; }
        public decimal? Price { get; set; }

        public bool SubsidyBased { get; set; }

        [StringValidation] public string ProductPicture { get; set; }
        public decimal CalcPrice { get; internal set; }
        public int ProductSalesPlanningId { get; set; }
        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string StateTitle { get; set; }
        [StringValidation] public string RepresentationAddress { get; set; }
        public bool? RepresentationAddressIsEncrypted { get; set; }
        [StringValidation] public string RepresentationTelephone { get; set; }
        [StringValidation] public string CoursePlanningSchedule { get; set; }
        public int? EndDate { get; set; }
        /// <summary>
        ///     تاریخ شروع
        ///</summary>
        public int? StartDate { get; set; }
        /// <summary>
        ///  کد ووچر 
        /// </summary>
        [StringValidation] public string VocherCode { get; set; }

        [StringValidation] public string ModificationDate { get; set; }
        /// <summary>
        /// توضیحات مربوط به مرکز
        /// </summary>
        public string CompanyDescription { get; set; }
        /// <summary>
        /// فرمت زیر نویس مربوط به ProductSalePlaning 
        /// </summary>
        public string ProductSalePlaningSubTitleFormat { get; set; }
        /// <summary>
        /// فرمت زیر نویس مربوط به Company 
        /// </summary>
        public string CompanySubTitleFormat { get; set; }
        /// <summary>
        /// شناسه قالب محصول
        /// </summary>
        public int ProductTemplateId { get; set; }
        /// <summary>
        /// عنوان قالب محصول
        /// </summary>
        public string ProductTemplateTitle { get; set; }


        public List<OrderItemChildDetailPresentationViewModel> OrderItemChildDetailList { get; set; }
        /// <summary>
        /// مدل تعلق گرفتن یارانه اشتغال به کاربر و دوره
        /// </summary>
        [CanBeNull]
        public EmploymentSubsidyDto EmploymentSubsidyDto { get; set; }

        /// <summary>
        /// آیا کاربر جاری رزومه را به انداره کافی پر کرده است
        /// </summary>
        public bool IsResumeFillOutEnough { get; set; }

        /// <summary>
        /// متن پیامی که کاربر بعد از کلیک روی دکمه "منتظر تایید تامین کننده برای پرداخت" به او نمایش داده می شود 
        /// </summary>
        public string MessageForUserRequireSupplierApprovalBeforePaymentProducts { get; set; }

        /// <summary>
        /// شناسه تنظیم فروش مربوط به productSalesPlanning
        /// </summary>
        public int SaleConfigurationId { get; set; }

        /// <summary>
        /// حداقل مبلغ خرید مربوط به تنظیم فروش
        /// </summary>
        public decimal? MinimumPurchasePrice { get; set; }
    }
    /// <summary>
    /// نمایش عنوان و قیمت و قیمت با تخفیف برای بچه های آیتم سفارش
    /// که در جزییات سفارش در صورتی که آیتم سفارش دارای بچه باشد
    /// این اطلاعات را نمایش می دهد
    /// </summary>
    public class OrderItemChildDetailPresentationViewModel
    {
        /// <summary>
        /// شناسه فرزند آیتم سفارش
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        public decimal Price { get; set; }


    }

    /// <summary>
    /// مدل مربوط به صفحه جزییات سفارش
    /// </summary>
    public class OrderDetailPagePresentationViewModel
    {
        public OrderDetailPagePresentationViewModel()
        {
            orderDetailPresentationViewModel = new OrderDetailPresentationViewModel();
            orderItemDetailPresentationViewModels = new List<OrderItemDetailPresentationViewModel>();
        }
        public OrderDetailPresentationViewModel orderDetailPresentationViewModel { get; set; }
        public List<OrderItemDetailPresentationViewModel> orderItemDetailPresentationViewModels { get; set; }

        public bool InstallmentBased { get; set; }
        public bool SubsidyBased { get; internal set; }
        public bool AddressBased { get; internal set; }

        public decimal? PricePerInstallment { get; set; }

        public int? InstallmentCount { get; set; }
    }
    /// <summary>
    /// مدل مربوط به نمایش هر سفارش در لیست سفارشات
    /// </summary>
    public class OrderListPresentationViewModel
    {
        public OrderListPresentationViewModel()
        {
            OrderSimpleViewModel = new OrderSimplePresentationViewModel();
            OrderItemSimplePresentationViewModels = new List<OrderItemSimplePresentationViewModel>();
        }
        public OrderSimplePresentationViewModel OrderSimpleViewModel { get; set; }
        public List<OrderItemSimplePresentationViewModel> OrderItemSimplePresentationViewModels { get; set; }

    }
    /// <summary>
    /// مدل مربوط به صفحه لیست سفارشات
    /// </summary>
    //public class OrderStatisticsPagePresentationViewModel
    //{
    //    public bool SubsidyBased;
    //    public bool AddressBased;
    //    public List<OrderListPresentationViewModel> CurrentOrder;
    //    public List<OrderListPresentationViewModel> WaitingForSubsidyOrder;
    //    public List<OrderListPresentationViewModel> WaitingForPayOrder;
    //    public List<OrderListPresentationViewModel> CompletePayOrder;
    //    public List<OrderListPresentationViewModel> WaitingForResultOrder;
    //    public List<OrderListPresentationViewModel> RefundOrPartialRefundOrder;
    //    public List<OrderListPresentationViewModel> CanceledOrder;
    //}
}