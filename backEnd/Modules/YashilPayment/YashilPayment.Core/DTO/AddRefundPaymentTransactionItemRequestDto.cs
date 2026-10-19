namespace YashilPayment.Core.DTO
{
    /// <summary>
    /// dto
    /// مربوط به اضافه کردن یک رکورد جدید به جدول
    /// RefundPaymentTransactionItemRequest
    /// </summary>
    public class AddRefundPaymentTransactionItemRequestDto
    {
        /// <summary>
        /// شناسه رزرو سفارش
        /// </summary>
        public string ReservationNumber { get; set; }
        /// <summary>
        /// شناسه ترمینال
        /// </summary>
        public int TerminalId { get; set; }
        /// <summary>
        /// دلیل رد
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// شناسه تامین کننده
        /// </summary>
        public int SupplierId { get; set; }
    }
}
