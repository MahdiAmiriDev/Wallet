using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    
    public class ProductGroupRepository : GenericApplicationBasedRepository<ProductGroup, int>, IProductGroupRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public ProductGroupRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        
        public IQueryable<ProductGroup> GetByCustomFilter( int? parentId, int? categoryGroupId, bool? isIncludeTax)
        {
            var query = GetAll(true);
            if(parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            
            if(categoryGroupId.HasValue)
            {
                query = query.Where(x => x.CategoryGroupId == categoryGroupId.Value);
            }

            if (isIncludeTax.HasValue && isIncludeTax.Value)
            {
                query = query.Where(x => x.IsIncludeTax);
            }

            if (isIncludeTax.HasValue && !isIncludeTax.Value)
            {
                query = query.Where(x => !x.IsIncludeTax);
            }
            
            return query;
        }
         
    }
}      
