			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IFactorDeductionRepository : IGenericRepository<FactorDeduction,int>
    {
		          
        IQueryable<FactorDeduction> GetByCustomFilter( int? factorId);
           
		
    }
}      
 