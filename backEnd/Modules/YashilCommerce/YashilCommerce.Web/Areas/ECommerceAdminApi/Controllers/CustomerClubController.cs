using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CustomerClubController : BaseController<CustomerClub, int, CustomerClubListViewModel, CustomerClubEditModel, CustomerClubSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClubService _customerClubService;
        private readonly IUserPrincipal _userPrincipal;


        public CustomerClubController(ICustomerClubService customerClubService, IMapper mapper,
            IYashilLogger<CustomerClub> logger,
            IPermissionService permissionService, 
            IUserPrincipal userPrincipal) :
            base(customerClubService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _customerClubService = customerClubService;

            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetAllByApplicationId")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetAllByApplicationId(CustomDataSourceLoadOptions loadOptions)
        {
            var customerClubs = _customerClubService.GetAllByApplicationId(_userPrincipal.ApplicationId);
            return await DataSourceLoader.LoadAsync(
                customerClubs.ProjectTo<CustomerClubSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetAllCustomerClubHasActiveContract")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetAllCustomerClubHasActiveContract(CustomDataSourceLoadOptions loadOptions,int productSalesPlanningId)
        {
            var customerClubs = _customerClubService.GetAllCustomerClubHasActiveContract(_userPrincipal.ApplicationId, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(
                customerClubs.ProjectTo<CustomerClubSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}
