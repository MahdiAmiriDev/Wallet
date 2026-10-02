using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class SubsidyRepository : GenericApplicationBasedRepository<Subsidy, int>, ISubsidyRepository
    {
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly YashilAppDbContext _context;
        public SubsidyRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }

        public Task<Subsidy> GetApplicationCurrentSubsidyAsync(int applicationId)
        {
            return DbSet.FirstOrDefaultAsync(x => x.ApplicationId == applicationId && !x.Deleted && x.IsActive);
        }

        public Subsidy GetCurrentSubsidy()
        {
            return DbSet.FirstOrDefault(x => x.ApplicationId == _userPrincipal.ApplicationId && !x.Deleted && x.IsActive);
        }
        /// <summary>
        /// تنظیم یارانه قبلی برنامه را غیر فعال می کند 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public async Task DeactivateOldSubsidyByApplicationIdAsync(int applicationId)
        {
            var subsidy = DbSet.Where(x => x.ApplicationId == applicationId && x.IsActive);

            await subsidy.UpdateAsync(x => new Subsidy()
            { IsActive = false, IsSataActive = false, IsVezaratActive = false });
        }

        public IQueryable<Subsidy> GetByCustomFilter(int? categoryGroupId, int? subsidyGroupId)
        {
            var query = GetAll(true);
            if (categoryGroupId.HasValue)
            {
                query = query.Where(x => x.CategoryGroupId == categoryGroupId);
            }

            if (subsidyGroupId.HasValue)
            {
                query = query.Where(x => x.SubsidyGroupId == subsidyGroupId);
            }
            return query.AsNoTracking();
        }

        public IQueryable<Subsidy> FindById(int id, bool readOnly = true)
        {
            var applicationId = _userPrincipal.ApplicationId != 0 ?
                _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId;
            return GetAll(readOnly, applicationId).Where(x => x.Id == id);
        }
        public IQueryable<Subsidy> GetByCategoryGroupId(int? categoryGroupId, bool readOnly = true)
        {
            var applicationId = _userPrincipal.ApplicationId != 0 ?
                _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId;
            return GetAll(readOnly, applicationId).Where(x => x.CategoryGroupId == categoryGroupId);
        }
        public async Task UpdateSubsidyStatusBySubsidyConsumption(int applicationId)
        {
            await _context.Database.ExecuteSqlRawAsync($"shp.UpdateSubsidyStatusBySubsidyConsumption {applicationId}");
        }
    }
}
