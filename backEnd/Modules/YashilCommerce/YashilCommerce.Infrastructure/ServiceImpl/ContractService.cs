using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ContractService : GenericService<Contract, int>, IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ISaleConfigurationService _saleConfigurationService;
        public ContractService(IUnitOfWork unitOfWork,
            IContractRepository contractRepository,
            IUserPrincipal userPrincipal,
             ISaleConfigurationService saleConfigurationService, IEventPublisher eventPublisher)
            : base(unitOfWork, contractRepository, userPrincipal, eventPublisher)
        {
            _contractRepository = contractRepository;
            _saleConfigurationService = saleConfigurationService;
        }

        public IQueryable<Contract> GetByCustomFilter(int? companyId, int? representationId,
            int? contractType, int? contractStatus, int? addressId)
        {
            return _contractRepository.GetByCustomFilter(companyId, representationId, contractType, contractStatus, addressId);
        }


        public async Task DeactivateOldContractsAsync(int companyId, int? representationId,int currentSaleConfigurationId)
        {
            await _saleConfigurationService.DeactivateOldSaleConfigurationsAsync(companyId, representationId, currentSaleConfigurationId);
        }

        public Contract GetWithSaleConfiguration(int contractId, bool readOnly)
        {
            return _contractRepository.GetWithSaleConfiguration(contractId, readOnly);
        }


    }
}