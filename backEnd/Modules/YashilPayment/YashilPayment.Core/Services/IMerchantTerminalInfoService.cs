
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
	public interface IMerchantTerminalInfoService : IGenericService<MerchantTerminalInfo,int>
    {
                 
        IQueryable<MerchantTerminalInfo> GetByCustomFilter( int? merchantId);
           
		
    }
}      
 