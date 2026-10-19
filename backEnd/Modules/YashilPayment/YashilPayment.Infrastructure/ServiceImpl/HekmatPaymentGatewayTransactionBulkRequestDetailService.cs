using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class HekmatPaymentGatewayTransactionBulkRequestDetailService :
        GenericService<HekmatPaymentGatewayTransactionBulkRequestDetail, int>, IHekmatPaymentGatewayTransactionBulkRequestDetailService
    {
        private readonly IHekmatPaymentGatewayTransactionBulkRequestDetailRepository _hekmatPaymentGatewayTransactionBulkRequestDetailRepository;

        public HekmatPaymentGatewayTransactionBulkRequestDetailService(IUnitOfWork unitOfWork, IHekmatPaymentGatewayTransactionBulkRequestDetailRepository hekmatPaymentGatewayTransactionBulkRequestDetailRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, hekmatPaymentGatewayTransactionBulkRequestDetailRepository, userPrincipal, eventPublisher)
        {
            _hekmatPaymentGatewayTransactionBulkRequestDetailRepository = hekmatPaymentGatewayTransactionBulkRequestDetailRepository;
        }


        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetByCustomFilter(int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId)
        {
            return _hekmatPaymentGatewayTransactionBulkRequestDetailRepository.GetByCustomFilter(appDocumentId, parentId, hekmatPaymentGatewayTransactionBulkRequestId);
        }

        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetInvalidHekmatBulkRequest(
            int userId,
            int hekmatPaymentGatewayTransactionBulkRequestId,
            bool showInvalidItem)
        {
            return _hekmatPaymentGatewayTransactionBulkRequestDetailRepository
                .GetInvalidHekmatBulkRequestDetail(userId, hekmatPaymentGatewayTransactionBulkRequestId, showInvalidItem);
        }

        public Task<int> DeleteStatusNotDeterminedAsync(int userId)
        {
            return GetTransactionBulkRequestDetailStatusNotDeterminedByUserId(userId).ExecuteDeleteAsync();
        }

        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetTransactionBulkRequestDetailStatusNotDeterminedByUserId(int userId)
        {
            return FindByUserId(userId)
                .Where(x => x.CreateBy == userId && !x.StatusCode.HasValue);
        }
        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> FindByUserId(int userId)
        {
            return _hekmatPaymentGatewayTransactionBulkRequestDetailRepository.FindByUserId(userId);
        }
    }
}
