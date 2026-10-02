
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IContentCategoryService : IGenericService<ContentCategory,int>
    {
                 
        IQueryable<ContentCategory> GetByCustomFilter( int? parentId);
           
		
    }
}      
 