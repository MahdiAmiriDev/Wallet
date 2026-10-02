using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ItemGroup  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ItemGroup()
{
    AdditionalUserProp = new HashSet<AdditionalUserProp>();
    CustomerClubPaymentInstallment = new HashSet<CustomerClubPaymentInstallment>();
    CustomerClubPaymentInstallmentOldSite = new HashSet<CustomerClubPaymentInstallmentOldSite>();
    InverseNextItemGroup = new HashSet<ItemGroup>();
    InverseParent = new HashSet<ItemGroup>();
    OldSiteRefundDetail = new HashSet<OldSiteRefundDetail>();
    Payment = new HashSet<Payment>();
    ProductSalesPlanning = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningItemGroup = new HashSet<ProductSalesPlanningItemGroup>();
    RefundDetailRefundDetailForPaymentGroup = new HashSet<RefundDetail>();
    RefundDetailRefundDetailGroup = new HashSet<RefundDetail>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// گروه پدر
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// موجودیت
///</summary>
public int AppEntityId { get; set; }
/// <summary>
///         /// وضعیت گروه
///</summary>
public int ItemGroupStatusType { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
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
///         /// تاریخ ثبت نهایی
///</summary>
public DateTime? ConfirmDate { get; set; }
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
///         /// کد دسته بندی گروه
///</summary>
public int? ItemGroupSubId { get; set; }
/// <summary>
///         /// گروه بعدی
///</summary>
public int? NextItemGroupId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual AppEntity AppEntity { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ItemGroup NextItemGroup { get; set; }
    public virtual ItemGroup Parent { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserProp { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallment { get; set; }
public virtual ICollection<CustomerClubPaymentInstallmentOldSite>
    CustomerClubPaymentInstallmentOldSite { get; set; }
public virtual ICollection<ItemGroup>
    InverseNextItemGroup { get; set; }
public virtual ICollection<ItemGroup>
    InverseParent { get; set; }
public virtual ICollection<OldSiteRefundDetail>
    OldSiteRefundDetail { get; set; }
public virtual ICollection<Payment>
    Payment { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanning { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    ProductSalesPlanningItemGroup { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailRefundDetailForPaymentGroup { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailRefundDetailGroup { get; set; }
    }
 }
