using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class AdditionalUserPropRepository : GenericApplicationBasedRepository<AdditionalUserProp, int>,
        IAdditionalUserPropRepository
    {
        private readonly IRepresentationCategoryRepository _representationCategoryRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public AdditionalUserPropRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, IRepresentationCategoryRepository representationCategoryRepository, ApplicationConfiguration applicationConfiguration) : base(context,
            userPrincipal)
        {
            _representationCategoryRepository = representationCategoryRepository;
            _applicationConfiguration = applicationConfiguration;
        }

        /// <summary>
        /// چک میکند کاربری با شناسه باشگاه و کد باشگاه وجود دارد یا نه
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <param name="customerClubCode"></param>
        /// <returns></returns>
        public bool CheckExistenceByFilter(int? customerClubId, string customerClubCode)
        {
            if (customerClubId.HasValue && !string.IsNullOrEmpty(customerClubCode))
            {
                var query = DbSet.Where(x =>
                    x.CustomerClubId == customerClubId && x.CustomerClubCode == customerClubCode);
                return query.Any();
            }
            else
            {
                return false;
            }
        }

        public AdditionalUserProp GetByUserIdAndApplicationId(int userId, int applicationId)
        {
            return DbSet.FirstOrDefault(x => x.UserId == userId && x.ApplicationId == applicationId);
        }

        public IQueryable<AdditionalUserProp> GetByCustomFilter(int? userId, int? applicationId)
        {
            var query = GetAll(true);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (applicationId.HasValue)
            {
                query = query.Where(x => x.ApplicationId == applicationId);
            }
            return query.AsNoTracking();
        }

        public IQueryable<AdditionalUserProp> GetByCompanyId(int companyId, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.CompanyId == companyId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId)
        {
            return DbSet.AnyAsync(x => x.RepresentationCategory.CategoryId == categoryId);
        }

        public Task<bool> ExistsAnyAdditionalUserPropServiceByItemGroupId(int itemGroupId)
        {
            return DbSet.AnyAsync(x => x.AdditionalUserPropGroupId == itemGroupId);
        }

        public IQueryable<AdditionalUserProp> GetUsersAssignToGroup(int groupId, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.AdditionalUserPropGroupId == groupId);
            if (readOnly)
            {
                return query.AsNoTracking();
            }
            return query;
        }

        public IQueryable<AdditionalUserProp> GetUsersForNewGroup(bool readOnly = true)
        {
            var query = DbSet.Where(x => !x.AdditionalUserPropGroupId.HasValue);
            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<AdditionalUserProp> FindByUserId(int id)
        {
            return DbSet.Where(x => x.UserId == id);
        }

        public AdditionalUserProp GetByUserId(int userId, bool readOnly = false)
        {
            var query = readOnly ? DbSet : DbSet.AsNoTracking();
            return query.FirstOrDefault(x => x.UserId == userId);
        }
        public async Task<IQueryable<AdditionalUserProp>> GetAdditionalUserPropNotAssignedToGroupAsync(
            int groupId,
            int? companyId,
            int? representationId,
            int? categoryId,
            int? userGroupId)
        {
            var query = DbSet.Where(x =>
                x.ApplicationId == _applicationConfiguration.ApplicationId &&
                !x.AdditionalUserPropGroupId.HasValue);
            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId);
            }
            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId);
            }

            if (representationId.HasValue && categoryId.HasValue)
            {
                // دریافت دسته بندی نمیاندگی
                var representationCategoryId =
                      await _representationCategoryRepository.GetByCustomFilter(
                              representationId.Value, categoryId)
                          .Select(x => x.Id).FirstOrDefaultAsync();
                query = query.Where(x => x.RepresentationCategoryId == representationCategoryId);
            }
            if (userGroupId.HasValue)
            {
                query = query.Where(x => x.User.UserGroupMappingUser.Any(ugm => ugm.UserGroupId == userGroupId.Value));
            }
            return query;
        }
        public IQueryable<AdditionalUserProp> GetAdditionalUsersPropAssignedToGroup(int itemGroupId)
        {
            var query = DbSet.Where(x => x.AdditionalUserPropGroupId == itemGroupId);
            return query;
        }
        public async Task AssignSelectedItemsToGroupAsync(List<int> additionalUserPropIds, int groupId, bool assign)
        {
            if (!assign)
            {
                await DbSet.Where(x => x.AdditionalUserPropGroupId == groupId && additionalUserPropIds.Contains(x.Id))
                    .UpdateFromQueryAsync(aup => new AdditionalUserProp
                    {
                        AdditionalUserPropGroupId = null
                    });
            }
            else
            {
                await DbSet.Where(x => !x.AdditionalUserPropGroupId.HasValue && additionalUserPropIds.Contains(x.Id))
                    .UpdateFromQueryAsync(aup => new AdditionalUserProp
                    {
                        AdditionalUserPropGroupId = groupId
                    });
            }
        }

    }
}