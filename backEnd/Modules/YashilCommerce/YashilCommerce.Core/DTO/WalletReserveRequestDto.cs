namespace YashilCommerce.Core.DTO
{
    public class WalletReserveRequestDto
    {
        public int OrderId { get; set; }
        public int WalletAssignmentId { get; set; }
        public decimal Amount { get; set; }
        public string IdempotencyKey { get; set; }
    }
}
