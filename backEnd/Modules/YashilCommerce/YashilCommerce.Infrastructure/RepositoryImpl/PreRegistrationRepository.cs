using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class PreRegistrationRepository : GenericApplicationBasedRepository<PreRegistration, int>,
        IPreRegistrationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public PreRegistrationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<PreRegistration> GetByCustomFilter(int? userId, int? productSalesPlanningId)
        {
            var query = GetAll(true);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }

            if (productSalesPlanningId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanningId == productSalesPlanningId.Value);
            }


            return query;
        }
    }
}