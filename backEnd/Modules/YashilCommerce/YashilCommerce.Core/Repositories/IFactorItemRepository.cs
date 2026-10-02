using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorItemRepository : IGenericRepository<FactorItem, int>
    {

        IQueryable<FactorItem> GetByCustomFilter(int? factorId, int? orderItemId);
        IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemStatistic(int factorId);
        /// <summary>
        /// دریافت لیست آیتم های فاکتور یک فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorItem> GetFactorItems(int factorId);

        /// <summary>
        /// دریافت لیست آیتمهای سفارش یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatView> GetFactorGroupFactorItems(int factorGroupId);

        /// <summary>
        /// آیتم های فاکتور نشده بر اساس مرکز و نمایندگس کاربر لاگین شده 
        /// </summary>
        /// <returns></returns>
        IQueryable<UnFactoredOrderItemView> GetCurrentUserUnFactoredOrderItems(int?[] productSalesPlanningIds);
        /// <summary>
        /// دریافت لیست آیتم های یک فاکتور از
        /// view factorItemDetailStatistic
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemListStatistic(int factorId);

        /// <summary>
        /// برای دریافت لیست آیتمهای فاکتور نشده براساس فیلتر مرکز و نمایندگی
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        IQueryable<UnFactoredOrderItemView> GetUnFactoredOrderItems(int companyId, int? representationId);

        void SetFactorGroupIdForDeletedFactoredOrderItems(int factorGroupId, int applicationId);
        /// <summary>
        ///  بررسی می کند که آیا سایر کسورات در فاکتور تامین کننده لحاظ شده است یا خیر؟
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<bool> CheckOtherDeductionsAreIncludedInTheSupplierInvoice(int factorId);
        /// <summary>
        /// بررسی چک کردن آیتم های غیر مجاز در فاکتور
        /// این متد sp
        /// ChecksIsInvalidFactorItemAsync
        /// را صدا میزند
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IEnumerable<InvalidFactorItemDomainModel> ChecksIsInvalidFactorItemAsync(int factorId);
        /// <summary>
        /// تنظیمات فروش مر بوط به آیتم های سفارش مرکز و نمایندگی کاربر جاری را بررسی می کند
        /// در صورتی که فیلد کانورت بر اساس دسته بندی مالیاتی انتخاب نشده باشد
        /// نام کالا  -  کدکالا - واحدسنجش در دسته بندی محصولات مقدار داشته باشند
        /// اگر این مقادیر مقدار نداشته باشند باید از صدور فاکتور جلوگیری شود
        /// </summary>
        /// <returns></returns>
        Task<bool> ValidateCurrentUserUnFactoredOrderItemsMainCategoryInfo();

        /// <summary>
        /// دریافت خلاصه پرداخت آیتم های فاکتور برای اساس درصد
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<ItemPaymentSummaryDto> GetFactorItemPaymentSummary(int factorId);

        /// <summary>
        ///  دریافت لیست آیتم های یک فاکتور براساس نوع عضویت و شناسه فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        IQueryable<FactorItem> GetFactorItemsByMembershipType(int factorId, int? membershipId);

        /// <summary>
        /// بروزرسانی مقدار
        /// insurancePercentage
        /// insurancePrice
        /// مربوط به فاکتور ایتم ها بر اساس
        /// مقدار
        /// insurancePercentage
        /// در تنظیم فروش مربوط به هر ایتم سفارش
        /// نکته مقدار
        /// insurancePrice
        /// بر اساس محاسباتی که طبق فرمول در ادامه وجود دارد بروز می شود
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<int> UpdateAllFactorItemInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAndPriceAsync(int factorId);

        /// <summary>
        /// بروزرسانی مقدار
        /// OtherDeductionsPercentage
        /// OtherDeductionsPrice
        /// مربوط به فاکتور ایتم ها بر اساس
        /// مقدار
        /// OtherDeductionsPercentage
        /// در تنظیم فروش مربوط به هر ایتم سفارش
        /// نکته مقدار
        /// OtherDeductionsPrice
        /// بر اساس محاسباتی که طبق فرمول در ادامه وجود دارد بروز می شود
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public Task<int> UpdateAllFactorItemOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(int factorId);

        /// <summary>
        /// دریافت اطلاعات کاستوم شده از ایتم های فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<List<FactorItemCustomInformationDto>> GetFactorItemsCustomInformationAsync(int factorId);

        /// <summary>
        /// بروزرسانی شامل بیمه بودن ایتم های فاکتور بر اساس
        /// گروه بندی مالیاتی
        /// که در تنظیم فروش ست شده است
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task UpdateIsIncludeInsuranceFromSaleConfigurationIsGroupByTaxCategoryColumnAsync(int factorId);

        /// <summary>
        /// بروزرسانی مقدار شامل بیمه بودن ایتم های سفارش بر اساس این که
        /// مقدار آن با مقدار تنظیم شده در تنظیم فروش برای فیلد
        /// AppProductsIncludeInsurance
        /// متفاوت باشد.
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task UpdateIsIncludeInsuranceFromSaleConfigurationAllProductsIncludeInsuranceAsync(int factorId);
    }
}
