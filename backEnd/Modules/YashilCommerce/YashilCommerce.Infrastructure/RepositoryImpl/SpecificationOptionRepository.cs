using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class SpecificationOptionRepository : GenericApplicationBasedRepository<SpecificationOption, int>,
        ISpecificationOptionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public SpecificationOptionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<SpecificationOption> GetByCustomFilter(int? specificationId)
        {
            var query = GetAll(true);
            if (specificationId.HasValue)
            {
                query = query.Where(x => x.SpecificationId == specificationId.Value);
            }

            return query;
        }

        public IQueryable<SpecificationOption> GetByCategoryIds(List<int> categories)
        {
            var categorySpecificationOptions = from cs in _context.CategorySpecification.Where(x => categories.Contains(x.CategoryId))
                join so in _context.SpecificationOption
                    on cs.SpecificationId equals so.SpecificationId
                select so;
            var productTypeSpecificationOptions=
                from c in _context.Category.Where(x => categories.Contains(x.Id) && x.ProductTypeId.HasValue)
                join pts in _context.ProductTypeSpecification on c.ProductTypeId equals pts.ProductTypeId
                join so in _context.SpecificationOption on pts.SpecificationId equals so.SpecificationId
                select so;
            return categorySpecificationOptions.Union(productTypeSpecificationOptions).Distinct();
        }
    }
}