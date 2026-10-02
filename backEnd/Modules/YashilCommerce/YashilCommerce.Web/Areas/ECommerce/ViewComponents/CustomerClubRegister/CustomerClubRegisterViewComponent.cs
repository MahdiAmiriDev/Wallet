using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.CustomerClubRegister
{
    public class CustomerClubRegisterViewComponent : ViewComponent
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly ICustomerClubService _customerClubService;

        public CustomerClubRegisterViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper, ICustomerClubService customerClubService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _customerClubService = customerClubService;

        }

        public async System.Threading.Tasks.Task<IViewComponentResult> InvokeAsync()
        {
            System.Linq.IQueryable<Yashil.Core.Entities.CustomerClub> customerClubs = _customerClubService.GetAllByApplicationId(_applicationConfiguration.ApplicationId);
            var customerClubList = await customerClubs.ProjectTo<CustomerClubListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View("_CustomerClubRegister", customerClubList);


        }
    }
}
