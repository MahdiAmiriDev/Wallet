using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class RepresentationEstablishedLicenseTypeService : GenericService<RepresentationEstablishedLicenseType,int>, IRepresentationEstablishedLicenseTypeService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IRepresentationEstablishedLicenseTypeRepository _representationEstablishedLicenseTypeRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public RepresentationEstablishedLicenseTypeService (IUnitOfWork unitOfWork, IRepresentationEstablishedLicenseTypeRepository representationEstablishedLicenseTypeRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, representationEstablishedLicenseTypeRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_representationEstablishedLicenseTypeRepository = representationEstablishedLicenseTypeRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<RepresentationEstablishedLicenseType> GetByCustomFilter( int? representationId, int? establishedLicenseType)
        {
            return _representationEstablishedLicenseTypeRepository.GetByCustomFilter(representationId,establishedLicenseType);
        }
           

    }
}      
 