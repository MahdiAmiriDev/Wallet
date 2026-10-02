using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ModifiedOrderItemListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string OldOrderItemTitle { get; set; }

        [StringValidation] public string NewOrderTitle { get; set; }

        [StringValidation] public string ModifiedOrderItemTypeTitle { get; set; }

        [StringValidation] public string ModifiedOrderItemStatusTypeTitle { get; set; }

        [StringValidation] public string AssigneeTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        [StringValidation] public string? Description { get; set; }
        public int LastOrderItemId { get; set; }

        [StringValidation] public string? ExpertDescription { get; set; }


    }

    public class ModifiedOrderItemListForModifyViewModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        [StringValidation] public string MobileNumber { get; set; }
        [StringValidation] public string NationalCode { get; set; }
        [StringValidation] public string LastOrderItemTitle { get; set; }
        [StringValidation] public string LastOrderItemCode { get; set; }
        [StringValidation] public string NewOrderItemCode { get; set; }
        public int Quantity { get; set; }
        [StringValidation] public string CompanyTitle { get; internal set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        [StringValidation] public string ProductCode { get; set; }
        [StringValidation] public string OrderNumber { get; set; }
        [StringValidation] public string ModifiedOrderItemType { get; set; }
        [StringValidation] public string ModifiedOrderItemTypeTitle { get; set; }
        [StringValidation] public string ModifiedOrderItemStatusTypeTitle { get; set; }
        public bool AllowEdit { get => false; }
        public int ModifiedOrderItemStatusType { get; set; }
        public bool AllowDelete
        {
            get
            {
                return ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier;
            }
        }
        [StringValidation] public string? Description { get; set; }
        [StringValidation] public string AssigneeTitle { get; internal set; }
        public int LastOrderItemId { get; set; }

        [StringValidation] public string? ExpertDescription { get; set; }
    }

    public class ModifiedOrderItemListForModifyActionViewModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        [StringValidation] public string MobileNumber { get; set; }
        [StringValidation] public string NationalCode { get; set; }
        [StringValidation] public string LastOrderItemTitle { get; set; }
        [StringValidation] public string LastOrderItemCode { get; set; }
        /// <summary>
        /// اختلاف قیمت قدیم با جدید
        /// </summary>
        public decimal PriceDifference { get; set; }
        public int Quantity { get; set; }
        [StringValidation] public string CompanyTitle { get; internal set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        [StringValidation] public string ProductCode { get; set; }
        [StringValidation] public string OrderNumber { get; set; }
        [StringValidation] public string ModifiedOrderItemTypeTitle { get; set; }
        [StringValidation] public string ModifiedOrderItemStatusTypeTitle { get; set; }
        public int? ModifiedOrderItemType { get; set; }
        public bool AllowEdit
        {
            get
            {
                return ModifiedOrderItemStatusType != (int)ModifiedOrderItemStatusTypeEnum.Completed;
            }
        }
        public int ModifiedOrderItemStatusType { get; set; }
        public bool AllowDelete { get => false; }
        [StringValidation] public string? Description { get; set; }
        [StringValidation] public string AssigneeTitle { get; internal set; }
        public int LastOrderItemId { get; set; }
        public bool AllowRefundOrderAndPayment
        {
            get
            {
                if (ModifiedOrderItemType == (int)ModifiedOrderItemTypeEnum.CancelOrderItem &&
                    ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier)
                {
                    return true;
                }
                return false;
            }
        }
        public bool AllowEdiModifiedOrder
        {
            get
            {
                if (ModifiedOrderItemType == (int)ModifiedOrderItemTypeEnum.ChangeOrderItem &&
                    ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier)
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
                if (AllowEdiModifiedOrder || AllowRefundOrderAndPayment)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// اجازه برگشت به قبل اصلاح آیتم سفارش انجام شده
        /// </summary>
        public bool AllowCancelModification { get; set; }

        [StringValidation] public string OldOrderItemTitle { get; set; }
        /// <summary>
        /// آیا آیتم سفارش انتخابی فاکتور شده است؟
        /// </summary>
        public bool OrderItemHasBeenFactored { get; set; }
        /// <summary>
        /// شناسه سفارش 
        /// </summary>
        public int OrderId { get; set; }
    }
    public class ModifiedOrderItemEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int OldOrderItemId { get; set; }
        public int? NewOrderId { get; set; }
        public int? ModifiedOrderItemType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ModifiedOrderItemStatusType { get; set; }
        public int? Assignee { get; set; }
        [StringValidation] public string Description { get; set; }
        [StringValidation] public string? ExpertDescription { get; set; }

    }
    public class ModifiedOrderItemSimpleEditModel
    {
        public int Id { get; set; }
        public int? ProductSalePlanningId { get; set; }
        public int? RepresentationId { get; set; }
        public bool? RejectRequest { get; set; }
        [StringValidation] public string? ExpertDescription { get; set; }
    }

    public class ModifiedOrderItemSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

}
