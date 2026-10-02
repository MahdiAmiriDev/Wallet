using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.ControllersExtenders;
using Yashil.Core.CustomViewModels.UserManagement;
using Yashil.Core.Entities;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UserExtenderController : BaseController<User, int, UserListViewModel, UserEditModel,
        UserSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserControllerExtender _userControllerExtender;
        public UserExtenderController(IUserService userService, IMapper mapper, IYashilLogger<User> yashilLogger, IPermissionService permissionService, IUserControllerExtender userControllerExtender) : base(userService, mapper, yashilLogger, permissionService)
        {
            _mapper = mapper;
            _userControllerExtender = userControllerExtender;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="representationCategoryId"></param>
        /// <param name="personId"></param>
        /// <param name="considerCurrentUserAdditionalData">اطلاعات اضافی کاربر مانند مرکز و نمایندگی و دپارتمان نمایندگی را در نظر بگیرد</param>
        /// <returns></returns>
        [HttpGet("GetForSelectByFilter")]
        public Task<LoadResult> GetForSelectByFilter(CustomDataSourceLoadOptions loadOptions,
            int? companyId, 
            int? representationId,
            int? representationCategoryId,
            int? personId,
            bool considerCurrentUserAdditionalData = true)
        {
            var users = _userControllerExtender.GetForSelectByFilter(companyId, representationId, representationCategoryId, personId, considerCurrentUserAdditionalData);
            return DataSourceLoader.LoadAsync(
                users.ProjectTo<UserSelectSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
    }
}
