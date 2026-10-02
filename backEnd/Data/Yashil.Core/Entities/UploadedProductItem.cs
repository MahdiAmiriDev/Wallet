using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedProductItem  :IBaseEntity<int>
    {
public int Id { get; set; }
public int AppDocumentId { get; set; }
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
///         /// نوع
///</summary>
public string Type { get; set; }
/// <summary>
///         /// دسته اصلی(پایه)
///</summary>
public string ParentCategoryTilte { get; set; }
/// <summary>
///         /// زیر دسته (رشته)
///</summary>
public string ChildCategoryTilte { get; set; }
/// <summary>
///         /// مبلغ با تخفیف آزمون
///</summary>
public string TestBasePrice { get; set; }
/// <summary>
///         /// آیا مبلغ با تخفیف آزمون اشتباه میباشد
///</summary>
public bool? TestBasePriceIsInValid { get; set; }
public string Published { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// هزینه ارسال
///</summary>
public string DeliveryCost { get; set; }
/// <summary>
///         /// آیا قیمت پایه  اشتباه میباشد
///</summary>
public bool? DeliveryCostIsInValid { get; set; }
/// <summary>
///         /// پیام خطا
///</summary>
public string ErrorMessage { get; set; }
/// <summary>
///         /// قیمت با تخفیف کتاب
///</summary>
public string BookBasePrice { get; set; }
/// <summary>
///         /// آیا قیمت با تخفیف کتاب اشتباه می باشد 
///</summary>
public bool? BookBasePriceIsInValid { get; set; }
/// <summary>
///         /// آیا وضعیت انتشار مقدار اشتباه دارد 
///</summary>
public bool? PublishedIsInValid { get; set; }
/// <summary>
///         /// دسته اصلی / زیر دسته
///</summary>
public string CategoryTitle { get; set; }
/// <summary>
///         /// نوع ووچر
///</summary>
public string VocherTypeTitle { get; set; }
/// <summary>
///         /// شناسه نوع ووچر
///</summary>
public int? VocherTypeId { get; set; }
/// <summary>
///         /// عنوان دسته بندی محصول خطا دارد
///</summary>
public bool? CategoryTitleIsInValid { get; set; }
/// <summary>
///         /// نوع محصول معتبر نمی باشد
///</summary>
public bool? TypeIsInValid { get; set; }
/// <summary>
///         /// نمایش زیرنویس
///</summary>
public string ProductSalePlanningDisplaySubTitle { get; set; }
/// <summary>
///         /// نمایش زیرنویس نا معتبر است
///</summary>
public bool? ProductSalePlanningDisplaySubTitleIsInvalid { get; set; }
/// <summary>
///         /// درصد تخفیف طرح باقرالعلوم
///</summary>
public string TbaoCustomerClubDiscountPercentage { get; set; }
/// <summary>
///         /// آیا درصد تخفیف طرح باقرالعلوم اشتباه میباشد
///</summary>
public bool? TbaoCustomerClubDiscountPercentageIsInValid { get; set; }
/// <summary>
///         /// درصد تخفیف سایر باشگاه ها
///</summary>
public string OtherCustomerClubDiscountPercentage { get; set; }
/// <summary>
///         /// آیا درصد تخفیف سایر باشگاه ها اشتباه میباشد
///</summary>
public bool? OtherCustomerClubDiscountPercentageIsInValid { get; set; }
/// <summary>
///         /// شناسه گروه محصول برای فاکتور و کانورت
///</summary>
public string ProductGroupIdForBook { get; set; }
public bool? ProductGroupIdForBookIsInvalid { get; set; }
public string ProductGroupIdForExam { get; set; }
public bool? ProductGroupIdForExamIsInvalid { get; set; }
public string FullDescription { get; set; }
public string StartDate { get; set; }
public string EndDate { get; set; }
public string MaxCapacity { get; set; }
public string TeacherUserName { get; set; }
public string StudentUserName { get; set; }
public string Representation { get; set; }
public string Sku { get; set; }
public bool? ProductIdIsInvalid { get; set; }
public bool? ProductSalesPlanningTeacherUserNameIsInvalid { get; set; }
public bool? ProductSalesPlanningStudentUserNameIsInvalid { get; set; }
public bool? RepresentationIsInvalid { get; set; }
public bool? ProductSalesPlanningIsInvalid { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual VocherType VocherType { get; set; }
    }
 }
