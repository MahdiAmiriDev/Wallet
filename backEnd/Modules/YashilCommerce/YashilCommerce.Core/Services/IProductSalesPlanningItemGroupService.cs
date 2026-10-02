using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IProductSalesPlanningItemGroupService : IGenericService<ProductSalesPlanningItemGroup, int>
    {

        IQueryable<ProductSalesPlanningItemGroup> GetByCustomFilter(int? parentId, int? itemGroupId, int? productSalesPlanningId);

        /// <summary>
        /// دریافت آیتم های گروه بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId"> برنامه ریزی دوره</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanningItemGroup> GetItemGroupByProductSalePlanningId(int productSalesPlanningId, bool readOnly = false);
        /// <summary>
        /// اساین برنامه ریزی دوره به گروههای انتخابی
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="groupsId">شناسه گروه ها</param>
        /// <returns></returns>
        Task<(bool isSucces ,string message)> AssignProductSalePlanningToItemGroupAsync
            (int productSalePlanningId, List<int> groupsId);
        /// <summary>
        /// بررسی می کند که برنامه ریزی دوره در گروه ارسالی در پارمتر وجود دارد یا خیر 
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="itemGroupId">شناسه گروه</param>
        /// <returns></returns>
        Task<bool> CheckExistProductSalePlanningToItemGroupAsync(int productSalesPlanningId, int itemGroupId);
        /// <summary>
        /// دریافت شناسه های آیتم گروه  بر اساس شناسه برنامه ریزی دوره
        ///  برای انتخاب چک باکس آیتم گروه ها در فرم آیتم گروه های برنامه ریزی دوره  
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<List<int>> GetItemGroupIdsAsync(int productSalesPlanningId);
        
    }
}
