using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ModifiedOrderItemService : GenericService<ModifiedOrderItem, int>, IModifiedOrderItemService
    {
        private const string IsNotPossibleToRegisterRefundMessage = "کاربر گرامی، به دلیل اینکه برای پرداخت حکمت این سفارش قبلا عودت وجه ثبت شده است .امکان عودت وجه مجدد وجود ندارد";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModifiedOrderItemRepository _modifiedOrderItemRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ISubsidyService _subsidyService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IRefundDetailService _refundDetailService;
        private readonly IRefundService _refundService;
        private readonly ICustomerClubPaymentInstallmentService _customerClubPaymentInstallmentService;
        private readonly IYashilLogger<ModifiedOrderItem> _logger;

        public ICustomerClubService ClubService { get; }

        public ModifiedOrderItemService(IUnitOfWork unitOfWork, IModifiedOrderItemRepository modifiedOrderItemRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            IRefundDetailService refundDetailService,
            IRefundService refundService,
            IProductSalesPlanningService productSalesPlanningService,
            IOrderItemService orderItemService,
            ApplicationConfiguration applicationConfiguration,
            ISubsidyService subsidyService,
            IPaymentService paymentService,
            ICustomerClubPaymentInstallmentService customerClubPaymentInstallmentService,
            IOrderService orderService, ICustomerClubService customerClubService, IYashilLogger<ModifiedOrderItem> logger)
            : base(unitOfWork, modifiedOrderItemRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _modifiedOrderItemRepository = modifiedOrderItemRepository;
            _userPrincipal = userPrincipal;
            _productSalesPlanningService = productSalesPlanningService;
            _orderItemService = orderItemService;
            _applicationConfiguration = applicationConfiguration;
            _subsidyService = subsidyService;
            _orderService = orderService;
            ClubService = customerClubService;
            _logger = logger;
            _paymentService = paymentService;
            _refundDetailService = refundDetailService;
            _refundService = refundService;
            _customerClubPaymentInstallmentService = customerClubPaymentInstallmentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ModificationOrderItemId</param>
        /// <param name="newProductSalePlanningId"></param>
        /// <param name="representationId"></param>
        /// <param name="rejectRequest">رد کردن درخواست تغییر سفارش</param>
        /// <param name="changeToItSelf">بازگشت به آیتم سفارش قبلی</param>
        /// <param name="newOrderStatusType">وضعیت سفارش جدید</param>
        /// <param name="expertDescription">توضیحات کارشناس</param>
        /// <param name="quantity"></param>
        public async Task<string> ApplyModificationAsync(
            int id,
            int? newProductSalePlanningId,
            int? representationId,
            bool? rejectRequest,
            string expertDescription = null,
            bool changeToItSelf = false,
            int? quantity = 1,
            OrderStatusTypesEnum newOrderStatusType = 0)
        {
            var modifiedOrderItem = Get(id);
            modifiedOrderItem.ExpertDescription = expertDescription;
            if (modifiedOrderItem.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.Completed)
            {
                return "این درخواست قبلا تکمیل شده است و شما مجاز به تکرار آن نمی باشید";
            }
            var oldOrderItem = _orderItemService.GetByModifiedOrderItemId(id)
                .Select(x => new
                {
                    x.Id,
                    Total = x.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed && x.SubsidyPrice > 0
                    ? x.TotalInclSubsidy
                    : x.TotalExclSubsidy,
                    x.Order.UserId,
                    x.Order.User.AdditionalUserPropUser.CustomerClubId,
                    x.OrderId,
                    x.Order.SubsidyStatusType,
                    x.Order.SubsidyType
                }).FirstOrDefault();




            if (oldOrderItem == null)
            {
                return "بروز خطا در تغییر سفارش، آیتم سفارش مورد نظر یافت نشد";
            }

            //باید در مراحل بعدی از این کد باشگاه مشتریان استفاده کرد نه از کد باشگاه مشتریان کاربر جاری!!!!

            if (rejectRequest.HasValue && rejectRequest.Value)
            {
                //انصراف از تغییر سفارش
                modifiedOrderItem.ModifiedOrderItemStatusType = (int)ModifiedOrderItemStatusTypeEnum.RejectedByCrmExpert;
                await UpdateAsync(modifiedOrderItem, modifiedOrderItem.Id);
                await SaveChangeAsync();
            }
            else
            {
                var previousRefundAmount = await GetOrderTotalRefundAmountInCompleteStateAsync(oldOrderItem.OrderId);
                if (!previousRefundAmount.HasValue)
                {
                    // اجازه تغییر سفارش ندارد
                    return "باتوجه به اینکه عودت وجه های مربوط به این خرید هنوز انجام نشده است، شما مجاز به تغییر این سفارش نمی باشید";
                }
                if (modifiedOrderItem.ModifiedOrderItemType == (int)ModifiedOrderItemTypeEnum.CancelOrderItem)
                {
                    var orderItemsCount = await _orderItemService.GetOrderItemsByOrderId(oldOrderItem.OrderId, true)
                        .CountAsync();
                    //در صورتی که تعداد آیتم های یک سفارش، یک عدد باشد 
                    // کل این سفارش به حالت مرجوع می رود
                    if (orderItemsCount > 0)
                    {
                        if (orderItemsCount == 1)
                        {
                            var transaction = await _unitOfWork.BeginTransactionAsync();

                            var res = await _orderService.CancelOrder(oldOrderItem.OrderId, _userPrincipal.Id);
                            if (res >= 1)
                            {
                                var isSussesModifiedOrderItem = await UpdateOrderAndModifiedOrderItemStatus(oldOrderItem.Id,
                                    oldOrderItem.OrderId,
                                     null, null, previousRefundAmount.Value,
                                    modifiedOrderItem);
                                if (!isSussesModifiedOrderItem.isSucess)
                                {
                                    await transaction.RollbackAsync();
                                    return isSussesModifiedOrderItem.msg;
                                }

                                await transaction.CommitAsync();
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                            }
                        }
                        else
                        {
                            var itemsWithSubsidy = ModifyOrderItemPreview(oldOrderItem.Id, null, null, true, null,
                                changeToItSelf);
                            var itemsWithoutSubsidy = ModifyOrderItemPreview(oldOrderItem.Id, null, null, false, null,
                                changeToItSelf);

                            var transaction = await _unitOfWork.BeginTransactionAsync();
                            try
                            {
                                var isSussesModifiedOrderItem = await UpdateOrderAndModifiedOrderItemStatus(oldOrderItem.Id, oldOrderItem.OrderId, itemsWithoutSubsidy, itemsWithSubsidy, previousRefundAmount.Value, modifiedOrderItem);
                                if (!isSussesModifiedOrderItem.isSucess)
                                {
                                    await transaction.RollbackAsync();
                                    return isSussesModifiedOrderItem.msg;
                                }
                                await transaction.CommitAsync();
                            }
                            catch (Exception e)
                            {
                                await transaction.RollbackAsync();
                                Console.WriteLine(e);
                                throw;
                            }

                        }
                    }
                }
                // TODO:Check Calc Price ***
                else if (modifiedOrderItem.ModifiedOrderItemType == (int)ModifiedOrderItemTypeEnum.ChangeOrderItem && newProductSalePlanningId.HasValue)
                {
                    var productSalesPlanning = await _productSalesPlanningService.GetAsync(newProductSalePlanningId, true);
                    if (productSalesPlanning.RepresentationId.HasValue)
                    {
                        representationId = productSalesPlanning.RepresentationId;
                    }
                    var itemsWithSubsidy = ModifyOrderItemPreview(oldOrderItem.Id, newProductSalePlanningId, representationId, true, null, changeToItSelf);
                    var itemsWithoutSubsidy = ModifyOrderItemPreview(oldOrderItem.Id, newProductSalePlanningId, representationId, false, null, changeToItSelf);

                    var newItemWithSubsidy = itemsWithSubsidy.FirstOrDefault(x => !x.ParentProductSalePlanningId.HasValue && x.ProductSalesPlanningId == newProductSalePlanningId);

                    var newItemWithoutSubsidy = itemsWithoutSubsidy.FirstOrDefault(x => !x.ParentProductSalePlanningId.HasValue && x.ProductSalesPlanningId == newProductSalePlanningId);

                    if (representationId != null)
                    {
                        //var saleConfigurationAttribute =
                        //    await _saleConfigurationService.GetSaleConfigurationAttributeAsync(
                        //    newProductSalePlanningId.Value, orderUserCustomerClubId);


                        if (newItemWithoutSubsidy != null)
                        {
                            var transaction = await _unitOfWork.BeginTransactionAsync();
                            try
                            {
                                var newOrderItem = await AddNewOrderItemForModifiedOrder(
                                    oldOrderItem.UserId,
                                    oldOrderItem.OrderId,
                                    oldOrderItem.SubsidyType,
                                    newProductSalePlanningId.Value,
                                    representationId.Value,
                                    newItemWithoutSubsidy,
                                    newItemWithSubsidy,
                                    oldOrderItem.Id,
                                    itemsWithoutSubsidy,
                                    itemsWithSubsidy);

                                var isSussesModifiedOrderItem = await UpdateOrderAndModifiedOrderItemStatus(oldOrderItem.Id, oldOrderItem.OrderId,
                                        itemsWithoutSubsidy, itemsWithSubsidy,
                                        previousRefundAmount.Value, modifiedOrderItem, newOrderItem.Id, newOrderStatusType);
                                if (!isSussesModifiedOrderItem.isSucess)
                                {
                                    await transaction.RollbackAsync();
                                    return isSussesModifiedOrderItem.msg;
                                }
                                await _orderItemService.SetOrderItemPriceRightShareAsync(oldOrderItem.OrderId);

                                await transaction.CommitAsync();

                                return string.Empty;
                            }
                            catch (Exception)
                            {
                                await transaction.RollbackAsync();
                                throw;
                            }
                        }
                        throw new Exception("تنظیم فروش برای نمایندگی و یا مرکز تعریف نشده است");
                    }

                }
                else if (modifiedOrderItem.ModifiedOrderItemType == (int)ModifiedOrderItemTypeEnum.ModifyQuantity)
                {
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// اضافه کردن آیتم سفارش جدید به سفارش
        /// </summary>
        /// <param name="orderUserId"></param>
        /// <param name="orderId">کد سفارش</param>
        /// <param name="oldOrderItemSubsidyType"></param>
        /// <param name="newProductSalePlanningId"></param>
        /// <param name="representationId"></param>
        /// <param name="newItemWithoutSubsidy"></param>
        /// <param name="newItemWithSubsidy"></param>
        /// <param name="oldOrderItemId"></param>
        /// <param name="itemsWithoutSubsidy"></param>
        /// <param name="itemsWithSubsidy"></param>
        /// <returns></returns>
        private async Task<OrderItem> AddNewOrderItemForModifiedOrder(
            int orderUserId,
            int orderId,
            int? oldOrderItemSubsidyType,
            int newProductSalePlanningId,
            int representationId,
            CalcedOrderItemPrice newItemWithoutSubsidy,
            CalcedOrderItemPrice newItemWithSubsidy,
            int oldOrderItemId,
            IReadOnlyCollection<CalcedOrderItemPrice> itemsWithoutSubsidy,
            IReadOnlyCollection<CalcedOrderItemPrice> itemsWithSubsidy)
        {

            var applicationSubsidyConfiguration = await
                _subsidyService.GetApplicationCurrentSubsidyAsync(_applicationConfiguration.ApplicationId);
            //درصد سود یارانه
            decimal? subsidyProfitPercentage = null;
            /*  حتی در صورتی که اپلیکیشن در حال حاظر مبتنی بر یارانه نباشد  
                باز هم ممکن است این سفارش مبتنی بر یارانه ثبت شده باشد  
                به همین دلیل در این قسمت نباید چک شود که آیا اپلیکیشن مبتنی بر یارنه است یا نه  
              */
            if (oldOrderItemSubsidyType.HasValue && applicationSubsidyConfiguration != null)
            {
                subsidyProfitPercentage = oldOrderItemSubsidyType.Value == (int)SubsidyTypesEnum.Vezarat ? applicationSubsidyConfiguration.VezaratProfitPercentage : applicationSubsidyConfiguration.SataProfitPercentage;
            }

            OrderItem newOrderItem;



            if (newItemWithoutSubsidy != null && newItemWithSubsidy != null)
            {
                newOrderItem = await _orderItemService.CreateOrderItemEntityAsync(
                    newItemWithoutSubsidy.SaleConfigurationId,
                    newItemWithoutSubsidy.DiscountPercentage,
                    newItemWithoutSubsidy.ProfitPercentage.Value,
                    newItemWithoutSubsidy.InsurancePercentage.Value,
                    newItemWithoutSubsidy.OtherDeductionsPercentage ?? 0,
                    newItemWithoutSubsidy.WagePercentage.Value,
                    newItemWithSubsidy,
                    newItemWithoutSubsidy,
                    subsidyProfitPercentage,
                    newProductSalePlanningId,
                    representationId,
                    _userPrincipal.ApplicationId,
                    orderUserId,
                    newItemWithoutSubsidy.IsOtherDeductionsIncludeInSupplierInvoice
                    );
                newOrderItem.OrderId = orderId;
                //در صورتی که آیتم سفارش از نوع گروهی بود، ایتم های فرزند این آیتم نیز به سفارش اضافه می شود
                var itemsWithoutSubsidyChildren = itemsWithoutSubsidy.Where(x =>
                    x.ParentProductSalePlanningId == newItemWithoutSubsidy.ProductSalesPlanningId);
                foreach (var childItemsWithoutSubsidy in itemsWithoutSubsidyChildren)
                {
                    var childItemsWithSubsidy = itemsWithSubsidy.FirstOrDefault(x => x.ProductSalesPlanningId == childItemsWithoutSubsidy.ProductSalesPlanningId);
                    if (childItemsWithSubsidy != null)
                    {
                        var childOrderItem = await _orderItemService.CreateOrderItemEntityAsync(
                            childItemsWithSubsidy.SaleConfigurationId,
                            childItemsWithSubsidy.DiscountPercentage,
                            childItemsWithSubsidy.ProfitPercentage.Value,
                            childItemsWithSubsidy.InsurancePercentage.Value,
                            childItemsWithSubsidy.OtherDeductionsPercentage ?? 0,
                            childItemsWithSubsidy.WagePercentage.Value,
                            childItemsWithSubsidy,
                            childItemsWithoutSubsidy,
                            subsidyProfitPercentage,
                            childItemsWithSubsidy.ProductSalesPlanningId,
                            representationId,
                            _userPrincipal.ApplicationId,
                            orderUserId,
                            childItemsWithSubsidy.IsOtherDeductionsIncludeInSupplierInvoice);
                        childOrderItem.OrderId = orderId;
                        newOrderItem.InverseParent.Add(childOrderItem);
                    }
                    else
                    {
                        throw new Exception("برز خطا در تغییر سفارش و ضافه کردن آیتم جدید به سفارش");
                    }

                }

                await _orderItemService.AddAsync(newOrderItem, true);
            }
            else
            {
                throw new Exception($" {oldOrderItemId}<<--oldOrderItem:بروز خطا در فرایند تغییر سفارش");
            }

            return newOrderItem;
        }

        /// <summary>
        /// بروز رسانی قیمت های سفارش بر اساس تغییر صورت گرفته و تغییر وضعیت درخواست تغییر سفارش
        /// </summary>
        /// <param name="oldOrderItemId"></param>
        /// <param name="orderId">کد سفارش</param>
        /// <param name="itemsWithoutSubsidy"></param>
        /// <param name="itemsWithSubsidy"></param>
        /// <param name="previousRefundAmount"></param>
        /// <param name="modifiedOrderItem">آیتم سفارش ویرایش شده</param>
        /// <param name="newOrderItemId">کد آیتم سفارش جدید</param>
        /// <param name="newOrderStatusType">وضعیت سفارش</param>
        /// <returns>تغییر وضعیت با موفقیت انجام شد</returns>
        private async Task<(bool isSucess, string msg)> UpdateOrderAndModifiedOrderItemStatus(int oldOrderItemId, int orderId, List<CalcedOrderItemPrice> itemsWithoutSubsidy,
            List<CalcedOrderItemPrice> itemsWithSubsidy, decimal previousRefundAmount, ModifiedOrderItem modifiedOrderItem, int? newOrderItemId = null,
            OrderStatusTypesEnum newOrderStatusType = 0, string expertDescription = null)
        {
            await _orderItemService.DeleteWithChildrenLogicalAsync(oldOrderItemId);

            var updateModifiedOrderPricesRes = await UpdateModifiedOrderPricesAsync(orderId, oldOrderItemId,
                itemsWithoutSubsidy, itemsWithSubsidy, previousRefundAmount, modifiedOrderItem.Id, false, newOrderStatusType);

            if (!updateModifiedOrderPricesRes.isSuccess)
            {
                return (false, updateModifiedOrderPricesRes.msg);
            }
            if (newOrderItemId.HasValue)
            {
                modifiedOrderItem.NewOrderItemId = newOrderItemId.Value;
            }
            modifiedOrderItem.ModifiedOrderItemStatusType = (int)ModifiedOrderItemStatusTypeEnum.Completed;

            await UpdateAsync(modifiedOrderItem, modifiedOrderItem.Id);
            await SaveChangeAsync();
            return (true, null);
        }

        /// <summary>
        /// قیمت سفارش را بر اساس تغییر صورت گرفته بروز می کند
        /// </summary>
        /// <param name="orderId">کد سفارش</param>
        /// <param name="oldOrderItemId"></param>
        /// <param name="itemsWithoutSubsidy">لیست آیتم ها بعد از تغییر بدون سوبسید</param>
        /// <param name="itemsWithSubsidy">لیست آیتم ها بعد از تغییر با سوبسید</param>
        /// <param name="previousRefundAmount"></param>
        /// <param name="modifiedOrderItemId"></param>
        /// <param name="saveAfterChange"></param>
        /// <param name="newOrderStatusType">وضعیت سفارش جدید</param>
        /// <returns> تغییر سفارش انجام شد</returns>
        private async Task<(bool isSuccess, string msg)> UpdateModifiedOrderPricesAsync(
            int orderId,
            int oldOrderItemId,
            IReadOnlyCollection<CalcedOrderItemPrice> itemsWithoutSubsidy,
            IReadOnlyCollection<CalcedOrderItemPrice> itemsWithSubsidy,
            decimal previousRefundAmount,
            int modifiedOrderItemId,
            bool saveAfterChange = false,
            OrderStatusTypesEnum newOrderStatusType = 0
            )
        {
            var order = await _orderService.GetAsync(orderId, false);

            var oldOrderTotal = order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed ? order.TotalInclSubsidy : order.TotalExclSubsidy;

            if (itemsWithoutSubsidy != null)
            {
                itemsWithoutSubsidy = itemsWithoutSubsidy.Where(x => !x.ParentProductSalePlanningId.HasValue).ToList();
            }

            if (itemsWithSubsidy != null)
            {
                itemsWithSubsidy = itemsWithSubsidy.Where(x => !x.ParentProductSalePlanningId.HasValue).ToList();
            }

            order.TaxExclSubsidy = itemsWithoutSubsidy == null ? 0 : itemsWithoutSubsidy.Sum(x => x.Tax);
            order.TaxInclSubsidy = itemsWithSubsidy == null ? 0 : itemsWithSubsidy.Sum(x => x.Tax);

            order.TotalExclSubsidy = itemsWithoutSubsidy == null ? 0 : itemsWithoutSubsidy.Sum(x => x.Total);
            order.TotalInclSubsidy = itemsWithSubsidy == null ? 0 : itemsWithSubsidy.Sum(x => x.Total);

            var newOrderTotal = order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed ? order.TotalInclSubsidy : order.TotalExclSubsidy;
            var totalPayment = _paymentService.GetPaymentsByOrderId(order.Id)
                .Where(x => x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid).Sum(x => x.Total);

            var refundAmount = totalPayment - previousRefundAmount - newOrderTotal;

            if (refundAmount < 0 && (int)newOrderStatusType == 0)
            {
                order.OrderStatusType = (int)OrderStatusTypesEnum.IncompletePayment;
            }
            else if (newOrderStatusType != 0)
            {
                order.OrderStatusType = (int)newOrderStatusType;
            }
            else
            {
                OrderStatusTypesEnum? orderNewStatus = OrderStatusTypesEnum.Complete;
                if (refundAmount > 0 && newOrderTotal != null && oldOrderTotal != null)
                {
                    var addRefundResult = await AddRefundAsync(orderId, oldOrderItemId,
                        refundAmount, newOrderTotal.Value, oldOrderTotal.Value, modifiedOrderItemId);

                    if (!addRefundResult.isSuccess)
                    {
                        return (false, addRefundResult.msg);
                    }

                    orderNewStatus = addRefundResult.orderStatusType;
                }

                if (orderNewStatus == OrderStatusTypesEnum.IncompletePayment)
                {
                    order.OrderStatusType = (int)OrderStatusTypesEnum.IncompletePayment;
                }
                else if (order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment)
                {
                    order.OrderStatusType = (int)OrderStatusTypesEnum.Complete;
                }
            }
            var res = await _orderService.UpdateAsync(order, order.Id);
            if (saveAfterChange)
            {
                await _orderItemService.SaveChangeAsync();
            }

            return (true, null);
        }

        /// <summary>
        /// درج رکورد های مربوط به عودت وجه
        /// کل پرداخت های ناجای این سفارش لغو می شود و در صورتی که جمع سایر پرداختی ها کمتر از مبلغ جدید سفارش بود  پرداخت ناقص
        /// اتفاق می افتد و در غیر این صورت سفارش کامل است
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="oldOrderItemId"></param>
        /// <param name="totalRefundAmount"></param>
        /// <param name="newOrderTotal"></param>
        /// <param name="oldOrderTotal"></param>
        /// <returns>در صورتی که بعد ثبت رکوردهای عودت، عودت اتفاق بیافتد وضعیت سفارش کامل و در غیر این صورت ناقص می باشد </returns>
        private async Task<(OrderStatusTypesEnum? orderStatusType, bool isSuccess, string msg)> AddRefundAsync(int orderId, int oldOrderItemId, decimal? totalRefundAmount, decimal newOrderTotal, decimal oldOrderTotal, int modifiedOrderItemId)
        {
            // لیست تمامی پرداخت های سفارش
            var orderPayments = await _paymentService.GetPaymentsByOrderId(orderId)
                .Where(x => x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                .Select(x => new OrderPayment
                {
                    PaymentId = x.Id,
                    PaymentGroupId = x.PaymentGroupId,
                    PaymentMethodType = x.PaymentMethodType,
                    Total = x.Total
                }).ToListAsync();
            // 
            var orderRefundDetails = await _refundDetailService.GetByOrderId(orderId).
                Where(x => x.RefundStatusType == (int)RefundStatusTypeEnum.Done).
                Select(x => new RefundPayment
                {
                    Id = x.Id,
                    PaymentId = x.PaymentId.Value,
                    OldPaymentAmount = x.OldPaymentAmount,
                    NewPaymentAmount = x.NewPaymentAmount,
                    PaymentMethodType = x.Payment.PaymentMethodType
                }).ToListAsync();

            //کوچکترین مقدار 
            //NewPaymentAmount
            //مربوط به هر پرداخت 
            //(PaymentId)
            //به عنوان مقدار پرداختی قبلی آن پرداخت در نظر گرفته می شود
            var orderOldPaymentTotalAmount = orderRefundDetails.GroupBy(x => x.PaymentId).Select(x => new PaymentTotalAmount { PaymentId = x.Key, PaymentOldTotalAmount = x.Min(c => c.NewPaymentAmount) }).ToList();
            var refund = new Refund
            {
                CreateBy = _userPrincipal.Id,
                CreationDate = DateTime.Now,
                CreatorOrganizationId = _userPrincipal.OrganizationId,
                ApplicationId = _userPrincipal.ApplicationId,
                AccessLevelId = 0,
                OrderId = orderId,
                OldPrice = oldOrderTotal,
                NewPrice = newOrderTotal,
                TotalRefundAmount = totalRefundAmount
            };
            //جمع مبلغ محاسبه شده مبلغ عودت وجه به ازای کل تراکنش ها که در هر مرحله از محاسبه به این مبلغ افزوده می شود 
            decimal? calculatedRefundAmount = 0;

            //آیا عودت وجه ناجا اتفاق افتاده است؟
            bool existsWalletPaymentRefund = false;

            calculatedRefundAmount = await AddWalletRefundDetail(calculatedRefundAmount, orderPayments, refund, modifiedOrderItemId);
            if (calculatedRefundAmount > 0)
            {
                existsWalletPaymentRefund = true;
            }
            calculatedRefundAmount = AddRefundDetail(totalRefundAmount, calculatedRefundAmount, orderPayments, orderOldPaymentTotalAmount, refund, modifiedOrderItemId, orderRefundDetails, PaymentMethodTypesEnum.Ansar);
            calculatedRefundAmount = AddRefundDetail(totalRefundAmount, calculatedRefundAmount, orderPayments, orderOldPaymentTotalAmount, refund, modifiedOrderItemId, orderRefundDetails, PaymentMethodTypesEnum.Saman);
            calculatedRefundAmount = AddRefundDetail(totalRefundAmount, calculatedRefundAmount, orderPayments, orderOldPaymentTotalAmount, refund, modifiedOrderItemId, orderRefundDetails, PaymentMethodTypesEnum.Samin);
            calculatedRefundAmount = AddRefundDetail(totalRefundAmount, calculatedRefundAmount, orderPayments, orderOldPaymentTotalAmount, refund, modifiedOrderItemId, orderRefundDetails, PaymentMethodTypesEnum.Hekmat);

            if (!existsWalletPaymentRefund && calculatedRefundAmount < totalRefundAmount)
            {
                // در این حالت قبلا یک عودت وجه از نوع پرداخت حکمت اتفاق افتاده است
                //و امکان عودت بیش از یک بار برای تراکنش حکمت مقدور نمی باشد
                return (null, false, IsNotPossibleToRegisterRefundMessage);
            }

            //در صورتی که برای این سفارش عودتی وجود داشت این رکورد درج می شود
            if (refund.RefundDetail.Count > 0)
            {
                await _refundService.AddAsync(refund);
            }
            // تنها وقتی این شرط
            //اتفاق می افتد که حداقل یک پرداخت ناجا وجود داشته باشد
            //و حاصل جمع سایر پرداختی ها کمتر از قیمت جدید باشد
            //و در این حالت وضعیت سفارش پرداخت ناقص می شود
            if (existsWalletPaymentRefund && calculatedRefundAmount != totalRefundAmount)
            {
                return (OrderStatusTypesEnum.IncompletePayment, true, null);
            }
            return (OrderStatusTypesEnum.Complete, true, null);
        }

        /// <summary>
        /// این متد پرداخت های ناجای مربوط به این سفارش را لغو و یا عودت می کند
        /// </summary>
        /// <param name="calculatedRefundAmount"></param>
        /// <param name="orderPayments"></param>
        /// <param name="refund"></param>
        /// <param name="modifiedOrderItemId"></param>
        private async Task<Decimal?> AddWalletRefundDetail(decimal? calculatedRefundAmount,
            List<OrderPayment> orderPayments, Refund refund, int modifiedOrderItemId)
        {
            //لیست پرداخت های ناجای مربوط به سفارش
            var filteredPayments = orderPayments.Where(x => x.PaymentMethodType == (int)PaymentMethodTypesEnum.Wallet);
            foreach (var payment in filteredPayments.OrderByDescending(x => x.Total))
            {
                // برای پرداخت ناجا این عدد بر اساس اقساط پرداخت شده کاربر محاسبه می شود
                var currentYearAndMonth = DateFunction.TodayShamsiYearAndMonthInt();

                // اگر پرداخت ناجا گروه بندی شده باشد آن را عودت می زنیم
                if (payment.PaymentGroupId.HasValue)
                {
                    // جمع مبالغ کسر شده از کاربر
                    var totalPaidInstallmentAmount = _customerClubPaymentInstallmentService.GetByCustomFilter(payment.PaymentId, null).Where(x => x.YearMonth < currentYearAndMonth && x.DemandGroupId.HasValue).Sum(x => x.InstallmentAmount);
                    //کل مبالغ کسر شده از کاربر را باید عودت دهیم
                    var refundDetail = CreateNewRefundDetailEntity(
                          RefundTypeEnum.Naja, payment.PaymentId,
                          payment.Total, 0, totalPaidInstallmentAmount, modifiedOrderItemId);
                    refund.RefundDetail.Add(refundDetail);
                    // اقساط تراکنش پرداخت ناجا را که برای مطالبه اقساط به ناجا ارسال نشده است حذف می کند
                    // یعنی مقدار DemandGroupId
                    // برابر با Null
                    // باشد.
                    await _customerClubPaymentInstallmentService.DeleteRecordsNotSentForDemandByPaymentIdAsync(payment.PaymentId, false);
                }
                else
                {
                    //  در صورتی که پرداخت ناجا گروه بندی نشده باشد وضعیت پرداخت آن را به انصراف گروه بندی نشده تغییر می دهیم
                    //  و عودتی بابت آن ثبت نمی کنیم
                    var paymentForCancel = await _paymentService.GetAsync(payment.PaymentId, false);
                    paymentForCancel.PaymentStatusType = (int)PaymentStatusTypesEnum.UnGroupedCancellation;
                    //چون هنوز هیچ قسطی برای ناجا فرستاده نشده است ، لیست اقساط پاکی می شود
                    await _customerClubPaymentInstallmentService.DeleteByPaymentIdAsync(payment.PaymentId, false);
                    await _paymentService.UpdateAsync(paymentForCancel, paymentForCancel.Id);
                }
                calculatedRefundAmount += payment.Total;
            }
            return calculatedRefundAmount;
        }

        /// <summary>
        /// عودت زدن پرداخت های آنلاین و حکمت
        /// </summary>
        /// <param name="totalRefundAmount">کل مبلغی که باید عودت داده شود</param>
        /// <param name="calculatedRefundAmount">مقدار عودت محاسبه شده تا لحظه فراخوانی این متد</param>
        /// <param name="orderPayments">لیست پرداخت های این سفارش</param>
        /// <param name="orderOldPaymentTotalAmount">آخرین وضعیت پرداختهای این سفارش بعد از عودت های قبلی</param>
        /// <param name="refund">رکورد مربط به عودت</param>
        /// <param name="modifiedOrderItemId"></param>
        /// <param name="orderRefundedPayments">لیست عودت وجه های مربوط به این سفارش</param>
        /// <param name="paymentMethodType">روش های پرداخت آنلاین و حکمت</param>
        private decimal? AddRefundDetail(decimal? totalRefundAmount, decimal? calculatedRefundAmount,
            List<OrderPayment> orderPayments, List<PaymentTotalAmount> orderOldPaymentTotalAmount,
            Refund refund, int modifiedOrderItemId, List<RefundPayment> orderRefundedPayments,
            PaymentMethodTypesEnum paymentMethodType)
        {
            if (calculatedRefundAmount < totalRefundAmount)
            {
                var filteredPayments = orderPayments.Where(x => x.PaymentMethodType == (int)paymentMethodType);
                foreach (var payment in filteredPayments.OrderByDescending(x => x.Total))
                {
                    #region بررسی اینکه آیا قبلا عودت وجهی بابت این پرداخت انجام شده است یا نه؟
                    var paymentAlreadyRefunded = orderRefundedPayments.Any(x => x.PaymentId == payment.PaymentId && x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat);
                    if (paymentAlreadyRefunded)
                    {
                        continue;
                    }

                    #endregion

                    // به این دلیل این متغیر تعریف شده که ممکن است در عودت وجه های قبلی  کل مبلغ مربوط به این پرداخت و یا قسمتی از آن
                    // عودت خورده باشد
                    decimal? oldPaymentTotalAmount = orderOldPaymentTotalAmount.FirstOrDefault(x => x.PaymentId == payment.PaymentId)?.PaymentOldTotalAmount;
                    //آخرین مقدار مربوط به پرداخت بعد از عودت های قبلی
                    if (!oldPaymentTotalAmount.HasValue)
                    {
                        oldPaymentTotalAmount = payment.Total;
                    }
                    decimal? currentPaymentRefundAmount;
                    if (totalRefundAmount - calculatedRefundAmount >= oldPaymentTotalAmount)
                    {
                        currentPaymentRefundAmount = oldPaymentTotalAmount;
                    }
                    else
                    {
                        currentPaymentRefundAmount = totalRefundAmount - calculatedRefundAmount;
                    }// در صورتی که مقدار عودت وجه صفر بود رکوردی درج نشود
                    if (currentPaymentRefundAmount > 0)
                    {
                        var newPaymentAmount = oldPaymentTotalAmount - currentPaymentRefundAmount;

                        //برای پرداخت های حکمت این مقدار نال درج می شود و بعدا از طریق اکسلی که حکمت فرستاده مقدار دهی می شود
                        decimal? refundAmount = 0;
                        if (paymentMethodType == PaymentMethodTypesEnum.Ansar ||
                            paymentMethodType == PaymentMethodTypesEnum.Saman ||
                            paymentMethodType == PaymentMethodTypesEnum.Samin)
                        {
                            // برای پرداخت های انلاین این مقدار پر می شود
                            refundAmount = currentPaymentRefundAmount.Value;
                        }

                        if (paymentMethodType == PaymentMethodTypesEnum.Hekmat)
                        {
                            // برای پرداخت های حکمت این مقدار نال می شود
                            refundAmount = null;
                        }

                        var refundType = paymentMethodType == PaymentMethodTypesEnum.Hekmat
                            ? RefundTypeEnum.Hekmat
                            : RefundTypeEnum.Online;
                        var refundDetail = CreateNewRefundDetailEntity(
                            refundType, payment.PaymentId,
                            oldPaymentTotalAmount.Value, newPaymentAmount.Value, refundAmount, modifiedOrderItemId);
                        refund.RefundDetail.Add(refundDetail);
                        calculatedRefundAmount += currentPaymentRefundAmount;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return calculatedRefundAmount;
        }
        public IQueryable<ModifiedOrderItem> GetByCustomFilter(int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            return _modifiedOrderItemRepository.GetByCustomFilter(oldOrderItemId, newOrderId, modifiedOrderItemType, modifiedOrderItemStatusType, assignee);
        }
        public List<CalcedOrderItemPrice> ModifyOrderItemPreview(int oldOrderItemId, int? newProductSalePlanningId, int? representationId, bool subsidyBased, bool? calcMaxSubsidy, bool changeToItSlef)
        {
            return _modifiedOrderItemRepository.ModifyOrderItemPreview(oldOrderItemId, newProductSalePlanningId, representationId, subsidyBased, calcMaxSubsidy, changeToItSlef);
        }

        public class RefundDetailSimpleModel
        {
            public int Id { get; }
            public int RefundId { get; }
            public decimal? RefundAmount { get; }
            public int? RefundDetailGroupId { get; }
            public int? RefundStatusType { get; }
            public Payment Payment { get; }

            public RefundDetailSimpleModel(int id, int refundId, decimal? refundAmount, int? refundDetailGroupId, int? refundStatusType, Payment payment)
            {
                Id = id;
                RefundId = refundId;
                RefundAmount = refundAmount;
                RefundDetailGroupId = refundDetailGroupId;
                RefundStatusType = refundStatusType;
                Payment = payment;
            }
        }

        /// <summary>
        /// جمع عودت وجه های این سفارش که با موفقیت انجام شده است
        /// در صورتی که تمامی عودت وجه های مربوط به این محصول در مرحله ثبت اولیه باشند 
        /// آنها را پاک می کند
        /// </summary>
        /// <param name="orderId">شماره سفارش</param>
        /// <returns>در صورتی که مقدار بازگشتی نال باشد یعنی تمام عودت وجه های مربوط به این تراکنش هنوز انجام نشده است</returns>
        private async Task<decimal?> GetOrderTotalRefundAmountInCompleteStateAsync(int orderId)
        {
            var refundDetailQuery = _refundDetailService.GetByOrderId(orderId);
            var refundDetails = await refundDetailQuery.Select(x => new RefundDetailSimpleModel(x.Id, x.RefundId, x.RefundAmount, x.RefundDetailGroupId, x.RefundStatusType, x.Payment)).ToListAsync();
            var refundGroupedDetails = refundDetails.GroupBy(x => x.RefundId).ToList();

            if (!refundGroupedDetails.Any())
            {
                return 0;
            }
            await DeleteUnGroupedOrderRefunds(refundDetails);

            //تمام رکورد های عودت وجه مربوط به این تراکنش موفق بوده است
            var allRefundsDone = refundDetails.All(x => x.RefundDetailGroupId.HasValue && x.RefundAmount != null && x.RefundStatusType == (int)RefundStatusTypeEnum.Done);
            if (!allRefundsDone)
            {
                //در صورتی که تمامی رکورد های عودت وجه این تراکنش هنوز
                //کامل نشده بود مقدار خالی بر میگرداند تا این سفارش  قابل ویرایش نباشد
                return null;
            }
            return refundDetails.Sum(x => x.RefundAmount);
        }

        /// <summary>
        /// حذف عودت وجه های گروه بندی نشده مربوط به این تراکنش
        /// </summary>
        /// <param name="refundDetails"></param>
        /// <returns></returns>
        private async Task DeleteUnGroupedOrderRefunds(List<RefundDetailSimpleModel> refundDetails)
        {
            var refundGroupedDetails = refundDetails.GroupBy(x => x.RefundId).ToList();
            foreach (var refundDetailList in refundGroupedDetails)
            {
                //تمامی عودت وجه های این سفارش در وضعیت ثبت اولیه هستند و هنوز برای آنها کاری نشده است
                var allRefundDetailsIsInRegisterState = refundDetailList.All(x =>
                    !x.RefundDetailGroupId.HasValue && x.RefundStatusType == (int)RefundStatusTypeEnum.Register);
                if (allRefundDetailsIsInRegisterState)
                {
                    //در صورتی که تمامی عودت وجه های مربوط به این سفارش در مرحله ثبت اولیه باشند، این عودت ها حذف می شوند
                    foreach (var refundDetail in refundDetailList)
                    {
                        refundDetails.Remove(refundDetail);
                        await _refundDetailService.DeleteAsync(refundDetail.Id);
                    }

                    await _refundService.DeleteAsync(refundDetailList.Key);
                }
            }
        }

        /// <summary>
        /// ایجاد رکورد عودت وجه مربوط به این سفارش
        /// </summary>
        /// <param name="refundType"></param>
        /// <param name="paymentId"></param>
        /// <param name="oldPaymentAmount"></param>
        /// <param name="newPaymentAmount"></param>
        /// <param name="refundAmount"></param>
        /// <returns></returns>
        private RefundDetail CreateNewRefundDetailEntity(
            RefundTypeEnum refundType,
            int paymentId,
            decimal oldPaymentAmount,
            decimal newPaymentAmount,
            Decimal? refundAmount,
            int modifiedOrderItemId)
        {
            return new()
            {
                CreateBy = _userPrincipal.Id,
                Assignee = _userPrincipal.Id,
                CreationDate = DateTime.Now,
                CreatorOrganizationId = _userPrincipal.OrganizationId,
                ApplicationId = _userPrincipal.ApplicationId,
                AccessLevelId = 0,
                RefundStatusType = (int)RefundStatusTypeEnum.Register,
                RefundType = (int)refundType,
                OldPaymentAmount = oldPaymentAmount,
                NewPaymentAmount = newPaymentAmount,
                RefundAmount = refundAmount,
                PaymentId = paymentId,
                ModifiedOrderItemId = modifiedOrderItemId
            };
        }
        /// <summary>
        ///  آیتم سفارش اصلاح شده را به قبل برمی گرداند
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        public async Task<int> CancelModification(int modifiedOrderItemId)
        {
            return await _modifiedOrderItemRepository.CancelModification(modifiedOrderItemId);
        }
        /// <summary>
        /// بررسی می کند که modifiedOrderItemId
        /// آخرین رکورد برای درج شده در 
        /// modifiedOrderItem
        /// برای order 
        /// می باشد 
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        public async Task<bool> IsLastModifiedOrderItemRecord(int modifiedOrderItemId)
        {
            return await _modifiedOrderItemRepository.IsLastModifiedOrderItemRecord(modifiedOrderItemId);
        }
        /// <summary>
        /// اجازه لغو تغییر سفارش 
        /// ابتدا بررسی می کند که این رکورد اصلاح سفارش آخرین رکورد آیتم سفارش برای اصلاح باشد
        /// سپس در صورتی رکورد اصلاحی دارای عودت وجه باشد 
        /// بررسی می کند که اگر نوع عودت وجه حکمت یا ناجا باشد و گروه بندی شده نباشد یعنی RefundDetailGroupId 
        /// دارای مقدار نباشد 
        /// و اگر نوع عودت وجه آنلاین (پرداخت شتاب) باشد باید گروه بندی برای پرداخت نشده باشد.یعنی
        /// RefundDetailForPaymentGroupId 
        /// مقدار نداشته باشد.
        /// در اینصورت  اجازه لغو تغییر سفارش  را می دهد
        /// </summary>
        public async Task<ResultStatus<int>> AllowCancelModification(int modifiedOrderItemId)
        {
            // بررسی می کند که آیا این رکورد تغییر سفارش،آخرین تغییر مربوط به سفارش می باشد
            var isLastModifiedOrderItem = await IsLastModifiedOrderItemRecord(modifiedOrderItemId);
            if (!isLastModifiedOrderItem)
            {
                return new ResultStatus<int>(false, "کاربر گرامی این رکورد قابل برگشت به قبل از تغییر نمی باشد.");
            }
            //بررسی می کند که آیا عودت وجه مربوط به رکورد تغییر آیتم سفارش گروه بندی شده است
            var isGrouped = await _refundDetailService.IsGroupedRefundDetail(modifiedOrderItemId);
            if (isGrouped)
            {
                return new ResultStatus<int>(false, "کاربر گرامی این رکورد گروه بندی عودت وجه شده است و قابل برگشت به قبل از تغییر نیست.");
            }

            return new ResultStatus<int>(true, "");
        }
        /// <summary>
        /// بررسی می کند که آیتم سفارش قبلی در وضعیت اصلاح آیتم سفارش ارسالی وجود دارد یا خیر ؟
        /// </summary>
        /// <param name="oldOrderItemId">شناسه آیتم سفارش قبلی</param>
        /// <param name="modifiedOrderItemStatusType">وضعیت اصلاح آیتم سفارش</param>
        /// <returns></returns>
        public async Task<bool> CheckExistOldOrderItemByModifyOrderItemStatusType(int oldOrderItemId, int modifiedOrderItemStatusType)
        {
            return await _modifiedOrderItemRepository.
                CheckExistOldOrderItemByModifyOrderItemStatusType(oldOrderItemId, modifiedOrderItemStatusType);
        }
        /// <summary>
        /// اصلاح پرداختی اضافی سفارش :
        /// در صورتی که سفارشی پرداخت اضافی داست باید یکی از آیتم های آن سفارش را به خود آن آیتم سفارش 
        /// تغییر بدهیم با این شرط که قیمت های آیتم جدید همان قیمت های آیتم قبلی باشد
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        public async Task<bool> ApplyChangeOrderItemToItSelfAsync(int orderItemId)
        {
            #region Add ModifiedOrderItem
            int? modifiedOrderItemId;
            var orderItem = await _orderItemService.GetAsync(orderItemId, true);
            var modifyOrderItem = _modifiedOrderItemRepository.GetByCustomFilter(orderItemId, null,
                (int)ModifiedOrderItemTypeEnum.ChangeOrderItem,
                (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier, null);
            if (modifyOrderItem.Any())
            {
                modifiedOrderItemId = modifyOrderItem.FirstOrDefault()?.Id;
            }
            else
            {
                var modifiedOrderItemModel = new ModifiedOrderItem
                {
                    OldOrderItemId = orderItemId,
                    CreateBy = _userPrincipal.Id,
                    CreationDate = DateTime.Now,
                    ModifiedOrderItemStatusType = (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier,
                    ModifiedOrderItemType = (int)ModifiedOrderItemTypeEnum.ChangeOrderItem,
                    NewOrderItemId = orderItemId,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CreatorOrganizationId = _userPrincipal.OrganizationId
                };
                var addedModifiedOrderItem = await AddAsync(modifiedOrderItemModel);
                await SaveChangeAsync();
                modifiedOrderItemId = addedModifiedOrderItem.Id;
            }
            #endregion
            if (modifiedOrderItemId.HasValue)
            {
                var result = await ApplyModificationAsync(modifiedOrderItemId.Value, orderItem.ProductSalesPlanningId, orderItem.RepresentationId, false, null, true);
                if (!string.IsNullOrEmpty(result))
                {

                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;

        }

        public IQueryable<ModifiedOrderItem> FindById(int id, bool readOnly)
        {
            return _modifiedOrderItemRepository.FindById(id, readOnly);
        }

        /// <summary>
        /// این متد عملیات تغییر دوره برای سفارشی که در وضعیت
        /// منتظر تایید تامین کننده است را انجام می دهد.
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<(bool isSuccess, string message)> ChangeOrderItemProductSalesPlanningForOrderWithWaitingForSupplierApprovalStatusAsync(int orderItemId,
            int productSalesPlanningId, string description)
        {
            string modifyPspResult = string.Empty;

            try
            {
                var modifiedOrderItem = await AddAsync(new ModifiedOrderItem()
                {
                    OldOrderItemId = orderItemId,
                    ModifiedOrderItemType = (int)ModifiedOrderItemTypeEnum.ChangeOrderItem,
                    ModifiedOrderItemStatusType = (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier,
                    Description = description,
                    CreateBy = _userPrincipal.Id,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    AccessLevelId = (int)AccessLevelEnum.Normal,
                    Deleted = false,
                    CreatorOrganizationId = 1
                }, true);

                int orderItemRepresentationId = await _orderItemService.GetOrderItemRepresentationIdAsync(orderItemId);

                modifyPspResult = await ApplyModificationAsync(modifiedOrderItem.Id, productSalesPlanningId, orderItemRepresentationId, null, newOrderStatusType: OrderStatusTypesEnum.WaitingForPayment);

                if (!string.IsNullOrEmpty(modifyPspResult))
                    throw new Exception("Error occurred in applying productSalesPlanning modification");

                await _modifiedOrderItemRepository.UpdateModifiedOrderItemStatusTypeAsync(modifiedOrderItem.Id,
                    ModifiedOrderItemStatusTypeEnum.Completed);

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(ModifiedOrderItem),
                    $"بروز خطا به هنگام تغییر برنامه ریزی دوره با شناسه ایتم {orderItemId} به شناسه دوره {productSalesPlanningId}", e);

                return (false, string.IsNullOrEmpty(modifyPspResult) ? "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید." : modifyPspResult);
            }

            return (true, string.Empty);
        }
    }


    /// <summary>
    /// جمع عودت وجه های این سفارش که با موفقیت انجام شده است
    /// در صورتی که تمامی عودت وجه های مربوط به این محصول در مرحله ثبت اولیه باشند 
    /// آنها را پاک می کند
    /// </summary>
    class PaymentTotalAmount
    {
        /// <summary>
        /// کد سفارش
        /// </summary>
        public int PaymentId { get; set; }
        /// <summary>
        /// آخرین مقدار پرداختی این سفارش بعد از عودت وجه های قبلی
        /// </summary>
        public decimal? PaymentOldTotalAmount { get; set; }
    }
    /// <summary>
    /// پرداختی های سفارش
    /// </summary>
    class OrderPayment
    {
        public int PaymentId { get; set; }
        public int? PaymentMethodType { get; set; }
        public decimal Total { get; set; }
        public int? PaymentGroupId { get; internal set; }
    }

    class RefundPayment
    {
        /// <summary>
        /// Refund Id
        /// </summary>
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public decimal OldPaymentAmount { get; set; }
        public decimal NewPaymentAmount { get; set; }
        public int? PaymentMethodType { get; set; }
    }
}