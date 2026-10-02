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
    public class CustomerClubCardController : BaseController<CustomerClubCard, int, CustomerClubCardListViewModel, CustomerClubCardEditModel, CustomerClubCardSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClubCardService _customerClubCardService;
        private readonly IYashilLogger<CustomerClubCard> _logger;
        private readonly IPermissionService _permissionService;
        public CustomerClubCardController(ICustomerClubCardService customerClubCardService, IMapper mapper, IYashilLogger<CustomerClubCard> logger, IPermissionService permissionService) : base(customerClubCardService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _customerClubCardService = customerClubCardService;
            _logger = logger;
            _permissionService = permissionService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? personId, int? customerClubId)
        {
            var customerClubCards = _customerClubCardService.GetByCustomFilter(personId, customerClubId);
            return await DataSourceLoader.LoadAsync(customerClubCards.ProjectTo<CustomerClubCardListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? personId, int? customerClubId)
        {
            var customerClubCards = _customerClubCardService.GetByCustomFilter(personId, customerClubId);
            return await DataSourceLoader.LoadAsync(customerClubCards.ProjectTo<CustomerClubCardSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}