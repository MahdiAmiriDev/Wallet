namespace YashilCommerce.Core.DTO
{
    public class WalletScopeDto
    {
        public long Id { get; set; }
        public int WalletProgramId { get; set; }
        public int WalletScopeType { get; set; }
        public int? CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        public int? CityId { get; set; }
        public int? ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletScopeEditDto : WalletScopeDto { }

    public class WalletScopeTargetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class WalletScopeDeleteDto
    {
        public long Id { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class WalletScopeResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public WalletScopeDto Scope { get; set; }

        /// <summary>دلیل رد عملیات مدیریتی Scope را بازمی‌گرداند.</summary>
        public static WalletScopeResultDto Failure(string message)
            => new WalletScopeResultDto { Message = message };
    }
}
