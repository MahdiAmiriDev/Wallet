			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ICitySubsidyFactorRepository : IGenericRepository<CitySubsidyFactor,int>
    {
		          
        IQueryable<CitySubsidyFactor> GetByCustomFilter( int? cityId);
           
		
    }
}      
 