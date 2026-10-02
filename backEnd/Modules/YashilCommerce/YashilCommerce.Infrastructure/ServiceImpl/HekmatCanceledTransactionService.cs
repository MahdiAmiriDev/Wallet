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
	public class HekmatCanceledTransactionService : GenericService<HekmatCanceledTransaction,int>, IHekmatCanceledTransactionService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatCanceledTransactionRepository _hekmatCanceledTransactionRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public HekmatCanceledTransactionService (IUnitOfWork unitOfWork, IHekmatCanceledTransactionRepository hekmatCanceledTransactionRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
			: base(unitOfWork, hekmatCanceledTransactionRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_hekmatCanceledTransactionRepository = hekmatCanceledTransactionRepository;
			_userPrincipal = userPrincipal;
        }


        public Task<HekmatCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            return _hekmatCanceledTransactionRepository.GetByRefundDetailIdAsync(refundDetailId, readOnly);
        }
    }
}      
 