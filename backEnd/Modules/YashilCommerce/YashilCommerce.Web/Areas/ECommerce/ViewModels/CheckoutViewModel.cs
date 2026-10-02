using System.Collections.Generic;
using Yashil.Core.CustomViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            OrderDetailViewModels = new List<RecipientListViewModel>();
            MembershipTypeList = new List<CommonBaseDataSimpleViewModel>();
            CardTypeList = new List<CommonBaseDataListViewModel>();
            RecipientViewList = new List<RecipientListViewModel>();
        }
        public List<RecipientListViewModel> OrderDetailViewModels { get; set; }
        public List<CommonBaseDataSimpleViewModel> MembershipTypeList { get; set; }
        public List<CommonBaseDataListViewModel> CardTypeList { get; set; }
        public List<RecipientListViewModel> RecipientViewList { get; set; }
        public RecipientListViewModel SelectedRecipient { get; set; }
        /// <summary>
        /// is application address based or not
        /// </summary>
        public bool AddressBased { get; set; }
        /// <summary>
        /// is application subsidy based or not
        /// </summary>
        public bool SubsidyBased { get; set; }
    }
    public class CommonBaseDataListViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }
        public string ExtendedProps { get; set; }
        public int CommonBaseTypeId { get; set; }
    }

}
