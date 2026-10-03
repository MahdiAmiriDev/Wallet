namespace Yashil.Infrastructure.Util
{
    public partial interface ICurrency
    {
        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        string FormatPrice(decimal price, decimal exchangeRate);
    }
}
