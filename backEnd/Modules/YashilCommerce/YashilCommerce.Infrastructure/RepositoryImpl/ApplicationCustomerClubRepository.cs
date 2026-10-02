			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ApplicationCustomerClubRepository : GenericApplicationBasedRepository<ApplicationCustomerClub,int>, IApplicationCustomerClubRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ApplicationCustomerClubRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<ApplicationCustomerClub> GetByCustomFilter( int? customerClubId)
        {
            var query= GetAll(true);
                         if ( customerClubId.HasValue)
                {
                    query = query.Where(x => x.CustomerClubId == customerClubId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
