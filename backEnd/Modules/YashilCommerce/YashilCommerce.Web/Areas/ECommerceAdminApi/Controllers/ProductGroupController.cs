
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductGroupController : BaseController<ProductGroup, int, ProductGroupListViewModel, ProductGroupEditModel, ProductGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductGroupService _ProductGroupService;
        private readonly IYashilLogger<ProductGroup> _logger;
        private readonly IPermissionService _permissionService;

        public ProductGroupController(IProductGroupService ProductGroupService,
        IMapper mapper,
        IYashilLogger<ProductGroup> logger,
        IPermissionService permissionService) : base(ProductGroupService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _ProductGroupService = ProductGroupService;
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? categoryGroupId, bool? isIncludeTax)
        {
            var ProductGroups = _ProductGroupService.GetByCustomFilter(parentId, categoryGroupId, isIncludeTax);
            return await DataSourceLoader.LoadAsync(ProductGroups.ProjectTo<ProductGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? categoryGroupId)
        {
            var ProductGroups = _ProductGroupService.GetByCustomFilter(parentId, categoryGroupId, null);
            return await DataSourceLoader.LoadAsync(ProductGroups.ProjectTo<ProductGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(ProductGroupEditModel editModel)
        {
            var validationResult = await ValidateProductGroupCodeAndMainCode(editModel.Code, editModel.MainCode);

            if (!validationResult.isValid)
                return validationResult.message;

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(ProductGroupEditModel editModel)
        {
            var validationResult = await ValidateProductGroupCodeAndMainCode(editModel.Code, editModel.MainCode, editModel.Id);

            if (!validationResult.isValid)
                return validationResult.message;

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        /// <summary>
        /// این متد بررسی میکند که کد وارد شده معتبر است یا نه
        /// در صورتی که تکراری باشد نامعتبر است
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mainCode"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<(bool isValid, string message)> ValidateProductGroupCodeAndMainCode(string code, string mainCode, int id = 0)
        {
            if (string.IsNullOrEmpty(mainCode))
                return (false, "کاربر گرامی مقدار کد اصلی نمی تواند خالی باشد");

            if (string.IsNullOrEmpty(code))
                return (false, "کاربر گرامی مقدار کد نمی تواند خالی باشد");

            //حالت درج
            if (id == 0 && await _ProductGroupService.GetByCode(code).AnyAsync())
                return (false, "کاربر گرامی مقدار کد ارسالی قبلا در سامانه ثبت شده است");


            //حالت بروزرسانی
            if (id != 0)
            {
                var ids = await _ProductGroupService.GetByCode(code)
                    .Select(x => x.Id).ToListAsync();

                //خودش را بروز می کند
                if (ids.Count == 1 && ids[0] == id)
                    return (true, string.Empty);

                //مثلش وجود نداشته
                if (ids.Count == 0)
                    return (true, string.Empty);


                return (false, "کاربر گرامی مقدار کد ارسالی قبلا در سامانه ثبت شده است");
            }


            return (true, string.Empty);
        }


        [HttpGet("GetProductGroupForSelect")]
        [YashilAuthorization("GetForSelect")]
        public async Task<LoadResult> GetProductGroupForSelectAsync(CustomDataSourceLoadOptions loadOptions, bool isIncludeTax = false)
        {
            return await DataSourceLoader.LoadAsync(_ProductGroupService.GetByCustomFilter(null, null, isIncludeTax)
                .ProjectTo<ProductGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}