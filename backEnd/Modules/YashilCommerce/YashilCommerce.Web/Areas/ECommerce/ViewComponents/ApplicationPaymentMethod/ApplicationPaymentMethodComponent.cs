using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Core;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.ApplicationPaymentMethod
{
    /// <summary>
    /// کامپوننت لیست درگاه های بانکی
    /// </summary>
    public class ApplicationPaymentMethodComponent : YashilViewComponent
    {
        private readonly IApplicationPaymentMethodService _applicationPaymentMethodService;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ApplicationPaymentMethodComponent(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
            IApplicationPaymentMethodService applicationPaymentMethodService) : base(applicationConfiguration, mapper)
        {
            _applicationPaymentMethodService = applicationPaymentMethodService;
            _applicationConfiguration = applicationConfiguration;
        }
        /// <summary>
        /// لیست درگاه های بانکی را به صورت رادیو باتن نمایش می دهد 
        /// </summary>
        /// <param name="limitedToBankInternetPaymentGateway">محدود به درگاه پرداخت اینترنتی برای اعضای باشگاه مشتریانی که در لوکال نیستند</param>
        /// <param name="limitToHekmat">محدود به پرداخت از طریق حکمت</param>
        /// <param name="saleType">نوع فروش</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(
            bool limitToHekmat,
            bool limitedToBankInternetPaymentGateway,
            int? saleType
            )
        {
            var applicationPaymentMethods =
                await _applicationPaymentMethodService
                    .GetApplicationPaymentMethodsAsync(_applicationConfiguration.ApplicationId, true);
            if (applicationPaymentMethods != null)
            {
                List<PaymentMethodType> applicationPaymentMethodList;
                if (limitToHekmat)
                {
                    applicationPaymentMethodList = await applicationPaymentMethods.Where(x => x.Id == (int)PaymentMethodTypesEnum.Hekmat).ToListAsync();
                }
                else if (limitedToBankInternetPaymentGateway)
                {
                    applicationPaymentMethodList = await applicationPaymentMethods.Where(x => x.Id != (int)PaymentMethodTypesEnum.Hekmat).ToListAsync();
                }
                else
                {
                    applicationPaymentMethodList = await applicationPaymentMethods.ToListAsync();
                }

                if (!saleType.HasValue)
                {
                    applicationPaymentMethodList = new List<PaymentMethodType>();
                }

                else
                {
                    if (saleType == (int)SaleTypesEnum.CashPayment)
                    {
                        applicationPaymentMethodList =
                            await applicationPaymentMethods.Where(x => x.Id != (int)PaymentMethodTypesEnum.Hekmat)
                                .ToListAsync();
                    }
                    else if (saleType == (int)SaleTypesEnum.InstallmentPayment)
                    {
                        applicationPaymentMethodList =
                            await applicationPaymentMethods.Where(x => x.Id == (int)PaymentMethodTypesEnum.Hekmat)
                                .ToListAsync();
                    }
                }

                return View("_ApplicationPaymentMethod", applicationPaymentMethodList);
            }
            return View("_ApplicationPaymentMethod", new List<PaymentMethodType>());
        }
    }
}
