using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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
    public class CitySubsidyFactorController : BaseController<CitySubsidyFactor, int, CitySubsidyFactorListViewModel, CitySubsidyFactorEditModel, CitySubsidyFactorSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICitySubsidyFactorService _citySubsidyFactorService;

        public CitySubsidyFactorController(ICitySubsidyFactorService citySubsidyFactorService,
        IMapper mapper,
        IYashilLogger<CitySubsidyFactor> logger,
        IPermissionService permissionService) : base(citySubsidyFactorService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _citySubsidyFactorService = citySubsidyFactorService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? cityId)
        {
            var citySubsidyFactors = _citySubsidyFactorService.GetByCustomFilter(cityId);
            return await DataSourceLoader.LoadAsync(citySubsidyFactors.ProjectTo<CitySubsidyFactorListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? cityId)
        {
            var citySubsidyFactors = _citySubsidyFactorService.GetByCustomFilter(cityId);
            return await DataSourceLoader.LoadAsync(citySubsidyFactors.ProjectTo<CitySubsidyFactorSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        protected override async Task<string> CustomValidateBeforeUpdateAsync(CitySubsidyFactorEditModel editModel)
        {
            if (editModel.VezaratSubsidyFactor <= 0)
            {
                return "کاربر گرامی ضریب یارانه وزارت باید یزرگتر از صفر باشد.";
            }
            if (editModel.SataSubsidyFactor <= 0)
            {
                return "کاربر گرامی ضریب یارانه ساتا باید بزرگتر از صفر باشد.";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        protected override async Task<string> CustomValidateBeforeInsertAsync(CitySubsidyFactorEditModel editModel)
        {
            if (editModel.VezaratSubsidyFactor <= 0)
            {
                return "کاربر گرامی ضریب یارانه وزارت باید بزرگتر از صفر باشد.";
            }
            if (editModel.SataSubsidyFactor <= 0)
            {
                return "کاربر گرامی ضریب یارانه ساتا باید بزرگتر از صفر باشد.";
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }
    }
}