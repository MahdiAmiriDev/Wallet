using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.CustomDomainModels;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using YashilPayment.Core.Enums;
using YashilPayment.Core.DTO;
using Yashil.Core.Enums;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class RefundPaymentTransactionItemRequestService : GenericService<RefundPaymentTransactionItemRequest, int>,
        IRefundPaymentTransactionItemRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefundPaymentTransactionItemRequestRepository _refundPaymentTransactionItemRequestRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IHekmatPaymentGatewayTransactionService _hekmatPaymentGatewayTransactionService;
        private readonly IPaymentTransactionItemRefundService _paymentTransactionItemRefundService;


        public RefundPaymentTransactionItemRequestService(IUnitOfWork unitOfWork,
            IRefundPaymentTransactionItemRequestRepository refundPaymentTransactionItemRequestRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IHekmatPaymentGatewayTransactionService hekmatPaymentGatewayTransactionService, IPaymentTransactionItemRefundService paymentTransactionItemRefundService)
            : base(unitOfWork, refundPaymentTransactionItemRequestRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _refundPaymentTransactionItemRequestRepository = refundPaymentTransactionItemRequestRepository;
            _userPrincipal = userPrincipal;
            _hekmatPaymentGatewayTransactionService = hekmatPaymentGatewayTransactionService;
            _paymentTransactionItemRefundService = paymentTransactionItemRefundService;
        }


        public IQueryable<RefundPaymentTransactionItemRequest> GetByCustomFilter(int? parentId,
            int? yashilHekmatTerminalId, int? refundPaymentTransactionItemRequestType,
            int? refundPaymentTransactionItemRequestStatus)
        {
            return _refundPaymentTransactionItemRequestRepository.GetByCustomFilter(parentId, yashilHekmatTerminalId,
                refundPaymentTransactionItemRequestType, refundPaymentTransactionItemRequestStatus);
        }

        public IQueryable<IsiBookCompleteOrderDomainModel> GetIsiBookCompleteOrder(int vendorId, int startDateInt,
            int endDateInt)
        {
            return _refundPaymentTransactionItemRequestRepository.GetIsiBookCompleteOrder(vendorId, startDateInt,
                endDateInt);
        }

        /// <summary>
        /// این متد جهت ایجاد یک رکورد جدید در جدول
        /// RefundPaymentTransactionItemRequest
        /// کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public Task AddRefundPaymentTransactionItemRequestAsync(AddRefundPaymentTransactionItemRequestDto dto)
        {
            return AddAsync(new RefundPaymentTransactionItemRequest()
            {
                YashilHekmatTerminalId = dto.TerminalId,
                ReservationNumber = dto.ReservationNumber,
                RefundPaymentTransactionItemRequestStatus =
                    (int)RefundPaymentTransactionItemRequestStatusEnum.InitialRequestBySupplier,
                RefundPaymentTransactionItemRequestType = (int)RefundPaymentTransactionItemRequestTypeEnum.Cancel,
                RejectRreason = dto.RejectReason,
                CreateBy = _userPrincipal.Id,
                CreationDate = DateTime.Now,
                Deleted = false,
                CreatorOrganizationId = 1
            });
        }

        public Task<bool> HasUserInsertRecordBeforeFroPaymentTransactionAsync(int yashilHekmatTerminalId, string reservationNumber)
        {
            return GetAll(true)
                   .AnyAsync(x => x.YashilHekmatTerminalId == yashilHekmatTerminalId &&
                                  x.ReservationNumber == reservationNumber);
        }

        public async Task<(bool isSucces, string message)> ApplyRefundPaymentTransactionItemRequestActionAsync(int paymentTransactionItemRefundRequestId, bool? rejectRequest)
        {
            var refundPaymentTransactionItemRequest = await GetAsync(paymentTransactionItemRefundRequestId, false);
            if (refundPaymentTransactionItemRequest == null)
            {
                return (false, "رکورد درخواست عودت تراکنش یافت نشد");
            }
            if (refundPaymentTransactionItemRequest.RefundPaymentTransactionItemRequestStatus !=
                (int)RefundPaymentTransactionItemRequestStatusEnum.InitialRequestBySupplier)
            {
                return (false, "این درخواست قبلا تکمیل شده است و شما مجاز به تکرار آن نمی باشید");
            }
            // درخواست کاربر رد می شود 
            if (rejectRequest.HasValue && rejectRequest.Value)
            {
                refundPaymentTransactionItemRequest.RefundPaymentTransactionItemRequestStatus = (int)
                    RefundPaymentTransactionItemRequestStatusEnum.RejectByCrm;
                await UpdateAsync(refundPaymentTransactionItemRequest, refundPaymentTransactionItemRequest.Id, true);
                return (true, string.Empty);
            }
            else
            {
                //TODO برای درخواست هایی از جنس ویرایش تراکنش باید درخواست های عودت قبلی بررسی و سپس اجازه ویرایش داده شود - برای توسعه ویرایش تراکنش پرداخت 
                
                // کاربر درخواست را تایید کرده و دخواست از جنس لغو می باشد
                if (refundPaymentTransactionItemRequest.RefundPaymentTransactionItemRequestType ==
                    (int)RefundPaymentTransactionItemRequestTypeEnum.Cancel)
                {
                    var transaction = await _unitOfWork.BeginTransactionAsync();
                    try
                    {
                        // ابتدا تراکنش درگاه حکمت را باید به وضعیت لغو تغییر بدهیم
                        var refundedHekmatTransaction = await _hekmatPaymentGatewayTransactionService.RefundHekmatTransactionAsync(
                               refundPaymentTransactionItemRequest.YashilHekmatTerminalId,
                               refundPaymentTransactionItemRequest.ReservationNumber);
                        if (!refundedHekmatTransaction.isSucces)
                        {
                            await transaction.RollbackAsync();
                            return (false, "کاربر گرامی تراکنش پرداخت حکمت برای این درخواست یافت نشد.  .");
                        }
                        else
                        {
                            // سپس باید سفارش مربوط به پرداخت تراکنش را لغو کنیم یا از طریق  
                            var terminalPaymentTransactionRefundFailMessage = await RefundTerminalPaymentTransaction(
                                  refundPaymentTransactionItemRequest.YashilHekmatTerminalId,
                                  refundPaymentTransactionItemRequest.ReservationNumber);

                            if (string.IsNullOrEmpty(terminalPaymentTransactionRefundFailMessage))
                            {
                                // در نهایت برای این تراکنش حکمت عودت وجه ثبت می کنیم

                                #region Update  RefundPaymentTransactionItemRequestStatus To AcceptByCrm

                                refundPaymentTransactionItemRequest.RefundPaymentTransactionItemRequestStatus =
                                    (int)RefundPaymentTransactionItemRequestStatusEnum.AcceptByCrm;

                                await UpdateAsync(refundPaymentTransactionItemRequest,
                                    refundPaymentTransactionItemRequest.Id, true);

                                #endregion
                                
                                #region  Add PaymentTransactionItemRefund
                                var hekmatTransaction = await _hekmatPaymentGatewayTransactionService.GetByReservationNumberAndTerminalIdAsync(
                                    refundPaymentTransactionItemRequest.ReservationNumber,
                                    refundPaymentTransactionItemRequest.YashilHekmatTerminalId);

                                var paymentTransactionItemRefund =
                                    _paymentTransactionItemRefundService.CreateNewPaymentTransactionItemRefundEntity(
                                        paymentTransactionItemRefundRequestId,
                                        refundPaymentTransactionItemRequest.YashilHekmatTerminalId,
                                        hekmatTransaction.Price,
                                        RefundTypeEnum.Hekmat);

                                await _paymentTransactionItemRefundService.AddAsync(paymentTransactionItemRefund);


                                #endregion

                                await transaction.CommitAsync();
                                return (true, string.Empty);
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return (false, "کاربر گرامی خطایی در لغو تراکنش وجود دارد.");
                            }
                            

                        }
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return (false, "کاربر گرامی خطایی در لغو تراکنش وجود دارد.");
                    }
                }
                else
                {
                    return (false, "امکان درخواست شما وجود ندارد.");
                }
            }

        }
        /// <summary>
        /// تراکنش پرداخت یا سفارش مربوط به ترمینال پذیرنده را عودت می زند
        /// </summary>
        /// <param name="yashilHekmatTerminalId">شناسه ترمینال</param>
        /// <param name="reservationNumber">شناسه پذرنده</param>
        /// <returns></returns>
        public Task<string> RefundTerminalPaymentTransaction(int yashilHekmatTerminalId, string reservationNumber)
        {
            // چون دیتا بیس پروژه ایزی بوک در کنار دریتا بیس پروژه یاشیل می باشد پس با 
            // sp 
            // این فرآیند را انجام می دهیم
            return _refundPaymentTransactionItemRequestRepository
                .RefundTerminalPaymentTransaction(
                yashilHekmatTerminalId, reservationNumber);
        }

        public IQueryable<RefundPaymentTransactionItemRequest> GetInitialRegistrationPaymentTransactionItemRequestsForActions()
        {
            return GetPaymentTransactionItemRefundRequestsByStatus(RefundPaymentTransactionItemRequestStatusEnum
                .InitialRequestBySupplier);
        }

        public IQueryable<RefundPaymentTransactionItemRequest> GetPaymentTransactionItemRefundRequestsByStatus(
            RefundPaymentTransactionItemRequestStatusEnum refundPaymentTransactionItemRequestStatus,
            bool readOnly = true)
        {
            return GetAll(readOnly).Where(x =>
                x.RefundPaymentTransactionItemRequestStatus == (int)refundPaymentTransactionItemRequestStatus);
        }
        
    }
}
