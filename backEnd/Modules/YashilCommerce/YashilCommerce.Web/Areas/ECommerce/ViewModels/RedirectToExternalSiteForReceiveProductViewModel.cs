namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class RedirectToExternalSiteForReceiveProductViewModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ButtonText { get; set; }
        public string ApiKey { get; set; }
        public int OrderItemId { get; set; }

    }
}
