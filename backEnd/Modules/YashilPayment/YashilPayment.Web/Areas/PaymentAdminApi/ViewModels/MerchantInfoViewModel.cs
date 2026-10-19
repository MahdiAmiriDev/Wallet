using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class MerchantInfoListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// کد
        /// </summary>
        [StringValidation] public string? Code { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>

        [StringValidation] public string? Title { get; set; }
        /// <summary>
        /// سطح دسترسی
        /// </summary>

        [StringValidation] public string? AccessLevelTitle { get; set; }
        /// <summary>
        /// نام پذیرنده
        /// </summary>

        [StringValidation] public string MerchantName { get; set; }
        /// <summary>
        /// شهر
        /// </summary>

        [StringValidation] public string? CityTitle { get; set; }
        /// <summary>
        /// تلفن ثابت
        /// </summary>

        [StringValidation] public string? PhoneNumber { get; set; }
        /// <summary>
        /// موبایل
        /// </summary>

        [StringValidation] public string? MobileNumber { get; set; }
        /// <summary>
        /// کد پستی
        /// </summary>

        [StringValidation] public string? PostalCode { get; set; }
        /// <summary>
        /// کاربر پذیرنده
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// نام کاربری پذیرنده
        /// </summary>

        [StringValidation] public string Username { get; set; }

    }
    public class MerchantInfoEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// کد
        /// </summary>
        [StringLength(300)][StringValidation]
        public string? Code { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringLength(600)]
        [StringValidation] public string? Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringValidation] public string? Description { get; set; }
        /// <summary>
        /// سطح دسترسی
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        /// <summary>
        /// نام پذیرنده
        /// </summary>
        [Required] [StringValidation] public string MerchantName { get; set; }
        /// <summary>
        /// شهر
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// تلفن ثابت
        /// </summary>
        [StringLength(100)] 
        [StringValidation]
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// موبایل
        /// </summary>
        [StringLength(22)]
        [StringValidation] public string? MobileNumber { get; set; }
        /// <summary>
        /// کد پستی
        /// </summary>
        [StringLength(100)] [StringValidation] public string? PostalCode { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        [StringValidation] public string? Address { get; set; }
        /// <summary>
        /// کاربر
        /// </summary>
        [Range(0, int.MaxValue)][Required] public int UserId { get; set; }
    }
    public class MerchantInfoSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringValidation] public string? Title { get; set; }

    }

}
