using System.Linq;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class AffiliateService : GenericService<Affiliate, int>, IAffiliateService
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly IAffiliateRepository _affiliateRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public AffiliateService(IUnitOfWork unitOfWork, IAffiliateRepository affiliateRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher)
            : base(unitOfWork, affiliateRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _affiliateRepository = affiliateRepository;
            _userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<Affiliate> GetByCustomFilter(int? addressId)
        {
            return _affiliateRepository.GetByCustomFilter(addressId);
        }


    }
}
