using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order, int>
    {
        IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, int? userId, bool? isMessageSent = null);
        IQueryable<OrderItem> GetWaitingForResultOrderItems(int applicationId);
        IQueryable<Order> GetByCustomFilter(int? userId, int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType, int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId, int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType, int? companyId, int? representationId, int? orderCreationDateFromDate, int? orderCreationDateToDate);

        List<CalcedOrderItemPrice> GetShoppingCartItemPrices(
            int? recipientId,
            Guid shoppingCartCode,
            bool subsidyBased,
            bool? calcMaxSubsidy,
            int? customerClubId);
        /// <summary>
        /// دریافت سفارشات کاربر برای هر اپلیکیشن 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetUserOrders(int applicationId);
        /// <summary>
        /// تب سفارشات جاری : آخرین سفارش ثبت شده کاربر را نمایش میدهد 
        ///  سفارش هایی که" منتظر تایید تامین کننده" میباشد
        /// و سفارش هایی که منتظر تایید یارانه می باشند
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetCurrentOrders(int applicationId);
        /// <summary>
        /// سفارشات مبتنی بر یارانه و منتظر تایید یارانه
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetWaitingForSubsidyOrders(int applicationId);

        /// <summary>
        ///  سفارش کامل شده با پرداخت موفق
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="userId"></param>
        ///  <param name="isMessageSent">پیام ارسال شده است</param>
        /// <returns></returns>
        IQueryable<Order> GetCompletePayOrders(int applicationId, int? userId, bool? isMessageSent = null);
        /// <summary>
        /// سفارش منتظر نتیجه درگاه پرداخت
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetWaitingForResultOrders(int applicationId);
        /// <summary>
        ///سفارش عودت داده شده
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetRefundOrders(int applicationId);
        /// <summary>
        ///سفارش انصراف داده شده
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetCanceledOrders(int applicationId);
        IQueryable<Order> GetOrderIncludeAddress(int orderId);
        void CancelExpiredOrders(int applicationId);
        /// <summary>
        /// وضعیت سوبسید را بر می  گرداند
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int? GetSubsidyStatus(int orderId);
        IQueryable<Order> GetUserLastOrder(int userId);
        /// <summary>
        /// تعیین وضعیت یارانه ساتا
        /// </summary>
        /// <returns></returns>
        IQueryable<Order> GetAwaitingSataSubsidyApproval();

        /// <summary>
        /// لیست سفارش های مبتنی بر یارانه تایید شده و یا ثبت اولیه
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="orderStatusTypesEnum"></param>
        /// <returns></returns>
        IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByOrderStatusType(int? userId, string cardNumber, OrderStatusTypesEnum? orderStatusTypesEnum);

        /// <summary>
        /// لیست سفارش های مبتنی بر یارانه تایید شده و یا ثبت اولیه
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="userId"></param>
        /// <param name="getOnlyCompletedOrders"></param>
        /// <param name="getForCurrentUser"></param>
        /// <returns></returns>
        IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByFilter(int userId, string cardNumber, bool getOnlyCompletedOrders, bool getForCurrentUser);

        /// <summary>
        /// سفارش های کاربر را که دارای یارانه تایید شده میباشند بر می گرداند    
        /// </summary>
        /// <param name="userId">کاربر</param>
        /// <param name="recipientId">تحویل گیرنده</param>
        /// <param name="orderStatusType">وضعیت سفارش</param>
        /// <returns></returns>
        public IQueryable<Order> GetSubsidyConfirmedByRecipientIdAndOrderStatusType(int userId, int? recipientId, OrderStatusTypesEnum orderStatusType);
        Task<int> CancelOrder(int orderId, int userId);
        IQueryable<OrderItem> GetWaitingForSubsidyOrderItems(int applicationId);
        IQueryable<OrderItem> GetCurrentOrderItems(int applicationId);
        IQueryable<OrderItem> GetRefundOrderItems(int applicationId);
        IQueryable<OrderItem> GetCanceledOrderItems(int applicationId);
        /// <summary>
        /// برای نمایش اطلاعات یک سفارش
        /// این متد در نمایش جزییات سفارش در فرم لیست همه سفارش ها کاربرد دارد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IQueryable<Order> GetOrderStatistic(int orderId);
        /// <summary>
        /// کووری سفارش با کد مشخص شده  را بر می گرداند
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="readOnly">خواندنی باشد</param>
        /// <returns></returns>
        IQueryable<Order> FindOrderById(int orderId, int? applicationId, bool readOnly = true);

        /// <summary>
        /// دریافت سفارشات یک برنامه
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="readOnly">خواندنی باشد</param>
        /// <returns></returns>
        IQueryable<Order> GetAllByApplicationId(bool readOnly = false, int? applicationId = null);
        /// <summary>
        /// دریافت سفارشات با پرداخت حکمت ناموفق
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="isMessageSent"></param>
        /// <returns></returns>
        IQueryable<Order> GetPaymentFailedOrders(int applicationId,   bool? isMessageSent = null);

        /// <summary>
        /// دریافت سفارش های مربوط به یک برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Order> GetByProductSalesPlanningId(int? productSalesPlanningId, int applicationId, bool readOnly);

        /// <summary>
        /// دریافت سفارش های مربوط به کاربر جاری که در وضعیت منتظر تایید تامین کننده قبل از پرداخت است.
        /// </summary>
        /// <returns></returns>
        IQueryable<Order> GetCurrentUserWaitingForSupplierApprovalForPaymentOrder();

        /// <summary>
        /// این متد جهت دریافت ایتم های سفارش کاربر جاری
        /// که در وضعیت منتظرتایید تامین کننده قبل از پرداخت هستند کاربرد دارد
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderItem> GetCurrentUserWaitingForSupplierApprovalForPaymentOrderItem();

        /// <summary>
        /// فیلتر کردن سفارش ها بر اساس شناسه نمایندگی
        /// به این صورت که یا باید نمایندگی که در ایتم سفارش درج شده برابر باشد
        /// یا باید برنامه ریزی دوره دارای نمایندگی باشد و شناسه آن برابر شناسه ارسالی باشد.
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Expression<Func<Order, bool>> RepresentationFilter(int representationId);

        /// <summary>
        /// دریافت سفارش به همراه ایتم ها و اطلاعات محصول و برنامه ریزی محصول
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Order> GetOrderIncludeOrderItemAndProduct(int orderId, bool readOnly);
    }


}
