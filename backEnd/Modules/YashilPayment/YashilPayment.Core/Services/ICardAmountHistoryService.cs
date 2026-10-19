
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilPayment.Core.Services
{
	public interface ICardAmountHistoryService : IGenericService<CardAmountHistory,int>
    {

        IQueryable<CardAmountHistory> GetByCustomFilter(int? cardInfoId);
    }
}      
 