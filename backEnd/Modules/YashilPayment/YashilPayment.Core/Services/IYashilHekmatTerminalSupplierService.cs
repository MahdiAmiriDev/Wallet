using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
	public interface IYashilHekmatTerminalSupplierService : IGenericService<YashilHekmatTerminalSupplier, int>
    {
        IQueryable<YashilHekmatTerminalSupplier> GetByCustomFilter( int? parentId, int? userId);

        /// <summary>
        /// در صورتی که کاربر که در حال حاظر در سامانه لاگین کرده است
        /// ادمین باشد تمامی رکورد های مربوط به جدول را برمیگرداند
        /// و در غیر این صورت فقط رکورد که مربوط به کاربر لاگین شده است را برمیگرداند
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        IQueryable<YashilHekmatTerminalSupplier> GetForSelect(bool isAdmin);

        /// <summary>
        /// این متد بررسی می کند که شناسه کاربری ارسالی به
        /// شناسه فروشنده ارسالی دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <param name="yashilHekmatTerminalId"></param>
        /// <returns></returns>
        Task<bool> CheckUserHasAccessToHekmatTerminalSupplierAsync(int userId, int supplierId,int? yashilHekmatTerminalId = null);




    }
}
