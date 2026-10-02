namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class UpdateOrderNationalCodeParam
    {
        /// <summary>
        /// orderId 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// کدملی جدید
        /// </summary>
        public string NationalCode { get; set; }
    }

    public class UpdateOrderRecipientMobileNumberParam
    {
        /// <summary>
        /// orderId 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شماره موبایل جدید
        /// </summary>
        public string RecipientMobileNumber { get; set; }
        /// <summary>
        /// شماره موبایل قبلی
        /// </summary>
        public string OldMobileNumber { get; set; }
    }
}
