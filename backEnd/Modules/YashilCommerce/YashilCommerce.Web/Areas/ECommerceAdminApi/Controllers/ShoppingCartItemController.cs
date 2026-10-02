using System;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core.Enums;
using Yashil.Core.Web;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Core.Classes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ShoppingCartItemController : BaseController<ShoppingCartItem, int, ShoppingCartItemListViewModel, ShoppingCartItemEditModel, ShoppingCartItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IRepresentationService _representationService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IWebHelper _webHelper;

        public ShoppingCartItemController(IShoppingCartItemService shoppingCartItemService, ISaleConfigurationService saleConfigurationService,
            ApplicationConfiguration applicationConfiguration, IUserPrincipal userPrincipal,
            IMapper mapper, IYashilLogger<ShoppingCartItem> logger, IPermissionService permissionService,
            IProductSalesPlanningService productSalesPlanningService, IRepresentationService representationService,
            IWebHelper webHelper, StoreConfiguration storeConfiguration) : base(shoppingCartItemService, mapper,
                logger, permissionService)
        {
            _mapper = mapper;
            _shoppingCartItemService = shoppingCartItemService;
            _productSalesPlanningService = productSalesPlanningService;
            _representationService = representationService;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _saleConfigurationService = saleConfigurationService;
            _webHelper = webHelper;
            _storeConfiguration = storeConfiguration;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? shoppingCartType, Guid? code, int? productSalesPlanningId)
        {
            var shoppingCartItems = _shoppingCartItemService.GetByCustomFilter(shoppingCartType, code, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(shoppingCartItems.ProjectTo<ShoppingCartItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? shoppingCartType, Guid? code, int? productSalesPlanningId)
        {
            var shoppingCartItems = _shoppingCartItemService.GetByCustomFilter(shoppingCartType, code, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(shoppingCartItems.ProjectTo<ShoppingCartItemSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// Add To Shopping Cart Item
        /// </summary>
        /// <param name="addToCartViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("AddToCart")]
        public async Task<ResultStatus<int>> AddToCart(AddToCartViewModel addToCartViewModel)
        {
            try
            {
                if (addToCartViewModel.ProductSalesPlanningId.HasValue)
                {
                    bool existsProductSalePlanningInApplication = await _productSalesPlanningService.CheckExistsProductSalePlanningInApplicationAsync(addToCartViewModel.ProductSalesPlanningId.Value, _applicationConfiguration.ApplicationId, true);

                    if (existsProductSalePlanningInApplication)
                    {
                        string code = HttpContext.Request.Cookies[CookiKey.CartId];
                        Guid guidCode = default;
                        if (string.IsNullOrWhiteSpace(code))
                        {
                            guidCode = Guid.NewGuid();
                        }
                        else if (!string.IsNullOrWhiteSpace(code))
                        {
                            if (!Guid.TryParse(code, out guidCode))
                            {
                                guidCode = Guid.NewGuid();
                            }
                            else
                            {
                                //بررسی می کند که آیا محصولی از نوع نیاز به تایید تامین کننده 
                                //قبل از پرداخت در سبد خرید وجود دارد یا نه
                                if (await _shoppingCartItemService
                                        .CheckExistProductRequireSupplierApprovalBeforePaymentAsync(guidCode))
                                {


                                    return new ResultStatus<int>(false,
                                        "کاربر گرامی در حالتی که محصول موجود در سبد خرید از نوع نیاز به تایید تامین کننده پیش از خرید باشد امکان افزودن محصول جدید پیش از تعیین تکلیف محصول جاری وجود ندارد.");

                                }

                                //بررسی می کند که اگر ایتمی در سبد خرید هست که از نوع نیاز به 
                                //تایید کننده قبل از خرید وجود نداشته باشد
                                //و
                                //محصولی که الان در حال اضافه کردن است از نوع نیاز به تایید قبل پراخت نباشد
                                //یعنی اگر محصولی عادی در سبد خرید باشد امکان افزودن محصول از نوع نیاز به تایید تامین کننده 
                                //وجود ندارد
                                if (await _shoppingCartItemService

                                    .CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(guidCode) &&
                                    await _productSalesPlanningService
                                        .IsProductNeedToSupplierApprovalBeforePaymentAsync
                                            (addToCartViewModel.ProductSalesPlanningId.Value))
                                {
                                    return new ResultStatus<int>(false,
                                        "کاربر گرامی شما محصولی از نوع عادی در سبد خرید خود دارید در این حالت امکان افزودن محصولی که از نوع نیاز به تایید تامین کننده قبل از خرید است وجود ندارد لازم است محصول جدید نیز از نوع عادی باشد");

                                }
                            }
                        }

                        // نمایندگی مربوط به برنامه ریزی فروش
                        int? representationId = await _productSalesPlanningService.GetRepresentationIdByProductSalesPlanningIdAsync(addToCartViewModel.ProductSalesPlanningId.Value, _applicationConfiguration.ApplicationId);
                        if (!representationId.HasValue && addToCartViewModel.RepresentationId.HasValue)
                        {
                            //در این حالت نمایندگی را کاربر انتخاب کرده است و محصول برای نمایندگی برنامه ریزی فروش ندارد
                            var representation = await _representationService.GetAsync(addToCartViewModel.RepresentationId.Value, true);
                            if (representation != null)
                            {
                                representationId = representation.Id;
                            }
                        }
                        if (representationId.HasValue)
                        {
                            int shoppingCartType = (int)ShoppingCartTypeEnum.ShoppingCart;
                            // Set ShoppingCartType WishList or ShoppingCart
                            if (addToCartViewModel.CartType == (int)ShoppingCartTypeEnum.WishList)
                            {
                                shoppingCartType = (int)ShoppingCartTypeEnum.WishList;
                            }
                            //Get CartId From Cookie and set to Code in ShoppingCartItem Entity
                            // CartId type is Guid : Uniq For each user 

                            _webHelper.AddCookie(CookiKey.CartId, guidCode.ToString(), DateTimeOffset.Now.AddDays(1));


                            var currentProductSaleConfigurationAttribute =
                                await _saleConfigurationService.GetSaleConfigurationAttributeByCurrentUserCustomerClubAsync(
                                    addToCartViewModel.ProductSalesPlanningId.Value);


                            if (currentProductSaleConfigurationAttribute == null)
                            {
                                return new ResultStatus<int> { IsSuccess = false, Message = "برای محصول انتخاب شده برنامه ریزی فروش تعریف نشده است" };
                            }

                            var saleConfigurationAttributes = await
                                _saleConfigurationService.GetSaleConfigurationAttributeByShoppingCartCodeAsync(
                                    guidCode,
                                    _applicationConfiguration.ApplicationId);





                            //var allProductsInSameInstallmentBasedMode =
                            //    saleConfigurationAttributes.Count(x => 
                            //        x.AllowedToPayInInstallment == currentProductSaleConfigurationAttribute.AllowedToPayInInstallment)
                            //    == saleConfigurationAttributes.Count;
                            //if (!allProductsInSameInstallmentBasedMode)
                            //{
                            //    return new ResultStatus<int> { IsSuccess = false, Message = "تمامی محصولات یک سبد خرید باید از یک نوع نقدی یا اقساطی باشند!" };
                            //}

                            saleConfigurationAttributes.Add(currentProductSaleConfigurationAttribute);

                            bool saleConfigurationAttributeSaleTypesIsCompatible = _saleConfigurationService.CheckSaleConfigurationAttributeSaleTypes(saleConfigurationAttributes);

                            if (!saleConfigurationAttributeSaleTypesIsCompatible)
                            {
                                return new ResultStatus<int> { IsSuccess = false, Message = "تمامی محصولات یک سبد خرید باید مدل پرداخت نقدی یا اقساطی  باشند." };
                            }

                            bool allProductsHasSameInstallmentCount = await _shoppingCartItemService.CheckAllSaleConfigurationsHasSameInstallmentCountAsync(
                                shoppingCartType,
                                guidCode,
                                addToCartViewModel.ProductSalesPlanningId.Value,
                                saleConfigurationAttributes);

                            if (!allProductsHasSameInstallmentCount)
                            {
                                return new ResultStatus<int> { IsSuccess = false, Message = "تعداد اقساط تمام کالاهای یک سبد خرید باید یکسان باشد!" };
                            }
                            var checkDuplicate = await CheckDuplicateItemsInTheCart(addToCartViewModel);
                            if (!checkDuplicate.IsSuccess)
                            {
                                return new ResultStatus<int>
                                {
                                    IsSuccess = false,
                                    Message = "کاربر گرامی این محصول قبلا به سبد خرید شما اضافه شده است"
                                };
                            }
                            await _shoppingCartItemService.AddShoppingCartItemAync(new ShoppingCartItem
                            {
                                Code = guidCode,
                                ProductSalesPlanningId = addToCartViewModel.ProductSalesPlanningId.Value,
                                ShoppingCartType = shoppingCartType,
                                RepresentationId = representationId.Value,
                                Quantity = addToCartViewModel.Quantity ?? 1,
                                CreateBy = _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id,
                                CreationDate = DateTime.Now,
                                ApplicationId = _applicationConfiguration.ApplicationId
                            }, true);
                            return new ResultStatus<int> { IsSuccess = true };
                        }
                    }
                    else
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "کابر گرامی محصول انتخاب شده قابل فروش نمی باشد"
                        };
                    }
                }
                return new ResultStatus<int> { IsSuccess = false };
            }
            catch (Exception e)
            {
                return new ResultStatus<int> { IsSuccess = false, Message = "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید." };
            }

        }

        /// <summary>
        /// بررسی میکند که محصول برنامه ریزی شده در هنگام اضافه به سبد خرید، در سبد خرید وجود دارد یا خیر 
        /// </summary>
        /// <param name="addToCartViewModel"></param>
        /// <returns>در صورتی که مقدار بازگشتی صحیح بود نمی تواند به سبد خدید اضافه کند و در غیر این صورت می تواند</returns>
        [AllowAnonymous]
        [HttpPost("CheckDuplicateItemsInTheCart")]
        public async Task<ResultStatus<int>> CheckDuplicateItemsInTheCart(AddToCartViewModel addToCartViewModel)
        {
            string code = HttpContext.Request.Cookies[CookiKey.CartId];
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (Guid.TryParse(code, out Guid guidCode) && addToCartViewModel.ProductSalesPlanningId.HasValue)
                {

                    //تعداد آیتم های تکراری در سبد خرید را برمیگرداند
                    var duplicateItemQuantity = await _shoppingCartItemService.GetDuplicateItemQuantityAsync(addToCartViewModel.CartType, guidCode, addToCartViewModel.ProductSalesPlanningId.Value, addToCartViewModel.RepresentationId);
                    if (duplicateItemQuantity > 0)
                    {
                        if (duplicateItemQuantity >= _storeConfiguration.MaxShoppingCardItemQuantity)
                        {
                            return new ResultStatus<int>
                            {
                                IsSuccess = false,
                                Message = duplicateItemQuantity.ToString(),
                            };
                        }
                        return new ResultStatus<int>
                        {
                            IsSuccess = true,
                            Message = duplicateItemQuantity.ToString()
                        };
                    }

                }
            }
            return new ResultStatus<int> { IsSuccess = true };
        }

        /// <summary>
        /// Delete Shopping Cart Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cartType"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Delete/{id}")]
        public async Task<ResultStatus<int>> DeleteAsync(int id, int cartType = (int)ShoppingCartTypeEnum.ShoppingCart)
        {
            //Get CartId From Cookie For Delete  ShoppingCartItem
            string code = HttpContext.Request.Cookies[CookiKey.CartId];
            if (!string.IsNullOrWhiteSpace(code) && Guid.TryParse(code, out var guidCode))
            {
                var deleteResult = await _shoppingCartItemService.DeleteShoppingCartItemAsync(cartType, id, guidCode, true);

                return new ResultStatus<int>
                {
                    IsSuccess = string.IsNullOrEmpty(deleteResult),
                    Message = string.IsNullOrEmpty(deleteResult) ? "آیتم سبد خرید با موفقیت حذف شد" : deleteResult,
                    ReturnUrl = "/ShoppingCartItem"
                };
            }

            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در حذف آیتم سبد خرید",
                ReturnUrl = "/ShoppingCartItem"
            };
        }
    }
}