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
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class CardAmountHistoryController : BaseController<CardAmountHistory, int, CardAmountHistoryListViewModel, CardAmountHistoryEditModel, CardAmountHistorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICardAmountHistoryService _cardAmountHistoryService;
        private readonly IYashilLogger<CardAmountHistory> _logger;
        private readonly IPermissionService _permissionService;
        public CardAmountHistoryController(ICardAmountHistoryService cardAmountHistoryService, IMapper mapper, IYashilLogger<CardAmountHistory> logger, IPermissionService permissionService) : base(cardAmountHistoryService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _cardAmountHistoryService = cardAmountHistoryService;
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? cardInfoId)
        {
            var cardAmountHistories = _cardAmountHistoryService.GetByCustomFilter(cardInfoId);
            return await DataSourceLoader.LoadAsync(cardAmountHistories.ProjectTo<CardAmountHistoryListViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? cardInfoId)
        {
            var cardAmountHistories = _cardAmountHistoryService.GetByCustomFilter(cardInfoId);
            return await DataSourceLoader.LoadAsync(cardAmountHistories.ProjectTo<CardAmountHistorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}      