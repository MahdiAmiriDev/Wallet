using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.Payment.ViewModels;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels;
using Yashil.Infrastructure.Util;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using YashilPayment.Web.Helper;
using Yashil.Core.Protos;
using PersianCalendar = System.Globalization.PersianCalendar;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    public class YashilIpgController : YashilMvcBaseController<CardInfo>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardInfoService _cardInfoService;
        private readonly IMerchantInfoService _merchantInfoService;
        private readonly ICreditOrCashPaymentService _creditOrCashPaymentService;
        private readonly IInstallmentService _installmentService;
        public YashilIpgController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<CardInfo> logger, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor, ICardInfoService cardInfoService, IMerchantInfoService merchantInfoService, ICreditOrCashPaymentService creditOrCashPaymentService, IInstallmentService installmentService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cardInfoService = cardInfoService;
            _merchantInfoService = merchantInfoService;
            _creditOrCashPaymentService = creditOrCashPaymentService;
            _installmentService = installmentService;
        }

        //ابتدا این متد توسط پذیرنده برای دریافت توکن فراخوانی می شود
        [AllowAnonymous]
        [HttpPost("PaymentRequest")]
        public ResultModel PaymentRequest([FromBody] PaymentRequestModel paymentRequest)
        {
            ResultModel resultModel = new ResultModel
            {
                Succeeded = false,
                Status = 0
            };
            string checkPaymentRequest = PaymentValidator.CheckPaymentRequest(paymentRequest);
            if (string.IsNullOrEmpty(checkPaymentRequest))
            {
                string ipAddress = string.Empty;
                if (_httpContextAccessor.HttpContext is { Connection.RemoteIpAddress: { } })
                {
                    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                MerchantInfo merchantInfo = _merchantInfoService.GetMerchantInfoByIdAndIp(Convert.ToInt32(paymentRequest.MerchantId), Convert.ToInt32(paymentRequest.TerminalId), ipAddress);
                if (merchantInfo != null)
                {
                    string data = $"{paymentRequest.MerchantId};{paymentRequest.TerminalId};{paymentRequest.ReservationNumber};{paymentRequest.Amount};{paymentRequest.InstallmentCount}";
                    using IEnumerator<MerchantTerminalInfo> enumerator = merchantInfo.MerchantTerminalInfo.GetEnumerator();
                    enumerator.MoveNext();
                    bool result = CryptoHelper.VerifySignature(data, paymentRequest.SignData, enumerator.Current.PublicKey);
                    if (result)
                    {
                        data = $"{ipAddress};{DateTime.Now.AddMinutes(20)};{paymentRequest.MerchantId};{paymentRequest.TerminalId};{paymentRequest.ReservationNumber};{paymentRequest.LocalDateTime};{paymentRequest.ReturnUrl};{paymentRequest.Amount};{paymentRequest.InstallmentCount}";
                        string token = CryptographyHelper.AesEncrypt(data);
                        resultModel.Message = token;
                        resultModel.Succeeded = true;
                    }
                    else
                    {
                        resultModel.Status = 3;
                        resultModel.Message = "امضا معتبر نمی باشد";
                    }
                }
                else
                {
                    resultModel.Status = 2;
                    resultModel.Message = "اطلاعات پذیرنده معتبر نمی باشد";
                }
            }
            else
            {
                resultModel.Status = 1;
                resultModel.Message = checkPaymentRequest;
            }
            return resultModel;
        }

        //پذیرنده توکن را به صفحه اصلی درگاه پرداخت ارسال و کاربر را به این صفحه هدایت می نماید
        public IActionResult Index([FromQuery(Name = "t")] string token)
        {
            var resultModel = new ResultModel
            {
                Succeeded = false,
                Status = 0
            };
            token = WebUtility.UrlDecode(token);
            token = Regex.Replace(token, "%2b", "+", RegexOptions.IgnoreCase);
            string decrypted;
            try
            {
                decrypted = CryptographyHelper.AesDecrypt(token);
            }
            catch
            {
                resultModel.Message = "توکن معتبر نمی باشد";
                resultModel.Status = 1;
                return View("IPGError", resultModel);
            }
            string[] splitted = decrypted.Split(';');
            string ipAddress = string.Empty;
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
            {
                ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (!ipAddress.Equals(splitted[0]))
            {
                resultModel.Message = "آدرس سایت پذیرنده معتبر نمی باشد";
                resultModel.Status = 2;
                return View("IPGError", resultModel);
            }
            MerchantInfo merchantInfo = _merchantInfoService.GetMerchantInfoByIdAndIp(Convert.ToInt32(splitted[2]), Convert.ToInt32(splitted[3]), ipAddress);
            if (merchantInfo == null)
            {
                resultModel.Message = "اطلاعات پذیرنده معتبر نمی باشد";
                resultModel.Status = 3;
                return View("IPGError", resultModel);
            }
            int compare = DateTime.Compare(Convert.ToDateTime(splitted[1]), DateTime.Now);
            if (compare < 0)
            {
                resultModel.Message = "زمان اعتبار توکن منقضی شده است";
                resultModel.Status = 4;
                return View("IPGError", resultModel);
            }
            UserCardInfoViewModel userCardInfo = new UserCardInfoViewModel
            {
                PaymentRequestToken = token,
                PurchaseInfoDto = new PurchaseInfoDto()
            };
            int amount = Convert.ToInt32(splitted[7]);
            int installmentCount = Convert.ToInt32(splitted[8]);
            userCardInfo.PurchaseInfoDto.TotalAmount = amount;
            if (installmentCount > 0)
            {
                double installmentAmount = amount / installmentCount;
                userCardInfo.PurchaseInfoDto.Amount = Math.Round(installmentAmount);
                userCardInfo.PurchaseInfoDto.InstallmentCount = installmentCount;
            }
            return View(userCardInfo);
        }

        //فرآیند خرید ریالی یا اعتباری توسط این متد بررسی و انجام می شود
        [HttpPost("Purchase")]
        public async Task<IActionResult> Purchase(UserCardInfoViewModel userCardInfo)
        {
            PurchaseResult purchaseResult = new PurchaseResult { Status = 0 };
            string decrypted;
            string token = userCardInfo.PaymentRequestToken;
            /*string token = WebUtility.UrlDecode(userCardInfo.Token);
            token = Regex.Replace(token, "%2b", "+", RegexOptions.IgnoreCase);
            userCardInfo.Token = token;*/
            try
            {
                decrypted = CryptographyHelper.AesDecrypt(token);
            }
            catch (Exception e)
            {
                ViewBag.Message = "توکن معتبر نمی باشد";
                return View("Index", userCardInfo);
            }
            string[] splitted = decrypted.Split(';');
            int compare = DateTime.Compare(Convert.ToDateTime(splitted[1]), DateTime.Now);
            if (compare < 0)
            {
                ViewBag.Message = "زمان اعتبار توکن منقضی شده است";
                return View("Index", userCardInfo);
            }
            if (userCardInfo.ExpireDateYear.Length == 2)
            {
                userCardInfo.ExpireDateYear = "14" + userCardInfo.ExpireDateYear;
            }
            CardInfo cardInfo = _cardInfoService.GetCardInfoByCardNumber(userCardInfo.CardNumber, userCardInfo.CVV2, int.Parse(userCardInfo.ExpireDateYear + userCardInfo.ExpireDateMonth));
            if (cardInfo != null)
            {
                PersianCalendar pc = new PersianCalendar();
                DateTime thisDate = DateTime.Now;
                int thisMonth = pc.GetMonth(thisDate);
                int thisYear = pc.GetYear(thisDate);
                int expireDateMonth = int.Parse(userCardInfo.ExpireDateMonth);
                int expireDateYear = int.Parse(userCardInfo.ExpireDateYear);
                if (thisYear > expireDateYear || (thisYear == expireDateYear && thisMonth > expireDateMonth))
                {
                    ViewBag.Message = "تاریخ انقضای کارت سپری شده است";
                    return View("Index", userCardInfo);
                }
                int id = cardInfo.Id;
                if (cardInfo.SecondPassword != null && cardInfo.SecondPassword.Equals(userCardInfo.SecondPassword))
                {
                    CreditOrCashPayment creditOrCashPayment = new CreditOrCashPayment();
                    int TotalAmount = Convert.ToInt32(splitted[7]);
                    int amount = TotalAmount;
                    int installmentCount = Convert.ToInt32(splitted[8]);
                    bool insufficeint = false;
                    if (installmentCount > 0)
                    {
                        double installmentAmount = amount / installmentCount;
                        installmentAmount = Math.Round(installmentAmount);
                        amount = Convert.ToInt32(installmentAmount);
                        if (amount > cardInfo.CreditAmount)
                        {
                            insufficeint = true;
                        }
                        else
                        {
                            cardInfo.CreditAmount -= amount;
                        }
                    }
                    else
                    {
                        if (amount > cardInfo.CashAmount)
                        {
                            insufficeint = true;
                        }
                        else
                        {
                            cardInfo.CashAmount -= amount;
                        }
                    }
                    if (insufficeint)
                    {
                        ViewBag.Message = "موجودی کافی نیست";
                        return View("Index", userCardInfo);
                    }
                    string orderId = splitted[4];
                    string ipAddress = splitted[0];
                    string MerchantId = splitted[2];
                    string TerminalId = splitted[3];
                    creditOrCashPayment.CardId = id;
                    creditOrCashPayment.MerchantId = Convert.ToInt32(MerchantId);
                    creditOrCashPayment.OrderId = orderId;
                    int thisDay = pc.GetDayOfMonth(thisDate);
                    int thisHour = pc.GetHour(thisDate);
                    int thisMinute = pc.GetMinute(thisDate);
                    int thisSecond = pc.GetSecond(thisDate);
                    string buyDate = thisYear.ToString() + "/" + (thisMonth < 10 ? "0" + thisMonth.ToString() : thisMonth.ToString()) + "/" + (thisDay < 10 ? "0" + thisDay.ToString() : thisDay.ToString()) + " " + (thisHour < 10 ? "0" + thisHour.ToString() : thisHour.ToString()) + ":" + (thisMinute < 10 ? "0" + thisMinute.ToString() : thisMinute.ToString()) + ":" + (thisSecond < 10 ? "0" + thisSecond.ToString() : thisSecond.ToString());
                    creditOrCashPayment.BuyDate = DateTime.Now;
                    creditOrCashPayment.TotalAmount = TotalAmount;
                    //true is credit and false is cash
                    if (installmentCount > 0)
                    {
                        creditOrCashPayment.CreditOrCash = true;
                        creditOrCashPayment.InstallmentNumber = installmentCount;
                    }
                    else
                    {
                        creditOrCashPayment.CreditOrCash = false;
                    }
                    creditOrCashPayment.Status = 1093;
                    creditOrCashPayment.ApplicationId = 3;
                    creditOrCashPayment.CreateBy = 7;
                    creditOrCashPayment.CreationDate = DateTime.Now;
                    creditOrCashPayment.AccessLevelId = 0;
                    creditOrCashPayment.Deleted = false;
                    creditOrCashPayment.CreatorOrganizationId = 1;
                    creditOrCashPayment = await _creditOrCashPaymentService.AddAsync(creditOrCashPayment, true);
                    await _cardInfoService.UpdateAsync(cardInfo, id, true);
                    string data = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", orderId, TotalAmount, installmentCount, amount, creditOrCashPayment.Id, DateTime.Now.AddMinutes(15), ipAddress, MerchantId, TerminalId);
                    //این توکن با کلیک کاربر روی دکمه تکمیل فرآیند خرید برای پذیرنده ارسال می شود
                    purchaseResult.Token = CryptographyHelper.AesEncrypt(data);
                    purchaseResult.ReservationNumber = orderId;
                    purchaseResult.RefrenceNumber = creditOrCashPayment.Id.ToString();
                    purchaseResult.Url = splitted[6];
                    //در صورتی که فرآیند خرید به درستی انجام شود صفحه تکمیل فرآیند خرید به کاربر نمایش داده می شود
                    return View("Cont", purchaseResult);
                }
                else
                {
                    ViewBag.Message = "رمز وارد شده اشتباه است";
                }
            }
            else
            {
                ViewBag.Message = "اطلاعات کارت معتبر نمی باشد";
            }
            return View("Index", userCardInfo);
        }

        //پذیرنده توکن دریافت شده از فرآیند تکمیل خرید را جهت صحت سنجی برای درگاه ارسال می کند
        [AllowAnonymous]
        [HttpPost("Verify")]
        public async Task<VerifyResult> Verify([FromBody] VerifyData verifyData)
        {
            //TODO: use a scheduler to refund when verify method is not called after 15 min
            VerifyResult verifyResult = new VerifyResult();
            string checkVerifyData = PaymentValidator.CheckVerifyData(verifyData);
            if (string.IsNullOrEmpty(checkVerifyData))
            {
                string token = verifyData.Token;
                byte[] signData = verifyData.SignData;
                string decrypted;
                try
                {
                    decrypted = CryptographyHelper.AesDecrypt(token);
                }
                catch (Exception e)
                {
                    verifyResult.Status = 2;
                    verifyResult.Message = "توکن معتبر نمی باشد";
                    return verifyResult;
                }
                string[] splitted = decrypted.Split(';');
                string ipAddress = string.Empty;
                if (_httpContextAccessor.HttpContext is { Connection.RemoteIpAddress: { } })
                {
                    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                if (ipAddress.Equals(splitted[6]))
                {
                    MerchantInfo merchantInfo = _merchantInfoService.GetMerchantInfoByIdAndIp(Convert.ToInt32(splitted[7]), Convert.ToInt32(splitted[8]), splitted[6]);
                    if (merchantInfo != null)
                    {
                        System.Collections.Generic.IEnumerator<MerchantTerminalInfo> enumerator = merchantInfo.MerchantTerminalInfo.GetEnumerator();
                        enumerator.MoveNext();
                        if (CryptoHelper.VerifySignature(token, signData, enumerator.Current.PublicKey))
                        {
                            int compare = DateTime.Compare(Convert.ToDateTime(splitted[5]), DateTime.Now);
                            if (compare > 0)
                            {
                                int id = Convert.ToInt32(splitted[4]);
                                CreditOrCashPayment creditOrCashPayment = _creditOrCashPaymentService.Get(id, false);
                                if (creditOrCashPayment != null)
                                {
                                    if (creditOrCashPayment.Status != 1095)
                                    {
                                        verifyResult.Status = 0;
                                        int installmentCount = Convert.ToInt32(splitted[2]);
                                        int installmentAmount = Convert.ToInt32(splitted[3]);
                                        int totalAmount = Convert.ToInt32(splitted[1]);
                                        verifyResult.OrderId = splitted[0];
                                        verifyResult.Amount = totalAmount;
                                        verifyResult.SystemTraceNo = id;
                                        creditOrCashPayment.Status = 1095;
                                        await _creditOrCashPaymentService.UpdateAsync(creditOrCashPayment, id, true);
                                        if (installmentCount > 0)
                                        {
                                            verifyResult.InstallmentCount = installmentCount;
                                            Installment installment;
                                            DateTime date = DateTime.Now;
                                            DateTime nextMonth = date;
                                            PersianCalendar pc = new PersianCalendar();
                                            for (int i = 0; i < installmentCount; i++)
                                            {
                                                nextMonth = nextMonth.AddMonths(1);
                                                int year = pc.GetYear(nextMonth);
                                                int month = pc.GetMonth(nextMonth);
                                                int day = pc.GetDayOfMonth(nextMonth);
                                                string duedate = year.ToString() + (month < 10 ? "0" + month.ToString() : month.ToString()) + (day < 10 ? "0" + day.ToString() : day.ToString());
                                                installment = new Installment();
                                                installment.ApplicationId = 3;
                                                installment.CreateBy = 7;
                                                installment.CreationDate = date;
                                                installment.AccessLevelId = 0;
                                                installment.Deleted = false;
                                                installment.CreatorOrganizationId = 1;
                                                installment.PaymentId = id;
                                                installment.InstallmentAmount = installmentAmount;
                                                installment.PaymentStatus = 1093;
                                                installment.DueDate = Convert.ToInt32(duedate);
                                                await _installmentService.AddAsync(installment, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        verifyResult.Status = 8;
                                        verifyResult.Message = "عملیات تکراری است";
                                    }
                                }
                                else
                                {
                                    verifyResult.Status = 7;
                                    verifyResult.Message = "شماره پیگیری یافت نشد";
                                }
                            }
                            else
                            {
                                verifyResult.Status = 6;
                                verifyResult.Message = "زمان اعتبار توکن منقضی شده است";
                            }
                        }
                        else
                        {
                            verifyResult.Status = 5;
                            verifyResult.Message = "امضا معتبر نمی باشد";
                        }
                    }
                    else
                    {
                        verifyResult.Status = 4;
                        verifyResult.Message = "اطلاعات پذیرنده معتبر نمی باشد";
                    }
                }
                else
                {
                    verifyResult.Status = 3;
                    verifyResult.Message = "آدرس سایت پذیرنده معتبر نمی باشد";
                }
            }
            else
            {
                verifyResult.Status = 1;
                verifyResult.Message = checkVerifyData;
            }
            return verifyResult;
        }

        //این متد توسط پذیرنده برای برگشت دادن مبلغ ریالی یا اعتباری فراخوانی می شود
        [AllowAnonymous]
        [HttpPost("RefundRequest")]
        public async Task<ResultModel> RefundRequest([FromBody] RefundRequest refundRequest)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.Succeeded = false;
            resultModel.Status = 0;
            string checkRefundRequest = CheckRefundRequest(refundRequest);
            if (string.IsNullOrEmpty(checkRefundRequest))
            {
                string ipAddress = string.Empty;
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Connection != null && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                {
                    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                MerchantInfo merchantInfo = _merchantInfoService.GetMerchantInfoByIdAndIp(Convert.ToInt32(refundRequest.MerchantId), Convert.ToInt32(refundRequest.TerminalId), ipAddress);
                if (merchantInfo != null)
                {
                    string data =
                        $"{refundRequest.MerchantId};{refundRequest.TerminalId};{refundRequest.SystemTraceNo};{refundRequest.Amount}";
                    System.Collections.Generic.IEnumerator<MerchantTerminalInfo> enumerator = merchantInfo.MerchantTerminalInfo.GetEnumerator();
                    enumerator.MoveNext();
                    bool result = CryptoHelper.VerifySignature(data, refundRequest.SignData, enumerator.Current.PublicKey);
                    if (result)
                    {
                        int SystemTraceNo = Convert.ToInt32(refundRequest.SystemTraceNo);
                        int Amount = Convert.ToInt32(refundRequest.Amount);
                        CreditOrCashPayment creditOrCashPayment = _creditOrCashPaymentService.Get(SystemTraceNo, false);
                        if (creditOrCashPayment != null)
                        {
                            if (creditOrCashPayment.Status != 1097 && creditOrCashPayment.Status != 1096 && creditOrCashPayment.Status != 1125)
                            {
                                DateTime thisDate = DateTime.Now;
                                PersianCalendar pc = new PersianCalendar();
                                int thisYear = pc.GetYear(thisDate);
                                int thisMonth = pc.GetMonth(thisDate);
                                int thisDay = pc.GetDayOfMonth(thisDate);
                                int thisHour = pc.GetHour(thisDate);
                                int thisMinute = pc.GetMinute(thisDate);
                                int thisSecond = pc.GetSecond(thisDate);
                                string ReturnedDate = thisYear.ToString() + "/" + (thisMonth < 10 ? "0" + thisMonth.ToString() : thisMonth.ToString()) + "/" + (thisDay < 10 ? "0" + thisDay.ToString() : thisDay.ToString()) + " " + (thisHour < 10 ? "0" + thisHour.ToString() : thisHour.ToString()) + ":" + (thisMinute < 10 ? "0" + thisMinute.ToString() : thisMinute.ToString()) + ":" + (thisSecond < 10 ? "0" + thisSecond.ToString() : thisSecond.ToString());
                                creditOrCashPayment.ReturnedDate = DateTime.Now;
                                CardInfo cardInfo = _cardInfoService.Get(creditOrCashPayment.CardId, false);
                                //true is credit and false is cash
                                if (creditOrCashPayment.CreditOrCash)
                                {
                                    double installmentAmount = (double)(creditOrCashPayment.TotalAmount / creditOrCashPayment.InstallmentNumber);
                                    installmentAmount = Math.Round(installmentAmount);
                                    cardInfo.CreditAmount += Convert.ToInt32(installmentAmount);
                                    creditOrCashPayment.ReturnedAmount = Convert.ToInt32(installmentAmount);
                                    int status;
                                    /*if (Amount == installmentAmount)
                                    {*/
                                    status = 1097;
                                    /*}
                                    else
                                    {
                                        status = 1096;
                                    }*/
                                    creditOrCashPayment.Status = status;
                                    System.Collections.Generic.List<Installment> installments = _installmentService.GetInstallmentByPaymentId(creditOrCashPayment.Id);
                                    foreach (Installment installment in installments)
                                    {
                                        installment.PaymentStatus = status;
                                      await  _installmentService.UpdateAsync(installment, installment.Id, true);
                                    }
                                }
                                else
                                {
                                    creditOrCashPayment.ReturnedAmount = Amount;
                                    cardInfo.CashAmount += Amount;
                                    if (creditOrCashPayment.TotalAmount == Amount)
                                    {
                                        creditOrCashPayment.Status = 1097;
                                    }
                                    else
                                    {
                                        creditOrCashPayment.Status = 1096;
                                    }
                                }
                               await _creditOrCashPaymentService.UpdateAsync(creditOrCashPayment, creditOrCashPayment.Id, true);
                                await _cardInfoService.UpdateAsync(cardInfo, cardInfo.Id, true);
                                resultModel.Message = "با موفقیت انجام شد";
                                resultModel.Succeeded = true;
                            }
                            else
                            {
                                resultModel.Status = 5;
                                resultModel.Message = "عملیات تکراری است";
                            }
                        }
                        else
                        {
                            resultModel.Status = 4;
                            resultModel.Message = "شماره پیگیری یافت نشد";
                        }
                    }
                    else
                    {
                        resultModel.Status = 3;
                        resultModel.Message = "امضا معتبر نمی باشد";
                    }
                }
                else
                {
                    resultModel.Status = 2;
                    resultModel.Message = "اطلاعات پذیرنده معتبر نمی باشد";
                }
            }
            else
            {
                resultModel.Status = 1;
                resultModel.Message = checkRefundRequest;
            }
            return resultModel;
        }

        /*private bool VerifyData(string data, string sign)
        {
            bool result;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("<RSAKeyValue><Modulus>qIKEbq9gB77wp2jFYsPAd1FlmkKwDZpoGZN1ovM8rH8UxYTkzR4xphQsZucCRhbhchM/+mhuiFoxwOUhd4WzmnSDs/tf8E+hrNKPgl4GAhr9V+vo++D61v1lcK1mozSHyQKEdMOXECRd3s6Iydn8wNzrfq9CVxXX18wTb8YXNWU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
            result = rsa.VerifyData(Encoding.UTF8.GetBytes(data), SHA256.Create(), Convert.FromBase64String(sign));
            return result;
        }*/


        private string CheckRefundRequest(RefundRequest refundRequest)
        {
            string result = string.Empty;
            if (string.IsNullOrWhiteSpace(refundRequest.MerchantId))
            {
                result += "MerchantId must not be null. ";
            }
            if (string.IsNullOrWhiteSpace(refundRequest.TerminalId))
            {
                result += "TerminalId must not be null. ";
            }
            if (refundRequest.SystemTraceNo <= 0)
            {
                result += "SystemTraceNo must not be zero or less. ";
            }
            if (string.IsNullOrWhiteSpace(refundRequest.SignData.ToString()))
            {
                result += "SignData must not be null. ";
            }
            if (refundRequest.Amount <= 0)
            {
                result += "Amount must not be zero or less. ";
            }
            return result;
        }


    }
}
