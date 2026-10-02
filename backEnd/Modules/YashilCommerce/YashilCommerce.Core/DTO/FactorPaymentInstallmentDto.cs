using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.DTO;
public class FactorPaymentInstallmentInfoDto
{
    /// <summary>
    /// سال ماه
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ماه
    /// </summary>
    public string MonthName { get; set; }

    /// <summary>
    /// سال
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// آیا مجاز به آپلود اقساط فاکتور می باشد
    /// در صورتی که اقساط فاکتور را در ماه خاصی به صورت کامل پرداخت نکرده باشند
    /// این فید مقدار
    /// True
    /// دارد
    /// </summary>
    public bool AllowUploadFactorPaymentInstallment { get; set; }

}

public class FactorPaymentInstallmentPerMonthDto
{
    public int Id { get; set; }
    /// <summary>
    /// کد نمایندگی یا مرکز
    /// </summary>
    public string CompanyOrRepresentationCode { get; set; }
    public string RepresentationTitle { get; set; }
    public string CompanyTitle { get; set; }
    /// <summary>
    /// عنوان نمایندگی یا مرکز
    /// </summary>
    public string CompanyOrRepresentationTitle =>
        RepresentationTitle ?? CompanyTitle;
    /// <summary>
    /// کد فاکتور
    /// </summary>
    public string FactorCode { get; set; }
    /// <summary>
    /// تعداد اقساط پرداختی
    /// </summary>
    public int NumberOfInstallmentsPaid { get; set; }
    /// <summary>
    /// مبلغ فاکتور : مجموع مبلغ پرداختی فاکتور
    /// </summary>
    public string SumAmountPaymentToSupplier { get; set; }
    /// <summary>
    /// مبلغ یک قسط
    /// </summary>
    public string InstallmentAmount { get; set; }
    /// <summary>
    /// عودت وجه
    /// </summary>
    public decimal? RefundAmount { get; set; }
    /// <summary>
    /// میلغ پرداختی : مبلغی که پس از کسر عودت (یا سایر کسورات) به کاربر قرار است پرداخت شود
    /// </summary>
    public decimal? PaidAmount { get; set; }
    /// <summary>
    /// مانده مبلغ
    /// </summary>
    public decimal? RemainingPaymentToSupplier { get; set; }
    /// <summary>
    /// مجموع پرداختی : مجموع پرداختی هایی که از فاکتور تا الان انجام شده است
    /// </summary>
    public decimal? TotalPaymentToSupplier { get; set; }
    /// <summary>
    /// شماره قسط
    /// </summary>
    public string InstallmentNumber { get; set; }
    /// <summary>
    /// مانده قسط
    /// </summary>
    public string NumberOfInstallmentsRemaining { get; set; }
    /// <summary>
    /// شماره حساب
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// شماره شبا
    /// </summary>
    public string ShebaNumber { get; set; }
    /// <summary>
    /// نام صاحب حساب
    /// </summary>
    public string AccountOwner { get; set; }
    /// <summary>
    /// نام بانک
    /// </summary>
    public string BankName { get; set; }
}
/// <summary>
/// پارامتر هایی که برای اقساط پرداختی فاکتور از سمت کلاینت به بک اند ارسال شده است
/// </summary>
public class FactorPaymentInstallmentExportToExcelParamsDto
{
    /// <summary>
    /// شناسه های پرداخت اقساط فاکتور که کاربر انتخاب کرده است
    /// </summary>
    public List<int> Ids { get; set; }
    /// <summary>
    /// سال و ماه
    /// </summary>
    public int YearMonth { get; set; }
}

public class RegisterPaymentInstallmentReferencParamsDto
{
    /// <summary>
    /// سال و ماه
    /// </summary>
    public int? YearMonth { get; set; }
    /// <summary>
    /// شناسه مرجع پرداخت
    /// </summary>
    public string PaymentReferenceId { get; set; }
    /// <summary>
    /// تاریخ مرجع پرداخت
    /// </summary>
    public string PaymentReferenceDate { get; set; }
    /// <summary>
    /// زمان ثبت اطلاعات فایل اکسل در جدول
    /// UploadedFactorInstallmentPayment
    /// </summary>
    public DateTime CreationDate { get; set; }
}


