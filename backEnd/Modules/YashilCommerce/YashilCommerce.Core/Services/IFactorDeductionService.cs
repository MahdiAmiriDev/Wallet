
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IFactorDeductionService : IGenericService<FactorDeduction,int>
    {
                 
        IQueryable<FactorDeduction> GetByCustomFilter( int? factorId);
           
		
    }
}      
 