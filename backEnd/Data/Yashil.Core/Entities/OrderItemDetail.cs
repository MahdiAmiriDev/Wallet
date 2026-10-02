using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OrderItemDetail  :IBaseEntity<int>,IApplicationBasedEntity
    {
public OrderItemDetail()
{
    InverseParent = new HashSet<OrderItemDetail>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
public int OrderItemId { get; set; }
/// <summary>
///         /// میزان تخفیف (شامل مالیات)
///</summary>
public decimal? DiscountAmountInclTax { get; set; }
/// <summary>
///         /// قیمت پایه ضرب در تعداد محصول
///</summary>
public decimal Price { get; set; }
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
///         /// مبلغ یارانه (شامل سود یارانه)
///</summary>
public decimal? SubsidyPriceIncProfit { get; set; }
/// <summary>
///         /// مبلغ یارانه (شامل مالیات و شامل سود یارانه)
///</summary>
public decimal? SubsidyPriceIncTaxIncProfit { get; set; }
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
///         /// مبلغ سود از تامین کننده
///</summary>
public decimal ProfitPrice { get; set; }
/// <summary>
///         /// مبلغ سایر کسورات از فاکتور تامین کننده
///</summary>
public decimal OtherDeductionsPrice { get; set; }
/// <summary>
///         /// مبلغ بیمه مربوط به فاکتور تامین کننده
///</summary>
public decimal InsurancePrice { get; set; }
/// <summary>
///         /// مبلغ ارزش افزوده مربوط به فاکتور تامین کننده
///</summary>
public decimal ValueAddedPrice { get; set; }
public decimal? Netprice { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    public virtual OrderItemDetail Parent { get; set; }
public virtual ICollection<OrderItemDetail>
    InverseParent { get; set; }
    }
 }
