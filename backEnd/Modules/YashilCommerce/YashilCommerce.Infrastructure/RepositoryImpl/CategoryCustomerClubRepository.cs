using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CategoryCustomerClubRepository : GenericApplicationBasedRepository<CategoryCustomerClub, int>, ICategoryCustomerClubRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CategoryCustomerClubRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<CategoryCustomerClub> GetByCustomFilter(int? customerClubId, int? categoryId, int? applicationId)
        {
            var query = GetAll(true, applicationId);
            if (customerClubId.HasValue)
            {
                query = query.Where(x => x.CustomerClubId == customerClubId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            return query;
        }



    }
}
