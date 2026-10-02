using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ContractRepository : GenericApplicationBasedRepository<Contract, int>, IContractRepository
    {
        private readonly YashilAppDbContext _context;

        public ContractRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
        }


        public IQueryable<Contract> GetByCustomFilter(int? companyId, int? representationId,
            int? contractType, int? contractStatus, int? addressId)
        {
            var query = GetAll(true);
            if (companyId.HasValue)
            {
                query = query.Where(x => x.SaleConfiguration.CompanyId == companyId.Value);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.SaleConfiguration.RepresentationId == representationId.Value);
            }
            if (contractType.HasValue)
            {
                query = query.Where(x => x.ContractType == contractType.Value);
            }

            if (contractStatus.HasValue)
            {
                query = query.Where(x => x.ContractStatus == contractStatus.Value);
            }
            return query;
        }

        public override void CustomiseEntityBeforeAdd(Contract t)
        {
            SetApplicationBasedEntityDefaultPropValues(t.SaleConfiguration);
            SetEntityDefaultPropValues(t.SaleConfiguration);
            base.CustomiseEntityBeforeAdd(t);
        }

        public override Contract Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.SaleConfiguration)
                .FirstOrDefault(x => x.Id == (int)id);
        }

        public override ValueTask<Contract> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var firstOrDefaultAsync = DbSet.Include(x => x.SaleConfiguration)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<Contract>(firstOrDefaultAsync);
        }

        public Contract GetActiveContractUser(int userId)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
            if (additionalUserProp?.CompanyId != null)
            {
                return DbSet.Where(x =>
                    x.SaleConfiguration.CompanyId == additionalUserProp.CompanyId &&
                    x.SaleConfiguration.IsActive &&
                    ((additionalUserProp.RepresentationId.HasValue &&
                                                     x.SaleConfiguration.RepresentationId == additionalUserProp.RepresentationId.Value) ||
                                                     (!additionalUserProp.RepresentationId.HasValue && !x.SaleConfiguration.RepresentationId.HasValue))
                    )
                    .Include(x => x.SaleConfiguration).ThenInclude(x => x.YashilScheduler).FirstOrDefault();
            }

            return null;

        }

        public Contract GetWithSaleConfiguration(int contractId, bool readOnly)
        {
            return DbSet.Include(x => x.SaleConfiguration).FirstOrDefault(x => x.Id == contractId);
        }

        public override Contract Delete(object id, bool logical = false)
        {
            var saleConfiguration = _context.SaleConfiguration.FirstOrDefault(x => x.Contract.Any(c => c.Id == (int)id));
            if (saleConfiguration != null)
            {
                _context.Remove(saleConfiguration);
            }
            return base.Delete(id, logical);
        }
        public override Contract Delete(Contract entity)
        {
            var saleConfiguration = _context.SaleConfiguration.FirstOrDefault(x => x.Contract.Any(c => c.Id == entity.Id));
            if (saleConfiguration != null)
            {
                _context.Remove(saleConfiguration);
            }
            return base.Delete(entity);
        }
    }
}