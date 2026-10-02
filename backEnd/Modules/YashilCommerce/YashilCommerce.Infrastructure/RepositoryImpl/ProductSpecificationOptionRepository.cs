using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductSpecificationOptionRepository : GenericApplicationBasedRepository<ProductSpecificationOption, int>,
        IProductSpecificationOptionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public ProductSpecificationOptionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public override ProductSpecificationOption Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.SpecificationOption.Specification).FirstOrDefault(x => x.Id == (int) id);
        }

        public override ValueTask<ProductSpecificationOption> GetAsync(object id, bool readOnly = true)
        {
            int key = (int) id;
            var firstOrDefaultAsync = DbSet.Include(x => x.SpecificationOption.Specification)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<ProductSpecificationOption>(firstOrDefaultAsync);
        }

        public IQueryable<ProductSpecificationOption> GetByCustomFilter(int? productId, int? specificationOption)
        {
            var query = GetAll(true);
            if (productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }

            if (specificationOption.HasValue)
            {
                query = query.Where(x => x.SpecificationOptionId == specificationOption.Value);
            }


            return query;
        }
    }
}