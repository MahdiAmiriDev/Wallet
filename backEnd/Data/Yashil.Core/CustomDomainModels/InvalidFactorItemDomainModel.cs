namespace Yashil.Core.CustomDomainModels
{
    public class InvalidFactorItemDomainModel
    {
        public int Id { get; set; }
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public string OrderTitle { get; set; }
        /// <summary>
        ///         /// قیمت با تخفیف
        ///</summary>
        public decimal OrderItemPriceIncDiscount { get; set; }
        /// <summary>
        ///         /// قیمت  صحیح با تخفیف
        ///</summary>
        public decimal CorrectOrderItemPriceIncDiscount { get; set; }
        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        public decimal ProfitPrice { get; set; }
        /// <summary>
        /// مبلغ تخفیف صحیح
        /// </summary>
        public decimal CorrectProfitPrice { get; set; }
        /// <summary>
        ///         /// میزان تخفیف (بدون مالیات)
        ///</summary>
        public decimal DiscountAmountExclTax { get; set; }
        /// <summary>
        /// مبلغ صحیح  میزان تخفیف (بدون مالیات)
        /// </summary>
        public decimal CorrectDiscountAmountExclTax { get; set; }
        /// <summary>
        ///         /// میزان کارمزد  (بدون مالیات)
        ///</summary>
        public decimal WageAmountExcTax { get; set; }
        /// <summary>
        /// مبلغ صحیح میزان کارمزد  (بدون مالیات)
        /// </summary>
        public decimal CorrectWageAmountExcTax { get; set; }
    }
}