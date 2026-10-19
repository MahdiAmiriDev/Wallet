namespace YashilPayment.Core.Classes
{
    public class ApplicationConfigKey
    {

        public static string YashilPaymentYashilHekmatTerminalDefaultLogoName = "YashilPayment.YashilHekmatTerminal.DefaultLogoName";
        /// <summary>
        /// نقش مربوط به پذیرنده ها که می توانند اطلاعات مربوط به تراکنش خود را مشاهده کنند
        /// </summary>
        public static string YashilPaymentConfigurationMerchantRole = "YashilPayment.Configuration.MerchantRole";
        /// <summary>
        /// سقف پرداخت اقساطی با حکمت
        /// </summary>
        public static string YashilHekmatMinimumPriceToPay = "YashilCommerce.PaymentMethod.HekmatMinimumPriceToPay";
        /// <summary>
        /// شناسه دسته بندی مالیات
        /// </summary>
        public static string YashilPaymentTaxDefaultTaxCategoryId = "YashilPayment.Tax.DefaultTaxCategoryId";
    }
}
