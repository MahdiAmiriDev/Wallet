using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CustomEcommerceRepository : GenericApplicationBasedRepository<Yashil.Core.Entities.Order, int>, ICustomEcommerceRepository
    {
        private readonly YashilAppDbContext _context;

        public CustomEcommerceRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
        }
        public async Task CancelExpiredOrdersAsync()
        {
            var payments = _context.Order.Where(x =>
            (x.OrderStatusType == (int)OrderStatusTypesEnum.Processing &&
            (!x.SubsidyStatusType.HasValue && x.CreationDate > DateTime.Now.AddMinutes(60))
            ||
            (
             (x.SubsidyStatusType.HasValue && x.SubsidyStatusType != (int)SubsidyStatusTypesEnum.InitialRegistration && x.CreationDate > DateTime.Now.AddDays(5))
            )
            ));
            await payments.ForEachAsync(x => x.OrderStatusType = (int)OrderStatusTypesEnum.CancelBySystem);
            _context.UpdateRange(payments);
        }
    }
}
