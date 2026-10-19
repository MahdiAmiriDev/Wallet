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
using Yashil.Infrastructure.Extensions;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class YashilHekmatTerminalController : BaseController<YashilHekmatTerminal, int, YashilHekmatTerminalListViewModel, YashilHekmatTerminalEditModel, YashilHekmatTerminalSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IYashilHekmatTerminalService _yashilHekmatTerminalService;

        public YashilHekmatTerminalController(IYashilHekmatTerminalService yashilHekmatTerminalService,
        IMapper mapper,
        IYashilLogger<YashilHekmatTerminal> logger,
        IPermissionService permissionService) : base(yashilHekmatTerminalService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _yashilHekmatTerminalService = yashilHekmatTerminalService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? hekmatTerminalId, int? merchantId)
        {
            var yashilHekmatTerminals = _yashilHekmatTerminalService.GetByCustomFilter(hekmatTerminalId, merchantId);
            return await DataSourceLoader.LoadAsync(yashilHekmatTerminals.ProjectTo<YashilHekmatTerminalListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? hekmatTerminalId, int? merchantId)
        {
            var yashilHekmatTerminals = _yashilHekmatTerminalService.GetByCustomFilter(hekmatTerminalId, merchantId);
            return await DataSourceLoader.LoadAsync(yashilHekmatTerminals.ProjectTo<YashilHekmatTerminalSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// بررسی فرمت و مقادیر پارامتر های ورودی قبل از درج ترمینال
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(YashilHekmatTerminalEditModel editModel)
        {
            if (!string.IsNullOrEmpty(editModel.IpAddress) && !editModel.IpAddress.IsValidIpAddress())
            {
                return "کاربر گرامی، لطفا فرمت آی پی آدرس را صحیح وارد نمایید.";
            }
            if (string.IsNullOrEmpty(editModel.PublicKey))
            {
                return "کاربر گرامی، لطفا کلید عمومی را مقدار دهی نمایید.";
            }
            if (editModel.MaxInstallmentCount <= 0)
            {
                return "کاربر گرامی، لطفا سقف اقساط را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MaxAmount <= 0)
            {
                return "کاربر گرامی، لطفا حداکثر مبلغ تراکنش را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MinAmount <= 0)
            {
                return "کاربر گرامی، لطفا حداقل مبلغ تراکنش را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MaxAmount < editModel.MinAmount)
            {
                return "کاربر گرامی، لطفا حداکثر مبلغ باید بزرگتر از حداقل مبلغ باشد.";
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }
        /// <summary>
        /// بررسی فرمت و مقادیر پارامتر های ورودی قبل از ویرایش ترمینال
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeUpdateAsync(YashilHekmatTerminalEditModel editModel)
        {
            if (!string.IsNullOrEmpty(editModel.IpAddress) && !editModel.IpAddress.IsValidIpAddress())
            {
                return "کاربر گرامی، لطفا فرمت آی پی آدرس را صحیح وارد نمایید.";
            }
            if (string.IsNullOrEmpty(editModel.PublicKey))
            {
                return "کاربر گرامی، لطفا کلید عمومی را مقدار دهی نمایید.";
            }
            if (editModel.MaxInstallmentCount <= 0)
            {
                return "کاربر گرامی، لطفا سقف اقساط را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MaxAmount <= 0)
            {
                return "کاربر گرامی، لطفا حداکثر مبلغ تراکنش را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MinAmount <= 0)
            {
                return "کاربر گرامی، لطفا حداقل مبلغ تراکنش را بزرگتر از 0 مقدار دهی نمایید.";
            }
            if (editModel.MaxAmount < editModel.MinAmount)
            {
                return "کاربر گرامی، لطفا حداکثر مبلغ باید بزرگتر از حداقل مبلغ باشد.";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
    }
}