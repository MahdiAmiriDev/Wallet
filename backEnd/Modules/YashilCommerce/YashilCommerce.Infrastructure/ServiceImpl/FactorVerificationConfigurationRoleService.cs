	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class FactorVerificationConfigurationRoleService : GenericService<FactorVerificationConfigurationRole,int>, IFactorVerificationConfigurationRoleService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IFactorVerificationConfigurationRoleRepository _factorVerificationConfigurationRoleRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public FactorVerificationConfigurationRoleService (IUnitOfWork unitOfWork, IFactorVerificationConfigurationRoleRepository factorVerificationConfigurationRoleRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, factorVerificationConfigurationRoleRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_factorVerificationConfigurationRoleRepository = factorVerificationConfigurationRoleRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<FactorVerificationConfigurationRole> GetByCustomFilter( int? factorVerificationConfigurationId, int? roleId)
        {
            return _factorVerificationConfigurationRoleRepository.GetByCustomFilter(factorVerificationConfigurationId,roleId);
        }

        public IQueryable<FactorVerificationConfigurationRole> GetFactorVerificationConfigurationRoles(int factorVerificationConfigurationId)
        {
            return _factorVerificationConfigurationRoleRepository.GetFactorVerificationConfigurationRoles(
                factorVerificationConfigurationId);
        }

        public bool CheckExistenceFactorVerificationConfigurationRole(int factorVerificationConfigurationId)
        {
            return _factorVerificationConfigurationRoleRepository.CheckExistenceFactorVerificationConfigurationRole(
                factorVerificationConfigurationId);
        }
    }
}      
 