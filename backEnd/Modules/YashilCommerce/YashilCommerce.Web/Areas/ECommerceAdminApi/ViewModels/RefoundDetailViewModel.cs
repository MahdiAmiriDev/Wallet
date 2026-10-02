using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class RefundDetailListViewModel
    {
        public int Id { get; set; }
        public int PaidDateInt { get; set; }
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        public string TerminalNumber { get; set; }
         

        [StringValidation] public string RefundTypeTitle { get; set; }

        [StringValidation] public string RefundStatusTypeTitle { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        ///  کد پرداخت
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// کد سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationName { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }

        /// <summary>
        /// نام استان
        /// </summary>
        [StringValidation] public string ProvinceTitle { get; set; }
        /// <summary>
        /// نام شهر
        /// </summary>
        [StringValidation] public string CityTitle { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }

        public long OldPaymentAmount { get; set; }

        public long NewPaymentAmount { get; set; }

        public long? RefundAmount { get; set; }

        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// اجازه تایید و یا رد آیتم
        /// </summary>
        public bool AllowConfirm { get; set; }

        public DateTime? RefundAmountSetDate { get; set; }
        [StringValidation] public string RefundAmountSetByFullName { get; set; }
        public bool Confirmed { get; set; }
        /// <summary>
        /// آیا اطلاعات حساب دریافت شده است یا خیر؟
        /// </summary>
        public bool IsSetPersonBankAccount { get; set; }
        /// <summary>
        /// عنوان گروه بندی عودت وجه به همراه پدرش
        /// </summary>
        [StringValidation] public string RefundDetailGroupTitle { get; set; }
        /// <summary>
        /// عنوان گروه بندی پرداخت عودت وجه بهمراه پدرش
        /// </summary>
        [StringValidation] public string RefundDetailForPaymentGroupTitle { get; set; }

    }

    public class RefundDetailStatisticViewModel
    {
        public int Id { get; set; }
        public int PaidDateInt { get; set; }
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        public string TerminalNumber { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        ///  کد پرداخت
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// کد سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationName { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }

        /// <summary>
        /// نام استان
        /// </summary>
        [StringValidation] public string ProvinceTitle { get; set; }
        /// <summary>
        /// نام شهر
        /// </summary>
        [StringValidation] public string CityTitle { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }

        public long OldPaymentAmount { get; set; }

        public long NewPaymentAmount { get; set; }

        public long? RefundAmount { get; set; }
        public long? AnnouncedRefundAmount { get; set; }

        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// اجازه وارد کردن مبلغ عودت وجه توسط کاربر
        /// </summary>
        public bool AllowAnnouncedRefundAmount { get; set; }

    }


    public class RefundDetailEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringValidation] public string Description { get; set; }
        public int? RefundType { get; set; }
        public int? RefundStatusType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int RefundId { get; set; }
        public int? RefundGroupId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PaymentId { get; set; }
        public int? PersonBankAccountId { get; set; }

        [Required] public decimal OldPaymentAmount { get; set; }

        [Required] public decimal NewPaymentAmount { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundTime { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        /// <summary>
        /// توضیحات مربوط به بایگانی
        /// </summary>
        [StringValidation] public string ArchiveDescription { get; set; }
        /// <summary>
        /// انتصاب شده به کاربر جهت پیگیری
        /// </summary>
        public int? Assignee { get; set; }
    }


    public class RefundDetailCustomListViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public int PaidDateInt { get; set; }
        /// <summary>
        /// تاریخ ثبت عودت
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// ردیف گردش ترمینال
        /// </summary>
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        /// <summary>
        /// نوع عودت وجه : آنلاین یا حکمت
        /// </summary>
        [StringValidation] public string RefundTypeTitle { get; set; }
        /// <summary>
        /// وضعیت عودت وجه
        /// </summary>
        [StringValidation] public string RefundStatusTypeTitle { get; set; }
        /// <summary>
        /// کد سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }

        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationName { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// شماره موبایل تحویل گیرنده
        /// </summary>
        [StringValidation] public string? MobileNumber { get; set; }
        /// <summary>
        ///  شماره کارت حکمت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// مبلغ قبلی پرداخت
        /// </summary>
        public long OldPaymentAmount { get; set; }
        /// <summary>
        /// مبلغی که باید به کاربر پرداخت شود
        /// </summary>
        public long? RefundAmount { get; set; }
        /// <summary>
        /// نام بانک
        /// </summary>
        [StringValidation] public string BankTypeTitle { get; set; }
        /// <summary>
        /// نام شعبه
        /// </summary>
        [StringValidation] public string BranchName { get; set; }
        /// <summary>
        /// شماره حساب
        /// </summary>
        [StringValidation] public string AccountNumber { get; set; }
        /// <summary>
        /// شماره شبا
        /// </summary>
        [StringValidation] public string ShebaNumber { get; set; }
        /// <summary>
        /// نام گروه عودت وجه
        /// </summary>
        [StringValidation] public string RefundDetailGroupTitle { get; set; }
        /// <summary>
        /// نام 
        /// </summary>
        [StringValidation] public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [StringValidation] public string LastName { get; set; }
        /// <summary>
        /// نام کاربری که عودت وجه را ثبت کرده است
        /// </summary>
        [StringValidation] public string RefundCreatorName { get; set; }
        /// <summary>
        ///نام صاحب حساب 
        /// </summary>
        [StringValidation] public string BankAccountOwnerFullName { get; set; }

        [StringValidation] public string CreateByUserName { get; set; }
        [StringValidation] public string CreateByFullName { get; set; }
        /// <summary>
        /// سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// پرداخت
        /// </summary>
        public int PaymentId { get; set; }
       
        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        [StringValidation] public string PaymentTitle { get; set; }
        /// <summary>
        /// تعداد رویدادهای ثبت شده
        /// </summary>
        public int IssueLogWorkCount { get; set; }
        /// <summary>
        /// اجازه برای بایگانی
        /// </summary>
        public bool AllowArchive { get; set; }
        /// <summary>
        /// اجازه برای حذف از بایگانی
        /// </summary>
        public bool AllowUnArchive { get; set; }
        /// <summary>
        /// انتصاب شده به کاربر جهت پیگیری
        /// </summary>
        public int Assignee { get; set; }
        /// <summary>
        /// انتصاب شده به کاربر جهت پیگیری
        /// </summary>
        [StringValidation] public string AssigneeTitle { get; set; }
        /// <summary>
        /// شناسه حساب بانکی
        /// </summary>
        public int? PersonBankAccountId{ get; set; }

        /// <summary>
        /// اجازه پاک کردن حساب بانکی از عودت وجه 
        /// </summary>
        public bool AllowRemovePersonBankAccount { get; set; }

    }

    public class RefundDetailSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// توضیحات بایگانی
        /// </summary>
        [StringValidation] public string ArchiveDescription { get; set; }
    }
    public class RefundDetailConfirmViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public long? AnnouncedRefundAmount { get; set; }
    }
    public class RefundDetailPersonBankAccountEditModel
    {
        public int RefundDetailId { get; set; }
        public int PersonBankAccountId { get; set; }
    }
    public class RefundDetailFullInfoListViewModel
    {
        public int Id { get; set; }
        public int PaidDateInt { get; set; }
        [StringValidation] public string InstallmentTrackingCode { get; set; }
        public string TerminalNumber { get; set; }


        [StringValidation] public string RefundTypeTitle { get; set; }

        [StringValidation] public string RefundStatusTypeTitle { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        ///  کد پرداخت
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// کد سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationName { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// نام شهر
        /// </summary>
        [StringValidation] public string CityTitle { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }

        public long OldPaymentAmount { get; set; }

        public long NewPaymentAmount { get; set; }

        public long? RefundAmount { get; set; }
        public DateTime? RefundAmountSetDate { get; set; }
        [StringValidation] public string RefundAmountSetByFullName { get; set; }
        public bool Confirmed { get; set; }
        /// <summary>
        /// آیا اطلاعات حساب دریافت شده است یا خیر؟
        /// </summary>
        public bool IsSetPersonBankAccount { get; set; }
        /// <summary>
        /// عنوان گروه بندی عودت وجه به همراه پدرش
        /// </summary>
        [StringValidation] public string RefundDetailGroupTitle { get; set; }
        /// <summary>
        /// عنوان گروه بندی پرداخت عودت وجه بهمراه پدرش
        /// </summary>
        [StringValidation] public string RefundDetailForPaymentGroupTitle { get; set; }

        /// <summary>
        /// تاریخ ثبت عودت
        /// </summary>
        public DateTime CreationDate { get; set; }

    }

    public class RefundAssignedUserListViewModel
    {
        public int Count { get; set; }
        [StringValidation] public string AssigneeFullName { get; set; }    
    }

}
