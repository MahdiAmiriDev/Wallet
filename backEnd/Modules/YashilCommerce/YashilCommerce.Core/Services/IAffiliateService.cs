
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IAffiliateService : IGenericService<Affiliate,int>
    {
                 
        IQueryable<Affiliate> GetByCustomFilter( int? addressId);
           
		
    }
}      
 