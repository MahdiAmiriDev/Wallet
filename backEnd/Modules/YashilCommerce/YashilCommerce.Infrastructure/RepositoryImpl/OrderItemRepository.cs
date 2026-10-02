using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class OrderItemRepository : GenericApplicationBasedRepository<OrderItem, int>, IOrderItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public OrderItemRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        /// <summary>
        /// لیست آیتم های سفارش بر اساس مرکز و نمایندگی کاربر
        /// </summary>
        /// <param name="readOnly"></param>
        /// <param name="applicationId"></param>
        /// <param name="emptyIfUserHasNotAnyCompany">در صورتی که کاربر مرکز نداشت لیست را خالی نمایش دهد؟</param>
        /// <returns></returns>
        private IQueryable<OrderItem> PureGetAll(bool readOnly = false, int? applicationId = null,
            bool emptyIfUserHasNotAnyCompany = false)
        {
            IQueryable<OrderItem> query = base.GetAll(readOnly, applicationId);
            query = ApplyUserAccessFilterToQuery(query, emptyIfUserHasNotAnyCompany);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// لیست آیتم های پدر سفارش کاربر لاگین شده با فیلتر مرکز و نمایندگی کاربر
        /// </summary>
        /// <param name="readOnly"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public override IQueryable<OrderItem> GetAll(bool readOnly = false, int? applicationId = null)
        {
            return PureGetAll(readOnly, applicationId).Where(x => !x.ParentId.HasValue);
        }

        /// <summary>
        /// بر اساس مرکز و نمایندگی کاربر لاگین شده لیست آیتم هایس سفارش فیلتر می شود
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="showCompletedOrderStatus"></param>
        /// <param name="orderStatusType"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <param name="recipientNationalCode"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="cardNumber"></param>
        /// <param name="fillNotModified"></param>
        /// <param name="saleType"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetByCustomFilter(int? orderId, int? productSalesPlanningId, int? companyId,
            int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType,
            int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null,
            string mobileNumber = null, string cardNumber = null, bool fillNotModified = false)
        {

            var query = _context.OrderItem.AsQueryable().Where(x => !x.ParentId.HasValue);
            query = ApplyUserAccessFilterToQuery(query);
            if (orderId.HasValue)
            {
                query = query.Where(x => x.OrderId == orderId.Value);
            }

            if (productSalesPlanningId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanningId == productSalesPlanningId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanning.Product.CompanyId == companyId);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (showCompletedOrderStatus.HasValue && showCompletedOrderStatus.Value)
            {
                query = query.Where(x => x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete);
            }

            if (!string.IsNullOrEmpty(recipientNationalCode))
            {
                query = query.Where(x => x.Order.NationalCode == recipientNationalCode);
            }

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                query = query.Where(x => x.Order.User.UserName == mobileNumber);
            }

            if (!string.IsNullOrEmpty(cardNumber))
            {
                query = query.Where(x => x.Order.CardNumber == cardNumber);
            }

            if (fillNotModified)
            {
                query = query.Where(x => x.ModifiedOrderItemOldOrderItem.All(y =>
                    y.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RejectedByCrmExpert ||
                    y.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.Completed));
            }

            if (orderStatusType.HasValue)
            {
                query = query.Where(x => x.Order.OrderStatusType == orderStatusType
                                         && (
                                             (orderStatusType.Value == (int)OrderStatusTypesEnum.Refund &&
                                              x.Deleted)
                                             || (orderStatusType.Value != (int)OrderStatusTypesEnum.Refund &&
                                                 !x.Deleted)
                                         )

                );

            }

            if (!orderStatusType.HasValue)
            {
                query = query.Where(x => !x.Deleted);
            }

            if (orderCreationDateFromDate.HasValue)
            {
                query = query.Where(x => x.Order.CreationDateInt >= orderCreationDateFromDate.Value);
            }

            if (orderCreationDateToDate.HasValue)
            {
                query = query.Where(x => x.Order.CreationDateInt <= orderCreationDateToDate.Value);
            }

            return query.AsNoTracking();
        }


        public IQueryable<OrderItem> GetUserOrderItems(bool readOnly = false)
        {
            return DbSet.Where(x => !x.Deleted && x.Order.UserId == _userPrincipal.Id && !x.ParentId.HasValue);
        }


        public IQueryable<OrderItem> GetUserOrderDetailItems(int? orderId, bool readOnly = false)
        {
            return DbSet.Where(x => !x.Deleted && x.Order.UserId == _userPrincipal.Id && x.OrderId == orderId);
        }


        IQueryable<OrderItem> ApplyUserAccessFilterToQuery(IQueryable<OrderItem> query,
            bool emptyIfUserHasNotAnyCompany = false)
        {
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .Select(x => new { x.CompanyId, x.UserId, x.RepresentationId })
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);

            if (additionalUserProp?.CompanyId != null)
            {
                query = query.Where(x => x.ProductSalesPlanning.Product.CompanyId == additionalUserProp.CompanyId);

                if (additionalUserProp.RepresentationId != null)
                {
                    query = query.Where(x => x.RepresentationId == additionalUserProp.RepresentationId);
                }
                //else
                //{
                //    query = query.Where(x => !x.ProductSalesPlanning.RepresentationId.HasValue);
                //}
            }
            else
            {
                if (emptyIfUserHasNotAnyCompany)
                {
                    query = query.Where(x => false);
                }
            }

            return query;
        }


        public IQueryable<OrderItem> GetUserOrderItemsByOrderId(int orderId)
        {
            return DbSet.Where(x =>
                !x.Deleted && x.OrderId == orderId && x.Order.UserId == _userPrincipal.Id && !x.ParentId.HasValue);
        }


        public IQueryable<OrderItem> GetCurrentUserOrderItems(int? orderId, int applicationId)
        {
            return GetAllByApplicationId(true, applicationId).Where(x =>
                x.OrderId == orderId && x.Order.UserId == _userPrincipal.Id && !x.ParentId.HasValue);
        }

        /// <summary>
        /// قبل از پرداخت آیتم سفارشات تکراری را بر میگرداند 
        /// در صورتی که در وضعیت های ثبت اولیه - در حال بررسی - موفق تکمیل شده - پرداخت ناقص
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="shoppingCartCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetDuplicateOrderItemBeforePay(string nationalCode, Guid? shoppingCartCode,
            int? orderId)
        {
            var boughtOrderItems = DbSet.Where(c =>
                !c.Deleted &&
                !c.ParentId.HasValue &&
                (c.Order.OrderStatusType == (int)OrderStatusTypesEnum.InitialRegistration ||
                 c.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete ||
                 c.Order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment ||
                 c.Order.OrderStatusType == (int)OrderStatusTypesEnum.Processing));

            if (shoppingCartCode != Guid.Empty)
            {
                var shoppingCartProductSalesPlanning = _context.ShoppingCartItem
                    .Where(s => s.Code == shoppingCartCode.Value)
                    .Select(x => new { x.RepresentationId, x.ProductSalesPlanningId });

                boughtOrderItems = boughtOrderItems.Where(x => x.Order.NationalCode == nationalCode &&
                                                               shoppingCartProductSalesPlanning.Any(s =>
                                                                   s.RepresentationId == x.RepresentationId &&
                                                                   s.ProductSalesPlanningId ==
                                                                   x.ProductSalesPlanningId));
            }
            else if (orderId.HasValue)
            {
                var orderItem = DbSet.Where(oi => !oi.Deleted && oi.OrderId == orderId);

                boughtOrderItems = boughtOrderItems.Where(c => c.OrderId != orderId && orderItem.Any(oi =>
                    oi.RepresentationId == c.RepresentationId &&
                    oi.ProductSalesPlanningId == c.ProductSalesPlanningId &&
                    oi.Order.NationalCode == c.Order.NationalCode));
            }

            return boughtOrderItems;
        }

        public IQueryable<OrderItem> GetByModifiedOrderItemId(int modifiedOrderItemId)
        {
            return _context.ModifiedOrderItem.Where(x => x.Id == modifiedOrderItemId).Select(x => x.OldOrderItem)
                .AsNoTracking();
        }

        public OrderItem GetOrderItem(int orderId, int productSalesPlanningId, int representationId)
        {
            /*
             شرط حذف منطقی چک نشود
             */
            return DbSet.FirstOrDefault(x =>
                x.OrderId == orderId && !x.ParentId.HasValue && x.RepresentationId == representationId &&
                x.ProductSalesPlanningId == productSalesPlanningId);
        }

        public IQueryable<OrderItem> GetOrderItemsByOrderId(int orderId, bool readOnly = false)
        {
            var query = DbSet.Where(x => !x.Deleted && x.OrderId == orderId && !x.ParentId.HasValue);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public IQueryable<OrderItem> GetCompletePayOrderItems(int applicationId, bool readOnly = false)
        {
            return GetAllByApplicationId(readOnly, applicationId)
                .Where(x => x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete);
        }

        public Task<int> DeleteWithChildrenLogicalAsync(int orderItemId)
        {
            return _context.Database.ExecuteSqlRawAsync(
                $" UPDATE shp.OrderItem SET [Deleted] = 1 ,ModifyBy ={_userPrincipal.Id}" +
                $" FROM   shp.OrderItem" +
                $" WHERE  Id = {orderItemId} " +
                $" OR  (ParentId IS NOT NULL AND ParentId = {orderItemId})");
            //return _context.OrderItem.Where(x => x.ParentId == orderItemId || x.Id == orderItemId)
            //      .UpdateAsync(x => new OrderItem() { Deleted = true });
        }

        public IQueryable<OrderItem> GetAllWithDeletedOrderItems(int orderId)
        {
            var query = DbSet.AsNoTracking().Where(x =>
                !x.ParentId.HasValue &&
                x.OrderId == orderId &&
                x.ApplicationId == _userPrincipal.ApplicationId);
            return ApplyUserAccessFilterToQuery(query);
        }

        public IQueryable<OrderItemFullInfoView> GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum subsidyType,
            int orderCreationDateFromDate, int orderCreationDateToDate)
        {
            return _context.OrderItemFullInfoView.AsNoTracking().Where(x =>
                x.OrderStatusType == (int)OrderStatusTypesEnum.Complete &&
                !x.ParentId.HasValue &&
                !x.OrderItemIsDeleted &&
                x.SubsidyStatusTypeId.HasValue &&
                (x.SubsidyStatusTypeId == (int)SubsidyStatusTypesEnum.Confirmed || x.SubsidyStatusTypeId == (int)SubsidyStatusTypesEnum.Rejected) &&
                x.SubsidyTypeId.HasValue && x.SubsidyTypeId == (int)subsidyType &&
                x.CreationDateInt >= orderCreationDateFromDate &&
                x.CreationDateInt <= orderCreationDateToDate
            );
        }

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
        public Task<bool> GetSaleConfigurationAllProductsIncludeInsuranceAsync(int saleConfigurationId)
        {
            return _context.SaleConfiguration
                .Where(sc => sc.Id == saleConfigurationId)
                .Select(sc => sc.AllProductsIncludeInsurance)
                .FirstOrDefaultAsync();
        }



        #region Factored or Unfacored OrderItems Methods

        /// <summary>
        /// آیتم های سفارشی که فرزند ندارند مربوط به کاربری که بر اساس مرکز و نمایندگی اش به آنها دسترسی دارد
        /// </summary>
        /// <param name="readOnly"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IQueryable<OrderItem> GetAllForFactor(bool readOnly = false, int? applicationId = null)
        {
            return PureGetAll(readOnly, applicationId, true).Where(x => x.InverseParent.Count == 0);
        }

        /// <summary>
        /// کووری مربوط ه آیتم های فاکتور نشده بر اساس فیلتر مرکز و نمایندگی کاربر
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetUnFactoredOrderItems(bool readOnly = false)
        {
            // the order Item should not be factored before
            var query = GetAllForFactor(readOnly);
            query = query.Where(x => x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete &&
                                     x.FactorItem == null
            );
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// get all orderItems for the specific user based on company Id or representation Id
        /// </summary>
        /// <returns></returns>
        public IQueryable<OrderItem> GetUnFactoredOrderItemsInTaxCategory(int taxCategoryId, bool readOnly = false)
        {
            return GetUnFactoredOrderItems(readOnly)
                .Where(x => x.ProductSalesPlanning.Product.TaxCategoryId == taxCategoryId);
        }

        public bool CheckExistenceUnFactoredOrderItemsInTaxCategory(int taxCategoryId)
        {
            return GetUnFactoredOrderItemsInTaxCategory(taxCategoryId, true).Any();
        }

        /// <summary>
        /// آیا این محصول در آیتم سفارشات وجود دارد 
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns></returns>
        public bool IsProductInOrderItems(int productId)
        {
            return DbSet.Any(x => x.ProductSalesPlanning.ProductId == productId);
        }

        public Task<int> SetOrderItemPriceRightShareAsync(int orderId, int userId)
        {
            return _context.Database.ExecuteSqlRawAsync($"[shp].[SetOrderItemPriceRightShare] {1},{userId},{orderId}");
        }

        /// <summary>
        /// دریافت لیست سفارشات برنامه
        /// </summary>
        /// <param name="readOnly">آیا خواندنی است</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetAllByApplicationId(bool readOnly = false, int? applicationId = null)
        {
            return base.GetAll(readOnly, applicationId);
        }

        public IQueryable<OrderItem> GetAllOrderItemsByFilter(
            int productSalesPlanningId,
            bool withDeleted = false,
            bool readOnly = false)
        {
            /*
             * این نکته اینجا حائز اهمیت است که
             * نباید از متد GetAll استفاده نمود زیرا
             * دوحالت وجود دارد
             * 1- این متد از طریق پنل عمومی استفاده میشود
             * 2- اسن متد از طریق پنل ادمین مورد استفاده قرار میگیرد
             * در هر دوحالت ممکن است applicationId کاربر و appsetting.applicationId
             * برابر نباشند در نتیجه با مشکل مواجه میشویم لذا در اینجا چون productSalesPlanningId وجود دارد
             * و در دل خود ApplicationId دارد نیازی به استفاده از
             * متد GetAll نیست
             *
             */
            var query = DbSet.Where(x =>
                x.ProductSalesPlanningId == productSalesPlanningId && (withDeleted || (!withDeleted && !x.Deleted)) &&
                !x.ParentId.HasValue);

            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<OrderItem> GetOrderItemChildrenByParentId(int orderItemId, int applicationId)
        {
            return GetAllByApplicationId(true, applicationId)
                .Where(x => x.ParentId.Value == orderItemId);
        }


        /// <summary>
        /// آیستم های سفارش یک تنظیم فروش که فرزند ندارند و فاکتورنشده اند
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <returns></returns>
        public IQueryable<OrderItem> GetUnFactoredOrderItemBySaleConfigurationId(int saleConfigurationId)
        {
            return DbSet.Where(x => x.SaleConfigurationId == saleConfigurationId
                                         && x.InverseParent.Count == 0
                                         && x.FactorItem == null);

        }
        #endregion
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
            var query = DbSet.Where(x => x.Deleted
            && x.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed
            && x.SubsidyPriceIncTaxIncProfit > 0
            && (x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete ||
                x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Refund)
            && x.Order.Refund.Any()
            && x.FactorItem.Factor.FactorGroup != null
            && x.InverseParent.Count == 0
            && x.FactorItem.Factor.FactorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed);
            if (orderCreationDateFromDate.HasValue)
            {
                query = query.Where(x => x.Order.CreationDateInt >= orderCreationDateFromDate.Value);
            }
            if (orderCreationDateToDate.HasValue)
            {
                query = query.Where(x => x.Order.CreationDateInt <= orderCreationDateToDate.Value);
            }
            if (factorId.HasValue)
            {
                query = query.Where(x => x.FactorItem.Factor.Id == factorId.Value);
            }
            if (subsidyType.HasValue && subsidyType.Value > 0)
            {
                query = query.Where(x => x.Order.SubsidyType == subsidyType.Value);
            }

            return query;
        }
    }
}