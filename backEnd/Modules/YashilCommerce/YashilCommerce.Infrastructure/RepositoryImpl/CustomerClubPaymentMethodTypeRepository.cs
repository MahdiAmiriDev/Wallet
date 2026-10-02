using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CustomerClubPaymentMethodTypeRepository : GenericRepository<CustomerClubPaymentMethodType, int>, ICustomerClubPaymentMethodTypeRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CustomerClubPaymentMethodTypeRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<CustomerClubPaymentMethodType> GetByCustomFilter(int? paymentMethodType, int? customerClubId)
        {
            var query = GetAll(true);
            if (paymentMethodType.HasValue)
            {
                query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
            }
            if (customerClubId.HasValue)
            {
                query = query.Where(x => x.CustomerClubId == customerClubId.Value);
            }
            return query;
        }

        public IQueryable<PaymentMethodType> GetByUserId(int userId)
        {
            return DbSet.Where(x => x.CustomerClub.AdditionalUserProp
                .Any(c => c.UserId == userId))
                .Select(x =>
            new PaymentMethodType
            {
                Id = x.PaymentMethodType,
                Title = x.PaymentMethodTypeNavigation.Title
            });
        }

        public IQueryable<PaymentMethodType> GetByCustomerClubId(int customerClubId)
        {
            return DbSet.Where(x => x.CustomerClubId == customerClubId)
                .Select(x =>
                    new PaymentMethodType
                    {
                        Id = x.PaymentMethodType,
                        Title = x.PaymentMethodTypeNavigation.Title
                    });
        }
    }
}
