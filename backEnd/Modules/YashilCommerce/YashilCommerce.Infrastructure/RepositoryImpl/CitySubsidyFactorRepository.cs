			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class CitySubsidyFactorRepository : GenericApplicationBasedRepository<CitySubsidyFactor,int>, ICitySubsidyFactorRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public CitySubsidyFactorRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<CitySubsidyFactor> GetByCustomFilter( int? cityId)
        {
            var query= GetAll(true);
                         if ( cityId.HasValue)
                {
                    query = query.Where(x => x.CityId == cityId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
