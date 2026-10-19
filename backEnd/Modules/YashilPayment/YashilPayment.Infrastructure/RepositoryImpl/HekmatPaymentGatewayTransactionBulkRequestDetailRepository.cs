using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{

    public class HekmatPaymentGatewayTransactionBulkRequestDetailRepository : GenericRepository<HekmatPaymentGatewayTransactionBulkRequestDetail, int>, IHekmatPaymentGatewayTransactionBulkRequestDetailRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatPaymentGatewayTransactionBulkRequestDetailRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetByCustomFilter(int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId)
        {
            var query = GetAll(true);
            if (appDocumentId.HasValue)
            {
                query = query.Where(x => x.AppDocumentId == appDocumentId.Value);
            }

            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            if (hekmatPaymentGatewayTransactionBulkRequestId.HasValue)
            {
                query = query.Where(x => x.HekmatPaymentGatewayTransactionBulkRequestId == hekmatPaymentGatewayTransactionBulkRequestId.Value);
            }

            return query;
        }

        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetInvalidHekmatBulkRequestDetail(
            int userId,
            int hekmatPaymentGatewayTransactionBulkRequestId,
            bool showInvalidItem)
        {
            var query = GetAll(true)
                .Where(x => x.CreateBy == userId && !x.StatusCode.HasValue);
            if (showInvalidItem)
            {
                query = query.Where(x =>
                    (x.CardNumberIsInValid.HasValue && x.CardNumberIsInValid.Value) ||
                    (x.NationalCodeIsInValid.HasValue && x.NationalCodeIsInValid.Value) ||
                    (x.InstallmentCountIsInValid.HasValue && x.NationalCodeIsInValid.Value) ||
                    (x.PriceIsInValid.HasValue && x.PriceIsInValid.Value)
                );
            }
            return query;
        }

        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> FindByUserId(int userId)
        {
            return GetAll(true).Where(x => x.CreateBy == userId);
        }
    }
}
