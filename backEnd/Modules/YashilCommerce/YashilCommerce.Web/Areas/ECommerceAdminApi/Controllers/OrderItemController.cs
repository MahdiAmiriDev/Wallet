using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomDomainModels.Report;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilReport.Core.Services;
using YashilUserManagement.Core.Services;
using YashilCommerce.Core.Enums;
using YashilCommerce.Infrastructure.ServiceImpl;
using System.ComponentModel.Design;
using static Stimulsoft.Report.Func;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class OrderItemController : BaseController<OrderItem, int, OrderItemListViewModel, OrderItemEditModel, OrderItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IOrderItemService _orderItemService;
        private readonly IYashilLogger<OrderItem> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IAppConfigService _appConfigService;
        private readonly IReportStoreService _reportStoreService;
        private readonly IUserApplicationService _userApplicationService;
        private readonly IRepresentationService _representationService;


        public OrderItemController(IOrderItemService orderItemService,
            IMapper mapper, IYashilLogger<OrderItem> logger,
            IPermissionService permissionService,
            IUserPrincipal userPrincipal,
            IAdditionalUserPropService additionalUserPropService,
            IAppConfigService appConfigService,
            IReportStoreService reportStoreService, IUserApplicationService userApplicationService,
            IRepresentationService representationService) : base(orderItemService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _orderItemService = orderItemService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
            _appConfigService = appConfigService;
            _reportStoreService = reportStoreService;
            _userApplicationService = userApplicationService;
            _representationService = representationService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization(ActionName = "OrderItemByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? orderId, int? productSalesPlanningId, int? companyId, int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null, bool fillNotModified = false)
        {
            await CheckHasAccessForCompanyAndRepresentationAsync(companyId, representationId);

            var orderItems = _orderItemService.GetByCustomFilter(orderId, productSalesPlanningId, companyId, representationId, saleType, showCompletedOrderStatus, orderStatusType, orderCreationDateFromDate, orderCreationDateToDate, recipientNationalCode, mobileNumber, cardNumber, fillNotModified);

            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }



        [HttpGet("GetLimitedInformation")]
        [YashilAuthorization(ActionName = "LimitedOrderItemInformation")]
        public async Task<LoadResult> GetLimitedInformation(CustomDataSourceLoadOptions loadOptions, int? orderId, int? productSalesPlanningId, int? companyId, int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null, bool fillNotModified = false)
        {
            //دسترسی مرکز و نمایندگی داخل repositpry  چک شده است و نباید در این قسمت چک شود
            var orderItems = _orderItemService
                .GetByCustomFilter(orderId, productSalesPlanningId, companyId, representationId, saleType, showCompletedOrderStatus, orderStatusType, orderCreationDateFromDate, orderCreationDateToDate, recipientNationalCode, mobileNumber, cardNumber, fillNotModified);
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemListCustomViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد جهت دریافت لیست سفارش ها کاربرد دارد
        /// این متد مشابه متد
        /// GetLimitedInformation
        /// می باشد با این تفاوت که متد قبلی برای کامپوننت های
        /// Devextreme
        /// بهینه شده است و این متد برای تحویل داده ها به
        /// api
        /// بیرونی مناسب است
        /// </summary>
        /// <param name="orderStatusType">وضعیت سفارش </param>
        /// <param name="orderCreationDateFromDate">سفارش ها از</param>
        /// <param name="orderCreationDateToDate">سفارش ها تا تاریخ</param>
        /// <returns></returns>
        [HttpGet("GetOrderItemList")]
        [YashilAuthorization(ActionName = "LimitedOrderItemInformation")]
        public async Task<List<OrderItemListCustomViewModel>> GetGetOrderListAsync(int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
            //دسترسی مرکز و نمایندگی داخل repositpry  چک شده است و نباید در این قسمت چک شود
            return await _orderItemService
                .GetByCustomFilter(null, null, null, null, null,
                    null, orderStatusType, orderCreationDateFromDate, orderCreationDateToDate,
            null, null, null, false)
            .ProjectTo<OrderItemListCustomViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        /// برای نمایش همه ایتم های یک سفارش
        /// این متد حذف شده ها را نیز نشان میدهد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="orderId">کد آیتم سفارش</param>
        /// <returns></returns>
        [HttpGet("GetAllWithDeletedOrderItems")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAllWithDeletedOrderItems(CustomDataSourceLoadOptions loadOptions, int orderId)
        {

            var orderItems = _orderItemService.GetAllWithDeletedOrderItems(orderId);
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemWithDeletedListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// آیتم های سفارش حذف نشده کاربر
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetUserOrderItems")]
        public async Task<LoadResult> GetUserOrderItems(CustomDataSourceLoadOptions loadOptions)
        {
            var orderItems = _orderItemService.GetUserOrderItems();
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// لیست گزارش مربوط به یارانه مصرفی تمام پروژه های مبتی بر یارانه ساتا
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetSataSubsidyBasedOrderItemFullInfoView")]
        [YashilAuthorization(ActionName = "SataSubsidyBasedOrderItemFullInfo")]
        public async Task<LoadResult> GetSataSubsidyBasedOrderItemFullInfoView(CustomDataSourceLoadOptions loadOptions, int orderCreationDateFromDate, int orderCreationDateToDate)
        {
            var orderItems = _orderItemService.GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum.Sata, orderCreationDateFromDate, orderCreationDateToDate);
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemFullInfoViewViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// لیست گزارش مربوط به یارانه مصرفی تمام پروژه های مبتی بر یارانه وزارت
        /// </summary>
        /// <param name="loadOptions"></param>
        [HttpGet("GetVezaratSubsidyBasedOrderItemFullInfoView")]
        [YashilAuthorization(ActionName = "VezaratSubsidyBasedOrderItemFullInfo")]
        public async Task<LoadResult> GetVezaratSubsidyBasedOrderItemFullInfoView(CustomDataSourceLoadOptions loadOptions, int orderCreationDateFromDate, int orderCreationDateToDate)
        {
            var orderItems = _orderItemService.GetSubsidyBasedOrderItemFullInfoView(SubsidyTypesEnum.Vezarat, orderCreationDateFromDate, orderCreationDateToDate);
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemFullInfoViewViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        /// <summary>
        /// برای دریافت خروجی اکسل از فرم اطلاعات کامل سفارش مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <param name="representationId"></param>
        /// <param name="orderStatusType"></param>
        /// <param name="applicationId"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetOrderItemFullInfoExcelFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GetOrderItemFullInfoExcelFileAsync(int companyId,
            int orderCreationDateFromDate,
            int orderCreationDateToDate,
            int? representationId = null,
            int? orderStatusType = null,
            int? applicationId = null)
        {
            // دریافت اطلاعات مرکز و نمایندگی کاربر لاگین شده
            var currentUserAdditionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
            
            companyId = companyId == 0 && currentUserAdditionalUserProp.CompanyId.HasValue
                ? currentUserAdditionalUserProp.CompanyId.Value
                : companyId ;
            // کاربر باید حتما پارامتر مرکز را مقدار دهد
            if (companyId == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا companyId را ارسال کنید");
            }
            if (orderCreationDateFromDate == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا orderCreationDateFromDate را ارسال کنید");
            }
            if (orderCreationDateToDate == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا orderCreationDateToDate را ارسال کنید");
            }
            // اگر applicationId مقدار نداشته باشد آنگاه مقدار را از کاربر لاگین شده میخوانیم
            applicationId ??= _userPrincipal.ApplicationId;
            var hasCurrentUserAccessToApplication = _userApplicationService.CheckUserHasAccessToApplication(_userPrincipal.Id, applicationId.Value);
            if (!hasCurrentUserAccessToApplication)
            {
                throw new Exception($"کاربر گرامی شما به این برنامه دسترسی ندارید");
            }

            // اگر اطلاعات مرکز و نمایندگی کاربر لاگین شده وجود نداشته باشد خطایی رخ داده است
            if (currentUserAdditionalUserProp == null)
            {
                throw new Exception($"additional Prop for User {_userPrincipal.Id} not found");
            }
            if (currentUserAdditionalUserProp.CompanyId != null && currentUserAdditionalUserProp.CompanyId != companyId)
            {
                throw new Exception("کاربر گرامی شما به این مرکز دسترسی ندارید");
            }

            if (currentUserAdditionalUserProp.RepresentationId != null && currentUserAdditionalUserProp.RepresentationId != representationId)
            {
                throw new Exception("کاربر گرامی شما به این نمایندگی دسترسی ندارید");
            }
            var orderItemFullInfoReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceOrderItemFullInfoReportId);

            ReportStoreWithParam reportStoreWithParam = new ReportStoreWithParam()
            {
                Params = new Dictionary<string, string>
                {
                    { "companyId", companyId.ToString() },
                    { "representationId", representationId?.ToString()},
                    { "orderStatusType", orderStatusType?.ToString()},
                    { "applicationId", applicationId.ToString() },
                    { "orderCreationDateFromDate", orderCreationDateFromDate.ToString() },
                    { "orderCreationDateToDate", orderCreationDateToDate.ToString() }
                }
            };

            var reportFile = await _reportStoreService.GetReportFileAsync(orderItemFullInfoReportId, StiExportFormat.Excel2007, reportStoreWithParam);
            if (reportFile != null)
            {
                _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(OrderItem), "GetOrderItemFullInfoFile Record", null);

                return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"orderItemFullInfo.xlsx");
            }

            throw new Exception("کاربر گرامی خطایی رخ داده است لطفا با مدیر سایت تماس بگیرید");
        }

        [HttpGet("GetOrderItemFullInfo")]
        [YashilAuthorization(ActionName = "OrderItemFullInfo")]
        public async Task<LoadResult> GetOrderItemFullInfo(CustomDataSourceLoadOptions loadOptions, int? orderId, int? productSalesPlanningId, int? companyId, int? representationId, int? saleType, bool? showCompletedOrderStatus, int? orderStatusType, int? orderCreationDateFromDate, int? orderCreationDateToDate, string recipientNationalCode = null, string mobileNumber = null, string cardNumber = null, bool fillNotModified = false)
        {

            var orderItems = _orderItemService.GetByCustomFilter(orderId, productSalesPlanningId, companyId, representationId, saleType, showCompletedOrderStatus, orderStatusType, orderCreationDateFromDate, orderCreationDateToDate, recipientNationalCode, mobileNumber, cardNumber, fillNotModified);
            return await DataSourceLoader.LoadAsync(orderItems.ProjectTo<OrderItemFullInfoListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// عملکرد فروش تامین کنندگان 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <param name="representationId"></param>
        /// <param name="orderStatusType"></param>
        /// <param name="applicationId"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetSupplierSalesPerformanceExcelFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GetSupplierSalesPerformanceExcelFileAsync(int companyId, int orderCreationDateFromDate, int orderCreationDateToDate, int? representationId = null, int? orderStatusType = null, int? applicationId = null)
        {
            // کاربر باید حتما پارامتر مرکز را مقدار دهد
            if (companyId == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا companyId را ارسال کنید");
            }
            if (orderCreationDateFromDate == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا orderCreationDateFromDate را ارسال کنید");
            }
            if (orderCreationDateToDate == 0)
            {
                throw new Exception($"کاربر گرامی پارامترهای ارسالی مجاز نیستند، لطفا orderCreationDateToDate را ارسال کنید");
            }
            // اگر applicationId مقدار نداشته باشد آنگاه مقدار را از کاربر لاگین شده میخوانیم
            applicationId ??= _userPrincipal.ApplicationId;
            var hasCurrentUserAccessToApplication = _userApplicationService.CheckUserHasAccessToApplication(_userPrincipal.Id, applicationId.Value);
            if (!hasCurrentUserAccessToApplication)
            {
                throw new Exception($"کاربر گرامی شما به این برنامه دسترسی ندارید");
            }
            // دریافت اطلاعات مرکز و نمایندگی کاربر لاگین شده
            var currentUserAdditionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
            // اگر اطلاعات مرکز و نمایندگی کاربر لاگین شده وجود نداشته باشد خطایی رخ داده است
            if (currentUserAdditionalUserProp == null)
            {
                throw new Exception($"additional Prop for User {_userPrincipal.Id} not found");
            }
            if (currentUserAdditionalUserProp.CompanyId != null && currentUserAdditionalUserProp.CompanyId != companyId)
            {
                throw new Exception("کاربر گرامی شما به این مرکز دسترسی ندارید");
            }

            if (currentUserAdditionalUserProp.RepresentationId != null && currentUserAdditionalUserProp.RepresentationId != representationId)
            {
                throw new Exception("کاربر گرامی شما به این نمایندگی دسترسی ندارید");
            }
            var orderItemFullInfoReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCompanySupplierSalesPerformanceReportId);

            ReportStoreWithParam reportStoreWithParam = new ReportStoreWithParam()
            {
                Params = new Dictionary<string, string>
                {
                    { "companyId", companyId.ToString() },
                    { "representationId", representationId?.ToString()},
                    { "orderStatusType", orderStatusType?.ToString()},
                    { "applicationId", applicationId.ToString() },
                    { "orderCreationDateFromDate", orderCreationDateFromDate.ToString() },
                    { "orderCreationDateToDate", orderCreationDateToDate.ToString() }
                }
            };

            var reportFile = await _reportStoreService.GetReportFileAsync(orderItemFullInfoReportId, StiExportFormat.Excel2007, reportStoreWithParam);
            if (reportFile != null)
            {
                _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(OrderItem), "GetOrderItemFullInfoFile Record", null);

                return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"SupplierSalesPerformance.xlsx");
            }

            throw new Exception("کاربر گرامی خطایی رخ داده است لطفا با مدیر سایت تماس بگیرید");
        }
        async Task CheckHasAccessForCompanyAndRepresentationAsync(int? companyId, int? representationId)
        {
            bool userHasAccessForCompanyAndRepresentation = await _representationService.CheckUserAccessForCompanyAndRepresentationAsync(companyId, representationId, _userPrincipal.Id);

            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "Get"),
                    nameof(Factor), "Get Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(OrderItem)}");
            }
        }

        /// <summary>
        /// دریافت لیست سفارشاتی که پس از پرداخت وجه عودت شده و دارای یارانه است
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="subsidyType"></param>
        /// <param name="orderCreationDateFromDate"></param>
        /// <param name="orderCreationDateToDate"></param>
        /// <returns></returns>
        [HttpGet("GetAllRefundedOrderItemsByDate")]
        [YashilAuthorization(ActionName = "GetAllRefundedOrderItemsByDate")]
        public async Task<LoadResult> GetAllRefundedOrderItemsByDateAsync(CustomDataSourceLoadOptions loadOptions, int? factorId, int? subsidyType, int? orderCreationDateFromDate, int? orderCreationDateToDate)
        {
           var orderItems = _orderItemService.GetAllRefundedOrderItems(factorId,subsidyType, orderCreationDateFromDate, orderCreationDateToDate);
            return await DataSourceLoader.LoadAsync(
                orderItems.ProjectTo<OrderItemRefundedViewModel>(
                _mapper.ConfigurationProvider), loadOptions);
        }

    }
}