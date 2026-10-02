using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class OrderRepository : GenericApplicationBasedRepository<Order, int>, IOrderRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public OrderRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }
        public IQueryable<Order> GetByCustomFilter(int? userId, int? billingAddressId, int? shippingAddressId, int? recipientId, int? orderStatusType, int? shippingStatusType, int? paymentStatusType, int? paymentMethodType, int? rewardPointsHistoryEntryId, int? affiliateId, int? salesPlanningId, int? subsidyId, int? subsidyStatusType, int? subsidyType, int? companyId, int? representationId, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
            var query = GetAll(true);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }
            if (billingAddressId.HasValue)
            {
                query = query.Where(x => x.BillingAddressId == billingAddressId.Value);
            }
            if (shippingAddressId.HasValue)
            {
                query = query.Where(x => x.ShippingAddressId == shippingAddressId.Value);
            }
            if (orderStatusType.HasValue)
            {
                query = query.Where(x => x.OrderStatusType == orderStatusType.Value);
            }
            if (shippingStatusType.HasValue)
            {
                query = query.Where(x => x.ShippingStatusType == shippingStatusType.Value);
            }
            if (rewardPointsHistoryEntryId.HasValue)
            {
                query = query.Where(x => x.RewardPointsHistoryEntryId == rewardPointsHistoryEntryId.Value);
            }
            if (affiliateId.HasValue)
            {
                query = query.Where(x => x.AffiliateId == affiliateId.Value);
            }
            if (subsidyId.HasValue)
            {
                query = query.Where(x => x.SubsidyId == subsidyId.Value);
            }
            if (subsidyStatusType.HasValue)
            {
                query = query.Where(x => x.SubsidyStatusType == subsidyStatusType.Value);
            }
            if (subsidyType.HasValue)
            {
                query = query.Where(x => x.SubsidyType == subsidyType.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(x =>
                    x.OrderItem.Any(oi => oi.ProductSalesPlanning.SaleConfiguration.CompanyId == companyId.Value));
            }

            if (representationId.HasValue)
            {
                query = query.Where(x =>
                    x.OrderItem.Any(oi => oi.RepresentationId == representationId.Value));
            }

            if (orderCreationDateFromDate.HasValue)
            {
                query = query.Where(x =>
                    x.OrderItem.Any(oi => oi.Order.CreationDateInt >= orderCreationDateFromDate.Value));
            }

            if (orderCreationDateToDate.HasValue)
            {
                query = query.Where(x =>
                    x.OrderItem.Any(oi => oi.Order.CreationDateInt <= orderCreationDateToDate.Value));
            }

            if (paymentMethodType.HasValue)
            {
                query = query.Where(x =>
                    x.Payment.Any(payment => payment.PaymentMethodType == paymentMethodType.Value));
            }

            return query;
        }

        /// <summary>
        /// دریافت سفارش های مربوط به یک برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<Order> GetByProductSalesPlanningId(int? productSalesPlanningId, int applicationId, bool readOnly)
        {
            var query = GetAll(readOnly, applicationId);

            if (productSalesPlanningId is > 0)
                query = query.Where(order => order.OrderItem.Any(oi => oi.ProductSalesPlanningId == productSalesPlanningId));

            return query;
        }

        public List<CalcedOrderItemPrice> GetShoppingCartItemPrices(
            int? recipientId,
            Guid shoppingCartCode,
            bool subsidyBased,
            bool? isVezarat,
            int? customerClubId)
        {


            object[] param = {
                new SqlParameter
                {
                    ParameterName = "@RecipientId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    IsNullable = true,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = (object)recipientId ?? DBNull.Value,
                },
                new SqlParameter
                {
                    ParameterName = "@ShoppingCartCode",
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = shoppingCartCode,
                },
                new SqlParameter
                {
                    ParameterName = "@Subsidybased",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = subsidyBased,
                },
                new SqlParameter
                {
                    ParameterName = "@ApplicationId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = _applicationConfiguration.ApplicationId,
                },
                new SqlParameter
                {
                    ParameterName = "@IsVezarat",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    IsNullable = true,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = (object)isVezarat??DBNull.Value,
                }
                ,
                new SqlParameter
                {
                    ParameterName = "@CustomerClubId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    IsNullable = true,
                    Value = customerClubId
                }
                ,
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    IsNullable = true,
                    Value = _userPrincipal.Id
                }
            };


            return _context.CalcedOrderItemPrices
                .FromSqlRaw(" EXEC [shp].[CalcOrderItemsPrice] @RecipientId,@ShoppingCartCode,@Subsidybased,@ApplicationId,@IsVezarat,@CustomerClubId,@UserId", param).ToList();
        }
        /// <summary>
        /// دریافت سفارشات کاربر برای هر اپلیکیشن 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetUserOrders(int applicationId)
        {
            return GetAllByApplicationId(true, applicationId).
                OrderByDescending(x => x.CreationDateInt).Where(x => x.UserId == _userPrincipal.Id && x.ApplicationId == applicationId);
        }
        public IQueryable<Order> GetOrderIncludeAddress(int orderId)
        {
            return DbSet.Where(x => !x.Deleted && x.Id == orderId)
                .Include(x => x.ShippingAddress)
                .Include(b => b.ShippingAddress.City);
        }
        /// <summary>
        /// تب سفارشات جاری : آخرین سفارش ثبت شده کاربر را نمایش میدهد 
        ///  سفارش هایی که" منتظر تایید تامین کننده" میباشد
        /// و سفارش هایی که منتظر تایید یارانه می باشند
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCurrentOrders(int applicationId)
        {
            var lastOrder = GetAllByApplicationId(true, applicationId).Where(x => x.UserId == _userPrincipal.Id &&

             x.Payment.Any(p => p.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid))
                .OrderByDescending(x => x.Id).Take(1);
            var waitingOrder = GetAllByApplicationId(true, applicationId).Where(x => x.UserId == _userPrincipal.Id &&

            x.OrderStatusType != (int)OrderStatusTypesEnum.CancelBySystem
            && x.OrderStatusType != (int)OrderStatusTypesEnum.Canceled);
            return lastOrder.Union(waitingOrder).OrderByDescending(x => x.Id);
        }

        public IQueryable<OrderItem> GetCurrentOrderItems(int applicationId)
        {
            return SelectOrderItems(GetCurrentOrders(applicationId));
        }


        /// <summary>
        /// سفارشات مبتنی بر یارانه و منتظر تایید یارانه
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetWaitingForSubsidyOrders(int applicationId)
        {
            return GetAllByApplicationId(true, applicationId).Where(x => x.UserId == _userPrincipal.Id &&

            x.OrderStatusType != (int)OrderStatusTypesEnum.Canceled &&
            x.OrderStatusType != (int)OrderStatusTypesEnum.CancelBySystem &&
            x.OrderStatusType != (int)OrderStatusTypesEnum.Complete &&
            x.SubsidyType.HasValue &&
            x.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration).OrderByDescending(x => x.Id);
        }

        /// <summary>
        /// سفارشات مبتنی بر یارانه و منتظر تایید یارانه
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetWaitingForSubsidyOrderItems(int applicationId)
        {
            return SelectOrderItems(GetWaitingForSubsidyOrders(applicationId));
        }

        /// <summary>
        ///  سفارش کامل شده 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="userId"></param>
        /// <param name="isMessageSent"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCompletePayOrders(int applicationId, int? userId, bool? isMessageSent = null)
        {
            var query = GetAllByApplicationId(true, applicationId).Where(x => (
                                                               !userId.HasValue || x.UserId == userId) &&
                                     x.OrderStatusType == (int)OrderStatusTypesEnum.Complete);
            if (isMessageSent.HasValue)
            {
                query = query.Where(x => x.IsMessageSent == isMessageSent);
            }
            return query.OrderByDescending(x => x.Id);
        }

        public IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, int? userId, bool? isMessageSent = null)
        {
            return SelectOrderItems(GetCompletePayOrders(applicationId, userId, isMessageSent));
        }

        public IQueryable<OrderItem> GetWaitingForResultOrderItems(int applicationId)
        {
            return SelectOrderItems(GetWaitingForResultOrders(applicationId));

        }

        private IQueryable<OrderItem> SelectOrderItems(IQueryable<Order> orders)
        {
            return orders.SelectMany(x => x.OrderItem.Where(oi => !oi.ParentId.HasValue));
        }

        /// <summary>
        /// سفارش منتظر نتیجه درگاه پرداخت
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetWaitingForResultOrders(int applicationId)
        {
            return GetAllByApplicationId(true, applicationId).Where(x =>
            x.UserId == _userPrincipal.Id &&
            x.ApplicationId == applicationId &&
            x.Payment.Any(p => p.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway ||
                       p.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)).OrderByDescending(x => x.Id);
        }
        /// <summary>
        ///سفارش مرجوعی  داده شده
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetRefundOrders(int applicationId)
        {
            return GetAllByApplicationId(true, applicationId).Where(x => x.UserId == _userPrincipal.Id && x.ApplicationId == applicationId
                                        && x.OrderStatusType == (int)OrderStatusTypesEnum.Refund).OrderByDescending(x => x.Id);

        }
        public IQueryable<OrderItem> GetRefundOrderItems(int applicationId)
        {
            return SelectOrderItems(GetRefundOrders(applicationId));
        }

        /// <summary>
        ///سفارش انصراف داده شده
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<Order> GetCanceledOrders(int applicationId)
        {
            return GetAllByApplicationId(true, applicationId)
                .Where(x => x.ApplicationId == applicationId && x.UserId == _userPrincipal.Id
                                        && (x.OrderStatusType == (int)OrderStatusTypesEnum.Canceled
                                            || x.OrderStatusType == (int)OrderStatusTypesEnum.CancelBySystem))
                .OrderByDescending(x => x.Id);
        }

        public IQueryable<OrderItem> GetCanceledOrderItems(int applicationId)
        {
            return SelectOrderItems(GetCanceledOrders(applicationId));
        }

        public IQueryable<Order> GetOrderStatistic(int orderId)
        {
            return DbSet.AsNoTracking().Where(x => x.Id == orderId);
        }

        public async void CancelExpiredOrders(int applicationId)
        {
            try
            {
                var date = DateTime.Now;
                await _context.Database.ExecuteSqlRawAsync($"shp.CancelExpiredOrders {applicationId},'{date}' ");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public int? GetSubsidyStatus(int orderId)
        {
            var orderStatus = _context.Order.Select(x => new { x.Id, x.SubsidyStatusType }).FirstOrDefault(x => x.Id == orderId);
            if (orderStatus != null)
            {
                return orderStatus.SubsidyStatusType;
            }
            return null;
        }

        public IQueryable<Order> GetUserLastOrder(int userId)
        {
            return _context.Order.Where(x => x.UserId == userId).OrderByDescending(x => x.Id).Take(1);
        }
        /// <summary>
        /// تعیین وضعیت یارانه ساتا
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order> GetAwaitingSataSubsidyApproval()
        {
            // هنگامی اجازه تغییر یارانه ساتا را داریم که 
            // سفارش در حالت ثبت اولیه باشد
            // یا پرداخت ناقص داشته باشد 
            // یا پرداخت ناموفق داشته است
            return DbSet.Where(x => (x.OrderStatusType == (int)OrderStatusTypesEnum.InitialRegistration
                                    || x.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment
                                    || x.OrderStatusType == (int)OrderStatusTypesEnum.UnsuccessfulPayment)
                               && x.SubsidyType == (int)SubsidyTypesEnum.Sata
                               && x.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration);
        }

        /// <summary>
        /// لیست سفارش های مبتنی بر یارانه تایید شده و یا ثبت اولیه
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="orderStatusType"></param>
        /// <returns></returns>
        public IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByOrderStatusType(int? userId, string cardNumber, OrderStatusTypesEnum? orderStatusType)
        {
            var query = GetNotRejectedSubsidyBasedOrdersQuery(cardNumber);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (orderStatusType.HasValue)
            {
                query = query.Where(x => x.OrderStatusType == (int)orderStatusType.Value);
            }
            return query;
        }

        public IQueryable<Order> GetNotRejectedSubsidyBasedOrdersByFilter(int userId, string cardNumber, bool getOnlyCompletedOrders, bool getForCurrentUser)
        {
            var query = GetNotRejectedSubsidyBasedOrdersQuery(cardNumber);

            if (getForCurrentUser)
            {
                query = query.Where(x => x.UserId == userId);
            }
            else
            {
                query = query.Where(x => x.UserId != userId);
            }

            if (getOnlyCompletedOrders)
            {
                query = query.Where(x => x.OrderStatusType == (int)OrderStatusTypesEnum.Complete);
            }
            else
            {
                query = query.Where(x => (x.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment ||
                          x.OrderStatusType == (int)OrderStatusTypesEnum.InitialRegistration ||
                            x.OrderStatusType == (int)OrderStatusTypesEnum.UnsuccessfulPayment ||
                          x.OrderStatusType == (int)OrderStatusTypesEnum.Processing ||
                          x.OrderStatusType == (int)OrderStatusTypesEnum.WaitingForSupplierApprovalBeforeEditOrder ||
                          x.OrderStatusType == (int)OrderStatusTypesEnum.WaitingForSupplierApprovalBeforereRefundOrder));
            }
            return query;

        }

        private IQueryable<Order> GetNotRejectedSubsidyBasedOrdersQuery(string cardNumber)
        {
            return GetAllByApplicationId(true, _applicationConfiguration.ApplicationId).Where(x =>
               !x.Deleted &&
               x.CardNumber == cardNumber &&
               x.OrderItem.Sum(i => i.SubsidyPrice) > 0 &&
               (x.SubsidyStatusType.HasValue
               && x.Subsidy.IsActive
               && x.SubsidyStatusType != (int)SubsidyStatusTypesEnum.Rejected));
        }

        /*

*/
        /// <summary>
        /// سفارش های کاربر را که دارای یارانه تایید شده میباشند بر می گرداند    
        /// </summary>
        /// <param name="userId">کاربر</param>
        /// <param name="recipientId">تحویل گیرنده</param>
        /// <param name="orderStatusType">وضعیت سفارش</param>
        /// <returns></returns>
        public IQueryable<Order> GetSubsidyConfirmedByRecipientIdAndOrderStatusType(int userId, int? recipientId, OrderStatusTypesEnum orderStatusType)
        {
            var recipient = _context.Recipient.Where(r => r.Id == recipientId.Value && recipientId.HasValue);
            var query = _context.Order.Where(x => !x.Deleted &&
             x.ApplicationId == _applicationConfiguration.ApplicationId &&
             x.UserId == userId &&
             recipient.Any(r => r.CardNumber == x.CardNumber) &&
             (x.SubsidyStatusType.HasValue && x.SubsidyStatusType.Value == (int)SubsidyStatusTypesEnum.Confirmed) &&
             x.OrderStatusType == (int)orderStatusType
                    );
            return query;
        }

        public Task<int> CancelOrder(int orderId, int userId)
        {
            return _context.Database.ExecuteSqlRawAsync($" [shp].[CancelOrder] {orderId},{userId} ");
        }


        public override IQueryable<Order> GetAll(bool readOnly = false, int? applicationId = null)
        {
            var query = base.GetAll(readOnly, applicationId);
            var additionalUserProp =
                _context.AdditionalUserProp.Where(x => x.UserId == _userPrincipal.Id).AsNoTracking()
                .Select(x => new
                {
                    x.RepresentationId,
                    x.CompanyId
                }).FirstOrDefault();
            if (additionalUserProp != null && additionalUserProp.CompanyId.HasValue)
            {
                if (additionalUserProp.RepresentationId != null)
                {
                    query = query.Where(RepresentationFilter(additionalUserProp.RepresentationId.Value));
                }
                else
                {
                    query = query.Where(x =>
                        x.OrderItem.Any(i => i.ProductSalesPlanning.Product.CompanyId == additionalUserProp.CompanyId));
                }
            }


            return query.AsNoTracking();

        }

        public IQueryable<Order> FindOrderById(int orderId, int? applicationId, bool readOnly = true)
        {
            return base.GetAll(readOnly, applicationId).Where(x => x.Id == orderId);
        }
        /// <summary>
        /// دریافت سفارشات یک برنامه
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="readOnly">خواندنی باشد</param>
        /// <returns></returns>
        public IQueryable<Order> GetAllByApplicationId(bool readOnly = false, int? applicationId = null)
        {
            return base.GetAll(readOnly, applicationId);
        }

        public IQueryable<Order> GetPaymentFailedOrders(int applicationId, bool? isMessageSent = null)
        {
            var query = GetAllByApplicationId(true, applicationId).Where(x =>
                                 x.OrderStatusType != (int)OrderStatusTypesEnum.Complete
                                 && x.Payment.Any(p =>
                                     p.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                                     && p.PaymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful
                                     && !string.IsNullOrEmpty(p.AuthorizationTransactionId)
                                     && !string.IsNullOrEmpty(p.AuthorizationTransactionResult)));
            if (isMessageSent.HasValue)
            {
                query = query.Where(x => x.IsMessageSent == isMessageSent);
            }
            return query.OrderByDescending(x => x.Id);
        }

        /// <summary>
        /// دریافت سفارش های مربوط به کاربر جاری که در وضعیت منتظر تایید تامین کننده قبل از پرداخت است.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order> GetCurrentUserWaitingForSupplierApprovalForPaymentOrder()
        {
            return GetAllByApplicationId(true, _applicationConfiguration.ApplicationId)
                .Where(order =>
                    order.UserId == _userPrincipal.Id &&
                    order.OrderStatusType == (int)OrderStatusTypesEnum.RequiredSupplierApprovalBeforePayment);
        }

        /// <summary>
        /// این متد جهت دریافت ایتم های سفارش کاربر جاری
        /// که در وضعیت منتظرتایید تامین کننده قبل از پرداخت هستند کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public IQueryable<OrderItem> GetCurrentUserWaitingForSupplierApprovalForPaymentOrderItem()
        {
            return SelectOrderItems(GetAllByApplicationId(true, _applicationConfiguration.ApplicationId)
               .Where(x => x.UserId == _userPrincipal.Id &&
                            x.OrderStatusType == (int)OrderStatusTypesEnum.RequiredSupplierApprovalBeforePayment)
               .OrderByDescending(x => x.Id));
        }

        /// <summary>
        /// فیلتر کردن سفارش ها بر اساس شناسه نمایندگی
        /// به این صورت که یا باید نمایندگی که در ایتم سفارش درج شده برابر باشد
        /// یا باید برنامه ریزی دوره دارای نمایندگی باشد و شناسه آن برابر شناسه ارسالی باشد.
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public Expression<Func<Order, bool>> RepresentationFilter(int representationId)
        {
            return x => x.OrderItem.Any(i => i.RepresentationId == representationId) ||
                 x.OrderItem.Any(item =>
                     item.ProductSalesPlanning.RepresentationId.HasValue &&
                     item.ProductSalesPlanning.RepresentationId == representationId);
        }

        public IQueryable<Order> GetOrderIncludeOrderItemAndProduct(int orderId,bool readOnly)
        {
            var query = DbSet.Where(x => !x.Deleted && x.Id == orderId)
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.ProductSalesPlanning.Product);

            return readOnly ? query.AsNoTracking() : query;
        }
    }
}
