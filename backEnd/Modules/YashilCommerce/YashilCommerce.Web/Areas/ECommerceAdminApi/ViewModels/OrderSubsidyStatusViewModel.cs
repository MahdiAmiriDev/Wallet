namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class OrderSubsidyStatusViewModel
    {
        /// <summary>
        /// تعداد سفارشات 
        /// </summary>
        public int OrdersCount { get; set; }

        /// <summary>
        /// مجموع یارانه
        /// </summary>
        public decimal? SubsidySum { get; set; }

        public bool CheckOnlyCompletedOrders { get; set; }
        public bool CheckForCurrentUser { get; set; }
    }
}
