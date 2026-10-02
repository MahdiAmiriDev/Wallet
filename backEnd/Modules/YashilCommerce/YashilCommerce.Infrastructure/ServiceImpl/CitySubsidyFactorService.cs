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
	public class CitySubsidyFactorService : GenericService<CitySubsidyFactor,int>, ICitySubsidyFactorService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly ICitySubsidyFactorRepository _citySubsidyFactorRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public CitySubsidyFactorService (IUnitOfWork unitOfWork, ICitySubsidyFactorRepository citySubsidyFactorRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, citySubsidyFactorRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_citySubsidyFactorRepository = citySubsidyFactorRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<CitySubsidyFactor> GetByCustomFilter( int? cityId)
        {
            return _citySubsidyFactorRepository.GetByCustomFilter(cityId);
        }
           

    }
}      
 