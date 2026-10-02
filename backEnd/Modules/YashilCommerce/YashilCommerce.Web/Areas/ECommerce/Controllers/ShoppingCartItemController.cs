using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Core.Enums;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using System.Threading.Tasks;
using Yashil.Core.Web;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Classes;
using YashilUserManagement.Core.Services;
using System.Collections.Generic;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class ShoppingCartItemController : YashilMvcBaseController<ShoppingCartItem>
    {
        private readonly IMapper _mapper;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IOrderService _orderService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IAddressService _addressService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;

        public ShoppingCartItemController(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
            IYashilLogger<ShoppingCartItem> logger,
            IPermissionService permissionService,
            IShoppingCartItemService shoppingCartItemService,
            IOrderService orderService,
            StoreConfiguration storeConfiguration,

            IUserPrincipal userPrincipal,
            IAdditionalUserPropService additionalUserPropService, IAddressService addressService, IProductSalesPlanningService productSalesPlanningService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _shoppingCartItemService = shoppingCartItemService;
            _orderService = orderService;
            _storeConfiguration = storeConfiguration;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
            _addressService = addressService;
            _productSalesPlanningService = productSalesPlanningService;
        }

        /// <summary>
        /// Load Cart Item in ShoppingCart Page 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index(int cartType = (int)ShoppingCartTypeEnum.ShoppingCart)
        {
            var model = await FillShoppingCartItems();

            model.MinimumPurchaseAmountErrorMessages =
                ValidateMinimumPurchaseAmount(model.OrderItemDetailPresentationViewModels);

            return View(model);
        }

        private async Task<OrderSummaryViewModel> FillShoppingCartItems()
        {
            OrderSummaryViewModel orderSummaryViewModel = new OrderSummaryViewModel();

            #region Fill Order Items
            string code = HttpContext.Request.Cookies[CookiKey.CartId];
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (Guid.TryParse(code, out var guidCode))
                {
                    var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
                    var shoppingCartItems = _shoppingCartItemService.GetByCustomFilter((int)ShoppingCartTypeEnum.ShoppingCart, guidCode, null);

                    var shoppingCartItemPricesWithMaxSubsidyForVezarat =
                        _orderService.GetShoppingCartItemPricesForCalcPrice(
                            null, guidCode,
                            userAdditionalUserProp.SupportSubsidy,
                            userAdditionalUserProp.CustomerClubId,
                            true);
                    var shoppingCartItemPricesWithMaxSubsidyForSata =
                        _orderService.GetShoppingCartItemPricesForCalcPrice(
                            null, guidCode,
                            userAdditionalUserProp.SupportSubsidy,
                            userAdditionalUserProp.CustomerClubId
                            , false);

                    var sumVezaratSubside = shoppingCartItemPricesWithMaxSubsidyForVezarat.Sum(x => x.Subsidy);
                    var sumSataSubside = shoppingCartItemPricesWithMaxSubsidyForSata.Sum(x => x.Subsidy);
                    orderSummaryViewModel.IsResumeFillOutEnough = userAdditionalUserProp.IsResumeFillOutEnough;
                    var orderItemDetailPresentationViewModels = await shoppingCartItems.ProjectTo<OrderItemDetailPresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                    foreach (var item in orderItemDetailPresentationViewModels.ToList())
                    {
                        item.RepresentationAddress = _addressService.GetFullAddress(item.RepresentationAddress,
                            item.RepresentationAddressIsEncrypted);
                        item.SubsidyBased = (sumVezaratSubside.HasValue || sumSataSubside.HasValue) && userAdditionalUserProp.SupportSubsidy;
                        item.ProductPicture = _storeConfiguration.ProductDefaultPictureName;



                        var vezaratItemPrice = shoppingCartItemPricesWithMaxSubsidyForVezarat.FirstOrDefault(x => x.ProductSalesPlanningId == item.ProductSalesPlanningId);
                        var sataItemPrice = shoppingCartItemPricesWithMaxSubsidyForSata.FirstOrDefault(x => x.ProductSalesPlanningId == item.ProductSalesPlanningId);
                        item.EmploymentSubsidyDto = await _productSalesPlanningService.
                            CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(item.ProductSalesPlanningId);

                        if (vezaratItemPrice != null)
                        {
                            item.SubsidyPrice = vezaratItemPrice.Subsidy;
                            item.MaxSataSubsidyPrice = sataItemPrice?.Subsidy;
                            item.MaxVezaratSubsidyPrice = vezaratItemPrice.Subsidy;
                            item.Price = vezaratItemPrice.Price;
                            item.Total = vezaratItemPrice.Total;
                            if (vezaratItemPrice.CalcedPrice != null)
                                item.CalcPrice = vezaratItemPrice.CalcedPrice.Value;

                            item.DiscountPercentage = vezaratItemPrice.DiscountPercentage;
                            item.DiscountAmountInclTax = vezaratItemPrice.Discount;

                        }
                        else
                        {
                            //  چون محصول اضافه شده در سبد خرید برای باشگاه کاربر لاگین کرده ویژگی تنظیم فروش فعال ندارد
                            //  این محصول را از سبد خرید کاربر حذف می کنیم
                            if (_userPrincipal.Id != 0)
                            {
                                orderItemDetailPresentationViewModels.Remove(item);
                                await _shoppingCartItemService.DeleteAsync(item.ShoppingCartItemId, true);
                            }
                        }
                    }
                    /*لیست آیتم های سفارش پر می شود*/
                    orderSummaryViewModel.OrderItemDetailPresentationViewModels = orderItemDetailPresentationViewModels;

                    #endregion
                    #region Fill Order Summary
                    var orderSummaryForPayViewModel = shoppingCartItemPricesWithMaxSubsidyForVezarat.GroupBy(x => 1).Select(c =>
                                new OrderSummaryForPayViewModel
                                {
                                    SumDiscount = c.Sum(x => x.Discount),
                                    SumPrice = c.Sum(x => x.Price),
                                    SumMaxSubsidySata = sumSataSubside,
                                    SumMaxSubsidyVezarat = sumVezaratSubside,
                                    AllItemsHasValue = c.All(x => x.CalcedPrice.HasValue),
                                    OrderItemsCount = c.Sum(x => x.Quantity),
                                    SumWage = c.Sum(x => x.Wage),
                                }
                    ).FirstOrDefault();
                    if (orderSummaryForPayViewModel != null)
                    {
                        orderSummaryForPayViewModel.SubsidyBased = (sumVezaratSubside.HasValue || sumSataSubside.HasValue) && userAdditionalUserProp.SupportSubsidy;
                        orderSummaryViewModel.OrderSummaryForPayViewModel = orderSummaryForPayViewModel;
                    }
                    #endregion
                }
            }
            return orderSummaryViewModel;
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر حداقل مبلغ خرید
        /// مربوط به یک تنظیم فروش را رعایت کرده است یا نه
        /// برخی از تنظیم فروش ها می توانند دارای حداقل مبلغ خرید باشند و
        /// کاربر در صورتی که از آن مبلغ کمتر خرید کرده باشد نمی تواند خرید خود را نهایی کند.
        /// </summary>
        /// <param name="orderItems"></param>
        /// <returns></returns>
        [NonAction]
        private List<string> ValidateMinimumPurchaseAmount(List<OrderItemDetailPresentationViewModel> orderItems)
        {
            var errorMessages = new List<string>();

            var groupedOrders = orderItems.Where(x =>
                    x.MinimumPurchasePrice.HasValue)
                .Select(x => new
                {
                    CalcPrice = x.CalcPrice,
                    SaleConfiguraionId = x.SaleConfigurationId,
                    MinimumPurchaseAmount = x.MinimumPurchasePrice,
                    ProductTitle = x.Title,

                }).GroupBy(c => c.SaleConfiguraionId);

            if (groupedOrders == null || !groupedOrders.Any())
                return errorMessages;


            foreach (var group in groupedOrders)
            {
                var sumOfCalcPrices = group.Sum(x => x.CalcPrice);

                var minimumPurchaseAmount = group.First().MinimumPurchaseAmount!.Value;

                if (sumOfCalcPrices < minimumPurchaseAmount)
                {
                    foreach (var item in group)
                    {
                        errorMessages.Add("کاربر گرامی حداقل مبلغ خرید از مرکز یا نمایندگی مربوط به" +
                                          $" محصول '{item.ProductTitle}' برابر {item.MinimumPurchaseAmount:N0} ریال می باشد" +
                                          " .در صورت عدم رعایت حداقل مبلغ خرید امکان ادامه سفارش نمی باشد");
                    }
                }
            }

            return errorMessages;
        }







    }
}

