using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IRecipientRepository : IGenericRepository<Recipient, int>
    {
        IQueryable<Recipient> GetByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null);
        int GetCountByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null);
        Address CloneRecipientAddress(int recipientId);
        IQueryable<Recipient> GetUserRecipients(int userId);
        IQueryable<Recipient> GetIncludeAddress(int recipientId);
        /// <summary>
        /// برای دریافت recipient کاربر لاگین شده
        /// </summary>
        /// <param name="recipientId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Recipient> GetUserRecipient(int recipientId, bool readOnly);
        IQueryable<Recipient> GetUserRecipientsByApplicationId(int userId, int applicationId);
    }
}
