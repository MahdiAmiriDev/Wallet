using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class HekmatPaymentGatewayTransactionBulkRequestCategoryService : GenericService<HekmatPaymentGatewayTransactionBulkRequestCategory, int>, IHekmatPaymentGatewayTransactionBulkRequestCategoryService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatPaymentGatewayTransactionBulkRequestCategoryRepository _hekmatPaymentGatewayTransactionBulkRequestCategoryRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public HekmatPaymentGatewayTransactionBulkRequestCategoryService (IUnitOfWork unitOfWork, IHekmatPaymentGatewayTransactionBulkRequestCategoryRepository hekmatPaymentGatewayTransactionBulkRequestCategoryRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, hekmatPaymentGatewayTransactionBulkRequestCategoryRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_hekmatPaymentGatewayTransactionBulkRequestCategoryRepository = hekmatPaymentGatewayTransactionBulkRequestCategoryRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
         
        
        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestCategory> GetByCustomFilter( int? parentId, int? yashilHekmatTerminalId)
        {
            return _hekmatPaymentGatewayTransactionBulkRequestCategoryRepository.GetByCustomFilter(parentId,yashilHekmatTerminalId);
        }
        
    }
}      
 