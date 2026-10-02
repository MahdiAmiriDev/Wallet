using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductSalesPlanningItemGroupRepository : GenericApplicationBasedRepository<ProductSalesPlanningItemGroup, int>, IProductSalesPlanningItemGroupRepository
    {
        private readonly YashilAppDbContext _context;
        public ProductSalesPlanningItemGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) 
            : base(context, userPrincipal)
        {
            _context = context;
        }


        public IQueryable<ProductSalesPlanningItemGroup> GetByCustomFilter(int? parentId, int? itemGroupId, int? productSalesPlanningId)
        {
            var query = GetAll(true);
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            if (itemGroupId.HasValue)
            {
                query = query.Where(x => x.ItemGroupId == itemGroupId.Value);
            }
            if (productSalesPlanningId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanningId == productSalesPlanningId.Value);
            }


            return query;
        }

        public IQueryable<ProductSalesPlanningItemGroup> GetItemGroupByProductSalePlanningId(int productSalePlanningId, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.ProductSalesPlanningId == productSalePlanningId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public Task<bool> CheckExistProductSalePlanningToItemGroupAsync(int productSalesPlanningId, int itemGroupId)
        {
            return DbSet.AsNoTracking().AnyAsync(x =>
                x.ProductSalesPlanningId == productSalesPlanningId &&
                x.ItemGroupId == itemGroupId
            );
        }

        public IEnumerable<int> GetGroupsIdsNotAssignForProductSalesPlanning(int productSalePlanningId, List<int> groupsId)
        {
            // دریافت آیتم گروه هایی که قبلا به برنامه ریزی دوره فوق اساین شده اند
            var groupsIdsAssignForProductSalesPlanning =
                GetByProductSalePlanningId(productSalePlanningId)
                    .Select(x => x.ItemGroupId);

            // دریافت شناسه آیتم گروه هایی که پدر می باشند 
            var parentItemsGroup = _context.ItemGroup.Where(ig =>
                    ig.AppEntityId == (int)AppEntityEnum.AdditionalUserProp &&
                    ig.InverseParent.Count > 0)
                .Select(x => x.Id);

            // گروه هایی که قبلا به برنامه ریزی دوره اساین شده اند 
            //  و گروه هایی که دارای پدر می باشند
            // باید از لیست گروه هایی ارسالی در پارامتر خارج شوند
            return groupsId.Where(x =>
                     !groupsIdsAssignForProductSalesPlanning.Contains(x) &&
                     !parentItemsGroup.Contains(x)
                     )
                 .Select(g => g);
        }

        public IQueryable<ProductSalesPlanningItemGroup> GetByProductSalePlanningId(int productSalePlanningId)
        {
            return GetAll(true).Where(x => x.ProductSalesPlanningId == productSalePlanningId);
        }

        public Task<int> DeleteProductSalePlanningItemGroupsNotExistsInGroupsIdAsync(int productSalePlanningId, List<int> groupsId)
        {
            return DbSet.Where(x =>
                x.ProductSalesPlanningId == productSalePlanningId &&
                !groupsId.Contains(x.ItemGroupId)).DeleteFromQueryAsync();
        }
    }
}
