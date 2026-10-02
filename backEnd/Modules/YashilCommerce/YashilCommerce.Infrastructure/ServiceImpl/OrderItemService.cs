using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Enums;
using Yashil.Core.CustomDtos.Payment;
using YashilCommerce.Infrastructure.RepositoryImpl;
using Aspose.Cells.DigitalSignatures;
using Google.Protobuf.WellKnownTypes;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class OrderItemService : GenericService<OrderItem, int>, IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IVocherService _voucherService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IProductSalesPlanningDiscountService _productSalesPlanningDiscountService;



        public OrderItemService(
            IUnitOfWork unitOfWork,
            IVocherService voucherService,
            IOrderItemRepository orderItemRepository,
            IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher,
            ApplicationConfiguration applicationConfiguration,
            IAdditionalUserPropService additionalUserPropService,
            IProductSalesPlanningDiscountService productSalesPlanningDiscountService)
            : base(unitOfWork, orderItemRepository, userPrincipal, eventPublisher)
        {
            _orderItemRepository = orderItemRepository;
            _userPrincipal = userPrincipal;
            _voucherService = voucherService;
            _applicationConfiguration = applicationConfiguration;
            _additionalUserPropService = additionalUserPropService;
            _productSalesPlanningDiscountService = productSalesPlanningDiscountService;
        }

        public IQueryable<OrderItem> GetByCustomFilter(int? orderId, int? productSalesPlanningId, int? companyId, int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null, bool fillNotModified = false)
        {
            return _orderItemRepository.GetByCustomFilter(orderId, productSalesPlanningId, companyId, representationId, saleType, showCompletedOrderStatus, orderStatusType, orderCreationDateFromDate, orderCreationDateToDate, recipientNationalCode, mobileNumber, cardNumber, fillNotModified);
        }

        public IQueryable<OrderItem> GetUserOrderItems()
        {
            return _orderItemRepository.GetUserOrderItems();
        }

        public IQueryable<OrderItem> GetUserOrderItemsByOrderId(int orderId)
        {
            return _orderItemRepository.GetUserOrderItemsByOrderId(orderId);
        }

        public IQueryable<OrderItem> GetCurrentUserOrderItems(int? orderId, int applicationId)
        {
            return _orderItemRepository.GetCurrentUserOrderItems(orderId, applicationId);
        }
        public void AssignVoucherForOrderItems(int applicationId)
        {
            _voucherService.AssignVocherForOrderItems(applicationId);
        }
        /// <summary>
        /// قبل از پرداخت آیتم سفارشات تکراری را بر میگرداند 
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetDuplicateOrderItemBeforePay(string nationalCode, Guid? shoppingCartCode, int? orderId)
        {
            return _orderItemRepository.GetDuplicateOrderItemBeforePay(nationalCode, shoppingCartCode, orderId);
        }

        public IQueryable<OrderItem> GetByModifiedOrderItemId(int modifiedOrderItemId)
        {
            return _orderItemRepository.GetByModifiedOrderItemId(modifiedOrderItemId);
        }

        public async Task<OrderItem> CreateOrderItemEntityAsync(
           int saleConfigurationId,
           decimal saleConfigurationDiscountPercentage,
           decimal profitPercentage,
           decimal insurancePercentage,
           decimal otherDeductionsPercentage,
           decimal wagePercentage,
           CalcedOrderItemPrice calculatedOrderItemPriceWithSubsidy,
           CalcedOrderItemPrice calculatedOrderItemPriceWithoutSubsidy,
           decimal? subsidyProfitPercentage,
           int productSalesPlanningId,
           int representationId,
           int applicationId,
           int orderUserId,
           bool isOtherDeductionsIncludeInSupplierInvoice,
           int quantity = 1
        )
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(orderUserId);

            if (calculatedOrderItemPriceWithoutSubsidy.CalcedPrice.HasValue)
            {
                var productSalePlanningDiscountPercentage =
                   await _productSalesPlanningDiscountService.GetAllByApplicationId(
                            _applicationConfiguration.ApplicationId, true)
                        .Where(x =>
                            x.ProductSalesPlanningId == productSalesPlanningId
                            && x.CustomerClubId == userAdditionalUserProp.CustomerClubId)
                        .Select(x => (decimal?)x.DiscountPercentage)
                        .FirstOrDefaultAsync();

                //سود
                var profitPrice = calculatedOrderItemPriceWithoutSubsidy.Price * (profitPercentage / 100);
                //هزینه ارسال
                var additionalShippingCharge = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.AdditionalShippingCharge
                    : calculatedOrderItemPriceWithoutSubsidy.AdditionalShippingCharge;

                var netPrice = calculatedOrderItemPriceWithoutSubsidy.CalcedPrice.Value + additionalShippingCharge - profitPrice;
                var insurancePrice = netPrice * (insurancePercentage / 100);
                var otherDeductionsPrice = netPrice * (otherDeductionsPercentage / 100);

                //مقدار allProductsIncludeInsurance
                //را زا تنظیم فروش می خوانیم و در 
                //OrderItem
                //قرار می دهیم
                bool allProductsIncludeInsurance = await _orderItemRepository.GetSaleConfigurationAllProductsIncludeInsuranceAsync(saleConfigurationId);


                return new OrderItem
                {
                    SaleConfigurationId = saleConfigurationId,
                    CreateBy = _userPrincipal.Id,
                    ApplicationId = applicationId,
                    CreationDate = DateTime.Now,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    ProductSalesPlanningId = productSalesPlanningId,
                    RepresentationId = representationId,
                    Quantity = quantity,
                    Price = calculatedOrderItemPriceWithoutSubsidy.Price,
                    BasePrice = calculatedOrderItemPriceWithoutSubsidy.BasePrice,
                    DiscountPercentage = productSalePlanningDiscountPercentage ?? saleConfigurationDiscountPercentage,
                    DiscountAmountExclTax = calculatedOrderItemPriceWithoutSubsidy.Discount,
                    PriceIncDiscount = calculatedOrderItemPriceWithoutSubsidy.CalcedPrice.Value,
                    WageAmountExcTax = calculatedOrderItemPriceWithoutSubsidy.Wage,
                    SubsidyPrice = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.Subsidy
                        : null,
                    SubsidyProfitPercentage = subsidyProfitPercentage,
                    SubsidyPriceIncProfit = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.Subsidy * (1 + subsidyProfitPercentage / 100)
                        : null,
                    SubsidyPriceIncTaxIncProfit = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.Subsidy *
                          (1 + subsidyProfitPercentage / 100) * 1
                        : null,
                    TaxPercentage = calculatedOrderItemPriceWithoutSubsidy.TaxPercentage,
                    TotalExclSubsidy = calculatedOrderItemPriceWithoutSubsidy.Total,
                    TotalInclSubsidy = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.Total
                        : null,
                    TaxPriceExclSubsidy = calculatedOrderItemPriceWithoutSubsidy.Tax,
                    TaxPriceInclSubsidy = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.Tax
                        : null,

                    ProfitPercentage = profitPercentage,
                    ProfitPrice = profitPrice,

                    InsurancePercentage = insurancePercentage,
                    InsurancePrice = insurancePrice,

                    OtherDeductionsPercentage = otherDeductionsPercentage,
                    OtherDeductionsPrice = otherDeductionsPrice,

                    ValueAddedPrice = profitPrice + otherDeductionsPrice,
                    AdditionalShippingCharge = additionalShippingCharge,
                    WagePercentage = wagePercentage,
                    CitySubsidyFactor = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.CitySubsidyFactor : null,

                    RepresentationSubsidyFactor = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.RepresentationSubsidyFactor : null,

                    ProductSubsidyFactor = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.ProductSubsidyFactor : null,

                    CategorySubsidyFactor = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.CategorySubsidyFactor : null,

                    SubsidyCeilingPerHekmatCard = userAdditionalUserProp.SupportSubsidy
                    ? calculatedOrderItemPriceWithSubsidy.SubsidyCeilingPerHekmatCard : null,

                    SubsidyId = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.SubsidyId
                        : null,
                    TaxCategoryId = calculatedOrderItemPriceWithoutSubsidy.TaxCategoryId,
                    CategoryGroupId = calculatedOrderItemPriceWithoutSubsidy.CategoryGroupId,
                    IsTaxExempt = calculatedOrderItemPriceWithoutSubsidy.IsTaxExempt,
                    AllProductsIncludeInsurance = allProductsIncludeInsurance,
                    HasTaxCertificate = userAdditionalUserProp.SupportSubsidy
                        ? calculatedOrderItemPriceWithSubsidy.HasTaxCertificate
                        : calculatedOrderItemPriceWithoutSubsidy.HasTaxCertificate,
                    IsOtherDeductionsIncludeInSupplierInvoice = isOtherDeductionsIncludeInSupplierInvoice
                };
            }

            return null;
        }

        public OrderItem GetOrderItem(int orderId, int productSalesPlanningId, int representationId)
        {
            return _orderItemRepository.GetOrderItem(orderId, productSalesPlanningId, representationId);
        }

        public IQueryable<OrderItem> GetOrderItemsByOrderId(int orderId, bool readOnly = false)
        {
            return _orderItemRepository.GetOrderItemsByOrderId(orderId, readOnly);
        }

        public IQueryable<OrderItemFullInfoView> GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum subsidyTypesEnum, int orderCreationDateFromDate, int orderCreationDateToDate)
        {
            return _orderItemRepository.GetSubsidyBasedOrderItemFullInfoView(subsidyTypesEnum, orderCreationDateFromDate, orderCreationDateToDate);
        }

        public IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, bool readOnly = false)
        {
            return _orderItemRepository.GetCompletePayOrderItems(applicationId, readOnly);
        }

        public Task<int> DeleteWithChildrenLogicalAsync(int orderItemId)
        {
            return _orderItemRepository.DeleteWithChildrenLogicalAsync(orderItemId);
        }

        public IQueryable<OrderItem> GetAllWithDeletedOrderItems(int orderId)
        {
            return _orderItemRepository.GetAllWithDeletedOrderItems(orderId);
        }



        #region  factor Services

        /// <summary>
        /// get all orderItems for the specific user based on company Id or representation Id
        /// </summary>
        /// <param name="taxCategoryId">tax category Id</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetUnFactoredOrderItemsInTaxCategory(int taxCategoryId, bool readOnly = false)
        {
            return _orderItemRepository.GetUnFactoredOrderItemsInTaxCategory(taxCategoryId, readOnly);
        }

        public IQueryable<OrderItem> GetUserUnFactoredOrderItems(bool readOnly = false)
        {
            return _orderItemRepository.GetUnFactoredOrderItems(readOnly);
        }
        public bool CheckExistenceUnFactoredOrderItemsInTaxCategory(int taxCategoryId)
        {
            return _orderItemRepository.CheckExistenceUnFactoredOrderItemsInTaxCategory(taxCategoryId);
        }
        /// <summary>
        /// آیا این محصول در آیتم سفارشات وجود دارد 
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns></returns>
        public bool IsProductInOrderItems(int productId)
        {
            return _orderItemRepository.IsProductInOrderItems(productId);

        }

        public Task<int> SetOrderItemPriceRightShareAsync(int orderId)
        {
            var userId = _userPrincipal.Id != 0 ? _userPrincipal.Id : _applicationConfiguration.SysUserId;
            return _orderItemRepository.SetOrderItemPriceRightShareAsync(orderId, userId);
        }



        /// <summary>
        /// دریافت آیتم های فرزند بر اساس شناسه آیتم سفارش
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش </param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetOrderItemChildrenByParentId(int orderItemId, int applicationId)
        {
            return _orderItemRepository.GetOrderItemChildrenByParentId(orderItemId, applicationId);
        }

        #endregion
        public IQueryable<OrderItem> GetAllOrderItemsByFilter(int productSalesPlanningId, bool considerDeleted = true, bool readOnly = false)
        {
            return _orderItemRepository.GetAllOrderItemsByFilter(productSalesPlanningId, considerDeleted, readOnly);
        }

        /// <summary>
        /// این متد
        /// orderItem
        /// را بر اساس شناسه آن در صورتی که به کاربر تعلق داشته باشد
        /// را برمیگرداند
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        public Task<OrderItem> GetCurrentUserOrderItemAsync(int orderItemId)
        {
            return GetAll(false)
                .Where(orderItem => orderItem.Id == orderItemId &&
                                    orderItem.Order.UserId == _userPrincipal.Id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// فیلد دارای گزارش ارزش افزروده در آیتم سفارش های فاکتور نشده مربوط به این تنظیم فروش 
        /// را به روز رسانی می کنیم
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <param name="hasTaxCertificate">دارای گواهی ارزش افزوده</param>
        /// <returns></returns>
        public Task<int> UpdateHasTaxCertificateForUnFactoredOrderItemBySaleConfigurationIdAsync(int saleConfigurationId, bool hasTaxCertificate)
        {
            return _orderItemRepository.GetUnFactoredOrderItemBySaleConfigurationId(
                  saleConfigurationId).Where(x => x.HasTaxCertificate != hasTaxCertificate)
                  .UpdateFromQueryAsync(oi => new OrderItem
                  {
                      HasTaxCertificate = hasTaxCertificate
                  });
        }

        public Task<int> GetOrderItemRepresentationIdAsync(int orderItemId)
        {
            return GetAll(true)
                .Where(x => x.Id == orderItemId)
                .Select(c => c.RepresentationId)
                .FirstOrDefaultAsync();

        }

        /// <summary>
        /// دریافت لیست سفارشاتی که پس از پرداخت وجه عودت شده و دارای یارانه است
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="subsidyType"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetAllRefundedOrderItems(int? factorId, int? subsidyType, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
            return _orderItemRepository.GetAllRefundedOrderItems(factorId, subsidyType, orderCreationDateFromDate, orderCreationDateToDate);
        }

    }
}
