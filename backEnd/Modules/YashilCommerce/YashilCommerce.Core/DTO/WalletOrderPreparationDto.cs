namespace YashilCommerce.Core.DTO
{
    public class WalletOrderPreparationRequestDto
    {
        public int OrderId { get; set; }
        public int WalletProgramId { get; set; }
        public int PaymentMethodType { get; set; }
        public string AttemptKey { get; set; }
    }

    public class WalletOrderPreparationResultDto
    {
        public int OrderId { get; set; }
        public int WalletProgramId { get; set; }
        public int PaymentId { get; set; }
        public int PaymentMethodType { get; set; }
        public bool IsReplay { get; set; }
        public int PaymentStatusType { get; set; }
        public bool CanRedirect { get; set; }
        public string RedirectUrl { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal WalletAmount { get; set; }
        public decimal RemainingPayable { get; set; }
        public WalletReserveResultDto Reservation { get; set; }
        public WalletReserveErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => ErrorCode == WalletReserveErrorCode.None;

        /// <summary>خطای آماده‌سازی را بدون ادعای ایجاد Reservation برمی‌گرداند.</summary>
        public static WalletOrderPreparationResultDto Failure(WalletReserveErrorCode code, string message)
            => new WalletOrderPreparationResultDto { ErrorCode = code, Message = message };
    }
}
