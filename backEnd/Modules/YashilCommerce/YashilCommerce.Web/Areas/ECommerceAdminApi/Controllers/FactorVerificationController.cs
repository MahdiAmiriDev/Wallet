using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class FactorVerificationController : BaseController<FactorVerification, int, FactorVerificationListViewModel,
        FactorVerificationEditModel, FactorVerificationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorVerificationService _factorVerificationService;
        private readonly IYashilLogger<FactorVerification> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IFactorService _factorService;

        public FactorVerificationController(IFactorVerificationService factorVerificationService, IMapper mapper,
            IYashilLogger<FactorVerification> logger, IPermissionService permissionService,
            IFactorService factorService) : base(factorVerificationService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorVerificationService = factorVerificationService;
            _logger = logger;
            _permissionService = permissionService;
            _factorService = factorService;
        }

        [HttpGet("GetForConfirmList")]
        [YashilAuthorization(ActionName = "GetForConfirmList")]
        public async Task<LoadResult> GetForConfirmList(CustomDataSourceLoadOptions loadOptions)
        {
            var factorVerifications = _factorVerificationService.GetForConfirm();
            return await DataSourceLoader.LoadAsync(
                factorVerifications.ProjectTo<FactorVerificationListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }


        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? factorId,
            int? factorConfirmationType, int? factorVerificationConfigurationRoleId)
        {
            var factorVerifications = _factorVerificationService.GetByCustomFilter(factorId, factorConfirmationType,
                factorVerificationConfigurationRoleId);
            return await DataSourceLoader.LoadAsync(
                factorVerifications.ProjectTo<FactorVerificationListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// دریافت تمام بررسی های فاکتور مربوط به فاکتور داده شده
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorVerificationsForFactor")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetFactorVerificationsForFactorByCustomFilterForList(
            CustomDataSourceLoadOptions loadOptions, int factorId)
        {

            await CheckUserAccessForFactorId(factorId);

            var factorVerifications =
                _factorVerificationService.GetFactorVerificationsForFactorByCustomFilterForList(factorId);
            return await DataSourceLoader.LoadAsync(
                factorVerifications.ProjectTo<FactorVerificationListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// دریافت لیسلیست همه توضیحات مربوط به بررسی فاکتور
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorVerificationDescriptionForFactor")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetFactorVerificationDescriptionForFactor(CustomDataSourceLoadOptions loadOptions,
            int factorId)
        {
            // کسی مجاز به دیدن دلایل رد فاکتور است که اجازه نمایش فاکتور ها را داشته باشد
            // و کسی مجاز به نمایش فاکتور هاست که مجوز GETeNTITIES را داشته باشد
            // همچنین صادر کنندگانن فاکتور نیز میتوانند تایید کنندگان فاکتور را مشاهده کنند

            await CheckUserAccessForFactorId(factorId);

            var factorVerifications = _factorVerificationService.GetFactorVerificationDescriptionForFactor(factorId);
            return await DataSourceLoader.LoadAsync(
                factorVerifications
                    .ProjectTo<FactorVerificationDescriptionListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// تایید بررسی فاکتور
        /// </summary>
        /// <param name="factorVerificationSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("ConfirmFactorVerification")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<IActionResult> ConfirmFactorVerification(
            FactorVerificationSimpleViewModel factorVerificationSimpleViewModel)
        {

            var factorVerification = await _factorVerificationService.GetAsync(factorVerificationSimpleViewModel.Id, false);
            if (factorVerification != null)
            {
                var result = await _factorService.ConfirmFactorVerification(factorVerificationSimpleViewModel.Id, true);
                if (result != null)
                {
                    return Accepted(new ResultStatus<int>()
                    {
                        IsSuccess = true,
                        Message = "تایید فاکتور با موفقیت انجام شد "
                    });
                }

                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی شما نمیتوانید فاکتور را تایید کنید"
                });
            }

            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = "خطا در تایید فاکتور"
            });
        }

        /// <summary>
        /// رد بررسی فاکتور
        /// </summary>
        /// <param name="factorVerificationSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("RejectFactorVerification")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<IActionResult> RejectFactorVerification(
            CustomFactorVerificationSimpleViewModel factorVerificationSimpleViewModel)
        {

            if (String.IsNullOrEmpty(factorVerificationSimpleViewModel.Description))
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی لطفا توضیحات را وارد کنید"
                });
            }

            var factorVerification = await _factorVerificationService.GetAsync(factorVerificationSimpleViewModel.Id, false);
            if (factorVerification != null)
            {
                factorVerification.Description = factorVerificationSimpleViewModel.Description;
                var result = await _factorService.RejectFactorVerification(factorVerification, true);
                if (result != null)
                {
                    return Accepted(new ResultStatus<int>()
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی شما نمی توانید فاکتور را رد کنید "
                    });
                }

                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = true,
                    Message = "رد فاکتور با موفقیت انجام شد "
                });
            }

            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = "خطا در رد فاکتور"
            });
        }

        /// <summary>
        /// بررسی اینکه برای بررسی فاکتور داده شده، بررسی فاکتور بعدی وجود دارد
        /// یعنی بررسی فاکتور دیگری وجود دارد که بعد از بررسی فاکتور داده شده باید فاکتور را بررسی کند یا خیر
        /// </summary>
        /// <param name="factorVerificationSimpleViewModel"></param>
        /// <returns>false در صورت وجود نداشتن</returns>
        [HttpPost("CheckExistenceSubsequentRecord")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public bool? CheckExistenceSubsequentRecord(
            FactorVerificationSimpleViewModel factorVerificationSimpleViewModel)
        {

            var factorVerification = _factorVerificationService.Get(factorVerificationSimpleViewModel.Id, true);
            if (factorVerification != null)
            {
                return _factorVerificationService.CheckExistenceSubsequentRecord(
                    factorVerification.FactorVerificationConfigurationRole.VerificationOrder,
                    factorVerification.FactorId);
            }

            return null;
        }

        private async Task CheckUserAccessForFactorId(int factorId)
        {
            bool userHasAccessForFactor = await _factorService.CheckUserAccessForFactorId(factorId);
            if (!userHasAccessForFactor)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }
        }


    }
}