using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class FactorPaymentController : BaseController<FactorPayment, int, FactorPaymentListViewModel,
        FactorPaymentEditModel, FactorPaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorPaymentService _factorPaymentService;
        private readonly IYashilLogger<FactorPayment> _logger;
        private readonly IFactorService _factorService;

        public FactorPaymentController(IFactorPaymentService factorPaymentService, IMapper mapper,
            IYashilLogger<FactorPayment> logger, IPermissionService permissionService, IFactorService factorService) : base(factorPaymentService,
            mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorPaymentService = factorPaymentService;
            _logger = logger;
            _factorService = factorService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int factorId)
        {
            await CheckUserAccessForFactorId(factorId);

            var factorPayments = _factorPaymentService.GetByCustomFilter(factorId);
            return await DataSourceLoader.LoadAsync(
                factorPayments.ProjectTo<FactorPaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int factorId)
        {
            await CheckUserAccessForFactorId(factorId);

            var factorPayments = _factorPaymentService.GetByCustomFilter(factorId);
            return await DataSourceLoader.LoadAsync(
                factorPayments.ProjectTo<FactorPaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// برای دریافت لیت پرداختی های یک فاکتور
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorPaymentsList")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetFactorPaymentsList(CustomDataSourceLoadOptions loadOptions, int factorId)
        {

            await CheckUserAccessForFactorId(factorId);

            var factorPayments = _factorPaymentService.GetFactorPayments(factorId);
            return await DataSourceLoader.LoadAsync(
            factorPayments.ProjectTo<FactorPaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(FactorPaymentEditModel editModel)
        {
            // اگر مبلغ وارد شده کاربر صفر باشد باید خطا دهد
            if (editModel.Price == 0)
            {
                return "کاربر گرامی مبلغ وارد شده باید بیش از صفر باشد";
            }
            if (editModel.InstallmentCount == 0)
            {
                return "کاربر گرامی تعداد اقساط نمیتواند صفر باشد";
            }
            var factorPaymentStatistic = _factorService.GetFactorStatistic(editModel.FactorId);
            // اگر تعداد اقساط وارد شده بیش از تعداد اقساط باقیمانده باشد خطا دهد
            if (factorPaymentStatistic.RemainingFactorInstallmentCount < editModel.InstallmentCount)
            {
                return "کاربر گرامی تعداد اقساط وارد شده بیش از تعداد اقساط باقیمانده است";
            }
            // اگر مبلغ وارد شده بیش از مبلغ باقیمانده باشد باید خطا دهد
            if (factorPaymentStatistic.SumRemaining >= editModel.Price)
            {
                return await base.CustomValidateBeforeInsertAsync(editModel);
            }
            return "کاربر گرامی مبلغ وارد شده بیش از مبلغ باقیمانده است";
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(FactorPaymentEditModel editModel)
        {
            if (editModel.Price == 0)
            {
                return "کاربر گرامی مبلغ وارد شده باید بیش از صفر باشد";
            }
            if (editModel.InstallmentCount == 0)
            {
                return "کاربر گرامی تعداد اقساط نمی تواند صفر باشد";
            }
            // هنگام بروزرسانی نباید مقادیر از قبل وارد شده ایتم در حال ویرایش را در نظر گرفت
            // پس هنگامی که اطلاعات در حال چک شدن است
            // برای آیتم در حال ویرایش این موارد را در نظر نمیگیریم و مبالغ کم شده را زیاد میکنیم و تعداد اقساط کم شده ان را نیز اضافه میکنیم
            var factorPaymentStatistic = _factorService.GetFactorStatistic(editModel.FactorId);
            var factorPayment = _factorPaymentService.Get(editModel.Id, true);
            if (factorPaymentStatistic.RemainingFactorInstallmentCount + factorPayment.InstallmentCount < editModel.InstallmentCount)
            {
                return "کاربر گرامی تعداد اقساط نمی تواند بیش از تعداد اقساط باقیماند باشد";
            }
            if (factorPaymentStatistic.SumRemaining + factorPayment.Price >= editModel.Price)
            {
                return await base.CustomValidateBeforeUpdateAsync(editModel);
            }
            return "کاربر گرامی مبلغ وارد شده بیش از مبلغ باقیمانده است";
        }


        /// <summary>
        /// جهت دریافت مبلغ اقساط
        /// این متد در پرداختی فاکتور مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="factorPaymentId"></param>
        /// <param name="installmentCount"></param>
        /// <returns></returns>
        [HttpGet("GetFactorPaymentInstallmentPrice")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<decimal?> GetFactorPaymentInstallmentPrice(int factorId, int? factorPaymentId, int installmentCount)
        {

            await CheckUserAccessForFactorId(factorId);

            if (installmentCount == 0)
            {
                return 0;
            }
            // دریافت اطلاعات مربوط به پرداخت فاکتور
            var factorPaymentStatistic = _factorService.GetFactorStatistic(factorId);
            // درصورتی که پرداختی فاکتور در حالت ویرایش باشد نباید مبلغ و تعداد قسط آن را در نظر بگیریم
            if (factorPaymentId != null)
            {
                var factorPayment = _factorPaymentService.Get(factorPaymentId, true);
                if (factorPayment == null)
                {
                    return null;
                }
                factorPaymentStatistic.SumPaid -= factorPayment.Price;
                factorPaymentStatistic.PaidFactorInstallmentCount -= factorPayment.InstallmentCount;
            }
            // در صورتی که تعداد اقساط وارد شده بیش از تعداد اقساط باقیمانده باشد 0 برمیگرداند
            if (installmentCount > factorPaymentStatistic.RemainingFactorInstallmentCount)
            {
                return 0;
            }
            /*
             * مبلغ قسط بدین صورت خواهد بود که مبلغ باقیمانده تقسیم بر تعداد اقساط باقیمانده ضربدر تعداد اقساط وارد شده توسط کاربر
             */
            var result = (factorPaymentStatistic.SumRemaining / factorPaymentStatistic.RemainingFactorInstallmentCount) *
                installmentCount;
            return result;
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