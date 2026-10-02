using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using System;
using System.Threading.Tasks;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Enums;
using Yashil.Core.CustomDtos.Payment;

namespace YashilCommerce.Core.Services
{
    public interface IOrderItemService : IGenericService<OrderItem, int>
    {
        IQueryable<OrderItem> GetByCustomFilter(int? orderId, int? productSalesPlanningId, int? companyId,
            int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType,
            int? orderCreationDateFromDate, int? orderCreationDateToDate,
            string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null,
            bool fillNotModified = false);

        IQueryable<OrderItem> GetUserOrderItems();

        /// <summary>
        /// get all orderItems for the specific user based on company Id or representation Id
        /// </summary>
        /// <param name="taxCategoryId">tax category Id</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetUnFactoredOrderItemsInTaxCategory(int taxCategoryId, bool readOnly = false);

        /// <summary>
        /// دریافت همه آیتمهای سفارش فاکتور نشده کاربر لاگین شده
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetUserUnFactoredOrderItems(bool readOnly = false);

        IQueryable<OrderItem> GetUserOrderItemsByOrderId(int orderId);
        IQueryable<OrderItem> GetCurrentUserOrderItems(int? orderId, int applicationId);
        IQueryable<OrderItem> GetByModifiedOrderItemId(int modifiedOrderItemId);


        void AssignVoucherForOrderItems(int applicationId);

        /// <summary>
        /// قبل از پرداخت آیتم سفارشات تکراری را بر میگرداند 
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetDuplicateOrderItemBeforePay(string nationalCode, Guid? shoppingCartCode,
            int? orderId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="discountPercentage"></param>
        /// <param name="profitPercentage"></param>
        /// <param name="insurancePercentage"></param>
        /// <param name="otherDeductionsPercentage"></param>
        /// <param name="wagePercentage"></param>
        /// <param name="calculatedOrderItemPriceWithSubsidy"></param>
        /// <param name="calculatedOrderItemPriceWithoutSubsidy"></param>
        /// <param name="subsidyProfitPercentage"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="representationId"></param>
        /// <param name="isOtherDeductionsIncludeInSupplierInvoice"></param>
        /// <param name="applicationId"></param>
        /// <param name="orderUserId">برای جاهایی گه قبلا سفارش ذخیره شده است باید با کد کاربر سفارش و در غیر این صورت کد کاربر لاگین شده مقدار دهی شود</param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<OrderItem> CreateOrderItemEntityAsync(
            int saleConfigurationId,
            decimal discountPercentage,
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
            );

        /// <summary>
        /// آیتم سفارش را بر میگرداند حتی اگر حذف منطقی شده باشد
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        OrderItem GetOrderItem(int orderId, int productSalesPlanningId, int representationId);

        IQueryable<OrderItem> GetOrderItemsByOrderId(int orderId, bool readOnly = false);

        /// <summary>
        /// لیست تراکنش هایی که از ساتا سوبسید گرفته اند
        /// </summary>
        /// <param name="subsidyTypesEnum"></param>
        /// <returns></returns>
        IQueryable<OrderItemFullInfoView> GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum subsidyTypesEnum, int orderCreationDateFromDate, int orderCreationDateToDate);

        /// <summary>
        /// لیست آیتم های سفارش تکمیل شده کاربر در برنامه 
        /// </summary>
        /// <param name="applicationId">کد برنامه</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, bool readOnly = false);

        /// <summary>
        /// حذف  منطقی آیتم سفارش و فرزندان آیتم سفارش
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Task<int> DeleteWithChildrenLogicalAsync(int orderItemId);

        /// <summary>
        /// برای نمایش همه ایتم های یک سفارش
        /// این متد حذف شده ها را نیز نشان میدهد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllWithDeletedOrderItems(int orderId);
        /// <summary>
        /// آیا این محصول در آیتم سفارشات وجود دارد 
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns></returns>
        bool IsProductInOrderItems(int productId);

        /// <summary>
        /// درج سهم پرداخت آیتم های سفارش
        /// </summary>
        /// <param name="orderId"></param>
        Task<int> SetOrderItemPriceRightShareAsync(int orderId);

        /// <summary>
        /// لیست آیتم های سفارش یک برنامه ریزی دوره را برمیگرداند.
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="considerDeleted"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllOrderItemsByFilter(int productSalesPlanningId, bool considerDeleted = true, bool readOnly = false);

        /// <summary>
        /// دریافت آیتم های فرزند بر اساس شناسه آیتم سفارش
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش </param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetOrderItemChildrenByParentId(int orderItemId, int applicationId);

        /// <summary>
        /// این متد
        /// orderItem
        /// را بر اساس شناسه آن در صورتی که به کاربر تعلق داشته باشد
        /// را برمیگرداند
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Task<OrderItem> GetCurrentUserOrderItemAsync(int orderItemId);

        /// <summary>
        /// فیلد دارای گزارش ارزش افزروده در آیتم سفارش های فاکتور نشده مربوط به این تنظیم فروش 
        /// را به روز رسانی می کنیم
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <param name="hasTaxCertificate">دارای گواهی ارزش افزوده</param>
        /// <returns></returns>
        Task<int> UpdateHasTaxCertificateForUnFactoredOrderItemBySaleConfigurationIdAsync(int saleConfigurationId,bool hasTaxCertificate);

        /// <summary>
        /// دریافت شناسه نمایندگی مروبط به یک ایتم سفارش
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Task<int> GetOrderItemRepresentationIdAsync(int orderItemId);
        /// <summary>
        /// دریافت لیست سفارشاتی که پس از پرداخت وجه عودت شده و دارای یارانه است
        /// </summary>
        /// <param name="orderCreationDateFromDate"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllRefundedOrderItems(int? factorId, int? subsidyType, int? orderCreationDateFromDate, int? orderCreationDateToDate);

    }
}