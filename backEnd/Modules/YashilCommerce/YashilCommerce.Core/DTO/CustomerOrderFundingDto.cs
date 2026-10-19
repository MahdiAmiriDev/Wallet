namespace YashilCommerce.Core.DTO
{
    public class CustomerOrderFundingDto
    {
        public int OrderId { get; set; }
        public decimal RecordedPaidAmount { get; set; }
        public decimal WalletCapturedAmount { get; set; }
        public decimal WalletRefundedAmount { get; set; }
        public decimal? OpenWalletReservedAmount { get; set; }
        public decimal? OpenGatewayPaymentAmount { get; set; }
        public int? PendingVibeCapturePaymentId { get; set; }
        public int? PendingHekmatOnlineCapturePaymentId { get; set; }
        public decimal WalletNetAmount => WalletCapturedAmount - WalletRefundedAmount;
    }
}
