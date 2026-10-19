using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletBenefitsService
    {
        IQueryable<WalletCardGroupDto> GetCardGroups(int programId);
        IQueryable<WalletCardMemberDto> GetCardMembers(int groupId);
        IQueryable<WalletCouponBatchDto> GetCouponBatches(int programId);
        Task<WalletBenefitResultDto> CreateCardGroupAsync(WalletCardGroupCreateDto request);
        Task<WalletBenefitResultDto> AddCardAsync(WalletCardMemberCreateDto request);
        Task<WalletBenefitResultDto> ActivateCardGroupAsync(int groupId);
        Task<WalletBenefitResultDto> MatchRecipientCardAsync(int recipientId);
        Task<WalletCouponBatchResultDto> CreateCouponBatchAsync(WalletCouponBatchCreateDto request);
        Task<WalletBenefitResultDto> ActivateCouponBatchAsync(int batchId);
        Task<WalletBenefitResultDto> CancelCouponBatchAsync(string attemptKey);
        Task<WalletBenefitResultDto> ClaimCouponAsync(string code);
    }
}
