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
	public class HekmatEffectlessModifiedTransactionService : GenericService<HekmatEffectlessModifiedTransaction,int>, IHekmatEffectlessModifiedTransactionService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatEffectlessModifiedTransactionRepository _hekmatEffectlessModifiedTransactionRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public HekmatEffectlessModifiedTransactionService (IUnitOfWork unitOfWork, IHekmatEffectlessModifiedTransactionRepository hekmatEffectlessModifiedTransactionRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
			: base(unitOfWork, hekmatEffectlessModifiedTransactionRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_hekmatEffectlessModifiedTransactionRepository = hekmatEffectlessModifiedTransactionRepository;
			_userPrincipal = userPrincipal;
        }


        public Task<HekmatEffectlessModifiedTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            return _hekmatEffectlessModifiedTransactionRepository.GetByRefundDetailIdAsync(refundDetailId, readOnly);
        }
    }
}      
 