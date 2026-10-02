using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ShoppingCartItem  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public Guid Code { get; set; }
/// <summary>
///         /// نمایندگی
///</summary>
public int RepresentationId { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int ShoppingCartType { get; set; }
/// <summary>
///         /// برنامه ریزی فروش
///</summary>
public int ProductSalesPlanningId { get; set; }
public string AttributesXml { get; set; }
/// <summary>
///         /// قیمت مشتری
///</summary>
public decimal CustomerEnteredPrice { get; set; }
/// <summary>
///         /// تعداد
///</summary>
public int Quantity { get; set; }
/// <summary>
///         /// تاریخ شروع اجاره
///</summary>
public DateTime? RentalStartDateUtc { get; set; }
/// <summary>
///         /// تاریخ پایان اجاره
///</summary>
public DateTime? RentalEndDateUtc { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// ایجاد کننده
///</summary>
public int CreateBy { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
/// <summary>
///         /// ویرایش کننده
///</summary>
public int? ModifyBy { get; set; }
/// <summary>
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual CommonBaseData ShoppingCartTypeNavigation { get; set; }
    }
 }
