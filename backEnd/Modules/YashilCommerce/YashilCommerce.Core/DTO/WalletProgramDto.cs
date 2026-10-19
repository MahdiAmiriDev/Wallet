using System;

namespace YashilCommerce.Core.DTO
{
    public class WalletProgramDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int WalletProgramType { get; set; }
        public int WalletProgramAllocationMethodType { get; set; }
        public decimal TotalAmount { get; set; }
        public int? TargetUserCount { get; set; }
        public decimal? AmountPerUser { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal ConsumedAmount { get; set; }
        public decimal ReservedAmount { get; set; }
        public decimal ExpiredAmount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int WalletProgramStatusType { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletProgramEditDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int WalletProgramType { get; set; }
        public int WalletProgramAllocationMethodType { get; set; }
        public decimal TotalAmount { get; set; }
        public int? TargetUserCount { get; set; }
        public decimal? AmountPerUser { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletProgramStatusDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletProgramResultDto
    {
        public bool IsSuccess { get; set; }
        public int EntityId { get; set; }
        public string Message { get; set; }
        public WalletProgramDto Program { get; set; }

        /// <summary>نتیجه ناموفق عملیات مدیریتی Program را به همراه دلیل بازمی‌گرداند.</summary>
        public static WalletProgramResultDto Failure(string message)
            => new WalletProgramResultDto { Message = message };
    }
}
