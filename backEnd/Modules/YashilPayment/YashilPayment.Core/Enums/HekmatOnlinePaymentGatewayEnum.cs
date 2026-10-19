namespace YashilPayment.Core.Enums
{
    /// <summary>
    /// وضعیت پرداخت هنگامی که از درگاه به سمت متد
    /// CallBackUrl
    /// بر می گردد
    /// </summary>
    public enum HekmatOnlinePaymentGatewayPaymentStatusEnum
    {
        /// <summary>
        /// پرداخحت موفق
        /// </summary>
        Ok,
        /// <summary>
        /// انصراف از پرداخت/پرداخت ناموفق
        /// </summary>
        Cancel
    }
}
