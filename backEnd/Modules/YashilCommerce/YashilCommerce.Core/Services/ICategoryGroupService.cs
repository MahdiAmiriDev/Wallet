using System.Collections.Generic;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
	public interface ICategoryGroupService : IGenericService<CategoryGroup,int>
    {
        /// <summary>
        /// این متد برای دریافت دسته بندی های گروه اپلیکیشن جاری کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryGroupInfoDto>> GetCurrentApplicationCategoryGroups();
        /// <summary>
        /// دریافت نام گروه دسته بندی بر اساس شناسه گروه دسته بندی
        /// </summary>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        Task<string> GetTitleAsync(int categoryGroupId);
    }
}      
 