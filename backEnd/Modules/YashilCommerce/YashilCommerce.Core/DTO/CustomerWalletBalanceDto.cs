using System.Collections.Generic;

namespace YashilCommerce.Core.DTO
{
    public class CustomerWalletBalanceDto
    {
        public List<CustomerWalletBalanceItemDto> Items { get; set; } = new List<CustomerWalletBalanceItemDto>();
        public decimal AssignedAmount { get; set; }
        public decimal ConsumedAmount { get; set; }
        public decimal ReservedAmount { get; set; }
        public decimal ExpiredAmount { get; set; }
        public decimal AvailableAmount { get; set; }
    }

    public class CustomerWalletBalanceItemDto
    {
        public int WalletProgramId { get; set; }
        public string ProgramTitle { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal ConsumedAmount { get; set; }
        public decimal ReservedAmount { get; set; }
        public decimal ExpiredAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public bool IsUsable { get; set; }
        public bool HasBalanceMismatch { get; set; }
    }
}
