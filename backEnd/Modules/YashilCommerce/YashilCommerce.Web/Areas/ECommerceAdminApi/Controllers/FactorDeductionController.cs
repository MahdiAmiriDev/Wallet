using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
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
    public class FactorDeductionController : BaseController<FactorDeduction, int, FactorDeductionListViewModel, FactorDeductionEditModel, FactorDeductionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorDeductionService _factorDeductionService;
        private readonly IFactorService _factorService;

        public FactorDeductionController(IFactorDeductionService factorDeductionService, IMapper mapper, IYashilLogger<FactorDeduction> logger, IPermissionService permissionService, IFactorPaymentService factorPaymentService, IFactorService factorService) : base(factorDeductionService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorService = factorService;
            _factorDeductionService = factorDeductionService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? factorId)
        {
            var factorDeductions = _factorDeductionService.GetByCustomFilter(factorId);
            return await DataSourceLoader.LoadAsync(factorDeductions.ProjectTo<FactorDeductionListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? factorId)
        {
            var factorDeductions = _factorDeductionService.GetByCustomFilter(factorId);
            return await DataSourceLoader.LoadAsync(factorDeductions.ProjectTo<FactorDeductionSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(FactorDeductionEditModel editModel)
        {
            var factorPaymentStatistic = _factorService.GetFactorStatistic(editModel.FactorId);
            if (factorPaymentStatistic.SumRemaining < editModel.Price)
            {
                return "کاربر گرامی مبلغ وارد شده نمی تواند بیش از مبلغ باقیمانده فاکتور باشد";
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(FactorDeductionEditModel editModel)
        {
            var factorPaymentStatistic = _factorService.GetFactorStatistic(editModel.FactorId);
            var factorDeduction = _factorDeductionService.Get(editModel.Id, true);
            // در هنگام بروزرسانی نباید مبلغ کسورات آیتم در حال ویرایش را در نظر گرفت لذا باید مبلغ آن به مبلغ باقیمانده اضافه گردد
            if (factorPaymentStatistic.SumRemaining + factorDeduction.Price < editModel.Price)
            {
                return "کاربر گرامی مبلغ وارد شده نمی تواند بیش از مبلغ باقیمانده فاکتور باشد";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
    }
}