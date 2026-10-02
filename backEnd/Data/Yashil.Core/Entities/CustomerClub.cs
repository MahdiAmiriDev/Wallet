using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CustomerClub  :IBaseEntity<int>
    {
public CustomerClub()
{
    AdditionalUserProp = new HashSet<AdditionalUserProp>();
    AppEntityCustomerClub = new HashSet<AppEntityCustomerClub>();
    ApplicationCustomerClub = new HashSet<ApplicationCustomerClub>();
    CategoryCustomerClub = new HashSet<CategoryCustomerClub>();
    CustomerClubCard = new HashSet<CustomerClubCard>();
    CustomerClubPaymentMethodType = new HashSet<CustomerClubPaymentMethodType>();
    Factor = new HashSet<Factor>();
    Order = new HashSet<Order>();
    ProductSalesPlanningDiscount = new HashSet<ProductSalesPlanningDiscount>();
    SaleConfigurationAttribute = new HashSet<SaleConfigurationAttribute>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// پرداخت با کیف پول
///</summary>
public bool PayWithWallet { get; set; }
/// <summary>
///         /// مبتنی بر IPG
///</summary>
public bool IpgBased { get; set; }
/// <summary>
///         /// مبتنی بر سرویس
///</summary>
public bool ServiceBased { get; set; }
/// <summary>
///         /// مبتنی بر پایگاه داده محلی
///</summary>
public bool IsLocalClub { get; set; }
/// <summary>
///         /// آدرس سرویس
///</summary>
public string ServiceUrl { get; set; }
/// <summary>
///         /// آدرس سرویس
///</summary>
public string IpgPageUrl { get; set; }
/// <summary>
///         /// نام کاربری سرویس
///</summary>
public string UserName { get; set; }
/// <summary>
///         /// کلمه عبور سرویس
///</summary>
public string Password { get; set; }
/// <summary>
///         /// مشمول یارانه
///</summary>
public bool SupportSubsidy { get; set; }
/// <summary>
///         /// ارسال کد خاص باشگاه
///</summary>
public bool HasCustomCode { get; set; }
/// <summary>
///         /// ارسال کد ملی
///</summary>
public bool HasNationalCode { get; set; }
/// <summary>
///         /// ارسال شماره موبایل
///</summary>
public bool HasMobileNumber { get; set; }
public string CardNumberLabel { get; set; }
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
///         /// بر اساس آدرس می باشد
///</summary>
public bool AddressBased { get; set; }
/// <summary>
///         /// نوع مدل ثبت نام
///</summary>
public int RegistrationModeType { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData RegistrationModeTypeNavigation { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserProp { get; set; }
public virtual ICollection<AppEntityCustomerClub>
    AppEntityCustomerClub { get; set; }
public virtual ICollection<ApplicationCustomerClub>
    ApplicationCustomerClub { get; set; }
public virtual ICollection<CategoryCustomerClub>
    CategoryCustomerClub { get; set; }
public virtual ICollection<CustomerClubCard>
    CustomerClubCard { get; set; }
public virtual ICollection<CustomerClubPaymentMethodType>
    CustomerClubPaymentMethodType { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
public virtual ICollection<ProductSalesPlanningDiscount>
    ProductSalesPlanningDiscount { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttribute { get; set; }
    }
 }
