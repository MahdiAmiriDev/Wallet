			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IYashilSchedulerRepository : IGenericRepository<YashilScheduler,int>
    {
		          
        IQueryable<YashilScheduler> GetByCustomFilter( int? occuranceType);
           
		
    }
}      
 