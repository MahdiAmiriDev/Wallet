using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IProductGroupService : IGenericService<ProductGroup, int>
    {
        IQueryable<ProductGroup> GetByCustomFilter( int? parentId, int? categoryGroupId,bool? isIncludeTax);

        /// <summary>
        /// دریافت
        /// ProductCategory
        /// های مربوط به یک شناسه گروه دسته بندی
        /// </summary>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        IQueryable<ProductGroup> GetByCategoryGroupId(int categoryGroupId);

        /// <summary>
        /// دریافت ProductGroup
        /// براساس
        /// MainCode
        /// </summary>
        /// <param name="mainCode"></param>
        /// <returns></returns>
        IQueryable<ProductGroup> GetByMainCode(string mainCode);

        /// <summary>
        /// دریافت
        /// productGroup
        /// با استفاده از کد
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IQueryable<ProductGroup> GetByCode(string code);


    }
}
