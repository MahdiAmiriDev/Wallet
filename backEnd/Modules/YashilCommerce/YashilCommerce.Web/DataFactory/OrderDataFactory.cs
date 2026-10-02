using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Enums;
using Yashil.Core.Web;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilNews.Core.Services;
using YashilUserManagement.Core.Services;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Util;
using YashilCommerce.Core.DTO;
using System.Collections.Generic;

namespace YashilCommerce.Web.DataFactory
{
    public class OrderDataFactory : IOrderDataFactory
    {
        private readonly IRecipientService _recipientService;
        private readonly IMapper _mapper;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IYashilCmsService _cmsService;
        private readonly IOrderItemService _orderItemService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICommonBaseDataService _commonBaseDataService;
        private readonly ISubsidyService _subsidyService;
        private readonly IPaymentService _paymentService;
        private readonly IAppConfigService _appConfigService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IYashilLogger<Order> _logger;
        private readonly IAddressService _addressService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;


        public OrderDataFactory(IRecipientService recipientService, IMapper mapper
            , ApplicationConfiguration applicationConfiguration, IShoppingCartItemService shoppingCartItemService
            , IOrderService orderService, IHttpContextAccessor httpContextAccessor
            , IYashilCmsService cmsService, IOrderItemService orderItemService
            , ISaleConfigurationService saleConfigurationService, IUserPrincipal userPrincipal
            , ICommonBaseDataService commonBaseDataService, ISubsidyService subsidyService
            , IPaymentService paymentService, IAppConfigService appConfigService,
            IAdditionalUserPropService additionalUserPropService,
            IYashilLogger<Order> logger,
            IAddressService addressService, IProductSalesPlanningService productSalesPlanningService, IProductService productService)
        {
            _recipientService = recipientService;
            _mapper = mapper;
            _applicationConfiguration = applicationConfiguration;
            _shoppingCartItemService = shoppingCartItemService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _cmsService = cmsService;
            _orderItemService = orderItemService;
            _saleConfigurationService = saleConfigurationService;
            _userPrincipal = userPrincipal;
            _commonBaseDataService = commonBaseDataService;
            _subsidyService = subsidyService;
            _paymentService = paymentService;
            _appConfigService = appConfigService;
            _additionalUserPropService = additionalUserPropService;
            _logger = logger;
            _addressService = addressService;
            _productSalesPlanningService = productSalesPlanningService;
        }
        /// <summary>
        /// اطلاعات مربوط به خلاصه سفارش را بر اساس کد سفارش پر می کند
        /// </summary>
        /// <param name="orderId">کد سفارش</param>
        /// <returns></returns>
        public async Task<OrderSummaryViewModel> FillOrderSummaryByOrderId(int? orderId)
        {
            var orderSummaryViewModel = new OrderSummaryViewModel();
            if (orderId.HasValue)
            {
                var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                var order = _orderService.GetOrderIncludeAddress(orderId.Value);
                #region Fill Order Address
                OrderAddressViewModel recipientOrderAddressViewModel = await order.ProjectTo<OrderAddressViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                if (recipientOrderAddressViewModel != null)
                {
                    recipientOrderAddressViewModel.AddressBased = userAdditionalUserProp.AddressBased;
                    recipientOrderAddressViewModel.AllowEditAddress = false;
                    recipientOrderAddressViewModel.MobileNumber = recipientOrderAddressViewModel.MobileNumber;
                    recipientOrderAddressViewModel.FullAddress = _addressService.GetFullAddress(
                        recipientOrderAddressViewModel.FullAddress, recipientOrderAddressViewModel.IsEncrypted);
                    orderSummaryViewModel.OrderAddressViewModel = recipientOrderAddressViewModel;
                }
                #endregion

                // در صورتی که یارانه ساتا در وضعیت "ثبت اولیه" باشد، مجدد به ساتا رست می زند
                if (recipientOrderAddressViewModel != null
                    && recipientOrderAddressViewModel.SubsidyType == (int)SubsidyTypesEnum.Sata
                     && recipientOrderAddressViewModel.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration)
                {
                    var subsidyStatusType = await GetSataSubsidyStatusTypeAsync(recipientOrderAddressViewModel.NationalCode);
                    // وضعیت یارانه تایید میشود و در جدول recipient
                    // نتیجه یارانه تایید و نوع یارانه ساتا و نوع عضویت ساتا به روز رسانی میشود
                    if (subsidyStatusType != null)
                    {
                        await _orderService.UpdateSubsidyStatusTypeAsync(orderId.Value, subsidyStatusType.Value, true);
                        recipientOrderAddressViewModel.SubsidyStatusType = subsidyStatusType.Value;
                        recipientOrderAddressViewModel.SubsidyStatusTypeTitle =
                            _commonBaseDataService.GetTitle(subsidyStatusType.Value);
                    }
                }

                var orderItemList = _orderItemService.GetCurrentUserOrderItems(orderId, _applicationConfiguration.ApplicationId);

                var orderItemDetailPresentationViewModels =
                    await orderItemList.ProjectTo<OrderItemDetailPresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                orderSummaryViewModel.GuidanceText = await _appConfigService
                  .GetStringValueAsync(ApplicationConfigKey.YashilCommerceOrderNeedToGuidance);



                foreach (var orderItem in orderItemDetailPresentationViewModels)
                {
                    #region Employment
                    orderItem.IsResumeFillOutEnough = userAdditionalUserProp.IsResumeFillOutEnough;
                    orderItem.EmploymentSubsidyDto = await _productSalesPlanningService.CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(orderItem.ProductSalesPlanningId);
                    #endregion


                    if (!string.IsNullOrEmpty(orderItem.ProductSalePlaningSubTitleFormat)
                        && !string.IsNullOrEmpty(orderItem.CompanySubTitleFormat))
                    {
                        try
                        {
                            orderItem.CompanySubTitleFormat = string.Format(orderItem.CompanySubTitleFormat, CurrencyUtility.FormatPrice(orderItem.Price), "");
                        }
                        catch (Exception e)
                        {
                            _logger.InsertLog(LogLevel.Error,
                                (int)Events.InvalidModel,
                                nameof(order),
                                "خطا در StringFormat مربوط به SubTitleFormat In OrderDataFactory ", e);
                        }


                    }
                    var orderItemChildList = await _orderItemService
                        .GetOrderItemChildrenByParentId(orderItem.Id
                            , _applicationConfiguration.ApplicationId)
                        .Select(x => new OrderItemChildDetailPresentationViewModel
                        {
                            Id = x.Id,
                            Price = x.Price,
                            Title = x.ProductSalesPlanning.Product.Title,

                        })
                        .ToListAsync();

                    foreach (var child in orderItemChildList)
                    {
                        orderItem.OrderItemChildDetailList
                            .Add(new OrderItemChildDetailPresentationViewModel
                            {
                                Id = child.Id,
                                Price = child.Price,
                                Title = child.Title,

                            });
                    }


                }


                var orderDetailPresentationViewModel = await order.ProjectTo<OrderDetailPresentationViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                if (orderDetailPresentationViewModel == null)
                {
                    _logger.InsertLog(LogLevel.Error,
                        (int)Events.InvalidModel,
                        nameof(order),
                        "orderDetailPresentationViewModel = null In FillOrderSummaryByOrderId", null);
                    return null;
                }

                #region Check If Subsidy Based 
                decimal? sumSubsidy = orderItemDetailPresentationViewModels.Sum(x => x.SubsidyPrice);
                bool subsidyBased = sumSubsidy.HasValue && sumSubsidy > 0 &&
                                    orderDetailPresentationViewModel.SubsidyStatusType.HasValue &&
                                    orderDetailPresentationViewModel.SubsidyStatusType != (int)SubsidyStatusTypesEnum.Rejected;

                #endregion

                #region Fill Order Items
                var defaultPictureName = await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceProductDefaultPictureName);
                foreach (var item in orderItemDetailPresentationViewModels)
                {
                    item.ProductPicture = defaultPictureName;
                    item.SubsidyBased = subsidyBased;
                    item.Total = subsidyBased ? item.TotalInclSubsidy : item.TotalExclSubsidy;
                    item.RepresentationAddress = _addressService.GetFullAddress(item.RepresentationAddress, item.RepresentationAddressIsEncrypted);
                }

                orderSummaryViewModel.OrderItemDetailPresentationViewModels = orderItemDetailPresentationViewModels;
                #endregion

                #region Fill Order Sammary
                decimal remainingAmount = 0;
                if (orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment)
                {
                    var amountForPay = await _orderService.CalcAmountForPayAsync(
                            (int)PaymentMethodTypesEnum.Hekmat,
                        orderId.Value);
                    remainingAmount = amountForPay.Total;
                }

                var orderSummaryForPayViewModel = orderItemDetailPresentationViewModels.GroupBy(_ => 1).Select(x => new OrderSummaryForPayViewModel
                {
                    OrderId = orderId,
                    SumTotal = subsidyBased ? orderDetailPresentationViewModel.TotalInclSubsidy : orderDetailPresentationViewModel.TotalExclSubsidy,
                    SumTotalForPay = orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment ? remainingAmount : (subsidyBased ? orderDetailPresentationViewModel.TotalInclSubsidy : orderDetailPresentationViewModel.TotalExclSubsidy),
                    AllItemsHasValue = true,
                    OrderItemsCount = x.Sum(c => c.Quantity),
                    SumPrice = x.Sum(c => c.Quantity * c.Price),
                    SumDiscount = x.Sum(c => c.DiscountAmountExclTax),
                    SumWage = x.Sum(c => c.WageAmountExcTax),
                    SumSubsidy = subsidyBased ? x.Sum(c => c.SubsidyPrice) : null,
                    SubsidyBased = subsidyBased,
                    OrderStatusType = orderDetailPresentationViewModel.OrderStatusType
                }).FirstOrDefault();
                // پرداخت درحال بررسی از درگاه حکمت دارد یا خیر
                var payment = _paymentService.GetPaymentsByOrderId(orderDetailPresentationViewModel.Id);
                if (payment != null && payment.Any(x => (x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                                                         || x.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)
                                                        && x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat))
                {
                    orderSummaryViewModel.IsWaitingResponseFromHekmatPaymentGateway = true;
                    orderSummaryViewModel.AuthorizationTransactionResult = payment.OrderByDescending(x => x.Id).FirstOrDefault()?.AuthorizationTransactionResult;
                }

                if (orderSummaryForPayViewModel != null && orderSummaryForPayViewModel.OrderId.HasValue)
                {
                    orderSummaryForPayViewModel.LimitToHekmatPayment = subsidyBased &&
                                                                       orderDetailPresentationViewModel
                                                                           .SubsidyStatusType ==
                                                                       (int)SubsidyStatusTypesEnum.InitialRegistration;

                    orderSummaryForPayViewModel.SubsidyBased = subsidyBased;
                    orderSummaryForPayViewModel.SumTax = subsidyBased
                        ? orderDetailPresentationViewModel.TaxInclSubsidy
                        : orderDetailPresentationViewModel.TaxExclSubsidy;


                    var orderAllSaleConfigurations =
                        await _saleConfigurationService
                            .GetSaleConfigurationAttributeByOrderIdAsync(
                                orderSummaryForPayViewModel.OrderId.Value,
                                orderDetailPresentationViewModel.UserId);

                    var saleType =
                     GetSaleTypeForDisplayPaymentMethods(orderAllSaleConfigurations);


                    if (saleType.HasValue)
                    {
                        orderSummaryForPayViewModel.SaleType = saleType.Value;
                    }


                    var allSaleConfigurationsAllowedToPayInInstallment =
                        orderAllSaleConfigurations.All(x => x.AllowedToPayInInstallment);

                    if (allSaleConfigurationsAllowedToPayInInstallment && orderSummaryForPayViewModel.SumTotal != null)
                    {
                        orderSummaryForPayViewModel.InstallmentCount =
                            await _saleConfigurationService
                                .GetSaleConfigurationsInstallmentCountAsync(orderSummaryForPayViewModel.SumTotal.Value,
                                    orderAllSaleConfigurations);


                        orderSummaryForPayViewModel.InstallmentAmount = orderSummaryForPayViewModel.SumTotal /
                                                                        orderSummaryForPayViewModel
                                                                            .InstallmentCount;
                    }
                    orderSummaryForPayViewModel.AllowedToPayInInstallment = allSaleConfigurationsAllowedToPayInInstallment;

                    orderSummaryForPayViewModel.RequireSupplierApprovalBeforePayment = 
                        orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.RequiredSupplierApprovalBeforePayment;
                    // var allowSelectPaymentGateway 

                    var (allowSelectPaymentGateway, isPayWithWallet) = await _orderService.CheckAllowSelectPaymentGateway(
                        orderDetailPresentationViewModel.MembershipType, orderDetailPresentationViewModel.CardNumber,
                          orderSummaryForPayViewModel.SumSubsidy,
                        orderDetailPresentationViewModel.OrderStatusType);
                    decimal.TryParse(await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilHekmatMinimumPriceToPay),
                         out var hekmatMinimumPriceToPay);

                    orderSummaryForPayViewModel.LimitedToBankInternetPaymentGateway =
                        orderDetailPresentationViewModel.MembershipType == (int)MembershipTypesEnum.Others ||
                         (!isPayWithWallet && hekmatMinimumPriceToPay != 0 &&
                         orderSummaryForPayViewModel.SumTotalForPay <= hekmatMinimumPriceToPay);

                    orderSummaryForPayViewModel.AllowSelectPaymentGateway =
                        allowSelectPaymentGateway && (orderDetailPresentationViewModel.OrderStatusType !=
                                             (int)OrderStatusTypesEnum.Complete && orderDetailPresentationViewModel.OrderStatusType != (int)OrderStatusTypesEnum.IncompleteCashAndInstallmentPay);
                    orderSummaryForPayViewModel.AllowContinueShopping = _orderService.CheckAllowContinueShopping(
                        allowSelectPaymentGateway, isPayWithWallet, orderDetailPresentationViewModel.MembershipType,
                        orderDetailPresentationViewModel.CardNumber, orderSummaryForPayViewModel.SumSubsidy,
                        orderDetailPresentationViewModel.OrderStatusType);
                    orderSummaryForPayViewModel.ReasonForNotBeingAllowedPay = "";
                    orderSummaryForPayViewModel.OrderIsComplete = orderDetailPresentationViewModel.OrderStatusType ==
                                                                  (int)OrderStatusTypesEnum.Complete;

                    orderSummaryForPayViewModel.SubsidyStatusTypeTitle =
                        orderDetailPresentationViewModel.SubsidyStatusTypeTitle;
                    orderSummaryForPayViewModel.SubsidyStatusType = orderDetailPresentationViewModel.SubsidyStatusType;
                    orderSummaryViewModel.OrderSummaryForPayViewModel = orderSummaryForPayViewModel;
                    orderSummaryViewModel.OrderIsComplete = orderDetailPresentationViewModel.OrderStatusType ==
                                                            (int)OrderStatusTypesEnum.Complete;
                    orderSummaryViewModel.OrderStatusType = orderDetailPresentationViewModel.OrderStatusType;
                    orderSummaryViewModel.OrderStatusTypeTitle = orderDetailPresentationViewModel.OrderStatusTypeTitle;
                    orderSummaryViewModel.SubsidyStatusTypeTitle =
                        orderDetailPresentationViewModel.SubsidyStatusTypeTitle;
                    orderSummaryViewModel.OrderId = orderId;
                    orderSummaryViewModel.SubsidyBased = subsidyBased;
                    orderSummaryViewModel.Title = orderDetailPresentationViewModel.Title;

                    if (orderDetailPresentationViewModel.OrderStatusType != (int)OrderStatusTypesEnum.Complete)
                    {
                        if (subsidyBased)
                        {
                            if (!allowSelectPaymentGateway)
                            {
                                orderSummaryViewModel.CustomMessage =
                                    _cmsService.GetContentByPagePositionId(_applicationConfiguration.ApplicationId,
                                        (int)PagePositionTypeEnum.OrderSummeryReasonsForInabilityToPay);
                            }
                            else
                            {
                                orderSummaryViewModel.CustomMessage =
                                    _cmsService.GetContentByPagePositionId(_applicationConfiguration.ApplicationId,
                                        (int)PagePositionTypeEnum.OrderSummeryWaitingForsubsidyConfirm);
                            }
                        }
                    }
                }

                #endregion
                #region  FillOrderSuccessFulPaymentListSummary
                if (payment != null && payment.Any(p => p.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid))
                {
                    var successFulPaymentListSummary = payment.ProjectTo<SuccessFulPaymentListViewModel>(_mapper.ConfigurationProvider).Where(p => p.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid).ToList();
                    orderSummaryViewModel.OrderSuccessFulPaymentListSummary = successFulPaymentListSummary;
                }
                #endregion
            }
            return orderSummaryViewModel;
        }
        /// <summary>
        /// اطلاعات مربوط به خلاصه سفارش را بر اساس کد تحویل گیرنده پر می کند
        /// </summary>
        /// <param name="recipientId">کد سفارش</param>
        /// <returns></returns>
        public async Task<OrderSummaryViewModel> FillOrderSummaryByRecipientId(int? recipientId)
        {
            var orderSummaryViewModel = new OrderSummaryViewModel();
            string code = _httpContextAccessor.HttpContext?.Request.Cookies[CookiKey.CartId];
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            if (userAdditionalUserProp != null && !string.IsNullOrWhiteSpace(code))
            {
                if (Guid.TryParse(code, out var guidCode))
                {
                    if (recipientId != null)
                    {
                        var recipient = _recipientService.GetUserRecipient(recipientId.Value);
                        if (recipient.Any())
                        {
                            #region Fill Order Address
                            /*آدرس تحویل گیرنده کالا به همراه مشخصات کارت  و نوع عضویت*/
                            var recipientOrderAddressViewModel = await recipient.ProjectTo<OrderAddressViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                            int? subsidyType = null;
                            if (recipientOrderAddressViewModel != null)
                            {
                                recipientOrderAddressViewModel.FullAddress =
                                    _addressService.GetFullAddress(recipientOrderAddressViewModel.FullAddress,
                                        recipientOrderAddressViewModel.IsEncrypted);
                                recipientOrderAddressViewModel.AddressBased = userAdditionalUserProp.AddressBased;
                                /*آدرس مربوط به سفارش پر می شود*/
                                orderSummaryViewModel.OrderAddressViewModel = recipientOrderAddressViewModel;
                            }

                            decimal? sumSubsidy = 0;
                            bool subsidyCanBeUsed = false;

                            if (userAdditionalUserProp.SupportSubsidy && recipientOrderAddressViewModel != null)
                            {
                                subsidyType = recipientOrderAddressViewModel.MembershipType == (int?)MembershipTypesEnum.Vezarat ?
                                    (int?)SubsidyTypesEnum.Vezarat :
                                    recipientOrderAddressViewModel.MembershipType == (int?)MembershipTypesEnum.Sata ?
                                        (int?)SubsidyTypesEnum.Sata : null;

                                var subsidyBasedShoppingCartItemPriceList =
                                    _orderService
                                        .GetShoppingCartItemPricesForCalcPrice(
                                            recipientId,
                                            guidCode,
                                            true,
                                            userAdditionalUserProp.CustomerClubId);
                                sumSubsidy = subsidyBasedShoppingCartItemPriceList.Sum(x => x.Subsidy);
                                subsidyCanBeUsed = sumSubsidy > 0;
                            }


                            //  در صورتی که مقدار یارانه صفر باشد مقدار این فیلد نال می ماند 
                            int? subsidyStatusType = null;


                            //در صورتی که برای این تحویل گیرنده و کد ملی قبلا استعلام ساتا را انجام داده باشیم، دوباره این کار را نمی کنیم
                            //در صورتی ک ه وضعیت یارانه در ثبت اولیه باشد دوباره استعلام را انجام می دهیم

                            if (recipientOrderAddressViewModel != null)
                            {
                                if (userAdditionalUserProp.SupportSubsidy &&
                                    string.IsNullOrEmpty(recipientOrderAddressViewModel
                                        .NationalCodeForSubsidyApproval) ||
                                    recipientOrderAddressViewModel.NationalCodeForSubsidyApproval !=
                                    recipientOrderAddressViewModel.NationalCode
                                    || recipientOrderAddressViewModel.SubsidyStatusType ==
                                    (int)SubsidyStatusTypesEnum.InitialRegistration
                                )
                                {
                                    if (sumSubsidy != 0)
                                    {
                                        //اگر یارانه ساتایی باشد
                                        if (recipientOrderAddressViewModel.MembershipType ==
                                            (int)MembershipTypesEnum.Sata)
                                        {
                                            subsidyStatusType =
                                             await GetSataSubsidyStatusTypeAsync(recipientOrderAddressViewModel.NationalCode);
                                            // وضعیت یارانه تایید میشود و در جدول recipient
                                            // نتیجه یارانه تایید و نوع یارانه ساتا و نوع عضویت ساتا به روز رسانی میشود
                                            await _recipientService.UpdateSubsidyStatusTypeAsync(recipientId,
                                                  (int?)SubsidyTypesEnum.Sata,
                                                  recipientOrderAddressViewModel.NationalCode, subsidyStatusType);
                                            recipientOrderAddressViewModel.SubsidyStatusType = subsidyStatusType;

                                        }
                                        //اگر شماره کارت وزارتی باشد
                                        else if (subsidyType == (int?)SubsidyTypesEnum.Vezarat &&
                                                 !string.IsNullOrEmpty(recipientOrderAddressViewModel.CardNumber))
                                        {
                                            // آیا شماره کارت را در پایگاه داده شماره کارت های معتبر موجود میباشد ؟ 
                                            //TODO:Validate Card Number From Hekmat
                                            // اگر شماره کارت وزارتی بود و در شماره کارت های معتبر موجود بود یارانه وزارتی تایید میشود
                                            subsidyStatusType = (int)SubsidyStatusTypesEnum.Confirmed;
                                            // وضعیت یارانه تایید میشود و در جدول recipient
                                            // نتیجه یارانه تایید و نوع یارانه وزارت و نوع عضویت وزارت به روز رسانی میشود
                                            await _recipientService.UpdateSubsidyStatusTypeAsync(recipientId,
                                                 (int?)SubsidyTypesEnum.Vezarat,
                                                 recipientOrderAddressViewModel.NationalCode, subsidyStatusType);
                                        }

                                    }

                                    // Check Sata Subsidy Status
                                }
                                else
                                {
                                    if (sumSubsidy != 0)
                                    {
                                        subsidyStatusType = recipientOrderAddressViewModel.SubsidyStatusType;
                                    }
                                }
                                if (subsidyStatusType.HasValue)
                                {
                                    recipientOrderAddressViewModel.SubsidyStatusTypeTitle =
                                        _commonBaseDataService.GetTitle(subsidyStatusType.Value);
                                }
                                else
                                {
                                    recipientOrderAddressViewModel.SubsidyStatusTypeTitle = null;
                                }
                            }

                            #endregion
                            #region Fill Order Items

                            var shoppingCartItems = _shoppingCartItemService.GetByCustomFilter((int)ShoppingCartTypeEnum.ShoppingCart, guidCode, null);

                            var shoppingCartItemPrices =
                                _orderService
                                    .GetShoppingCartItemPricesForCalcPrice(
                                        recipientId.Value,
                                        guidCode,
                                        userAdditionalUserProp.SupportSubsidy
                                        && subsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed,
                                        userAdditionalUserProp.CustomerClubId
                                        );


                            var sumSubside = shoppingCartItemPrices.Sum(x => x.Subsidy);
                            var orderItemDetailPresentationViewModels = await shoppingCartItems.ProjectTo<OrderItemDetailPresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                            if (orderItemDetailPresentationViewModels != null && orderItemDetailPresentationViewModels.Count > 0)
                            {
                                var defaultPicName = await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceProductDefaultPictureName);
                                foreach (var item in orderItemDetailPresentationViewModels)
                                {
                                    item.RepresentationAddress =
                                        _addressService.GetFullAddress(item.RepresentationAddress,
                                            item.RepresentationAddressIsEncrypted);
                                    var itemPrice = shoppingCartItemPrices.FirstOrDefault(x => x.ProductSalesPlanningId == item.ProductSalesPlanningId);
                                    item.ProductPicture = defaultPicName;
                                    item.SubsidyPrice = itemPrice.Subsidy;
                                    item.Price = itemPrice.Price;
                                    item.Total = itemPrice.Total;
                                    item.DiscountAmountInclTax = itemPrice.Discount;
                                    item.SubsidyBased = sumSubside.HasValue && userAdditionalUserProp.SupportSubsidy;
                                    item.EmploymentSubsidyDto = await _productSalesPlanningService.CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(item.ProductSalesPlanningId);
                                    item.IsResumeFillOutEnough = userAdditionalUserProp.IsResumeFillOutEnough;
                                }
                                /*لیست آیتم های سفارش پر می شود*/
                                orderSummaryViewModel.OrderItemDetailPresentationViewModels = orderItemDetailPresentationViewModels;
                                #endregion
                                #region Fill Order Summary

                                if (shoppingCartItemPrices.Any(x => !x.Total.HasValue))
                                {
                                    _logger.InsertLog(LogLevel.Error,
                                        (int)Events.InvalidModel,
                                        nameof(recipient),
                                        "قیمت نهایی یکی از آیتم های سبد خرید در محاسبه مقدار ندارد", null);
                                    return null;
                                }

                                var shoppingCartItemsTotal = shoppingCartItemPrices.Sum(x => x.Total.Value);


                                var installmentCount = await _orderService.
                                    GetShoppingCartItemsMinInstallmentCountAsync(
                                        shoppingCartItemsTotal, guidCode);

                                var subsidyBased = userAdditionalUserProp.SupportSubsidy && sumSubside > 0;

                                var orderSummaryForPayViewModel = shoppingCartItemPrices.GroupBy(_ => 1).Select(c =>
                                    new OrderSummaryForPayViewModel
                                    {
                                        SumDiscount = c.Sum(x => x.Discount),
                                        SumPrice = c.Sum(x => x.Price),
                                        SumSubsidy = c.Sum(x => x.Subsidy),
                                        SumTotal = c.Sum(x => x.Total),
                                        AllItemsHasValue = c.All(x => x.CalcedPrice.HasValue),
                                        OrderItemsCount = c.Sum(x => x.Quantity),
                                        RecipientId = recipientId.Value,
                                        SumWage = c.Sum(x => x.Wage),
                                        SumTax = c.Sum(x => x.Tax),
                                        SumShippingCharge = c.Sum(x => x.AdditionalShippingCharge)
                                    }
                                ).FirstOrDefault();
                                orderSummaryForPayViewModel.SubsidyCanBeUsed = subsidyCanBeUsed;
                                orderSummaryForPayViewModel.SubsidyBased = subsidyBased;
                                orderSummaryViewModel.SubsidyBased = subsidyBased;

                                orderSummaryForPayViewModel.AllowedToPayInInstallment =
                                    installmentCount.HasValue && installmentCount.Value > 1;

                                var saleConfigurationAttribute
                                     = (await _saleConfigurationService.
                                     GetSaleConfigurationAttributeByShoppingCartCodeAsync
                                     (guidCode, _applicationConfiguration.ApplicationId));

                                var saleType =
                                    GetSaleTypeForDisplayPaymentMethods(saleConfigurationAttribute);


                                if (saleType.HasValue)
                                {
                                    orderSummaryForPayViewModel.SaleType = saleType.Value;
                                }


                                if (installmentCount.HasValue)
                                {
                                    installmentCount = installmentCount > 0 ? installmentCount : 1;
                                    orderSummaryForPayViewModel.InstallmentCount = installmentCount;
                                    orderSummaryForPayViewModel.InstallmentAmount = orderSummaryForPayViewModel.SumTotal / installmentCount;
                                }

                                var (allowSelectPaymentGateway, isPayWithWallet) =
                                    await _orderService.CheckAllowSelectPaymentGateway(recipientOrderAddressViewModel.MembershipType, recipientOrderAddressViewModel.CardNumber, sumSubside);

                                orderSummaryForPayViewModel.AllowSelectPaymentGateway = allowSelectPaymentGateway;

                                decimal.TryParse(await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilHekmatMinimumPriceToPay), out var hekmatMinimumPriceToPay);

                                orderSummaryForPayViewModel.LimitedToBankInternetPaymentGateway =
                                    recipientOrderAddressViewModel.MembershipType ==
                                    (int)MembershipTypesEnum.Others ||
                                    (!isPayWithWallet && hekmatMinimumPriceToPay != 0
                                    && orderSummaryForPayViewModel.SumTotal <= hekmatMinimumPriceToPay);
                                
                                orderSummaryForPayViewModel.RequireSupplierApprovalBeforePayment =
                                    await _shoppingCartItemService.CheckExistProductRequireSupplierApprovalBeforePaymentAsync(guidCode);

                                orderSummaryForPayViewModel.ButtonTextRequireSupplierApprovalBeforePayment =
                                    await _appConfigService.GetStringValueAsync(ApplicationConfigKey
                                        .YashilCommercePreOrderButtonText);

                                orderSummaryViewModel.OrderSummaryForPayViewModel = orderSummaryForPayViewModel;
                                orderSummaryViewModel.SubsidyStatusType = recipientOrderAddressViewModel.SubsidyStatusType;


                                #endregion
                            }

                        }
                    }
                }
            }
            return orderSummaryViewModel;
        }

        private async Task<int?> GetSataSubsidyStatusTypeAsync(string nationalCode)
        {
            // آیا کد ملی خریدار میتواند یارانه بگیرد یا خیر ؟؟
            var sataSubsidyApprovalResult = await _subsidyService.TrySataSubsidyApprovalAsync(nationalCode);

            //در صورتی که به هر دلیلی نتوان از طریق سرویس وضعیت یارانه ساتا را مشخص کرد
            //باید وضعیت یارانه ، ثبت اولیه شود تا بعدا تعیین تکلیف گردد
            int subsidyStatusType = (int)SubsidyStatusTypesEnum.InitialRegistration;
            // اگر نتیحه چک کردن یارانه ساتا True برگشت
            if (sataSubsidyApprovalResult.CanCheck)
            {
                subsidyStatusType = sataSubsidyApprovalResult.Result
                    ? (int)SubsidyStatusTypesEnum.Confirmed
                    : (int)SubsidyStatusTypesEnum.Rejected;
            }
            return subsidyStatusType;
        }

        public async Task<CheckoutViewModel> PrepareCheckoutViewModelByRecipientIdAsync(int? recipientId)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var recipients =
                _recipientService.GetUserRecipientsByApplicationId(_userPrincipal.Id,
                    _applicationConfiguration.ApplicationId);

            var memberShipTypes = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.MembershipType.ToString());
            var cardTypes = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.CardType.ToString());

            var checkoutViewModel = new CheckoutViewModel
            {
                MembershipTypeList = memberShipTypes
                    .ProjectTo<CommonBaseDataSimpleViewModel>(_mapper.ConfigurationProvider).ToList(),
                CardTypeList = cardTypes.ProjectTo<CommonBaseDataListViewModel>(_mapper.ConfigurationProvider).ToList(),
                RecipientViewList = recipients.ProjectTo<RecipientListViewModel>(_mapper.ConfigurationProvider).ToList()
            };
            if (userAdditionalUserProp.AddressBased)
            {
                foreach (var recipientListViewModel in checkoutViewModel.RecipientViewList)
                {
                    recipientListViewModel.Name = !string.IsNullOrEmpty(recipientListViewModel.Name)
                        ? CryptographyHelper.AesDecrypt(recipientListViewModel.Name)
                        : recipientListViewModel.Name;
                    recipientListViewModel.LastName = !string.IsNullOrEmpty(recipientListViewModel.LastName)
                        ? CryptographyHelper.AesDecrypt(recipientListViewModel.LastName)
                        : recipientListViewModel.LastName;
                    recipientListViewModel.FullAddress = _addressService.GetFullAddress(recipientListViewModel.FullAddress,
                        recipientListViewModel.IsEncryptedAddress);
                }
            }
            checkoutViewModel.SelectedRecipient = recipientId.HasValue
                 ? checkoutViewModel.RecipientViewList.FirstOrDefault(x => x.Id == recipientId)
                 : checkoutViewModel.RecipientViewList.FirstOrDefault();

            // set the address based and subsidy based 

            checkoutViewModel.AddressBased = userAdditionalUserProp.AddressBased;
            checkoutViewModel.SubsidyBased = userAdditionalUserProp.SupportSubsidy;

            return checkoutViewModel;
        }
        /// <summary>
        /// در صورتی نوع فروش همه محصولات یکسان باشد 
        /// نوع فروش اولین آیتم را بر می گرداند
        /// در غیر اینصورت یعنی نوع فروش محصولات یکسان نیستند
        /// حال اگر اجازه پرداخت اقساطی برابر با 
        /// True باشد
        /// یعنی نوع فروش محصولات یا اقساطی با نقدی اقساطی می باشد 
        /// پس باید نوع فروش اقساطی برگردانده شود تا فقط درگاه اقساطی نمایش داده شود
        /// در غیر اینصورت نوع فروش محصولات  نقدی اقساطی یا نقدی می باشد
        ///  پس باید نوع فروش نقدی برگردانده شود تا درگاه نقدی نمایش داده شود
        /// </summary>
        /// <param name="saleConfigurationAttributeDtoList"></param>
        /// <returns></returns>
        private int? GetSaleTypeForDisplayPaymentMethods(List<SaleConfigurationAttributeDto>
            saleConfigurationAttributeDtoList)
        {
            var allSaleTypesHasValue = saleConfigurationAttributeDtoList.Any(x => x.SaleType.HasValue);

            if (!allSaleTypesHasValue)
            {
                return null;
            }
            var allProductHaveSameSaleTypes = saleConfigurationAttributeDtoList.
                GroupBy(x => x.SaleType).Count() == 1;

            if (allProductHaveSameSaleTypes)
            {
                return saleConfigurationAttributeDtoList
                    .Select(x => x.SaleType).FirstOrDefault();
            }

            var isCashPayment = saleConfigurationAttributeDtoList.All(x =>
                x.SaleType == (int)SaleTypesEnum.CashPayment ||
                x.SaleType == (int)SaleTypesEnum.CashPaymentAndInstallmentPayment);

            if (isCashPayment)
            {
                return (int)SaleTypesEnum.CashPayment;
            }

            //نقدی اقساطی یا اقساطی
            var allowedToPayInInstallment =
                saleConfigurationAttributeDtoList.All(x => x.AllowedToPayInInstallment);

            if (allowedToPayInInstallment)
            {
                var isInstallmentPayment = saleConfigurationAttributeDtoList.All(x =>
                    x.SaleType == (int)SaleTypesEnum.InstallmentPayment ||
                    x.SaleType == (int)SaleTypesEnum.CashPaymentAndInstallmentPayment);
                if (isInstallmentPayment)
                {
                    return (int)SaleTypesEnum.InstallmentPayment;
                }
            }

            return null;

        }
    }
}
