using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IFactorItemService : IGenericService<FactorItem, int>
    {
        IQueryable<FactorItem> GetByCustomFilter(int? factorId, int? orderItemId);

        /// <summary>
        /// گزارش خلاصه وضعیت آیتم های فاکتور بر اساس کد فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemStatistic(int factorId);

        /// <summary>
        /// دریافت لیست آیتم های فاکتور یک فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        IQueryable<FactorItem> GetFactorItems(int factorId);

        /// <summary>
        /// دریافت لیست آیتمهای سفارش یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatView> GetFactorGroupFactorItems(int factorGroupId);
        /// <summary>
        /// برای دریافت لیست آیتمهای فاکتور نشده بر اساس مرکز و نمایندگی کاربر لاگین شده
        /// </summary>
        /// <returns></returns>
        IQueryable<UnFactoredOrderItemView> GetCurrentUserUnFactoredOrderItems(int?[] productSalesPlanningIds);

        /// <summary>
        /// دریافت لیست ایستم های فاکتور از
        /// view
        /// از
        /// factorDetailItem Statistic
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsPurchasedFromSuppliersView> GetFactorItemListStatistic(int factorId);

        /// <summary>
        /// برای دریافت لیست آیتمهای فاکتور نشده بر اساس فیلتر مرکز و نمایندگی
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// در هنگام بررسی فاکتور در فرم بررسی فاکتور
        /// پس از زدن دکمه بررسی فاکتور ابتدا این متد صدا زده میشود
        /// این متد بررسی میکند که محاسبات قیمتی درست انجام شده است یا خیر
        /// محاسبات قیمتی توسط sp  با نام
        /// ChecksIsInvalidFactorItemAsync
        /// بررسی میشوند
        /// اگر محاسبات قیمتی درست نبود در فرم دکمه های تایید و رد را نشان نمیدهیم
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IEnumerable<InvalidFactorItemDomainModel> GetInvalidFactorItem(int factorId);
        /// <summary>
        /// بررسی می کند که آیا آیتم سفارش انتخاب شده برای لغو فاکتور شده می باشد یا خیر ؟ 
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش </param>
        /// <returns></returns>
        Task<bool> CheckOrderItemHasBeenFactored(int orderItemId);
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
        /// در تنظیم فروش شناسه فاکتور ارسالی
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
        public Task<int> UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(int factorId);

        /// <summary>
        /// بروزرسانی شامل بیمه بودن ایتم های مربوط به یک فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public Task UpdateFactorItemIsIncludeInsuranceFromProductCategoryAsync(int factorId);
    }
}