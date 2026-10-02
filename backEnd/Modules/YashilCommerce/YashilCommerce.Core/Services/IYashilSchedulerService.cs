
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IYashilSchedulerService : IGenericService<YashilScheduler,int>
    {
                 
        IQueryable<YashilScheduler> GetByCustomFilter( int? occuranceType);
           
		
    }
}      
 