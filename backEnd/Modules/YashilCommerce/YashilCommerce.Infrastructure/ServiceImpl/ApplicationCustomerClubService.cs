	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ApplicationCustomerClubService : GenericService<ApplicationCustomerClub,int>, IApplicationCustomerClubService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IApplicationCustomerClubRepository _applicationCustomerClubRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ApplicationCustomerClubService (IUnitOfWork unitOfWork, IApplicationCustomerClubRepository applicationCustomerClubRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, applicationCustomerClubRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_applicationCustomerClubRepository = applicationCustomerClubRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ApplicationCustomerClub> GetByCustomFilter( int? customerClubId)
        {
            return _applicationCustomerClubRepository.GetByCustomFilter(customerClubId);
        }
           

    }
}      
 