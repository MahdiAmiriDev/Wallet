using System;

namespace YashilCommerce.Core.DTO
{
    public class WalletGrantCreateDto
    {
        public int WalletAssignmentId { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
    }

    public class WalletGrantDto
    {
        public int Id { get; set; }
        public int WalletProgramId { get; set; }
        public int WalletAssignmentId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime OccurredAt { get; set; }
    }

    public class WalletGrantResultDto
    {
        public bool IsSuccess { get; set; }
        public bool IsReplay { get; set; }
        public bool IsConflict { get; set; }
        public string Message { get; set; }
        public WalletGrantDto Grant { get; set; }

        /// <summary>خطای اعتبارسنجی یا تعارض کلید درخواست را بدون اثر مالی بازمی‌گرداند.</summary>
        public static WalletGrantResultDto Failure(string message, bool conflict = false)
            => new WalletGrantResultDto { Message = message, IsConflict = conflict };
    }
}
