using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IAppEntityCustomerClubRepository : IGenericRepository<AppEntityCustomerClub, int>
    {
        IQueryable<AppEntityCustomerClub> GetByCustomFilter(int? appEntityId, int? customerclubId);
    }
}