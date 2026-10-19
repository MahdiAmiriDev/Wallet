using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
	public class CardAmountHistoryRepository : GenericApplicationBasedRepository<CardAmountHistory,int>, ICardAmountHistoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public CardAmountHistoryRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }

        public IQueryable<CardAmountHistory> GetByCustomFilter(int? cardInfoId)
        {
            var query = GetAll(true);
            if (cardInfoId.HasValue)
            {
                query = query.Where(x => x.CardInfoId == cardInfoId.Value);
            }


            return query;
        }

    }
}      
