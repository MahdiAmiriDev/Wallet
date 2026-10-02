using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilUserManagement.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class RefundDetailRepository : GenericApplicationBasedRepository<RefundDetail, int>, IRefundDetailRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IUserRoleRepository _userRoleRepository;

        public RefundDetailRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            IUserRoleRepository userRoleRepository) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _userRoleRepository = userRoleRepository;
        }

        public IQueryable<RefundDetail> GetByCustomFilter(int? refundType, int? refundStatusType, int? refundId,
            int? refundGroupId, int? paymentId, int? personBankAccountId, int? orderId)
        {
            var query = GetAll(true);
            if (refundType.HasValue)
            {
                query = query.Where(x => x.RefundType == refundType.Value);
            }

            if (refundStatusType.HasValue)
            {
                query = query.Where(x => x.RefundStatusType == refundStatusType.Value);
            }

            if (refundId.HasValue)
            {
                query = query.Where(x => x.RefundId == refundId.Value);
            }

            if (refundGroupId.HasValue)
            {
                query = query.Where(x => x.RefundDetailGroupId == refundGroupId.Value);
            }

            if (paymentId.HasValue)
            {
                query = query.Where(x => x.PaymentId == paymentId.Value);
            }

            if (personBankAccountId.HasValue)
            {
                query = query.Where(x => x.PersonBankAccountId == personBankAccountId.Value);
            }

            if (orderId.HasValue)
            {
                query = query.Where(x => x.Payment.OrderId == orderId);
            }


            return query;
        }

        public IQueryable<RefundDetail> GetByOrderId(int orderId)
        {
            return DbSet.Where(x => x.Refund.OrderId == orderId);
        }

        #region RefundDetailForPayment Groupping Methods

        public Task<bool> ExistsAnyRefundDetailForPaymentWithThisItemGroupIdAsync(int itemGroupId)
        {
            return DbSet.AnyAsync(x => x.RefundDetailForPaymentGroupId == itemGroupId);
        }

        public Task<int> AssignItemGroupForRefundDetailForPaymentAndConfirmWithArchiveAsync(int paymentMethodType,
            int groupId)
        {
            return GetNotGroupedRefundDetailsForPayment(paymentMethodType)
                .UpdateAsync(x => new RefundDetail
                {
                    RefundDetailForPaymentGroupId = groupId,
                    IsArchived = true,
                    ArchiveBy = _userPrincipal.Id,
                    ArchiveDate = DateTime.Now
                });
        }

        public IQueryable<RefundDetail> GetRefundDetailsForPaymentAssignedToGroup(int groupId, bool readOnly)
        {
            var query = DbSet.Where(x => x.RefundDetailForPaymentGroupId == groupId);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public IQueryable<RefundDetail> GetNotGroupedRefundDetailsForPayment(int paymentMethodType,
            bool readOnly = false)
        {
            var query = DbSet
                .Where(x =>
                    !x.RefundDetailForPaymentGroupId.HasValue &&
                    x.PersonBankAccountId.HasValue &&
                    x.Payment.PaymentMethodType == paymentMethodType &&
                    x.RefundAmount.HasValue &&
                    (x.RefundDetailGroupId.HasValue || x.RefundType == (int)RefundTypeEnum.Online)
                );
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        #endregion

        #region Refund Detail Groupping Methods

        public Task<bool> ExistsAnyRefundDetailWithThisItemGroupIdAsync(int itemGroupId)
        {
            return DbSet.AnyAsync(x => x.RefundDetailGroupId == itemGroupId);
        }

        //public Task AssignItemGroupForNajaOldSiteRefundDetailAsync(int groupId)
        //{
        //    //return _context.OldSiteRefundDetail.Where(x => !x.RefundDetailGroupId.HasValue)
        //    //    .UpdateAsync(x => new OldSiteRefundDetail
        //    //    {
        //    //        RefundDetailGroupId = groupId
        //    //    });
        //}

        /// <summary>
        /// عودت وجه هایی که گروه بندی نشده باشد 
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Task<int> AssignItemGroupForRefundDetailAndConfirmAsync(int paymentMethodType, int groupId,
            int yearMonth)
        {
            return GetRefundDetailsForNewGroup(paymentMethodType, yearMonth)
                .UpdateAsync(x => new RefundDetail
                {
                    RefundStatusType = (int)RefundStatusTypeEnum.Done,
                    RefundDetailGroupId = groupId
                });
        }

        /// <summary>
        /// عودت وجه هایی که گروه بندی نشده باشد 
        ///   برای آیتم های گروه بندی نشده نمایش میدهد
        /// </summary>
        /// <param name="paymentMethodType">روش پرداخت</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetNotGroupedRefundDetails(int paymentMethodType, bool readOnly = false)
        {
            var query = DbSet.Where(x =>
                !x.RefundDetailGroupId.HasValue && x.Payment.PaymentMethodType == paymentMethodType);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// عودت وجه هایی که گروه بندی نشده که تاریخ پرداخت آنها قبل از 
        /// yearMonth
        /// باشد را به عنوان گروه جدید برای ارسال به حکمت گروه بندی میکند 
        /// </summary>
        /// <param name="paymentMethodType">روش پرداخت</param>
        /// <param name="yearMonth">تاریخ پرداخت</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetRefundDetailsForNewGroup(int paymentMethodType, int? yearMonth = null,
            bool readOnly = false)
        {
            var query = DbSet.Where(x =>
                (!x.RefundDetailGroupId.HasValue)
                && x.Payment.PaymentMethodType == paymentMethodType);
            if (yearMonth.HasValue)
            {
                query = query.Where(x => x.Payment.YearMonth <= yearMonth.Value);
            }

            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public IQueryable<HekmatCanceledOrModifiedTransactionDomainModel> GetHekmatCanceledOrModifiedTransactionsAsync(
            string cardNumber, long price)
        {
            // در جدول اطلاعات لغو که از سمت حکمت ارسال می شود و توسط فرم در جدول 
            // hekmatCanceledTransaction
            // ذخیره شده است رکوردها را با توحه به مبلغ و شماره کارت ارسالی از فرم برمیگرداند
            var hekmatCanceledTransaction = _context.HekmatCanceledTransaction.Where(x =>
                x.CartNumber == cardNumber && x.SendTransactionPrice == price).Select(x =>
                new HekmatCanceledOrModifiedTransactionDomainModel
                {
                    CardNumber = x.CartNumber,
                    Price = x.SendTransactionPrice,
                    RefundPrice = x.RefundPrice,
                    TransactionDate = x.TransactionDate
                });
            //
            // در جدول اطلاعات لغوبدون تاثیر که از سمت حکمت ارسال می شود و توسط فرم در جدول 
            // hekmatEffectlessCanceledTransactions
            // ذخیره شده است رکوردها را با توحه به مبلغ و شماره کارت ارسالی از فرم برمیگرداند
            var hekmatEffectlessCanceledTransactions =
                _context.HekmatEffectlessCanceledTransaction.Where(x =>
                    x.CartNumber == cardNumber && x.ModifiedTransactionPrice == price).Select(x =>
                    new HekmatCanceledOrModifiedTransactionDomainModel
                    {
                        CardNumber = x.CartNumber,
                        Price = x.ModifiedTransactionPrice,
                        RefundPrice = 0,
                        TransactionDate = x.TransactionDate
                    });
            // در جدول اطلاعات اصلاح بدون تاثیر که از سمت حکمت ارسال می شود و توسط فرم در جدول 
            // hekmatEffectlessModifiedTransactions
            // ذخیره شده است رکوردها را با توحه به مبلغ و شماره کارت ارسالی از فرم برمیگرداند

            var hekmatEffectlessModifiedTransactions =
                _context.HekmatEffectlessModifiedTransaction.Where(x =>
                    x.ModifiedTransactionPrice == price && x.CartNumber == cardNumber).Select(x =>
                    new HekmatCanceledOrModifiedTransactionDomainModel
                    {
                        CardNumber = x.CartNumber,
                        Price = x.ModifiedTransactionPrice,
                        RefundPrice = 0,
                        TransactionDate = x.TransactionDate
                    });


            // در جدول اطلاعات اصلاح که از سمت حکمت ارسال می شود و توسط فرم در جدول 
            // hekmatModifiedTransactionPrice
            // ذخیره شده است رکوردها را با توحه به مبلغ و شماره کارت ارسالی از فرم برمیگرداند
            //
            var hekmatModifiedTransactionPrice =
                _context.HekmatModifiedTransaction.Where(x =>
                    x.CartNumber == cardNumber && x.SendTransactionPrice == price).Select(x =>
                    new HekmatCanceledOrModifiedTransactionDomainModel
                    {
                        CardNumber = x.CartNumber,
                        Price = x.SendTransactionPrice,
                        RefundPrice = x.RefundPrice,
                        TransactionDate = x.TransactionDate
                    });


            return hekmatCanceledTransaction.Union(hekmatEffectlessCanceledTransactions)
                .Union(hekmatEffectlessModifiedTransactions).Union(hekmatModifiedTransactionPrice).AsNoTracking();
        }

        public IQueryable<RefundDetail> GetRefundDetailsAssignedToGroup(int groupId, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.RefundDetailGroupId == groupId);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetGroupedItems(int? archiveMode, bool readOnly = false)
        {
            var query = GetAllGroupedItems(archiveMode, readOnly);


            var userIsAdmin = _userRoleRepository.IsAdminCurrentUser();
            if (!userIsAdmin)
            {
                query = query.Where(x => x.Assignee == _userPrincipal.Id);
            }
            return query;
        }

        public IQueryable<RefundDetail> GetRefundDetailStatistic(int refundDetailId, bool readOnly = false)
        {
            return DbSet.Where(x => x.Id == refundDetailId);
        }

        /// <summary>
        /// بررسی میکند اطلاعات حساب شخص در عودتی ها وجود دارد یا خیر 
        /// </summary>
        /// <param name="personBankAccountId">اطلاعات حساب شخص</param>
        /// <returns></returns>
        public bool CheckExistAnyRefundDetailByPersonBankAccountId(int personBankAccountId)
        {
            return DbSet.Any(rd => rd.PersonBankAccountId == personBankAccountId);
        }

        public Task<int> UpdateHekmatRefundDetailRefundAmountAsync()
        {
            return _context.Database.ExecuteSqlRawAsync("EXEC [shp].[SetRefundDetailIdInHekmatRefund]");
        }

        /// <summary>
        /// رکورد عودت وجه در صورتی که تعیین تکلیف نشده باشد یعنی 
        /// بعد از بارگذاری سی دی تراکنش های عودتی، مبلغ عودت به کاربر آن مقدار نگیرد
        ///  .و کارشناس مربوطه نیز مقدار عودتی به کاربر را از سی دی پیدا نکند تا از طریق فرم وارد کند،
        ///  مجددا این رکورد را در عودتی های ماه جدید به حکمت ارسال میکند
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        public Task<int> SendRefundDetailToNewGroupAsync(int refundDetailId)
        {
            return DbSet.Where(r => r.Id == refundDetailId & !r.RefundAmount.HasValue)
                .UpdateAsync(x => new RefundDetail
                {
                    RefundDetailGroupIdHistory = x.RefundDetailGroupId.HasValue
                        ? x.RefundDetailGroupIdHistory + "," + x.RefundDetailGroupId.Value
                        : null,
                    RefundDetailGroupId = null,
                    RefundStatusType = (int)RefundStatusTypeEnum.Register
                });
        }

        /// <summary>
        /// بررسی می کند اگر نوع عودت وجه ناجا و حکمت باشد (آنلاین نباشد) و گروه بندی شده باشد و یا 
        /// نوع عودت وجه آنلاین باشد و گروه بندی پرداخت شده باشد
        /// True برمی گرداند
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        public async Task<bool> IsGroupedRefundDetail(int modifiedOrderItemId)
        {
            return await DbSet.AnyAsync(x => x.ModifiedOrderItemId == modifiedOrderItemId
                                             &&
                                             (
                                                 (x.RefundType != (int)RefundTypeEnum.Online &&
                                                  x.RefundDetailGroupId.HasValue)
                                                 ||
                                                 (x.RefundType == (int)RefundTypeEnum.Online &&
                                                  x.RefundDetailForPaymentGroupId.HasValue)
                                             )
            );
        }

        public IQueryable<RefundDetail> GetAllGroupedItems(int? archiveMode, bool readOnly = false)
        {
            IQueryable<RefundDetail> query = DbSet.Where(x =>
                x.ApplicationId == _userPrincipal.ApplicationId &&
                (
                    (x.Confirmed.HasValue && x.Confirmed.Value && x.RefundDetailGroupId.HasValue) ||
                    x.RefundType == (int)RefundTypeEnum.Online) &&
                !x.RefundDetailForPaymentGroupId.HasValue &&
                x.RefundAmount.HasValue && x.RefundAmount > 0 &&
                x.RefundType != (int)RefundTypeEnum.Naja).OrderByDescending(x => x.CreationDate);
            if (archiveMode.HasValue)
            {
                // اگر آرشیو شده ها را بخواهیم
                if (archiveMode.Value == (int)ArchiveModeEnum.Archived)
                {
                    query = query.Where(x => x.IsArchived);
                }
                // اگر آرشیو نشده ها را بخواهیم
                else if (archiveMode.Value == (int)ArchiveModeEnum.NotArchived)
                {
                    query = query.Where(x => !x.IsArchived);
                }
                // در غیر این صورت یعنی همه را میخواهیم
            }

            if (readOnly)
            {
                return query.AsNoTracking();
            }


            return query;
        }
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
        public IQueryable<RefundDetail> GetRefundDetailFullInfo(int applicationId, int refundCreationDateFromDate,
            int refundCreationDateToDate, bool? showGrouped, bool? showPaymentGrouped, bool? showHasBankAccount)
        {
            var query = GetAll(true, applicationId).Where(rd => rd.CreationDateInt >= refundCreationDateFromDate
                && rd.CreationDateInt <= refundCreationDateToDate);

            if (showGrouped.HasValue)
            {
                query = query.Where(rd => showGrouped.Value && rd.RefundDetailGroupId.HasValue);
            }
            if (showPaymentGrouped.HasValue)
            {
                query = query.Where(rd => showPaymentGrouped.Value && rd.RefundDetailForPaymentGroupId.HasValue);
            }
            if (showHasBankAccount.HasValue)
            {
                query = query.Where(rd => showHasBankAccount.Value && rd.PersonBankAccountId.HasValue);
            }
            return query.AsNoTracking();
        }
        /// <summary>
        /// نام کاربرانی که عودت وجه به انها تخصیص داده و تعداد عودت اختصاص داده شده  را بر می گرداند
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public IQueryable<RefundAssignedDomainModel> GetAssignedUsers(int applicationId)
        {
            var query = GetAll(true, applicationId).AsNoTracking()
                .Where(x => !string.IsNullOrEmpty(x.AssigneeNavigation.PersonNavigation.LastName));
            return query.GroupBy(x => new
            {
                assignee = x.Assignee,
                assigneeFullName = string.Concat(x.AssigneeNavigation.PersonNavigation.Name, " "
                                                     + x.AssigneeNavigation.PersonNavigation.LastName)
            })
                 .Select(rd => new RefundAssignedDomainModel()
                 {
                     Count = rd.Count(),
                     PersonName = rd.Key.assigneeFullName
                 });
        }

        public IQueryable<RefundDetail> FindById(int id, bool readOnly = true)
        {
            return GetAll(readOnly).Where(x => x.Id == id);
        }

        public IQueryable<RefundDetail> GetRefundDetailsbyFactorId(int factorId)
        {
            return DbSet.Where(x => x.Payment.Order.OrderItem.Any(o => o.FactorItem.FactorId == factorId));
        }

        #endregion
    }
}