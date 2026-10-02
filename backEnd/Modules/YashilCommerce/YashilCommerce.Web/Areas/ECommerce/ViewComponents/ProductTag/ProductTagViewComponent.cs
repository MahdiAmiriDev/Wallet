using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YashilCommerce.Core.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using AutoMapper;


namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.ProductTag
{
    /// <summary>
    /// کامپوننت مربوط به نمایش تگ های دوره ها یا محصولات
    /// </summary>
    public class ProductTagViewComponent : ViewComponent
    {
        private readonly IProductTagsService _productTagsService;
        private readonly IMapper _mapper;

        public ProductTagViewComponent(IProductTagsService productTagsService, IMapper mapper)
        {

            _productTagsService = productTagsService;
            _mapper = mapper;
        }


        /// <summary>
        /// این متد تگ های مرتبط با برنامه ریزی دوره را  نمایش می دهد
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int productSalesPlanningId)
        {
            var productTagsSimpleViewModel = await _productTagsService.GetByProductSalesPlanningId(productSalesPlanningId)
                    .ProjectTo<ProductTagsSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            return View("productTag", productTagsSimpleViewModel);
        }

    }
}