using System;

namespace YashilCommerce.Core.DTO
{
    public class WalletAssignmentDto
    {
        public int Id { get; set; }
        public int WalletProgramId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal ConsumedAmount { get; set; }
        public decimal ReservedAmount { get; set; }
        public decimal ExpiredAmount { get; set; }
        public decimal? AvailableAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int WalletAssignmentStatusType { get; set; }
        public int WalletAssignmentSourceType { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletAssignmentCreateDto
    {
        public int WalletProgramId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class WalletAssignmentUserDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class WalletAssignmentResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public WalletAssignmentDto Assignment { get; set; }

        /// <summary>دلیل رد ایجاد Assignment را بازمی‌گرداند.</summary>
        public static WalletAssignmentResultDto Failure(string message)
            => new WalletAssignmentResultDto { Message = message };
    }
}
