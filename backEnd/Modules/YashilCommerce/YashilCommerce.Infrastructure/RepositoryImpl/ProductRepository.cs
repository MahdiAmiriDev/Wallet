using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductRepository : GenericApplicationBasedRepository<Product, int>, IProductRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            ICategoryRepository categoryRepository) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _categoryRepository = categoryRepository;
        }

        public IQueryable<Product> GetByCustomFilter(string title, string code, int? categoryId, int? companyId,
            int? appEntityId, int? productTemplateId, int? giftCardTypeId, int? downloadActivationType,
            int? recurringCyclePeriodType, int? taxCategoryId, int? basePriceUnitType, int? basePriceBaseUnitType, bool? showArchives)
        {

            IQueryable<Product> query = _context.Set<Product>().Where(x => x.CompanyId == companyId).AsNoTracking();
            
           
            if (categoryId.HasValue)
            {
                var category = _context.Category.FirstOrDefault(x => x.Id == categoryId.Value);
                if (category != null)
                {
                    query = query.Where(x =>
                        x.ProductCategory.Any(c => c.Category.CodePath.StartsWith(category.CodePath)));
                }
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(x => x.Code.Contains(code));
            }

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(x => x.Title.Contains(title));
            }

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            if (appEntityId.HasValue)
            {
                query = query.Where(x => x.AppEntityId == appEntityId.Value);
            }

            if (productTemplateId.HasValue)
            {
                query = query.Where(x => x.ProductTemplateId == productTemplateId.Value);
            }

            if (giftCardTypeId.HasValue)
            {
                query = query.Where(x => x.GiftCardTypeId == giftCardTypeId.Value);
            }

            if (downloadActivationType.HasValue)
            {
                query = query.Where(x => x.DownloadActivationType == downloadActivationType.Value);
            }

            if (recurringCyclePeriodType.HasValue)
            {
                query = query.Where(x => x.RecurringCyclePeriodType == recurringCyclePeriodType.Value);
            }

            if (taxCategoryId.HasValue)
            {
                query = query.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }

            if (basePriceUnitType.HasValue)
            {
                query = query.Where(x => x.BasePriceUnitType == basePriceUnitType.Value);
            }

            if (basePriceBaseUnitType.HasValue)
            {
                query = query.Where(x => x.BasePriceBaseUnitType == basePriceBaseUnitType.Value);
            }

            if (showArchives is false)
            {
                query = query.Where(x => x.Deleted == false);
            }

            return query.OrderByDescending(x => x.Id);
        }

        public int GetProductCompanyId(int productId)
        {
            return DbSet.Where(x => x.Id == productId).Select(x => x.CompanyId).FirstOrDefault();
        }

        public IQueryable<Product> GetCategoryProducts(int applicationId, int? categoryId, int? companyId, int? cityId,
            bool loadSubCategories, int count = 20)
        {
            if (count > 40)
            {
                count = 20;
            }

            var category = _context.Category.FirstOrDefault(x => x.Id == categoryId);
            if (category != null)
            {
                return DbSet.Include(x => x.ProductSalesPlanning).Where(x =>
                        x.ApplicationId == applicationId && !x.Deleted && x.VisibleIndividually && x.Published &&
                        x.ProductSalesPlanning.Any() &&
                        x.ProductCategory.Any(c => c.Category.Code.Contains(category.Code)))
                    .OrderBy(x => x.DisplayOrder)
                    .ThenByDescending(x => x.CreateBy).Take(count);
            }

            return null;
        }

        public override Product Get(object id, bool readOnly = false)
        {
            var query = DbSet.Include(x => x.ProductCategory);
            if (readOnly)
                return query.AsNoTracking().FirstOrDefault(x => x.Id == (int)id);
            else return query.FirstOrDefault(x => x.Id == (int)id);
        }

        public override ValueTask<Product> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var product = DbSet
                .Include(x => x.ProductCategory)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<Product>(product);
        }

        public IQueryable<Product> GetByCategoryIdAndCompanyId(int categoryId, int? companyId, bool hierarchical = true)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp?.CompanyId != null)
            {
                companyId = additionalUserProp.CompanyId.Value;
            }

            var category = _context.Category.Find(categoryId);


            var query = _context.Product.Include(x => x.ProductSalesPlanning).Where(x =>
                x.CompanyId == companyId && !x.Deleted && x.VisibleIndividually && x.Published);
            if (hierarchical)
            {
                query = query.Where(x => x.ProductCategory.Any(pc => pc.CategoryId == categoryId));
            }
            else
            {
                query = query.Where(
                    x => x.ProductCategory.Any(pc => pc.Category.CodePath.StartsWith(category.CodePath)));
            }

            return query.AsNoTracking();

        }

        public IQueryable<Product> GetRepresentationProductsByCategoryId(int? companyId, int? representationId,
            int? categoryId, bool hierarchical)
        {
            var representationMainCategory = _context.RepresentationCategory.Where(x =>
                !representationId.HasValue || x.RepresentationId == representationId);
            IQueryable<Product> query = DbSet;
            if (companyId.HasValue)
            {
                query = DbSet.Where(x => x.CompanyId == companyId);
            }

            if (representationId.HasValue)
            {
                query = DbSet.Where(x => x.Company.Representation.Any(r => r.Id == representationId.Value));
            }

            if (!representationId.HasValue && !categoryId.HasValue)
            {
                return query.Where(c => c.ApplicationId == _userPrincipal.ApplicationId);
            }

            var rootNode = _context.Category
                .Where(x => x.ApplicationId == _userPrincipal.ApplicationId && !x.ParentId.HasValue)
                .Select(x => new { x.Id, x.CodePath }).FirstOrDefault();
            if (rootNode != null && (!categoryId.HasValue || rootNode.Id == categoryId))
            {
                return query.Where(c => c.ApplicationId == _userPrincipal.ApplicationId
                                        && representationMainCategory.Any(rmc => c.ProductCategory
                                            .Any(pc => pc.Category.CodePath.Contains(rmc.Category.CodePath))))
                    .AsNoTracking();
            }
            else
            {
                var codePath = _categoryRepository.Get(categoryId, true).CodePath;
                return query.Where(c => c.ApplicationId == _userPrincipal.ApplicationId
                                        && representationMainCategory.Any(rmc =>
                                            codePath.StartsWith(rmc.Category.CodePath)) &&
                                        c.ProductCategory.Any(pc => pc.Category.CodePath.StartsWith(codePath)))
                    .AsNoTracking();
            }
        }

        /// <summary>
        /// این متد برای ذخیره و یا به روز رسانی اطلاعات محصول بارگذاری شده از جدول 
        /// UploadedProductItem
        /// به جدول های 
        /// product  - ProductSalePlaning - DependentProductSale - ProductCategory
        /// میباشد
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public Task<int> AddDataFromUploadedProductItemAsync(int applicationId, int userId, int companyId)
        {
            return _context.Database.ExecuteSqlRawAsync(
                $"[shp].[AddDataFromUploadedProductItem] {applicationId},{userId},{companyId},{_userPrincipal.OrganizationId} ");

        }

        /// <summary>
        /// اطلاعات کامل محصولاتی که منتشر شده و قابل مشاهده در سایت و مربوط به مرکز را بر میگرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public IEnumerable<ProductDetailDomainModel> GetCompanyConfirmedAndPublishedProducts(int companyId)
        {
            try
            {
                return _context.ProductDetailDomainModels.FromSqlRaw(
                    $"[shp].[GetCompanyConfirmedAndPublishedProducts] {companyId}");

            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// بررسی می کند که کد محصول تکراری است یا خیر 
        /// </summary>
        /// <param name="code">کد محصول</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public bool IsProductCodeDuplicate(string code, int applicationId)
        {
            return DbSet.Any(p => p.ApplicationId == applicationId && p.Code == code);
        }

        public Task<Product> GetByproductSalesPlanningIdAsync(int productSalesPlanningId)
        {
            return DbSet
                .Where(p => p.ProductSalesPlanning
                    .Any(x => x.Id == productSalesPlanningId))
                .FirstOrDefaultAsync();

        }

        public IQueryable<Product> FindById(int productId, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.Id == productId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<Product> GetByCompanyId(int companyId, bool readOnly = true)
        {
            var query = DbSet.Where(p => p.CompanyId == companyId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public Task<bool> ExistProductByCategoryIdAsync(int categoryId)
        {
            return DbSet.AnyAsync(x =>
                x.DefaultCategoryId == categoryId ||
                x.ProductCategory.Any(pc => pc.CategoryId == categoryId));
        }

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> ArchiveAsync(int id)
        {
            return await DbSet.Where(x => x.Id == id).UpdateAsync(
                x => new()
                {
                    Deleted = true
                });
        }

        /// <summary>
        /// می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RestoreFromArchiveAsync(int id)
        {
            return await DbSet.Where(x => x.Id == id).UpdateAsync(
                x => new()
                {
                    Deleted = false
                });
        }

        public IQueryable<ProductMainCategoryView> GetProductMainCategoryView() => _context.ProductMainCategoryView.AsQueryable();

        public Task<List<ProductSimpleDomainModel>> GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(int companyId)
        {
            return _context.ProductSimpleDomainModels.FromSqlRaw($"[shp].[GetSimpleCompanyConfirmedAndPublishedProducts] {companyId},{_userPrincipal.ApplicationId}").ToListAsync();
        }
    }
}