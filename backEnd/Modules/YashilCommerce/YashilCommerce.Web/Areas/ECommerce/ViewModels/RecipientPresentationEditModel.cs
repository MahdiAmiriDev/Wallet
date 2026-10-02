using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Yashil.Core.CustomViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class RecipientPresentationEditModel : RecipientEditModel
    {
        public RecipientPresentationEditModel()
        {
            MembershipTypes = new List<CommonBaseDataSimpleViewModel>();
            GenderTypes = new List<CommonBaseDataSimpleViewModel>();
        }


        public List<CommonBaseDataSimpleViewModel> MembershipTypes { get; set; }
        /// <summary>
        /// جنسیت
        /// </summary>
        public List<CommonBaseDataSimpleViewModel> GenderTypes { get; set; }
        /// <summary>
        /// نسبت خریدار با دارنده حکمت کارت
        /// </summary>
        public List<CommonBaseDataSimpleViewModel> RelationshipWithBuyerList { get; set; }

        [DisplayName("آدرس")]
        //[Required(ErrorMessage = @"لطفا آدرس را وارد نمایید")]
        [StringLength(600)]
        public string FullAddress { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = @"لطفا شهر را انتخاب کنید")]
        [DisplayName("شهر")]
        //[Required(ErrorMessage = @"لطفا شهر را انتخاب کنید")]
        public int? CityId { get; set; }
        public List<CityViewModel> CityListModelViewModel { get; set; }
        public string CityTitle { get; set; }
        public bool HasCardNumber { get; set; }
        public bool? IsEncrypted { get; set; }
        /// <summary>
        /// نسب تحویل گیرنده با صاحب کارت
        /// </summary>
        public int? RelationshipWithBuyerType { get; set; }
    }
}
