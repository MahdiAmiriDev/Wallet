using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;
using YashilPayment.Core.Enums;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
	public class RefundPaymentTransactionItemRequestListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }
		public int? ParentId { get; set; }
	
		public int Id { get; set; }
	
		public string ParentTitle { get; set; }
	
		public string YashilHekmatTerminalTitle { get; set; }
	
		public string ReservationNumber { get; set; }
	
		public string Code { get; set; }
	
		public string RefundPaymentTransactionItemRequestTypeTitle { get; set; }
	
		public string RefundPaymentTransactionItemRequestStatusTitle { get; set; }
	
		public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
	}


	public class RefundPaymentTransactionItemRequestEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		public int? ParentId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int YashilHekmatTerminalId { get; set; }		
	
		[StringLength(100)]
		[Required]
		
		public string ReservationNumber { get; set; }		
	
		[StringLength(300)]
		public string Code { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int RefundPaymentTransactionItemRequestType { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int RefundPaymentTransactionItemRequestStatus { get; set; }		
	
		[StringLength(800)]
		public string CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }		
	
	}

	public class RefundPaymentTransactionItemRequestSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
		
		public int ParentId { get; set; }
		
		
		public string Title { get; set; }
		
	}
	/// <summary>
	/// این مدل برای رد یا تایید درخواست لغو و ویا ویرایش سفارش استفاده می شود
	/// </summary>
    public class PaymentTransactionItemRefundRequestSimpleEditModel
    {
		/// <summary>
		/// شناسه درخواست عودت تراکنش پرداخت
		/// </summary>
        public int PaymentTransactionItemRefundRequestId { get; set; }
		/// <summary>
		/// آیا درخواست لغو شود
		/// </summary>
        public bool? RejectRequest { get; set; }
    }
	/// <summary>
	/// مدلی که برای تایید یا رد درخواست عودت تراکشن داده شده است
	/// </summary>
    public class PaymentTransactionItemRefundRequestListForModifyActionViewModel
    {

        public int Id { get; set; }

        public string YashilHekmatTerminalTitle { get; set; }

        public string ReservationNumber { get; set; }

        public string Code { get; set; }
        public int RefundPaymentTransactionItemRequestType { get; set; }

        public int RefundPaymentTransactionItemRequestStatus { get; set; }

        public string RefundPaymentTransactionItemRequestTypeTitle { get; set; }

        public string RefundPaymentTransactionItemRequestStatusTitle { get; set; }
        public bool allowRefundTransactionAndTerminalPayment
        {
            get
            {
                if (RefundPaymentTransactionItemRequestType == (int)RefundPaymentTransactionItemRequestTypeEnum.Cancel &&
                    RefundPaymentTransactionItemRequestStatus == (int)RefundPaymentTransactionItemRequestStatusEnum.InitialRequestBySupplier)
                {
                    return true;
                }
                return false;
            }
        }
       
        public bool AllowRejectRequest
        {
            get
            {
                if (allowRefundTransactionAndTerminalPayment)
                {
                    return true;
                }
                return false;
            }
        }
       
    }
}      
