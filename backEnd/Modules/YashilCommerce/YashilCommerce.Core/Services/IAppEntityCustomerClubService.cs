using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IAppEntityCustomerClubService : IGenericService<AppEntityCustomerClub, int>
    {
        IQueryable<AppEntityCustomerClub> GetByCustomFilter(int? appEntityId, int? customerclubId);
    }
}