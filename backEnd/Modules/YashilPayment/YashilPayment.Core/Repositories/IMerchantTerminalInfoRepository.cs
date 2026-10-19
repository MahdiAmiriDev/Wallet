			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IMerchantTerminalInfoRepository : IGenericRepository<MerchantTerminalInfo,int>
    {
		          
        IQueryable<MerchantTerminalInfo> GetByCustomFilter( int? merchantId);
           
		
    }
}      
 