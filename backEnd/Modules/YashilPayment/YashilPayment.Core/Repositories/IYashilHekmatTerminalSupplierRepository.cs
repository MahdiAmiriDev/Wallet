using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IYashilHekmatTerminalSupplierRepository : IGenericRepository<YashilHekmatTerminalSupplier,int>
    {
        IQueryable<YashilHekmatTerminalSupplier> GetByCustomFilter( int? parentId, int? userId);
    }
}      
 