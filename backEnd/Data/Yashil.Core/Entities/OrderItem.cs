using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OrderItem  :IBaseEntity<int>,IApplicationBasedEntity
    {
public OrderItem()
{
    InverseParent = new HashSet<OrderItem>();
    Issue = new HashSet<Issue>();
    ModifiedOrderItemNewOrderItem = new HashSet<ModifiedOrderItem>();
    ModifiedOrderItemOldOrderItem = new HashSet<ModifiedOrderItem>();
    OrderItemDependentProductSale = new HashSet<OrderItemDependentProductSale>();
    OrderItemDetail = new HashSet<OrderItemDetail>();
    OrderItemPayment = new HashSet<OrderItemPayment>();
    OrderTrakking = new HashSet<OrderTrakking>();
    VocherNavigation = new HashSet<Vocher>();
    WalletReservation = new HashSet<WalletReservation>();
    WalletTransaction = new HashSet<WalletTransaction>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد پدر-برای محصولات ترکیبی استفاده می شود.و در لیست سفارشات فقط آنهایی که کد پدرشان خالی است آورده می شود
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// تنظیم فروش
///</summary>
public int SaleConfigurationId { get; set; }
/// <summary>
///         /// شناسه سفارش
///</summary>
public int OrderId { get; set; }
/// <summary>
///         /// شناسه محصول
///</summary>
public int ProductSalesPlanningId { get; set; }
/// <summary>
///         /// شناسه نمایندگی
///</summary>
public int RepresentationId { get; set; }
/// <summary>
///         /// تعداد
///</summary>
public int Quantity { get; set; }
/// <summary>
///         /// قیمت پایه ضرب در تعداد محصول
///</summary>
public decimal Price { get; set; }
/// <summary>
///         /// درصد تخفیف
///</summary>
public decimal DiscountPercentage { get; set; }
/// <summary>
///         /// میزان تخفیف (شامل مالیات)
///</summary>
public decimal? DiscountAmountInclTax { get; set; }
/// <summary>
///         /// میزان تخفیف (بدون مالیات)
///</summary>
public decimal? DiscountAmountExclTax { get; set; }
/// <summary>
///         /// قیمت با تخفیف
///</summary>
public decimal PriceIncDiscount { get; set; }
/// <summary>
///         /// میزان کارمزد  (شامل مالیات)
///</summary>
public decimal? WageAmountIncTax { get; set; }
/// <summary>
///         /// میزان کارمزد  (بدون مالیات)
///</summary>
public decimal? WageAmountExcTax { get; set; }
/// <summary>
///         /// مبلغ یارانه
///</summary>
public decimal? SubsidyPrice { get; set; }
/// <summary>
///         /// درصد سود یارانه
///</summary>
public decimal? SubsidyProfitPercentage { get; set; }
/// <summary>
///         /// مبلغ یارانه (شامل سود یارانه)
///</summary>
public decimal? SubsidyPriceIncProfit { get; set; }
/// <summary>
///         /// مبلغ یارانه (شامل مالیات و شامل سود یارانه)
///</summary>
public decimal? SubsidyPriceIncTaxIncProfit { get; set; }
/// <summary>
///         /// درصد مالیات
///</summary>
public decimal? TaxPercentage { get; set; }
/// <summary>
///         /// مبلغ مالیات مورد سفارش
///</summary>
public decimal? TaxPriceInclSubsidy { get; set; }
/// <summary>
///         /// مجموع مبلغ پرداختی
///</summary>
public decimal? TotalInclSubsidy { get; set; }
/// <summary>
///         /// مبلغ مالیات مورد سفارش
///</summary>
public decimal? TaxPriceExclSubsidy { get; set; }
/// <summary>
///         /// مجموع مبلغ پرداختی
///</summary>
public decimal? TotalExclSubsidy { get; set; }
/// <summary>
///         /// درصد سود از تامین کننده
///</summary>
public decimal ProfitPercentage { get; set; }
/// <summary>
///         /// مبلغ سود از تامین کننده
///</summary>
public decimal ProfitPrice { get; set; }
/// <summary>
///         /// مبلغ عودت داده شده
///</summary>
public decimal? RefundedAmount { get; set; }
/// <summary>
///         /// هزینه اصلی این مورد سفارش (در هنگام سفارش) ، تعداد 1
///</summary>
public decimal? OriginalProductCost { get; set; }
/// <summary>
///         /// شرح ویژگی
///</summary>
public string AttributeDescription { get; set; }
/// <summary>
///         /// ویژگی های محصول در قالب XML
///</summary>
public string AttributesXml { get; set; }
/// <summary>
///         /// تعداد دانلود
///</summary>
public int? DownloadCount { get; set; }
/// <summary>
///         /// آیا دانلود فعال شده است یا خیر
///</summary>
public bool IsDownloadActivated { get; set; }
/// <summary>
///         /// شناسه مجوز دانلود
///</summary>
public int? LicenseDownloadId { get; set; }
/// <summary>
///         /// وزن کل یک مورد
///</summary>
public decimal? ItemWeight { get; set; }
/// <summary>
///         /// تاریخ شروع اجاره محصول
///</summary>
public DateTime? RentalStartDateUtc { get; set; }
/// <summary>
///         /// تاریخ پایان اجاره محصول
///</summary>
public DateTime? RentalEndDateUtc { get; set; }
/// <summary>
///         /// کد
///</summary>
public Guid? Code { get; set; }
/// <summary>
///         /// ایجاد کننده
///</summary>
public int CreateBy { get; set; }
/// <summary>
///         /// ویرایش کننده
///</summary>
public int? ModifyBy { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
/// <summary>
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// کد ووچر
///</summary>
public int? VocherId { get; set; }
/// <summary>
///         /// درصد سایر کسورات مربوط به فاکتور تامین کننده
///</summary>
public decimal OtherDeductionsPercentage { get; set; }
/// <summary>
///         /// مبلغ سایر کسورات از فاکتور تامین کننده
///</summary>
public decimal OtherDeductionsPrice { get; set; }
/// <summary>
///         /// درصد بیمه مربوط به فاکتور تامین کننده
///</summary>
public decimal InsurancePercentage { get; set; }
/// <summary>
///         /// مبلغ بیمه مربوط به فاکتور تامین کننده
///</summary>
public decimal InsurancePrice { get; set; }
/// <summary>
///         /// مبلغ ارزش افزوده مربوط به فاکتور تامین کننده
///</summary>
public decimal ValueAddedPrice { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
public decimal? Netprice { get; set; }
public decimal BasePrice { get; set; }
/// <summary>
///         /// درصد کارمزد سایت
///</summary>
public decimal? WagePercentage { get; set; }
/// <summary>
///         /// ضریب افزایش سقف یارانه برای شهر
///</summary>
public decimal? CitySubsidyFactor { get; set; }
/// <summary>
///         /// ضریب افزایش سقف یارانه برای نمایندگی
///</summary>
public decimal? RepresentationSubsidyFactor { get; set; }
/// <summary>
///         /// ضریب افزایش سقف یارانه برای نمایندگی
///</summary>
public decimal? ProductSubsidyFactor { get; set; }
/// <summary>
///         /// ضریب افزایش سقف یارانه برای دسته محصول
///</summary>
public decimal? CategorySubsidyFactor { get; set; }
/// <summary>
///         /// سقف یارانه برای هر حکمت کارت
///</summary>
public int? SubsidyCeilingPerHekmatCard { get; set; }
/// <summary>
///         /// شناسه یارانه
///</summary>
public int? SubsidyId { get; set; }
/// <summary>
///         /// شناسه دسته بندی مالیاتی
///</summary>
public int TaxCategoryId { get; set; }
/// <summary>
///         /// شناسه گروه دسته بندی
///</summary>
public int CategoryGroupId { get; set; }
/// <summary>
///         /// ضرایب یارانه اعمال بشود
///</summary>
public bool? ApplySubsidyFactor { get; set; }
/// <summary>
///         /// اتظیم فروش در این لحظه معاف از مالیات است
///</summary>
public bool? IsTaxExempt { get; set; }
public bool AllProductsIncludeInsurance { get; set; }
/// <summary>
///         /// آیا این آیتم سفارش از طریق ای پی آی اشتراک دریافت کرده است؟
///</summary>
public bool? IsSubscriptionWithApi { get; set; }
/// <summary>
///         /// تامین کننده این سفارش دارای گواهی ارزش افزوده می باشد.این فیلد در هنگام ثبت سفارش از تنظیم فروش مربوطه خوانده می شود
///</summary>
public bool HasTaxCertificate { get; set; }
public bool? IsOtherDeductionsIncludeInSupplierInvoice { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CategoryGroup CategoryGroup { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Order Order { get; set; }
    public virtual OrderItem Parent { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
    public virtual Subsidy Subsidy { get; set; }
    public virtual TaxCategory TaxCategory { get; set; }
    public virtual Vocher Vocher { get; set; }
    public virtual FactorItem FactorItem { get; set; }
public virtual ICollection<OrderItem>
    InverseParent { get; set; }
public virtual ICollection<Issue>
    Issue { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemNewOrderItem { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemOldOrderItem { get; set; }
public virtual ICollection<OrderItemDependentProductSale>
    OrderItemDependentProductSale { get; set; }
public virtual ICollection<OrderItemDetail>
    OrderItemDetail { get; set; }
public virtual ICollection<OrderItemPayment>
    OrderItemPayment { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakking { get; set; }
public virtual ICollection<Vocher>
    VocherNavigation { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransaction { get; set; }
    }
 }
