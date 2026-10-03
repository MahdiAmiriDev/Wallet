using System.Threading.Tasks;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IWalletReservationRepository
    {
        Task<Order> FindOrderAsync(int applicationId, int orderId);
        Task<WalletReservation> FindByIdempotencyKeyAsync(int applicationId, string idempotencyKey);
        Task<WalletReservation> FindActiveByOrderAsync(int applicationId, int orderId, int reservedStatus);
        Task<WalletAssignment> FindAssignmentWithProgramAsync(int assignmentId);
        Task<int> TryIncreaseAssignmentReservedAsync(int assignmentId, int applicationId, int userId, decimal amount);
        Task<int> IncreaseProgramReservedAsync(int programId, int applicationId, decimal amount);
        Task InsertAsync(WalletReservation reservation);
    }
}
