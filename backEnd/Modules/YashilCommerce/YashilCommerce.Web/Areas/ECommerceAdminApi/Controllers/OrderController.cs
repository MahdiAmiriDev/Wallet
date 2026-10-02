using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using DNTCaptcha.Core;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Enums;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilUserManagement.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Web;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using Language = DNTCaptcha.Core.Language;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class OrderController : BaseController<Order, int, OrderListViewModel, OrderEditModel, OrderSimpleViewModel>
    {
        private static readonly string ResultStatusOfSubsidyBasedOrdersMessage = @$"کاربر گرامی در حال حاضر یک سفارش منتظر تایید دارید،
            بعد از مشخص شدن سفارش منتظر تایید می توانید سفارش جدید ثبت کنید.";
        private static readonly ConcurrentDictionary<string, LockInfo> ProcessingShoppingCarts = new();

        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IYashilLogger<Order> _logger;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IRecipientService _recipientService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly ISubsidyService _subsidyService;
        private readonly ICustomerClubCardService _customerClubCardService;
        private readonly ICustomerClubService _customerClubService;
        private readonly IOrderItemService _orderItemService;
        private readonly IPaymentService _paymentService;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly IHekmatService _hekmatService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IModifiedOrderItemService _modifiedOrderItemService;

        public OrderController(IOrderService orderService, IMapper mapper, IYashilLogger<Order> logger,
            IPermissionService permissionService,
            IUserPrincipal userPrincipal, IRecipientService recipientService,
            ApplicationConfiguration applicationConfiguration,
            IAppConfigService appConfigService, IShoppingCartItemService shoppingCartItemService,
            ISubsidyService subsidyService, ICustomerClubCardService customerClubCardService,
            ICustomerClubService customerClubService, IOrderItemService orderItemService,
            IPaymentService paymentService,
            ISaleConfigurationService saleConfigurationService,
            IDNTCaptchaValidatorService validatorService, IHekmatService hekmatService, IAdditionalUserPropService additionalUserPropService, IProductSalesPlanningService productSalesPlanningService, IModifiedOrderItemService modifiedOrderItemService)
            : base(orderService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _logger = logger;
            _userPrincipal = userPrincipal;
            _recipientService = recipientService;
            _applicationConfiguration = applicationConfiguration;
            _appConfigService = appConfigService;
            _shoppingCartItemService = shoppingCartItemService;
            _subsidyService = subsidyService;
            _customerClubCardService = customerClubCardService;
            _customerClubService = customerClubService;
            _orderItemService = orderItemService;
            _paymentService = paymentService;
            _validatorService = validatorService;
            _hekmatService = hekmatService;
            _additionalUserPropService = additionalUserPropService;
            _productSalesPlanningService = productSalesPlanningService;
            _modifiedOrderItemService = modifiedOrderItemService;
            _saleConfigurationService = saleConfigurationService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? userId,
            int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType,
            int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId,
            int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType,
            int? companyId, int? representationId, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
            var orders = _orderService.GetByCustomFilter(userId, billingAddressId, shippingAddressId, recipientId,
                orderStatusType, shippingStatusType, paymentStatusType, paymentMethodType, rewardPointsHistoryEntryId,
                affiliateId, salesPlanningId, subsidyId, subsidyStatusType, subsidyType, companyId, representationId,
                orderCreationDateFromDate, orderCreationDateToDate);
            return await DataSourceLoader.LoadAsync(orders.ProjectTo<OrderListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }


        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? userId,
            int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType,
            int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId,
            int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType,
            int? companyId, int? representationId)
        {
            var orders = _orderService.GetByCustomFilter(userId, billingAddressId, shippingAddressId, recipientId,
                orderStatusType, shippingStatusType, paymentStatusType, paymentMethodType, rewardPointsHistoryEntryId,
                affiliateId, salesPlanningId, subsidyId, subsidyStatusType, subsidyType, companyId, representationId,
                null, null);
            return await DataSourceLoader.LoadAsync(
                orders.ProjectTo<OrderSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetUserOrders")]
        public async Task<LoadResult> GetUserOrders(CustomDataSourceLoadOptions loadOptions)
        {
            var orderItems = _orderService.GetUserOrders(_applicationConfiguration.ApplicationId);
            return await DataSourceLoader.LoadAsync(
                orderItems.ProjectTo<OrderListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// ذخیره سفارش از سبد خرید 
        /// </summary>
        /// <param name="shoppingCartParam"></param>
        /// <returns></returns>
        [HttpPost("SaveShoppingCartToOrder")]
        public async Task<ResultStatus<int>> SaveShoppingCartToOrder(ShoppingCartParam shoppingCartParam)
        {
            if (!LockCartIdForSaveShoppingCartToOrder())
            {
                return await CreateOrderErrorMessageAsync(true);
            }

            if (shoppingCartParam.PaymentMethodType == (int)PaymentMethodTypesEnum.Samin
                && shoppingCartParam.PurchaseType.HasValue
                && shoppingCartParam.PurchaseType.Value != (int)SaminPurchaseTypeEnum.Credit
                && shoppingCartParam.PurchaseType.Value != (int)SaminPurchaseTypeEnum.Debit
                && shoppingCartParam.PurchaseType.Value != (int)SaminPurchaseTypeEnum.Installment
               )
            {
                return await CreateOrderErrorMessageAsync(
                    false,
                    "کاربر گرامی نوع پرداخت انتخابی در درگاه ثمین امکان پذیر نمی باشد. ");
            }
            var orderId = shoppingCartParam.OrderId;
            int? paymentMethodType = null;
            bool requiresSubsidyApproval;
            if (!shoppingCartParam.OrderId.HasValue)
            {
                #region Find Recipient

                var code = HttpContext.Request.Cookies[CookiKey.CartId];
                if (string.IsNullOrWhiteSpace(code))
                {
                    return await CreateOrderErrorMessageAsync(
                        false,
                        "بروز خطا در فرایند صدور پیش فاکتور");

                }

                var canParse = Guid.TryParse(code, out var guidCode);
                if (!canParse)
                {
                    return await CreateOrderErrorMessageAsync(false, "بروز خطا در فرایند صدور پیش فاکتور");
                }

                if (!shoppingCartParam.RecipientId.HasValue)
                {
                    return await CreateOrderErrorMessageAsync(false, "لطفا گیرنده محصول را مشخص کنید");
                }

                // سفارش در وضعیت منتظر تایید تامین کننده برای پرداخت باید ثبت بشود
                // این را برای کالاهای سبد خرید بررسی می کنیم
                var existProductRequireSupplierInShoppingCart = await
                      _shoppingCartItemService.CheckExistProductRequireSupplierApprovalBeforePaymentAsync(guidCode);

                if (existProductRequireSupplierInShoppingCart !=
                    shoppingCartParam.RequiredSupplierApprovalBeforePayment)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, "Order",
                        $"بروز خطا در ثبت سفارش - existProductRequireSupplierInShoppingCart = {existProductRequireSupplierInShoppingCart} ," +
                        $"shoppingCartParam.RequiredSupplierApprovalBeforePayment = {shoppingCartParam.RequiredSupplierApprovalBeforePayment}  ", null);

                    return await CreateOrderErrorMessageAsync(false, "کاربر گرامی امکان ثبت سفارش ندارید");
                }

                var recipient = await _recipientService.GetAsync(shoppingCartParam.RecipientId.Value, true);
                if (recipient == null)
                {
                    return await CreateOrderErrorMessageAsync(false, "لطفا گیرنده محصول را مشخص کنید");
                }

                #endregion

                #region FillCardNumber بر اساس نوع پروژه شماره کارت را مقدار می کند

                // در صورتی که پروژه محدود به باشگاه مشتریان باشد مقدار شماره کارت را به صورت زیر پر می کند
                var cardNumber = recipient.CardNumber;


                #endregion

                #region Find Custommer Club

                PayWithWalletDomainModel payWithWalletDomainModel = null;
                if (!string.IsNullOrWhiteSpace(recipient.CardNumber))
                {
                    payWithWalletDomainModel =
                        _customerClubService.GetWalletPaymentInfoByCardNumber(recipient.CardNumber);
                }


                #endregion

                #region Set Subsidy Status Type

                int? subsidyType = null;

                //  قیمت های آیتم های سبد خرید با یارانه
                var subsidyBasedShoppingCartItemPrices = new List<CalcedOrderItemPrice>();
                var parentSubsidyBasedShoppingCartItemPrices = new List<CalcedOrderItemPrice>();
                decimal? sumSubsidy = 0;

                //  در صورتی که مقدار یارانه صفر باشد مقدار این فیلد NUll می ماند 
                int? subsidyStatusType = null;

                var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                //if (_storeConfiguration.SubsidyBased)
                if (userAdditionalUserProp.SupportSubsidy)
                {
                    subsidyBasedShoppingCartItemPrices =
                        _orderService.GetShoppingCartItemPrices(
                            shoppingCartParam.RecipientId.Value,
                            guidCode,
                            true,
                            userAdditionalUserProp.CustomerClubId
                            );

                    parentSubsidyBasedShoppingCartItemPrices = subsidyBasedShoppingCartItemPrices
                        .Where(x => !x.ParentProductSalePlanningId.HasValue).ToList();

                    sumSubsidy = parentSubsidyBasedShoppingCartItemPrices.Sum(x => x.Subsidy);

                    // در صورتی که سوبسید مقدار داشت باشد، فیلد های مربوط به یارانه پر  می شود
                    if (recipient.SubsidyStatusType.HasValue && sumSubsidy > 0)
                    {
                        subsidyStatusType = recipient.SubsidyStatusType;
                        subsidyType = recipient.SubsidyType;
                    }
                }

                if (recipient.MembershipType == (int)MembershipTypesEnum.Sata &&
                    subsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration)
                {
                    return await CreateOrderErrorMessageAsync(
                        false,
                        "کاربر گرامی وضعیت یارانه شما ثبت اولیه می باشد و تا تعیین وضعیت یارانه مجاز به پرداخت نمی باشید، لطفا به مرحله تحویل گیرنده رفته و مجددا تلاش نمایید.");


                }

                requiresSubsidyApproval = CheckRequiresSubsidyApproval(sumSubsidy, subsidyStatusType);

                #endregion



                #region CheckSubsidyConfirmOrders   بررسی میکند که با این شماره کارت سفارشی در وضعیت درحال بررسی با یارانه تایید شده دارد یا خیر

                var existSubsidyConfirmedProcessingOrder = GetResultStatusOfSubsidyBasedOrders(recipient.CardNumber,
                    OrderStatusTypesEnum.Processing);
                if (existSubsidyConfirmedProcessingOrder != null && existSubsidyConfirmedProcessingOrder.IsSuccess)
                {
                    return await CreateOrderErrorMessageAsync(
                        false,
                        ResultStatusOfSubsidyBasedOrdersMessage + @$"(درصورتی که چنین سفارشی در لیست سفارشات شما وجود نداشت
            لطفا با واحد پشتیبانی به شماره {await _appConfigService.GetStringValueAsync(ApplicationConfigKey.SiteConfigurationPhone)} -021 تماس بگیرید.)"
                         );
                }

                #endregion

                #region Check All Items Has Same  Installment Count

                var saleConfigurationAttributeDtoList = await
                    _saleConfigurationService.GetSaleConfigurationAttributeByShoppingCartCodeAsync(guidCode,
                        _applicationConfiguration.ApplicationId);

                bool saleConfigurationAttributeSaleTypesIsCompatible = _saleConfigurationService.CheckSaleConfigurationAttributeSaleTypes(saleConfigurationAttributeDtoList);

                if (!saleConfigurationAttributeSaleTypesIsCompatible)
                {
                    return await CreateOrderErrorMessageAsync(
                        false, "تمامی محصولات یک سبد خرید باید مدل پرداخت نقدی یا اقساطی  باشند.");
                }

                #endregion
                #region Set Payment Method Type

                //اگر مبلغ یارانه صفر بود یا یارانه نداشت یا یارانه تایید شده بود 
                //  درگاه  برای پرداخت انتخاب شده باشد و کاربر پرداخت انجام دهد 
                if (!shoppingCartParam.RequiredSupplierApprovalBeforePayment)
                {
                    paymentMethodType = TrySetPaymentMethodType(shoppingCartParam.PaymentMethodType);
                    if (!paymentMethodType.HasValue)
                    {
                        return await CreateOrderErrorMessageAsync(
                            false,
                            "کاربر گرامی لطفا یک روش پرداخت را انتخاب کنید");
                    }
                }

                #endregion

                #region Create order Entity

                var applicationSubsidyConfiguration = await
                    _subsidyService.GetApplicationCurrentSubsidyAsync(_applicationConfiguration.ApplicationId);

                var shoppingCartItemPricesWithoutSubsidy =
                    _orderService.GetShoppingCartItemPrices(
                        shoppingCartParam.RecipientId.Value,
                        guidCode,
                        false,
                        userAdditionalUserProp.CustomerClubId);
                var parentShoppingCartItemPricesWithoutSubsidy =
                    shoppingCartItemPricesWithoutSubsidy.Where(x => !x.ParentProductSalePlanningId.HasValue).ToList();
                //NOTE: application Id must set from applicationConfiguration.id
                var totalExclSubsidy = parentShoppingCartItemPricesWithoutSubsidy.Sum(x => x.Total);
                var totalInclSubsidy = parentSubsidyBasedShoppingCartItemPrices.Sum(x => x.Total);
                if ((totalExclSubsidy == null || totalExclSubsidy == 0)
                    && (totalInclSubsidy == null || totalInclSubsidy == 0))
                {
                    return await CreateOrderErrorMessageAsync();
                }

                var order = new Order
                {
                    Title = " ",
                    NationalCode = recipient.NationalCode,
                    CreateBy = _userPrincipal.Id,
                    ApplicationId = _applicationConfiguration.ApplicationId,
                    CreationDate = DateTime.Now,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    Code = Guid.NewGuid(),
                    UserId = _userPrincipal.Id,
                    CardNumber = cardNumber,
                    RecipientMobileNumber = recipient.MobileNumber,
                    RecipientName = recipient.Name,
                    RecipientLastName = recipient.LastName,
                    RecipientRelationshipWithBuyerId = recipient.RelationshipWithBuyerType,
                    CityId = recipient.Address != null ? recipient.Address.CityId : null,
                    RecipientBirthDate = recipient.BirthDate,
                    RecipientGenderType = recipient.GenderType,
                    CreationDateInt = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    OrderStatusType = shoppingCartParam.RequiredSupplierApprovalBeforePayment ? (int)OrderStatusTypesEnum.RequiredSupplierApprovalBeforePayment : (int)OrderStatusTypesEnum.InitialRegistration,
                    ShippingStatusType = (int)ShippingStatusTypesEnum.NoShippingRequired,
                    MembershipType = recipient.MembershipType,
                    AddressId = recipient.AddressId,
                    WagePercentage = Convert.ToDecimal(await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilTmsSalesWagePercentage)),
                    TaxExclSubsidy = parentShoppingCartItemPricesWithoutSubsidy.Sum(x => x.Tax),
                    TaxInclSubsidy = parentSubsidyBasedShoppingCartItemPrices.Sum(x => x.Tax),
                    TotalExclSubsidy = totalExclSubsidy,
                    TotalInclSubsidy = totalInclSubsidy,
                    SubsidyType = subsidyType,
                    SubsidyStatusType = subsidyStatusType,
                    SubsidyId = applicationSubsidyConfiguration?.Id,
                    CustomerClubCardId = payWithWalletDomainModel?.CustomerClubCardId,
                    CreationDateYearInt = int.Parse(DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString()
                        .Substring(0, 4)),
                    CreationDateMonthInt = int.Parse(DateFunction
                        .ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString()
                        .Substring(4, 2)),
                    CreationDateDayInt = int.Parse(DateFunction
                        .ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString()
                        .Substring(6, 2)),
                    CustomerClubId = userAdditionalUserProp.CustomerClubId,
                    IsMessageSent = false
                };

                #endregion

                //TODO: Check This For Bug
                var shoppingCartItems =
                    _shoppingCartItemService.
                        GetByCustomFilter((int)ShoppingCartTypeEnum.ShoppingCart,
                        guidCode, null);
                var shoppingCartItemList = await shoppingCartItems
                    .ProjectTo<ShoppingCartItemListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                if (shoppingCartItemList.Count != parentShoppingCartItemPricesWithoutSubsidy.Count)
                {
                    return await CreateOrderErrorMessageAsync(false, "امکان خرید همزمان کالای اقساطی با نقدی موجود نمی باشد");
                }

                #region Set  profit Percentage


                decimal? subsidyProfitPercentage = null;
                if (userAdditionalUserProp.SupportSubsidy)
                {
                    if (subsidyType.HasValue)
                    {
                        subsidyProfitPercentage = subsidyType.Value == (int)SubsidyTypesEnum.Vezarat
                            ? applicationSubsidyConfiguration.VezaratProfitPercentage
                            : applicationSubsidyConfiguration.SataProfitPercentage;
                    }
                }

                #endregion
                //درج آیتم سفارش
                #region Create Order Items
                foreach (var parentCartItemPricesWithoutSubsidy in shoppingCartItemPricesWithoutSubsidy.Where(x =>
                        !x.ParentProductSalePlanningId.HasValue))
                {
                    var productSalesPlanningHasCapacity =
                       await ChecksProductSalesPlanningHaveCapacityAsync
                        (parentCartItemPricesWithoutSubsidy.ProductSalesPlanningId);
                    if (!productSalesPlanningHasCapacity)
                    {
                        var productTitle = await _productSalesPlanningService
                              .FindById
                                  (parentCartItemPricesWithoutSubsidy.ProductSalesPlanningId)
                              .Select(x => x.Product.Title)
                              .FirstOrDefaultAsync();
                        return await CreateOrderErrorMessageAsync(
                            false, $"کاربر گرامی ظرفیت دروه {productTitle} به اتمام رسیده است، شما قادر به خرید این دوره نمی باشید.");
                    }

                    var shoppingCartItem = shoppingCartItemList.FirstOrDefault(x =>
                        x.ProductSalesPlanningId == parentCartItemPricesWithoutSubsidy.ProductSalesPlanningId);

                    var parentCartItemPricesSubsidyBased = subsidyBasedShoppingCartItemPrices.FirstOrDefault(x =>
                        x.ProductSalesPlanningId == parentCartItemPricesWithoutSubsidy.ProductSalesPlanningId);

                    if (!parentCartItemPricesWithoutSubsidy.SaleConfigurationAttributeId.HasValue)
                    {
                        return await CreateOrderErrorMessageAsync(
                            false, "امکان محاسبه قیمت برای برخی از کالا های انتخابی شما مقدور نمی باشد");
                    }

                    if (parentCartItemPricesWithoutSubsidy?.Price != 0 &&
                        parentCartItemPricesWithoutSubsidy?.Total != 0 &&
                        parentCartItemPricesWithoutSubsidy?.Price != null &&
                        parentCartItemPricesWithoutSubsidy.CalcedPrice.HasValue)
                    {




                        //NOTE: applicationId must set from application configuration
                        var parentOrderItem = await _orderItemService.CreateOrderItemEntityAsync(
                            parentCartItemPricesWithoutSubsidy.SaleConfigurationId,
                            parentCartItemPricesWithoutSubsidy.DiscountPercentage,
                            parentCartItemPricesWithoutSubsidy.ProfitPercentage.Value,
                            parentCartItemPricesWithoutSubsidy.InsurancePercentage.Value,
                            parentCartItemPricesWithoutSubsidy.OtherDeductionsPercentage ?? 0,
                            parentCartItemPricesWithoutSubsidy.WagePercentage.Value,
                            parentCartItemPricesSubsidyBased,
                            parentCartItemPricesWithoutSubsidy,
                            subsidyProfitPercentage,
                            shoppingCartItem.ProductSalesPlanningId,
                            shoppingCartItem.RepresentationId,
                            _applicationConfiguration.ApplicationId,
                            _userPrincipal.Id,
                            parentCartItemPricesWithoutSubsidy.IsOtherDeductionsIncludeInSupplierInvoice,
                            shoppingCartItem.Quantity);
                        if (parentOrderItem == null)
                        {
                            return await CreateOrderErrorMessageAsync(true);
                        }

                        foreach (var childCartItemPricesWithoutSubsidy in shoppingCartItemPricesWithoutSubsidy
                                     .Where(x => x.ParentProductSalePlanningId.HasValue &&
                                                 x.ParentProductSalePlanningId == parentCartItemPricesWithoutSubsidy
                                                     .ProductSalesPlanningId))
                        {
                            var childCartItemPricesSubsidyBased = subsidyBasedShoppingCartItemPrices.FirstOrDefault(x =>
                                x.ProductSalesPlanningId == childCartItemPricesWithoutSubsidy.ProductSalesPlanningId);

                            var childOrderItem = await _orderItemService.CreateOrderItemEntityAsync(
                                parentCartItemPricesWithoutSubsidy.SaleConfigurationId,
                                parentCartItemPricesWithoutSubsidy.DiscountPercentage,
                                parentCartItemPricesWithoutSubsidy.ProfitPercentage.Value,
                                parentCartItemPricesWithoutSubsidy.InsurancePercentage.Value,
                                parentCartItemPricesWithoutSubsidy.OtherDeductionsPercentage ?? 0,
                                parentCartItemPricesWithoutSubsidy.WagePercentage.Value,
                                childCartItemPricesSubsidyBased,
                                childCartItemPricesWithoutSubsidy,
                                subsidyProfitPercentage,
                                childCartItemPricesWithoutSubsidy.ProductSalesPlanningId,
                                shoppingCartItem.RepresentationId,
                                _applicationConfiguration.ApplicationId,
                                _userPrincipal.Id,
                                childCartItemPricesSubsidyBased.IsOtherDeductionsIncludeInSupplierInvoice,
                                shoppingCartItem.Quantity);

                            if (childOrderItem == null)
                            {
                                return await CreateOrderErrorMessageAsync(true);
                            }

                            order.OrderItem.Add(childOrderItem);
                            parentOrderItem.InverseParent.Add(childOrderItem);
                        }

                        order.OrderItem.Add(parentOrderItem);
                    }
                    else
                    {
                        return await CreateOrderErrorMessageAsync(true);
                    }
                }
                #endregion

                #region add Order With Items
                // اگر آیتم های سفارش نال بود سفارش درج نشود و پیام خطا به کاربر نمایش می دهد 
                if (order.OrderItem.Count == 0)
                {
                    return await CreateOrderErrorMessageAsync(true);
                }
                var insertedOrder = await _orderService.AddOrderAndClearShoppingCartAsync(order, shoppingCartParam.RecipientId.Value, guidCode);
                if (insertedOrder != null)
                {
                    orderId = insertedOrder.Id;
                }
                else
                {
                    return await CreateOrderErrorMessageAsync(true);
                }

                if (shoppingCartParam.RequiredSupplierApprovalBeforePayment)
                {
                    UnLockCartIdForSaveShoppingCartToOrder();
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        ReturnUrl = Url.Action("ShowMessage", "Order", new
                        {
                            message = await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommercePreOrderMessage)

                        })

                    };
                }
                #endregion
            }
            else
            {
                //در این حالت سفارش قبلا ذخیره شده است و کاربر می خواهد آن را پرداخت کند- از صفحه لیست سفارشات
                //به این قسمت آمده است

                var order = _orderService.GetOrderIncludeAddress(shoppingCartParam.OrderId.Value);
                var orderDetailPresentationViewModel = await order
                    .ProjectTo<OrderDetailPresentationViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                if (orderDetailPresentationViewModel != null)
                {
                    if (orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.Canceled
                        || orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.CancelBySystem)
                    {
                        return await CreateOrderErrorMessageAsync();
                    }

                    #region CheckSubsidyConfirmOrders   بررسی میکند که با این شماره کارت سفارشی در وضعیت درحال بررسی با یارانه تایید شده دارد یا خیر
                    var existSubsidyConfirmedProcessingOrder = GetResultStatusOfSubsidyBasedOrders(orderDetailPresentationViewModel.CardNumber,
                        OrderStatusTypesEnum.Processing);
                    if (existSubsidyConfirmedProcessingOrder != null && existSubsidyConfirmedProcessingOrder.IsSuccess)
                    {
                        return await CreateOrderErrorMessageAsync(false, ResultStatusOfSubsidyBasedOrdersMessage + @$"(درصورتی که چنین سفارشی در لیست سفارشات شما وجود نداشت
            لطفا با واحد پشتیبانی به شماره {await _appConfigService.GetStringValueAsync(ApplicationConfigKey.SiteConfigurationPhone)} -021 تماس بگیرید.)");
                    }
                    #endregion

                    if (orderDetailPresentationViewModel.OrderStatusType == (int)OrderStatusTypesEnum.IncompleteCashAndInstallmentPay)
                        shoppingCartParam.PaymentMethodType = (int)PaymentMethodTypesEnum.Hybrid;

                    paymentMethodType = TrySetPaymentMethodType(shoppingCartParam.PaymentMethodType);
                    if (!paymentMethodType.HasValue)
                    {
                        return await CreateOrderErrorMessageAsync(false, "کاربر گرامی لطفا یک روش پرداخت را انتخاب کنید");
                    }

                    var orderItemList =
                        _orderItemService.GetCurrentUserOrderItems(orderId, _applicationConfiguration.ApplicationId);

                    var orderItemDetailPresentationViewModels = await orderItemList
                        .ProjectTo<OrderItemDetailPresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();

                    foreach (var orderItem in orderItemDetailPresentationViewModels)
                    {
                        var productSalesPlanningHasCapacity =
                           await ChecksProductSalesPlanningHaveCapacityAsync(orderItem.ProductSalesPlanningId);
                        if (!productSalesPlanningHasCapacity)
                        {
                            var productTitle = await _productSalesPlanningService
                                  .FindById(orderItem.ProductSalesPlanningId)
                                  .Select(x => x.Product.Title)
                                  .FirstOrDefaultAsync();
                            return await CreateOrderErrorMessageAsync(
                                false, $"کاربر گرامی ظرفیت دروه " +
                                       $"{productTitle}" +
                                       $" به اتمام رسیده است، شما قادر به خرید این دوره نمی باشید.");
                        }
                    }
                    decimal? sumSubsidy = orderItemDetailPresentationViewModels.Sum(x => x.SubsidyPrice);
                    requiresSubsidyApproval =
                        CheckRequiresSubsidyApproval(sumSubsidy, orderDetailPresentationViewModel.SubsidyStatusType);
                }
                else
                {
                    return await CreateOrderErrorMessageAsync();
                }
            }

            UnLockCartIdForSaveShoppingCartToOrder();
            if (paymentMethodType.HasValue)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    ReturnUrl = GetRedirectUrl(
                        paymentMethodType.Value,
                        requiresSubsidyApproval,
                        orderId.Value,
                        shoppingCartParam.PurchaseType
                    )
                };
            }
            return await CreateOrderErrorMessageAsync(
                false,
                "کاربر گرامی روش پرداخت انتخاب نشده است و امکان پرداخت وجود ندارد.");

        }
        private bool LockCartIdForSaveShoppingCartToOrder()
        {
            var code = HttpContext.Request.Cookies[CookiKey.CartId];
            if (!string.IsNullOrWhiteSpace(code))
            {

                if (ProcessingShoppingCarts.TryAdd(code, new LockInfo { Index = 1, ExpireAt = GetLockExpireTime() }))
                {
                    return true;
                }
                else
                {
                    ProcessingShoppingCarts.TryGetValue(code, out var lockInfo);
                    if (lockInfo != null && lockInfo.ExpireAt > DateTime.Now)
                    {
                        ProcessingShoppingCarts.TryRemove(code, out _);
                    }
                    return false;
                }
            }

            return true;
        }

        private DateTime GetLockExpireTime()
        {
            return DateTime.Now.AddSeconds(30);
        }

        private void UnLockCartIdForSaveShoppingCartToOrder()
        {
            var code = HttpContext.Request.Cookies[CookiKey.CartId];

            if (!string.IsNullOrWhiteSpace(code))
            {
                ProcessingShoppingCarts.TryRemove(code, out _);
            }
        }

        private async Task<ResultStatus<int>> CreateOrderErrorMessageAsync(bool redirectToErrorPage = false, string message = null)
        {
            UnLockCartIdForSaveShoppingCartToOrder();

            if (redirectToErrorPage)
            {
                return new ResultStatus<int>() { IsSuccess = false, ReturnUrl = Url.Action("Index", "Error") };

            }
            if (string.IsNullOrWhiteSpace(message))
            {
                message =
                    $"بروز خطا در ثبت سفارش - لطفا با واحد پشتیبانی به شماره {await _appConfigService.GetStringValueAsync(ApplicationConfigKey.SiteConfigurationPhone)} - 021 تماس بگیرید.";
            }
            return new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = message
            };
        }

        /// <summary>
        /// انتخاب روش پرداختی براساس انتخاب کاربر و اینکه عضو باشگاه مشتریان است و یا نه
        /// </summary>
        /// <param name="shoppingCartParamPaymentMethodType">نوع روش پرداختی انتخابی کاربر</param>
        /// <returns></returns>
        private int? TrySetPaymentMethodType(int? shoppingCartParamPaymentMethodType)
        {
            if (shoppingCartParamPaymentMethodType.HasValue)
            {
                return shoppingCartParamPaymentMethodType;
            }

            return null;
        }

        /// <summary>
        /// نیاز به تایید یارانه است یا نه؟
        /// </summary>
        /// <param name="sumSubsidy"></param>
        /// <param name="subsidyStatusType"></param>
        /// <returns></returns>
        private bool CheckRequiresSubsidyApproval(decimal? sumSubsidy, int? subsidyStatusType)
        {
            return sumSubsidy.HasValue && sumSubsidy != 0 &&
                   subsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration;
        }

        /// <summary>
        /// با توجه به نوع پرداخت و وضعیت یارانه تصمیم می گیرد که کدام آدرس بازگردانده شود
        /// </summary>
        /// <param name="paymentMethodType">روش پرداخت</param>
        /// <param name="requiresSubsidyApproval">نیاز به تایید یارانه</param>
        /// <param name="purchaseType">نوع خرید</param>
        /// <param name="orderId">کد سفارش</param>
        /// <returns></returns>
        private string GetRedirectUrl(int? paymentMethodType,
            in bool requiresSubsidyApproval,
            int orderId,
            int? purchaseType)
        {
            if (requiresSubsidyApproval || paymentMethodType == null)
            {
                return Url.Action("LastOrderStatus", "Order", new { oId = orderId });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Wallet)
            {
                return Url.Action("Index", "WalletPayment", new PayWithWalletParam { OId = orderId });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Hekmat)
            {
                return Url.Action("Index", "HekmatPayment", new { oId = orderId });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Ansar)
            {
                return Url.Action("Index", "AnsarPayment", new { oId = orderId });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Saman)
            {
                return Url.Action("Index", "SamanPayment", new { oId = orderId });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Samin)
            {
                return Url.Action("Index", "SaminPayment", new { oId = orderId, purchaseType });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Saderat)
            {
                return Url.Action("Index", "SaderatPayment", new { oId = orderId, purchaseType });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Hybrid)
            {
                return Url.Action("Index", "HekmatPayment", new { oId = orderId, isHybrid = true });
            }

            if (paymentMethodType == (int)PaymentMethodTypesEnum.Vibe)
            {
                return Url.Action("Index", "VibePayment", new { oId = orderId});
            }
            if (paymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline)
            {
                return Url.Action("Index", "HekmatOnlinePaymentGateway", new { orderId });
            }

            return "/";
        }

        [HttpPost("PayWithWallet")]
        public async Task<ResultStatus<int>> PayWithWallet([FromForm] PayWithWalletParam payWithWalletParam)
        {
            var code = payWithWalletParam.Code;
            var oId = payWithWalletParam.OId;
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.Persian, DisplayMode.NumberToWord))
            {
                return new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = @"عدد وارد شده با متن همخوانی ندارد!"
                };
            }

            if (!oId.HasValue)
            {
                return new ResultStatus<int> { IsSuccess = false, Message = "خطا در انجام تراکنش!" };
            }

            var order = await _orderService.GetAsync(oId, false);

            var payment =
                await _orderService.CreateNewPaymentAndReturnAsync(
                    oId.Value, PaymentMethodTypesEnum.Wallet,
                    string.Empty,
                    string.Empty);

            //**********************Fix This***************************************************************
            if (order.CardNumber != null)
            {
                var customerClubCard = await _customerClubCardService.GetValidCard(true, true, code);
                //*************************************************************************************
                // Check Exist in customerClubCard Code 

                if (customerClubCard == null)
                {
                    await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, PaymentStatusTypesEnum.Unsuccessful,
                           PaymentMethodTypesEnum.Wallet, "اطلاعات کارت وارد شده صحیح نمی باشد", "", true);
                    return new ResultStatus<int> { IsSuccess = false, Message = "اطلاعات کارت وارد شده صحیح نمی باشد" };
                }

                if (payment == null || (!payment.Total.HasValue || payment.Total == 0))
                {
                    if (payment != null)
                    {
                        await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, PaymentStatusTypesEnum.Unsuccessful,
                                 PaymentMethodTypesEnum.Wallet, "اطلاعات سفارش صحیح نمی باشد", "", true);
                    }

                    return new ResultStatus<int> { IsSuccess = false, Message = "اطلاعات سفارش صحیح نمی باشد" };
                }

                // :TODO Calc And Save MaximumNumberPurchases
                // check Customer Club Card has MaximumPurchaseAmount and MaximumNumberPurchases  

                // باقیمانده اعتبار
                var remainingAmount = customerClubCard.MaximumPurchaseAmount - customerClubCard.AmountUsed;
                // باقیمانده اعتبار مخالف صفر و برابر یا بیشتر از قیمت سفارش
                if (remainingAmount != 0 && payment.Total <= remainingAmount)
                {
                    try
                    {
                        order.CustomerClubCardId = customerClubCard.Id;
                        await _orderService.UpdateAsync(order, order.Id);
                        await _orderService.UpdateOrderStatusAsync(oId.Value, OrderStatusTypesEnum.Complete);
                        await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, PaymentStatusTypesEnum.Paid,
                               PaymentMethodTypesEnum.Wallet, "پرداخت با موفقیت انجام شد", "");

                        customerClubCard.AmountUsed += payment.Total;
                        await _customerClubCardService.UpdateAsync(customerClubCard, customerClubCard.Id, true);
                    }
                    catch (Exception e)
                    {
                        _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                            "بروز خطا در پرداخت درگاه کیف پول", e);
                        throw;
                    }
                }
                else if (remainingAmount == 0 || payment.Total > remainingAmount)
                {
                    await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, PaymentStatusTypesEnum.Unsuccessful,
                           PaymentMethodTypesEnum.Wallet, "قیمت سفارش بیشتر از مقدار باقی مانده سقف خرید است", "", true);
                }
            }

            return new ResultStatus<int>
            {
                IsSuccess = true,
                ReturnUrl = Url.Action("VerifyPaymentTransaction", "WalletPayment", new { paymentId = payment.PaymentId })
            };
        }

        /// <summary>
        /// دکمه انصراف از پرداخت کیف پول
        /// </summary>
        /// <param name="cancelWithWalletParam"></param>
        /// <returns></returns>
        [HttpPost("CancelPayWithWallet")]
        public async Task<ResultStatus<int>> CancelPayWithWallet(CancelWithWalletParam cancelWithWalletParam)
        {
            // بررسی میشود که سفارش و پرداخت وجود داشته باشد
            if (cancelWithWalletParam.OId != null)
            {
                var order = await _orderService.GetAsync(cancelWithWalletParam.OId.Value, true);
                PaymentInformation payment =
                    await _orderService.CreateNewPaymentAndReturnAsync(
                        cancelWithWalletParam.OId.Value,
                        PaymentMethodTypesEnum.Wallet,
                        string.Empty,
                        string.Empty);
                if (order == null || payment == null)
                {
                    // سفارش یا =رداخت وجود ندارد
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "اطلاعات وارد شده صحیح نمیباشد"
                    };
                }

                try
                {
                    // بروزرسانی پرداخت جهت انصراف کاربر
                    await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, PaymentStatusTypesEnum.Cancel,
                          PaymentMethodTypesEnum.Wallet, "کاربر دکمه انصراف از پرداخت را زده است", "", true);
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        ReturnUrl = Url.Action("VerifyPaymentTransaction", "WalletPayment",
                            new { paymentId = payment.PaymentId })
                    };
                }
                catch
                {
                    // خطا در هنگام بروزرسانی پرداخت به انصراف کاربر
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "خطا در بروزرسانی"
                    };
                }
            }

            // خطا در هنگام بروزرسانی پرداخت به انصراف کاربر
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا"
            };
        }

        /// <summary>
        /// سفارشات منتظر تایید یارانه ساتا
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetAwaitingSataSubsidyApproval")]
        [YashilAuthorization(ActionName = "AwaitingSataSubsidyApproval")]
        public async Task<LoadResult> GetAwaitingSataSubsidyApproval(CustomDataSourceLoadOptions loadOptions)
        {
            var orders = _orderService.GetAwaitingSataSubsidyApproval();
            return await DataSourceLoader.LoadAsync(orders.ProjectTo<OrderListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// بررسی سفارش تکراری بر اساس کد ملی کاربر
        /// </summary>
        /// <param name="shoppingCartParam">پارامتر های سبد خرید</param>
        /// <returns></returns>
        [HttpPost("checkDuplicateOrderForPay")]
        public async Task<ResultStatus<int>> CheckDuplicateOrderForPay(ShoppingCartParam shoppingCartParam)
        {
            Guid guidCode = Guid.Empty;
            string nationalCode = string.Empty;
            if (!shoppingCartParam.OrderId.HasValue)
            {
                #region Find Recipient

                var code = HttpContext.Request.Cookies[CookiKey.CartId];
                if (string.IsNullOrWhiteSpace(code))
                {
                    return new ResultStatus<int>
                    { IsSuccess = false, Message = "بروز خطا در فرایند صدور پیش فاکتور", ReturnUrl = "/" };
                }

                var canParse = Guid.TryParse(code, out guidCode);
                if (!canParse)
                {
                    return new ResultStatus<int>
                    { IsSuccess = false, Message = "بروز خطا در فرایند صدور پیش فاکتور", ReturnUrl = "/" };
                }

                if (!shoppingCartParam.RecipientId.HasValue)
                    return new ResultStatus<int> { IsSuccess = false, Message = "لطفا گیرنده محصول را مشخص کنید" };
                var recipient = await _recipientService.GetAsync(shoppingCartParam.RecipientId.Value, true);
                if (recipient == null)
                {
                    return new ResultStatus<int> { IsSuccess = false, Message = "لطفا گیرنده محصول را مشخص کنید" };
                }
                // دریافت سفارشات با تایید یارانه در وضعیت ثبت اولیه می باشند
                var existSubsidyConfirmedProcessingOrder = GetResultStatusOfSubsidyBasedOrders(recipient.CardNumber, OrderStatusTypesEnum.InitialRegistration);
                if (existSubsidyConfirmedProcessingOrder.IsSuccess)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        ReturnUrl = "#",
                        Message = ResultStatusOfSubsidyBasedOrdersMessage + @$"(درصورتی که چنین سفارشی در لیست سفارشات شما وجود نداشت
            لطفا با واحد پشتیبانی به شماره {await _appConfigService.GetStringValueAsync(ApplicationConfigKey.SiteConfigurationPhone)} -021 تماس بگیرید.)"

                    };
                }

                var duplicateOrderItem = await CheckDuplicateOrderItemBeforePayAsync(recipient.NationalCode, guidCode, shoppingCartParam.OrderId);
                if (duplicateOrderItem.IsSuccess)
                {
                    return new ResultStatus<int>
                    {
                        Message = duplicateOrderItem.Message,
                        IsSuccess = true
                    };
                }
                #endregion
            }
            else
            {
                var duplicateOrderItem = await CheckDuplicateOrderItemBeforePayAsync(nationalCode, guidCode, shoppingCartParam.OrderId);
                if (duplicateOrderItem.IsSuccess)
                {
                    return new ResultStatus<int>
                    {
                        Message = duplicateOrderItem.Message,
                        IsSuccess = true
                    };
                }
            }
            return new ResultStatus<int>
            {
                IsSuccess = false
            };
        }
        /// <summary>
        /// بررسی می کند که آیا 
        /// سفارشاتی با کد ملی تحویل گیرنده در وضعیت های
        /// موفق تکمیل شده  - ثبت اولیه - درحال بررسی
        /// وجود دارد یا خیر
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private async Task<ResultStatus<int>> CheckDuplicateOrderItemBeforePayAsync(string nationalCode, Guid? shoppingCartCode, int? orderId)
        {
            var orderItem =
                    _orderItemService.GetDuplicateOrderItemBeforePay(nationalCode, shoppingCartCode, orderId);
            var orderItemList = await orderItem.ProjectTo<OrderItemListDuplicateViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (orderItemList.Count > 0)
            {
                return new ResultStatus<int>
                {
                    Message = string.Join(",", orderItemList.Select(x => x.ProductTitle)),
                    IsSuccess = true
                };
            }
            else
                return new ResultStatus<int>
                {
                    IsSuccess = false
                };

        }

        /// <summary>
        /// بروز رسانی کد ملی سفارش
        /// </summary>
        /// <param name="updateOrderNationalCodeParam"></param>
        /// <returns></returns>
        [HttpPost("updateOrderNationalCode")]
        [YashilAuthorization(ActionName = "UpdateOrderNationalCode")]
        public async Task<IActionResult> UpdateOrderNationalCode(
            UpdateOrderNationalCodeParam updateOrderNationalCodeParam)
        {

            if (!updateOrderNationalCodeParam.NationalCode.IsValidNationalCode())
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فرمت کدملی وارد شده صحیح نمیباشد لطفا پس از بررسی مجددا تلاش نمایید."
                });
            }

            var messageResult = await _orderService.UpdateOrderNationalCode(updateOrderNationalCodeParam.Id,
                updateOrderNationalCodeParam.NationalCode);
            if (messageResult != null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = messageResult
                });
            }

            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = true,
                Message = "کاربر گرامی کد ملی با موفقیت تغییر پیدا کرد."
            });
        }

        ///// <summary>
        ///// بررسی میکند آیا سفارش با یارانه تایید شده در وضعیت ثبت اولیه کاربر دارد یا خیر ؟ 
        ///// </summary>
        ///// <param name="shoppingCartParam"></param>
        ///// <returns></returns>
        //[HttpPost("CheckExistSubsidyConfirmedInitialRegistrationOrder")]
        //public async Task<ResultStatus<int>> CheckExistSubsidyConfirmedInitialRegistrationOrder(
        //    ShoppingCartParam shoppingCartParam)
        //{
        //    // پرداخت های حکمت کاربر که در وضعیت پرداخت ارسال به درگاه یا منتظر تایید پرداخت باشد، تعیین تکلیف می شود
        //    await _hekmatService.UpdateUserHekmatTypeOrderAndPaymentStatusAsync(_userPrincipal.Id);
        //    var recipient = await _recipientService.GetAsync(shoppingCartParam.RecipientId, true);
        //    if (recipient != null)
        //    {
        //        //تعداد سفارش های ثبت اولیه که یارانه تایید شده دارند را برمیگیرداند
        //        return GetResultStatusOfSubsidyBasedOrders(_userPrincipal.Id, recipient.CardNumber,
        //            OrderStatusTypesEnum.InitialRegistration);
        //    }

        //    return new ResultStatus<int>
        //    {
        //        IsSuccess = false
        //    };
        //}

        /// <summary>
        /// تعداد سفارشات دارای یارنه با توجه به وضعیت های ارسالی را برمی گرداند
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="orderStatusTypesEnum"></param>
        /// <returns></returns>
        private ResultStatus<int> GetResultStatusOfSubsidyBasedOrders(string cardNumber,
            OrderStatusTypesEnum orderStatusTypesEnum)
        {
            var orderCount =
                _orderService.GetNotRejectedSubsidyBasedOrdersCountByOrderStatusType(cardNumber,
                    orderStatusTypesEnum);
            if (orderCount > 0)
            {
                return new ResultStatus<int>
                {
                    Message = orderCount.ToString(),
                    IsSuccess = true
                };
            }
            else
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        ///  سفارش به وضعیت Canceled میرود 
        /// </summary>
        /// <param name="orderStatusParam"></param>
        /// <returns></returns>
        [HttpPost("CancelOrder")]
        public async Task<ResultStatus<int>> CancelOrder(OrderStatusParam orderStatusParam)
        {
            if (!orderStatusParam.OrderId.HasValue)
            {
                return new ResultStatus<int>
                { IsSuccess = false, Message = "بروز خطا در فرایند انصراف سفارش", ReturnUrl = "/" };
            }

            var order = await _orderService.GetAsync(orderStatusParam.OrderId.Value, true);
            if (order != null && order.UserId == _userPrincipal.Id &&
                _orderService.CheckOrderAllowCancel(order.Id, (OrderStatusTypesEnum)order.OrderStatusType))
            {
                order.OrderStatusType = (int)OrderStatusTypesEnum.Canceled;
                await _orderService.UpdateAsync(order, orderStatusParam.OrderId.Value, true);



                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    ReturnUrl = ""
                };
            }

            return new ResultStatus<int>
            { IsSuccess = false, Message = "بروز خطا در فرایند انصراف سفارش", ReturnUrl = "/" };
        }

        /// <summary>
        ///    در صورتی که اپلیکیشن مبتنی بر یارانه باشد قبل از نمایش صفحه پیش فاکتور و محاسبه یارانه برای کالاهی انتخابی چک می شود   
        ///   در صورتی که  در لیست سفارشات قبلی کاربر، سفارشی دارای یارانه تایید شده و یا ثبت اولیه با وضعیت سفارش ثبت اولیه باشد
        ///   ان سفارش  را به وضعیت لغو سیستمی تغییر می دهیم
        ///   تا یارانه این گونه سفارش ها برای سفارش جدید لحاظ شود
        /// </summary>
        /// <param name="shoppingCartParam"></param>
        /// <returns></returns>
        [HttpPost("CancelSubsidyConfirmedInitialRegistrationOrder")]
        public async Task<ResultStatus<int>> CancelSubsidyConfirmedInitialRegistrationOrder(
            ShoppingCartParam shoppingCartParam)
        {
            // پرداخت های حکمت کاربر که در وضعیت پرداخت ارسال به درگاه یا منتظر تایید پرداخت باشد، تعیین تکلیف می شود
            var isSuccessfulUpdate = await _hekmatService.UpdateUserHekmatTypeOrderAndPaymentStatusAsync(_userPrincipal.Id);
            if (isSuccessfulUpdate)
            {
                var decryptedRecipientIdInt = CryptographyHelper.Base64Decode(CryptographyHelper.AesDecrypt(shoppingCartParam.RecipientIdString));
                if (string.IsNullOrEmpty(decryptedRecipientIdInt))
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "خطا - شناسه تحویل گیرنده معتبر نمی باشد."
                    };
                }

                if (int.TryParse(decryptedRecipientIdInt, out var recipientIdInt))
                {
                    var recipient = await _recipientService.GetAsync(recipientIdInt, true);
                    if (recipient != null)
                    {
                        if (_applicationConfiguration.AddressBased && !recipient.AddressId.HasValue)
                        {
                            return new ResultStatus<int>
                            {
                                IsSuccess = false,
                                Message = "کاربر گرامی اطلاعات آدرس تحویل گیرنده انتخابی وارد نشده است، لطفا پس از اصلاح از طریق زدن دکمه افزودن یا ویرایش تحویل گیرنده اقدام نمایید"
                            };
                        }
                        var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                        if (userAdditionalUserProp.AddressBased && (string.IsNullOrEmpty(recipient.Name) || string.IsNullOrEmpty(recipient.LastName)))
                        {
                            return new ResultStatus<int>
                            {
                                IsSuccess = false,
                                Message = "کاربر گرامی اطلاعات نام و نام خانوادگی تحویل گیرنده انتخابی وارد نشده است، لطفا پس از اصلاح از طریق زدن دکمه افزودن یا ویرایش تحویل گیرنده اقدام نمایید"
                            };
                        }
                        var isSuccess = await _orderService.CancelSubsidyConfirmedInitialRegistrationOrder(_userPrincipal.Id,
                            recipient.CardNumber);
                        if (isSuccess)
                        {
                            return new ResultStatus<int>
                            {
                                IsSuccess = true
                            };
                        }
                    }
                }
                return new ResultStatus<int>
                {
                    Message = "خطا - در دریافت شناسه تحویل گیرنده.",
                    IsSuccess = false
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = false
            };

        }

        /// <summary>
        /// برای نمایش اطلاعات یک سفارش
        /// این متد در نمایش جزییات سفارش در فرم لیست همه سفارش ها کاربرد دارد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("GetOrderStatistic")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<OrderStatisticViewModel> GetOrderStatistic(int orderId)
        {

            var orderStatistic = _orderService.GetOrderStatistic(orderId);
            var orderStatisticViewModel = await orderStatistic.ProjectTo<OrderStatisticViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (orderStatisticViewModel is { OrderStatusType: (int)OrderStatusTypesEnum.IncompletePayment })
            {
                var amountForPay = await _orderService.CalcAmountForPayAsync((int)PaymentMethodTypesEnum.Ansar, orderId);
                orderStatisticViewModel.RemainingAmountToPay = amountForPay.Total;
            }

            return orderStatisticViewModel;
        }

        private Task<bool> ChecksProductSalesPlanningHaveCapacityAsync(int productSalesPlanningId)
        {
            return _productSalesPlanningService
                 .ChecksProductSalesPlanningHaveCapacityAsync(productSalesPlanningId);
        }

        /// <summary>
        /// دریافت اطلاعات تمامی سفارش های که
        /// orderStatusType
        /// آن ها برابر است با
        /// WaitingForRequiredSupplierApprovalBeforePayment
        /// می باشد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentStatus")]
        [YashilAuthorization(ActionName = "GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentStatus")]
        public async Task<LoadResult> GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentStatusAsync(CustomDataSourceLoadOptions loadOptions, int? productSalesPlanningId, int? representationId)
        {

            if (representationId is null)
                return new LoadResult()
                {
                    data = Enumerable.Empty<Order>(),
                };

            if (productSalesPlanningId is > 0)
                if (!await _productSalesPlanningService.HasCurrentUserAccessToProductSalesPlanningAsync(productSalesPlanningId.Value))
                    throw new Exception($"current user has not access to productSalesPlanning with id {productSalesPlanningId.Value}");


            return await DataSourceLoader.LoadAsync(_orderService
                .GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentAsync(
                    productSalesPlanningId, _userPrincipal.ApplicationId, true, representationId)
                .ProjectTo<OrdersWithWaitingForRequiredSupplierApprovalBeforePaymentViewModel>(
                    _mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این مد جهت بروزرسانی وضعیت یک سفارش از حالت
        /// نیاز به تایید تامین کننده به حالت منتظر پرداخت کاربرد دارد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("ChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment")]
        [YashilAuthorization(ActionName = "ChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment")]
        public async Task<ResultStatus<int>> ChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPaymentAsync(int? orderId)
        {
            if (!orderId.HasValue || orderId.Value == 0)
                return new ResultStatus<int>(false, "شناسه سفارش ارسالی معتبر نمی باشد.");

            (bool hasAccess, Order order) = await _orderService
                .HasCurrentUserAccessToChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPaymentAsync(orderId.Value);

            if (!hasAccess)
                return new ResultStatus<int>(false, "بروز خطا کاربر گرامی دسترسی لازم برای تغییر وضعیت سفارش برای شما وجود ندارد.");

            try
            {
                order.OrderStatusType = (int)OrderStatusTypesEnum.WaitingForPayment;
                await _orderService.UpdateAsync(order, order.Id, true);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                    "بروز خطا در پرداخت درگاه کیف پول", e);
                return new ResultStatus<int>(false, "بروز خطا به هنگام بروزرسانی وضعیت سفارش.");
            }

            return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");
        }

        /// <summary>
        /// این مد جهت بروزرسانی وضعیت یک سفارش از حالت
        /// نیاز به تایید تامین کننده به حالت منتظر پرداخت کاربرد دارد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("changeOrderStatusFromWaitingForSupplierApprovalToCancel")]
        [YashilAuthorization(ActionName = "changeOrderStatusFromWaitingForSupplierApprovalToCancel")]
        public async Task<ResultStatus<int>> ChangeOrderStatusFromWaitingForSupplierApprovalToCancelAsync(int? orderId)
        {
            if (!orderId.HasValue || orderId.Value == 0)
                return new ResultStatus<int>(false, "شناسه سفارش ارسالی معتبر نمی باشد.");

            (bool hasAccess, Order order) = await _orderService
                .HasCurrentUserAccessToChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPaymentAsync(orderId.Value);


            if (!hasAccess)
                return new ResultStatus<int>(false, "بروز خطا کاربر گرامی دسترسی لازم برای تغییر وضعیت سفارش برای شما وجود ندارد.");

            try
            {
                order.OrderStatusType = (int)OrderStatusTypesEnum.RejectBySupplier;
                await _orderService.UpdateAsync(order, order.Id, true);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                    "بروز خطا در پرداخت درگاه کیف پول", e);
                return new ResultStatus<int>(false, "بروز خطا به هنگام بروزرسانی وضعیت سفارش.");
            }

            return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");
        }

        /// <summary>
        /// این متد جهت تغییر بسته سفارشی که در حالت
        /// منتظر تایید توسط تامین کننده است کاربرد دارد.
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="newProductSalesPlanningId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpGet("ChangeOrderItemProductSalesPlanningForWaitingForSupplierApprovalStatus")]
        [YashilAuthorization(ActionName = "ChangeOrderItemProductSalesPlanningForWaitingForSupplierApprovalStatus")]
        public async Task<ResultStatus<int>> ChangeOrderItemProductSalesPlanningForWaitingForSupplierApprovalStatusAsync(int? orderItemId,
            int? newProductSalesPlanningId, string description)
        {
            if (!orderItemId.HasValue || orderItemId.Value == 0 ||
                !newProductSalesPlanningId.HasValue || newProductSalesPlanningId.Value == 0)
                return new ResultStatus<int>(false, "شناسه سفارش ارسالی معتبر نمی باشد.");

            if (!await _productSalesPlanningService.HasCurrentUserAccessToOrderItemProductSalesPlanningAsync(orderItemId.Value))
                return new ResultStatus<int>(false, "بروز خطا کاربر گرامی دسترسی لازم برای تغییر وضعیت سفارش برای شما وجود ندارد.");


            var modifyPspResult = await _modifiedOrderItemService
                .ChangeOrderItemProductSalesPlanningForOrderWithWaitingForSupplierApprovalStatusAsync(orderItemId.Value,
                    newProductSalesPlanningId.Value, description);

            if (!modifyPspResult.isSuccess)
                return new ResultStatus<int>(false, string.IsNullOrEmpty(modifyPspResult.message) ?
                    "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید." : modifyPspResult.message);

            return new ResultStatus<int>(true, "عملیات موفقیت آمیز انجام شد.");

        }



        /// <summary>
        /// متدی جهت بروزرسانی شماره موبایل گیرنده سفارش
        /// </summary>
        /// <param name="updateOrderRecipientMobileParam">پارامترهای مورد نیاز تغییر شماره موبایل</param>
        /// <returns></returns>
        [HttpPost("UpdateOrderRecipientMobileNumber")]
        [YashilAuthorization(ActionName = "UpdateOrderRecipientMobileNumber")]
        public async Task<IActionResult> UpdateOrderRecipientMobileNumberAsync(UpdateOrderRecipientMobileNumberParam updateOrderRecipientMobileParam)
        {

            if (!updateOrderRecipientMobileParam.RecipientMobileNumber.IsValidMobileNumber())
            {
                return BadRequest(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فرمت موبایل وارد شده صحیح نمی باشد لطفا پس از بررسی مجددا تلاش کنید."
                });
            }

            var messageResult = await _orderService.UpdateOrderRecipientMobileNumberAsync(updateOrderRecipientMobileParam.Id,
                updateOrderRecipientMobileParam.RecipientMobileNumber,updateOrderRecipientMobileParam.OldMobileNumber);

            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = messageResult.success,
                Message = messageResult.message
            });

        }

    }
    internal class LockInfo
    {
        public int Index { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}