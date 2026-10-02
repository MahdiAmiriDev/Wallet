using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDtos.Representation;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using DateTime = System.DateTime;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class RepresentationRepository : GenericApplicationBasedRepository<Representation, int>, IRepresentationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IProductSalesPlanningRepository _productSalesPlanningRepository;
        public RepresentationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration, IProductSalesPlanningRepository productSalesPlanningRepository) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _productSalesPlanningRepository = productSalesPlanningRepository;
        }

        public override Representation Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.Address).FirstOrDefault(x => x.Id == (int)id);
            // return base.Get(id, readOnly);
        }

        public override ValueTask<Representation> GetAsync(object id, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.Id == (int)id);
            if (readOnly)
            {
                query = query.AsNoTracking();
            }
            var representation = query.Include(x => x.Address).FirstOrDefaultAsync();
            return new ValueTask<Representation>(representation);
        }

        public override IQueryable<Representation> GetAll(bool readOnly = false, int? applicationId = null)
        {
            var query = base.GetAll(readOnly).AsNoTracking();
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking().FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp != null)
            {
                if (additionalUserProp.RepresentationId.HasValue)
                    query = query.Where(x => x.Id == additionalUserProp.RepresentationId);
                else
                    // TODO:fixed Filter
                    query = query;//.Where(x => x.CompanyMainCategory == additionalUserProp.CompanyMainCategory);
            }

            return query;
        }


        public IQueryable<Representation> GetByCompanyMainCategory(int companyId)
        {
            // TODO:fixed Filter
            return GetAll(true);//.Where(x => x.CompanyMainCategory == companyId);
        }

        public override Representation Add(Representation t)
        {
            SetDefaultPropsForAdd(t.Address);
            return base.Add(t);
        }

        public override Task<Representation> AddAsync(Representation t)
        {
            SetDefaultPropsForAdd(t.Address);
            return base.AddAsync(t);
        }
        void SetDefaultPropsForAdd(Address t)
        {
            t.ApplicationId = _userPrincipal.ApplicationId;
            t.CreateBy = _userPrincipal.Id;
            t.CreationDate = DateTime.Now;
            t.CreatorOrganizationId = _userPrincipal.OrganizationId;
        }
        public IQueryable<Representation> GetByCustomFilter(int? addressId, int? companyId, int? founderId, int? licenseType, int? ownershipType, bool showConfirmRepresentation = false)
        {
            var query = GetAll(true).Where(r => r.Confirmed).AsNoTracking();
            if (showConfirmRepresentation)
            {
                query = query.Where(r => r.Confirmed);
            }
            if (addressId.HasValue)
            {
                query = query.Where(x => x.AddressId == addressId.Value);
            }
            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }
            if (founderId.HasValue)
            {
                query = query.Where(x => x.FounderId == founderId.Value);
            }
            if (licenseType.HasValue)
            {
                query = query.Where(x => x.LicenseType == licenseType.Value);
            }
            if (ownershipType.HasValue)
            {
                query = query.Where(x => x.OwnershipType == ownershipType.Value);
            }


            return query;
        }
        /// <summary>
        /// Get Representation By City Id
        /// </summary>
        /// <param name="cityId">شناسه شهر</param>
        /// <returns></returns>
        public IQueryable<Representation> GetByCityId(int cityId)
        {
            return _context.Representation.AsNoTracking().Where(x => x.Address.CityId == cityId && x.ApplicationId == _applicationConfiguration.ApplicationId).Select(x => x).Distinct();

        }

        /// <summary>
        /// نمایندگی های مربوط به مرکز در هر شهر
        /// </summary>
        /// <param name="cityId"> شهر</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Representation> GetByCityIdAndCompanyId(int? cityId, int? companyId, bool readOnly = true)
        {
            var query = _context.Representation.Where(x => x.Confirmed &&
            x.ApplicationId == _applicationConfiguration.ApplicationId &&
            (!cityId.HasValue || x.Address.CityId == cityId.Value) &&
            (!companyId.HasValue || x.CompanyId == companyId.Value))
                .OrderBy(x => x.DisplayOrder);

            if (readOnly)
            {
                return query.AsNoTracking();
            }
            return query;
        }

        public Representation GetUserRepresentation(int userPrincipalId)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == userPrincipalId);
            if (additionalUserProp?.RepresentationId != null)
            {
                return Get(additionalUserProp.RepresentationId);
            }
            return null;
        }
        // TODO:Check Calc Price ***
        public IQueryable<Representation> GetCompanyRepresentationByFilter(
            int customerClubId,
            int applicationId,
            List<int> company,
            int? cityId,
            int? categoryId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup)
        {
            IQueryable<Representation> representations =
                DbSet.AsNoTracking().Where(x => x.ApplicationId == applicationId);
            if (cityId.HasValue)
            {
                representations = representations.Where(x => x.Address.CityId == cityId);
            }

            if (company != null && company.Count >= 1)
            {
                var companyId = company.FirstOrDefault();
                representations = representations.Where(x => x.CompanyId == companyId);
            }

            if (categoryId.HasValue)
            {
                var codePath = _context.Category.Where(x => x.Id == categoryId.Value)
                    .Select(x => x.CodePath).FirstOrDefault();
                representations = representations.Where(x => x.ProductSalesPlanning
                    .Where(psp => psp.Confirmed.Value && psp.Published)
                    .Any(psp => psp.Product.ProductCategory
                        .Any(t => t.Category.CodePath.StartsWith(codePath))));
            }

            representations = representations.Where(x =>
                    x.ProductSalesPlanning.AsQueryable().Any(
                        _productSalesPlanningRepository
                            .GetProductSalePlanningIsActiveFilter(
                                applicationId,
                                customerClubId,
                                categoryGroupId,
                                isEmploymentCategoryGroup))

                // x.ProductSalesPlanning.Any(psp =>
                //psp.Confirmed.Value &&
                //psp.SaleConfiguration.IsActive &&
                //psp.SaleConfiguration.SaleConfigurationAttribute
                //    .Any(s => s.CustomerClubId == customerClubId && s.IsActive) &&

                //(!categoryGroupId.HasValue ||
                //(!isEmploymentCategoryGroup && psp.Product.DefaultCategory.CategoryGroupId == categoryGroupId.Value) ||
                //(isEmploymentCategoryGroup && psp.Product.ProductType == (int)ProductTypeEnum.Employment)) 

                //psp.Published.Value &&
                //psp.Price.HasValue &&
                //psp.Price.Value > 0 &&
                //(!psp.EndDate.HasValue || psp.EndDate.Value >= todayInt) &&
                //psp.SaleConfiguration.IsActive &&
                //(!psp.SaleConfiguration.LicenseExpireDate.HasValue ||
                // psp.SaleConfiguration.LicenseExpireDate > todayInt) &&
                //psp.SaleConfiguration.ToDate > todayInt
                //)
            );


            return representations;

        }

        public string GetTitle(int id)
        {
            return DbSet.AsNoTracking().Where(x => x.Id == id).Select(x => x.Title).FirstOrDefault();
        }
        /// <summary>
        /// دریافت تنظیمات کلاس مجازی بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">برنامه ریزی دوره</param>
        /// <returns></returns>
        public Task<Representation> GetByProductSalePlanningASync(int productSalePlanningId)
        {
            return DbSet
                .FirstOrDefaultAsync(x => x.ProductSalesPlanning
                    .Any(pr => pr.Id == productSalePlanningId));
        }

        public IQueryable<Representation> GetByCompanyId(int companyId, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.CompanyId == companyId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<Representation> GetRepresentationByParams(List<int> companyIds)
        {
          return DbSet.Where(x=> companyIds.Contains(x.CompanyId));
        }

        public IQueryable<Representation> GetCompanyDetails(int id)
        {
            return DbSet.Where(x => x.Id == id); ;
        }

        public IQueryable<Representation> GetByProductSalesPlanningCompany(int productSalesPlanningId)
        {
            return DbSet.Where(x =>
                x.Company.Product.Any(p => p.ProductSalesPlanning.Any(psp => psp.Id == productSalesPlanningId))).AsNoTracking();
        }
    }
}