using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Web.Areas.ECommerce.DataService;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.CourseCarouselWithImageHorizontal
{
    /// <summary>
    /// کامپوننت نمایش کروسل میانی صفحه ی هوم
    /// </summary>
    public class CourseCarouselWithImageHorizontalViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;

        public CourseCarouselWithImageHorizontalViewComponent(PublicDataService publicDataService)
        {
            _publicDataService = publicDataService;
        }

        /// <summary>
        /// دریافت اطلاعات مربوط به کروسل دوره ها و تنظیم جزئیات نمایش آن
        /// </summary>
        /// <param name="getBestSellerCourses">دریافت جدید ترین دوره ها</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(bool getBestSellerCourses)
        {
            List<CompanyProductSalesPlanningViewModel> companyProductSalesPlanningViewModelList;

            CourseCarouselWithImageHorizontalViewModel courseCarouselWithImageHorizontalViewModel = new();


            if (getBestSellerCourses)
            {
                companyProductSalesPlanningViewModelList = await _publicDataService.GetBestSellers();
                
                
                courseCarouselWithImageHorizontalViewModel.CarouselTitle ="پرفروش ترین دوره ها";
                courseCarouselWithImageHorizontalViewModel.CarouselName = "bestSellerCarousel";
            }
            else
            {
                companyProductSalesPlanningViewModelList = await _publicDataService.GetNewest();
               
               
                courseCarouselWithImageHorizontalViewModel.CarouselTitle = "جدید ترین دوره ها";
                courseCarouselWithImageHorizontalViewModel.CarouselName = "newestCarousel";
            }

            var companyProductsSalesPlanning = companyProductSalesPlanningViewModelList.Prepend(new CompanyProductSalesPlanningViewModel
            {
                Id = -1,
                CompanyTitle = string.Empty,
                ProductTitle = string.Empty,
                CompanyId = -1,
            });
            courseCarouselWithImageHorizontalViewModel.CompanyProductSalesPlanningViewModel = companyProductsSalesPlanning;

            return View(courseCarouselWithImageHorizontalViewModel);
        }




    }
}
