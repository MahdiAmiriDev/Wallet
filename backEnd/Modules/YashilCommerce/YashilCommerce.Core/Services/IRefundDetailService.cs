using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IRefundDetailService : IGenericService<RefundDetail, int>
    {

        IQueryable<RefundDetail> GetByCustomFilter(int? refundType, int? refundStatusType, int? refundId, int? refundGroupId, int? paymentId, int? personBankAccountId, int? orderId);
        IQueryable<RefundDetail> GetByOrderId(int orderId);
        Task<bool> ExistsAnyRefundDetailWithThisItemGroupIdAsync(int itemGroupId);
        Task<bool> AssignItemGroupForRefundDetailAndConfirm(int id);
        IQueryable<RefundDetail> GetRefundDetailsAssignedToGroup(int groupId, bool readOnly = false);
        IQueryable<RefundDetail> GetRefundDetailsForPaymentAssignedToGroup(int groupId, bool readOnly = false);

        IQueryable<RefundDetail> GetNotGroupedRefundDetails(int paymentMethodId, bool readOnly = false);

        /// <summary>
        /// لیست عودت وجهایی که گروه بنده شده اند و برای آنها شماره حساب ثبت شده است 
        /// </summary>
        /// <param name="itemGroupSubId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetNotGroupedRefundDetailsForPayment(int itemGroupSubId, bool readOnly = false);
        bool AllowEdit(int id);
        /// <summary>
        /// کد گزارش مربوط به عودت وجه
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> GetRefundDetailGroupReportIdAsync(int groupId);
        /// <summary>
        /// کد گزارش مربوط به پرداخت عودت وجه
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> GetRefundDetailForPaymentGroupReportIdAsync(int groupId);

        /// <summary>
        /// آیا رکورد عودت وجهی جهت پرداخت با این کد گروه وجود دارد
        /// </summary>
        /// <param name="itemGroupId"></param>
        Task<bool> ExistsAnyRefundDetailForPayWithThisItemGroupIdAsync(int itemGroupId);
        /// <summary>
        /// گروه بندی پرداخت عودت وجه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<bool> AssignItemGroupForRefundDetailForPaymentAndConfirmWithArchiveAsync(int id);

        /// <summary>
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// اگر ادمین باشد همه عوذت وجه ها را نشان میدهد و اگر 
        /// archive mode 1 آرشیو شده ها
        /// archive mode 2 آرشیو نشده ها
        /// archive mode 3 همه موارد
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetGroupedItems(int? archiveMode, bool readOnly = false);

        IQueryable<RefundDetail> GetRefundDetailStatistic(int refundDetailId);
        /// <summary>
        /// بررسی میکند که برای آیتم گروه بندی عودت داده شده حکمت آیا مجاز به تایید اطلاعات است یا خیر
        /// در واقع باید چک کنیم که اطلاعات حکمت آمده است یا خیر و اینکه مبلغ عودت با مبلغ اعلام شده حکمت برابر است یا خیر
        /// در غیر این صورت پیغام های مناسبی به کاربر داده میشود
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        Task<string> IsValidHekmatRefundDetailForRejectAsync(int refundDetailId);

        /// <summary>
        /// تایید تراکنش عودت
        /// این متد زمانی استفاده میشود که بخواهد بررسی کننده عودتی ها عودتی را تایید کند
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="announcedRefundAmount"></param>
        /// <returns></returns>
        Task<string> ConfirmRefundDetailAsync(int refundDetailId, long? announcedRefundAmount);
        /// <summary>
        /// رد کردن تایید عودت
        /// این متد زمانی استفاده میشود که بررسی کننده عودت وجهی ها 
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        Task<string> RejectRefundDetailAsync(int refundDetailId);
        /// <summary>
        /// بررسی میکند اطلاعات حساب شخص در عودتی ها وجود دارد یا خیر 
        /// </summary>
        /// <param name="personBankAccountId">اطلاعات حساب شخص</param>
        /// <returns></returns>
        bool CheckExistAnyRefundDetailByPersonBankAccountId(int personBankAccountId);

        /// <summary>
        /// بروز رسانی تراکنش های عودتی حکمت بر اساس فایل دریافتی از حکمت که در جداول عودتی های حمت درج شده است
        /// </summary>
        Task<int> UpdateHekmatRefundDetailRefundAmountAsync();

        /// <summary>
        /// جهت بررسی اجازه تایید عودت وجه
        /// بررسی میشود که کاربر اجازه تایید بررسی عودت وجه را دارد یا خیر
        /// اگر داشته باشد و مبلغ درون سی دی حکمت نباشد باید خود کاربر مبلغ را وارد کند
        /// </summary>
        /// <param name="id"></param>
        /// <param name="announcedRefundAmount"></param>
        /// <returns></returns>
        Task<string> IsValidHekmatRefundDetailForConfirmAsync(int id, long? announcedRefundAmount);

        /// <summary>
        /// عودت وجه هایی که گروه بندی نشده باشد 
        /// یا گروه بندی شده باشد ولی مقدار عودت به کاربر آن
        /// NUll
        /// باشد، برای آیتم های گروه جدید ،گروه بندی می شود
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <param name="yearMonth">سال و ماه</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetRefundDetailsForNewGroup(int paymentMethodId, int? yearMonth = null, bool readOnly = false);
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
        Task<string> SendRefundDetailToNewGroupAsync(int refundDetailId);
        /// <summary>
        /// بررسی می کند برای هر پرداخت  قیمت جدید وجود دارد  
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        bool ChecksThereAreNewPriceByPaymentId(int paymentId);
        /// <summary>
        /// بررسی می کند که آیا عودت وجه مربوط به رکورد تغییر آیتم سفارش گروه بندی شده است
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        Task<bool> IsGroupedRefundDetail(int modifiedOrderItemId);
        /// <summary>
        /// جهت بایگانی کردن یک عودت وجه
        /// در این متد ابتدا چک میکنیم که اطلاعات حساب دریافت شده باشد اگر دریافت شده مجاز به بایگانی هستیم
        /// در غیر این صورت نمیتوان بایگانی انجام داد
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="archiveDescription"></param>
        /// <returns></returns>
        Task<string> ArchiveRefundDetailAsync(int refundDetailId, string archiveDescription);

        /// <summary>
        /// جهت حذف از بایگانی عودت وجه
        ///  هنگامی اجازه حذف از بایگانی داریم که عودت وجه قبلا آرشیو شده باشد
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        Task<string> UnArchiveRefundDetailAsync(int refundDetailId);
        /// <summary>
        /// دریافت لیست تمامی عودت وجه های گروه بندی شده
        /// archive mode 1 آرشیو شده ها
        /// archive mode 2 آرشیو نشده ها
        /// archive mode 3 همه موارد
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetAllGroupedItems(int? archiveMode, bool readOnly = false);
        /// <summary>
        /// جهت انتصاب یک عودت وجه به یک کاربر
        /// کاربر باید نقش crm را داشته باشد
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="assigneeId"></param>
        /// <returns></returns>
        Task<string> AssignRefundDetailAsync(int refundDetailId, int? assigneeId);
        /// <summary>
        /// اطلاعات کامل عودت وجه ها را بر اساس فیلتر ارسالی پارامتر ها نمایش می دهد
        /// </summary>
        /// <param name="showGrouped">نمایش گروه بندی شده </param>
        /// <param name="showPaymentGrouped">نمایش گروه بندی برای پرداخت شده</param>
        /// <param name="showHasBankAccount">نمایش دارای اطلاعات حساب</param>
        /// <param name="refundCreationDateFromDate">تاریخ شروع ثبت عودت</param>
        /// <param name="refundCreationDateToDate">تاریخ پایان ثبت عودت</param>
        /// <returns></returns>
        IQueryable<RefundDetail> GetRefundDetailFullInfo(int refundCreationDateFromDate, int refundCreationDateToDate, bool? showGrouped, bool? showPaymentGrouped, bool? showHasBankAccount);
        /// <summary>
        /// نام کاربرانی که عودت وجه به انها تخصیص داده و تعداد عودت اختصاص داده شده  را بر می گرداند
        /// </summary>
        /// <returns></returns>
        IQueryable<RefundAssignedDomainModel> GetAssignedUsers();

        /// <summary>
        /// دریافت اطلاعات عودت وجه بر اساس شناسه جزییات عودت وجه
        /// </summary>
        /// <param name="id">شناسه جزییات عودت وجه</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<RefundDetail> FindById(int id, bool readOnly = true);

        /// <summary>
        /// حذف اطلاعات حساب از رکورد عودت وجه
        /// </summary>
        /// <param name="refundDetailId">شناسه جزییات عودت وجه</param>
        /// <returns></returns>
        Task<(bool isSuccess, string message)> RemovePersonBankAccountFromRefundDetailAsync(int refundDetailId);


        IQueryable<RefundDetail> GetRefundDetailsbyFactorId(int factorId);
    }
}
