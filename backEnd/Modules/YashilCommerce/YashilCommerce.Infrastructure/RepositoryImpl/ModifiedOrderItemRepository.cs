using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ModifiedOrderItemRepository : GenericApplicationBasedRepository<ModifiedOrderItem, int>,
        IModifiedOrderItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ModifiedOrderItemRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfigurator) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfigurator;
        }

        public IQueryable<ModifiedOrderItem> GetByCustomFilter(int? oldOrderItemId, int? newOrderId,
            int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            var query = GetAll(true);
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            if (additionalUserProp != null && additionalUserProp.CompanyId.HasValue)
            {
                if (additionalUserProp.RepresentationId != null)
                {
                    query = query.Where(x =>
                        x.OldOrderItem.ProductSalesPlanning.RepresentationId == additionalUserProp.RepresentationId);
                }
                else
                {
                    query = query.Where(x =>
                        x.OldOrderItem.ProductSalesPlanning.Product.CompanyId == additionalUserProp.CompanyId);
                }
            }

            if (oldOrderItemId.HasValue)
            {
                query = query.Where(x => x.OldOrderItemId == oldOrderItemId.Value);
            }

            if (newOrderId.HasValue)
            {
                query = query.Where(x => x.NewOrderItemId == newOrderId.Value);
            }

            if (modifiedOrderItemType.HasValue)
            {
                query = query.Where(x => x.ModifiedOrderItemType == modifiedOrderItemType.Value);
            }

            if (modifiedOrderItemStatusType.HasValue)
            {
                query = query.Where(x => x.ModifiedOrderItemStatusType == modifiedOrderItemStatusType.Value);
            }

            if (assignee.HasValue)
            {
                query = query.Where(x => x.Assignee == assignee.Value);
            }


            return query.OrderByDescending(x => x.Id).AsNoTracking();
        }

        public List<CalcedOrderItemPrice> ModifyOrderItemPreview(
            int oldOrderItemId,
            int? newProductSalePlanningId,
            int? representationId,
            bool subsidyBased,
            bool? calcMaxSubsidy,
            bool changeToItSelf = false
        )
        {
            int? calcMaxSubsidyInt = null;
            if (calcMaxSubsidy.HasValue)
            {
                calcMaxSubsidyInt = calcMaxSubsidy.Value ? 1 : 0;
            }
            return _context.CalcedOrderItemPrices.FromSqlRaw(" EXECUTE shp.ModifyOrderItem {0},{1},{2},{3},{4},{5},{6}",
                oldOrderItemId,
                newProductSalePlanningId,
                subsidyBased,
                _userPrincipal.ApplicationId,
                calcMaxSubsidyInt,
                representationId,
                changeToItSelf).ToList();
        }
        /// <summary>
        ///  آیتم سفارش اصلاح شده را به قبل برمی گرداند
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        public async Task<int> CancelModification(int modifiedOrderItemId)
        {
            return await _context.Database.ExecuteSqlRawAsync($"EXECUTE [shp].[CancelModifiedOrderItem] {modifiedOrderItemId} ");
        }
        /// <summary>
        /// بررسی می کند که modifiedOrderItemId
        /// آخرین رکورد برای درج شده در 
        /// modifiedOrderItem
        /// که در وضعیت تایید شده توسط کارشناس
        /// crm
        /// یا تمکمیل شده باشد
        /// برابر با رکوردی که شناسه آن در پارامتر است
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        public async Task<bool> IsLastModifiedOrderItemRecord(int modifiedOrderItemId)
        {
            var orderId = DbSet.Where(x => x.Id == modifiedOrderItemId).Select(m => m.OldOrderItem.OrderId).FirstOrDefault();
            return await DbSet.Where(x => x.OldOrderItem.OrderId == orderId &&
          (x.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.Completed
          || x.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.ApprovedByCrmExpert
          )
            ).MaxAsync(c => c.Id) == modifiedOrderItemId;
        }
        /// <summary>
        /// بررسی می کند که آیتم سفارش قبلی در وضعیت اصلاح آیتم سفارش ارسالی وجود دارد یا خیر ؟
        /// </summary>
        /// <param name="oldOrderItemId">شناسه آیتم سفارش قبلی</param>
        /// <param name="modifiedOrderItemStatusType">وضعیت اصلاح آیتم سفارش</param>
        /// <returns></returns>
        public async Task<bool> CheckExistOldOrderItemByModifyOrderItemStatusType(int oldOrderItemId, int modifiedOrderItemStatusType)
        {
            return await DbSet.AsNoTracking()
                .Where(x => x.OldOrderItemId == oldOrderItemId && x.ModifiedOrderItemStatusType == modifiedOrderItemStatusType).AnyAsync();
        }

        public IQueryable<ModifiedOrderItem> FindById(int id, bool readOnly)
        {
            return GetAll(readOnly).Where(x => x.Id == id);
        }

        public Task<int> UpdateModifiedOrderItemStatusTypeAsync(int id, ModifiedOrderItemStatusTypeEnum newStatusType)
        {
            return GetAll(true)
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(c => c.ModifiedOrderItemStatusType, (int)newStatusType));
        }
    }
}
