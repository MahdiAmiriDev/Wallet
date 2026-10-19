using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Extensions;
using YashilPayment.Core.Classes;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class MerchantInfoController : BaseController<MerchantInfo, int, MerchantInfoListViewModel,
        MerchantInfoEditModel, MerchantInfoSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IMerchantInfoService _merchantInfoService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppConfigService _appConfigService;

        public MerchantInfoController(IMerchantInfoService merchantInfoService, IMapper mapper,
            IYashilLogger<MerchantInfo> logger, IPermissionService permissionService, IUserPrincipal principal,
            IAppConfigService appConfigService) : base(
            merchantInfoService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _merchantInfoService = merchantInfoService;
            _userPrincipal = principal;
            _appConfigService = appConfigService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? cityId,
            int? userId)
        {
            var merchantInfos = _merchantInfoService.GetByCustomFilter(cityId, userId);
            return await DataSourceLoader.LoadAsync(
                merchantInfos.ProjectTo<MerchantInfoListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? cityId,
            int? userId)
        {
            var merchantInfos = _merchantInfoService.GetByCustomFilter(cityId, userId);
            return await DataSourceLoader.LoadAsync(
                merchantInfos.ProjectTo<MerchantInfoSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetUserMerchants")]
        public async Task<LoadResult> GetUserMerchants(CustomDataSourceLoadOptions loadOptions)
        {
            var merchants = _merchantInfoService.GetByCustomFilter(null, _userPrincipal.Id);
            if (!merchants.Any())
            {
                merchants = _merchantInfoService.GetAll(true);
            }

            return await DataSourceLoader.LoadAsync(
                merchants.ProjectTo<MerchantInfoSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetCurrentUserMerchantId")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public MerchantInfoSimpleViewModel? GetCurrentUserMerchantId()
        {

            var merchants = _merchantInfoService.GetByCustomFilter(null, _userPrincipal.Id);
            return merchants.ProjectTo<MerchantInfoSimpleViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
        }


        [HttpGet("GetMerchantRoleId")]
        public async Task<IActionResult> GetMerchantRoleId()
        {
            var roleId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilPaymentConfigurationMerchantRole);
            return Accepted(new ResultStatus<int>()
            {
                EntityId = roleId,
                IsSuccess = true
            });
        }

        /// <summary>
        /// بررسی فرمت تلفن ثابت و تلفن همراه قبل از درج پذیرنده
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(MerchantInfoEditModel editModel)
        {
            if (!string.IsNullOrEmpty(editModel.PhoneNumber) && !editModel.PhoneNumber.IsValidPhoneNumber())
            {
                return "کاربر گرامی، لطفا فرمت تلفن ثابت را صحیح وارد نمایید.";
            }

            if (!string.IsNullOrEmpty(editModel.MobileNumber) && !editModel.MobileNumber.IsValidMobileNumber())
            {
                return "کاربر گرامی، لطفا فرمت تلفن همراه را صحیح وارد نمایید.";
            }

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        /// <summary>
        /// بررسی فرمت تلفن ثابت و تلفن همراه قبل از ویرایش پذیرنده
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeUpdateAsync(MerchantInfoEditModel editModel)
        {
            if (!string.IsNullOrEmpty(editModel.PhoneNumber) && !editModel.PhoneNumber.IsValidPhoneNumber())
            {
                return "کاربر گرامی، لطفا فرمت تلفن ثابت را صحیح وارد نمایید.";
            }

            if (!string.IsNullOrEmpty(editModel.MobileNumber) && !editModel.MobileNumber.IsValidMobileNumber())
            {
                return "کاربر گرامی، لطفا فرمت تلفن همراه را صحیح وارد نمایید.";
            }

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
    }
}