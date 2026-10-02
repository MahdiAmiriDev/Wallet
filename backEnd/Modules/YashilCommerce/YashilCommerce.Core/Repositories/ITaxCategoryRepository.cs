using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ITaxCategoryRepository : IGenericRepository<TaxCategory,int>
    {
        IQueryable<TaxCategory> FindById(int id);
    }
}      
 