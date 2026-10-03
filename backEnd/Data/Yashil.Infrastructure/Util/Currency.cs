using System;

namespace Yashil.Infrastructure.Util
{
   public partial class Currency:ICurrency
    {
        public Currency()
        {
        }
        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public virtual string FormatPrice(decimal price, decimal exchangeRate)
        {
            var priceCalculation = ConvertCurrency(price, exchangeRate);
            priceCalculation = Math.Round(priceCalculation, 2);
            return string.Format("{0}", priceCalculation);
        }
        /// <summary>
        /// Converts currency to exchangeRate
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        public decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;
            return decimal.Zero;
        }
    }
}
