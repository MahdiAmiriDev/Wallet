using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class AppDocument  :IBaseEntity<int>,IApplicationBasedEntity
    {
public AppDocument()
{
    HekmatApprovedTransaction = new HashSet<HekmatApprovedTransaction>();
    HekmatPaymentGatewayTransactionBulkRequestDetail = new HashSet<HekmatPaymentGatewayTransactionBulkRequestDetail>();
    HekmatTransaction = new HashSet<HekmatTransaction>();
    MediaSessionViewingHistory = new HashSet<MediaSessionViewingHistory>();
    UploadedBulkPasswordReset = new HashSet<UploadedBulkPasswordReset>();
    UploadedFactorInstallmentPayment = new HashSet<UploadedFactorInstallmentPayment>();
    UploadedFactorPayment = new HashSet<UploadedFactorPayment>();
    UploadedProductItem = new HashSet<UploadedProductItem>();
    UploadedQuestion = new HashSet<UploadedQuestion>();
    UploadedUser = new HashSet<UploadedUser>();
    UploadedVoucher = new HashSet<UploadedVoucher>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// نوع سند
///</summary>
public int DocTypeId { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// نام واقعی سند
///</summary>
public string OrginalName { get; set; }
/// <summary>
///         /// کد مالک سند
///</summary>
public long ObjectId { get; set; }
/// <summary>
///         /// سند
///</summary>
public byte[] DocumentFile { get; set; }
/// <summary>
///         /// توضیح کوتاه
///</summary>
public string ShortDescription { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int? DisplayOrder { get; set; }
/// <summary>
///         /// توضیح کامل
///</summary>
public string Description { get; set; }
/// <summary>
///         /// پسوند فایل
///</summary>
public string Extension { get; set; }
/// <summary>
///         /// نوع داده
///</summary>
public string ContentType { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// کد
///</summary>
public Guid? Code { get; set; }
/// <summary>
///         /// فایل روی دسیک ذخیره شده است؟
///</summary>
public bool SaveToDisk { get; set; }
public bool ConvertIsDone { get; set; }
/// <summary>
///         /// مقدار حافظه اشغال شده توسط فایل به صورت بایت
///</summary>
public long? FileSize { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual DocType DocType { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<HekmatApprovedTransaction>
    HekmatApprovedTransaction { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestDetail>
    HekmatPaymentGatewayTransactionBulkRequestDetail { get; set; }
public virtual ICollection<HekmatTransaction>
    HekmatTransaction { get; set; }
public virtual ICollection<MediaSessionViewingHistory>
    MediaSessionViewingHistory { get; set; }
public virtual ICollection<UploadedBulkPasswordReset>
    UploadedBulkPasswordReset { get; set; }
public virtual ICollection<UploadedFactorInstallmentPayment>
    UploadedFactorInstallmentPayment { get; set; }
public virtual ICollection<UploadedFactorPayment>
    UploadedFactorPayment { get; set; }
public virtual ICollection<UploadedProductItem>
    UploadedProductItem { get; set; }
public virtual ICollection<UploadedQuestion>
    UploadedQuestion { get; set; }
public virtual ICollection<UploadedUser>
    UploadedUser { get; set; }
public virtual ICollection<UploadedVoucher>
    UploadedVoucher { get; set; }
    }
 }
