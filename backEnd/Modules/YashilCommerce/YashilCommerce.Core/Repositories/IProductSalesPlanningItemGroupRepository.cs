using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IProductSalesPlanningItemGroupRepository : IGenericRepository<ProductSalesPlanningItemGroup, int>
    {

        IQueryable<ProductSalesPlanningItemGroup> GetByCustomFilter(int? parentId, int? itemGroupId, int? productSalesPlanningId);

        /// <summary>
        /// دریافت آیتم های گروه بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId"> برنامه ریزی دوره</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanningItemGroup> GetItemGroupByProductSalePlanningId(int productSalePlanningId, bool readOnly = false);
        /// <summary>
        /// بررسی می کند که برنامه ریزی دوره در گروه ارسالی در پارمتر وجود دارد یا خیر 
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="itemGroupId">شناسه گروه</param>
        /// <returns></returns>
        Task<bool> CheckExistProductSalePlanningToItemGroupAsync(int productSalesPlanningId, int itemGroupId);
        /// <summary>
        /// لیست شناسه گروهها که در پارامتر وجود دارد
        /// را در جدول
        /// ProductSalesPlanningItemGroup
        /// بررسی می کند و شناسه گروههایی را بر می گرداند
        /// که برای برنامه ریزی دوره ارسالی در پارمتر قبلا ثبت نشده باشند 
        /// </summary>
        /// <param name="productSalePlanningId">برنامه ریزی دوره</param>
        /// <param name="groupsId">شناسه گروهها</param>
        /// <returns></returns>
        IEnumerable<int> GetGroupsIdsNotAssignForProductSalesPlanning(int productSalePlanningId, List<int> groupsId);
        /// <summary>
        /// دریافت برنامه ریزی دوره آیتم گروه
        /// بر اساس شناسه برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanningItemGroup> GetByProductSalePlanningId(int productSalePlanningId);
        /// <summary>
        /// ابتدا آیتم گروه های مربوط به برنامه ریزی دوره ارسالی در پارامتر دریافت می کنیم
        /// آن آیتم گروه هایی که در شناسه گروه ها ارسالی در پارامتر وحود ندارند را
        /// ار جدول
        /// ProductSalePlanningItemGroup
        /// حذف می کنیم
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="groupsId">شناسه گروه</param>
        /// <returns></returns>
        Task<int> DeleteProductSalePlanningItemGroupsNotExistsInGroupsIdAsync(int productSalePlanningId, List<int> groupsId);
    }
}
