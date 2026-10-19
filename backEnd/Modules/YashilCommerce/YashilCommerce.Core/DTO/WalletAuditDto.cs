using System;
using System.Collections.Generic;

namespace YashilCommerce.Core.DTO
{
    public class WalletAuditIssueDto
    {
        public string Code { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string Message { get; set; }
    }

    public class WalletAuditResultDto
    {
        public int WalletProgramId { get; set; }
        public DateTime CheckedAtUtc { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<WalletAuditIssueDto> Issues { get; set; } = new List<WalletAuditIssueDto>();
        public bool HasDiscrepancies => Issues.Count > 0;

        /// <summary>خطای دسترسی یا نبود Program را بدون تغییر داده مالی برمی‌گرداند.</summary>
        public static WalletAuditResultDto Failure(string message)
            => new WalletAuditResultDto { Message = message };
    }
}
