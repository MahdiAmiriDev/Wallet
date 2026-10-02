using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ShoppingCartItemService : GenericService<ShoppingCartItem, int>, IShoppingCartItemService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IAppConfigService _appConfigService;
        private readonly IYashilLogger<ShoppingCartItem> _logger;


        public ShoppingCartItemService(
            IUnitOfWork unitOfWork,
            IShoppingCartItemRepository shoppingCartItemRepository,
            IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration,
            IEventPublisher eventPublisher,
            IProductSalesPlanningService productSalesPlanningService,
            ISaleConfigurationService saleConfigurationService, IAppConfigService appConfigService, IYashilLogger<ShoppingCartItem> logger)
            : base(unitOfWork, shoppingCartItemRepository, userPrincipal, eventPublisher)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _applicationConfiguration = applicationConfiguration;
            _productSalesPlanningService = productSalesPlanningService;
            _saleConfigurationService = saleConfigurationService;
            _appConfigService = appConfigService;
            _logger = logger;
        }
        public IQueryable<ShoppingCartItem> GetByCustomFilter(int? shoppingCartType, Guid? code, int? productSalesPlanningId)
        {
            return _shoppingCartItemRepository.GetByCustomFilter(shoppingCartType, code, productSalesPlanningId, _applicationConfiguration.ApplicationId);
        }

        public async Task AddShoppingCartItemAync(ShoppingCartItem shoppingCartItem, bool saveAfterAdd = false)
        {
            //TODO: Check  For Attribute Update
            int count = _shoppingCartItemRepository.GetShoppingCartCount(shoppingCartItem.ShoppingCartType, shoppingCartItem.Code, _applicationConfiguration.ApplicationId);

            var maximumItemCount =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceShoppingCartItemMaximumItemCount);

            if (count >= maximumItemCount)
                return;


            var currentShoppingCartItem = _shoppingCartItemRepository.GetShoppingCartItem(shoppingCartItem.ShoppingCartType, shoppingCartItem.Code, shoppingCartItem.ProductSalesPlanningId, _applicationConfiguration.ApplicationId);
            if (currentShoppingCartItem != null && currentShoppingCartItem.RepresentationId == shoppingCartItem.RepresentationId)
            {
                currentShoppingCartItem.Quantity = currentShoppingCartItem.Quantity + 1;
                await UpdateAsync(currentShoppingCartItem, currentShoppingCartItem.Id, saveAfterAdd);
            }
            else
            {
                await AddAsync(shoppingCartItem, saveAfterAdd);
            }
        }

        public async Task<string> DeleteShoppingCartItemAsync(int cartType, int id, Guid code, bool saveAfterDelete)
        {
            var shoppingCartItem = _shoppingCartItemRepository.Get(id, true);
            if (shoppingCartItem != null && shoppingCartItem.Code == code && shoppingCartItem.ShoppingCartType == cartType)
            {
                if (shoppingCartItem.Quantity == 1)
                {
                    await DeleteAsync(shoppingCartItem.Id, saveAfterDelete);
                }
                else
                {
                    shoppingCartItem.Quantity--;
                    await UpdateAsync(shoppingCartItem, shoppingCartItem.Id, saveAfterDelete);
                }

                return string.Empty;
            }

            _logger.InsertLog(LogLevel.Error, (int)Events.AccessDeniedDeleteEntity,
                nameof(DeleteShoppingCartItemAsync), "بروز خطا به هنگام تایید اعتبار درخواست حذف کالا از سبد خرید ", new Exception("Error occurred in deleting shopping cart item"));

            return "بروز خطا در حذف ایتم سبد خرید.";
        }

        public int GetShoppingCartItemsCount(int shoppingCartType, Guid code)
        {
            return _shoppingCartItemRepository.GetShoppingCartCount(shoppingCartType, code, _applicationConfiguration.ApplicationId);
        }

        public bool ClearShoppingCart(int shoppingCartType, Guid guidCode, bool saveAfterClear)
        {
            _shoppingCartItemRepository.ClearShoppingCart(shoppingCartType, guidCode);
            if (saveAfterClear)
            {
                SaveChangeAsync();
            }

            return true;
        }

        public Task<int> GetDuplicateItemQuantityAsync(int cartType, Guid guidCode, int productSalesPlanningId, int? representationId)
        {
            return _shoppingCartItemRepository.GetDuplicateItemQuantityAsync(cartType, guidCode, productSalesPlanningId, representationId, _applicationConfiguration.ApplicationId);
        }

        public async Task<bool> CheckAllSaleConfigurationsHasSameInstallmentCountAsync(int shoppingCartType,
            Guid guidCode,
            int productSalesPlanningId,
            List<SaleConfigurationAttributeDto> saleConfigurationAttributeDtoList)
        {
            if (saleConfigurationAttributeDtoList.Count == 0)
            {
                return true;
            }

            var newProductSalesPlanningPrice = await _productSalesPlanningService
                .FindById(productSalesPlanningId)
                .Select(x => new ProductSalesPlanningPriceDto
                {
                    SaleConfigurationId = x.SaleConfigurationId,
                    BasePrice = x.Price
                }).FirstOrDefaultAsync();


            var productSalePlanningPrice = await _shoppingCartItemRepository
                .GetByCustomFilter(shoppingCartType, guidCode, null,
                    _applicationConfiguration.ApplicationId)
                .Select(x => new ProductSalesPlanningPriceDto()
                {
                    SaleConfigurationId = x.ProductSalesPlanning.SaleConfigurationId,
                    BasePrice = x.ProductSalesPlanning.Price
                }).ToListAsync();

            productSalePlanningPrice.Add(newProductSalesPlanningPrice);


            return await _saleConfigurationService.CheckAllSaleConfigurationsHasSameInstallmentCountAsync(
                productSalePlanningPrice,
                saleConfigurationAttributeDtoList);
        }

        public Task<bool> CheckExistProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode)
        {
            return _shoppingCartItemRepository.CheckExistProductRequireSupplierApprovalBeforePaymentAsync(guidCode);
        }

        public Task<bool> CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode)
        {
            return _shoppingCartItemRepository.CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(guidCode);
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر محصولی در سبد خرید خود اضافه کرده است یا نه؟
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<bool> AnyItemExistsInShoppingCartAsync(Guid code)
        {
            return GetByCustomFilter((int)ShoppingCartTypeEnum.ShoppingCart, code, null).AnyAsync();
        }
    }


}
