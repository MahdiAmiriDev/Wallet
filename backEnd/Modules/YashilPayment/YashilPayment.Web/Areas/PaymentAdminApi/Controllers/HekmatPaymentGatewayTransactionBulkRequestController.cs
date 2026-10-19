using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MimeKit;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;


namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers;

public class HekmatPaymentGatewayTransactionBulkRequestController : BaseController<HekmatPaymentGatewayTransactionBulkRequest
        , int, HekmatPaymentGatewayTransactionBulkRequestListViewModel, HekmatPaymentGatewayTransactionBulkRequestEditModel,
        HekmatPaymentGatewayTransactionBulkRequestSimpleViewModel>
{
    private readonly IMapper _mapper;
    private readonly IHekmatPaymentGatewayTransactionBulkRequestService _hekmatPaymentGatewayTransactionBulkRequestService;
    private readonly IHekmatPaymentGatewayTransactionBulkRequestDetailService _hekmatPaymentGatewayTransactionBulkRequestDetailService;
    private readonly IUserPrincipal _userPrincipal;
    private readonly IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestController> _logger;

    public HekmatPaymentGatewayTransactionBulkRequestController(IHekmatPaymentGatewayTransactionBulkRequestService hekmatPaymentGatewayTransactionBulkRequestService,
        IMapper mapper,
        IYashilLogger<HekmatPaymentGatewayTransactionBulkRequest> logger,
        IPermissionService permissionService, IUserPrincipal userPrincipal, IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestController> logger1, IHekmatPaymentGatewayTransactionBulkRequestDetailService heatPaymentGatewayTransactionBulkRequestDetailService) : base(hekmatPaymentGatewayTransactionBulkRequestService, mapper, logger, permissionService)
    {
        _mapper = mapper;
        _userPrincipal = userPrincipal;
        _logger = logger1;
        _hekmatPaymentGatewayTransactionBulkRequestDetailService = heatPaymentGatewayTransactionBulkRequestDetailService;
        _hekmatPaymentGatewayTransactionBulkRequestService = hekmatPaymentGatewayTransactionBulkRequestService;
    }

    [HttpGet("GetByCustomFilterForList")]
    public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId)
    {
        var hekmatPaymentGatewayTransactionBulkRequests =
            _hekmatPaymentGatewayTransactionBulkRequestService.GetByCustomFilter(parentId, hekmatPaymentGatewayTransactionBulkRequestCategoryId);
        return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequests.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestListViewModel>(_mapper.ConfigurationProvider), loadOptions);
    }

    [HttpGet("GetByCustomForSelect")]
    public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId)
    {
        var hekmatPaymentGatewayTransactionBulkRequests = _hekmatPaymentGatewayTransactionBulkRequestService.GetByCustomFilter(parentId, hekmatPaymentGatewayTransactionBulkRequestCategoryId);
        return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequests.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
    }

    /// <summary>
    /// دانلود نمونه اکسل تراکنش های حکمت
    /// </summary>
    /// <returns></returns>
    [HttpGet("ExportSampleTransactionBulkRequestFileExcel")]
    public FileResult ExportSampleTransactionBulkRequestFileExcel()
    {
        var sampleTransactionFile = _hekmatPaymentGatewayTransactionBulkRequestService
            .SampleTransactionFileToWorkBook();

        return File(sampleTransactionFile,
            MimeTypes.GetMimeType("SampleTransactionFile.xlsx"),
            "SampleTransactionFile.xlsx");
    }

    /// <summary>
    /// بارگذاری اکسل تراکنش های حکمت
    /// </summary>
    /// <param name="transactionFile">فایل تراکنش</param>
    /// <param name="hekmatPaymentGatewayTransactionBulkRequestCategoryId">شناسه دسته بندی درخواست انبوه تراکنش حکمت </param>
    /// <returns></returns>
    [HttpPost("UploadHekmatTransactionFile/{hekmatPaymentGatewayTransactionBulkRequestCategoryId}")]
    [YashilAuthorization(ActionName = "PostEntity")]
    public async Task<ResultStatus<int>> UploadHekmatTransactionFileAsync(
        IFormFile transactionFile,
        int hekmatPaymentGatewayTransactionBulkRequestCategoryId)
    {
        try
        {
            if (hekmatPaymentGatewayTransactionBulkRequestCategoryId <= 0)
            {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "خطا در ارسال پارامتر دسته بندی "
                    };
            }

            // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
            await _hekmatPaymentGatewayTransactionBulkRequestService
                .DeleteStatusNotDeterminedBulkRequestWithDetailAsync(_userPrincipal.Id);
            // درج کاربران اکسل در جدول UploadedUser
            var res = await _hekmatPaymentGatewayTransactionBulkRequestService.BulkInsertFromExcelAsync(transactionFile, hekmatPaymentGatewayTransactionBulkRequestCategoryId);
            if (res.IsSuccess)
            {
                // موارد مربوط به معتبر بودن فایل بارگذاری شده را بررسی میکند
                var invalidUploadedUserCount = await _hekmatPaymentGatewayTransactionBulkRequestService
                    .GetInvalidUploadedHekmatTransactionBulkCountAsync(_userPrincipal.Id);
                if (invalidUploadedUserCount > 0)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "فایل بارگذاری شده دارای خطا می باشد، لطفا پس از رفع ایرادات فایل، مجددا آن را بارگذاری نمایید"
                    };
                }
                return new ResultStatus<int>
                {
                    EntityId = res.EntityId,
                    IsSuccess = true,
                    Message = "بارگذاری فایل اکسل با موفقیت انجام شد."
                };
            }

            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = res.Message
            };

        }
        catch(Exception e)
        {
            _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                nameof(HekmatPaymentGatewayTransactionBulkRequest), "HekmatPaymentGatewayTransactionBulkRequest" +
               "Bulk Insert Error",e);
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا در بارگذاری فایل اکسل"
            };
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست درج تراکنش حکمت به صورت انبوه </param>
    /// <returns></returns>
    [HttpPost("AddHekmatBulkRequestDetail")]
    [YashilAuthorization(ActionName = "PostEntity")]
    public async Task<ResultStatus<int>> AddHekmatBulkRequestDetailAsync(int hekmatPaymentGatewayTransactionBulkRequestId)
    {
        if (hekmatPaymentGatewayTransactionBulkRequestId <= 0)
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "کاربر گرامی، در ارسال پارامتر خطا رخ داده است"
            };
        }

        var existInvalidUploadedItem  = _hekmatPaymentGatewayTransactionBulkRequestDetailService
            .GetInvalidHekmatBulkRequest(_userPrincipal.Id,hekmatPaymentGatewayTransactionBulkRequestId,true)
            .Any();
        if (existInvalidUploadedItem)
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "کاربر گرامی، فایل بارگذاری شده دارای خطا می باشد"
            };
        }

        var res = await _hekmatPaymentGatewayTransactionBulkRequestService.AddHekmatPaymentGatewayTransactionFromBulkRequestAsync
        (hekmatPaymentGatewayTransactionBulkRequestId, _userPrincipal.Id);
        if (res > 0)
        {

            return new ResultStatus<int>
            {
                IsSuccess = true,
                Message = "کاربر گرامی، درج کاربر با موفقیت انجام شد."
            };
        }
        return new ResultStatus<int>
        {
            IsSuccess = false,
            Message = "کاربر گرامی، در ثبت نهایی درج تراکنش ها از طریق اکسل خطا رخ داده است"
        };

    }
}
