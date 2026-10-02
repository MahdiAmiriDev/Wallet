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
    public class CompanyGroupService : GenericService<CompanyGroup, int>, ICompanyGroupService
    {
        private readonly ICompanyGroupRepository _companyGroupRepository;
        public CompanyGroupService(IUnitOfWork unitOfWork, ICompanyGroupRepository companyGroupRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, companyGroupRepository, userPrincipal, eventPublisher)
        {
            _companyGroupRepository = companyGroupRepository;
        }


        public IQueryable<CompanyGroup> GetByCustomFilter(int? companyGroupId)
        {
            return _companyGroupRepository.GetByCustomFilter(companyGroupId);
        }
    }
}
