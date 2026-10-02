using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IRefundDetailRepository : IGenericRepository<RefundDetail, int>
    {

        IQueryable<RefundDetail> GetByCustomFilter(int? refundType, int? refundStatusType, int? refundId, int? refundGroupId, int? paymentId, int? personBankAccountId, int? orderId);
        IQueryable<RefundDetail> GetByOrderId(int orderId);
        Task<bool> ExistsAnyRefundDetailWithThisItemGroupIdAsync(int itemGroupId);
        Task<int> AssignItemGroupForRefundDetailAndConfirmAsync(int itemGroupStatusType, int groupId, int yearMonth);
        // Task AssignItemGroupForNajaOldSiteRefundDetailAsync(int groupId);
        IQueryable<RefundDetail> GetRefundDetailsAssignedToGroup(int groupId, bool readOnly = false);

        IQueryable<RefundDetail> GetNotGroupedRefundDetails(int paymentMethodType, bool readOnly = false);

        Task<bool> ExistsAnyRefundDetailForPaymentWithThisItemGroupIdAsync(int itemGroupId);
        Task<int> AssignItemGroupForRefundDetailForPaymentAndConfirmWithArchiveAsync(int value, int groupId);
        IQueryable<RefundDetail> GetNotGroupedRefundDetailsForPayment(int paymentMethodType, bool readOnly = false);
        IQueryable<RefundDetail> GetRefundDetailsForPaymentAssignedToGroup(int groupId, bool readOnly);

        /// <summary>
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// 
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetGroupedItems(int? archiveMode, bool readOnly = false);

        IQueryable<RefundDetail> GetRefundDetailStatistic(int refundDetailId, bool readOnly = false);
        /// <summary>
        /// بررسی میکند اطلاعات حساب شخص در عودتی ها وجود دارد یا خیر 
        /// </summary>
        /// <param name="personBankAccountId">اطلاعات حساب شخص</param>
        /// <returns></returns>
        bool CheckExistAnyRefundDetailByPersonBankAccountId(int personBankAccountId);

        Task<int> UpdateHekmatRefundDetailRefundAmountAsync();

        /// <summary>
        /// عودت وجه هایی که گروه بندی نشده باشد 
        /// یا گروه بندی شده باشد ولی مقدار عودت به کاربر آن
        /// NUll
        /// باشد، برای آیتم های گروه جدید نمایش میدهد
        /// </summary>
        /// <param name="paymentMethodType">روش پرداخت</param>
        /// <param name="yearMonth">سال و  ماه</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetRefundDetailsForNewGroup(int paymentMethodType, int? yearMonth = null, bool readOnly = false);

        /// <summary>
        /// در فرم گروه بندی عودت وجه کاربرد دارد
        ///  بر اساس مبلغ و شماره کارت
        /// لیست تراکنش های غودت وجه حکمت که با این شماره کارت و مبلغ همخوانی دارند را نشان میدهد
        /// عودت وجه ها از جدول های
        /// HekmatCanceledTransaction
        /// HekmatEffectlessCanceledTransaction
        /// HekmatEffectlessModifiedTransaction
        /// HekmatModifiedTransaction
        /// میخواند و برمیگرداند 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        IQueryable<HekmatCanceledOrModifiedTransactionDomainModel> GetHekmatCanceledOrModifiedTransactionsAsync(string cardNumber, long price);
        /// <summary>
        /// رکورد عودت وجه در صورتی که تعیین تکلیف نشده باشد یعنی 
        /// بعد از بارگذاری سی دی تراکنش های عودتی، مبلغ عودت به کاربر آن مقدار نگیرد
        ///  .و کارشناس مربوطه نیز مقدار عودتی به کاربر را از سی دی پیدا نکند تا از طریق فرم وارد کند،
        ///  مجددا این رکورد را در عودتی های ماه جدید به حکمت ارسال میکند
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        Task<int> SendRefundDetailToNewGroupAsync(int refundDetailId);
        /// <summary>
        /// بررسی می کند که آیا عودت وجه مربوط به رکورد تغییر آیتم سفارش گروه بندی شده است
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        Task<bool> IsGroupedRefundDetail(int modifiedOrderItemId);

        /// <summary>
        /// دریافت لیست تمامی عودت وجه های گروه بندی شده
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetAllGroupedItems(int? archiveMode, bool readOnly = false);
        /// <summary>
        /// اطلاعات کامل عودت وجه ها را بر اساس فیلتر ارسالی پارامتر ها نمایش می دهد
        /// </summary>
        /// <param name="showGrouped">نمایش گروه بندی شده </param>
        /// <param name="showPaymentGrouped">نمایش گروه بندی برای پرداخت شده</param>
        /// <param name="showHasBankAccount">نمایش دارای اطلاعات حساب</param>
        /// <param name="refundCreationDateFromDate">تاریخ شروع ثبت عودت</param>
        /// <param name="refundCreationDateToDate">تاریخ پایان ثبت عودت</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetRefundDetailFullInfo(int applicationId, int refundCreationDateFromDate, int refundCreationDateToDate, bool? showGrouped, bool? showPaymentGrouped, bool? showHasBankAccount);
        /// <summary>
        /// نام کاربرانی که عودت وجه به انها تخصیص داده و تعداد عودت اختصاص داده شده  را بر می گرداند
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<RefundAssignedDomainModel> GetAssignedUsers(int applicationId);

        /// <summary>
        /// دریافت اطلاعات عودت وجه بر اساس شناسه جزییات عودت وجه
        /// </summary>
        /// <param name="id">شناسه جزییات عودت وجه</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<RefundDetail> FindById(int id, bool readOnly = true);

        IQueryable<RefundDetail> GetRefundDetailsbyFactorId(int factorId);
    }
}
