using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
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
    public class PreRegistrationController : BaseController<PreRegistration, int, PreRegistrationListViewModel,
        PreRegistrationEditModel, PreRegistrationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPreRegistrationService _preRegistrationService;
        private readonly IYashilLogger<PreRegistration> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserPrincipal _userPrincipal;

        public PreRegistrationController(IPreRegistrationService preRegistrationService,
            IMapper mapper,
            IYashilLogger<PreRegistration> logger,
            IPermissionService permissionService, IUserPrincipal userPrincipal) : base(preRegistrationService, mapper,
            logger, permissionService)
        {
            _mapper = mapper;
            _preRegistrationService = preRegistrationService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? userId, int? productSalesPlanningId)
        {
            var preRegistrations = _preRegistrationService.GetByCustomFilter(userId, productSalesPlanningId);
            return await DataSourceLoader.LoadAsync(
                preRegistrations.ProjectTo<PreRegistrationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(PreRegistrationEditModel editModel)
        {
            var isUserPreRegisteredBefore =
                await _preRegistrationService.CheckUserIsPreRegisteredAsync(editModel.ProductSalesPlanningId,
                    _userPrincipal.Id);
            if (isUserPreRegisteredBefore)
            {
                return "کاربر گرامی نام شما قبلا در پیش ثبت نام درج شده است";
            }

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        /// <summary>
        /// هنگامی که در پنل عمومی کاربر دکمه پیش ثبت نام را بزند این متد فراخوانی میشود
        /// 
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        [HttpPost("PublicUserPreRegister/{productSalesPlanningId}/{nationalCode}")]
        [YashilAuthorization("PostEntity", nameof(PreRegistration))]
        public async Task<ResultStatus<int>> PublicUserPreRegister(int? productSalesPlanningId , int? nationalCode)
        {
            if (!productSalesPlanningId.HasValue || productSalesPlanningId == 0 || !nationalCode.HasValue)
            {
                return new ResultStatus<int>(false, "کاربر گرامی اطلاعات وارد شده معتبر نمی باشد");
            }

            var resultMsg =
                await _preRegistrationService.PublicUserPreRegisterAsync(productSalesPlanningId.Value,
                    _userPrincipal.Id,nationalCode.Value);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return new ResultStatus<int>(false, resultMsg);
            }

            return new ResultStatus<int>(true, String.Empty);
        }
    }
}