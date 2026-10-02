using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorItemRepository : GenericApplicationBasedRepository<FactorItem, int>, IFactorItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public FactorItemRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<FactorItem> GetByCustomFilter(int? factorId, int? orderItemId)
        {
            var query = GetAll(true);
            if (factorId.HasValue)
            {
                query = query.Where(x => x.FactorId == factorId.Value);
            }

            if (orderItemId.HasValue)
            {
                query = query.Where(x => x.OrderItemId == orderItemId.Value);
            }

            return query;
        }

        public IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemStatistic(int factorId)
        {
            return _context.FactorItemsPurchasedFromSuppliersView.Where(x => x.FactorId == factorId)
                .GroupBy(x => new { x.ProductSalesPlanningId, x.Title }).Select(
                    x => new FactorItemsPurchasedFromSuppliersView
                    {
                        ProductSalesPlanningId = x.Key.ProductSalesPlanningId,
                        Title = x.Key.Title,
                        Quantity = x.Sum(f => f.Quantity),
                        Price = x.Sum(fi => fi.Price),
                        DiscountWithProfitPrice = x.Sum(fi => fi.DiscountWithProfitPrice),
                        DiscountedPriceSumTaxAmount = x.Sum(fi => fi.DiscountedPriceSumTaxAmount),
                        TaxAmount = x.Sum(fi => fi.TaxAmount),
                        BasePrice = x.Min(fi => fi.BasePrice)
                    });
        }

        public IQueryable<FactorItem> GetFactorItems(int factorId)
        {
            return DbSet.Where(x => x.FactorId == factorId);
        }

        public IQueryable<UnFactoredOrderItemView> GetCurrentUserUnFactoredOrderItems(
            int?[] productSalesPlanningIds)
        {
            // دریافت اطلاعات کاربر لاگین شده
            var additionalUserProp = GetCurrentUserCompanyAndRepresentation();
            IQueryable<UnFactoredOrderItemView> query = null;
            if (additionalUserProp.companyId != null)
            {
                //query = _context.UnFactoredOrderItemView
                //   .Where(x => x.ApplicationId == _userPrincipal.ApplicationId &&
                //               x.CompanyId == additionalUserProp.companyId &&
                //               (!additionalUserProp.RepresentationId.HasValue ||
                //               x.RepresentationId == additionalUserProp.RepresentationId));
                query = GetUnFactoredOrderItems(additionalUserProp.companyId.Value, additionalUserProp.RepresentationId);
                if (productSalesPlanningIds != null && productSalesPlanningIds.Length > 0)
                {
                    query = query.Where(x => productSalesPlanningIds.Contains(x.ProductSalePlanningId));
                }
            }
            return query;
        }

        private (int? companyId, int? RepresentationId) GetCurrentUserCompanyAndRepresentation()
        {
            var res = _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => new { x.CompanyId, x.RepresentationId })
                .AsNoTracking().FirstOrDefault();
            return (res?.CompanyId, res?.RepresentationId);
        }

        public IQueryable<UnFactoredOrderItemView> GetUnFactoredOrderItems(int companyId, int? representationId)
        {
            return _context.UnFactoredOrderItemView
                .Where(x =>
                    x.ApplicationId == _userPrincipal.ApplicationId &&
                x.CompanyId == companyId &&
                (!representationId.HasValue ||
                 x.RepresentationId == representationId.Value));
        }

        public void SetFactorGroupIdForDeletedFactoredOrderItems(int factorGroupId, int applicationId)
        {
            var query = _context.FactorItem.Where(x =>
                  x.OrderItem.Deleted &&
                  !x.Factor.Deleted &&
                  !x.DeletedOrderItemGroupId.HasValue &&
                  x.Factor.FactorGroupId.HasValue &&
                  x.ApplicationId == applicationId
                 );
            query.Update(x => new FactorItem { DeletedOrderItemGroupId = factorGroupId });
        }

        public IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemListStatistic(int factorId)
        {
            return _context.FactorItemsPurchasedFromSuppliersView.Where(x => x.FactorId == factorId);
        }

        public IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatView> GetFactorGroupFactorItems(int factorGroupId)
        {
            return _context.FactorItemsSoldToTheSataAndVezaratAndHekmatView.Where(x => x.FactorGroupId == factorGroupId);
        }
        /// <summary>
        ///  بررسی می کند که آیا سایر کسورات در فاکتور تامین کننده لحاظ شده است یا خیر؟
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public async Task<bool> CheckOtherDeductionsAreIncludedInTheSupplierInvoice(int factorId)
        {
            return await DbSet.AnyAsync(fi => fi.FactorId == factorId && fi.Factor.SaleConfiguration.IsOtherDeductionsIncludeInSupplierInvoice);
        }

        public IEnumerable<InvalidFactorItemDomainModel> ChecksIsInvalidFactorItemAsync(int factorId)
        {
            return _context.InvalidFactorItemDomainModels.FromSqlRaw($" [shp].[ChecksIsInvalidFactorItem] {factorId}");
        }

        /// <summary>
        /// تنظیمات فروش مر بوط به آیتم های سفارش مرکز و نمایندگی کاربر جاری را بررسی می کند
        /// در صورتی که فیلد کانورت بر اساس دسته بندی مالیاتی انتخاب نشده باشد
        /// نام کالا  -  کدکالا - واحدسنجش در دسته بندی محصولات مقدار داشته باشند
        /// اگر این مقادیر مقدار نداشته باشند باید از صدور فاکتور جلوگیری شود
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ValidateCurrentUserUnFactoredOrderItemsMainCategoryInfo()
        {
            var query = GetCurrentUserUnFactoredOrderItems(null);
            return !await query.AnyAsync(x => !x.IsGroupedByTaxCategory &&
                                               (string.IsNullOrEmpty(x.MainCategoryTitle) ||
                                                 string.IsNullOrEmpty(x.MainCategoryCode) ||
                                                 string.IsNullOrEmpty(x.UnitOfMeasurement))
                                   );

        }


        /// <summary>
        /// دریافت خلاصه پرداخت آیتم های فاکتور برای اساس درصد
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public IQueryable<ItemPaymentSummaryDto> GetFactorItemPaymentSummary(int factorId)
        {
            return _context.Database
                .SqlQueryRaw<ItemPaymentSummaryDto>(@"
            SELECT 
                CAST(ROW_NUMBER() OVER (ORDER BY res.PaymentMethodType, res.InstallmentCount) AS INT) as Id,
                res.PaymentMethodType,
                res.InstallmentCount,
                res.price as Price,
                res.FactorItemPaymentTotal,
                ROUND((res.price/res.FactorItemPaymentTotal)*100, 0) as TotalPricePercentage
            FROM (
                SELECT 
                    base.GetBaseDataTitle(p.PaymentMethodType) as PaymentMethodType,
                    ISNULL(p.InstallmentCount, 1) as InstallmentCount,
                    SUM(oip.Price) as price,
                    MAX(f.Id) as FactorId,
                    MAX(SFIP.FactorItemPaymentTotal) as FactorItemPaymentTotal
                FROM shp.Factor f
                INNER JOIN shp.FactorItem AS fi ON fi.FactorId = f.Id
                INNER JOIN shp.OrderItem AS oi ON oi.Id = fi.OrderItemId
                INNER JOIN shp.Payment AS p ON oi.OrderId = p.OrderId
                INNER JOIN shp.OrderItemPayment AS oip ON oip.OrderItemId = oi.Id AND oip.PaymentId = p.Id
                INNER JOIN (
                    SELECT SUM(oip2.Price) as FactorItemPaymentTotal, fi2.FactorId
                    FROM shp.OrderItemPayment AS oip2
                    INNER JOIN shp.FactorItem AS fi2 ON fi2.OrderItemId = oip2.OrderItemId
                    GROUP BY fi2.FactorId
                ) SFIP ON SFIP.FactorId = f.Id
                WHERE f.id = {0}
                GROUP BY p.InstallmentCount, p.PaymentMethodType
            ) res", factorId);
        }

        /// <summary>
        ///  دریافت لیست آیتم های یک فاکتور براساس نوع عضویت و شناسه فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        public IQueryable<FactorItem> GetFactorItemsByMembershipType(int factorId, int? membershipId)
        {
            return DbSet.Where(x => membershipId.HasValue && membershipId != 0 ?
                x.Factor.Id == factorId && x.OrderItem.Order.MembershipType == membershipId :
                x.Factor.Id == factorId);
        }

        public Task<int> UpdateAllFactorItemInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAndPriceAsync(int factorId)
        {
            return DbSet.Where(factorItem => factorItem.FactorId == factorId)
                .Select(x => x.OrderItem)
                .Select(oi => new
                {
                    OrderItem = oi,
                    InsurancePercentage = oi.SaleConfiguration.InsurancePercentage,
                    InsurancePrice = oi.Netprice * (oi.SaleConfiguration.InsurancePercentage / 100)
                }).Where(x =>
                    x.OrderItem.InsurancePercentage != x.InsurancePercentage)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.OrderItem.InsurancePercentage,
                        u => u.InsurancePercentage)
                    .SetProperty(p => p.OrderItem.InsurancePrice,
                        u => u.InsurancePrice));
        }

        public Task<int> UpdateAllFactorItemOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(int factorId)
        {

            return DbSet.Where(x => x.FactorId == factorId)
                .Select(x => x.OrderItem)
                .Select(oi => new
                {
                    OrderItem = oi,

                    OtherDeductionsPercentage = oi.SaleConfiguration.SaleConfigurationAttribute
                        .Where(sca => sca.SaleConfigurationId == oi.SaleConfigurationId &&
                                      sca.CustomerClubId == oi.Order.CustomerClubId)
                        .Select(sca => sca.OtherDeductionsPercentage).FirstOrDefault(),

                    OtherDeductionsPrice = oi.SaleConfiguration.SaleConfigurationAttribute
                        .Where(sca => sca.SaleConfigurationId == oi.SaleConfigurationId &&
                                      sca.CustomerClubId == oi.Order.CustomerClubId)
                        .Select(sca =>
                            oi.Netprice * (sca.OtherDeductionsPercentage / 100))
                        .FirstOrDefault(),
                }).Where(x =>
                    (x.OrderItem.IsOtherDeductionsIncludeInSupplierInvoice.HasValue &&
                    !x.OrderItem.IsOtherDeductionsIncludeInSupplierInvoice.Value) &&
                    x.OrderItem.OtherDeductionsPercentage != x.OtherDeductionsPercentage)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.OrderItem.OtherDeductionsPercentage, u => u.OtherDeductionsPercentage)
                    .SetProperty(p => p.OrderItem.OtherDeductionsPrice, u => u.OtherDeductionsPrice));


        }

        public Task<List<FactorItemCustomInformationDto>> GetFactorItemsCustomInformationAsync(int factorId)
        {
            return GetAll(true)
                .Where(x => x.FactorId == factorId)
                .Select(x => new FactorItemCustomInformationDto
                {
                    FactorItemId = x.Id,
                    OrderItemId = x.OrderItemId,
                    ProductId = x.OrderItem.ProductSalesPlanning.ProductId

                }).ToListAsync();
        }


        public Task UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryAsync(int factorId)
        {
            return GetAll(true)
                .Where(factorItem => factorItem.FactorId == factorId)
                .Select(factorItem => new
                {
                    FactorItem = factorItem,
                    IsGroupByTaxCategory = factorItem.OrderItem.SaleConfiguration.IsGroupedByTaxCategory,
                    TaxCategoryIsIncludeInsurance = factorItem.OrderItem.TaxCategory.IsIncludeInsurance
                })
                .Where(s =>
                    s.FactorItem.IsIncludeInsurace !=
                    (s.IsGroupByTaxCategory && s.TaxCategoryIsIncludeInsurance))
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.FactorItem.IsIncludeInsurace,
                        b => (b.IsGroupByTaxCategory && b.TaxCategoryIsIncludeInsurance)));

        }

        public Task UpdateIsIncludeInsuranceFromSaleConfigurationAllProductsIncludeInsuranceAsync(int factorId)
        {
            return GetAll(true)
                .Where(factorItem => factorItem.FactorId == factorId &&
                                     factorItem.OrderItem.SaleConfiguration
                                         .AllProductsIncludeInsurance &&
                                     factorItem.IsIncludeInsurace != factorItem.OrderItem.SaleConfiguration
                                         .AllProductsIncludeInsurance)
                .Select(factorItem => new
                {
                    FactorItem = factorItem,
                    AllProductsIncludeInsurance = factorItem.OrderItem.SaleConfiguration.AllProductsIncludeInsurance
                })
                .ExecuteUpdateAsync(u
                    => u.SetProperty(s => s.FactorItem.IsIncludeInsurace, b => b.AllProductsIncludeInsurance));
        }

        public Task UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryColumnAsync(int factorId)
        {
            return GetAll(true)
                .Where(factorItem => factorItem.FactorId == factorId)
                .Select(factorItem => new
                {
                    FactorItem = factorItem,
                    IsGroupByTaxCategory = factorItem.OrderItem.SaleConfiguration.IsGroupedByTaxCategory,
                    TaxCategoryIsIncludeInsurance = factorItem.OrderItem.TaxCategory.IsIncludeInsurance
                })
                .Where(s =>
                    s.IsGroupByTaxCategory && s.TaxCategoryIsIncludeInsurance &&
                    !s.FactorItem.IsIncludeInsurace)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.FactorItem.IsIncludeInsurace,
                        true));

        }
    }
}