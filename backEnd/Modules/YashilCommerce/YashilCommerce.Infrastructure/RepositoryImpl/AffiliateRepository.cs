			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class AffiliateRepository : GenericApplicationBasedRepository<Affiliate,int>, IAffiliateRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public AffiliateRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<Affiliate> GetByCustomFilter( int? addressId)
        {
            var query= GetAll(true);
                         if ( addressId.HasValue)
                {
                    query = query.Where(x => x.AddressId == addressId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
