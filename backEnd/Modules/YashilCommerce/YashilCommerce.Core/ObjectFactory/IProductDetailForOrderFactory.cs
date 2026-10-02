using Microsoft.AspNetCore.Mvc;
/// <summary>
/// مشاهده جزییات کالا در قسمت  بررسی خرید و یا سبد خرید
/// </summary>
namespace YashilCommerce.Core.ObjectFactory
{
    public interface IProductDetailForOrderFactory
    {
        IViewComponentResult Invoke(int productSalesPlanningId);
    }
}
