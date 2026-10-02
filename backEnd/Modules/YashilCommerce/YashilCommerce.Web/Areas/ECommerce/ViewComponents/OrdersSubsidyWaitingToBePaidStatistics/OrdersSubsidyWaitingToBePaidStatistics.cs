using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.OrdersSubsidyWaitingToBePaidStatistics
{
    public class OrdersSubsidyWaitingToBePaidStatisticsViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;


        public OrdersSubsidyWaitingToBePaidStatisticsViewComponent(IMapper mapper,
            IOrderService orderService,
            IUserPrincipal userPrincipal,
            IAdditionalUserPropService additionalUserPropService)
        {
            _orderService = orderService;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
        }

        public IViewComponentResult Invoke(string cardNumber, bool checkOnlyCompletedOrders = false, bool checkForCurrentUser = false)
        {
            var userAdditionalUserProp = _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id).Result;
            OrderSubsidyStatusViewModel orderSubsidyStatusViewModel = null;
            if (userAdditionalUserProp.SupportSubsidy)
            {
                var orders = _orderService.GetNotRejectedSubsidyBasedOrdersByFilter(_userPrincipal.Id, cardNumber, checkOnlyCompletedOrders, checkForCurrentUser);
                orderSubsidyStatusViewModel = new OrderSubsidyStatusViewModel
                {
                    OrdersCount = orders.Count(),
                    SubsidySum = orders.SelectMany(c => c.OrderItem).Where(oi => !oi.Deleted && !oi.ParentId.HasValue).Sum(x => x.SubsidyPrice),
                    CheckOnlyCompletedOrders = checkOnlyCompletedOrders,
                    CheckForCurrentUser = checkForCurrentUser
                };
            }
            return View("_OrdersSubsidyWaitingToBePaidStatistics", orderSubsidyStatusViewModel);
        }
    }
}
