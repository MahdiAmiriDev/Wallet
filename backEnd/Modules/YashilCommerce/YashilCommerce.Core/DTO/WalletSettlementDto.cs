using System;

namespace YashilCommerce.Core.DTO
{
    public class WalletCaptureRequestDto
    {
        public int ReservationId { get; set; }
        public int PaymentId { get; set; }
        public string IdempotencyKey { get; set; }
    }

    public class WalletReleaseRequestDto
    {
        public int ReservationId { get; set; }
    }

    public enum WalletSettlementErrorCode
    {
        None,
        InvalidRequest,
        NotFound,
        InvalidState,
        Expired,
        PaymentNotPaid,
        BalanceMismatch,
        Conflict,
        ConcurrentConflict
    }

    public class WalletSettlementResultDto
    {
        public int ReservationId { get; set; }
        public int? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime? SettledAt { get; set; }
        public bool IsReplay { get; set; }
        public WalletSettlementErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => ErrorCode == WalletSettlementErrorCode.None;

        /// <summary>خطای Settlement را بدون ادعای تغییر مانده یا Ledger بازمی‌گرداند.</summary>
        public static WalletSettlementResultDto Failure(WalletSettlementErrorCode code, string message)
            => new WalletSettlementResultDto { ErrorCode = code, Message = message };
    }
}
