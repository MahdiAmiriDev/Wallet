using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

public class OrderItemListViewModel
{
    public int Id { get; set; }
    /// <summary>
    ///  شماره موبایل تحویل گیرنده
    /// </summary>
    [StringValidation] public string RecipientMobileNumber { get; set; }
    [StringValidation] public string ProductTitle { get; set; }
    [StringValidation] public string OrderNumber { get; set; }

    [StringValidation] public string MobileNumber { get; set; }

    [StringValidation] public string NationalCode { get; set; }

    [StringValidation] public string RepresentationCode { get; set; }

    [StringValidation] public string ProductCode { get; set; }

    [StringValidation] public string OrderCode { get; set; }

    public int CreationDateInt { get; set; }

    [StringValidation] public string RepresentationTitle { get; set; }

    [StringValidation] public string RepresentationCompanyTitle { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal PriceIncDiscount { get; set; }
    [StringValidation] public string Code { get; set; }
    public DateTime? OrderModificationDate { get; set; }
    [StringValidation] public string OrderStatus { get; internal set; }
    public int OrderStatusType { get; set; }
    public int OrderId { get; set; }
    /// <summary>
    /// مبلغ کارمزد کالا
    /// </summary>
    public decimal? WageAmountExcTax { get; set; }
    /// <summary>
    /// مبلغ یارانه
    /// </summary>
    public decimal? SubsidyPrice { get; set; }
    /// <summary>
    ///   اگر یارانه نداشت --مبلغ مالیات بدون یارانه
    /// </summary>
    public decimal? TaxPriceExclSubsidy { get; set; }
    /// <summary>
    ///   اگر یارانه داشت --مبلغ مالیات با یارانه
    /// </summary>
    public decimal? TaxPriceInclSubsidy { get; set; }
    /// <summary>
    /// مبلغ مالیات
    /// </summary>
    public long? TaxPrice { get; set; }

    /// <summary>
    ///  مبلغ نهایی آیتم سفارش برای پرداخت بدون یارانه یعنی آیتم سفارش یارانه ندارد
    /// </summary>
    public decimal? TotalExclSubsidy { get; set; }
    /// <summary>
    ///  مبلغ نهایی آیتم سفارش برای پرداخت با یارانه یعنی آیتم سفارش یارانه دارد
    /// </summary>
    public decimal? TotalInclSubsidy { get; set; }
    public long Total { get; set; }


    /// <summary>
    /// نوع یارانه - شاغل یا بازنشسته
    /// </summary>
    [StringValidation] public string SubsidyType { get; set; }
    /// <summary>
    /// وضعیت بارانه
    /// </summary>
    [StringValidation] public string SubsidyStatusType { get; set; }
    [StringValidation] public string CompanyTitle { get; internal set; }
    public bool Deleted { get; set; }
    /// <summary>
    /// شماره کارت
    /// </summary>
    [StringValidation] public string CardNumber { get; set; }
    /// <summary>
    /// هزینه ارسال
    /// </summary>
    public decimal? AdditionalShippingCharge { get; set; }
}

public class OrderItemListCustomViewModel
{
    /// <summary>
    /// کد آیتم سفارش
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///  شماره موبایل تحویل گیرنده
    /// </summary>
    [StringValidation] public string RecipientMobileNumber { get; set; }
    /// <summary>
    /// کد پیگیری
    /// </summary>
    [StringValidation] public string OrderNumber { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// کد ملی تحویل گیرنده
    /// </summary>
    [StringValidation] public string NationalCode { get; set; }
    /// <summary>
    /// شماره موبایل
    /// </summary>
    [StringValidation] public string MobileNumber { get; set; }
    /// <summary>
    /// نام محصول
    /// </summary>
    [StringValidation] public string ProductTitle { get; set; }
    /// <summary>
    /// کد محصول
    /// </summary>
    [StringValidation] public string ProductCode { get; set; }
    /// <summary>
    /// نام مرکز
    /// </summary>
    [StringValidation] public string CompanyTitle { get; set; }
    /// <summary>
    /// کد نمایندگی
    /// </summary>
    [StringValidation] public string RepresentationCode { get; set; }
    /// <summary>
    /// نام نمایندگی
    /// </summary>
    [StringValidation] public string RepresentationTitle { get; set; }
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public int CreationDateInt { get; set; }
    /// <summary>
    /// تاریخ تعیین وضعیت
    /// </summary>
    public DateTime? OrderModificationDate { get; set; }
    /// <summary>
    /// وضعیت سفارش
    /// </summary>
    [StringValidation] public string OrderStatus { get; internal set; }
    /// <summary>
    /// قیمت با تخفیف ریال
    /// </summary>
    public decimal PriceIncDiscount { get; set; }
    /// <summary>
    /// مبلغ یارانه
    /// </summary>
    public decimal? SubsidyPrice { get; set; }

    /// <summary>
    /// وضعیت بارانه
    /// </summary>
    [StringValidation] public string SubsidyStatusType { get; set; }
    public int OrderStatusType { get; set; }
    /// <summary>
    /// هزینه ارسال
    /// </summary>
    public decimal? AdditionalShippingCharge { get; set; }
    public int OrderId { get; set; }
    /// <summary>
    /// آیدی برنامه ریزی دوره
    /// </summary>
    public int ProductSalesPlanningId { get; set; }
    /// <summary>
    /// عنوان نسبت خریدار با صاحب حکمت کارت
    /// </summary>
    public string RelationshipWithBuyerTitle { get; set; }
    /// <summary>
    /// عنوان شهر سفارش دهنده
    /// </summary>
    public string CityTitle { get; set; }
}

/// <summary>
/// برای نمایش لیست آیتم های سفارش این ویومدل در نمایش همه آیتم های حتی دیلیت شده در فرم لیست همه سفارشات کاربرد دارد
/// </summary>
public class OrderItemWithDeletedListViewModel
{
    /// <summary>
    /// کد آیتم سفارش
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// آیدی برنامه ریزی دوره
    /// </summary>
    public int ProductSalesPlanningId { get; set; }
    /// <summary>
    /// روزهای تشکیل جلسه
    /// </summary>
    public string CoursePlanningSchedule { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// قیمت پایه
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// مبلغ کارمزد کالا
    /// </summary>
    public decimal? WageAmountExcTax { get; set; }
    /// <summary>
    /// مبلغ مالیات
    /// </summary>
    public long? TaxPrice { get; set; }
    /// <summary>
    /// مبلغ نهایی
    /// </summary>
    public long? Total { get; set; }
    /// <summary>
    /// نام محصول
    /// </summary>
    [StringValidation] public string ProductTitle { get; set; }
    /// <summary>
    /// کد محصول
    /// </summary>
    [StringValidation] public string ProductCode { get; set; }
    /// <summary>
    /// نام مرکز
    /// </summary>
    [StringValidation] public string CompanyTitle { get; set; }
    /// <summary>
    /// کد نمایندگی
    /// </summary>
    [StringValidation] public string RepresentationCode { get; set; }
    /// <summary>
    /// نام نمایندگی
    /// </summary>
    [StringValidation] public string RepresentationTitle { get; set; }
    /// <summary>
    /// قیمت با تخفیف ریال
    /// </summary>
    public decimal PriceIncDiscount { get; set; }
    /// <summary>
    /// مبلغ یارانه
    /// </summary>
    public decimal? SubsidyPrice { get; set; }
    /// <summary>
    /// هزینه ارسال
    /// </summary>
    public decimal? AdditionalShippingCharge { get; set; }
    public int OrderId { get; set; }
    public bool Deleted { get; set; }
    /// <summary>
    ///  عنوان نوع یارانه
    /// </summary>
    public string VocherTypeTitle { get; set; }
    /// <summary>
    /// شناسه نوع یارانه
    /// </summary>
    public string VocherTypeId { get; set; }
    /// <summary>
    /// شناسه یارانه
    /// </summary>
    public string VocherId { get; set; }
}

public class OrderItemEditModel : IBaseViewModel
{
    public int ViewModelId
    {
        get => Id;
        set => Id = value;
    }
    public int Id { get; set; }
    [StringLength(600)] [Required] [StringValidation] public string Title { get; set; }

    [Required] public Guid OrderItemGuid { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int OrderId { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int ProductSalesPlanningId { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int RepresentationId { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int Quantity { get; set; }

    [Required] public decimal UnitPrice { get; set; }

    [Required] public decimal DiscountPercentage { get; set; }

    [Required] public decimal DiscountAmountInclTax { get; set; }
    public decimal? DiscountAmountExclTax { get; set; }

    [Required] public decimal PriceIncDiscount { get; set; }

    [Required] public decimal WagePercentage { get; set; }

    [Required] public decimal WageAmountIncTax { get; set; }

    [Required] public decimal WageAmountExcTax { get; set; }

    [Required] public decimal SubsidyPrice { get; set; }

    [Required] public decimal SubsidyProfitPercentage { get; set; }

    [Required] public decimal SubsidyPriceIncPorfit { get; set; }

    [Required] public decimal SubsidyPriceIncTaxIncPorfit { get; set; }

    [Required] public decimal TaxPercentage { get; set; }

    [Required] public decimal TaxPrice { get; set; }

    [Required] public decimal PriceTotal { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int InstallmentCount { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int SaleType { get; set; }

    [Required] public decimal PricePerInstallment { get; set; }

    [Required] public decimal ProfitPercentage { get; set; }

    [Required] public decimal ProfitPrice { get; set; }
    public decimal? RefundedAmount { get; set; }

    [Required] public decimal OriginalProductCost { get; set; }
    [StringValidation] public string AttributeDescription { get; set; }
    [StringValidation] public string AttributesXml { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int DownloadCount { get; set; }
    public bool IsDownloadActivated { get; set; }
    public int? LicenseDownloadId { get; set; }
    public decimal? ItemWeight { get; set; }
    [StringLength(300)][StringValidation]
    public string Code { get; set; }
    [Range(0, int.MaxValue)]
    [Required] public int AccessLevelId { get; set; }
}

public class OrderItemSimpleViewModel : IBaseViewModel
{
    public int ViewModelId
    {
        get => Id;
        set => Id = value;
    }
    public int Id { get; set; }

    [StringValidation] public string Title { get; set; }

}

public class OrderItemListDuplicateViewModel
{

    public int Id { get; set; }
    /// <summary>
    /// نام محصول
    /// </summary>
    [StringValidation] public string ProductTitle { get; set; }
    /// <summary>
    /// وضعیت سفارش
    /// </summary>
    [StringValidation] public string OrderStatusTitle { get; set; }

}


public class OrderItemFullInfoListViewModel
{
    public int Id { get; set; }
    /// <summary>
    ///  شماره موبایل تحویل گیرنده
    /// </summary>
    [StringValidation] public string RecipientMobileNumber { get; set; }
    [StringValidation] public string ProductTitle { get; set; }
    [StringValidation] public string ProductGroup { get; set; }
    [StringValidation] public string OrderNumber { get; set; }

    [StringValidation] public string NationalCode { get; set; }

    [StringValidation] public string RepresentationCode { get; set; }

    [StringValidation] public string ProductCode { get; set; }

    public int CreationDateInt { get; set; }

    [StringValidation] public string RepresentationTitle { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal PriceIncDiscount { get; set; }
    public DateTime? OrderModificationDate { get; set; }
    [StringValidation] public string OrderStatus { get; internal set; }
    public int OrderStatusType { get; set; }
    public int OrderId { get; set; }
    [StringValidation] public string CompanyTitle { get; internal set; }
    /// <summary>
    /// هزینه ارسال
    /// </summary>
    public decimal? AdditionalShippingCharge { get; set; }
    /// <summary>
    /// آیدی برنامه ریزی دوره
    /// </summary>
    public int ProductSalesPlanningId { get; set; }
    /// <summary>
    /// روزهای تشکیل جلسه
    /// </summary>
    public string CoursePlanningSchedule { get; set; }
}

public class OrderItemRefundedViewModel
{
    /// <summary>
    /// شماره سفارش
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// کانورت شده
    /// </summary>
    public bool IsConverted { get; set; }

    /// <summary>
    /// یارانه خام
    /// </summary>
    public decimal? SubsidyPrice { get; set; }
    /// <summary>
    /// یارانه باکارمزد
    /// </summary>
    public decimal? SubsidyPriceIncProfit { get; set; }
    /// <summary>
    /// یارانه با کارمزد و مالیات
    /// </summary>
    public decimal? SubsidyPriceIncTaxIncProfit { get; set; }
    /// <summary>
    /// شماره کارت
    /// </summary>
    [CanBeNull] public string CardNumber { get; set; }
    /// <summary>
    /// کدملی
    /// </summary>
    [CanBeNull] public string NationalCode { get; set; }

    /// <summary>
    /// تاریخ خرید
    /// </summary>
    public int? PaidDateInt { get; set; }

    /// <summary>
    /// تاریخ عودت
    /// </summary>
    public int? RefundDateInt { get; set; }

    /// <summary>
    /// نوع یارانه
    /// </summary>
    public string SubsidyType { get; set; }
    /// <summary>
    /// عنوان گروه فاکتور
    /// </summary>
    public string FactorGroupTitle { get; set; }

}