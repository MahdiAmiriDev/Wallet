using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class IssueLogWorkRepository : GenericApplicationBasedRepository<IssueLogWork, int>, IIssueLogWorkRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public IssueLogWorkRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<IssueLogWork> GetByCustomFilter(int? issueId, int? issueType, int? orderId, int? orderItemId,
            int? paymentId)
        {
            var query = GetAll(true);
            if (issueId.HasValue)
            {
                query = query.Where(x => x.IssueId == issueId.Value);
            }
            if (issueType.HasValue)
            {
                query = query.Where(x => x.Issue.IssueType == issueType.Value);
            }
            if (orderId.HasValue)
            {
                query = query.Where(x => x.Issue.OrderId == orderId.Value);
            }
            if (orderItemId.HasValue)
            {
                query = query.Where(x => x.Issue.OrderItemId == orderItemId.Value);
            }

            if (paymentId.HasValue)
            {
                query = query.Where(x => x.Issue.PaymentId == paymentId.Value);
            }


            return query;
        }
        public override IssueLogWork Get(object id, bool readOnly = false)
        {
            var query = _context.IssueLogWork.Include(x => x.Issue);
            if (readOnly)
            {
                return query.AsNoTracking().FirstOrDefault(x => x.Id == (int)id);
            }
            else return query.FirstOrDefault(x => x.Id == (int)id);
        }
        public override ValueTask<IssueLogWork> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var query = DbSet.Include(x => x.Issue)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<IssueLogWork>(query);
        }
    }
}