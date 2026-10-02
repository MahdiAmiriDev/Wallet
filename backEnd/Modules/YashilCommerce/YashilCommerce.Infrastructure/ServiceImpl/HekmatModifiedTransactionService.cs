using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
    using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class HekmatModifiedTransactionService : GenericService<HekmatModifiedTransaction,int>, IHekmatModifiedTransactionService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatModifiedTransactionRepository _hekmatModifiedTransactionRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public HekmatModifiedTransactionService (IUnitOfWork unitOfWork, IHekmatModifiedTransactionRepository hekmatModifiedTransactionRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
			: base(unitOfWork, hekmatModifiedTransactionRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_hekmatModifiedTransactionRepository = hekmatModifiedTransactionRepository;
			_userPrincipal = userPrincipal;
        }
        /// <summary>
        /// دریافت تراکنش عودت بر اساس refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public Task<HekmatModifiedTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            return _hekmatModifiedTransactionRepository.GetByRefundDetailIdAsync(refundDetailId, readOnly);
        }
    }
}      
 