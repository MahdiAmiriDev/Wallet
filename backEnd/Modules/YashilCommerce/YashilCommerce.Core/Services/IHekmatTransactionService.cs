using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IHekmatTransactionService : IGenericService<HekmatTransaction, int>
    {

        /// <summary>
        /// فایل تراکنش هایی را که منتظر تعیین وضعیت می باشند را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<HekmatTransactionCustomModel> GetHekmatTransactions();
        /// <summary>
        /// دریافت فایل تراکنش های حکمت بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        ///  <param name="readOnly">فقط خواندنی</param>
        /// <returns></returns>
        IQueryable<HekmatTransaction> GetByUserId(int userId, bool readOnly);
        /// <summary>
        /// به روز رسانی نتیجه تراکنش های حکمت در دیتا بیس درگاه حکمت
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<string> UpdateHekmatTransactionAsync(int userId);
        /// <summary>
        /// نتیجه تراکنش های حکمت کاربر را که به طور کامل انجام نشده است حذف می کند 
        /// </summary>
        /// <param name="userId"></param>
        Task<int> DeleteByUserIdAsync(int userId);
        /// <summary>
        /// بارگذاری فایل نتیجه تراکنش های حکمت
        /// </summary>
        /// <param name="tmpHekmatTransactionFile">فایل نتیجه تراکنش حکمت</param>
        /// <returns></returns>
        Task<string> BulkInsertFromCsvAsync(IFormFile tmpHekmatTransactionFile);
        /// <summary>
        /// کد ملی و شماره موبایل و شماره تلفن تراکنش های حکمت را به حالت رمز در می آورد و در 
        /// ستون های جدا ریخته و ستون های کد ملی و شماره موبایل و تلفن را به عدد 1 به روز رسانی می کتد
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        Task EncryptHekmatTransactionsAsync(string key);
    }
}
