using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class UploadedFactorInstallmentPaymentListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }
		public int? ParentId { get; set; }
	
		public int Id { get; set; }
	
		public string Code { get; set; }
	
		public string CompanyOrRepresentationTitle { get; set; }
	
		public string FactorCode { get; set; }
	
		public int NumberOfInstallmentsPaid { get; set; }
	
		public decimal SumAmountPaymentToSupplier { get; set; }
	
		public decimal InstallmentAmount { get; set; }
	
		public decimal RefundAmount { get; set; }
	
		public decimal PaidAmount { get; set; }
	
		public decimal RemainingPaymentToSupplier { get; set; }
	
		public decimal TotalPaymentToSupplier { get; set; }
	
		public int InstallmentNumber { get; set; }
	
		public int NumberOfInstallmentsRemaining { get; set; }
	
		public string AccountNumber { get; set; }
	
		public string ShebaNumber { get; set; }
	
		public string BankAccountOwner { get; set; }
	
		public string BankName { get; set; }
	
		public string AppDocumentTitle { get; set; }
	
		public int YearMonth { get; set; }
	
		public int FactorPaymentInstallmentId { get; set; }
	
		public bool? CodeIsInvalid { get; set; }
	
		public bool? FactorCodeIsInvalid { get; set; }
	
		public bool? SumAmountPaymentToSupplierIsInvalid { get; set; }
	
		public bool? InstallmentAmountIsInvalid { get; set; }
	
		public bool? InstallmentNumberIsInvalid { get; set; }
	
		public bool? NumberOfInstallmentsRemainingIsInvalid { get; set; }
	
		public bool? AccountNumberIsInvalid { get; set; }
	
		public bool? ShebaNumberIsInvalid { get; set; }
	
		public bool? BankAccountOwnerIsInvalid { get; set; }
	
		public bool? FactorPaymentInstallmentIdIsInvalid { get; set; }

        public bool? PaidAmountIsInvalid { get; set; }

        public bool? RefundAmountIsInvalid { get; set; }

		public string ErrorMessage { get; set; }
    }


	public class UploadedFactorInstallmentPaymentEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		[StringLength(300)]
		public string Code { get; set; }		
	
		[StringLength(400)]
		[Required]
		
		public string CompanyOrRepresentationTitle { get; set; }		
	
		[StringLength(400)]
		[Required]
		
		public string FactorCode { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int NumberOfInstallmentsPaid { get; set; }		
	
		[Required]
		
		public decimal SumAmountPaymentToSupplier { get; set; }		
	
		[Required]
		
		public decimal InstallmentAmount { get; set; }		
	
		[Required]
		
		public decimal RefundAmount { get; set; }		
	
		[Required]
		
		public decimal PaidAmount { get; set; }		
	
		[Required]
		
		public decimal RemainingPaymentToSupplier { get; set; }		
	
		[Required]
		
		public decimal TotalPaymentToSupplier { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int InstallmentNumber { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int NumberOfInstallmentsRemaining { get; set; }		
	
		[StringLength(100)]
		[Required]
		
		public string AccountNumber { get; set; }		
	
		[StringLength(100)]
		[Required]
		
		public string ShebaNumber { get; set; }		
	
		[StringLength(400)]
		[Required]
		
		public string BankAccountOwner { get; set; }		
	
		[StringLength(400)]
		[Required]
		
		public string BankName { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AppDocumentId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int YearMonth { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int FactorPaymentInstallmentId { get; set; }		
		
	}

	public class UploadedFactorInstallmentPaymentSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
		
		
		public string Title { get; set; }
		
	}
	public class UploadedFactorInstallmentPaymentReferenceGroupViewModel
    {
        /// <summary>
        /// شناسه 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// سال و ماه
        /// </summary>
        public int YearMonth { get; set; }
        /// <summary>
        /// زمان ثبت جدول
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        ///  زمان ثبت جدول به فارسی
        /// </summary>
        public string CreationPersianDateTime { get; set; }
        /// <summary>
        /// شناسه مرجع پرداخت
        /// </summary>
        public string PaymentReferenceId { get; set; }
        /// <summary>
        /// زمان ثبت شناسه مرجع پرداخت
        /// </summary>
        public string PaymentReferencePersianDateTime { get; set; }
    }

	public class UploadedFactorInstallmentPaymentReferenceViewModel
	{
        public int Id { get; set; }
        /// <summary>
        /// نام دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyOrRepresentationTitle { get; set; }
        /// <summary>
        /// کد نمایندگی مرکز
        /// </summary>
        public string CompanyOrRepresentationCode { get; set; }
        /// <summary>
        /// مبلغ قسط
        /// </summary>
        public string InstallmentAmount { get; set; }
        /// <summary>
        /// شماره قسط
        /// </summary>
        public string InstallmentNumber { get; set; }
    }

}
