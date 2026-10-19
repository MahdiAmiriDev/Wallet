using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using System;
using Yashil.Core.Enums;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class PaymentTransactionItemRefundService : GenericService<PaymentTransactionItemRefund, int>, IPaymentTransactionItemRefundService
    {
        private readonly IPaymentTransactionItemRefundRepository _paymentTransactionItemRefundRepository;
		private readonly IUserPrincipal _userPrincipal;

        public PaymentTransactionItemRefundService (IUnitOfWork unitOfWork, IPaymentTransactionItemRefundRepository paymentTransactionItemRefundRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, paymentTransactionItemRefundRepository,userPrincipal,eventPublisher)
        {
            _paymentTransactionItemRefundRepository = paymentTransactionItemRefundRepository;
			_userPrincipal = userPrincipal;
        }
        
        public IQueryable<PaymentTransactionItemRefund> GetByCustomFilter(int? refundPaymentTransactionItemRequestId)
        {
            return _paymentTransactionItemRefundRepository.GetByCustomFilter(refundPaymentTransactionItemRequestId);
        }

        public PaymentTransactionItemRefund CreateNewPaymentTransactionItemRefundEntity(
            int paymentTransactionItemRefundRequestId,
            int yashilHekmatTerminalId,
            long oldPaymentAmount,
            RefundTypeEnum refundType
            )
        {
            return new PaymentTransactionItemRefund
            {
                RefundPaymentTransactionItemRequestId = paymentTransactionItemRefundRequestId,
                YashilHekmatTerminalId = yashilHekmatTerminalId,
                OldPaymentAmount = oldPaymentAmount,
                NewPaymentAmount = 0,
                RefundType = (int)refundType,
                RefundStatusType = (int)RefundStatusTypeEnum.Register,
                CreateBy = _userPrincipal.Id,
                ApplicationId = _userPrincipal.ApplicationId,
                CreatorOrganizationId = _userPrincipal.OrganizationId,
                AccessLevelId = 0,
                CreationDate = DateTime.Now,
                
            };
        }
    }
}      
 