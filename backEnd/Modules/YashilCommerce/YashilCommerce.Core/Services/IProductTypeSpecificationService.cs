
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IProductTypeSpecificationService : IGenericService<ProductTypeSpecification,int>
    {
                 
        IQueryable<ProductTypeSpecification> GetByCustomFilter( int? productTypeId, int? specificationId);
           
		
    }
}      
 