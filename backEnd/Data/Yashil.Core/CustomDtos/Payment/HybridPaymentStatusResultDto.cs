using System.Collections.Generic;
using Yashil.Core.Enums;

namespace Yashil.Core.CustomDtos.Payment;

public class HybridPaymentStatusResultDto
{
    public int Id { get; set; }
    /// <summary>
    /// شناسه پرداخت پذیرنده
    /// </summary>
    public string ReservationNumber { get; set; }
    /// <summary>
    ///  شماره پیگیری دارنده
    /// </summary>
    public string ReferenceNumber { get; set; }
    /// <summary>
    /// کد خطا :درصورتی که دریافت وضعیت پرداخت دارای خطا باشد کدی اعلام می شود که نشان دهنده خطای رخ داه است
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    ///  کد نتیجه پرداخت یعنی
    /// پرداخت با موفقیت انجام شد Successful = 1,
    /// پرداخت ناموفق می باشد Unsuccessful = 2,
    /// ثبت اولیه Initial = 4,
    /// پرداخت ناقص Incomplete = 3,
    /// عودت وجه Refund = 2,
    /// وضعیت نامشخص Unknown = -1,
    /// <see cref="PaymentGatewayResultStatusCodeEnum" />
    /// </summary>
    public int StatusCode { get; set; }

    public int? CashPaymentPercentage { get; set; }
    public int? InstallmentCount { get; set; }
    
    /// <summary>
    /// پیام
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// تاریخ درخواست
    /// </summary>
    public string PaidDate { get; set; }
    
    public IEnumerable<HybridPaymentDto> HybridPayments { get; set; }
}