using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.Entities;
using YashilDms.Core.Services;
using Yashil.Core.Protos;
using YashilCommerce.Core.Classes;
using YashilUserManagement.Core.Services;
using System.Net;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class HekmatService : IHekmatService
    {
        private readonly IHekmatPaymentConfiguration _hekmatPaymentConfiguration;
        private readonly IPaymentService _paymentService;
        private readonly IYashilLogger<object> _logger;
        private readonly IOrderService _orderService;
        private readonly ISqlBulkCopyExcelToDb _sqlBulkCopyExcelToDb;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefundDetailService _refundDetailService;
        private readonly IHekmatApprovedTransactionService _hekmatApprovedTransactionService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IConfiguration _configuration;
        private readonly IAppConfigService _appConfigService;
        private readonly IHostEnvironment _environment;


        public HekmatService(IHekmatPaymentConfiguration hekmatPaymentConfiguration,
            IOrderService orderService
            , IYashilLogger<object> logger,
       IPaymentService paymentService, ISqlBulkCopyExcelToDb oledbBasedRepository,
            IAppDocumentService appDocumentService,
            IUnitOfWork unitOfWork, IRefundDetailService refundDetailService,
            IHekmatApprovedTransactionService hekmatApprovedTransactionService,
            ApplicationConfiguration applicationConfiguration, IAdditionalUserPropService additionalUserPropService, IConfiguration configuration, IAppConfigService appConfigService, IHostEnvironment environment)
        {
            _hekmatPaymentConfiguration = hekmatPaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _sqlBulkCopyExcelToDb = oledbBasedRepository;
            _appDocumentService = appDocumentService;
            _unitOfWork = unitOfWork;
            _refundDetailService = refundDetailService;
            _logger = logger;
            _hekmatApprovedTransactionService = hekmatApprovedTransactionService;
            _applicationConfiguration = applicationConfiguration;
            _additionalUserPropService = additionalUserPropService;
            _configuration = configuration;
            _appConfigService = appConfigService;
            _environment = environment;
        }
        /// <summary>
        /// شسناسه پرداخت را به درگاه ارسال می کند و آخرین وضعیت پرداخت حکمت را دریافت می کند.
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت</param>
        /// <returns></returns>
        public async Task<PaymentStatusResultModel> GetPaymentStatusByPaymentIdAsync(int paymentId)
        {
            var reservationNumber = paymentId.ToString();
            var hekmatIpgService = CreateHekmatIpgService();
            var merchantId = Convert.ToInt32(_hekmatPaymentConfiguration.MerchantId);
            var terminalId = Convert.ToInt32(_hekmatPaymentConfiguration.TerminalId);
            var data = $"{merchantId};{terminalId};{reservationNumber}";
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(_hekmatPaymentConfiguration.PrivateKey.ToCharArray());
            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), SHA256.Create());
            try
            {
                return await hekmatIpgService.GetPaymentStatusAsync(new PaymentStatusModel()
                {
                    MerchantId = Convert.ToInt32(_hekmatPaymentConfiguration.MerchantId),
                    TerminalId = Convert.ToInt32(_hekmatPaymentConfiguration.TerminalId),
                    ReservationNumber = reservationNumber,
                    SignData = Convert.ToBase64String(signedData)

                });
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, "HekmatIpg", "failed to call grpc service CheckPaymentStatusAsync", e);
                return null;
            }

        }

        public async Task<bool> UpdateOrderAndPaymentStatusByPaymentIdAsync(int paymentId, int orderId)
        {
            var order = await _orderService.GetAsync(orderId, true);
            //اگر سفارش  در وضعیت موفق-تکمیل شده نباشد => وضعیت سفارش و وضعیت پرداخت را به روز رسانی کند 
            if (order == null)
            {
                return false;
            }

            var paymentStatus = await GetPaymentStatusByPaymentIdAsync(paymentId);
            if (paymentStatus != null)
            {
                if (paymentStatus.ErrorCode == (int)HekmatPaymentStatusResultErrorCodeEnum.Successful)
                {
                    if (paymentStatus.StatusCode == (int)HekmatPaymentStatusResultStatusCodeEnum.Successful)
                    {
                        //:TODO update To Payment And Update Order Status
                        //:TODO قیل از تغییر وضعیت سفارش چک شود که مجموع مبالغ پرداخت شده مساوی مبلغ سفارش باشد 
                        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatusTypesEnum.Complete);
                        await _paymentService.UpdatePaymentStatusAsync(paymentId,
                            PaymentStatusTypesEnum.Paid, PaymentMethodTypesEnum.Hekmat,
                            paymentStatus.Message, paymentStatus.RefrenceNumber, true);
                    }
                    else if (paymentStatus.StatusCode ==
                             (int)HekmatPaymentStatusResultStatusCodeEnum.Unsuccessful)
                    {
                        //اگر سفارش در وضعیت پرداخت ناقص بود و پرداخت کاربر وضعیت ناموفق داشت نباید وضعیت سفارش تغییر کند
                        if (order.OrderStatusType != (int)OrderStatusTypesEnum.IncompletePayment)
                        {
                            await _orderService.UpdateOrderStatusAsync(orderId,
                                OrderStatusTypesEnum.UnsuccessfulPayment);
                        }

                        await _paymentService.UpdatePaymentStatusAsync(paymentId,
                            PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.Hekmat,
                            paymentStatus.Message, paymentStatus.RefrenceNumber);
                        await _paymentService.SaveChangeAsync();
                    }
                    else if (paymentStatus.StatusCode ==
                             (int)HekmatPaymentStatusResultStatusCodeEnum.InQueueToProcess)
                    {
                        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatusTypesEnum.Processing);
                        await _paymentService.UpdatePaymentStatusAsync(paymentId,
                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                            PaymentMethodTypesEnum.Hekmat, paymentStatus.Message,
                            paymentStatus.RefrenceNumber, true);
                    }
                    else
                    {
                        _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                            $"failed in updating HekmatIpg in HekmatIpgservice UpdateOrderAndPaymentByReservationNumber with statusCode={paymentStatus.StatusCode} for paymentid ={paymentId}",
                            null);
                    }
                }
                else if (paymentStatus.ErrorCode ==
                         (int)HekmatPaymentStatusResultErrorCodeEnum.ReservationNotFound)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                        $"failed in updating HekmatIpg in HekmatIpgservice UpdateOrderAndPaymentByReservationNumber with paymentid ={paymentId} , reservation number Not found in IPG",
                        null);
                    /*
                     *وضعیت سفارش در این حالت نباید تغییر کند
                     * سفارش های با وضعیت ثبت اولیه  قبل از بررسی یارانه برای سفارش جدید، به وضعیت سفارش لغو سیستمی
                     * تغییر می کنند
                     *
                     */
                    await _paymentService.UpdatePaymentStatusAsync(paymentId,
                        PaymentStatusTypesEnum.Cancel, PaymentMethodTypesEnum.Hekmat, paymentStatus.Message,
                        paymentStatus.RefrenceNumber, true);
                }
                else
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                        $"failed in updating HekmatIpg in HekmatIpgservice UpdateOrderAndPaymentByReservationNumber with paymentid ={paymentId} , paymentStatus ErrorCode in IPG is = {paymentStatus.ErrorCode} and payment IPg message is = {paymentStatus.Message}",
                        null);
                }

                return true;
            }

            _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                $"failed in updating HekmatIpg in HekmatIpgservice UpdateOrderAndPaymentByReservationNumber with paymentid ={paymentId} , paymentStatus is null",
                null);
            return false;
        }

        /// <summary>
        /// پرداخت های حکمت کاربر که در وضعیت پرداخت
        /// ارسال به درگاه یا منتظر تایید پرداخت باشد 
        /// از طریق سرویس درگاه پرداخت حکمت تعیین تکلیف می شود
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public async Task<bool> UpdateUserHekmatTypeOrderAndPaymentStatusAsync(int userId)
        {
            bool res = true;
            var payments = await _paymentService.GetUserPendingHekmatPayment(userId).Select(x => new { x.OrderId, paymentId = x.Id, x.ReservationNumber }).ToListAsync();
            foreach (var payment in payments)
            {
                res = res & await UpdateOrderAndPaymentStatusByPaymentIdAsync(payment.paymentId, payment.OrderId);
            }
            return res;
        }

        /// <summary>
        /// تراکنش های حکمت
        /// </summary>
        /// <param name="hekmatTransactionFile"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<bool> BulkCopyHekmatApprovedDataAsync(IFormFile hekmatTransactionFile, string title)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var addAppDocumentResult = await AddAppDocumentAsync(hekmatTransactionFile, DocumentTypeEnum.HekmatApprovedTransaction);
                var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);
                var columns = new[]
                {
                    "HekmatCardNo", "Price", "TerminalId", "StoreName", "TransactionDate", "ProcessDate",
                    "InstallmentCount", "TransactionState", "InstallmentGroup", "InstallmentTrackingCode", "TraceNo",
                    "Token"
                };
                var res = await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatApprovedTransaction", filePath, 1, columns,
                    addAppDocumentResult.appDocument.Id);
                await _hekmatApprovedTransactionService.DeleteDuplicateHekmatApprovedTransactionsAsync();
                await _hekmatApprovedTransactionService.UpdateModifiedHekmatTransactionAsync();
                await transaction.CommitAsync();
                return res;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, "BulkCopyHekmatApprovedDataAsync",
                    $" بروز خطا در فرایند بارگذاری فایل اکسل تراکنش های حکمت ", e);
                return false;
            }
        }

        private async Task<(AppDocument appDocument, bool isSuccess, string message)> AddAppDocumentAsync(IFormFile formFile, DocumentTypeEnum documentTypeEnum)
        {
            return await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.HekmatApprovedTransaction, (int)documentTypeEnum,
                null, (int)AppEntityEnum.HekmatApprovedTransaction, formFile);
        }

        /// <summary>
        /// لغو و اصلاح حکمت
        /// </summary>
        /// <param name="hekmatRefundFile"></param>
        /// <returns></returns>
        public async Task<bool> BulkCopyHekmatRefundedTransactionsDataAsync(IFormFile hekmatRefundFile)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var addAppDocumentResult = await AddAppDocumentAsync(hekmatRefundFile, DocumentTypeEnum.HekmatRefundedTransactions);
                var filePath = _appDocumentService.GetFilePath(addAppDocumentResult.appDocument.Id);

                #region تراکنش های لغوی حکمت
                var hekmatCanceledTransactionColumns = new[]
                {
               "CartNumber", "RecieverCode", "StoreName", "TerminalNumber", "SendTransactionPrice",
                "TransactionDate", "ProcessDate", "CancleDate", "FullAnnouncedInstallmentPrice", "RefundPrice"
                };

                await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatCanceledTransaction", filePath, 0, hekmatCanceledTransactionColumns, addAppDocumentResult.appDocument.Id);
                #endregion
                #region تراکنش های لغو بدون تاثیر حکمت
                var hekmatEffectLessCancelColumns = new[]
                {"CartNumber", "TerminalNumber", "TerminalRotationRow", "TransactionDate",
                "ModifiedTransactionPrice", "StoreName"};
                await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatEffectlessCanceledTransaction", filePath, 1, hekmatEffectLessCancelColumns, addAppDocumentResult.appDocument.Id, 0);
                #endregion
                #region تراکنش های اصلاحی حکمت
                var hekmatModifiedColumns = new[]
                {
               "CartNumber", "RecieverCode", "StoreName", "TerminalNumber", "SendTransactionPrice",
                "FinalTransactionPrice", "TransactionDate", "ProcessDate", "ModifiedDate", "AnnouncedInstallmentCount",
                "RestOfInstallmentAnnounced", "RefundPrice"};
                await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatModifiedTransaction", filePath, 2, hekmatModifiedColumns, addAppDocumentResult.appDocument.Id);
                #endregion
                #region تراکنش های اصلاحی بدون تاثیر حکمت
                var hekmatEffectLessModifiedTransactionColumn = new[]
                {
                    "CartNumber", "TerminalNumber", "TerminalRotationRow", "TransactionDate","ModifiedTransactionPrice", "StoreName"
                };
                await _sqlBulkCopyExcelToDb.BulkInsertFromExcelOrCsvAsync("shp.HekmatEffectlessModifiedTransaction", filePath, 3, hekmatEffectLessModifiedTransactionColumn, addAppDocumentResult.appDocument.Id, 0);
                #endregion

                await _refundDetailService.UpdateHekmatRefundDetailRefundAmountAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, "BulkCopyHekmatApprovedDataAsync",
                    $" بروز خطا در فرایند بارگذاری فایل اکسل تراکنش های حکمت ", e);
                return false;
            }
        }

        public HekmatIPG.HekmatIPGClient CreateHekmatIpgService()
        {
            var httpHandler = new HttpClientHandler();


            //برای عبور از پراکسی این قطعه کد را فعال می کنیم در حالت توسعه
            //نیاز است نام کاربری و رمز عبور مربوط به پراکسی شخص وارد شود
            if (_environment.IsDevelopment())
            {
                //var proxy = new WebProxy("http://192.168.55.2:8080");
                //proxy.Credentials = new NetworkCredential("ir94", "artin");
                //httpHandler.Proxy = proxy;
                //httpHandler.UseProxy = true;
            }

            var grpcWebHandler = new GrpcWebHandler(httpHandler);
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var chanel = GrpcChannel.ForAddress(_hekmatPaymentConfiguration.EndpointAddress,
                new GrpcChannelOptions { HttpHandler = grpcWebHandler });

            return new HekmatIPG.HekmatIPGClient(chanel);
        }

        public async Task<PaymentRequestResultModel> RequestPaymentInHekmatIpgAsync(string reservationNumber,
            int amount, int installmentCount, string carNumber = null,
            string additionalData = null, bool isHybrid = false)
        {
            try
            {

                int merchantId;
                int terminalId;

                if (isHybrid)
                {
                    merchantId = int.Parse(CryptographyHelper.AesDecrypt(_configuration.GetSection("HybridPaymentConfiguration:MerchantId").Get<string>()));
                    terminalId = int.Parse(CryptographyHelper.AesDecrypt(_configuration.GetSection("HybridPaymentConfiguration:TerminalId").Get<string>()));

                    if (merchantId == 0 || terminalId == 0)
                        throw new Exception("HybridPaymentConfiguration:MerchantId Or " +
                                            "HybridPaymentConfiguration:TerminalId Dose not have Valid Value");

                }
                else if (await _additionalUserPropService.CheckIsHekmatCurrentUserCustomerClub())
                {
                    merchantId = Convert.ToInt32(_hekmatPaymentConfiguration.MerchantId);
                    terminalId = Convert.ToInt32(_hekmatPaymentConfiguration.TerminalId);
                }
                else
                {
                    merchantId = int.Parse(CryptographyHelper.AesDecrypt(_configuration.GetSection("PublicCustomerClubHekmatPaymentConfiguration:MerchantId").Get<string>()));
                    terminalId = int.Parse(CryptographyHelper.AesDecrypt(_configuration.GetSection("PublicCustomerClubHekmatPaymentConfiguration:TerminalId").Get<string>()));

                    if (merchantId == 0 || terminalId == 0)
                        throw new Exception("PublicCustomerClubHekmatPaymentConfiguration:MerchantId Or " +
                                            "PublicCustomerClubHekmatPaymentConfiguration:TerminalId Dose not have Valid Value");

                }

                var hekmatIpgService = CreateHekmatIpgService();


                var data = $"{merchantId};{terminalId};{reservationNumber};{amount};{installmentCount}";

                int? defaultCashPercentage = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceHybridDefaultCashPercentage);

                if (isHybrid && defaultCashPercentage.Value == 0)
                    defaultCashPercentage = null;
                else if (isHybrid && defaultCashPercentage.Value > 0)
                    data += data + $";{defaultCashPercentage}";

                var rsa = new RSACryptoServiceProvider();


                var privateKey = !isHybrid
                    ? _hekmatPaymentConfiguration.PrivateKey
                    : CryptographyHelper.AesDecrypt(_configuration.GetSection("HybridPaymentConfiguration:PrivateKey").Get<string>());

                rsa.ImportFromPem(privateKey.ToCharArray());


                byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), SHA256.Create());


                string returnUrl = $"{_applicationConfiguration.BaseUrl}hekmatpayment/VerifyPaymentTransaction";

                if (_environment.IsDevelopment() && isHybrid)
                {

                    returnUrl = $"https://localhost:2202/hekmatpayment/VerifyHybridPaymentTransaction/";
                }
                else if (isHybrid)
                {
                    returnUrl = _applicationConfiguration.BaseUrl + "hekmatpayment/VerifyHybridPaymentTransaction/";
                }

                return isHybrid
                    ? hekmatIpgService.HybridPaymentRequest(new PaymentRequestModel()
                    {
                        Amount = amount,
                        InstallmentCount = installmentCount,
                        TerminalId = terminalId,
                        CardNumber = carNumber,
                        LocalDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        ReturnUrl = returnUrl,
                        ReservationNumber = reservationNumber,
                        SignData = Convert.ToBase64String(signedData),
                        MerchantId = merchantId,
                        AdditionalData = additionalData,
                        UseFormula = !isHybrid,
                        CashPaymentPercentage = defaultCashPercentage,
                    })
                    : hekmatIpgService.PaymentRequest(new PaymentRequestModel
                    {
                        Amount = amount,
                        InstallmentCount = installmentCount,
                        TerminalId = terminalId,
                        CardNumber = carNumber,
                        LocalDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        ReturnUrl = $"{_applicationConfiguration.BaseUrl}hekmatpayment/VerifyPaymentTransaction",
                        ReservationNumber = reservationNumber,
                        SignData = Convert.ToBase64String(signedData),
                        MerchantId = merchantId,
                        AdditionalData = additionalData
                    });
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order", "بروز خطا در پرداخت درگاه حکمت-> hekmatService.HekmatRequest", e);
                return null;
            }

        }
    }
}
