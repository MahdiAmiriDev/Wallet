using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IHekmatTransactionRepository : IGenericRepository<HekmatTransaction, int>
    {

        /// <summary>
        /// فایل تراکنش هایی را که منتظر تعیین وضعیت می باشند را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<HekmatTransactionCustomModel> GetHekmatTransactions();

        /// <summary>
        /// به روز رسانی نتیجه تراکنش های حکمت در دیتا بیس درگاه حکمت
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<string> UpdateHekmatTransactionsAsync(int userId);
        /// <summary>
        /// نتیجه تراکنش های حکمت کاربر را که به طور کامل انجام نشده است حذف می کند 
        /// </summary>
        /// <param name="userId"></param>
        Task<int> DeleteByUserIdAsync(int userId);
        /// <summary>
        /// کد ملی و شماره موبایل و شماره تلفن تراکنش های حکمت را به حالت رمز در می آورد و در 
        /// ستون های جدا ریخته و ستون های کد ملی و شماره موبایل و تلفن را به عدد 1 به روز رسانی می کتد
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        Task EncryptHekmatTransactionsAsync(string key);
    }
}
