			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ProductTypeRepository : GenericApplicationBasedRepository<ProductType,int>, IProductTypeRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ProductTypeRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<ProductType> GetByCustomFilter( int? parentId)
        {
            var query= GetAll(true);
                         if ( parentId.HasValue)
                {
                    query = query.Where(x => x.ParentId == parentId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
