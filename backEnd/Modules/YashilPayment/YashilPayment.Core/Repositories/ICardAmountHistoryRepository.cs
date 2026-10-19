			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface ICardAmountHistoryRepository : IGenericRepository<CardAmountHistory,int>
    {
		IQueryable<CardAmountHistory> GetByCustomFilter(int? cardInfoId);

	}
}      
 