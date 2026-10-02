using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Repositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem, int>
    {

        IQueryable<OrderItem> GetByCustomFilter(int? orderId, int? productSalesPlanningId, int? companyId, int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null, bool fillNotModified = false);
        IQueryable<OrderItem> GetUserOrderItems(bool readOnly = false);
        IQueryable<OrderItem> GetUserOrderDetailItems(int? orderId, bool readOnly = false);

        /// <summary>
        /// get all orderItems for the specific user based on company Id or representation Id
        /// </summary>
        /// <param name="taxCategoryId">tax category Id</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetUnFactoredOrderItemsInTaxCategory(int taxCategoryId, bool readOnly = false);
        /// <summary>
        /// دریافت آیتمهای سفارش فاکتور نشده کاربر لاگین شده
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetUnFactoredOrderItems(bool readOnly = false);
        IQueryable<OrderItem> GetUserOrderItemsByOrderId(int orderId);
        bool CheckExistenceUnFactoredOrderItemsInTaxCategory(int taxCategoryId);
        IQueryable<OrderItem> GetCurrentUserOrderItems(int? orderId, int applicationId);

        /// <summary>
        /// قبل از پرداخت آیتم سفارشات تکراری را بر میگرداند 
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetDuplicateOrderItemBeforePay(string nationalCode, Guid? shoppingCartCode, int? orderId);
        IQueryable<OrderItem> GetByModifiedOrderItemId(int modifiedOrderItemId);

        /// <summary>
        /// آیتم سفارش را بر میگرداند حتی اگر حذف منطقی شده باشد
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        OrderItem GetOrderItem(int orderId, int productSalesPlanningId, int representationId);
        IQueryable<OrderItem> GetOrderItemsByOrderId(int orderId, bool readOnly = false);
        IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, bool readOnly = false);
        Task<int> DeleteWithChildrenLogicalAsync(int orderItemId);
        /// <summary>
        /// برای نمایش همه آیتم های سفارش یک سفارش
        /// این متد حذف شده ها را نیز نشان میدهد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllWithDeletedOrderItems(int orderId);
        /// <summary>
        /// آیتم های سفارش مبتی بر یارانه تایید شده و کامل و حذف نشده
        /// </summary>
        /// <param name="subsidyTypesEnum"></param>
        /// <returns></returns>
        IQueryable<OrderItemFullInfoView> GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum subsidyTypesEnum, int orderCreationDateFromDate, int orderCreationDateToDate);
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
        /// <param name="userId"></param>
        Task<int> SetOrderItemPriceRightShareAsync(int orderId, int userId);

        /// <summary>
        /// دریافت لیست سفارشات برنامه
        /// </summary>
        /// <param name="readOnly">آیا خواندنی است</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllByApplicationId(bool readOnly = false, int? applicationId = null);

        /// <summary>
        /// لیست آیتم های سفارش مربوط به یک برنامه ریزی دوره را برمیگرداند
        /// باید توجه داشته که به هیچعنوان نباید ApplicationId را در نظر گرفت و تنها با
        /// aproductSalesPlanning آیتم های سفارش را دریافت میکنیم
        /// زیرا ممکن است در پنل ادمین و پنل عمومی کاربر لاگین شده با applicationId متفاوت از
        /// applicationId برنامه باشد و این متد بدرستی کار نکند
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="considerDeleted"></param>
        /// <param name="withDeleted"></param>
        /// <param name="readOnly"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllOrderItemsByFilter(int productSalesPlanningId, bool withDeleted = false, bool readOnly = false);

        /// <summary>
        /// دریافت آیتم های فرزند بر اساس شناسه آیتم سفارش
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش </param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetOrderItemChildrenByParentId(int orderItemId, int applicationId);


        /// <summary>
        /// این متد جهت دریافت مقدار
        /// AllProductsIncludeInsurance
        /// مربوط به یک
        /// SaleConfiguration
        /// براساس شناسه
        /// SaleConfiguration
        /// کاربرد دارد
        /// به این دلیل این کار را کردیم که با
        /// inject ISaleConfigurationService
        /// دچار
        /// circular Dependency
        /// می شویم
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <returns>bool</returns>
        Task<bool> GetSaleConfigurationAllProductsIncludeInsuranceAsync(int saleConfigurationId);

        /// <summary>
        /// آیستم های سفارش یک تنظیم فروش که فرزند ندارند و فاکتورنشده اند
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <returns></returns>
        IQueryable<OrderItem> GetUnFactoredOrderItemBySaleConfigurationId(int saleConfigurationId);
        /// <summary>
        /// دریافت لیست سفارشاتی که پس از پرداخت وجه عودت شده و دارای یارانه است
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="subsidyType"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetAllRefundedOrderItems(int? factorId, int? subsidyType, int? orderCreationDateFromDate, int? orderCreationDateToDate);

    }
}
