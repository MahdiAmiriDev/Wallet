using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IFactorGroupService : IGenericService<FactorGroup, int>
    {
        IQueryable<FactorGroup> GetByCustomFilter(int? parentId);
        /// <summary>
        /// بروزرسانی و تایید نهایی گروه بندی فاکتور
        /// در صورتی که گروه بندی فاکتور دارای آیتم های فاکتور(گروه فاکتور باشد) و در وضعیت اولیه باشد میتواند کانورت فاکتور شود
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <param name="saveAfterAddOrUpdate"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<(bool isSuccessful, string message)> ConfirmFactorGroup(int factorGroupId, bool saveAfterAddOrUpdate = false);
        /// <summary>
        /// برای چک کردن اینکه گروه بندی فاکتور داده شده یک گروه پدر هست یا خیر
        /// </summary>
        /// <param name="factorGroup"></param>
        /// <returns></returns>
        bool CheckFactorGroupIsParent(FactorGroup factorGroup);
        /// <summary>
        /// بررسی اینکه گروه داده شده گروه پدر هست یا خیر
        /// </summary>
        /// <param name="factorGroup"></param>
        /// <returns></returns>
        bool CheckFactorGroupIsParent(int factorGroup);
        /// <summary>
        /// برای دریافت لیست گروه بندی های فاکتوری که میتوانند بعنوان گروه بندی پدر انتخاب شوند.
        /// </summary>
        /// <returns></returns>
        IQueryable<FactorGroup> GetForSelectParent();
        /// <summary>
        /// بررسی اینکه گروه داده شده دارای زیرگروهی میباشد یا خیر
        /// </summary>
        /// <param name="factorGroup"></param>
        /// <returns></returns>
        bool CheckParentHasAnyFactorGroup(FactorGroup factorGroup);
        /// <summary>
        /// مربوط به گزارش کانورت
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorGroupConvertReport> GetFactorGroupConvert(int factorGroupId);
        /// <summary>
        /// برای دریافت گروه بندی فاکتور اگر گروه بندی تایید شده باشد
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        FactorGroup GetFactorGroupIfConfirm(int factorGroupId, bool readOnly);

        /// <summary>
        ///  بررسی میکنیم که برای گروه داده شده داده های مجاز وارد شده است یا خیر
        /// یعنی باید نام کالا، کد کالا و واحد سنجش مقدار داشته باشند.
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
        /// بررسی می کند که آیا تسهیم پرداختی برای آیتم سفارش به درستی انجام شد است یا خیر
        /// </summary>
        /// <returns></returns>
        Task<bool> checkIsValidOrderItemPriceRightShareAsync();

        /// <summary>
        /// دریافت بر اساس شناسه گروه فاکتور
        /// </summary>
        /// <param name="id"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<FactorGroup> FindById(int id,bool readOnly = true);

        /// <summary>
        /// خارج کردن فاکتور ها از گروه فاکتور بر اساس شناسه گروه فاکتور
        /// </summary>
        /// <param name="id">شناسه گروه فاکتور</param>
        /// <returns></returns>
        Task<int> RemovingFactorsFromFactorGroupAsync(int id);
        /// <summary>
        /// تغییر وضعیت گروه فاکتور به وضعیت ثبت اولیه
        /// </summary>
        /// <returns></returns>
        Task<(bool IsSucces,string Message)> ChangeFactorGroupStateToInitialAsync(int factorGroupId);
        /// <summary>
        /// درج آیتم های مربوط به گروه فاکتور : 
        /// برای هر فاکتور آیتم های مربوط به مالی فاکتور درج می شود
        /// </summary>
        /// <param name="factorGroupId">شناسه گروه فاکتور</param>
        Task<int> AddFactorGroupItemAsync(int factorGroupId);
    }
}