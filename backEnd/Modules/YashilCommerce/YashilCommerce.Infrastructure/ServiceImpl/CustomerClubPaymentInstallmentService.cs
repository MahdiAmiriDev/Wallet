using System;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;
using Yashil.Core.Enums;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CustomerClubPaymentInstallmentService : GenericService<CustomerClubPaymentInstallment, int>, ICustomerClubPaymentInstallmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerClubPaymentInstallmentRepository _customerClubPaymentInstallmentRepository;
        private readonly IItemGroupService _itemGroupService;
        public CustomerClubPaymentInstallmentService(IUnitOfWork unitOfWork, ICustomerClubPaymentInstallmentRepository customerClubPaymentInstallmentRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IItemGroupService itemGroupService)
            : base(unitOfWork, customerClubPaymentInstallmentRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _customerClubPaymentInstallmentRepository = customerClubPaymentInstallmentRepository;
            _itemGroupService = itemGroupService;
        }

        public async Task DeleteByPaymentIdAsync(int paymentId, bool saveAfterDelete)
        {
            await _customerClubPaymentInstallmentRepository.DeleteByPaymentIdAsync(paymentId, saveAfterDelete);
            if (saveAfterDelete)
            {
                await SaveChangeAsync();
            }
        }

        public async Task DeleteRecordsAfterYearMonthByPaymentIdAsync(int paymentId, int currentYearAndMonth, bool saveAfterDelete)
        {
            await _customerClubPaymentInstallmentRepository.DeleteRecordsAfterYearMonthByPaymentIdAsync(paymentId, currentYearAndMonth, saveAfterDelete);
            if (saveAfterDelete)
            {
                await SaveChangeAsync();
            }
        }
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
        public async Task DeleteRecordsNotSentForDemandByPaymentIdAsync(int paymentId, bool saveAfterDelete)
        {
            await _customerClubPaymentInstallmentRepository.DeleteRecordsNotSentForDemandByPaymentIdAsync(paymentId);
            if (saveAfterDelete)
            {
                await SaveChangeAsync();
            }
        }

        public Task<bool> ExistsAnyCustomerClubPaymentInstallmentWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _customerClubPaymentInstallmentRepository.ExistsAnyCustomerClubPaymentInstallmentWithThisItemGroupIdAsync(itemGroupId);
        }

        public IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsAssignedToDemandGroup(int groupId)
        {
            return _customerClubPaymentInstallmentRepository
                .GetCustomerClubPaymentInstallmentsAssignedToDemandGroup(groupId);
        }

        public IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(int itemGroupSubId, bool readOnly)
        {
            return _customerClubPaymentInstallmentRepository
                .GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(itemGroupSubId, readOnly);
        }

        public async Task<bool> AssignAndConfirmCustomerClubPaymentInstallmentDemandGroup(int groupId)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var demandGroup = await _itemGroupService.GetAsync(groupId, false);
            try
            {
                if (demandGroup.ItemGroupSubId != null)
                {
                    int count = await _customerClubPaymentInstallmentRepository.AssignCustomerClubPaymentInstallmentDemandGroup(demandGroup.ItemGroupSubId.Value, groupId);
                    int oldCountUpdate = await _customerClubPaymentInstallmentRepository.AssignCustomerClubPaymentInstallmentOldSiteDemandGroup(demandGroup.ItemGroupSubId.Value, groupId);
                    if (count + oldCountUpdate > 0)
                    {
                        demandGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                        await _itemGroupService.UpdateAsync(demandGroup, demandGroup.Id, true);
                        await transaction.CommitAsync();

                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw ;
            }
            return false;
        }

        public IQueryable<CustomerClubPaymentInstallment> GetByCustomFilter(int? paymentId, int? yearMonth)
        {
            return _customerClubPaymentInstallmentRepository.GetByCustomFilter(paymentId, yearMonth);
        }


    }
}
