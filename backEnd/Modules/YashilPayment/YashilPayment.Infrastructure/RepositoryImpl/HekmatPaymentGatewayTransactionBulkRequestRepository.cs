using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;
using Yashil.Common.SharedKernel.Logging;

namespace YashilPayment.Infrastructure.RepositoryImpl
{

    public class HekmatPaymentGatewayTransactionBulkRequestRepository : GenericApplicationBasedRepository<HekmatPaymentGatewayTransactionBulkRequest, int>, IHekmatPaymentGatewayTransactionBulkRequestRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<HekmatPaymentGatewayTransactionBulkRequest> _logger;
        public HekmatPaymentGatewayTransactionBulkRequestRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, IYashilLogger<HekmatPaymentGatewayTransactionBulkRequest> logger) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _logger = logger;
        }

        public IQueryable<HekmatPaymentGatewayTransactionBulkRequest> GetByCustomFilter(int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId)
        {
            var query = GetAll(true);
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            if (hekmatPaymentGatewayTransactionBulkRequestCategoryId.HasValue)
            {
                query = query.Where(x => x.HekmatPaymentGatewayTransactionBulkRequestCategoryId == hekmatPaymentGatewayTransactionBulkRequestCategoryId.Value);
            }

            return query;
        }

        public Task<int> GetInvalidUploadedHekmatTransactionBulkCountAsync(int userId)
        {
            return _context.Database.ExecuteSqlRawAsync($"[pay].[ValidateHekmatTransactionBulkRequestDetail] {userId}");
        }
        public  Task DeleteByHekmatPaymentGatewayTransactionBulkRequestAsync(
            int hekmatPaymentGatewayTransactionBulkRequestId)
        {
           return DbSet.Where(t => t.Id == hekmatPaymentGatewayTransactionBulkRequestId)
               .ExecuteDeleteAsync();
        }

        public Task<int> AddHekmatPaymentGatewayTransactionFromBulkRequestAsync(
            int hekmatPaymentGatewayTransactionBulkRequestId,
            int userId)
        {
            object[] param = {
                new SqlParameter
                {
                    ParameterName = "@HekmatPaymentGatewayTransactionBulkRequestId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = hekmatPaymentGatewayTransactionBulkRequestId
                },
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = userId
                },
            };
            try
            {
                return _context.Database.ExecuteSqlRawAsync($"[shp].[AddHekmatPaymentGatewayTransactionFromBulkRequest] @HekmatPaymentGatewayTransactionBulkRequestId,@UserId", param);

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                    nameof(HekmatPaymentGatewayTransactionBulkRequest), $"Upload User Bulk Insert Error => {e.Message}", e.InnerException);
                return null;
            }
           
        }
    }
}
