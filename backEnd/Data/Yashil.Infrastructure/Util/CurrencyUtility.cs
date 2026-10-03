using System;

namespace Yashil.Infrastructure.Util
{
    public static class CurrencyUtility
    {
        public static string RemoveDecimal(decimal? price, int numberOfDecimal = 0)
        {
            if (!price.HasValue)
            {
                return string.Empty;
            }
            var priceCalculation = price.Value;
            priceCalculation = Math.Round(priceCalculation, numberOfDecimal);
            return string.Format("{0:n0}", priceCalculation);
        }
        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">قیمت</param>
        /// <param name="exchangeRate">نرخ ارز</param>
        /// <param name="numberOfDecimal"></param>
        /// <returns></returns>
        public static string FormatPrice(decimal? price, int numberOfDecimal = 0)
        {
            if (!price.HasValue)
            {
                return string.Empty;
            }
            decimal exchangeRate = 1 / (decimal)10;
            var priceCalculation = ConvertCurrency(price.Value, exchangeRate);
            priceCalculation = Math.Round(priceCalculation, numberOfDecimal);
            return string.Format("{0:n0}", priceCalculation);
        }
        /// <summary>
        /// Converts currency to exchangeRate
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        private static decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;
            return decimal.Zero;
        }
    }
}
