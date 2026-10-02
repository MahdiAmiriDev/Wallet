using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class SaleConfigurationRepository : GenericApplicationBasedRepository<SaleConfiguration, int>,
        ISaleConfigurationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public SaleConfigurationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<SaleConfiguration> GetByCustomFilter(int? representationId, int? companyId)
        {
            var query = GetAll(true);
            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            return query.AsNoTracking();
        }

        public List<SaleConfiguration> GetRepresentationSaleConfigurationByShoppingCartCode(Guid shoppingCartCode)
        {
            var representations = _context.ShoppingCartItem.Where(c => c.Code == shoppingCartCode).Select(c => new { c.RepresentationId, c.ProductSalesPlanningId });
            return DbSet.Where(x => x.IsActive && !x.Deleted && x.RepresentationId.HasValue && representations.Select(r => r.RepresentationId).Contains(x.RepresentationId.Value)).AsNoTracking().ToList();
        }
        public List<SaleConfiguration> GetCompanySaleConfigurationByShoppingCartCode(Guid shoppingCartCode)
        {
            var companies = _context.ShoppingCartItem.Where(c => c.Code == shoppingCartCode).Select(c => c.ProductSalesPlanning.Product.CompanyId);
            return DbSet.Where(x => x.IsActive && !x.Deleted && !x.RepresentationId.HasValue && companies.Contains(x.CompanyId)).AsNoTracking().ToList();
        }



        public async Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByOrderIdAsync(
            int orderId,
            int customerClubId)
        {
            var query = _context.OrderItem.Where(x => x.OrderId == orderId)
                .Select(x => x.SaleConfiguration);
            return (await MapToSaleConfigurationAttributeDtoAsync(query, customerClubId)).ToList();
        }

        public SaleConfiguration GetUserActiveSaleConfiguration(int userId)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
            if (additionalUserProp?.CompanyId != null)
            {
                return DbSet.Where(x => x.CompanyId == additionalUserProp.CompanyId && x.IsActive
                                                                                                      && ((additionalUserProp.RepresentationId.HasValue &&
                                                                                                           x.RepresentationId == additionalUserProp.RepresentationId.Value)
                                                                                                          ||
                                                                                                          (!additionalUserProp.RepresentationId.HasValue && !x.RepresentationId.HasValue
                                                                                                          ))).Include(x => x.YashilScheduler).FirstOrDefault();
            }

            return null;
        }

        public SaleConfiguration GetLastSaleConfigurationByDate(int userId)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
            if (additionalUserProp?.CompanyId != null)
            {
                return DbSet.Where(x => x.CompanyId == additionalUserProp.CompanyId
                                        && ((additionalUserProp.RepresentationId.HasValue && x.RepresentationId == additionalUserProp.RepresentationId.Value)
                                            || (!additionalUserProp.RepresentationId.HasValue && !x.RepresentationId.HasValue))).AsNoTracking()
                    .OrderByDescending(x => x.CreationDate)
                    .Include(x => x.YashilScheduler).FirstOrDefault();
            }

            return null;
        }

        public async Task<List<SaleConfigurationAttributeDto>>
            GetSaleConfigurationAttributeByShoppingCartCodeAsync(
                int customerClubId, Guid shoppingCartCode, int applicationId)
        {
            var companyRepresentations =
                _context.ShoppingCartItem
                    .Where(c => c.Code == shoppingCartCode && c.ApplicationId == applicationId).Select(x =>
                        new CompanyRepresentation
                        {
                            CompanyId = x.ProductSalesPlanning.Product.CompanyId,
                            RepresentationId = x.RepresentationId
                        }).AsNoTracking().Distinct().ToList();

            return await GetSaleConfigurationAttributeByCompanyAndRepresentationsAsync(customerClubId, companyRepresentations);
        }

        private async Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByCompanyAndRepresentationsAsync(
            int customerClubId,
            List<CompanyRepresentation> companyAndRepresentations)
        {
            var saleConfigurationAttributeDtoList = new List<SaleConfigurationAttributeDto>();
            foreach (var item in companyAndRepresentations)
            {
                var representationSaleConfigurationQuery = DbSet.AsNoTracking()
                    .Where(x => x.IsActive &&
                                !x.Deleted &&
                                x.RepresentationId.HasValue &&
                                x.RepresentationId == item.RepresentationId);

                var representationSaleConfiguration =
                    (await MapToSaleConfigurationAttributeDtoAsync(representationSaleConfigurationQuery, customerClubId)).FirstOrDefault();


                if (representationSaleConfiguration != null)
                {
                    saleConfigurationAttributeDtoList.Add(representationSaleConfiguration);
                }
                else
                {
                    var companySaleConfigurationQuery = DbSet.AsNoTracking()
                        .Where(x => x.IsActive && !x.Deleted && !x.RepresentationId.HasValue &&
                                    x.CompanyId == item.CompanyId);
                    var companySaleConfiguration =
                        (await MapToSaleConfigurationAttributeDtoAsync(companySaleConfigurationQuery, customerClubId)).FirstOrDefault();

                    ;
                    if (companySaleConfiguration != null)
                    {

                        saleConfigurationAttributeDtoList.Add(companySaleConfiguration);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            return saleConfigurationAttributeDtoList;
        }

        private async Task<IQueryable<SaleConfigurationAttributeDto>>
            MapToSaleConfigurationAttributeDtoAsync(
                IQueryable<SaleConfiguration> query, int customerClubId)
        {
            //آیا این باشگاه مشتری در گاه اقساطی حکمت را دارد یا نه؟
            var hasInstallmentPaymentMethodType =
                await _context.CustomerClub.AnyAsync(x => x.Id == customerClubId &&
                x.CustomerClubPaymentMethodType
                    .Any(p =>
                    p.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                    || p.PaymentMethodType == (int)PaymentMethodTypesEnum.Wallet
                    || p.PaymentMethodType == (int)PaymentMethodTypesEnum.Samin));

            return query.Select(x =>
                new SaleConfigurationAttributeDto
                {
                    SaleConfigurationId = x.Id,
                    SaleConfigurationAttributeId = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).Id,
                    CustomerClubId = customerClubId,
                    InstallmentExpression = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive)
                        .InstallmentExpression,
                    AllowedToPayInInstallment = hasInstallmentPaymentMethodType && x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).SaleType != (int)SaleTypesEnum.CashPayment,
                    SaleType = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).SaleType,
                    InstallmentCount = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive)
                        .InstallmentCount,
                    InsurancePercentage = x.InsurancePercentage,
                    WagePercentage = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).WagePercentage,
                    OtherDeductionsPercentage = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).OtherDeductionsPercentage,
                    ProfitPercentage = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive).ProfitPercentage,
                    RepresentationId = x.RepresentationId,
                    CompanyId = x.CompanyId,
                    ExpressionBasedInstallmentCount = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive)
                        .ExpressionBasedInstallmentCount,
                    DiscountPercentage = x.SaleConfigurationAttribute
                        .FirstOrDefault(c => c.CustomerClubId == customerClubId && c.IsActive)
                        .DiscountPercentage
                });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="saleConfigurationId"></param>
        /// <returns></returns>
        public async Task DeactivateOldSaleConfigurationsAsync(int companyId, int? representationId, int currentSaleConfigurationId)
        {
            // get list of saleConfiguration with these condition 
            // check companyId
            //check activate
            // check representation has value if yes check representationId
            var saleConfigurations = DbSet.Where(x => x.Id != currentSaleConfigurationId &&
            x.CompanyId == companyId &&
                                                      ((representationId.HasValue && x.RepresentationId == representationId.Value)
                                                      ||
                                                      (!representationId.HasValue && !x.RepresentationId.HasValue)) && x.IsActive);

            await saleConfigurations.UpdateAsync(x => new SaleConfiguration { IsActive = false });
        }

        public async Task<SaleConfiguration> GetSaleConfigurationAsync(int productSalesPlanningId, bool readOnly = false)
        {
            var query = _context.ProductSalesPlanning.Where(x =>
                x.Id == productSalesPlanningId &&
                x.SaleConfiguration.IsActive &&
                !x.SaleConfiguration.Deleted);
            if (readOnly)
            {
                query = query.AsNoTracking();
            }

            return await query
                .Select(x => x.SaleConfiguration)
                .FirstOrDefaultAsync();



            //var saleConfiguration = await query.FirstOrDefaultAsync(x => (x.RepresentationId.HasValue && x.Representation.ProductSalesPlanning.Any(ps => ps.Id == productSalesPlanningId)));
            //if (saleConfiguration == null)
            //{
            //    saleConfiguration = await query.FirstOrDefaultAsync(x => (!x.RepresentationId.HasValue && x.Company.Product.Any(p => p.ProductSalesPlanning.Any(ps => ps.Id == productSalesPlanningId))));
            //}
            //  return saleConfiguration;
        }



        public async Task<SaleConfigurationAttributeDto>
            GetSaleConfigurationAttributeAsync(
            int customerClubId,
            int productSalesPlanningId)
        {
            var query = _context.ProductSalesPlanning.Where(x =>
                x.Id == productSalesPlanningId &&
                !x.SaleConfiguration.Deleted);

            query = query.AsNoTracking();


            var saleConfiguration = query
                .Select(x => x.SaleConfiguration);

            return (await MapToSaleConfigurationAttributeDtoAsync(saleConfiguration, customerClubId)).FirstOrDefault();




            //var saleConfiguration = await query.FirstOrDefaultAsync(x => (x.RepresentationId.HasValue && x.Representation.ProductSalesPlanning.Any(ps => ps.Id == productSalesPlanningId)));
            //if (saleConfiguration == null)
            //{
            //    saleConfiguration = await query.FirstOrDefaultAsync(x => (!x.RepresentationId.HasValue && x.Company.Product.Any(p => p.ProductSalesPlanning.Any(ps => ps.Id == productSalesPlanningId))));
            //}
            //  return saleConfiguration;
        }




        /// <summary>
        /// تنظیم فروش فعال را برمیگرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public Task<SaleConfiguration> GetActiveSaleConfigurationAsync(int companyId, int? representationId)
        {
            return DbSet.FirstOrDefaultAsync(x =>
                x.CompanyId == companyId &&
                x.IsActive &&
                (
                    (representationId.HasValue &&
                     (x.RepresentationId == representationId.Value || !x.RepresentationId.HasValue)
                    )
                 ||
                 (!representationId.HasValue && !x.RepresentationId.HasValue)));
        }

        public async Task<SaleConfigurationAttributeDto>
            GetSaleConfigurationAttributeByFilterAsync(
                int customerClubId, int companyId, int? representationId)
        {
            var saleConfiguration = DbSet.Where(x =>
                x.CompanyId == companyId &&
                x.IsActive &&
                (
                    (representationId.HasValue &&
                     (x.RepresentationId == representationId.Value || !x.RepresentationId.HasValue)
                    )
                    ||
                    (!representationId.HasValue && !x.RepresentationId.HasValue)));
            return (await MapToSaleConfigurationAttributeDtoAsync(saleConfiguration, customerClubId)).FirstOrDefault();
        }

        public async Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeBySaleConfigurationIdAsync
           (int customerClubId, int saleConfigurationId)
        {
            var saleConfiguration = DbSet.AsNoTracking()
                        .Where(x => x.Id == saleConfigurationId && x.IsActive &&
                                    !x.Deleted);
            return (await MapToSaleConfigurationAttributeDtoAsync(saleConfiguration, customerClubId)).FirstOrDefault();
        }

        public IQueryable<SaleConfiguration> FindById(int saleConfigurationId, bool readOnly = true)
        {
            var query = DbSet.Where(x => x.Id == saleConfigurationId);
            return readOnly ? query.AsNoTracking() : query;
        }
    }




    class CompanyRepresentation
    {
        public int CompanyId { get; set; }
        public int RepresentationId { get; set; }
    }
}