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
    public class CompanyRepository : GenericApplicationBasedRepository<Company, int>, ICompanyRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IProductSalesPlanningRepository _productSalesPlanningRepository;
        private readonly IAdditionalUserPropCompanyGroupRepository _additionalUserPropCompanyGroupRepository;

        public CompanyRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration, IProductSalesPlanningRepository productSalesPlanningRepository, IAdditionalUserPropCompanyGroupRepository additionalUserPropCompanyGroupRepository) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _productSalesPlanningRepository = productSalesPlanningRepository;
            _additionalUserPropCompanyGroupRepository = additionalUserPropCompanyGroupRepository;
        }

        public override IQueryable<Company> GetAll(bool readOnly = false, int? applicationId = null)
        {
            var additionalUserProp = _context.AdditionalUserProp
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            // اگر برای کاربر مرکز تعریف شده باشد مرکز برمی گرداند.
            if (additionalUserProp?.CompanyId != null)
            {
                return base.GetAll(readOnly, applicationId)
                    .Where(x => x.Id == additionalUserProp.CompanyId.Value);
            }
            //در صورتی که برای کاربر مرکز تعریف نشده باشد
            //بررسی می کند که گروه مرکز تعریف شده است یا نه و اگر تعریف شده باشد AddtionalUserPropCompanyGroup در بخش
            // مرکزها رو از گروه مرکز برمی گرداند
            var userCompanyGroups = _additionalUserPropCompanyGroupRepository
                .GetUserCompanyGroups(_userPrincipal.Id, true);

            if (userCompanyGroups?.Any() == true)
            {
                var companies = base.GetAll(readOnly, applicationId);
                // استخراج شناسه مراکزی که کاربر از طریق گروه‌مرکز به آن‌ها دسترسی دارد
                // استفاده شد List به‌جای HashSet برای جلوگیری از شناسه‌های تکراری و جستجوی سریع‌تر
                var userCompanyIds = userCompanyGroups
                    .SelectMany(x => x.Company)
                    .Select(x => x.Id)
                    .ToHashSet();

                // با این کد شناسه والد مرکزهای گروه مرکز کاربر را نیز اضافه می‌کنیم
                //   کامل شود و فرزندان زیر والد خود نمایش داده شود(TreeView)تا ساختار درختی
                var parentIds = companies
                    .Where(x => userCompanyIds.Contains(x.Id) && x.ParentId.HasValue)
                    .Select(x => x.ParentId.Value);
               
                userCompanyIds.UnionWith(parentIds);

                return companies.Where(x => userCompanyIds.Contains(x.Id));
            }
            //  اگر گروه مرکز هم برای کاربر یافت نشد همه مراکز را برمیگردانیم
            return base.GetAll(readOnly, applicationId);
        }

        public IQueryable<Company> GetAllActiveCompanies(bool readOnly, int? applicationGroupId, int? applicationId)
        {
            var query = _context.Company.Where(x =>
                (x.SaleConfiguration.Any(s => s.IsActive) ||
                 x.Representation.Any(r => r.SaleConfiguration.Any(rs => rs.IsActive))) &&
                x.Confirmed && x.IsActive
                && x.Product.Any(p => p.ProductSalesPlanning.Any(
                    c => c.Product.VisibleIndividually && c.Published)));
            if (applicationId.HasValue)
            {
                query = query.Where(x => x.ApplicationId == applicationId.Value);
            }

            if (applicationGroupId.HasValue)
            {
                query = query.Where(x => x.Application.ApplicationGroupId == applicationGroupId.Value);
            }

            query = query.OrderBy(x => x.DisplayOrder);

            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<Company> GetApplicationCompanies(
            string searchTitle,
            int applicationId,
            int customerClubId,
            int? cityId,
            int? categoryId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup, bool orderByCompanyDisplayOrder = true)
        {
            var rootCategoryCodePath = _context.Category.Where(x => x.Id == categoryId).Select(x => x.CodePath)
                .FirstOrDefault();

            var query = DbSet.Where(x =>
                x.ApplicationId == applicationId &&
                x.ShowInMainPage.HasValue &&
                x.ShowInMainPage.Value &&
                
                x.IsActive &&
                x.Product.Any(c =>
                 
                    c.ProductSalesPlanning.AsQueryable().Any(
                        _productSalesPlanningRepository
                            .GetProductSalePlanningIsActiveFilter(
                                applicationId,
                                customerClubId,
                                categoryGroupId,
                                isEmploymentCategoryGroup))
                    &&
                    c.ProductSalesPlanning
                        .Any(psp =>
                            !cityId.HasValue || !psp.RepresentationId.HasValue ||
                            psp.Representation.Address.CityId == cityId.Value
                        )
                    && c.ProductCategory.Any(pc =>
                        rootCategoryCodePath == null ||
                        pc.Category.CodePath.StartsWith(rootCategoryCodePath))
                ));

            if (orderByCompanyDisplayOrder)
                query = query.OrderBy(x => x.DisplayOrder);

            return query;
        }

        public IQueryable<City> GetCityByCompanyId(int? companyId)
        {
            return _context.Address.Where(x =>
                    x.Representation.Any(r =>
                        r.IsActive &&
                        r.Confirmed &&
                        (!companyId.HasValue || r.CompanyId == companyId)))
                .Select(c => c.City).Distinct();
        }

        // TODO:Check Calc Price ***
        public IQueryable<City> GetCityByFilter(
            int customerClubId,
            int? companyId,
            int? categoryId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup,
            bool showIfHasPrice = true)
        {

            var categoryCodePath = _context.Category.AsNoTracking().Where(x => x.Id == categoryId).Select(x => x.CodePath)
                  .FirstOrDefault();

            IQueryable<City> query = _context.City.AsNoTracking().Where(x => x.Address.Any(a =>
                a.Representation.Any(r =>
                    !companyId.HasValue ||
                     (r.CompanyId == companyId.Value &&

                      r.ProductSalesPlanning.AsQueryable().Any(
                    _productSalesPlanningRepository
                        .GetProductSalePlanningIsActiveFilter(
                            _applicationConfiguration.ApplicationId,
                            customerClubId,
                            categoryGroupId,
                            isEmploymentCategoryGroup
                            ))
                      && r.ProductSalesPlanning.Any(psp =>

                                       //psp.SaleConfiguration.IsActive &&
                                       //            psp.SaleConfiguration.ToDate > todayShamsiInt &&
                                       //            (!psp.SaleConfiguration.LicenseExpireDate.HasValue ||
                                       //             psp.SaleConfiguration.LicenseExpireDate > todayShamsiInt) &&
                                       //psp.Published != null &&
                                       //psp.ApplicationId == _applicationConfiguration.ApplicationId &&
                                       //psp.Confirmed.Value &&
                                       //psp.Published.Value &&
                                       //!psp.Deleted &&
                                       //(!psp.EndDate.HasValue || psp.EndDate.Value > todayShamsiInt) &&
                                       //psp.SaleConfiguration
                                       //    .SaleConfigurationAttribute.Any(c =>
                                       //        c.CustomerClubId == customerClubId && c.IsActive
                                       // ) 

                                       //(!categoryGroupId.HasValue ||
                                       // (!isEmploymentCategoryGroup && psp.Product.DefaultCategory.CategoryGroupId == categoryGroupId.Value) ||
                                       // (isEmploymentCategoryGroup && psp.Product.ProductType == (int)ProductTypeEnum.Employment)) &&

                                       (!showIfHasPrice || (psp.Price.HasValue && psp.Price != 0))
                                      && (!categoryId.HasValue ||
                                          psp.Product.ProductCategory.Any(pc =>
                                              pc.Category.CodePath.StartsWith(categoryCodePath)))
                                    )))));
            return query;
        }

        public IQueryable<Company> GetCompaniesByCityId(int? cityId)
        {
            var query = _context.Company.Where(x => x.ApplicationId == _applicationConfiguration.ApplicationId);
            if (cityId.HasValue)
            {
                query = query.Where(x => x.Representation.Any(r => (r.Address.CityId == cityId)));
            }

            return query;
        }

        public IQueryable<Company> GetCompaniesHaveVoucherType(bool readOnly = true)
        {
            var companies = GetAll(readOnly).Where(x => x.VocherType.Any());
            return companies;
        }

        public string GetTitle(int id)
        {
            return _context.Company.AsNoTracking().Where(x => x.Id == id).Select(x => x.Title).FirstOrDefault();
        }

        public Company GetUserCompany(int userId)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
            if (additionalUserProp != null && additionalUserProp.CompanyId.HasValue)
            {
                return Get(additionalUserProp.CompanyId);
            }

            return null;
        }

        public Task<Company> GetByProductSalePlanningAsync(int productSalePlanningId)
        {
            return DbSet.FirstOrDefaultAsync(x => x.Product
                .Any(p =>
                    p.ProductSalesPlanning.Any(psp => psp.Id == productSalePlanningId)));
        }

        public IQueryable<Company> FindById(int id, bool readOnly = true)
        {
            var applicationId = _userPrincipal.ApplicationId != 0 ?
                _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId;
            return GetAll(readOnly, applicationId).Where(x => x.Id == id);
        }

        public Task UpdateCodePathAsync()
        {
            return _context.Database.ExecuteSqlRawAsync("EXECUTE [dbo].[updateCodePath] 'shp', 'Company'");
        }
        /// <summary>
        /// دریافت تمامی کمپانی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Company> GetAllCompanies(int applicationId, bool readOnly = false)
        {
            return base.GetAll(readOnly, applicationId).AsNoTracking();
        }

        /// <summary>
        /// دریافت مراکز به همراه فرزندان
        /// </summary>
        /// <param name="codePath"> codePath مرکز</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        public IQueryable<Company> GetCompaniesWithChildren(string codePath, int applicationId)
        {
            var query = _context.Company.Where(x => x.ApplicationId == applicationId);
            if (!string.IsNullOrEmpty(codePath))
            {
                query = query.Where(x => EF.Functions.Like(x.CodePath, $"%{codePath}%"));
            }
            return query.AsNoTracking();
        }
        /// <summary>
        /// دریافت پدر مرکز
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IQueryable<Company> FindParentCompany(int parentId)
        {
            var applicationId = _userPrincipal.ApplicationId != 0 ?
                _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId;
            return DbSet.Where(x => x.Id == parentId && x.ApplicationId == applicationId);
        }
    }
}