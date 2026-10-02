
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface ICategorySpecificationService : IGenericService<CategorySpecification,int>
    {
                 
        IQueryable<CategorySpecification> GetByCustomFilter( int? categoryId, int? specificationId);
           
		
    }
}      
 