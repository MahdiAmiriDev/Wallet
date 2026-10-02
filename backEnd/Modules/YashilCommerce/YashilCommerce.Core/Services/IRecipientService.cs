using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IRecipientService : IGenericService<Recipient, int>
    {
        IQueryable<Recipient> GetByCustomFilter(int? userId, int? addressId, int? membershipType,
            string nationalCode = null);

        int GetCountByCustomFilter([CanBeNull] int? userId, int? addressId, int? membershipType,
            string nationalCode = null);

        Task<bool> AddRecipientAsync(Recipient recipient, bool saveAfterInsert);

        Address CloneRecipientAddress(int recipientId);

        
        IQueryable<Recipient> GetUserRecipients(int userId);
        /// <summary>
        /// لیست دریافت کنندگان  خدمات بر اساس کاربر و اپلیکیشن
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<Recipient> GetUserRecipientsByApplicationId(int userId, int applicationId);

        IQueryable<Recipient> GetIncludeAddress(int recipientId);
        /// <summary>
        /// دریافت Recipient بر اساس یوزر لاگین شده
        /// </summary>
        /// <param name="recipientId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Recipient> GetUserRecipient(int recipientId, bool readOnly = false);
        Task<bool> UpdateSubsidyStatusTypeAsync(int? recipientId, int? subsidyType, string nationalCode, int? subsidyStatusType);
        IQueryable<CommonBaseData> GetMembershipTypes(bool hekmatBased, bool subsidyBased);
        /// <summary>
        /// دریافت نوع جنسیت
        /// </summary>
        /// <returns></returns>
        IQueryable<CommonBaseData> GetGenderTypes();

        /// <summary>
        /// درج یا بروزرسانی تحویل گیرنده کالا
        /// آدرس و تحویل گیرنده ای که کاربر در پنل عمومی وارد کرده است را دریافت میند
        /// و بر اساس آنها عملیات درج یا بروزرسانی را انجام میدهد
        /// همچنین در صورتی که hasCardNumber
        /// مقدار درست داشته باشد
        /// وضعیت باشگاه مشتریان را تغییر میدهد
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="address"></param>
        /// <param name="hasCardNumber"></param>
        /// <returns></returns>
        Task<(bool isSuccess, string message, int? recipientId)> AddOrUpdateRecipientAsync(Recipient recipient, Address address, bool hasCardNumber);
    }
}