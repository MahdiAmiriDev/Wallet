
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IProductSpecificationOptionService : IGenericService<ProductSpecificationOption,int>
    {
                 
        IQueryable<ProductSpecificationOption> GetByCustomFilter( int? productId, int? specificationOption);
           
		
    }
}      
 