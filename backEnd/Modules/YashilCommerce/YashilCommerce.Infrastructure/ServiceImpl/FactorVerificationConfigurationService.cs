using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorVerificationConfigurationService : GenericService<FactorVerificationConfiguration, int>,
        IFactorVerificationConfigurationService
    {
        private readonly IFactorVerificationConfigurationRepository _factorVerificationConfigurationRepository;

        public FactorVerificationConfigurationService(IUnitOfWork unitOfWork,
            IFactorVerificationConfigurationRepository factorVerificationConfigurationRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, factorVerificationConfigurationRepository, userPrincipal, eventPublisher)
        {
            _factorVerificationConfigurationRepository = factorVerificationConfigurationRepository;
        }


        public bool CheckExistenceActiveFactorVerificationConfiguration()
        {
            return _factorVerificationConfigurationRepository.CheckExistenceActiveFactorVerificationConfiguration();
        }

        /// <summary>
        /// get the active factorVerification based on applicationId
        /// </summary>
        /// <returns></returns>
        public FactorVerificationConfiguration GetActiveFactorVerificationConfiguration()
        {
            return _factorVerificationConfigurationRepository.GetActiveFactorVerificationConfiguration();
        }

        public bool CheckExistenceActiveFactorVerification()
        {
            return _factorVerificationConfigurationRepository.CheckExistenceActiveFactorVerification();
        }

        public void DeactivateOldFactorVerificationConfigurations()
        {
            _factorVerificationConfigurationRepository.DeactivateOldFactorVerificationConfigurations();
        }
    }
}