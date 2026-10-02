using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class CustomerClubController : YashilMvcBaseController<CustomerClub>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClubService _customerClubService;
        private readonly IUserPrincipal _userPrincipal;
        public CustomerClubController(IMapper mapper, ICustomerClubService customerClubService, IYashilLogger<CustomerClub> logger,
            IPermissionService permissionService, ApplicationConfiguration applicationConfiguration, IUserPrincipal userPrincipal) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _customerClubService = customerClubService;
            _userPrincipal = userPrincipal;
        }
         
        public async Task<ActionResult> CustomerClubMembershipVerify(int? custommerClubId)
        {
            var customerClub = await _customerClubService.GetAsync(custommerClubId, true);
            var customerClubEditModel = _mapper.Map<CustomerClub, CustomerClubMembershipVerifyViewModel>(customerClub);
            return View(customerClubEditModel);
        }
    }
}
