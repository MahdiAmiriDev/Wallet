using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Home.Product
{
    /// <summary>
    /// این کلاس ، سرویس
    ///ICategoryGroupService
    ///را اینجکت می کند و از متد 
    /// InvokeAsync
    /// استفاده می کند
    /// که در آن
    /// دسته بندی گروه اپلیکیشن جاری کاربرد را دریافت می کند
    /// و نمایش ویو کامپوننت مروبط به سوییپر دسته بندی ها
    /// </summary>
    ///<returns>ViewComponent</returns>
    public class CategoryGroupProductsSalePlanningViewComponent : ViewComponent
    {
        private readonly ICategoryGroupService _categoryGroupService;
        public CategoryGroupProductsSalePlanningViewComponent(ICategoryGroupService categoryGroupService)
        {
            _categoryGroupService = categoryGroupService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryGroups = await _categoryGroupService.GetCurrentApplicationCategoryGroups();

            return View("CategoryGroupProductsSalePlanning", categoryGroups);
        }

    }
}

