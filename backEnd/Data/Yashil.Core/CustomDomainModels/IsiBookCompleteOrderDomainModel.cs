namespace Yashil.Core.CustomDomainModels
{
    /// <summary>
    /// سفارش های کامل ایزی بوک
    /// </summary>
    public class IsiBookCompleteOrderDomainModel
    {
        public int Id => default;
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public string AuthorizationTransactionId { get; set; }
        public string PaymentMessage { get; set; }
        public int? InstallmentCount { get; set; }
        public int? CreationDateInt { get; set; }

        public string CreationDateString => CreationDateInt.ToString()
            .Insert(4, "/").Insert(7, "/");
    }
}
