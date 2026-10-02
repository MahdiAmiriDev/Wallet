using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class RepresentationCategoryRepository : GenericApplicationBasedRepository<RepresentationCategory, int>, IRepresentationCategoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public RepresentationCategoryRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        public IQueryable<RepresentationCategory> GetByCustomFilter(int? representationId, int? categoryId)
        {
            var query = GetAll(true);
            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            return query;
        }

        public IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned)
        {
            var rootCategory = _context.Category.FirstOrDefault(x => !x.ParentId.HasValue);
            if (assigned)
            {
                return DbSet.Where(x => x.RepresentationId == id && (!onlyRootCategories || onlyRootCategories && x.Category.ParentId == rootCategory.Id)).Select(x => x.Category);
            }

            var companyMainCategories = DbSet.Where(x => x.RepresentationId == id).Select(x => x.Category);
            return _context.Category.Where(x => !companyMainCategories.Contains(x) && (!onlyRootCategories || onlyRootCategories && x.ParentId == rootCategory.Id));
        }
        /// <summary>
        /// دریافت دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <param name="readOnly">خواندی باشد</param>
        /// <returns></returns>
        public IQueryable<RepresentationCategory> GetByRepresentationId(int representationId, bool readOnly)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking().FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp is { RepresentationId: { } })
            {
                representationId = additionalUserProp.RepresentationId.Value;
            }
            var query = DbSet
                .Where(x => x.RepresentationId == representationId);
           
            if (additionalUserProp is { RepresentationCategoryId: { } })
            {
                query = query.Where(x => x.Id == additionalUserProp.RepresentationCategoryId.Value);
            }
            return readOnly ? query.AsNoTracking() : query;
        }

        public Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId)
        {
            return DbSet.AnyAsync(x => x.CategoryId == categoryId);
        }

        public Task DeleteByCategoryIdAsync(int categoryId)
        {
           return DbSet.Where(x => x.CategoryId == categoryId)
                .DeleteFromQueryAsync();
        }

        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public Task DeleteByRepresentationIdAsync(int representationId)
        { 
            return DbSet.Where(x=>x.RepresentationId == representationId).DeleteFromQueryAsync();
        }
    }
}
