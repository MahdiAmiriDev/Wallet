using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class AppEntityCustomerClubRepository : GenericApplicationBasedRepository<AppEntityCustomerClub, int>,
        IAppEntityCustomerClubRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public AppEntityCustomerClubRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<AppEntityCustomerClub> GetByCustomFilter(int? appEntityId, int? customerclubId)
        {
            var query = GetAll(true);
            if (appEntityId.HasValue)
            {
                query = query.Where(x => x.AppEntityId == appEntityId.Value);
            }

            if (customerclubId.HasValue)
            {
                query = query.Where(x => x.CustomerclubId == customerclubId.Value);
            }


            return query;
        }
    }
}