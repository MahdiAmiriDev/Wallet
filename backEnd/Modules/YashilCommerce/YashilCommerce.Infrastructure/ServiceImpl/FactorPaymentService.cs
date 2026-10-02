using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorPaymentService : GenericService<FactorPayment, int>, IFactorPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IFactorPaymentRepository _factorPaymentRepository;

        public FactorPaymentService(IUnitOfWork unitOfWork, IFactorPaymentRepository factorPaymentRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, factorPaymentRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _factorPaymentRepository = factorPaymentRepository;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<FactorPayment> GetByCustomFilter(int factorId)
        {
            return _factorPaymentRepository.GetByCustomFilter(factorId);
        }

        public IQueryable<FactorPayment> GetFactorPayments(int factorId)
        {
            return _factorPaymentRepository.GetFactorPayments(factorId);
        }

    
       
    }
}