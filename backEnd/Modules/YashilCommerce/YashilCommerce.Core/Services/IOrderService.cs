using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Services
{
    public interface IOrderService : IGenericService<Order, int>
    {
        /// <summary>
        /// جهت تایید یارانه ساتا بوسیله سرویس ساتا
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<ResultStatus<int>> ConfirmSubsidyBySata(int orderId);

        IQueryable<OrderItem> GetWaitingForResultOrderItems(int applicationId);

        /// <summary>
        /// قیمت های مربوط به آیتم های سبد خرید را بر اساس گیرنده محصول و با در نظر گرفتن یارانه و سقف یارانه مشتری محاسبه می کند
        /// فقط آیتم هایی را در نظر می گیرد که پدر نداشته باشند
        /// آیتم های دارای پدر ایتم هایی هستند که مستقل به فروش نمی رسند و جمع مبالغ آنها در ایتم پدر آورده می شود
        /// </summary>
        /// <param name="recipientId">در صورتی که می خواهید بیشترین مقدار یارانه را بدون درنظر گرفتن یارانه محاسبه کنید، مقدار این فیلد را خالی بگذارید</param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="subsidyBased"></param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <param name="calcMaxSubsidy">ساتا: 0 و وزرات :1</param>
        /// <returns></returns>
        List<CalcedOrderItemPrice> GetShoppingCartItemPricesForCalcPrice(int? recipientId,
            Guid shoppingCartCode,
            bool subsidyBased,
            int customerClubId,
            bool? calcMaxSubsidy = null);
        IQueryable<Order> GetByCustomFilter(int? userId, int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType, int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId, int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType, int? companyId, int? representationId, int? orderCreationDateFromDate, int? orderCreationDateToDate);

        /// <summary>
        /// قیمت های مربوط به آیتم های سبد خرید را بر اساس گیرنده محصول و با در نظر گرفتن یارانه و سقف یارانه مشتری محاسبه می کند
        /// </summary>
        /// <param name="recipientId">در صورتی که می خواهید بیشترین مقدار یارانه را بدون درنظر گرفتن یارانه محاسبه کنید، مقدار این فیلد را خالی بگذارید</param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="subsidyBased"></param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <param name="calcMaxSubsidy">ساتا: 0 و وزرات :1</param>
        /// <returns></returns>
        List<CalcedOrderItemPrice> GetShoppingCartItemPrices(
            int? recipientId, Guid shoppingCartCode,
            bool subsidyBased, int? customerClubId,
            bool? calcMaxSubsidy = null);

        IQueryable<Order> GetUserOrders(int applicationId);
        //Task<int?> GetShoppingCartItemInstallmentCountAsync(List<ProductSalesPlanningPriceDto> productSalesPlanningPriceDtos,
        //    Guid shoppingCartCode);

        /// <summary>
        /// قیمت نهایی سفارش را می گیرد و اقساط را محاسبه می کند
        /// </summary>
        /// <param name="finalPrice">قیمت نهایی محاسبه شده</param>
        /// <param name="orderId">سفارش</param>
        /// <param name="orderUserId">شناسه کاربر سفارش</param>
        /// <returns></returns>
        Task<int?> GetOrderInstallmentCountAsync(decimal? finalPrice, int orderId, int orderUserId);
        Task<Order> AddOrderAndClearShoppingCartAsync(Order order, int recipientId, Guid guidCode);
        Task SendMessageForOrder();
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatusTypesEnum orderStatus,
            bool saveAfterUpdate = false);

        bool CheckOrderAllowPay(OrderStatusTypesEnum orderStatusTypes, SubsidyStatusTypesEnum? subsidyStatusType);

        Task<decimal?> GetOrderTotalAsync(SubsidyStatusTypesEnum subsidyStatusType, decimal? totalInclSubsidy,
            decimal? totalExclSubsidy);
        IQueryable<OrderItem> GetCurrentOrderItems(int applicationId);

        /// <summary>
        /// سفارش هایی که بیش از زمان مشخصی از عدم پرداخت آنها گذشه است
        /// را به وضعیت انصراف سیستمی می برد
        /// </summary>
        Task CancelExpiredOrdersAsync(bool saveAfterUpdate = false);
        IQueryable<Order> GetOrderIncludeAddress(int orderId);

        /// <summary>
        /// محاسبه قیمت سفارش بر اساس تایید سوبسید و اقساطی و نقدی
        /// اگر سفارش در وضعیت پرداخت ناقص باشد برای محاسبه پرداخت مبلغ باقیمانده
        /// از این متد استفاده میشود
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<AmountForPayment> CalcAmountForPayAsync(int paymentMethodType, int orderId);
        /// <summary>
        /// انصراف سفارش با پرداخت تکمیل شده 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        Task<int> CancelOrder(int orderId, int userId);

        /// <summary>
        /// get the ReservationNumber number of order
        /// it adds prefix with order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        string GetReservationNumber(int orderId, int paymentId);

        /// <summary>
        /// یک رکورد خرید جدید را مقدار دهی ، و درج می کند و بر می گرداند
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<PaymentInformation> CreateNewPaymentAndReturnAsync(
            int orderId,
            PaymentMethodTypesEnum paymentMethodType,
            string merchantId,
            string terminalId);


        /// <summary>
        /// آیا سفارش مجوز انصراف را دارد یا خیر
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        bool CheckOrderAllowCancel(int orderId, OrderStatusTypesEnum orderStatusType);

        /// <summary>
        /// آیا سفارش مجوز لغو (عودت وجه سفارش) را دارد یا خیر
        /// </summary>
        /// <returns></returns>
        bool CheckOrderAllowRefund(OrderStatusTypesEnum orderStatusTypes, bool allowRefund);

        /// <summary>
        /// آیا سفارش مجوز تغییر را دارد یا خیر
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="lastOrderId"></param>
        /// <param name="allowEdit"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        bool CheckOrderAllowEdit(int orderId, int? lastOrderId, bool allowEdit, OrderStatusTypesEnum orderStatusType);
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
        /// آیتم های سفارشات مبتنی بر یارانه و منتظر تایید یارانه
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetWaitingForSubsidyOrderItems(int applicationId);
        /// <summary>
        ///  سفارش کامل شده با پرداخت موفق
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<Order> GetCompletePayOrders(int applicationId);
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
        /// <summary>
        /// وضعیت سوبسید را بر می گرداند
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int? GetSubsidyStatus(int orderId);

        /// <summary>
        /// آیا کاربر اجازه انتخاب درگاه بانکی را دارد؟
        /// </summary>
        /// <param name="membershipType"></param>
        /// <param name="cardNumber"></param>
        /// <param name="isPayWithWallet">در داخل این متد مشخص میشود آیا پرداخت درون برنامه انجام می شود یا نه</param>
        /// <param name="sumSubsidy"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        Task<(bool result, bool isPayWithWallet)> CheckAllowSelectPaymentGateway(int membershipType,
           string cardNumber, decimal? sumSubsidy, int? orderStatusType = null);


        IQueryable<Order> GetUserLastOrder(int userId);

        /// <summary>
        /// تعیین وضعیت یارانه ساتا
        /// </summary>
        /// <returns></returns>
        IQueryable<Order> GetAwaitingSataSubsidyApproval();
        bool CheckAllowContinueShopping(bool allowSelectPaymentGateway, bool isPayWithWallet, int membershipType, string cardNumber, decimal? sumSubside, int? orderStatusType = null);


        /// <summary>
        /// متدی جهت بروزرسانی کد ملی سفارشات تایید شده
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        Task<string> UpdateOrderNationalCode(int orderId, string nationalCode);

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
        /// وضصعیت یارانه سفارش را به روز رسانی میکند
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="subsidyStatusType"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        Task<Order> UpdateSubsidyStatusTypeAsync(int orderId, int subsidyStatusType,
            bool saveAfterUpdate = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="orderStatusTypesEnum"></param>
        /// <returns></returns>
        int GetNotRejectedSubsidyBasedOrdersCountByOrderStatusType(string cardNumber, OrderStatusTypesEnum? orderStatusTypesEnum);
        /// <summary>
        ///    در صورتی که اپلیکیشن مبتنی بر یارانه باشد قبل از نمایش صفحه پیش فاکتور و محاسبه یارانه برای کالاهی انتخابی چک می شود   
        ///   در صورتی که  در لیست سفارشات قبلی کاربر، سفارشی دارای یارانه تایید شده و یا ثبت اولیه با وضعیت سفارش ثبت اولیه باشد
        ///   ان سفارش  را به وضعیت لغو سیستمی تغییر می دهیم
        ///   تا یارانه این گونه سفارش ها برای سفارش جدید لحاظ شود
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        Task<bool> CancelSubsidyConfirmedInitialRegistrationOrder(int userId, string cardNumber);

        IQueryable<OrderItem> GetRefundOrderItems(int applicationId);
        IQueryable<OrderItem> GetCanceledOrderItems(int applicationId);

        /// <summary>
        /// برای نمایش اطلاعات یک سفارش
        /// این متد در نمایش جزییات سفارش در فرم لیست همه سفارش ها کاربرد دارد
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IQueryable<Order> GetOrderStatistic(int orderId);

        Task<int?> GetShoppingCartItemsMinInstallmentCountAsync(decimal total, Guid shoppingCartCode);
        Task<bool> CheckUserHasAccessForOrder(int orderId);

        Task<Order> GetByOrderItemIdAsync(int orderItemId);

        #region Methods used in the IPG
        Task<(long amount, string resNumber, string mobileNumber, int? installmentCount)?> GetGenerateTokenParams(
            int? orderId,
            PaymentMethodTypesEnum paymentMethodType,
            string merchantCode,
            string terminalCode);

        Task<TResultModel> HttpClientSendAsync<TRequestModel, TResultModel>(
            TRequestModel requestModel,
            string url);

        ResultStatus<int> CheckRequestField(
            bool hasFormContentType,
            string contentType,
            string state,
            string refNum,
            string resNum);

        string CheckTransactionState(string transactionState,
            out PaymentStatusTypesEnum paymentStatusType);


        #endregion

        IQueryable<Order> FindById(int id, int? applicationId = null);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری برای شناسه
        /// orderItem
        /// ارسال شده سفارشی که پرداخت موفق دارد ثبت کرده است ؟
        /// همچنین در صورتی که پارامتر
        /// companyApiLocalKey
        /// نیز دارای مقدار باشد بررسی می کند که
        /// آیا مرکز محصول خریداری شده
        /// ApiSupported
        /// است و کلید آن برابر است با کلید ارسالی
        /// همچنین محصول خریداری شده آیا از نوع نیاز به هدایت به سایت
        /// company
        /// برای دریافت خدمات است یا نه بررسی های را برای شناسه ایتم سفارش ارسالی انجام میدهد
        /// </summary>
        /// <param name="orderItemId">شناسه ایتم سفارش</param>
        /// <param name="companyApiLocalKey"></param>
        IQueryable<OrderItem> CheckCurrentUserHasAccessToOrderItemAndPayCompletely(int orderItemId, string companyApiLocalKey = null);

        /// <summary>
        /// دریافت تمامی سفارش های که
        /// وضعیت سفارش آن ها در حالت منتظر تایید تامین کننده برای پرداخت است.
        ///
        /// اگر کاربر نمایندگی یا مرکز اساین شده داشته باشد به صورت خودکار فیلتر مرکز نمایندگی را اعمال میکند
        /// همچنین امکان دریافت بر اساس مرکز دلخواه نیز وجود دارد.
        /// </summary>
        /// <param name="productSalesPlanningId">فیلتر بر اساس شناسه برنامه ریزی دوره</param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <param name="representationId">فیلتر بر اساس نمایندگی</param>
        IQueryable<Order> GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentAsync(int? productSalesPlanningId, int applicationId, bool readOnly,int? representationId = null);

        /// <summary>
        /// دریافت تمامی سفارش های مربوط به شناسه برنامه ریزی دوره ارسالی
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Order> GetByProductSalesPlanningId(int productSalesPlanningId, int applicationId, bool readOnly);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری دسترسی لازم برای
        /// برای تغییر وضعیت سفارش از حالت منتظر تایید تامین کننده به منتظر
        /// پرداخت را دارد یا نه؟
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<(bool hasAccess, Order Order)> HasCurrentUserAccessToChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPaymentAsync(int orderId);

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
        /// متدی جهت بروزرسانی شماره موبایل گیرنده سفارش
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="recipientMobileNumber">شماره موبایل جدید</param>
        /// <param name="oldMobileNumber">شماره موبایل قبلی</param>
        /// <returns></returns>
        Task<(bool success,string message)> UpdateOrderRecipientMobileNumberAsync(int orderId, string recipientMobileNumber, string oldMobileNumber);

        /// <summary>
        /// دریافت سفارش در صورتی که حذف نشده باشد
        /// به همراه ایتم ها و برنامه ریزی محصول و محصول
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Order> GetOrderIncludeOrderItemAndProduct(int orderId,bool readOnly);
    }
}
