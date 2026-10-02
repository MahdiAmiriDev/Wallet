using System.Collections.Generic;
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
    public class SpecificationOptionService : GenericService<SpecificationOption, int>, ISpecificationOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISpecificationOptionRepository _specificationOptionRepository;
        private readonly IUserPrincipal _userPrincipal;

        public SpecificationOptionService(IUnitOfWork unitOfWork,
            ISpecificationOptionRepository specificationOptionRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher)
            : base(unitOfWork, specificationOptionRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _specificationOptionRepository = specificationOptionRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<SpecificationOption> GetByCustomFilter(int? specificationId)
        {
            return _specificationOptionRepository.GetByCustomFilter(specificationId);
        }

        public IQueryable<SpecificationOption> GetByCategoryIds(List<int> editModelCategories)
        {
            return _specificationOptionRepository.GetByCategoryIds(editModelCategories);
        }
    }
}