using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class AdditionalUserPropCompanyGroupController : BaseController<AdditionalUserPropCompanyGroup ,int,AdditionalUserPropCompanyGroupListViewModel, AdditionalUserPropCompanyGroupEditModel,AdditionalUserPropCompanyGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IAdditionalUserPropCompanyGroupService _additionalUserPropCompanyGroupService;
		private readonly IYashilLogger<AdditionalUserPropCompanyGroup> _logger;
        private readonly IPermissionService _permissionService;
        public AdditionalUserPropCompanyGroupController(IAdditionalUserPropCompanyGroupService additionalUserPropCompanyGroupService, 
        IMapper mapper, 
        IYashilLogger<AdditionalUserPropCompanyGroup> logger,
        IPermissionService permissionService) : base(additionalUserPropCompanyGroupService, mapper,logger, permissionService)
        {
            _mapper = mapper;
            _additionalUserPropCompanyGroupService = additionalUserPropCompanyGroupService;
			_logger = logger;
			_permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? additionalUserPropId, int? companyGroupId)
        {
            var additionalUserPropCompanyGroups = _additionalUserPropCompanyGroupService.GetByCustomFilter(additionalUserPropId,companyGroupId);
            return await DataSourceLoader.LoadAsync(additionalUserPropCompanyGroups.ProjectTo<AdditionalUserPropCompanyGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? additionalUserPropId, int? companyGroupId)
        {
            var additionalUserPropCompanyGroups = _additionalUserPropCompanyGroupService.GetByCustomFilter(additionalUserPropId,companyGroupId);
            return await DataSourceLoader.LoadAsync(additionalUserPropCompanyGroups.ProjectTo<AdditionalUserPropCompanyGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد در فرم تخصیص گروه مرکز به کاربر کاربرد دارد
        /// گروه هایی را که به کاربر اختصاص داده نشده اند را در باکس سمت راست فرم، لیست میکند و اپراتور می تواند این
        /// اپلیکیشن ها را در صورت لزوم به کاربر اختصاص دهد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id">شناسه کاربر</param>
        /// <returns></returns>
        [HttpGet("GetAdditionalUserPropCompanyGroupsNotAssignedToUserAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAdditionalUserPropCompanyGroupsNotAssignedToUserAsync(
            CustomDataSourceLoadOptions loadOptions, int id)
        {
            var entities = _additionalUserPropCompanyGroupService.GetAdditionalUserPropCompanyGroups(id, false);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CompanyGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد در فرم تخصیص گروه مرکز به کاربر کاربرد دارد
        /// گروه هایی را که به کاربر اختصاص داده شده اند را در باکس سمت چپ فرم، لیست میکند و اپراتور می تواند این
        /// اپلیکیشن ها را در صورت لزوم از اختصاص کاربر خارج نماید
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id">شناسه کاربر</param>
        /// <returns></returns>
        [HttpGet("GetAdditionalUserPropCompanyGroupsAssignedToUserAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAdditionalUserPropCompanyGroupsAssignedToUserAsync(
            CustomDataSourceLoadOptions loadOptions, int id)
        {
            var entities = _additionalUserPropCompanyGroupService.GetAdditionalUserPropCompanyGroups(id, true);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CompanyGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// این متد در فرم تخصیص کاربر به گروه مرکز کاربرد دارد
        /// این متد برای تخصیص گروه مرکز به کاربر و خارج کردن تخصیص کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="assignableListEditModel">مدل تخصیص</param>
        /// <returns></returns>
        [HttpPost("AssignSelectedItemsToUser")]
        [YashilAuthorization(ActionName = "ModifyUserPropCompanyGroups")]
        public async Task<bool> AssignSelectedCompanyGroupesToUserAsync(AssignableListEditModel assignableListEditModel)
        {
            var result = await _additionalUserPropCompanyGroupService.AssignSelectedCompanyGroupesToUser(
                [.. assignableListEditModel.SelectedItems], assignableListEditModel.GroupId,
                assignableListEditModel.Assign);

            if (result)
            {
                if (assignableListEditModel.Assign)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)Events.Insert, Events.Insert.ToString()), nameof(UserApplication), "Update Record", null);
                }
                else
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)Events.Delete, Events.Delete.ToString()), nameof(UserApplication), "Delete Record", null);
                }
            }
            return result;
        }

    }
}      