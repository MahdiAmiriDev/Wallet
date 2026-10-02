using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PaymentController : BaseController<Payment, int, PaymentListViewModel, PaymentEditModel, PaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IYashilLogger<Payment> _logger;

        public PaymentController(IPaymentService paymentService, IMapper mapper, IYashilLogger<Payment> logger, IPermissionService permissionService, IOrderService orderService) : base(paymentService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _logger = logger;
            _orderService = orderService;
            _paymentService = paymentService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? cardType, int? saleType, int? paymentStatusType, int? paymentMethodType, int orderId)
        {
            var userHasAccessForOrder = await _orderService.CheckUserHasAccessForOrder(orderId);
            if (!userHasAccessForOrder)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Payment)}");
            }
            var payments = _paymentService.GetByCustomFilter(cardType, saleType, paymentStatusType, paymentMethodType, orderId);
            return await DataSourceLoader.LoadAsync(payments.ProjectTo<PaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// این متد جهت دریافت جزئیات پرداخت یک سفارش کاربرد دارد
        /// این متد مشابه
        /// api
        /// GetByCustomFilterForList
        /// می باشد با یان تفاوت که قبلی با کامپوننت های
        /// Devxtreeme
        /// سازگار بوده و این متد برای
        /// api
        /// های که به خارج از زیرساخت ارائه می دهیم کارد دارد
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetPaymentDetail")]
        [YashilAuthorization("GetByCustomFilterForList")]
        public async Task<PaymentListViewModel> GetPaymentDetailAsync(int orderId)
        {
            var userHasAccessForOrder = await _orderService.CheckUserHasAccessForOrder(orderId);

            if (!userHasAccessForOrder)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Payment)}");
            }

            return await _paymentService.GetByCustomFilter(null, null, null, null, orderId)
                .ProjectTo<PaymentListViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
    }
}