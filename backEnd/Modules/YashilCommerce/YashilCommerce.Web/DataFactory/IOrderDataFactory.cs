using System.Threading.Tasks;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.DataFactory
{
    public interface IOrderDataFactory
    {
        Task<OrderSummaryViewModel> FillOrderSummaryByRecipientId(int? recipientId);
        /// <summary>
        /// اطلاعات مربوط به خلاصه سفارش را بر اساس کد سفارش پر می کند
        /// </summary>
        /// <param name="orderId">کد سفارش</param>
        /// <returns></returns>
        Task<OrderSummaryViewModel> FillOrderSummaryByOrderId(int? orderId);
        Task<CheckoutViewModel> PrepareCheckoutViewModelByRecipientIdAsync(int? recipientId);
     
    }
}
