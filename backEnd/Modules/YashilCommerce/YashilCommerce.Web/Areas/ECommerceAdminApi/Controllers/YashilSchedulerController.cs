using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class YashilSchedulerController : BaseController<YashilScheduler, int, YashilSchedulerListViewModel, YashilSchedulerEditModel, YashilSchedulerSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IYashilSchedulerService _yashilSchedulerService;
        private readonly IYashilLogger<YashilScheduler> _logger;
        private readonly IPermissionService _permissionService;
        public YashilSchedulerController(IYashilSchedulerService yashilSchedulerService, IMapper mapper, IYashilLogger<YashilScheduler> logger, IPermissionService permissionService) : base(yashilSchedulerService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _yashilSchedulerService = yashilSchedulerService;
            _logger = logger;
            _permissionService = permissionService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? occuranceType)
        {
            var yashilSchedulers = _yashilSchedulerService.GetByCustomFilter(occuranceType);
            return await DataSourceLoader.LoadAsync(yashilSchedulers.ProjectTo<YashilSchedulerListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? occuranceType)
        {
            var yashilSchedulers = _yashilSchedulerService.GetByCustomFilter(occuranceType);
            return await DataSourceLoader.LoadAsync(yashilSchedulers.ProjectTo<YashilSchedulerSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(YashilSchedulerEditModel editModel)
        {
            var message = CustomValidate(editModel);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(YashilSchedulerEditModel editModel)
        {
            var message = CustomValidate(editModel);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override Task CustomMapBeforeInsertAsync(YashilSchedulerEditModel editModel, YashilScheduler entity)
        {
            //به صورت پیش فرض تعداد دفعات تکرار را برابر 1 قرار می دهیم
            entity.OcuuranceValue = 1;

            return base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override Task CustomMapBeforeUpdateAsync(YashilSchedulerEditModel editModel, YashilScheduler entity)
        {
            //به صورت پیش فرض تعداد دفعات تکرار را برابر 1 قرار می دهیم
            entity.OcuuranceValue = 1;

            return base.CustomMapBeforeUpdateAsync(editModel, entity);
        }


        private string CustomValidate(YashilSchedulerEditModel editModel)
        {
            //به صورت پیش فرض تعداد دفعات تکرا را برابر 1 قرار می دهیم
            const int ocuuranceValue = 1;

            if (editModel.ToEra <= editModel.FromEra)
            {
                return "انتهای بازه زمانی باید بزرگتر از ابتدای آن باشد.";
            }

            if (editModel.ToEra > ocuuranceValue * 30)
            {
                return "حداکثر انتهای بازه زمانی " + ocuuranceValue * 30 + "میباشد";
            }

            if (editModel.RepeatNumber <= 0)
            {
                return "مقدار تکرار حداقل 1 میباشد";
            }

            if (editModel.EndDate != null && editModel.EndDate <= editModel.StartDate)
            {
                return "تاریخ پایان باید بعد از تاریخ شروع باشد";
            }

            return null;
        }
    }
}