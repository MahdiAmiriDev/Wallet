using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CategoryRepository : GenericApplicationBasedRepository<Category, int>, ICategoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public CategoryRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }

        public override IQueryable<Category> GetAll(bool readOnly = false, int? applicationId = null)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp != null)
            {
                // TODO:fixed Filter
                return
                    base.GetAll(
                        readOnly); //.Where(x => x.CompanyMainCategory. == additionalUserProp.CompanyMainCategory);
            }

            return base.GetAll(readOnly);
        }

        public IQueryable<Category> GetByCompanyMainCategory(int companyId)
        {
            // TODO:fixed Filter
            return
                DbSet.Where(
                    ApplicationBasedDefaultFilter()); // .Where(x => x.CompanyMainCategory.CompanyMainCategory == companyId);
        }

        public IQueryable<Category> GetByCompanyMainCategoryId(int companyMainCategoryId)
        {
            // TODO:fixed Filter
            return
                DbSet.Where(
                    ApplicationBasedDefaultFilter()); //.Where(x => x.CompanyMainCategoryId == companyMainCategoryId);
        }

        public IQueryable<Category> GetRepresentationCategories(int representationId)
        {
            var childCategories =
                from rc in _context.RepresentationCategory.Where(x => x.RepresentationId == representationId)
                from childCategory in GetAll(true, null)
                    .Where(childCategory => childCategory.CodePath.StartsWith(rc.Category.CodePath))
                select childCategory;
            var parentCategories =
                from rc in _context.RepresentationCategory.Where(x => x.RepresentationId == representationId)
                from parentCategory in GetAll(true, null)
                    .Where(parentCategory => rc.Category.CodePath.Contains(parentCategory.CodePath))
                select parentCategory;
            return childCategories.Union(parentCategories).Distinct();
        }

        public IQueryable<Category> GetRepresentationCategories(int representationId, int categoryId)
        {
            var representationCourseCategories = GetRepresentationCategories(representationId);
            var category = _context.Category.Find(categoryId);
            if (category != null)
            {
                return representationCourseCategories.Where(x => x.CodePath.StartsWith(category.CodePath));
            }

            return null;
        }

        public IQueryable<int> GetRepresentationCategoriesId(int representationId, int categoryId)
        {
            return GetRepresentationCategories(representationId, categoryId).Select(x => x.Id);
        }



        public override ValueTask<Category> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var firstOrDefaultAsync = DbSet.Where(ApplicationBasedDefaultFilter()).Include(x => x.CompanyMainCategory)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<Category>(firstOrDefaultAsync);
        }

        public IEnumerable<CategoryCustomModel> GetUsedCategories(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            string? categoryTitle,
            int? categoryGroupId = null,
            bool showIfHasPrice = true)
        {
            return _context.CategorySimpleModels.FromSqlRaw("EXECUTE [shp].[GetUsedCategories] {0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , applicationId, searchTitle, companyId, representationId, cityId, parentId, loadLevelOneNodes, customerClubId, categoryGroupId, categoryTitle);
        }


        public IEnumerable<CategoryCustomModel> GetRootUsedCategories(
            int? parentId,
            int? companyId,
            int? representationId,
            int? cityId,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId,
            bool showIfHasPrice = true)
        {
            return _context.CategorySimpleModels.FromSqlRaw("EXECUTE [shp].[GetRootUsedCategories]" +
                                                            " {0},{1},{2},{3},{4},{5},{6},{7}"
                , applicationId, companyId, representationId, cityId, parentId, loadLevelOneNodes, customerClubId, categoryGroupId);
        }

        public IQueryable<Category> GetCategoryWithParents(int categoryId)
        {
            var category = DbSet.FirstOrDefault(x => x.Id == categoryId);
            if (category != null)
            {
                return DbSet.Where(x => category.CodePath.StartsWith(x.CodePath));
            }

            return null;
        }

        public IQueryable<Category> GetByCustomFilter(int? parentId, int? companyId, int? representationId,
            bool loadLevelOneNodes)
        {
            return GetCategoriesByCustomFilter(_userPrincipal.ApplicationId, parentId, companyId, representationId,
                loadLevelOneNodes);
        }

        public IQueryable<Category> GetApplicationCategories(int applicationId, int? companyId)
        {
            if (companyId.HasValue)
            {
                return GetCategoriesByCustomFilter(applicationId, null, companyId, null, false).Distinct();
            }

            var categories =
                DbSet.Where(x => x.ApplicationId == applicationId && !x.Deleted && x.ProductCategory.Any());
            var res = from c in categories
                      from p in DbSet.Where(x =>
                          x.ApplicationId == applicationId && !x.Deleted && c.CodePath.Contains(x.CodePath))
                      select p;
            return res.AsNoTracking().Distinct().OrderBy(x => x.DisplayOrder);
        }

        private IQueryable<Category> GetCategoriesByCustomFilter(int applicationId, int? parentId, int? companyId,
            int? representationId, bool loadLevelOneNodes)
        {
            if (representationId.HasValue)
            {
                Representation representation = _context.Representation.FirstOrDefault(x => x.Id == representationId);
                if (representation != null)
                {
                    companyId = representation.CompanyId;
                }
                else
                {
                    return null;
                }
            }

            var query = GetAll(true);
            if (loadLevelOneNodes)
            {
                query = query.Where(x => !x.ParentId.HasValue);
            }

            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            if (companyId.HasValue)
            {
                var levelOneCategories = _context.Category
                    .Where(x => !x.CompanyId.HasValue &&
                                x.ApplicationId == applicationId).AsNoTracking();
                query = from rc
                        in _context.CompanyMainCategory.Where(x => x.ApplicationId == applicationId)
                            .Where(x => x.CompanyId == companyId)
                        from childCategory in _context.Category.Where(x => x.ApplicationId == applicationId)
                            .Where(childCategory => childCategory.CodePath.StartsWith(rc.Category.CodePath))
                        select childCategory;
                var parentNode = _context.Category.Where(x => x.ApplicationId == applicationId)
                    .Where(x => !x.ParentId.HasValue);


                query = query.Union(parentNode).Union(levelOneCategories);
            }
            else
            {
                query = query.Where(x => !x.CompanyId.HasValue).AsNoTracking();
            }


            return query;
        }

        public override Task<Category> AddAsync(Category t)
        {
            if (!t.ParentId.HasValue)
            {
                var rootCategory = DbSet.Where(ApplicationBasedDefaultFilter())
                    .FirstOrDefault(x => !x.ParentId.HasValue);
                if (rootCategory != null)
                {
                    t.ParentId = rootCategory.Id;
                }
            }

            return base.AddAsync(t);
        }

        public string GetTitle(int id)
        {
            return _context.Category.AsNoTracking().Where(x => x.Id == id).Select(x => x.Title).FirstOrDefault();
        }

        /// <summary>
        /// پایین ترین دسته محصول یک مرکز را به همراه دسته پدر آن بر می گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<LowLevelCategoryCustomModel> GetLowLevelCategoryWithParent(int companyId)
        {
            var companyMainCategory = from cmc in _context.CompanyMainCategory.Where(x => x.CompanyId == companyId)
                                      join c in _context.Category
                                          on cmc.CategoryId equals c.Id
                                      select new Category() { Id = c.Id, CodePath = c.CodePath };

            var lowLevelCategory = from pc in _context.Category
                                   join llc in _context.Category.Where(b =>
                                           companyMainCategory.Any(x => b.CodePath.StartsWith(x.CodePath))
                                           && !_context.Category.Any(c => c.ParentId == b.Id))
                                       on pc.Id equals llc.ParentId
                                   select new LowLevelCategoryCustomModel()
                                   {
                                       Title = llc.Title,
                                       Id = llc.Id,
                                       ParentId = llc.ParentId,
                                       ParentTitle = pc.Title
                                   };
            return lowLevelCategory;
        }

        public IQueryable<Category> GetRepresentationMainCategories(int representationId)
        {
            // var representationCategories = from rc in _context.RepresentationCategory.Where(x => x.RepresentationId == representationId)
            //     join category in _context.Category
            //         on rc.CategoryId equals category.Id
            //     select new Category() { Id = category.Id, CodePath = category.CodePath };

            //بدیت آوردن دسته بندی های یک نمایندگی که به آن اختصاص داده شده است
            var representationCategories = _context.RepresentationCategory
                .Where(x => x.RepresentationId == representationId).Select(rc => new Category()
                { Id = rc.Category.Id, CodePath = rc.Category.CodePath });
            // بدست آوردن نود اصلی دسته بندی
            // و نودهای سطح 1
            // سپس جوین زدن آن با دسته بندی های نمایندگی
            var mainCategories =
                from rootNode in _context.Category.Where(rc => !rc.ParentId.HasValue)
                join mainCategory in _context.Category
                        .Where(mc =>
                            representationCategories.Any(rc => rc.CodePath.StartsWith(mc.CodePath)))
                    on rootNode.Id equals mainCategory.ParentId
                select new Category() { Id = mainCategory.Id, CodePath = mainCategory.CodePath };
            return mainCategories;
        }

        // public IQueryable<Category> GetByMediaCustomFilter()
        // {
        //     var query = base.GetAll(true,_applicationConfiguration.ApplicationId);
        //     query = query.Where(x => x.MediaCategory.Any());
        //     return query;
        // }

        public bool CheckAllMainCategoriesHasUnitOfMeasurementAndCode()
        {
            var parentCategoryId =
                DbSet.Where(x => x.ParentId == null && x.ApplicationId == _userPrincipal.ApplicationId).AsNoTracking().Select(x => x.Id).FirstOrDefault();
            var mainCategories = GetAll(true).Where(x => x.ParentId == parentCategoryId);
            return !mainCategories
                .Any(x => string.IsNullOrEmpty(x.Code) || string.IsNullOrEmpty(x.UnitOfMeasurement));
        }

        public Task UpdateCodePathAsync()
        {
            return _context.Database.ExecuteSqlRawAsync("EXECUTE [dbo].[updateCodePath] 'shp', 'category'");
        }

        public IQueryable<Category> GetByCompanyId(int companyId, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.CompanyId == companyId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public Task<bool> CategoryIsParentAsync(int categoryId)
        {
            return DbSet.Where(x => x.ParentId == categoryId)
                .AnyAsync();
        }

        public IQueryable<Category> FindById(int id)
        {
            return DbSet.Where(x => x.Id == id);
        }

        public async Task<(IQueryable<Category> categories, int? currentUserCompanyId)> GetCurrentUserCategoriesAsync()
        {
            var currentUserCompanyId = await _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => x.CompanyId)
                .FirstOrDefaultAsync();

            var query = DbSet.Where(x => x.ApplicationId == _userPrincipal.ApplicationId &&
                                    (!x.CompanyId.HasValue || x.CompanyId == currentUserCompanyId)
                         );
            return (query, currentUserCompanyId);

        }

        /// <summary>
        /// دریافت دسته بندی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <returns> IQueryable<Category></returns>
        public IQueryable<Category> GetAllByApplicationId(int applicationId, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.ApplicationId == applicationId);

            return readOnly ? query.AsNoTracking() : query;
        }


        public IQueryable<Category> GetCategoryTreeByCodePath(string codePath)
        => _context.Category.FromSql($"EXEC shp.GetCategoryTreeByCodePath @CodePath = {codePath}");
    }
}