using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.ObjectFactory
{       
    public interface IProductSalePlanningFactory
    {
        Task<IViewComponentResult> InvokeAsync(string title, int? categoryId, int? representationId, int? cityId, int? categoryGroupId,List<int> productFilterCompany, bool showNoResultOnEmpty, int itemCount = 20,int? pageIndex = 1, bool? showProductsByList = null, bool showHomePageProductsView = false);

        Task<ProductsByFilterViewModel> GetProductPlanningByFilter(AdditionalUserPropsDto userAdditionalUserProp
            , string title, int? categoryId, int? categoryGroupId, int? cityId, List<int> company, int? representationId,
            bool showNoResultOnEmpty, int pageSize, int pageIndex, bool? showProductsByList);
    }
}