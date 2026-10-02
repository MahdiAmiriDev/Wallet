using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    /// <summary>
    /// ویودمدل عمومی مربوط به گروه بندی
    /// هرگروه بندی مخصوص به خود باید از این ویومدل ارث بری کند و مپینگ مربوط به خود را داشته باشد
    /// </summary>
    public class ItemGroupListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string ParentTitle { get; set; }

        public int ItemGroupType { get; set; }

        public int ItemGroupStatusType { get; set; }

        public DateTime? ConfirmDate { get; set; }

        
        /// <summary>
        /// تعداد آیتمهای گروه بندی
        /// </summary>
        public int ItemGroupingItemCount { get; set; }

        public int ChildsCount { get; set; }

        /// <summary>
        /// نمایش آیتم های گروه 
        /// </summary>
        public bool AllowModifyItemGroupItems
        {
            get
            {
                if (ChildsCount > 0 || ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed)
                {
                    return false;
                }
                else if (ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial && ItemGroupingItemCount >= 0)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///  نمایش دکمه ثبت نهایی
        /// </summary>
        public bool AllowConfirmItemGroupBtn
        {
            get
            {
                if (ChildsCount > 0 || ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed)
                {
                    return false;
                }
                else if (ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial && ItemGroupingItemCount > 0)
                {
                    return true;
                }

                return false;
            }
        }
        /// <summary>
        /// اجازه دانلود گزارش
        /// </summary>
        public bool AllowDownloadReport => ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed;
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        /// <summary>
        /// شناسه گروه بعدی
        /// این فیلد برای گروه هایی مانند مشاغل کاربرد دارد
        /// یه این منظور که کاربران یک گروه بعد از ظرایط امکان اساین به گروه بعدی را دارند
        /// </summary>
        public int? NextItemGroupId { get; set; }
    }
    /// <summary>
    /// ویومدل مربوط به پرداختی های کیف پول باگشاه مشتریان
    /// این ویومدل برای این تعریف شده است زیرا
    /// گروه بندی ها بنا به آیتمهایی که درون خود نگهداری میکنند بیزینس متفاوتی دارند
    /// و هنگام mapping
    /// باید شخصی سازی شوند
    /// این شخصی سازی بیشتر مربوط به
    /// AllowEdit,AllowDelete
    /// میباشد
    /// زیرا در هنگام اجازه حذف کردن تعداد آیتمهای گروه چک میشود و چون آین ایتم ها میتواند payment یا هرچیزی باشد
    /// پس باید مپینگ مربوط به خود را داشته باشند. 
    /// </summary>
    public class WalletPaymentGroupListViewModel : ItemGroupListViewModel
    {
        
    }


    public class CustomerClubPaymentInstallmentGroupListViewModel : ItemGroupListViewModel
    {

    }
    public class RefundDetailGroupListViewModel : ItemGroupListViewModel
    {

    }

    public class RefundDetailForPaymentPaymentGroupListViewModel : ItemGroupListViewModel
    {

    }
    public class AdditionalUserPropGroupListViewModel : ItemGroupListViewModel
    {

    }
    /// <summary>
    /// product sales planning group ویودمدل  مربوط به گروه بندی 
    /// </summary>
    public class ProductSalesPlanningGroupListViewModel : ItemGroupListViewModel
    {

    }
    public class ItemGroupEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]  [Required] [StringValidation] public string Title { get; set; }
        public int? ParentId { get; set; }
        [Range(0, int.MaxValue)] [Required] 
        public int ItemGroupType { get; set; }
        [Range(0,int.MaxValue)]
        public int ItemGroupSubId { get; set; }
        [Range(0, int.MaxValue)] [Required] public int ItemGroupStatusType { get; set; }
        [StringValidation] public string Description { get; set; }
        public DateTime? ConfirmDate { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
        /// <summary>
        /// شناسه گروه بعدی
        /// این فیلد برای گروه هایی مانند مشاغل کاربرد دارد
        /// یه این منظور که کاربران یک گروه بعد از ظرایط امکان اساین به گروه بعدی را دارند
        /// </summary>
        public int? NextItemGroupId { get; set; }

    }


    public class ItemGroupSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public int ParentId { get; set; }

        public string Code { get; set; }
    }

 
}