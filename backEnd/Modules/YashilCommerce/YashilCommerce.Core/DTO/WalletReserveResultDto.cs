using System;

namespace YashilCommerce.Core.DTO
{
    public enum WalletReserveErrorCode
    {
        None,
        InvalidRequest,
        OrderNotFound,
        UnauthorizedOrder,
        AssignmentMismatch,
        ScopeMismatch,
        InsufficientBalance,
        Conflict
    }

    public class WalletReserveResultDto
    {
        public int ReservationId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime ExpiresAt { get; set; }
        public WalletReserveErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => ErrorCode == WalletReserveErrorCode.None;

        public static WalletReserveResultDto Failure(WalletReserveErrorCode errorCode, string message)
            => new WalletReserveResultDto { ErrorCode = errorCode, Message = message };
    }
}
