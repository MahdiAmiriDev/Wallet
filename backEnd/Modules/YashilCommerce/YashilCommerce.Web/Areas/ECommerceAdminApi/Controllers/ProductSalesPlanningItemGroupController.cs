using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Common.Web.Infrastructure.BaseClasses;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductSalesPlanningItemGroupController : BaseController<ProductSalesPlanningItemGroup,
        int, ProductSalesPlanningItemGroupListViewModel, ProductSalesPlanningItemGroupEditModel,
        ProductSalesPlanningItemGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductSalesPlanningItemGroupService _productSalesPlanningItemGroupService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;

        public ProductSalesPlanningItemGroupController(IProductSalesPlanningItemGroupService productSalesPlanningItemGroupService,
        IMapper mapper,
        IYashilLogger<ProductSalesPlanningItemGroup> logger,
        IPermissionService permissionService, IProductSalesPlanningService productSalesPlanningService) : base(productSalesPlanningItemGroupService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _productSalesPlanningItemGroupService = productSalesPlanningItemGroupService;
        }
        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? itemGroupId, int? productSalesPlanningId)
        {
            var productSalesPlanningItemGroups = _productSalesPlanningItemGroupService.GetByCustomFilter(parentId, itemGroupId, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(productSalesPlanningItemGroups.ProjectTo<ProductSalesPlanningItemGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? itemGroupId, int? productSalesPlanningId)
        {
            var productSalesPlanningItemGroups = _productSalesPlanningItemGroupService.GetByCustomFilter(parentId, itemGroupId, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(productSalesPlanningItemGroups.ProjectTo<ProductSalesPlanningItemGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// دریافت آیتم های گروه بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetItemGroupByProductSalePlanningId")]
        [YashilAuthorization("GetEntities")]
        public async Task<LoadResult> GetItemGroupByProductSalePlanningId(CustomDataSourceLoadOptions loadOptions, int productSalesPlanningId)
        {
            var productSalesPlanningItemGroup =
                _productSalesPlanningItemGroupService.GetItemGroupByProductSalePlanningId(productSalesPlanningId, true);
            return await DataSourceLoader.LoadAsync(
                productSalesPlanningItemGroup.ProjectTo<ProductSalesPlanningItemGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }
        /// <summary>
        /// اساین کردن گروه های انتخابی به برنامه ریزی دوره
        /// </summary>
        /// <param name="assignProductSalePlanningToItemEditModel"></param>
        /// <returns></returns>
        [HttpPost("AssignProductSalePlanningToItemGroup")]
        [YashilAuthorization("PostEntity")]
        public async Task<ResultStatus<int>> AssignProductSalePlanningToItemGroup(
            AssignProductSalePlanningToItemEditModel assignProductSalePlanningToItemEditModel)
        {
            if (assignProductSalePlanningToItemEditModel.ProductSalesPlanningId <= 0 ||
                assignProductSalePlanningToItemEditModel.GroupsId.Count <= 0)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، اطلاعات ارسالی اشتباه می باشد."
                };
            }

            var isProductSalePlanningHeld =
                await _productSalesPlanningService.CheckProductSalePlanningHasBeenHeld(
                    assignProductSalePlanningToItemEditModel.ProductSalesPlanningId);
            if (isProductSalePlanningHeld.isHeld)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = isProductSalePlanningHeld.message
                };
            }

            (var succeed, string message) = await
                _productSalesPlanningItemGroupService.AssignProductSalePlanningToItemGroupAsync(
                    assignProductSalePlanningToItemEditModel.ProductSalesPlanningId,
                    assignProductSalePlanningToItemEditModel.GroupsId);
            return new ResultStatus<int>
            {
                IsSuccess = succeed,
                Message = message
            };
        }
        /// <summary>
        /// دریافت شناسه های آیتم گروه  بر اساس شناسه برنامه ریزی دوره
        ///  برای انتخاب چک باکس آیتم گروه ها در فرم آیتم گروه های برنامه ریزی دوره  
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        [HttpGet("GetItemGroupIds")]
        public Task<List<int>> GetItemGroupIdsAsync(int productSalesPlanningId)
        {
            return _productSalesPlanningItemGroupService.GetItemGroupIdsAsync(productSalesPlanningId);
        }
        /// <summary>
        /// دریافت لسیت آیتم گروه های اساین شده به برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        [HttpGet("GetItemGroupAssignedToProductSalesPlanning")]
        [YashilAuthorization("GetEntities")]
        public async Task<List<ItemGroupAssignedToProductSalesPlanningViewModel>> GetItemGroupAssignedToProductSalesPlanning(int productSalesPlanningId)
        {
            var productSalesPlanningItemGroup =
                _productSalesPlanningItemGroupService.GetItemGroupByProductSalePlanningId(productSalesPlanningId, true);
            return await productSalesPlanningItemGroup.ProjectTo<ItemGroupAssignedToProductSalesPlanningViewModel>
                (_mapper.ConfigurationProvider).ToListAsync();

        }
    }
}
