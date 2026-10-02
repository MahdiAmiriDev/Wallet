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
    public class FactorDeductionService : GenericService<FactorDeduction, int>, IFactorDeductionService
    {
        private readonly IFactorDeductionRepository _factorDeductionRepository;

        public FactorDeductionService(IUnitOfWork unitOfWork, IFactorDeductionRepository factorDeductionRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, factorDeductionRepository, userPrincipal, eventPublisher)
        {
            _factorDeductionRepository = factorDeductionRepository;
        }

        public IQueryable<FactorDeduction> GetByCustomFilter(int? factorId)
        {
            return _factorDeductionRepository.GetByCustomFilter(factorId);
        }
    }
}