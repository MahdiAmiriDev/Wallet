
			using System.Collections.Generic;
            using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IDependentProductSaleService : IGenericService<DependentProductSale,int>
    {
                 
        IQueryable<DependentProductSale> GetByCustomFilter( int? mainProduct, int? relatedProduct);


        void UpdateMainProductDependentProducts(List<int> editModelDependentProducts, in int editModelId);
    }
}      
 