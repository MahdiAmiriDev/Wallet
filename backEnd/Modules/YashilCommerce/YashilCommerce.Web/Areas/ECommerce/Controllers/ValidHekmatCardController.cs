	 
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerce.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
	public class ValidHekmatCardController : BaseController<ValidHekmatCard ,int,ValidHekmatCardListViewModel, ValidHekmatCardEditModel,ValidHekmatCardSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IValidHekmatCardService _validHekmatCardService;
		private readonly IYashilLogger<ValidHekmatCard> _logger;
        private readonly IPermissionService _permissionService;
        public ValidHekmatCardController(IValidHekmatCardService validHekmatCardService, IMapper mapper, IYashilLogger<ValidHekmatCard> logger, IPermissionService permissionService) : base(validHekmatCardService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _validHekmatCardService=validHekmatCardService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? hekmatCardType)
        {
            var validHekmatCards = _validHekmatCardService.GetByCustomFilter(hekmatCardType);
            return await DataSourceLoader.LoadAsync(validHekmatCards.ProjectTo<ValidHekmatCardListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? hekmatCardType)
        {
            var validHekmatCards = _validHekmatCardService.GetByCustomFilter(hekmatCardType);
            return await DataSourceLoader.LoadAsync(validHekmatCards.ProjectTo<ValidHekmatCardSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      