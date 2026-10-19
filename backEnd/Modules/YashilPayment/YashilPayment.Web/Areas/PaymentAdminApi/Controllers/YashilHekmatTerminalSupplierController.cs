using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class YashilHekmatTerminalSupplierController : BaseController<YashilHekmatTerminalSupplier, int, YashilHekmatTerminalSupplierListViewModel, YashilHekmatTerminalSupplierEditModel, YashilHekmatTerminalSupplierSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IYashilHekmatTerminalSupplierService _yashilHekmatTerminalSupplierService;
        private readonly IYashilLogger<YashilHekmatTerminalSupplier> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserPrincipal _userPrincipal;

        public YashilHekmatTerminalSupplierController(IYashilHekmatTerminalSupplierService yashilHekmatTerminalSupplierService,
        IMapper mapper,
        IYashilLogger<YashilHekmatTerminalSupplier> logger,
        IPermissionService permissionService, IUserRoleService userRoleService, IUserPrincipal userPrincipal) : base(yashilHekmatTerminalSupplierService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _yashilHekmatTerminalSupplierService = yashilHekmatTerminalSupplierService;
            _logger = logger;
            _permissionService = permissionService;
            _userRoleService = userRoleService;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? userId)
        {
            var yashilHekmatTerminalSuppliers = _yashilHekmatTerminalSupplierService.GetByCustomFilter(parentId, userId);
            return await DataSourceLoader.LoadAsync(yashilHekmatTerminalSuppliers.ProjectTo<YashilHekmatTerminalSupplierListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? userId)
        {
            var yashilHekmatTerminalSuppliers = _yashilHekmatTerminalSupplierService.GetByCustomFilter(parentId, userId);
            return await DataSourceLoader.LoadAsync(yashilHekmatTerminalSuppliers.ProjectTo<YashilHekmatTerminalSupplierSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات تامین کنندگان
        /// مربوط به درگاه پرداخت حکمت کاربرد دارد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetYashilHekmatTerminalSupplierInformationForSelect")]
        [YashilAuthorization("GetYashilHekmatTerminalSupplierInformationForSelect")]
        public async Task<LoadResult> GetSupplierInformationForSelectAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var isAdmin = await _userRoleService.IsAdminInApplicationAsync(_userPrincipal.Id, _userPrincipal.ApplicationId);

            return await DataSourceLoader.LoadAsync(_yashilHekmatTerminalSupplierService
                .GetForSelect(isAdmin)
                .ProjectTo<YashilHekmatTerminalSupplierSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


    }
}