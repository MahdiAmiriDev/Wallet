using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ApplicationPaymentMethodRepository : GenericApplicationBasedRepository<ApplicationPaymentMethod, int>, IApplicationPaymentMethodRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public ApplicationPaymentMethodRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        public IQueryable<PaymentMethodType> GetApplicationPaymentMethods(int applicationId, bool readOnly)
        {
            return DbSet.Where(x => x.ApplicationId == applicationId).Select(x =>
            new PaymentMethodType
            {
                Id = x.PaymentMethodType,
                Title = x.PaymentMethodTypeNavigation.Title
            });
        }

        public IQueryable<ApplicationPaymentMethod> GetByCustomFilter(int? paymentMethodType)
        {
            var query = GetAll(true);
            if (paymentMethodType.HasValue)
            {
                query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
            }
            return query;
        }
    }
}
