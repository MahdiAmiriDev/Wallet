using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CustomerWalletBalanceService : ICustomerWalletBalanceService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        public CustomerWalletBalanceService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>مانده Assignmentهای کاربر جاری را بدون اثر مالی می‌خواند.</summary>
        public async Task<CustomerWalletBalanceDto> GetCurrentUserBalanceAsync()
        {
            var result = new CustomerWalletBalanceDto();
            var userId = _principal?.Id ?? 0;
            var applicationId = _application.ApplicationId;
            if (userId <= 0 || applicationId <= 0)
                return result;

            var now = DateTime.UtcNow;
            var assignments = await _context.WalletAssignment.AsNoTracking()
                .Where(x => x.UserId == userId && x.ApplicationId == applicationId && !x.Deleted
                    && x.WalletProgram.ApplicationId == applicationId && !x.WalletProgram.Deleted)
                .Select(x => new
                {
                    x.AssignedAmount, x.ConsumedAmount, x.ReservedAmount, x.ExpiredAmount,
                    x.StartDate, x.EndDate, x.WalletAssignmentStatusType,
                    x.WalletProgramId,
                    ProgramTitle = x.WalletProgram.Title,
                    ProgramStatus = x.WalletProgram.WalletProgramStatusType,
                    ProgramStart = x.WalletProgram.StartDate,
                    ProgramEnd = x.WalletProgram.EndDate
                }).ToListAsync();

            foreach (var assignment in assignments)
            {
                var free = assignment.AssignedAmount - assignment.ConsumedAmount
                    - assignment.ReservedAmount - assignment.ExpiredAmount;
                var usable = assignment.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active
                    && assignment.ProgramStatus == (int)WalletProgramStatusEnum.Active
                    && assignment.StartDate <= now && (!assignment.EndDate.HasValue || assignment.EndDate.Value > now)
                    && assignment.ProgramStart <= now && (!assignment.ProgramEnd.HasValue || assignment.ProgramEnd.Value > now);
                var item = new CustomerWalletBalanceItemDto
                {
                    WalletProgramId = assignment.WalletProgramId,
                    ProgramTitle = assignment.ProgramTitle,
                    AssignedAmount = assignment.AssignedAmount,
                    ConsumedAmount = assignment.ConsumedAmount,
                    ReservedAmount = assignment.ReservedAmount,
                    ExpiredAmount = assignment.ExpiredAmount,
                    AvailableAmount = usable ? Math.Max(0m, free) : 0m,
                    IsUsable = usable,
                    HasBalanceMismatch = free < 0m
                };
                result.Items.Add(item);
                result.AssignedAmount += item.AssignedAmount;
                result.ConsumedAmount += item.ConsumedAmount;
                result.ReservedAmount += item.ReservedAmount;
                result.ExpiredAmount += item.ExpiredAmount;
                result.AvailableAmount += item.AvailableAmount;
            }

            return result;
        }
    }
}
