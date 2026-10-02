using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class IssueRepository : GenericApplicationBasedRepository<Issue, int>, IIssueRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public IssueRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<User> GetAssigneeByFiler(int? orderItemId, string recipientNationalCode = null, string mobileNumber = null)
        {

            if (orderItemId.HasValue)
            {
                return _context.Issue.Where(x => x.OrderItemId == orderItemId).Select(c => c.AssigneeNavigation).Distinct();
            }
            if (!string.IsNullOrEmpty(recipientNationalCode))
            {
                return _context.Issue.Where(x => x.OrderItem.Order.NationalCode == recipientNationalCode).Select(c => c.AssigneeNavigation).Distinct();
            }
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                return _context.Issue.Where(x => x.IssueOwnerNavigation.UserName == mobileNumber).Select(c => c.AssigneeNavigation).Distinct();
            }

            return null;
        }

        public IQueryable<Issue> GetByCustomFilter(int? orderItemId,int? orderId,int? paymentId, int? issueType, int? resolutionType, int? priorityType, int? issueStateType, int? assignee, int? issueOwner, string nonRegisteredUser = null)
        {
            var query = GetAll(true);

            if (orderItemId.HasValue)
            {
                query = query.Where(x => x.OrderItemId == orderItemId.Value);
            }
            if (orderId.HasValue)
            {
                query = query.Where(x => x.OrderId == orderId.Value);
            }
            if (paymentId.HasValue)
            {
                query = query.Where(x => x.PaymentId == paymentId.Value);
            }
            if (issueType.HasValue)
            {
                query = query.Where(x => x.IssueType == issueType.Value);
            }
            if (resolutionType.HasValue)
            {
                query = query.Where(x => x.ResolutionType == resolutionType.Value);
            }
            if (priorityType.HasValue)
            {
                query = query.Where(x => x.PriorityType == priorityType.Value);
            }
            if (issueStateType.HasValue)
            {
                query = query.Where(x => x.IssueStateType == issueStateType.Value);
            }
            if (assignee.HasValue)
            {
                query = query.Where(x => x.Assignee == assignee.Value);
            }
            if (issueOwner.HasValue)
            {
                query = query.Where(x => x.IssueOwner == issueOwner);
            }
            if (!string.IsNullOrEmpty(nonRegisteredUser))
            {
                query = query.Where(x => x.NonRegisteredUser == nonRegisteredUser);
            }
            return query;
        }



    }
}
