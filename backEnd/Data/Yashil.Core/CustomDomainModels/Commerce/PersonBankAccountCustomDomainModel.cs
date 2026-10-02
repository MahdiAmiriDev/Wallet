namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class PersonBankAccountCustomDomainModel
    {
        public int Id { get; set; }
        /// <summary>
        ///         /// شخص
        ///</summary>
        public int PersonId { get; set; }
        /// <summary>
        ///         /// بانک
        ///</summary>
        public int BankType { get; set; }
        /// <summary>
        ///         /// شماره کارت
        ///</summary>
        public string? CardNumber { get; set; }
        /// <summary>
        ///         /// شماره حساب
        ///</summary>
        public string AccountNumber { get; set; }
        /// <summary>
        ///         /// شماره شبا
        ///</summary>
        public string? ShebaNumber { get; set; }
        /// <summary>
        ///         /// نام شعبه
        ///</summary>
        public string? BranchName { get; set; }

        public string  FullName { get; set; }
        public int DepositStatus { get; set; }
    }
}
