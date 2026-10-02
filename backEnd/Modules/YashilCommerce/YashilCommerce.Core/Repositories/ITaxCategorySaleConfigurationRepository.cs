using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ITaxCategorySaleConfigurationRepository : IGenericRepository<TaxCategorySaleConfiguration,int>
    {
        IQueryable<TaxCategorySaleConfiguration> GetByCustomFilter( int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType);
    }
}      
 