using System;
using System.Collections.Generic;

namespace YashilCommerce.Core.DTO
{
    public class WalletBenefitResultDto
    {
        public bool IsSuccess { get; set; }
        public bool IsReplay { get; set; }
        public string Message { get; set; }
        public int? EntityId { get; set; }
        public int? WalletProgramId { get; set; }
        public static WalletBenefitResultDto Failure(string message) => new WalletBenefitResultDto { Message = message };
    }

    public class WalletCardGroupCreateDto
    {
        public int WalletProgramId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class WalletCardGroupDto
    {
        public int Id { get; set; }
        public int WalletProgramId { get; set; }
        public string Title { get; set; }
        public int WalletCardGroupStatusType { get; set; }
        public int MemberCount { get; set; }
    }

    public class WalletCardMemberDto
    {
        public int Id { get; set; }
        public int WalletCardGroupId { get; set; }
        public string MaskedCard { get; set; }
        public int WalletCardMemberStatusType { get; set; }
    }

    public class WalletCardMemberCreateDto
    {
        public int WalletCardGroupId { get; set; }
        public string CardNumber { get; set; }
    }

    public class WalletCouponBatchCreateDto
    {
        public int WalletProgramId { get; set; }
        public string AttemptKey { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal AmountPerCoupon { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class WalletCouponBatchResultDto : WalletBenefitResultDto
    {
        // فقط پاسخ نخست ساخت Batch شامل کدهای خام است؛ در دیتابیس ذخیره نمی‌شوند.
        public IReadOnlyList<string> Codes { get; set; }
        public bool RequiresCancellation { get; set; }
    }

    public class WalletCouponBatchDto
    {
        public int Id { get; set; }
        public int WalletProgramId { get; set; }
        public string AttemptKey { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal AmountPerCoupon { get; set; }
        public int WalletCouponBatchStatusType { get; set; }
    }
}
