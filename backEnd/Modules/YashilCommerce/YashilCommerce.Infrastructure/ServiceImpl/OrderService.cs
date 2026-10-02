using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Configurations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Dto;
using YashilUserManagement.Core.Services;


namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class OrderService : GenericService<Order, int>, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IRecipientService _recipient;
        private readonly ICustomerClubService _customerClubService;
        private readonly IPaymentService _paymentService;
        private readonly IYashilLogger<Order> _logger;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ISubsidyService _subsidyService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IOrderItemService _orderItemService;
        private readonly IVerifySmsService _verifySmsService;
        private readonly ISmsTemplatesConfiguration _smsTemplatesConfiguration;
        private readonly IVerifySmsServiceConfiguration _verifySmsServiceConfiguration;
        private readonly IActivityLogService _activityLogService;


        public OrderService(
            IUnitOfWork unitOfWork,
            IRecipientService recipient,
            IOrderRepository orderRepository, IUserPrincipal userPrincipal,
            ISaleConfigurationService saleConfigurationService,
            IShoppingCartItemService shoppingCartItemService,
            ICustomerClubService customerClubService, IPaymentService paymentService,
            IYashilLogger<Order> logger,
            IEventPublisher eventPublisher,
            IOrderItemService orderItemService,
            IProductSalesPlanningService productSalesPlanningService,
             ISubsidyService subsidyService,
            StoreConfiguration storeConfiguration,
            ApplicationConfiguration applicationConfiguration,
            IAdditionalUserPropService additionalUserPropService,
            ISmsTemplatesConfiguration smsTemplatesConfiguration, IVerifySmsService verifySmsService,
            IVerifySmsServiceConfiguration verifySmsServiceConfiguration, IActivityLogService activityLogService)
            : base(unitOfWork, orderRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _userPrincipal = userPrincipal;
            _saleConfigurationService = saleConfigurationService;
            _shoppingCartItemService = shoppingCartItemService;
            _recipient = recipient;
            _customerClubService = customerClubService;
            _paymentService = paymentService;
            _logger = logger;
            _subsidyService = subsidyService;
            _storeConfiguration = storeConfiguration;
            _applicationConfiguration = applicationConfiguration;
            _additionalUserPropService = additionalUserPropService;
            _smsTemplatesConfiguration = smsTemplatesConfiguration;
            _verifySmsService = verifySmsService;
            _verifySmsServiceConfiguration = verifySmsServiceConfiguration;
            _productSalesPlanningService = productSalesPlanningService;
            _orderItemService = orderItemService;
            _activityLogService = activityLogService;
        }
        public async Task<ResultStatus<int>> ConfirmSubsidyBySata(int orderId)
        {
            var order = Get(orderId);
            if (order == null)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "چنین سفارشی وجود ندارد"
                };
            }

            // آیا کد ملی خریدار میتواند یارانه بگیرد یا خیر ؟؟
            var sataSubsidyApprovalResult = await _subsidyService.TrySataSubsidyApprovalAsync(order.NationalCode);
            // اگر نتیحه چک کردن یارانه ساتا True برگشت
            if (sataSubsidyApprovalResult.CanCheck)
            {
                order.SubsidyStatusType = sataSubsidyApprovalResult.Result ? (int)SubsidyStatusTypesEnum.Confirmed : (int)SubsidyStatusTypesEnum.Rejected;
                var result = UpdateAsync(order, order.Id, true);
                if (result != null)
                {
                    if (order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = true,
                            Message = "کاربر گرامی یارانه تایید شد"
                        };
                    }
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی یارانه رد شد"
                    };

                }
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در بروزرسانی "
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در استعلام یارانه از ساتا"
            };
        }
        public IQueryable<Order> GetByCustomFilter(int? userId, int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType, int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId, int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType, int? companyId, int? representationId, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
            return _orderRepository.GetByCustomFilter(userId, billingAddressId, shippingAddressId, recipientId, orderStatusType, shippingStatusType, paymentStatusType, paymentMethodType, rewardPointsHistoryEntryId, affiliateId, salesPlanningId, subsidyId, subsidyStatusType, subsidyType, companyId, representationId, orderCreationDateFromDate, orderCreationDateToDate);
        }

        public async Task<int?> GetShoppingCartItemsMinInstallmentCountAsync(decimal total, Guid shoppingCartCode)
        {
            var customerClub = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            if (!customerClub.InstallmentBased)
            {
                return null;
            }

            var allShoppingCartSaleConfigurationAttributeDtoList =
                await _saleConfigurationService.GetSaleConfigurationAttributeByShoppingCartCodeAsync(shoppingCartCode,
                    _applicationConfiguration.ApplicationId);

            var allSaleConfigurationsAllowedToPayInInstallment = allShoppingCartSaleConfigurationAttributeDtoList
                .All(x => x.AllowedToPayInInstallment);

            if (allSaleConfigurationsAllowedToPayInInstallment)
            {
                return await _saleConfigurationService.GetSaleConfigurationsInstallmentCountAsync(total, allShoppingCartSaleConfigurationAttributeDtoList);
            }

            return null;
        }


        public async Task<Order> AddOrderAndClearShoppingCartAsync(Order order, int recipientId, Guid guidCode)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //:TODO Change Order Address Save Algorithm
                _shoppingCartItemService.ClearShoppingCart((int)ShoppingCartTypeEnum.ShoppingCart, guidCode, false);
                var address = _recipient.CloneRecipientAddress(recipientId);
                order.ShippingAddress = address;
                var addedOrder = await AddAsync(order, true);
                addedOrder.Title = _storeConfiguration.OrderReferenceNumberPrefix + "-" + addedOrder.Id;
                var updatedOrder = await UpdateAsync(addedOrder, addedOrder.Id, true);
                await transaction.CommitAsync();
                return updatedOrder;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, new EventId(1, Events.Insert.ToString()), nameof(Order), "Insert Record method AddOrderAndClearShoppingCart", e);
                return null;
            }

        }

        public IQueryable<Order> GetUserOrders(int applicationId)
        {
            return _orderRepository.GetUserOrders(applicationId);
        }



        /// <summary>
        /// تب سفارشات جاری : آخرین سفارش ثبت شده کاربر را نمایش میدهد 
        ///  سفارش هایی که" منتظر تایید تامین کننده" میباشد
        /// و سفارش هایی که منتظر تایید یارانه می باشند
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCurrentOrders(int applicationId)
        {
            return _orderRepository.GetCurrentOrders(applicationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus">وضعیت جدید سفارش</param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatusTypesEnum orderStatus,
            bool saveAfterUpdate = false)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment
                  && orderStatus != OrderStatusTypesEnum.Complete
                  && orderStatus != OrderStatusTypesEnum.Processing)
            {
                //فقط در صورتی وضعیت آن تغییر می کند که 
                //در صورتی که وضعیت تراکنش پرداخت ناقص باشد،
                return order;
            }

            if (order.OrderStatusType == (int)OrderStatusTypesEnum.Complete &&
                orderStatus != OrderStatusTypesEnum.IncompletePayment &&
                orderStatus != OrderStatusTypesEnum.Refund)
            {
                /*
                 *فقط در صورتی می توان سفارش کامل را تغییر وضعیت داد که وضعیت جدید
                 * پرداخت ناقص یا انصراف بعد از تایید نهایی باشد
                 */
                return order;
            }
            order.ModificationDate = DateTime.Now;
            order.ModifyBy = _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id;
            order.OrderStatusType = (int)orderStatus;



            return await UpdateAsync(order, order.Id, saveAfterUpdate);

        }

        public async Task<decimal?> GetOrderTotalAsync(SubsidyStatusTypesEnum subsidyStatusType, decimal? totalInclSubsidy, decimal? totalExclSubsidy)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            if (userAdditionalUserProp.SupportSubsidy)
            {
                if (subsidyStatusType == SubsidyStatusTypesEnum.Confirmed || subsidyStatusType == SubsidyStatusTypesEnum.InitialRegistration)
                {
                    return totalInclSubsidy;
                }
                else
                {
                    return totalExclSubsidy;
                }
            }
            else
            {
                return totalExclSubsidy;
            }

        }

        public IQueryable<OrderItem> GetCurrentOrderItems(int applicationId)
        {
            return _orderRepository.GetCurrentOrderItems(applicationId);
        }

        /// <summary>
        /// get order with Recipient dependency
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetOrderIncludeAddress(int orderId)
        {
            return _orderRepository.GetOrderIncludeAddress(orderId);
        }


        /// <summary>
        ///   if order was subsidy based it must be in state confirmed or rejected subsidy
        ///   if not just checking the state of payment
        ///   only in waiting, allowed, and rejected payment order allowed to pay
        /// </summary>
        /// <param name="orderStatusTypes"></param>
        /// <param name="subsidyStatusType"></param>
        /// <returns></returns>
        public bool CheckOrderAllowPay(OrderStatusTypesEnum orderStatusTypes, SubsidyStatusTypesEnum? subsidyStatusType)
        {
            if (orderStatusTypes == OrderStatusTypesEnum.Processing)
            {
                //Change Status Before Check
                //TODO:Check Payment Last Status Time Threshold For change Order Status From Processing To Complete or UnsuccessfulPayment
            }
            if (orderStatusTypes == OrderStatusTypesEnum.CancelBySystem ||
                orderStatusTypes == OrderStatusTypesEnum.Canceled ||
                orderStatusTypes == OrderStatusTypesEnum.Complete ||
                orderStatusTypes == OrderStatusTypesEnum.Refund ||
                orderStatusTypes == OrderStatusTypesEnum.Processing ||
                orderStatusTypes == OrderStatusTypesEnum.WaitingForSupplierApprovalBeforeEditOrder ||
                orderStatusTypes == OrderStatusTypesEnum.WaitingForSupplierApprovalBeforereRefundOrder)
            {
                return false;
            }
            /*
                InitialRegistration
                UnsuccessfulPayment
                IncompletePayment
            */
            // سفارش مبتنی بر یارانه میباشد
            if (subsidyStatusType != null)
            {
                // تکلیف یارانه مشخص شده است میتواند پرداخت کند
                if (subsidyStatusType == SubsidyStatusTypesEnum.Confirmed || subsidyStatusType == SubsidyStatusTypesEnum.Rejected)
                {
                    return true;
                }
                //تکلیف یارانه مشخص نشده است و نمیتواند پرداخت کند
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// آیا سفارش مجوز انصراف را دارد یا خیر
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        public bool CheckOrderAllowCancel(int orderId, OrderStatusTypesEnum orderStatusType)
        {
            if (orderStatusType != OrderStatusTypesEnum.Canceled && orderStatusType != OrderStatusTypesEnum.CancelBySystem)
            {
                var payments = _paymentService.GetPaymentsByOrderId(orderId).Select(x => new { x.Id, x.PaymentStatusType }).ToList();
                if (payments.Count() == 0 || payments.All(x => x.PaymentStatusType == (int)PaymentStatusTypesEnum.Cancel
                    || x.PaymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// آیا سفارش مجوز مرجوع شدن (عودت وجه سفارش) را دارد یا خیر
        /// </summary>
        /// <param name="orderStatusTypes"></param>
        /// <param name="allowRefund"></param>
        /// <returns></returns>
        public bool CheckOrderAllowRefund(OrderStatusTypesEnum orderStatusTypes, bool allowRefund)
        {
            if (!allowRefund)
            {
                return false;
            }
            //   سفارش کامل با پرداخت موفق یا در وضعیت پرداخت ناقص
            if ((orderStatusTypes == OrderStatusTypesEnum.Complete || orderStatusTypes == OrderStatusTypesEnum.IncompletePayment))
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// آیا سفارش مجوز تغییر را دارد یا خیر
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="lastOrderId"></param>
        /// <param name="allowEdit"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        public bool CheckOrderAllowEdit(int orderId, int? lastOrderId, bool allowEdit, OrderStatusTypesEnum orderStatusType)
        {
            if (!allowEdit)
            {
                return false;
            }
            var payments = _paymentService.GetPaymentsByOrderId(orderId).Select(x => new { x.Id, x.PaymentStatusType }).ToList();
            if ((!lastOrderId.HasValue && orderStatusType == OrderStatusTypesEnum.Complete)
                || (payments.Any(x => x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid) &&
                    (orderStatusType == OrderStatusTypesEnum.WaitingForSupplierApprovalBeforeEditOrder
                    || orderStatusType == OrderStatusTypesEnum.IncompletePayment)))
            {
                return true;
            }

            return false;
        }

        public async Task<AmountForPayment> CalcAmountForPayAsync(int paymentMethodType, int orderId)
        {
            var amountForPayment = new AmountForPayment();
            // درگاه پرداخت حکمت و ناجا نوع فروش اقساطی می شود
            // در گاه پرداخت ثمین نوع فروش نقدی اقساطی 
            // در غیر اینصورت نوع فروش نقدی
            var saleType = paymentMethodType == (int)PaymentMethodTypesEnum.Hekmat || paymentMethodType == (int)PaymentMethodTypesEnum.Wallet ? SaleTypesEnum.InstallmentPayment : (paymentMethodType == (int)PaymentMethodTypesEnum.Samin || paymentMethodType == (int)PaymentMethodTypesEnum.Hybrid) ? SaleTypesEnum.CashPaymentAndInstallmentPayment : SaleTypesEnum.CashPayment;
            // نوع فروش اگر اقساطی یا نقدی اقساطی باشد 
            // installmentBased  می باشد
            var installmentBased = saleType == SaleTypesEnum.InstallmentPayment ||
                                   saleType == SaleTypesEnum.CashPaymentAndInstallmentPayment;

            var order = _orderRepository.GetAllByApplicationId(true, _applicationConfiguration.ApplicationId)
                .Where(x => x.Id == orderId).Select(x =>
                    new OrderSimpleModel(
                        x.CustomerClubCardId,
                        x.SubsidyStatusType,
                        x.TotalInclSubsidy,
                        x.TotalExclSubsidy,
                        x.OrderStatusType,
                        x.UserId
                    )).FirstOrDefault();
            if (order != null && order.OrderStatusType != (int)OrderStatusTypesEnum.Complete)
            {

                var sumPaidTotal = _paymentService.GetOrderTotalPayment(orderId, PaymentStatusTypesEnum.Paid);

                if (order.TotalInclSubsidy.HasValue)
                {
                    order.TotalInclSubsidy -= sumPaidTotal;
                }

                if (order.TotalExclSubsidy.HasValue)
                {
                    order.TotalExclSubsidy -= sumPaidTotal;
                }

                int? installmentCountInclSubsidy = null;
                int? installmentCountExclSubsidy = null;

                if (installmentBased) //TODO :InstallmentBased
                {
                    installmentCountExclSubsidy =
                        await GetOrderInstallmentCountAsync(order.TotalExclSubsidy, orderId, order.UserId);
                    if (_storeConfiguration.SubsidyBased)
                        installmentCountInclSubsidy =
                            await GetOrderInstallmentCountAsync(order.TotalInclSubsidy, orderId, order.UserId);
                }

                if (_storeConfiguration.SubsidyBased &&
                    order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed)
                {
                    if (order.TotalInclSubsidy.HasValue)
                    {
                        amountForPayment.SubsidyBased = true;
                        if (saleType == SaleTypesEnum.InstallmentPayment ||
                            saleType == SaleTypesEnum.CashPaymentAndInstallmentPayment)
                        {
                            if (installmentCountInclSubsidy.HasValue)
                            {
                                amountForPayment.InstallmentCount = installmentCountInclSubsidy.Value;
                            }
                        }

                        amountForPayment.Total = order.TotalInclSubsidy.Value;
                    }
                }
                else
                {
                    if (order.TotalExclSubsidy.HasValue)
                    {
                        if (saleType == SaleTypesEnum.InstallmentPayment ||
                            saleType == SaleTypesEnum.CashPaymentAndInstallmentPayment)
                        {
                            if (installmentCountExclSubsidy.HasValue)
                            {
                                amountForPayment.InstallmentCount = installmentCountExclSubsidy.Value;
                            }
                        }

                        amountForPayment.Total = order.TotalExclSubsidy.Value;
                    }
                }
            }

            amountForPayment.SaleType = saleType;
            return amountForPayment;
        }

        public async Task<int?> GetOrderInstallmentCountAsync(decimal? finalPrice, int orderId, int orderUserId)
        {
            var orderAllSaleConfigurations = await _saleConfigurationService.GetSaleConfigurationAttributeByOrderIdAsync(orderId, orderUserId);
            if (finalPrice != null)
            {
                return await _saleConfigurationService.GetSaleConfigurationsInstallmentCountAsync(finalPrice.Value, orderAllSaleConfigurations);
            }

            return null;
        }

        //public IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, int? userId, bool? isMessageSent = null)
        //{
        //    return _orderRepository.GetCompletePayOrderItems(applicationId, userId, isMessageSent);
        //}

        public IQueryable<OrderItem> GetWaitingForResultOrderItems(int applicationId)
        {
            return _orderRepository.GetWaitingForResultOrderItems(applicationId);
        }

        public List<CalcedOrderItemPrice> GetShoppingCartItemPricesForCalcPrice(
            int? recipientId,
            Guid shoppingCartCode,
            bool subsidyBased,
            int customerClubId,
            bool? calcMaxSubsidy = null
            )
        {
            return _orderRepository.GetShoppingCartItemPrices(
                recipientId,
                shoppingCartCode,
                subsidyBased,
                calcMaxSubsidy,
                customerClubId)
                .Where(x => !x.ParentProductSalePlanningId.HasValue).ToList();
        }
        public List<CalcedOrderItemPrice> GetShoppingCartItemPrices(
            int? recipientId, Guid shoppingCartCode,
            bool subsidyBased, int? customerClubId,
            bool? calcMaxSubsidy = null)
        {
            return _orderRepository.GetShoppingCartItemPrices(
                recipientId, shoppingCartCode,
                subsidyBased, calcMaxSubsidy, customerClubId);
        }
        public string GetReservationNumber(int orderId, int paymentId)
        {
            return _storeConfiguration.OrderReferenceNumberPrefix + "_" + paymentId;
        }

        public async Task<PaymentInformation> CreateNewPaymentAndReturnAsync(int orderId,
            PaymentMethodTypesEnum paymentMethodType, string merchantId, string terminalId)
        {
            var calcAmountForPay = await CalcAmountForPayAsync((int)paymentMethodType, orderId);
            if (calcAmountForPay.Total <= 0)
            {
                return null;
            }
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var payment = new Payment
            {
                MerchantId = merchantId,
                Code = "1",
                OrderId = orderId,
                ReservationNumber = string.Empty,
                PaymentMethodType = (int)paymentMethodType,
                PaymentStatusType = (int)PaymentStatusTypesEnum.SendToPaymentGateway,
                InstallmentCount = calcAmountForPay.InstallmentCount,
                Total = calcAmountForPay.Total,
                ApplicationId = _applicationConfiguration.ApplicationId,
                SaleType = (int)calcAmountForPay.SaleType,
                YearMonth = DateFunction.TodayShamsiYearAndMonthInt(),
                PaidDateInt = DateFunction.TodayShamsiInt(),
                TerminalNumber = !string.IsNullOrEmpty(terminalId) ? terminalId : null
            };
            try
            {
                await _paymentService.AddAsync(payment);
                await _paymentService.SaveChangeAsync();
                payment.ReservationNumber = GetReservationNumber(orderId, payment.Id);
                await _paymentService.UpdateAsync(payment, payment.Id);
                await _paymentService.SaveChangeAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message + "خطا در ساخت رکورد پرداخت و بروز رسانی آن");
            }
            return new PaymentInformation { InstallmentCount = calcAmountForPay.InstallmentCount, Total = calcAmountForPay.Total, PaymentId = payment.Id, ReservationNumber = payment.ReservationNumber, Payment = payment };
        }

        /// <summary>
        /// سفارشات مبتنی بر یارانه و منتظر تایید یارانه
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetWaitingForSubsidyOrders(int applicationId)
        {
            return _orderRepository.GetWaitingForSubsidyOrders(applicationId);
        }

        public IQueryable<OrderItem> GetWaitingForSubsidyOrderItems(int applicationId)
        {
            return _orderRepository.GetWaitingForSubsidyOrderItems(applicationId);
        }

        /// <summary>
        ///  سفارش کامل شده با پرداخت موفق
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCompletePayOrders(int applicationId)
        {
            return _orderRepository.GetCompletePayOrders(applicationId, _userPrincipal.Id);
        }
        /// <summary>
        /// سفارش منتظر نتیجه درگاه پرداخت
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetWaitingForResultOrders(int applicationId)
        {
            return _orderRepository.GetWaitingForResultOrders(applicationId);
        }
        /// <summary>
        ///سفارش عودت داده شده
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetRefundOrders(int applicationId)
        {
            return _orderRepository.GetRefundOrders(applicationId);
        }
        /// <summary>
        ///سفارش انصراف داده شده-در این حالت هنوز پرداختی انجام نشده است
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCanceledOrders(int applicationId)
        {
            return _orderRepository.GetCanceledOrders(applicationId);
        }

        public async Task CancelExpiredOrdersAsync(bool saveAfterUpdate = false)
        {
            _orderRepository.CancelExpiredOrders(_applicationConfiguration.ApplicationId);
            if (saveAfterUpdate)
            {
                await SaveChangeAsync();
            }
        }

        public int? GetSubsidyStatus(int orderId)
        {
            return _orderRepository.GetSubsidyStatus(orderId);
        }

        /// <summary>
        /// بررسی اینکه آیا درگاه پرداخت نمایش داده شود یا نه
        /// </summary>
        /// <param name="membershipType"></param>
        /// <param name="cardNumber"></param>
        /// <param name="sumSubsidy"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        public async Task<(bool result, bool isPayWithWallet)> CheckAllowSelectPaymentGateway(int membershipType, string cardNumber, decimal? sumSubsidy, int? orderStatusType = null)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var payWithWalletDomainModel = _customerClubService.GetWalletPaymentInfoByCardNumber(cardNumber);
            var isPayWithWallet = payWithWalletDomainModel.IsPayWithWallet;
            if (!AllowContinueShoppingByOrderStatus(orderStatusType))
                return (false, isPayWithWallet);

            // نوع یارانه با توجه به انتخاب مشتری که ساتایی میباشد یا وزارتی
            var subsidyType = membershipType == (int?)MembershipTypesEnum.Vezarat ? (int?)SubsidyTypesEnum.Vezarat : membershipType == (int?)MembershipTypesEnum.Sata ? (int?)SubsidyTypesEnum.Sata : null;

            // در صورتی که پروژه مبتنی بر یارانه باشد و 
            // نوع عضویت انتخابی در مرحله تحویل گیرنده ساتایی باشد و مجموع یارانه اختصاص یافته به شماره حکمت بزرگتر از صفر باشد 
            // شماره کارت در باشگاه مشتریان سمت لوکال موجود باشد
            // کاربر نتواند درگاه بانکی انتخاب کند
            if ((userAdditionalUserProp.SupportSubsidy && sumSubsidy.HasValue && sumSubsidy > 0) || payWithWalletDomainModel.CustomerClubCardId.HasValue)
            {
                // اگر کاربر وزارتی بود و شماره حکمت آن در شماره حکمت کارت های معتبر موجود بود 
                //  پس یارنه آن مورد تایید میباشد و باید پرداخت رامشاهده کند 
                //  اگر یارانه ساتایی بود نیز یعنی با سرویس تعیین وضعیت یارانه ساتا نتیجه true
                //  داده پس یارانه ساتا تعیین تکلیف شده است و کاربر میتواند پرداخت کند
                if ((subsidyType == (int?)SubsidyTypesEnum.Vezarat || subsidyType == (int?)SubsidyTypesEnum.Sata
                     || (membershipType == (int)MembershipTypesEnum.Others && payWithWalletDomainModel.CustomerClubCardId.HasValue)))
                //&& !isPayWithWallet)
                {
                    return (true, isPayWithWallet);

                }
                else
                {
                    return (false, isPayWithWallet);
                }
            }
            else
            {
                return (true, isPayWithWallet);
            }
        }

        private bool AllowContinueShoppingByOrderStatus(int? orderStatusType)
        {
            if (orderStatusType != null && orderStatusType != (int)OrderStatusTypesEnum.InitialRegistration &&
               orderStatusType != (int)OrderStatusTypesEnum.UnsuccessfulPayment &&
                orderStatusType != (int)OrderStatusTypesEnum.IncompletePayment &&
                orderStatusType != (int)OrderStatusTypesEnum.WaitingForPayment &&
               orderStatusType != (int)OrderStatusTypesEnum.IncompleteCashAndInstallmentPay)
            {
                return false;
            }
            return true;
        }

        public IQueryable<Order> GetUserLastOrder(int userId)
        {
            return _orderRepository.GetUserLastOrder(userId);
        }
        /// <summary>
        /// تعیین وضعیت یارانه ساتا
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order> GetAwaitingSataSubsidyApproval()
        {
            return _orderRepository.GetAwaitingSataSubsidyApproval();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowSelectPaymentGateway"></param>
        /// <param name="isPayWithWallet"></param>
        /// <param name="membershipType"></param>
        /// <param name="cardNumber"></param>
        /// <param name="sumSubside"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        public bool CheckAllowContinueShopping(bool allowSelectPaymentGateway, bool isPayWithWallet, int membershipType, string cardNumber, decimal? sumSubside, int? orderStatusType = null)
        {
            if (allowSelectPaymentGateway)
            {
                return true;
            }
            if (isPayWithWallet && AllowContinueShoppingByOrderStatus(orderStatusType))
            {
                return true;
            }

            if (AllowContinueShoppingByOrderStatus(orderStatusType))
                return true;

            return false;
        }

        public async Task<string> UpdateOrderNationalCode(int orderId, string nationalCode)
        {
            //get order for update national code
            var order = Get(orderId);
            if (order == null)
            {
                return "کاربر گرامی سفارش مورد نظر وجود ندارد";
            }
            if (order.OrderStatusType != (int)OrderStatusTypesEnum.Complete)
            {
                return "کاربر گرامی تنها سفارشات تایید شده امکان تغییر کدملی را دارند.";
            }
            // چک میکنیم که آیا سفارش با کدملی قبلی یارانه ساتای تایید شده دارد یانه 
            if (order.SubsidyType == (int)SubsidyTypesEnum.Sata && order.TotalInclSubsidy < order.TotalExclSubsidy && order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed)
            {
                // آیا کد ملی جدید میتواند یارانه ساتا بگیرد یا خیر ؟؟
                var sataSubsidyApprovalResult = await _subsidyService.TrySataSubsidyApprovalAsync(nationalCode);

                if (sataSubsidyApprovalResult.CanCheck)
                {
                    // اگر نتیحه چک کردن یارانه ساتا false برگشت
                    if (!sataSubsidyApprovalResult.Result)
                    {
                        return "کاربر گرامی ، تغییر کدملی امکان پذیر نمی باشد. یارانه کدملی قبلی در سامانه ساتا تایید شده است ولی کدملی جدید در استعلام یارانه ساتا، مورد تایید نیست";
                    }
                }
                else
                {
                    //در صورتی که به هر دلیلی نتوان از طریق سرویس وضعیت یارانه ساتا را مشخص کرد
                    //باید وضعیت یارانه ، ثبت اولیه شود تا بعدا تعیین تکلیف گردد                    
                    return "کاربر گرامی با توجه به اینکه سرویس استعلام یارانه ساتا موقتا قطع می باشد، امکان تغییر کدملی در حال حاضر وجود ندارد";
                }

            }
            order.OldNationalCode = order.NationalCode;
            order.NationalCode = nationalCode;

            var result = await UpdateAsync(order, order.Id, true);
            if (result != null)
            {
                var logMessage = String.Format("تغییر کد ملی سفارش {0} از {1} به {2} توسط کاربر {3}", order.Title, order.OldNationalCode, order.NationalCode, _userPrincipal.UserName);
                _logger.InsertLog(LogLevel.Information, new EventId((int)Events.Update, Events.Update.ToString()), nameof(Order), logMessage, null);
                return null;
            }
            return @"خطا در تغییر کد ملی";
        }

        public async Task SendMessageForOrder()
        {
            try
            {
                var orders = await _orderRepository
                    .GetCompletePayOrders(_applicationConfiguration.ApplicationId, null, false)
                    .Select(x => new { x.Id, x.RecipientMobileNumber }).Take(50).ToListAsync();
                var successfulOrderParameters = new Dictionary<string, string>
                {
                    { "applicationName", _verifySmsServiceConfiguration.ApplicationName }
                };
                foreach (var order in orders)
                {
                    if (order.RecipientMobileNumber.IsValidMobileNumber())
                    {
                        var sentResult = await _verifySmsService.SendSmsAsync
                        (order.RecipientMobileNumber,
                            _smsTemplatesConfiguration.SuccessfulOrderTemplateId,
                            successfulOrderParameters);

                        if (sentResult.IsSuccess)
                        {
                            await UpdateAsync(new Order { Id = order.Id, IsMessageSent = true }, order.Id,
                                new List<string> { "IsMessageSent" }, true, true);
                        }
                    }
                }

                var paymentFailedOrders = await _orderRepository
                    .GetPaymentFailedOrders(_applicationConfiguration.ApplicationId, false)
                    .Select(o => new
                    {
                        o.Id,
                        o.RecipientMobileNumber,
                        o.Payment
                    })
                    .Take(50).ToListAsync();

                foreach (var order in paymentFailedOrders)
                {
                    if (order.RecipientMobileNumber.IsValidMobileNumber())
                    {
                        var sentResult = await _verifySmsService.SendSmsAsync
                        (order.RecipientMobileNumber,
                            _smsTemplatesConfiguration.UnsuccessfulOrderTemplateId,
                            new Dictionary<string, string>
                            {
                                { "applicationName", _verifySmsServiceConfiguration.ApplicationName },
                                {
                                    "message", order.Payment
                                        .Where(p =>
                                            p.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                                            && p.PaymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful
                                            && !string.IsNullOrEmpty(p.AuthorizationTransactionId)
                                            && !string.IsNullOrEmpty(p.AuthorizationTransactionResult))
                                        .OrderByDescending(x => x.Id)
                                        .Select(p => p.AuthorizationTransactionResult).FirstOrDefault()
                                }
                            }
                        );
                        if (sentResult.IsSuccess)
                        {
                            await UpdateAsync(new Order { Id = order.Id, IsMessageSent = true }, order.Id,
                                new List<string> { "IsMessageSent" }, true, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(Message),
                    "خطا در ارسال پیامک برای سفارشات تکمیل شده و سفارشات تایید نشده با پرداخت حکمت", ex);
            }

        }

        public async Task<Order> UpdateSubsidyStatusTypeAsync(int orderId, int subsidyStatusType,
            bool saveAfterUpdate = false)
        {
            var order = await _orderRepository.GetAsync(orderId);
            order.ModificationDate = DateTime.Now;
            order.ModifyBy = _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id;
            order.SubsidyStatusType = subsidyStatusType;
            return await UpdateAsync(order, order.Id, saveAfterUpdate, false);
        }

        public IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByFilter(int userId, string cardNumber, bool getOnlyCompletedOrders, bool getForCurrentUser)
        {
            return _orderRepository.GetNotRejectedSubsidyBasedOrdersByFilter(userId, cardNumber, getOnlyCompletedOrders, getForCurrentUser);
        }

        public IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByOrderStatusType(int? userId, string cardNumber, OrderStatusTypesEnum? orderStatusTypesEnum)
        {
            return _orderRepository.GetNotRejectedSubsidyBasedOrdersByOrderStatusType(userId, cardNumber, orderStatusTypesEnum);
        }
        public int GetNotRejectedSubsidyBasedOrdersCountByOrderStatusType(string cardNumber, OrderStatusTypesEnum? orderStatusTypesEnum)
        {
            return GetNotRejectedSubsidyBasedOrdersByOrderStatusType(null, cardNumber, orderStatusTypesEnum).Count();
        }

        public Task<int> CancelOrder(int orderId, int userId)
        {
            return _orderRepository.CancelOrder(orderId, userId);
        }

        public async Task<bool> CancelSubsidyConfirmedInitialRegistrationOrder(int userId, string cardNumber)
        {
            var notRejectedSubsidyBasedInitialRegistrationOrders = GetNotRejectedSubsidyBasedOrdersByOrderStatusType(_userPrincipal.Id, cardNumber, OrderStatusTypesEnum.InitialRegistration);
            if (notRejectedSubsidyBasedInitialRegistrationOrders != null)
            {
                foreach (var item in notRejectedSubsidyBasedInitialRegistrationOrders)
                {
                    await UpdateOrderStatusAsync(item.Id, OrderStatusTypesEnum.CancelBySystem);
                }
                await SaveChangeAsync();
            }

            return true;
        }

        public IQueryable<OrderItem> GetRefundOrderItems(int applicationId)
        {
            return _orderRepository.GetRefundOrderItems(applicationId);
        }

        public IQueryable<OrderItem> GetCanceledOrderItems(int applicationId)
        {
            return _orderRepository.GetCanceledOrderItems(applicationId);
        }

        public IQueryable<Order> GetOrderStatistic(int orderId)
        {
            return _orderRepository.GetOrderStatistic(orderId);
        }

        public async Task<bool> CheckUserHasAccessForOrder(int orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);

            if (_userPrincipal != null && order.UserId == _userPrincipal.Id)
            {
                return true;
            }

            if (_userPrincipal != null)
            {
                var additionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
                if (!additionalUserProp.CompanyId.HasValue)
                {
                    return true;
                }

                return await _orderItemService.GetAll(true).AnyAsync(x =>
                    x.OrderId == orderId &&
                    x.ProductSalesPlanning.Product.CompanyId == additionalUserProp.CompanyId &&
                    (
                        !additionalUserProp.RepresentationId.HasValue ||
                        x.ProductSalesPlanning.RepresentationId == additionalUserProp.RepresentationId
                    )
                );

            }
            return false;
        }


        public override async Task<Order> UpdateAsync(Order order, object key, bool saveAfterUpdate = false, bool publishEvent = true)
        {
            var result = await base.UpdateAsync(order, key, false, publishEvent);
            await _productSalesPlanningService.UpdateOrderProductSalesPlanningsCapacityAsync(order);
            if (saveAfterUpdate)
            {
                await SaveChangeAsync();
            }
            return result;
        }
        public override async Task<Order> AddAsync(Order order, bool saveAfterAdd = false, bool publishEvent = true)
        {
            var result = await base.AddAsync(order, false, publishEvent);
            await _productSalesPlanningService.UpdateOrderProductSalesPlanningsCapacityAsync(order);
            if (saveAfterAdd)
            {
                await SaveChangeAsync();
            }
            return result;
        }

        public override async Task<Order> UpdateAsync(Order order, object key, List<string> props, bool modifyProps,
            bool saveAfterUpdate = false,
            bool publishEvent = true)
        {
            var result = await base.UpdateAsync(order, key, props, modifyProps, false, publishEvent);
            await _productSalesPlanningService.UpdateOrderProductSalesPlanningsCapacityAsync(order);
            if (saveAfterUpdate)
            {
                await SaveChangeAsync();
            }

            return result;
        }
        public Task<Order> GetByOrderItemIdAsync(int orderItemId)
        {
            return GetAll(true).Where(x => x.OrderItem.Any(c => c.Id == orderItemId)).FirstOrDefaultAsync();
        }

        #region Methods used in the IPG

        public async Task<(
            long amount,
            string resNumber,
            string mobileNumber,
            int? installmentCount
            )?> GetGenerateTokenParams(
            int? orderId,
            PaymentMethodTypesEnum paymentMethodType,
            string merchantCode,
            string terminalCode)
        {
            if (!orderId.HasValue)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Order",
                    $"شناسه سفارش برای درج پرداخت و ارسال به درگاه مقدار ندارد",
                    null);

                return null;
            }
            var order = await GetAsync(orderId, true);
            if (order == null)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Order",
                    $"سفارشی با این شناسه وجود ندارد: {orderId.Value}",
                    null);
                return null;
            }

            var paymentInformation = await CreateNewPaymentAndReturnAsync(
                    orderId.Value,
                    paymentMethodType,
                    merchantCode,
                    terminalCode);

            string resNumber;
            long amount;
            if (paymentInformation != null &&
                paymentInformation.Total.HasValue &&
                paymentInformation.Total.Value != 0)
            {


                amount = decimal.ToInt64(paymentInformation.Total.Value);
                resNumber = GetReservationNumber(
                  orderId.Value, paymentInformation.PaymentId);
                return (amount,
                    resNumber,
                    order.RecipientMobileNumber,
                    paymentInformation.InstallmentCount);
            }
            else
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Order",
                    $"پرداختی برای این سفارش وجود ندارد یا مبلغ پرداخت این سفارش مقدار ندارد " +
                    $"OrderId: {orderId.Value}",
                    null);
                return null;
            }
        }


        [ItemCanBeNull]
        public async Task<TResultModel> HttpClientSendAsync<TRequestModel, TResultModel>(
            TRequestModel requestModel,
            string url)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var client = new HttpClient(httpHandler);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var data = JsonSerializer.Serialize(requestModel);
            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var responseModel = JsonSerializer
                    .Deserialize<TResultModel>
                        (await response.Content.ReadAsStringAsync());

                if (responseModel != null)
                {
                    return responseModel;
                }
                else
                {
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            "Payment", $" خروجی responseModel برابر با Null می باشد. ", null);
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment", e.Message, e);
            }

            return default;
        }

        public ResultStatus<int> CheckRequestField(
            bool hasFormContentType,
            string contentType,
            string state,
            string refNum,
            string resNum)
        {
            if (!hasFormContentType || string.IsNullOrEmpty(contentType))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا"
                };
            }

            if (state.Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است"
                };

            }

            if (refNum.Equals(string.Empty) && state.Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت"
                };

            }

            if (resNum.Equals(string.Empty) && state.Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در برقرار ارتباط با بانک"
                };

            }
            return new ResultStatus<int> { IsSuccess = true };
        }

        public string CheckTransactionState(string transactionState,
           out PaymentStatusTypesEnum paymentStatusType)
        {
            paymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
            if (transactionState.Equals(SamanTransactionState.CanceledByUser) || transactionState.Equals(string.Empty))
            {
                // Transaction was canceled by user
                paymentStatusType = PaymentStatusTypesEnum.Cancel;
                return PaymentTransactionStateMessage.CanceledByUser;

            }
            else if (transactionState.Equals(SamanTransactionState.InvalidAmount))
            {
                // Amount of revers teransaction is more than teransaction
                return PaymentTransactionStateMessage.InvalidAmount;
            }
            else if (transactionState.Equals(SamanTransactionState.InvalidTransaction))
            {
                // Can not find teransaction
                return PaymentTransactionStateMessage.InvalidTransaction;
            }
            else if (transactionState.Equals(SamanTransactionState.InvalidCardNumber))
            {
                // Card number is wrong
                return PaymentTransactionStateMessage.InvalidCardNumber;
            }
            else if (transactionState.Equals(SamanTransactionState.NoSuchIssuer))
            {
                // Issuer can not find
                return PaymentTransactionStateMessage.NoSuchIssuer;
            }
            else if (transactionState.Equals(SamanTransactionState.ExpiredCardPickUp))
            {
                // The card is expired
                return PaymentTransactionStateMessage.ExpiredCardPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.AllowablePINTriesExceededPickUp))
            {
                // For third time user enter a wrong PIN so card become invalid
                return PaymentTransactionStateMessage.AllowablePINTriesExceededPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.IncorrectPIN))
            {
                // Pin card is wrong
                return PaymentTransactionStateMessage.IncorrectPIN;
            }
            else if (transactionState.Equals(SamanTransactionState.ExceedsWithdrawalAmountLimit))
            {
                // Exceeds withdrawal from amount limit
                return PaymentTransactionStateMessage.ExceedsWithdrawalAmountLimit;
            }
            else if (transactionState.Equals(SamanTransactionState.TransactionCannotBeCompleted))
            {
                // PIN and PAD are currect but Transaction Cannot Be Completed
                return PaymentTransactionStateMessage.TransactionCannotBeCompleted;
            }
            else if (transactionState.Equals(SamanTransactionState.ResponseReceivedTooLate))
            {
                // Timeout occur
                return PaymentTransactionStateMessage.ResponseReceivedTooLate;
            }
            else if (transactionState.Equals(SamanTransactionState.SuspectedFraudPickUp))
            {
                // User did not insert cvv2 & expiredate or they are wrong.
                return PaymentTransactionStateMessage.SuspectedFraudPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.NoSufficientFunds))
            {
                // there are not suficient funds in the account
                return PaymentTransactionStateMessage.NoSufficientFunds;
            }
            else if (transactionState.Equals(SamanTransactionState.IssuerDownSlm))
            {
                // The bank server is down
                return PaymentTransactionStateMessage.IssuerDownSlm;
            }
            else if (transactionState.Equals(SamanTransactionState.TMEError))
            {
                // unknown error occur
                return PaymentTransactionStateMessage.TMEError;
            }

            return PaymentTransactionStateMessage.TMEError;
        }

        public IQueryable<Order> FindById(int id, int? applicationId = null)
        {
            return _orderRepository.FindOrderById(id, applicationId);
        }

        #endregion

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری برای شناسه
        /// orderItem
        /// ارسال شده سفارشی که پرداخت موفق دارد ثبت کرده است ؟
        /// همچنین در صورتی که پارامتر
        /// companyApiLocalKey
        /// نیز دارای مقدار باشد بررسی می کند که
        /// آیا مرکز محصول خریداری شده
        /// ApiSupported
        /// است و کلید آن برابر است با کلید ارسالی
        /// همچنین محصول خریداری شده آیا از نوع نیاز به هدایت به سایت
        /// company
        /// برای دریافت خدمات است یا نه بررسی های را برای شناسه ایتم سفارش ارسالی انجام میدهد
        /// </summary>
        /// <param name="orderItemId">شناسه ایتم سفارش</param>
        /// <param name="companyApiLocalKey"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> CheckCurrentUserHasAccessToOrderItemAndPayCompletely(int orderItemId, string companyApiLocalKey = null)
        {
            var query = _orderItemService.GetAll(true)
                .Where(orderItem => orderItem.Id == orderItemId &&
                                    orderItem.Order.UserId == _userPrincipal.Id &&
                                    orderItem.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete);

            if (!string.IsNullOrEmpty(companyApiLocalKey))
                query = query.Where(orderItem => orderItem.ProductSalesPlanning.Product.Company.ApiSupported.HasValue &&
                                                 orderItem.ProductSalesPlanning.Product.Company.ApiSupported.Value &&
                                                 orderItem.ProductSalesPlanning.Product.Company.ApiLocalKey ==
                                                 companyApiLocalKey &&
                                                 orderItem.ProductSalesPlanning.Product
                                                     .NeedRedirectToCompanySiteForReceive.HasValue &&
                                                 orderItem.ProductSalesPlanning.Product
                                                     .NeedRedirectToCompanySiteForReceive.Value
                );

            return query;
        }

        /// <summary>
        /// دریافت تمامی سفارش های که
        /// وضعیت سفارش آن ها در حالت منتظر تایید تامین کننده برای پرداخت است.
        ///
        /// اگر کاربر نمایندگی یا مرکز اساین شده داشته باشد به صورت خودکار فیلتر را بر این اساس انجام می دهد.
        /// </summary>
        /// <param name="productSalesPlanningId">فیلتر بر اساس شناسه برنامه ریزی دوره</param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <param name="representationId">فیلتر بر اساس نمایندگی</param>
        /// <returns></returns>
        public IQueryable<Order> GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentAsync(int? productSalesPlanningId, int applicationId, bool readOnly, int? representationId = null)
        {
            var query = _orderRepository.GetByProductSalesPlanningId(productSalesPlanningId, applicationId, readOnly)
                .Where(order => order.OrderStatusType == (int)OrderStatusTypesEnum.RequiredSupplierApprovalBeforePayment);

            if (representationId is > 0)
                query = query.Where(_orderRepository.RepresentationFilter(representationId.Value));

            return query;
        }

        /// <summary>
        /// دریافت سفارش های مربوط به یک برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Order> GetByProductSalesPlanningId(int productSalesPlanningId, int applicationId, bool readOnly)
        {
            return _orderRepository.GetByProductSalesPlanningId(productSalesPlanningId, applicationId, readOnly);
        }

        public async Task<(bool hasAccess, Order Order)> HasCurrentUserAccessToChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPaymentAsync(int orderId)
        {
            var orderAndProductSalesPlanningInformation = await FindById(orderId, _userPrincipal.ApplicationId).Select(order => new
            {
                Order = order,
                ProductSalesPlanningIds = order.OrderItem.Select(oi => oi.ProductSalesPlanningId).ToList()
            }).FirstOrDefaultAsync();

            if (orderAndProductSalesPlanningInformation == null ||
                orderAndProductSalesPlanningInformation.Order == null ||
                orderAndProductSalesPlanningInformation.Order.OrderStatusType == 0 ||
                orderAndProductSalesPlanningInformation.ProductSalesPlanningIds == null ||
                !orderAndProductSalesPlanningInformation.ProductSalesPlanningIds.Any())
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.AccessDeniedGetEntity, nameof(Order),
                    $"orderId has not valid orderStatusId or in not valid. order statusId is {orderId}", null);

                return (false, null);
            }

            foreach (var pspId in orderAndProductSalesPlanningInformation.ProductSalesPlanningIds)
            {

                if (!await _productSalesPlanningService.HasCurrentUserAccessToProductSalesPlanningAsync(pspId))
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.AccessDeniedGetEntity, nameof(Order),
                        $"current user has not access to productSalesPlanning." +
                        $"pspId:{pspId} , userId:{_userPrincipal.Id} ", null);

                    return (false, null);
                }
            }

            return (true, orderAndProductSalesPlanningInformation.Order);
        }

        /// <summary>
        /// دریافت سفارش های مربوط به کاربر جاری که در وضعیت منتظر تایید تامین کننده قبل از پرداخت است.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order> GetCurrentUserWaitingForSupplierApprovalForPaymentOrder()
        {
            return _orderRepository.GetCurrentUserWaitingForSupplierApprovalForPaymentOrder();
        }

        /// <summary>
        /// این متد جهت دریافت ایتم های سفارش کاربر جاری
        /// که در وضعیت منتظرتایید تامین کننده قبل از پرداخت هستند کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public IQueryable<OrderItem> GetCurrentUserWaitingForSupplierApprovalForPaymentOrderItem()
        {
            return _orderRepository.GetCurrentUserWaitingForSupplierApprovalForPaymentOrderItem();
        }

        /// <summary>
        /// متدی جهت بروزرسانی شماره موبایل گیرنده سفارش
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="recipientMobileNumber">شماره موبایل جدید</param>
        /// <param name="oldMobileNumber">شماره موبایل قبلی</param>
        /// <returns></returns>
        public async Task<(bool success, string message)> UpdateOrderRecipientMobileNumberAsync(int orderId, string recipientMobileNumber, string oldMobileNumber)
        {
            if(orderId <= 0) return (false, "کاربر گرامی سفارش مورد نظر وجود ندارد");

            if (!recipientMobileNumber.IsValidMobileNumber()) return (false, "کاربر گرامی شماره موبایل ثبت شده معتبر نیست");

            if (!oldMobileNumber.IsValidMobileNumber()) return (false, "کاربر گرامی شماره موبایل قبلی معتبر نیست");


            var order = Get(orderId);

            if (order == null)
            {
                return (false , "کاربر گرامی سفارش مورد نظر وجود ندارد");
            }

            if (oldMobileNumber != order.RecipientMobileNumber)
            {
                return (false, "کاربر گرامی شماره موبایل قبلی یافت نشد");
            }
            
            order.RecipientMobileNumber = recipientMobileNumber;

            var result = await UpdateAsync(order, order.Id, true);
            if (result != null)
            {
                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "بروزرسانی شماره موبایل گیرنده سفارش",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = "UpdateOrderRecipientMobileNumber",
                    ObjectId = orderId,
                    PreviousRecordContent = Newtonsoft.Json.JsonConvert.SerializeObject(order),
                    ResourceTitle = nameof(Order),
                    UserId = _userPrincipal.Id
                }, true);

                return (true,"شماره موبایل با موفقیت بروزرسانی شد");
            }
            return (false , @"خطا در تغییر شماره موبایل");
        }

        public IQueryable<Order> GetOrderIncludeOrderItemAndProduct(int orderId, bool readOnly) =>
            _orderRepository.GetOrderIncludeOrderItemAndProduct(orderId, readOnly);
    }

    internal class OrderSimpleModel
    {

        public int? CustomerClubCardId { get; set; }
        public int? SubsidyStatusType { get; set; }
        public decimal? TotalInclSubsidy { get; set; }
        public decimal? TotalExclSubsidy { get; set; }
        public int OrderStatusType { get; set; }
        public int UserId { get; set; }

        public OrderSimpleModel(int? customerClubCardId,
            int? subsidyStatusType, decimal? totalInclSubsidy,
            decimal? totalExclSubsidy, int orderStatusType, int userId)
        {

            CustomerClubCardId = customerClubCardId;
            SubsidyStatusType = subsidyStatusType;
            TotalInclSubsidy = totalInclSubsidy;
            TotalExclSubsidy = totalExclSubsidy;
            OrderStatusType = orderStatusType;
            UserId = userId;
        }


    }
}