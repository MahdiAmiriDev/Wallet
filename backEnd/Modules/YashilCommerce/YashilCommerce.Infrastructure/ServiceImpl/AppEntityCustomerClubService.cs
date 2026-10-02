using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class AppEntityCustomerClubService : GenericService<AppEntityCustomerClub, int>,
        IAppEntityCustomerClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppEntityCustomerClubRepository _appEntityCustomerClubRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;

        public AppEntityCustomerClubService(IUnitOfWork unitOfWork,
            IAppEntityCustomerClubRepository appEntityCustomerClubRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher)
            : base(unitOfWork, appEntityCustomerClubRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _appEntityCustomerClubRepository = appEntityCustomerClubRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<AppEntityCustomerClub> GetByCustomFilter(int? appEntityId, int? customerclubId)
        {
            return _appEntityCustomerClubRepository.GetByCustomFilter(appEntityId, customerclubId);
        }
    }
}