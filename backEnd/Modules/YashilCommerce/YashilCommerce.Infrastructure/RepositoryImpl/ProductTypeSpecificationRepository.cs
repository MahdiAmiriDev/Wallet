			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ProductTypeSpecificationRepository : GenericApplicationBasedRepository<ProductTypeSpecification,int>, IProductTypeSpecificationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ProductTypeSpecificationRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<ProductTypeSpecification> GetByCustomFilter( int? productTypeId, int? specificationId)
        {
            var query= GetAll(true);
                         if ( productTypeId.HasValue)
                {
                    query = query.Where(x => x.ProductTypeId == productTypeId.Value);
                }
                          if ( specificationId.HasValue)
                {
                    query = query.Where(x => x.SpecificationId == specificationId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
