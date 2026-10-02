using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels.BaseViewModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class FactorPaymentInstallmentController : BaseController<FactorPaymentInstallment, int, FactorPaymentInstallmentListViewModel, FactorPaymentInstallmentEditModel, FactorPaymentInstallmentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorPaymentInstallmentService _factorPaymentInstallmentService;
        private readonly IYashilLogger<FactorPaymentInstallment> _logger;
        private readonly IUserPrincipal _userPrincipal;


        public FactorPaymentInstallmentController(IFactorPaymentInstallmentService factorPaymentInstallmentService,
        IMapper mapper,
        IYashilLogger<FactorPaymentInstallment> logger,
        IPermissionService permissionService,
        IUserPrincipal userPrincipal) : base(factorPaymentInstallmentService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorPaymentInstallmentService = factorPaymentInstallmentService;
            _logger = logger;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? factorGroupItemId, int? installmentPaymentStatusType)
        {
            var factorPaymentInstallments = _factorPaymentInstallmentService.GetByCustomFilter(factorGroupItemId, installmentPaymentStatusType);
            return await DataSourceLoader.LoadAsync(factorPaymentInstallments.ProjectTo<FactorPaymentInstallmentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? factorGroupItemId, int? installmentPaymentStatusType)
        {
            var factorPaymentInstallments = _factorPaymentInstallmentService.GetByCustomFilter(factorGroupItemId, installmentPaymentStatusType);
            return await DataSourceLoader.LoadAsync(factorPaymentInstallments.ProjectTo<FactorPaymentInstallmentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// دریافت لیست تاریخ اقساط فاکتور 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetInstallmentMonthYearList")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetInstallmentMonthYearList(CustomDataSourceLoadOptions loadOptions)
        {
            var factorGroupedMonthYear = _factorPaymentInstallmentService
                .GetInstallmentMonthYearList();
            return await DataSourceLoader
                .LoadAsync(factorGroupedMonthYear.ProjectTo<PersianDateMonthViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// دریافت اکسل اقساط فاکتور بر اساس سال ماه 
        /// </summary>
        /// <param name="factorPaymentInstallmentExportToExcelParamsDto">پارامتر هایی که برای اقساط پرداختی فاکتور از سمت کلاینت به بک اند ارسال شده است</param>
        /// <returns></returns>
        [HttpPost("ExportToExcelFactorPaymentInstallment")]
        [YashilAuthorization("ExportToExcelFactorPaymentInstallment")]
        public async Task<FileResult> ExportToExcelFactorPaymentInstallmentAsync(
            [FromBody] FactorPaymentInstallmentExportToExcelParamsDto factorPaymentInstallmentExportToExcelParamsDto)
        {
            if (factorPaymentInstallmentExportToExcelParamsDto.Ids.Count <= 0
                || factorPaymentInstallmentExportToExcelParamsDto.YearMonth == 0)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(FactorPaymentInstallment),
                    $"factorPaymentInstallmentExportToExcelParamsDto.Ids.Count  = {factorPaymentInstallmentExportToExcelParamsDto.Ids.Count} " +
                    $"-  factorPaymentInstallmentExportToExcelParamsDto.YearMonth =  {factorPaymentInstallmentExportToExcelParamsDto.YearMonth}",
                    new Exception());

                return null;
            }
            try
            {
                var file = await _factorPaymentInstallmentService.ExportFactorPaymentInstallmentAsync(
                    factorPaymentInstallmentExportToExcelParamsDto.YearMonth,
                    factorPaymentInstallmentExportToExcelParamsDto.Ids
                    );
                return File(file, MimeTypes.GetMimeType(" FactorPaymentInstallment.xlsx"), "FactorPaymentInstallment.xlsx");

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException,
                    nameof(FactorPaymentInstallment),
                    "بروز خطا به هنگام دریافت اکسل محصولات", e);

                return null;
            }
        }

        /// <summary>
        /// دریافت اقساط پرداختی فاکتور
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="yearMonth">سال ماه</param>
        /// <param name="categoryGroupId">شناسه دسته بندی گروه</param>
        /// <param name="installmentPaymentStatusType">شناسه دسته بندی گروه</param>
        /// <returns></returns>
        [HttpGet("GetFactorPaymentInstallment")]
        public async Task<LoadResult> GetFactorPaymentInstallmentAsync(
            CustomDataSourceLoadOptions loadOptions,
            int? yearMonth,
            int? categoryGroupId,
            int? installmentPaymentStatusType)
        {
            var factorPaymentInstallment = _factorPaymentInstallmentService.GetFactorPaymentInstallment(
                yearMonth,
                categoryGroupId,
                installmentPaymentStatusType);
            return await DataSourceLoader.LoadAsync(
                factorPaymentInstallment.ProjectTo<FactorPaymentInstallmentSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
        /// <summary>
        /// ثبت گروهی شناسه مرجع اقساط پرداختی
        /// </summary>
        /// <param name="registerPaymentInstallmentReference">اطلاعات مرجع پرداخت</param>
        /// <returns></returns>
        [HttpPost("RegisterPaymentReference")]
        public async Task<IActionResult> RegisterPaymentReferenceAsync(RegisterPaymentInstallmentReferencParamsDto registerPaymentInstallmentReference)
        {
            try
            {
                var result = await _factorPaymentInstallmentService.BulkInsertPaymentReferencesAsync(registerPaymentInstallmentReference);

                if (result == 0)
                {
                    return Accepted(new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "مشکلی در ثبت شناسه مرجع پرداخت پیش آمد. لطفا مجدد سعی کنید"
                    });
                }
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "شناسه مرجع پرداخت با موفقیت ثبت شد"
                });
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException,
                  nameof(FactorPaymentInstallment),
                  "بروز خطا به هنگام ثبت شناسه مرجع پرداخت", e);

                return null;
            }
        }
    }
}