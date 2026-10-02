using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IUploadedVoucherRepository : IGenericRepository<UploadedVoucher, int>
    {

        IQueryable<UploadedVoucher> GetByCustomFilter(int? appDocumentId);

        /// <summary>
        /// حذف کوپن های کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        void DeleteByUserId(int userId);
        /// <summary>
        /// دریافت کد کوپن های بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        IQueryable<UploadedVoucher> GetByUserId(int userId);
        
    }
}
