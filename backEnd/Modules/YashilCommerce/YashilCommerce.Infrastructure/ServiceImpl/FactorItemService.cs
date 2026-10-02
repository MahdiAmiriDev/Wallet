using System;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;
using Yashil.Core.CustomDomainModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorItemService : GenericService<FactorItem, int>, IFactorItemService
    {
        private readonly IFactorItemRepository _factorItemRepository;
        private readonly YashilAppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IOrderItemService _orderItemService;


        public FactorItemService(IUnitOfWork unitOfWork, IFactorItemRepository factorItemRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, YashilAppDbContext context,
            IOrderItemService orderItemService, ICategoryService categoryService)
            : base(unitOfWork, factorItemRepository, userPrincipal, eventPublisher)
        {
            _factorItemRepository = factorItemRepository;
            _context = context;
            _orderItemService = orderItemService;
            _categoryService = categoryService;
        }

        public IQueryable<FactorItem> GetByCustomFilter(int? factorId, int? orderItemId)
        {
            return _factorItemRepository.GetByCustomFilter(factorId, orderItemId);
        }

        public IQueryable<UnFactoredOrderItemView> GetCurrentUserUnFactoredOrderItems(int?[] productSalesPlanningIds)
        {
            return _factorItemRepository.GetCurrentUserUnFactoredOrderItems(productSalesPlanningIds);
        }

        public IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemListStatistic(int factorId)
        {
            return _factorItemRepository.GetFactorItemListStatistic(factorId);
        }

        public IQueryable<UnFactoredOrderItemView> GetUnFactoredOrderItems(int companyId, int? representationId)
        {

            return _factorItemRepository.GetUnFactoredOrderItems(companyId, representationId);
        }

        public void SetFactorGroupIdForDeletedFactoredOrderItems(int factorGroupId, int applicationId)
        {
            _factorItemRepository.SetFactorGroupIdForDeletedFactoredOrderItems(factorGroupId, applicationId);
        }


        public IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemStatistic(int factorId)
        {
            return _factorItemRepository.GetFactorItemStatistic(factorId);
        }

        public IQueryable<FactorItem> GetFactorItems(int factorId)
        {
            return _factorItemRepository.GetFactorItems(factorId);
        }

        public IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatView> GetFactorGroupFactorItems(int factorGroupId)
        {
            return _factorItemRepository.GetFactorGroupFactorItems(factorGroupId);
        }
        /// <summary>
        ///  بررسی می کند که آیا سایر کسورات در فاکتور تامین کننده لحاظ شده است یا خیر؟
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public async Task<bool> CheckOtherDeductionsAreIncludedInTheSupplierInvoice(int factorId)
        {
            return await _factorItemRepository.CheckOtherDeductionsAreIncludedInTheSupplierInvoice(factorId);
        }

        public IEnumerable<InvalidFactorItemDomainModel> GetInvalidFactorItem(int factorId)
        {
            return _factorItemRepository.ChecksIsInvalidFactorItemAsync(factorId);
        }

        public Task<bool> CheckOrderItemHasBeenFactored(int orderItemId)
        {
            return GetByCustomFilter(null, orderItemId)
                .AnyAsync();
        }

        /// <summary>
        /// تنظیمات فروش مر بوط به آیتم های سفارش مرکز و نمایندگی کاربر جاری را بررسی می کند
        /// در صورتی که فیلد کانورت بر اساس دسته بندی مالیاتی انتخاب نشده باشد
        /// نام کالا  -  کدکالا - واحدسنجش در دسته بندی محصولات مقدار داشته باشند
        /// اگر این مقادیر مقدار نداشته باشند باید از صدور فاکتور جلوگیری شود
        /// </summary>
        /// <returns></returns>
        public Task<bool> ValidateCurrentUserUnFactoredOrderItemsMainCategoryInfo()
        {
            return _factorItemRepository.ValidateCurrentUserUnFactoredOrderItemsMainCategoryInfo();
        }


        /// <summary>
        /// دریافت خلاصه پرداخت آیتم های فاکتور برای اساس درصد
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public IQueryable<ItemPaymentSummaryDto> GetFactorItemPaymentSummary(int factorId)
        {
            return _factorItemRepository.GetFactorItemPaymentSummary(factorId);
        }

        /// <summary>
        ///  دریافت لیست آیتم های یک فاکتور براساس نوع عضویت و شناسه فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        public IQueryable<FactorItem> GetFactorItemsByMembershipType(int factorId, int? membershipId)
        {
            return _factorItemRepository.GetFactorItemsByMembershipType(factorId, membershipId);
        }


        public Task<int> UpdateAllFactorItemInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAndPriceAsync(int factorId)
        {

            return _factorItemRepository
               .UpdateAllFactorItemInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAndPriceAsync(factorId);
        }

        public Task<int> UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(int factorId)
        {
            return _factorItemRepository
                .UpdateAllFactorItemOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(factorId);
        }

        public async Task UpdateFactorItemIsIncludeInsuranceFromProductCategoryAsync(int factorId)
        {
            await UpdateIsIncludeInsuranceFromSaleConfigurationAllProductsIncludeInsuranceAsync(factorId);

            await UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryColumnAsync(factorId);

            var factorItemsCustomInformation = await GetFactorItemsCustomInformationAsync(factorId);

            foreach (var factorItemInfo in factorItemsCustomInformation.DistinctBy(x => x.ProductId))
            {
                var codePath = await _categoryService.GetProductCategoryCodePathAsync(factorItemInfo.ProductId);

                var categoryTreeList = await _categoryService.GetCategoryTreeByCodePath(codePath)
                    .ToListAsync();

                var isAnyCategoryIncludeInsurance = categoryTreeList.Any(x => x.IsIncludeInsurance);

                if (isAnyCategoryIncludeInsurance)
                    await GetAll(true)
                        .Where(fi => fi.Id == factorItemInfo.FactorItemId &&
                                     !fi.IsIncludeInsurace)
                        .ExecuteUpdateAsync(x =>
                            x.SetProperty(u => u.IsIncludeInsurace, true));
            }
        }

        /// <summary>
        /// بروزرسانی مقدار شامل بیمه بودن ایتم های سفارش بر اساس این که
        /// مقدار آن با مقدار تنظیم شده در تنظیم فروش برای فیلد
        /// AppProductsIncludeInsurance
        /// متفاوت باشد.
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public Task UpdateIsIncludeInsuranceFromSaleConfigurationAllProductsIncludeInsuranceAsync(int factorId)
        {
            return _factorItemRepository
                .UpdateIsIncludeInsuranceFromSaleConfigurationAllProductsIncludeInsuranceAsync(factorId);
        }

        /// <summary>
        /// بروزرسانی شامل بیمه بودن ایتم های فاکتور بر اساس
        /// گروه بندی مالیاتی
        /// که در تنظیم فروش ست شده است
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public Task UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryColumnAsync(int factorId)
        {
            return _factorItemRepository
                .UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryColumnAsync(factorId);

        }

        /// <summary>
        /// دریافت اطلاعات کاستوم شده از ایتم های فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public Task<List<FactorItemCustomInformationDto>> GetFactorItemsCustomInformationAsync(int factorId)
        {
            return _factorItemRepository.GetFactorItemsCustomInformationAsync(factorId);
        }
    }
}