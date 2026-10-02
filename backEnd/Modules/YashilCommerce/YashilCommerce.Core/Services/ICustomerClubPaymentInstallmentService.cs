using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ICustomerClubPaymentInstallmentService : IGenericService<CustomerClubPaymentInstallment, int>
    {

        IQueryable<CustomerClubPaymentInstallment> GetByCustomFilter(int? paymentId, int? yearMonth);
        Task DeleteByPaymentIdAsync(int paymentId, bool saveAfterDelete);
        Task DeleteRecordsAfterYearMonthByPaymentIdAsync(int paymentId, int currentYearAndMonth, bool saveAfterDelete);
        /// <summary>
        /// رکورد های اقساط پرداخت ناجا که برای مطالبه اقساط ناجا (پرداخت اقساط ناجا به شرکت) به ناجا ارسال نشده است
        /// یعنی DemandGroupId 
        /// برابر با
        /// Null
        /// باشد
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت ناجا</param>
        /// <param name="saveAfterDelete">ذخیره تغییرات بعد از حذف</param>
        /// <returns></returns>
        Task DeleteRecordsNotSentForDemandByPaymentIdAsync(int paymentId, bool saveAfterDelete);

        /// <summary>
        /// آیا مطالبات ناجایی با گروه بندی داده شده وجود دارد یا نه
        /// </summary>
        /// <param name="itemGroupId">گروه </param>
        /// <returns></returns>
        Task<bool> ExistsAnyCustomerClubPaymentInstallmentWithThisItemGroupIdAsync(int itemGroupId);
        /// <summary>
        /// دریافت لیست مطالبات گروه بندی شده متعلق به یک گروه بندی مطالبات ناجا
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsAssignedToDemandGroup(int groupId);
        /// <summary>
        /// جهت دریافت لیست مطالباتی که هنوز گروه بندی نشده اند
        /// </summary>
        /// <returns></returns>
        IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(int itemGroupSubId, bool readOnly = false);
        /// <summary>
        /// جهت انتصاب مطالباتی که تاکنون گروه بندی نشده اند به یک گروه انتخابی و ثبت نهایی گروه
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<bool> AssignAndConfirmCustomerClubPaymentInstallmentDemandGroup(int groupId);
    }
}
