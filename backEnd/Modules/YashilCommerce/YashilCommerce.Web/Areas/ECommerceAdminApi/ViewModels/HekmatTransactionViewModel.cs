using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class HekmatTransactionListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// شناسه فایل بارگذاری شده
        ///</summary>
        public int AppDocumentId { get; set; }
        /// <summary>
        /// کد
        ///</summary>
        [StringValidation] public string Code { get; set; }
        /// <summary>
        /// عنوان
        ///</summary>
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// توضیحات
        ///</summary>
        [StringValidation] public string Description { get; set; }
        /// <summary>
        /// کد رهگیری پذیرنده-ردیف گردش ترمینال-hekmatId-کد سمت حکمت
        ///</summary>
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        /// <summary>
        /// شماره کارت - CardHashPan
        ///</summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// کد ملی
        ///</summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// تعداد اقساط
        ///</summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        ///</summary>
        public long Price { get; set; }
        /// <summary>
        /// وضعیت تراکنش
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        /// کد خطا
        ///</summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// توکن حکمت - از سمت حکمت ارسال می شود
        ///</summary>
        [StringValidation] public string HekmatToken { get; set; }
        /// <summary>
        /// نتیجه تراکنش حکمت
        ///</summary>
        [StringValidation] public string TransactionResult { get; set; }
        /// <summary>
        /// آیا تراکنش مربوط به خرید آموزشی می باشد
        ///</summary>
        public bool IsLearn { get; set; }
        /// <summary>
        /// آیا عملیات به روز رسانی تراکنش های حکمت کامل انجام شده است
        ///</summary>
        public bool Done { get; set; }
        /// <summary>
        /// ایجاد کننده
        ///</summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// ویرایش کننده
        ///</summary>
        public int? ModifyBy { get; set; }
        /// <summary>
        /// زمان ایجاد
        ///</summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// زمان تغییر
        ///</summary>
        public DateTime? ModificationDate { get; set; }
        /// <summary>
        /// حذف شده
        ///</summary>
        public bool Deleted { get; set; }

    }


    public class HekmatTransactionEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        [StringLength(600)] [StringValidation] public string InstallmentTrackingCode { get; set; }
        [StringLength(60)] [StringValidation] public string CardNumber { get; set; }
       [StringLength(10)][StringValidation] public string NationalCode { get; set; }
        public int InstallmentCount { get; set; }
        public Int64 Price { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string HekmatToken { get; set; }
        [Required] [StringValidation] public string TransactionResult { get; set; }
        public bool IsLearn { get; set; }

    }





    public class HekmatTransactionSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }


    public class HekmatTransactionListCustomViewModel
    {
        /// <summary>
        /// ردیف گردش ترمینال - کد رهگیری پذیرنده
        /// </summary>
        public long HekmatReservationNumber { get; set; }
        /// <summary>
        /// کد ملی صاحب حکمت
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string HekmatCardNumber { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        ///تعداد اقساط 
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// وضعیت تراکنش
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// کد خطا
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// آیا سفارش آموزشی می باشد یا خیر ؟ 
        /// </summary>
        public int IsLearn { get; set; }
    }


    public class UploadedHekmatTransactionListViewModel 
    {
        public int Id { get; set; }
        /// <summary>
        /// ردیف گردش ترمینال - کد رهگیری پذیرنده
        /// </summary>
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        /// <summary>
        /// کد ملی صاحب حکمت
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        ///تعداد اقساط 
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// وضعیت تراکنش
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// کد خطا
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        ///توکن حکمت
        /// </summary>
        [StringValidation] public string HekmatToken { get; set; }
        /// <summary>
        /// نتیجه پرداخت حکمت
        /// </summary>
        [StringValidation] public string TransactionResult { get; set; }
        /// <summary>
        /// تاریخ تایید
        /// </summary>
        public DateTime TransactionDate { get; set; }
    }
}
