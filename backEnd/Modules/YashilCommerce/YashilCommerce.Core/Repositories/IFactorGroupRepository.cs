using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorGroupRepository : IGenericRepository<FactorGroup, int>
    {
        IQueryable<FactorGroup> GetByCustomFilter(int? parentId);
        /// <summary>
        /// برای بررسی اینکه گروه بندی فاکتور داده شده یک گروه پدر هست یا خیر
        /// یعنی یک گروه دیگر در دل خودش دارد یا خیر
        /// </summary>
        /// <param name="factorGroup"></param>
        /// <returns></returns>
        bool CheckFactorGroupIsParent(FactorGroup factorGroup);
        /// <summary>
        /// بررسی اینکه گروه فاکتور داده شده گروه پدر هست یا خیر
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        bool CheckFactorGroupIsParent(int factorGroupId);
        /// <summary>
        /// برای دریافت لیست گروه بندی های فاکتوری که میتوانند بعناوان گروه بندی پدر انتخاب شوند.
        /// </summary>
        /// <returns></returns>
        IQueryable<FactorGroup> GetForSelectParent();
        /// <summary>
        /// بررسی میکند که گروه فاکتور داده شده دارای زیر گروه هست یا خیر
        /// </summary>
        /// <param name="factorGroup"></param>
        /// <returns></returns>
        bool CheckParentHasAnyFactorGroup(FactorGroup factorGroup);
        /// <summary>
        /// گزارش مربوط به کانورت
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorGroupConvertReport> GetFactorGroupConvertReport(int factorGroupId);
        /// <summary>
        /// در صورتی که گروه بندی فاکتور در حالت تایید شده باشد فاکتور را برمیگرداند
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        FactorGroup GetFactorGroupIfConfirm(int factorGroupId, bool readOnly);

        /// <summary>
        /// بررسی میکند که تمامی فاکتورهای درون گروه کانورت فاکتور دارای کد کالا و واحد سنجش و عنوان کانورت دارند یا خیر
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        bool CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(int factorGroupId);
        /// <summary>
        /// پرداخت هایی که تایید شده اند اما در سهم پرداخت آیتم سفارش وجود ندارند 
        /// آنها را در جدول سهم پرداخت آیتم سفارش درج می کند
        /// </summary>
        Task<int> FixMissedOrderItemPriceRightShareAsync();
        /// <summary>
        ///  تعداد پرداخت های آیتم سفارش ها را که به درستی در جدول
        ///OrderItemPayment
        ///تسهیم نشده اند یا در جدول
        ///OrderItemPayment
        ///وجود ندارند را بر می گرداند
        /// </summary>
        /// <returns></returns>
        Task<int> GetMissedOrderItemPriceRightShareCount();
        /// <summary>
        /// دریافت بر اساس شناسه گروه فاکتور
        /// </summary>
        /// <param name="id"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<FactorGroup> FindById(int id, bool readOnly = true);
        /// <summary>
        /// درج آیتم های مربوط به گروه فاکتور
        /// برای هر فاکتور از گروه فاکتور آیتم های مربوط به مالی فاکتور درج می شود
        /// برای این کار
        /// sp : AddFactorGroupItemByFactorGroupId
        /// مربوط رو فراخوانی می کنیم
        /// </summary>
        /// <param name="factorGroupId">شناسه گروه فاکتور</param>
        Task<int> AddFactorGroupItemAsync(int factorGroupId);
    }
}