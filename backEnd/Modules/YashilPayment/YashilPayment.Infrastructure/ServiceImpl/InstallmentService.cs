	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using System.Collections.Generic;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class InstallmentService : GenericService<Installment,int>, IInstallmentService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IInstallmentRepository _installmentRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public InstallmentService (IUnitOfWork unitOfWork, IInstallmentRepository installmentRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, installmentRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_installmentRepository = installmentRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<Installment> GetByCustomFilter( int? paymentId, int? paymentStatus)
        {
            return _installmentRepository.GetByCustomFilter(paymentId,paymentStatus);
        }

        public List<Installment> GetInstallmentByPaymentId(int paymentId)
        {
            return _installmentRepository.GetInstallmentByPaymentId(paymentId).ToList();
        }

    }
}      
 