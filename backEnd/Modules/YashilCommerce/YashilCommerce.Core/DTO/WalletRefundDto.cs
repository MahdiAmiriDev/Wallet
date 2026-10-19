using System;

namespace YashilCommerce.Core.DTO
{
    public class WalletRefundRequestDto
    {
        public int CaptureTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
    }

    public enum WalletRefundErrorCode
    {
        None,
        InvalidRequest,
        NotFound,
        InvalidCapture,
        ExceedsCapturedAmount,
        BalanceMismatch,
        Conflict,
        ConcurrentConflict
    }

    public class WalletRefundResultDto
    {
        public int RefundTransactionId { get; set; }
        public int CaptureTransactionId { get; set; }
        public int OrderId { get; set; }
        public int WalletAssignmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime OccurredAt { get; set; }
        public bool IsReplay { get; set; }
        public WalletRefundErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => ErrorCode == WalletRefundErrorCode.None;

        /// <summary>خطای Refund را بدون ادعای تغییر مانده یا ایجاد Ledger برمی‌گرداند.</summary>
        public static WalletRefundResultDto Failure(WalletRefundErrorCode code, string message)
            => new WalletRefundResultDto { ErrorCode = code, Message = message };
    }
}
