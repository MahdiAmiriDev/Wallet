using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CategorySpecificationRepository : GenericApplicationBasedRepository<CategorySpecification, int>, ICategorySpecificationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CategorySpecificationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<CategorySpecification> GetByCustomFilter(int? categoryId, int? specificationId)
        {
            var query = GetAll(true);
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            if (specificationId.HasValue)
            {
                query = query.Where(x => x.SpecificationId == specificationId.Value);
            }


            return query;
        }



    }
}
