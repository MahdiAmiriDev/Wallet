using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CompanyMainCategoryRepository : GenericApplicationBasedRepository<CompanyMainCategory, int>, ICompanyMainCategoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICompanyRepository _companyRepository;

        public CompanyMainCategoryRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ICompanyRepository companyRepository) :
            base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _companyRepository = companyRepository;
        }

        public async Task AssignSelectedItemsToCompany(List<int> categories, int companyId, bool assign)
        {

            if (!assign)
            {
                var companyCategoriesForRemove =
                DbSet.Where(x =>
                    x.CompanyId == companyId &&
                    !_context.ProductCategory.Any(pc => pc.Product.CompanyId == companyId
                      && pc.Category.CodePath.StartsWith(x.Category.CodePath)));

                DbSet.RemoveRange(companyCategoriesForRemove.Where(x =>
                    categories.Contains(x.CategoryId)));
            }
            else
            {
                foreach (var companyMainCategory in categories.Select(categoryId =>
                    new CompanyMainCategory
                    {
                        CreateBy = _userPrincipal.Id,
                        CreationDate = DateTime.Now,
                        CreatorOrganizationId = _userPrincipal.OrganizationId,
                        ApplicationId = _userPrincipal.ApplicationId,
                        CompanyId = companyId,
                        CategoryId = categoryId
                    }))
                {
                    await AddAsync(companyMainCategory);
                }
            }
        }

        public IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCompanyMainCategory(int? id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CompanyMainCategory> GetMainCategoryByRepresentationId(int representationId)
        {
            var representation = _context.Representation.FirstOrDefault(x => x.Id == representationId);
            if (representation != null)
            {
                return GetMainCourseCategoriesByCompanyId(representation.CompanyId);
            }

            return null;
        }

        public IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCategoryId(int categoryId, in bool hierarchical = true)
        {
            var query = GetAll(true);
            // TODO:fixed Filter
            //     .Include(x => x.MainCategory).Include(x => x.Category)
            //     .Where(ApplicationBasedDefaultFilter());
            // query = query.Where(x => x.Category.Any(c => c.CodePath.StartsWith("," + categoryId)));
            return query;
        }

        public IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCompanyId(int? id)
        {
            var query = GetAll(true);
            // TODO:fixed Filter
            // .Include(x => ).Include(x => x.Category)
            // .Where(ApplicationBasedDefaultFilter());
            if (id.HasValue)
            {
                return query.Where(x => x.CompanyId == id);
            }

            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp != null && additionalUserProp.CompanyId.HasValue)
            {
                return query.Where(x => x.CompanyId == additionalUserProp.CompanyId);
            }

            return query;
        }

        public override IQueryable<CompanyMainCategory> GetAll(bool readOnly = false, int? applicationId = null)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp != null && additionalUserProp.CompanyId.HasValue)
            {
                return base.GetAll(readOnly)
                    .Where(x => x.CompanyId == additionalUserProp.CompanyId);
            }

            return base.GetAll(readOnly);
        }

        public IQueryable<CompanyMainCategory> GetByCustomFilter(int? companyId, int? mainCategoryId)
        {
            var query = GetAll(true);
            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            if (mainCategoryId.HasValue)
            {
                // TODO:fixed Filter
                query = query; //.Where(x => x. == mainCategoryId.Value);
            }


            return query;
        }

        public IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned)
        {
            var rootCategory = _context.Category.Where(x => x.ApplicationId == _userPrincipal.ApplicationId).FirstOrDefault(x => !x.ParentId.HasValue);
            if (assigned)
            {
                return DbSet.Where(ApplicationBasedDefaultFilter()).Where(x => x.CompanyId == id && (!onlyRootCategories || onlyRootCategories && x.Category.ParentId == rootCategory.Id)).Select(x => x.Category);
            }

            var companyMainCategories = DbSet.Where(ApplicationBasedDefaultFilter()).Where(x => x.CompanyId == id).Select(x => x.Category);
            return _context.Category.Where(x => x.ApplicationId == _userPrincipal.ApplicationId).Where(x => !companyMainCategories.Contains(x) && (!onlyRootCategories || onlyRootCategories && x.ParentId == rootCategory.Id));
        }
        /// <summary>
        /// بررسی می کند که آیا دسته بندی محصول از دسته بندی اصلی وجود دارد
        /// </summary>
        /// <param name="companyMainCategoryId">شناسه دسته بندی اصلی مرکز</param>
        /// <returns></returns>
        public Task<bool> CheckExistsProductCategoryOfMainCategoryAsync(int companyMainCategoryId)
        {
            return DbSet.AnyAsync(x => x.Id == companyMainCategoryId &&
            _context.ProductCategory
                .Any(pc => pc.Product.CompanyId == x.CompanyId &&
                    pc.Category.CodePath.StartsWith(x.Category.CodePath)));
        }

        public Task<bool> CheckExistCompanyMainCategoryByCategoryIdAsync(int categoryId)
        {
            return DbSet.Where(x => x.CategoryId == categoryId)
                .AnyAsync();
        }

        public IQueryable<CompanyMainCategory> FindById(int id,bool readOnly = true)
        {
            return GetAll(readOnly).Where(x => x.Id == id);
        }

        public Task DeleteByCategoryIdAsync(int categoryId)
        {
            return DbSet.Where(x => x.CategoryId == categoryId)
                .DeleteFromQueryAsync();
        }

        /// <summary>
        /// حذف بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Task DeleteByCompanyIdAsync(int companyId)
        {
            return DbSet.Where(x=>x.CompanyId == companyId)
                .DeleteFromQueryAsync();
        }
    }
}