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
    public class
        VocherController : BaseController<Vocher, int, VocherListViewModel, VocherEditModel, VocherSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IVocherService _vocherService;
        private readonly IYashilLogger<Vocher> _logger;
        private readonly IPermissionService _permissionService;

        public VocherController(IVocherService vocherService,
            IMapper mapper,
            IYashilLogger<Vocher> logger,
            IPermissionService permissionService) : base(vocherService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _vocherService = vocherService;
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? vocherTypeId)
        {
            var vochers = _vocherService.GetByCustomFilter(vocherTypeId);
            return await DataSourceLoader.LoadAsync(
                vochers.ProjectTo<VocherListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? vocherTypeId)
        {
            var vochers = _vocherService.GetByCustomFilter(vocherTypeId);
            return await DataSourceLoader.LoadAsync(
                vochers.ProjectTo<VocherSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}