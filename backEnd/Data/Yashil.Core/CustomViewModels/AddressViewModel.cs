using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.CustomViewModels
{

    public class AddressListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string FullAddress { get; set; }

        public int CityId { get; set; }

        [StringValidation] public string CityTitle { get; set; }

        [StringValidation] public string Email { get; set; }

        [StringValidation] public string Telephone { get; set; }

        [StringValidation] public string MobileNumber { get; set; }

        [StringValidation] public string FaxNumber { get; set; }

        [StringValidation] public string PostalCode { get; set; }

        public string AccessLevelTitle { get; set; }
        public bool IsEncrypted { get; set; }


    }


    public class AddressEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [DisplayName("آدرس")]
        [Required(ErrorMessage = @"آدرس اجباری است")]
        [StringLength(600)] [StringValidation]
        public string FullAddress { get; set; }
        [Range(0, int.MaxValue)]
        [Required] 
        public int CityId { get; set; }
        [StringLength(600)] [StringValidation] public string Email { get; set; }
        [StringLength(200)] [StringValidation] public string Telephone { get; set; }
        [StringLength(20)][StringValidation] public string MobileNumber { get; set; }
        [StringLength(20)][StringValidation] public string FaxNumber { get; set; }
       [StringLength(10)][StringValidation] public string PostalCode { get; set; }
    }





    public class AddressSimpleViewModel : IBaseViewModel
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
