using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class RefundDetailService : GenericService<RefundDetail, int>, IRefundDetailService
    {
        private const string WrongRefundAmountMessage = "کاربر گرامی مبلغ عودت با مبلغ اعلام شده توسط حکمت همخوانی ندارد";
        private const string RefundDetailIsConfirmedBeforeMessage = "کاربر گرامی این تراکنش قبلا تایید شده است";
        private const string RefundDetailNotFoundMessage = "کاربر گرامی تراکنش عودت مورد نظر یافت نشد";
        private const string RefundDetailAfterSendToNewGroupMessage = "کاربر گرامی رکورد عودت وجه به گروه جدید انتقال داده شد";

        private readonly IRefundDetailRepository _refundDetailRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IItemGroupService _itemGroupService;
        private readonly IAppConfigService _appConfigService;
        private readonly IHekmatCanceledTransactionService _hekmatCanceledTransactionService;
        private readonly IHekmatEffectlessCanceledTransactionService _hekmatEffectlessCanceledTransactionService;
        private readonly IHekmatEffectlessModifiedTransactionService _hekmatEffectlessModifiedTransactionService;
        private readonly IHekmatModifiedTransactionService _hekmatModifiedTransactionService;
        private readonly IUserService _userService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IYashilLogger<RefundDetail> _logger;

        public RefundDetailService(IUnitOfWork unitOfWork, IRefundDetailRepository refundDetailRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IItemGroupService itemGroupService,
            IHekmatModifiedTransactionService hekmatModifiedTransactionService,
            IAppConfigService appConfigService, IHekmatCanceledTransactionService hekmatCanceledTransactionService, IHekmatEffectlessCanceledTransactionService hekmatEffectlessCanceledTransactionService, IHekmatEffectlessModifiedTransactionService hekmatEffectlessModifiedTransactionService, IUserService userService, ApplicationConfiguration applicationConfiguration, IYashilLogger<RefundDetail> logger)
            : base(unitOfWork, refundDetailRepository, userPrincipal, eventPublisher)
        {
            _refundDetailRepository = refundDetailRepository;
            _userPrincipal = userPrincipal;
            _itemGroupService = itemGroupService;
            _appConfigService = appConfigService;
            _hekmatCanceledTransactionService = hekmatCanceledTransactionService;
            _hekmatEffectlessCanceledTransactionService = hekmatEffectlessCanceledTransactionService;
            _hekmatEffectlessModifiedTransactionService = hekmatEffectlessModifiedTransactionService;
            _userService = userService;
            _applicationConfiguration = applicationConfiguration;
            _logger = logger;
            _hekmatModifiedTransactionService = hekmatModifiedTransactionService;
        }

        public IQueryable<RefundDetail> GetByCustomFilter(int? refundType, int? refundStatusType, int? refundId, int? refundGroupId, int? paymentId, int? personBankAccountId, int? orderId)
        {
            return _refundDetailRepository.GetByCustomFilter(refundType, refundStatusType, refundId, refundGroupId, paymentId, personBankAccountId, orderId);
        }

        public IQueryable<RefundDetail> GetByOrderId(int orderId)
        {
            return _refundDetailRepository.GetByOrderId(orderId);
        }

        public Task<bool> ExistsAnyRefundDetailWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _refundDetailRepository.ExistsAnyRefundDetailWithThisItemGroupIdAsync(itemGroupId);
        }

        public Task<bool> ExistsAnyRefundDetailForPayWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _refundDetailRepository.ExistsAnyRefundDetailForPaymentWithThisItemGroupIdAsync(itemGroupId);
        }

        public async Task<bool> AssignItemGroupForRefundDetailForPaymentAndConfirmWithArchiveAsync(int groupId)
        {
            var paymentGroup = await _itemGroupService.GetAsync(groupId, false);
            if (paymentGroup.ItemGroupSubId.HasValue)
            {
                var count = await _refundDetailRepository.AssignItemGroupForRefundDetailForPaymentAndConfirmWithArchiveAsync(paymentGroup.ItemGroupSubId.Value, groupId);
                if (count > 0)
                {
                    paymentGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                    await _itemGroupService.UpdateAsync(paymentGroup, paymentGroup.Id, true);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AssignItemGroupForRefundDetailAndConfirm(int groupId)
        {
            var paymentGroup = await _itemGroupService.GetAsync(groupId, false);
            if (paymentGroup.ItemGroupSubId.HasValue)
            {
                var count = await _refundDetailRepository.AssignItemGroupForRefundDetailAndConfirmAsync(paymentGroup.ItemGroupSubId.Value, groupId, Int32.Parse(paymentGroup.Code));
                if (count > 0)
                {
                    paymentGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                    await _itemGroupService.UpdateAsync(paymentGroup, paymentGroup.Id, true);
                    return true;
                }
            }

            return false;
        }

        public IQueryable<RefundDetail> GetRefundDetailsAssignedToGroup(int groupId, bool readOnly = false)
        {
            return _refundDetailRepository.GetRefundDetailsAssignedToGroup(groupId, readOnly);
        }

        public IQueryable<RefundDetail> GetRefundDetailsForPaymentAssignedToGroup(int groupId, bool readOnly = false)
        {
            return _refundDetailRepository.GetRefundDetailsForPaymentAssignedToGroup(groupId, readOnly);
        }

        public IQueryable<RefundDetail> GetNotGroupedRefundDetails(int paymentMethodId, bool readOnly = false)
        {
            return _refundDetailRepository.GetNotGroupedRefundDetails(paymentMethodId, readOnly);
        }
        /// عودت وجه هایی که گروه بندی نشده باشد 
        /// یا گروه بندی شده باشد ولی مقدار عودت به کاربر آن
        /// NUll
        /// باشد، برای آیتم های گروه جدید ،گروه بندی می شود
        public IQueryable<RefundDetail> GetRefundDetailsForNewGroup(int paymentMethodId, int? yearMonth = null, bool readOnly = false)
        {
            return _refundDetailRepository.GetRefundDetailsForNewGroup(paymentMethodId, yearMonth, readOnly);
        }

        public IQueryable<HekmatCanceledOrModifiedTransactionDomainModel> GetHekmatCanceledOrModifiedTransactionsAsync(string cardNumber, long price)
        {
            return _refundDetailRepository.GetHekmatCanceledOrModifiedTransactionsAsync(cardNumber, price);
        }

        public IQueryable<RefundDetail> GetNotGroupedRefundDetailsForPayment(int paymentMethodId, bool readOnly = false)
        {
            return _refundDetailRepository.GetNotGroupedRefundDetailsForPayment(paymentMethodId, readOnly);
        }

        public bool AllowEdit(int id)
        {
            var paymentMethodType = _refundDetailRepository
                .GetAll(true).Where(x => x.Id == id)
                .Select(x => x.Payment.PaymentMethodType)
                .FirstOrDefault();
            if (paymentMethodType is (int)PaymentMethodTypesEnum.Hekmat)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<int> GetRefundDetailForPaymentGroupReportIdAsync(int groupId)
        {
            return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceRefundDetailForPaymentGroupReportId);
        }

        /// <summary>
        /// لیست عودتی های کل طرح باقرالعلوم بر اساس نوع پرداخت گروه بندی شده
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<int> GetRefundDetailGroupReportIdAsync(int groupId)
        {
            var group = _itemGroupService.Get(groupId, true);
            if (group.ItemGroupSubId == (int)PaymentMethodTypesEnum.Ansar
                || group.ItemGroupSubId == (int)PaymentMethodTypesEnum.Saman
                || group.ItemGroupSubId == (int)PaymentMethodTypesEnum.Samin
                )
            {
                return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceOnlineRefundDetailGroupReportId);
            }

            if (group.ItemGroupSubId == (int)PaymentMethodTypesEnum.Hekmat)
            {
                return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceHekmatRefundDetailGroupReportId);
            }

            if (group.ItemGroupSubId == (int)PaymentMethodTypesEnum.Wallet)
            {
                return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceNajaRefundDetailGroupReportId);
            }

            return -1;
        }

        /// <summary>
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="readOnly"></param>
        /// <param name="filterByAssignee"></param>
        /// <returns></returns>
        public IQueryable<RefundDetail> GetGroupedItems(int? archiveMode, bool readOnly = false)
        {
            return _refundDetailRepository.GetGroupedItems(archiveMode, readOnly);
        }

        public IQueryable<RefundDetail> GetRefundDetailStatistic(int refundDetailId)
        {
            return _refundDetailRepository.GetRefundDetailStatistic(refundDetailId, true);
        }

        public async Task<string> IsValidHekmatRefundDetailForRejectAsync(int refundDetailId)
        {
            var refundDetail = await GetAsync(refundDetailId, true);
            if (refundDetail != null)
            {
                if (refundDetail.Confirmed != null && refundDetail.Confirmed.Value)
                {
                    return RefundDetailIsConfirmedBeforeMessage;
                }
            }
            return null;
        }

        public async Task<string> ConfirmRefundDetailAsync(int refundDetailId, long? announcedRefundAmount)
        {
            var validationMessage = await IsValidHekmatRefundDetailForConfirmAsync(refundDetailId, announcedRefundAmount);
            if (string.IsNullOrEmpty(validationMessage))
            {
                var refundDetail = await GetAsync(refundDetailId, false);
                refundDetail.RefundAmountSetBy = _userPrincipal.Id;
                refundDetail.RefundAmountSetDate = DateTime.Now;
                refundDetail.Confirmed = true;
                //در صورتی که مقدار عودت وجه در فایل حکمت وجود نداشت کاربر می تواند به صورت دستی مقدار را وارد کند
                if (refundDetail.RefundAmount == null)
                {
                    if (announcedRefundAmount != null)
                    {
                        refundDetail.AnnouncedRefundAmount = announcedRefundAmount.Value;
                        refundDetail.RefundAmount = announcedRefundAmount.Value;
                    }
                }

                if (refundDetail.RefundAmount.HasValue)
                {
                    await UpdateAsync(refundDetail, refundDetail.Id, true);
                }
                return null;
            }
            return validationMessage;

        }
        public async Task<string> RejectRefundDetailAsync(int refundDetailId)
        {
            var isValidForConfirmOrRejectMsg = await IsValidHekmatRefundDetailForRejectAsync(refundDetailId);
            if (!string.IsNullOrEmpty(isValidForConfirmOrRejectMsg))
            {
                return isValidForConfirmOrRejectMsg;
            }

            var refundDetail = await GetAsync(refundDetailId, false);
            refundDetail.Confirmed = false;
            refundDetail.RefundAmountSetBy = _userPrincipal.Id;
            refundDetail.RefundAmountSetDate = DateTime.Now;
            await UpdateAsync(refundDetail, refundDetail.Id, true);
            return null;
        }

        /// <summary>
        /// بررسی میکند اطلاعات حساب شخص در عودتی ها وجود دارد یا خیر 
        /// </summary>
        /// <param name="personBankAccountId">اطلاعات حساب شخص</param>
        /// <returns></returns>
        public bool CheckExistAnyRefundDetailByPersonBankAccountId(int personBankAccountId)
        {
            return _refundDetailRepository.CheckExistAnyRefundDetailByPersonBankAccountId(personBankAccountId);
        }

        public Task<int> UpdateHekmatRefundDetailRefundAmountAsync()
        {
            return _refundDetailRepository.UpdateHekmatRefundDetailRefundAmountAsync();
        }

        public async Task<string> IsValidHekmatRefundDetailForConfirmAsync(int id, long? announcedRefundAmount)
        {

            var refundDetail = await GetAsync(id, true);
            if (refundDetail != null)
            {
                if (refundDetail.Confirmed != null && refundDetail.Confirmed.Value)
                {
                    return RefundDetailIsConfirmedBeforeMessage;
                }
                if (refundDetail.RefundType == (int)RefundTypeEnum.Hekmat)
                {
                    if (refundDetail.RefundAmount != null)
                    {
                        var hekmatCanceledTransaction = await _hekmatCanceledTransactionService.GetByRefundDetailIdAsync(refundDetail.Id);
                        if (hekmatCanceledTransaction != null)
                        {
                            if (hekmatCanceledTransaction.RefundPrice != refundDetail.RefundAmount)
                            {
                                return WrongRefundAmountMessage;
                            }

                            return null;
                        }
                        var hekmatEffectLessCanceledTransaction =
                            await _hekmatEffectlessCanceledTransactionService
                                .GetByRefundDetailIdAsync(refundDetail.Id);
                        if (hekmatEffectLessCanceledTransaction != null)
                        {
                            if (refundDetail.RefundAmount != 0)
                            {
                                return WrongRefundAmountMessage;
                            }

                            return null;
                        }

                        var hekmatEffectLessModifiedTransaction =
                           await _hekmatEffectlessModifiedTransactionService.GetByRefundDetailIdAsync(refundDetail.Id);
                        if (hekmatEffectLessModifiedTransaction != null)
                        {
                            if (refundDetail.RefundAmount != 0)
                            {
                                return WrongRefundAmountMessage;
                            }
                            return null;
                        }
                        var hekmatModifiedTransaction =
                            await _hekmatModifiedTransactionService.GetByRefundDetailIdAsync(refundDetail.Id);
                        if (hekmatModifiedTransaction != null)
                        {
                            if (refundDetail.RefundAmount != hekmatModifiedTransaction.RefundPrice)
                            {
                                return WrongRefundAmountMessage;
                            }
                            return null;
                        }

                        return "کاربر گرامی مقدار عودت وجه ثبت شده برای این تراکنش در " +
                               "عودت وجه های اعلامی حکمت یافت نشد، خواهشمند است با واحد فنی تماس بگیرید";
                    }
                    if (announcedRefundAmount != null)
                    {
                        return null;
                    }
                    return "کاربر گرامی لطفا مبلغ عودت را وارد نمایید.";
                }
                return "کاربر گرامی این تراکنش از نوع حکمت نمی باشد";
            }
            return RefundDetailNotFoundMessage;
        }
        /// <summary>
        /// رکورد عودت وجه در صورتی که تعیین تکلیف نشده باشد یعنی 
        /// بعد از بارگذاری سی دی تراکنش های عودتی، مبلغ عودت به کاربر آن مقدار نگیرد
        ///  .و کارشناس مربوطه نیز مقدار عودتی به کاربر را از سی دی پیدا نکند تا از طریق فرم وارد کند،
        ///  مجددا این رکورد را در عودتی های ماه جدید به حکمت ارسال میکند
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        public async Task<string> SendRefundDetailToNewGroupAsync(int refundDetailId)
        {
            var res = await _refundDetailRepository.SendRefundDetailToNewGroupAsync(refundDetailId);
            if (res > 0)
            {
                return RefundDetailAfterSendToNewGroupMessage;
            }
            return null;

        }
        /// <summary>
        /// بررسی می کند برای هر پرداخت  قیمت جدید وجود دارد  
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public bool ChecksThereAreNewPriceByPaymentId(int paymentId)
        {
            return GetByCustomFilter((int)RefundTypeEnum.Hekmat, (int)RefundStatusTypeEnum.Done,
                           null, null, paymentId, null, null).Any(rd => rd.NewPaymentAmount > 0);

        }
        /// <summary>
        /// بررسی می کند که آیا عودت وجه مربوط به رکورد تغییر آیتم سفارش گروه بندی شده است
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        public async Task<bool> IsGroupedRefundDetail(int modifiedOrderItemId)
        {
            return await _refundDetailRepository.IsGroupedRefundDetail(modifiedOrderItemId);
        }

        public async Task<string> ArchiveRefundDetailAsync(int refundDetailId, string archiveDescription)
        {
            // دریافت عودت وجه
            var refundDetail = await GetAsync(refundDetailId, false);
            if (refundDetail == null)
            {
                return "کاربر گرامی چنین عودتی وجود ندارد";
            }
            // ذخیره اطلاعات مربوط به کاربری که بایگانی میکند
            refundDetail.ArchiveBy = _userPrincipal.Id;
            // ذخیره تاریخ بایگانی
            refundDetail.ArchiveDate = DateTime.Now;
            refundDetail.IsArchived = true;
            if (!string.IsNullOrEmpty(archiveDescription))
            {
                refundDetail.ArchiveDescription = archiveDescription;
            }

            try
            {
                await UpdateAsync(refundDetail, refundDetail.Id, true);
                return string.Empty;
            }
            catch
            {
                return "خطا در بایگانی عودت وجه";
            }
        }

        public async Task<string> UnArchiveRefundDetailAsync(int refundDetailId)
        {
            // دریافت عودت وجه
            var refundDetail = await GetAsync(refundDetailId, false);
            if (refundDetail == null)
            {
                return "کاربر گرامی چنین عودتی وجود ندارد";
            }
            // اگر آرشیو نشده باشد خطا میدهیم
            if (!refundDetail.IsArchived)
            {
                return "کاربر گرامی این عودت وجه هنوز بایگانی نشده است";
            }

            refundDetail.IsArchived = false;

            try
            {
                await UpdateAsync(refundDetail, refundDetail.Id, true);
                return string.Empty;
            }
            catch
            {
                return "خطا در حذف از بایگانی";
            }
        }

        public IQueryable<RefundDetail> GetAllGroupedItems(int? archiveMode, bool readOnly = false)
        {
            return _refundDetailRepository.GetAllGroupedItems(archiveMode, readOnly);
        }

        public async Task<string> AssignRefundDetailAsync(int refundDetailId, int? assigneeId)
        {
            var refundDetail = await GetAsync(refundDetailId, false);
            if (refundDetail == null)
            {
                return "کاربر گرامی عودت وجه مورد نظر یافت نشد";
            }
            if (!assigneeId.HasValue)
            {
                return "کاربر گرامی برای انتصاب لطفا یک کاربر را انتخاب نمایید.";
            }

            var userHasCrmRole = await _userService.CheckUserHasRoleAsync(assigneeId.Value, _applicationConfiguration.CrmUserRole, null);
            if (!userHasCrmRole)
            {
                return "کاربر گرامی کاربر انتخاب شده صحیح نمیباشد لطفا کاربر دیگری را انتخاب نمایید.";
            }
            refundDetail.Assignee = assigneeId.Value;
            try
            {
                await UpdateAsync(refundDetail, refundDetail.Id, true);
                return string.Empty;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(RefundDetail), "Error in AssignRefundDetailAsync", e);
                return "کاربر گرامی بروزرسانی با خطا مواجه شد";
            }

        }
        /// <summary>
        /// اطلاعات کامل عودت وجه ها را بر اساس فیلتر ارسالی پارامتر ها نمایش می دهد
        /// </summary>
        /// <param name="showGrouped">نمایش گروه بندی شده </param>
        /// <param name="showPaymentGrouped">نمایش گروه بندی برای پرداخت شده</param>
        /// <param name="showHasBankAccount">نمایش دارای اطلاعات حساب</param>
        /// <param name="refundCreationDateFromDate">تاریخ شروع ثبت عودت</param>
        /// <param name="refundCreationDateToDate">تاریخ پایان ثبت عودت</param>`
        /// <returns></returns>
        public IQueryable<RefundDetail> GetRefundDetailFullInfo(int refundCreationDateFromDate, int refundCreationDateToDate, bool? showGrouped,
            bool? showPaymentGrouped, bool? showHasBankAccount)
        {
            return _refundDetailRepository.GetRefundDetailFullInfo(_applicationConfiguration.ApplicationId
                , refundCreationDateFromDate, refundCreationDateToDate,
                showGrouped, showPaymentGrouped, showHasBankAccount);
        }
        /// <summary>
        /// نام کاربرانی که عودت وجه به انها تخصیص داده و تعداد عودت اختصاص داده شده  را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefundAssignedDomainModel> GetAssignedUsers()
        {
            return _refundDetailRepository.GetAssignedUsers(_applicationConfiguration.ApplicationId);
        }

        public IQueryable<RefundDetail> FindById(int id, bool readOnly = true)
        {
            return _refundDetailRepository.FindById(id, readOnly);
        }

        public async Task<(bool isSuccess, string message)> RemovePersonBankAccountFromRefundDetailAsync(
            int refundDetailId)
        {
            var refundDetail = await
                FindById(refundDetailId, false)
                    .FirstOrDefaultAsync();
            if (refundDetail == null)
            {
                return (false, "کاربر گرامی، اطلاعات ارسال شده نامعتبر می باشد.");
            }

            if (refundDetail.RefundDetailForPaymentGroupId.HasValue)
            {
                return
                    (false, "کاربر گرامی،با توجه به اینکه رکورد عودت وجه برای پرداخت گروه بندی شده است لذا امکان حذف اطلاعات حساب وجود ندارد.");
            }

            refundDetail.PersonBankAccountId = null;
            var refundDetailUpdated =
                await UpdateAsync(refundDetail, refundDetail.Id, true);
            if (refundDetailUpdated != null)
            {
                return (true, "کاربر گرامی، اطلاعات حساب بانکی از رکورد عودت وجه انتخابی حذف شد.");
            }
            return
                (false, "کاربر گرامی، خطایی در حذف اطلاعات حساب از رکورد عودت وجه انتخابی رخ داده است.");
        }

        public IQueryable<RefundDetail> GetRefundDetailsbyFactorId(int factorId)
        {
            return _refundDetailRepository.GetRefundDetailsbyFactorId(factorId);
        }

    }
}
