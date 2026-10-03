using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class WalletReservationRepository : IWalletReservationRepository
    {
        private readonly YashilAppDbContext _context;

        public WalletReservationRepository(YashilAppDbContext context)
        {
            _context = context;
        }

        public Task<Order> FindOrderAsync(int applicationId, int orderId)
            => _context.Order.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == orderId && x.ApplicationId == applicationId && !x.Deleted);

        public Task<WalletReservation> FindByIdempotencyKeyAsync(int applicationId, string idempotencyKey)
            => _context.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.IdempotencyKey == idempotencyKey);

        public Task<WalletReservation> FindActiveByOrderAsync(int applicationId, int orderId, int reservedStatus)
            => _context.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.OrderId == orderId
                    && x.WalletReservationStatusType == reservedStatus);

        public Task<WalletAssignment> FindAssignmentWithProgramAsync(int assignmentId)
            => _context.WalletAssignment.AsNoTracking()
                .Include(x => x.WalletProgram)
                .FirstOrDefaultAsync(x => x.Id == assignmentId && !x.Deleted);

        public Task<int> TryIncreaseAssignmentReservedAsync(int assignmentId, int applicationId, int userId, decimal amount)
            => _context.WalletAssignment
                .Where(x => x.Id == assignmentId && x.ApplicationId == applicationId && x.UserId == userId && !x.Deleted
                    && x.AssignedAmount - x.ConsumedAmount - x.ReservedAmount - x.ExpiredAmount >= amount)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount + amount));

        public Task<int> IncreaseProgramReservedAsync(int programId, int applicationId, decimal amount)
            => _context.WalletProgram
                .Where(x => x.Id == programId && x.ApplicationId == applicationId && !x.Deleted)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount + amount));

        public async Task InsertAsync(WalletReservation reservation)
        {
            _context.WalletReservation.Add(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
