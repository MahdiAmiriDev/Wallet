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
    public class RepresentationPersonController : BaseController<RepresentationPerson, int, RepresentationPersonListViewModel, RepresentationPersonEditModel, RepresentationPersonSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepresentationPersonService _representationPersonService;
        private readonly IYashilLogger<RepresentationPerson> _logger;  private readonly IPermissionService _permissionService;
        public RepresentationPersonController(IRepresentationPersonService representationPersonService, IMapper mapper, IYashilLogger<RepresentationPerson> logger,IPermissionService permissionService): base(representationPersonService, mapper,logger,permissionService)
        {
            _mapper = mapper;
            _logger = logger; _permissionService = permissionService;
            _representationPersonService = representationPersonService;
        }

        [HttpGet("GetByRepresentationIdForList")]
        public async Task<LoadResult> GetByRepresentationIdForList(CustomDataSourceLoadOptions loadOptions, int representationId)
        {
            var representations = _representationPersonService.GetByRepresentationId(representationId);
            return await DataSourceLoader.LoadAsync(representations.ProjectTo<RepresentationPersonListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? representationId, int? personId, int? postId)
        {
            var representationPersons = _representationPersonService.GetByCustomFilter(representationId, personId, postId);
            return await DataSourceLoader.LoadAsync(representationPersons.ProjectTo<RepresentationPersonListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}