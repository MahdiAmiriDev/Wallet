using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class RepresentationListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringValidation] public string EconomicCode { get; set; }
        [StringValidation] public string Code { get; set; }
        [StringValidation] public string FounderFullName { get; set; }
        [StringValidation] public string Title { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }
        public int CompanyId { get; set; }

        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string StateTitle { get; set; }

        [StringValidation] public string Email { get; set; }

        [StringValidation] public string Telephone { get; set; }

        [StringValidation] public string FaxNumber { get; set; }

        [StringValidation] public string LicenseNumber { get; set; }

        [StringValidation] public string LicenseTypeTitle { get; set; }

        [StringValidation] public string OwnershipTypeTitle { get; set; }

        public int? Area { get; set; }

        [StringValidation] public string PostalCode { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        public bool IsCompanyMainRepresentation { get; set; }
        public bool Confirmed { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class RepresentationSearchViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string Telephone { get; set; }
        [StringValidation] public string FullAddress { get; set; }
        /// <summary>
        ///  ضریب سوبسید وزارت
        /// </summary>
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        ///   ضریب سوبسید ساتا
        ///</summary>
        public decimal SataSubsidyFactor { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }
    }

    public class RepresentationEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        public int LicenseExpireTime { get; set; }
        public bool Confirmed { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int FounderId { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(20)][StringValidation] public string EconomicCode { get; set; }
        [StringLength(600)]  [Required] [StringValidation] public string Title { get; set; }
        [StringLength(100)]  [Required] [StringValidation] public string ShortName { get; set; }
        [Range(0, int.MaxValue)] [Required] public int CompanyMainCategory { get; set; }
        [StringLength(400)][StringValidation] public string LicenseNumber { get; set; }
        public int? LicenseType { get; set; }
        public int? OwnershipType { get; set; }
        public int? Area { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
        public AddressEditModel Address { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CompanyId { get; set; }
        public bool IsCompanyMainRepresentation { get; set; }
        /// <summary>
        ///  ضریب سوبسید وزارت
        /// </summary>
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        ///   ضریب سوبسید ساتا
        ///</summary>
        public decimal SataSubsidyFactor { get; set; }
        /// <summary>
        ///         /// فعال
        ///</summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// آیا نمایندگی مرکزی استان است؟
        /// </summary>
        public bool? IsProvinceHub { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }
    }

    public class RepresentationSimpleViewModel : IBaseViewModel
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