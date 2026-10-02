using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.RelatedProduct
{
    /// <summary>
    /// کامپوننت مربوط به نمایش برنامه ریزی دوره های فعال مربوط
    /// به شناسه تگ های ارسالی
    /// که بر اساس دوره های که بیشترین تگ مشترک را با شناسه تگ های 
    /// ارسالی دارد مرتب سازی می کند
    /// </summary>
    public class RelatedProductViewComponent : ViewComponent
    {
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IMapper _mapper;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public RelatedProductViewComponent(IProductSalesPlanningService productSalesPlanningService, IMapper mapper, IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal)
        {
            _productSalesPlanningService = productSalesPlanningService;
            _mapper = mapper;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<int> tagIds)
        {

            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var productSalesPlannings = await _productSalesPlanningService
                .GetProductSalesPlanningRelatedToTagIds(tagIds, additionalUserProp.CustomerClubId)
                .ProjectTo<ProductSalePlanningViewModel>
                  (_mapper.ConfigurationProvider,new Dictionary<string, object>{{"customerClubId", additionalUserProp.CustomerClubId }})
            .Distinct().OrderByDescending(x => x.RelatedTagCount)
            .ToListAsync();

            ProductsByFilterViewModel productsByFilter = new()
            {
                IsResumeFillOutEnough = additionalUserProp.IsResumeFillOutEnough,
                ProductSalePlannings = productSalesPlannings,
                ShowNoResultOnEmpty = false,
                ShowProductsByList = false,
                SubsidyBased = additionalUserProp.SupportSubsidy
            };

            return View("RelatedProduct", productsByFilter);
        }
    }

}
