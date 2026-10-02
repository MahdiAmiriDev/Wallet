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
    public class HekmatEffectlessCanceledTransactionService : GenericService<HekmatEffectlessCanceledTransaction, int>,
        IHekmatEffectlessCanceledTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatEffectlessCanceledTransactionRepository _hekmatEffectlessCanceledTransactionRepository;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatEffectlessCanceledTransactionService(IUnitOfWork unitOfWork,
            IHekmatEffectlessCanceledTransactionRepository hekmatEffectlessCanceledTransactionRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, hekmatEffectlessCanceledTransactionRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _hekmatEffectlessCanceledTransactionRepository = hekmatEffectlessCanceledTransactionRepository;
            _userPrincipal = userPrincipal;
        }


        public Task<HekmatEffectlessCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            return _hekmatEffectlessCanceledTransactionRepository.GetByRefundDetailIdAsync(refundDetailId, readOnly);
        }
    }
}