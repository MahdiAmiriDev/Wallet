			using System.Collections.Generic;
            using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IDependentProductSaleRepository : IGenericRepository<DependentProductSale,int>
    {
		          
        IQueryable<DependentProductSale> GetByCustomFilter( int? mainProduct, int? relatedProduct);


        void UpdateMainProductDependentProducts(List<int> dependentProducts,  int mainProductId);
    }
}      
 