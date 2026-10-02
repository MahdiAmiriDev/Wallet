using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IIssueLogWorkService : IGenericService<IssueLogWork, int>
    {
        IQueryable<IssueLogWork> GetByCustomFilter(int? issueId, int? issueType, int? orderId, int? orderItemId,
            int? paymentId);
    }
}