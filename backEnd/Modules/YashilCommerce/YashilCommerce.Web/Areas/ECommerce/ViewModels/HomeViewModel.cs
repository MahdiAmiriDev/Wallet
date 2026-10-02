namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class HomeViewModel
    {
        public int EmploymentCategoryGroupId { get; set; }
        public bool IsHekmatCurrentUserCustomerClub { get; set; }
        /// <summary>
        /// نمایش دکمه ی لیست مشاغل در بالای صفحه ی هوم
        /// </summary>
        public bool ShowJobOfferListButton { get; set; }
    }
}
