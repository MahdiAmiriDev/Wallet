using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.Implementations;
using YashilPayment.Core.DTO;
using YashilPayment.Core.Enums;
using YashilPayment.Core.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Order = Yashil.Core.Entities.Order;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class VibeService : IVibeService
    {
        private readonly IYashilLogger<object> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPaymentService _paymentService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly IOrderService _orderService;
        private readonly VibeConfiguration _vibeConfiguration;
        private readonly IWebHelper _webHelper;
        private readonly YashilAppDbContext _walletContext;
        private readonly IWalletSettlementService _walletSettlement;

        public VibeService(IYashilLogger<object> logger,
            IHttpClientFactory httpClientFactory, IPaymentService paymentService,
            PaymentTransactionUtil paymentTransactionUtil, IOrderService orderService,
             VibeConfiguration vibeConfiguration, IWebHelper webHelper,
             YashilAppDbContext walletContext, IWalletSettlementService walletSettlement)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _paymentService = paymentService;
            _paymentTransactionUtil = paymentTransactionUtil;
            _orderService = orderService;
            _vibeConfiguration = vibeConfiguration;
            _webHelper = webHelper;
            _walletContext = walletContext;
            _walletSettlement = walletSettlement;
        }

        public async Task<CreateVibeOrderResponseDto> CreateOrderAsync(PaymentInformation paymentInformation, Order order)
        {


            if (_vibeConfiguration == null ||
                string.IsNullOrEmpty(_vibeConfiguration.CallBackUrl) ||
                string.IsNullOrEmpty(_vibeConfiguration.ApiKey) ||
                string.IsNullOrEmpty(_vibeConfiguration.Url))
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.InvalidModel, nameof(CreateOrderAsync),
                    $"vibe configuration is not set correctly: value is null {_vibeConfiguration == null} and props value is" +
                    $"{JsonConvert.SerializeObject(_vibeConfiguration ?? new VibeConfiguration())}", null);


                return new CreateVibeOrderResponseDto(string.Empty, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
            }

            var mixed = await _walletContext.WalletReservation.AsNoTracking().AnyAsync(x =>
                x.PaymentId == paymentInformation.PaymentId && !x.Deleted
                && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved);
            if (mixed)
            {
                // یک تلاش Mixed تنها یک بار مجاز به درخواست CreateOrder از Vibe است.
                var claimed = await _walletContext.Payment.Where(x =>
                    x.Id == paymentInformation.PaymentId && x.OrderId == order.Id
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                    && x.Total == paymentInformation.Total && (x.Token == null || x.Token == ""))
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.PaymentStatusType,
                        (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
                if (claimed != 1)
                    return new CreateVibeOrderResponseDto(string.Empty,
                        "درخواست Vibe برای این تلاش قبلاً ارسال شده یا وضعیت آن نامشخص است.");
            }

            try
            {
                var jsonBody = JsonSerializer.Serialize(new CreateVibeOrderRequestDto()
                {
                    OrderId = paymentInformation.ReservationNumber,
                    CartAmount = (int)paymentInformation.Total,
                    CallbackUrl = _vibeConfiguration.CallBackUrl,
                    Data = PrepareVibeOrderData(order),
                    Installments = _vibeConfiguration.InstallmentCount
                });

                using var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.Add("X-API-Key", CryptographyHelper.AesDecrypt(_vibeConfiguration.ApiKey));

                var apiResponse = await httpClient.PostAsync(new Uri(_vibeConfiguration.Url), new StringContent(jsonBody, Encoding.UTF8, "application/json"));


                if (apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var errorDetail = await apiResponse.Content.ReadAsStringAsync();

                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                        (int)Events.InvalidModel, nameof(CreateOrderAsync), errorDetail, null);

                    return new CreateVibeOrderResponseDto(string.Empty, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
                }

                var stringResponse = await apiResponse.Content.ReadAsStringAsync();

                var vibeOrderResponse = JsonSerializer.Deserialize<CreateVibeOrderResponseDto>(stringResponse);

                var uri = new Uri(vibeOrderResponse.PaymentUrl);

                var queryParams = QueryHelpers.ParseQuery(uri.Query);

                var token = queryParams["t"].ToString();

                //بررسی می کنیم که توکن حتما منحصر به فرد باشد
                if (await _paymentService.FindByToken(token, PaymentMethodTypesEnum.Vibe, true).AnyAsync())
                {
                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Warning,
                        (int)Events.InvalidModel, nameof(CreateOrderAsync),
                        $"duplicated vibe token error value is: {token}", null);

                    return new CreateVibeOrderResponseDto(string.Empty, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
                }

                //بررسی می کنیم حتما بروزرسانی توکن انجام شده باشد چون بعدا بر اساس آن باید پرداخت را پیدا کنیم.
                if ((await _paymentService.UpdateTokenAsync(token, paymentInformation.PaymentId)) != 1)
                {
                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Warning,
                        (int)Events.InvalidModel, nameof(CreateOrderAsync),
                        $"token value cannot be updated value is: {token}", null);

                    return new CreateVibeOrderResponseDto(string.Empty, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
                }

                if (mixed)
                {
                    // ارسال به درگاه فقط پس از ذخیره Token و بازگرداندن وضعیت به Send مجاز است.
                    var ready = await _walletContext.Payment.Where(x =>
                        x.Id == paymentInformation.PaymentId && x.Token == token
                        && x.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.PaymentStatusType,
                            (int)PaymentStatusTypesEnum.SendToPaymentGateway));
                    if (ready != 1)
                        return new CreateVibeOrderResponseDto(string.Empty,
                            "وضعیت تلاش Vibe پس از ثبت Token معتبر نیست.");
                }

                //بروزرسانی وضعیت سفارش به در حال پردازش
                //در این مرحله پرداخت هم در وضعیت ارسال به درگاه است
                await _orderService.UpdateOrderStatusAsync(order.Id,
                    OrderStatusTypesEnum.Processing, true);

                return vibeOrderResponse;
            }
            catch (Exception e)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.InvalidModel, nameof(CreateOrderAsync)
                    , $"بروز استثنا به هنگام ایجاد درخواست پرداخت درگاه وایب", e);

                return new CreateVibeOrderResponseDto(string.Empty, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
            }

        }

        public async Task<VerifyVibeOrderResponseDto> VerifyVibeOrderAsync(string refNumber, string status)
        {
            //نکته:
            //در درگاه وایب شماره
            //ref و res
            //هر دو یکی می باشند و برابر است با مقداری که از سمت پذیرنده که اپلیکیشن ما باشد
            //به درگاه ارسال شده است که معادل یک مقدار ثابت + شناسه 
            //payment است

            var verifyPaymentViewModel = new VerifyPaymentViewModel()
            {
                PaymentMethodType = PaymentMethodTypesEnum.Vibe
            };

            var mixedPayment = await _paymentService.FindByToken(refNumber, PaymentMethodTypesEnum.Vibe, true)
                .FirstOrDefaultAsync();
            if (mixedPayment != null)
            {
                var reservation = await _walletContext.WalletReservation.AsNoTracking()
                    .FirstOrDefaultAsync(x => !x.Deleted && x.PaymentId == mixedPayment.Id
                        && x.ApplicationId == mixedPayment.ApplicationId);
                if (reservation != null)
                    return await VerifyMixedAsync(mixedPayment, reservation, refNumber, status);
            }

            if (!_paymentTransactionUtil.PaymentTransactionStatus.TryAdd(refNumber, true))
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException, nameof(VerifyVibeOrderAsync), $" درخواست تایید پرداخت دوبار فراخوانی شده است  - refnumber :  {refNumber}",
                    null);

                return new VerifyVibeOrderResponseDto()
                {
                    verifyPaymentViewModel = verifyPaymentViewModel
                };
            }

            var payment = await _paymentService.FindByToken(refNumber, PaymentMethodTypesEnum.Vibe, true).FirstOrDefaultAsync();

            if (payment == null)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException, nameof(VerifyVibeOrderAsync), $"payment مرتبط با  toekn : یافت نشد token دریافتی {refNumber}",
                    null);

                return new VerifyVibeOrderResponseDto();
            }

            //اگر در وضعیت  پرداخت در حالت ارسال به درگاه نبود
            //یا از زمان ایجاد پرداخت بیش از سی دقیقه گذشته باشد یعنی نباید این سفارش رو تایید کرد
            if (payment.PaymentStatusType != (int)PaymentStatusTypesEnum.SendToPaymentGateway || payment.CreationDate.AddMinutes(30) < DateTime.Now)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.InvalidModel, nameof(CreateOrderAsync),
                    $"وضعیت پرداخت در حالت ارسال به درگاه نبوده است یا بیش از سی دقیقه از ثبت سفارش گذشته است شناسه پرداخت : {payment.Id}", null);

                await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                    PaymentMethodTypesEnum.Vibe, "پرداخت ناموفق", true);


                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                    OrderStatusTypesEnum.UnsuccessfulPayment, true);

                return new VerifyVibeOrderResponseDto();
            }

            //var application = await _applicationService.GetAsync(payment.ApplicationId, true);
            var baseUrl = _webHelper.GetBaseUrl(true);
            verifyPaymentViewModel.OrderListUrl = baseUrl + "/Order";

            if (!string.Equals(
                    VerifyVibePaymentStatusEnum.Ok.ToString(),
                    status,
                    StringComparison.OrdinalIgnoreCase))
            {

                await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                      PaymentMethodTypesEnum.Vibe, "انصراف کاربر از پرداخت", true);


                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                    OrderStatusTypesEnum.UnsuccessfulPayment, true);


                verifyPaymentViewModel.Message = "انصراف کاربر از پرداخت";
                verifyPaymentViewModel.RefNum = refNumber;
                verifyPaymentViewModel.ResNum = payment.ReservationNumber;
                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;

                return new VerifyVibeOrderResponseDto(true)
                {
                    verifyPaymentViewModel = verifyPaymentViewModel
                };
            }

            try
            {


                if (_vibeConfiguration == null ||
                    string.IsNullOrEmpty(_vibeConfiguration.ApiKey) ||
                    string.IsNullOrEmpty(_vibeConfiguration.Url))
                {
                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                        (int)Events.InvalidModel, nameof(CreateOrderAsync),
                        $"vibe configuration is not set correctly: value is null {_vibeConfiguration == null} and props value is" +
                        $"{JsonConvert.SerializeObject(_vibeConfiguration ?? new VibeConfiguration())}", null);


                    return new VerifyVibeOrderResponseDto();
                }

                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                    PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                    PaymentMethodTypesEnum.Vibe, "در انتظار تایید.", true);

                using var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.Add("X-API-Key", CryptographyHelper.AesDecrypt(_vibeConfiguration.ApiKey));

                var apiResponse = await httpClient.PostAsync(new Uri(_vibeConfiguration.VerifyUrl),
                    new StringContent(JsonConvert.SerializeObject(new { ref_id = refNumber }), Encoding.UTF8, "application/json"));

                var responseString = await apiResponse.Content.ReadAsStringAsync();

                if (apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {

                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                        (int)Events.PaymentException, nameof(VerifyVibeOrderAsync), responseString, null);


                    await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                        PaymentMethodTypesEnum.Vibe, "بروز خطا به هنگام تایید تراکنش.", true);

                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment, true);


                    verifyPaymentViewModel.Message = "بروز خطا به هنگام تایید تراکنش لطفا با مدیر سیستم تماس بگیرید.";
                    verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    verifyPaymentViewModel.RefNum = refNumber;
                    verifyPaymentViewModel.ResNum = payment.ReservationNumber;

                    return new VerifyVibeOrderResponseDto(true)
                    {
                        verifyPaymentViewModel = verifyPaymentViewModel
                    };
                }


                var verifyResult = JsonConvert.DeserializeObject<VerifyVibeOrderResponseDto>(responseString);

                if (verifyResult == null)
                {
                    verifyPaymentViewModel.Message =
                        "پاسخ نامعتبر از درگاه هنگام تایید تراکنش.";
                    await _paymentService.UpdatePaymentStatusAsync(
                        payment.Id,
                        PaymentStatusTypesEnum.Unsuccessful,
                        PaymentMethodTypesEnum.Vibe,
                        verifyPaymentViewModel.Message,
                        true);

                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment, true);


                    verifyPaymentViewModel.PaymentStatusType =
                        PaymentStatusTypesEnum.Unsuccessful;
                    verifyPaymentViewModel.RefNum = refNumber;
                    verifyPaymentViewModel.ResNum = payment.ReservationNumber;

                    return new VerifyVibeOrderResponseDto(true)
                    {
                        verifyPaymentViewModel = verifyPaymentViewModel
                    };
                }

                if (verifyResult is { Status: false })
                {
                    verifyPaymentViewModel.Message = "عدم تایید تراکنش از جانب درگاه";
                    await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                        PaymentMethodTypesEnum.Vibe, verifyPaymentViewModel.Message, true);

                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment, true);


                    verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    verifyPaymentViewModel.RefNum = refNumber;
                    verifyPaymentViewModel.ResNum = payment.ReservationNumber;

                    return new VerifyVibeOrderResponseDto(true)
                    {
                        verifyPaymentViewModel = verifyPaymentViewModel
                    };
                }
                verifyPaymentViewModel.Message = "تراکنش با موفقیت تایید شد.";
                await _orderService.UpdateOrderStatusAsync(payment.OrderId, OrderStatusTypesEnum.Complete, true);
                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                      PaymentStatusTypesEnum.Paid,
                      PaymentMethodTypesEnum.Vibe,
                      verifyPaymentViewModel.Message, true);


                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                verifyPaymentViewModel.RefNum = refNumber;
                verifyPaymentViewModel.ResNum = payment.ReservationNumber;

                return new VerifyVibeOrderResponseDto(true)
                {
                    verifyPaymentViewModel = verifyPaymentViewModel
                };

            }
            catch (Exception e)
            {

                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException, nameof(VerifyVibeOrderAsync), "بروز خطا به هنگام تایید تراکنش", e);

                await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                    PaymentMethodTypesEnum.Vibe, "بروز خطا به هنگام تایید تراکنش.", true);


                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                    OrderStatusTypesEnum.UnsuccessfulPayment, true);

                return new VerifyVibeOrderResponseDto();
            }
        }

        /// <summary>تلاش Vibe ترکیبی را فقط پس از نتیجه قطعی درگاه با کیف پول تسویه می‌کند.</summary>
        private async Task<VerifyVibeOrderResponseDto> VerifyMixedAsync(
            Yashil.Core.Entities.Payment payment, Yashil.Core.Entities.WalletReservation reservation,
            string refNumber, string status)
        {
            // قفل DB، callbackهای هم‌زمان چند سرور را برای یک Payment سری می‌کند.
            await using var lockConnection = new SqlConnection(_walletContext.Database.GetConnectionString());
            await lockConnection.OpenAsync();
            await using var lockTransaction = await lockConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await using var lockCommand = new SqlCommand(
                "DECLARE @result int; EXEC @result = sys.sp_getapplock "
                + "@Resource = @resource, @LockMode = 'Exclusive', @LockOwner = 'Transaction', "
                + "@LockTimeout = 15000; SELECT @result;",
                lockConnection, (SqlTransaction)lockTransaction);
            lockCommand.Parameters.AddWithValue("@resource", $"WalletMixed:Verify:Payment:{payment.Id}");
            if (Convert.ToInt32(await lockCommand.ExecuteScalarAsync()) < 0)
                return new VerifyVibeOrderResponseDto();
            payment = await _walletContext.Payment.AsNoTracking().FirstOrDefaultAsync(x => x.Id == payment.Id);
            reservation = await _walletContext.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reservation.Id);
            if (payment == null || reservation == null || payment.Deleted || reservation.Deleted
                || reservation.PaymentId != payment.Id
                || reservation.ApplicationId != payment.ApplicationId)
                return new VerifyVibeOrderResponseDto();
            var view = new VerifyPaymentViewModel
            {
                PaymentMethodType = PaymentMethodTypesEnum.Vibe,
                RefNum = refNumber,
                ResNum = payment.ReservationNumber,
                OrderListUrl = _webHelper.GetBaseUrl(true) + "/Order"
            };
            var validOrder = await _walletContext.Order.AsNoTracking().AnyAsync(x =>
                x.Id == payment.OrderId && x.ApplicationId == payment.ApplicationId
                && !x.Deleted && x.UserId == reservation.UserId);
            if (payment.PaymentMethodType != (int)PaymentMethodTypesEnum.Vibe
                || !validOrder || reservation.Amount <= 0m)
                return new VerifyVibeOrderResponseDto();

            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                return await FinishMixedCaptureAsync(payment, reservation, view);

            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful
                || payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Cancel
                || payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Void)
            {
                view.PaymentStatusType = (PaymentStatusTypesEnum)payment.PaymentStatusType;
                view.Message = "پرداخت درگاه ناموفق است.";
                return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
            }

            if (payment.PaymentStatusType != (int)PaymentStatusTypesEnum.SendToPaymentGateway
                && payment.PaymentStatusType != (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)
                return new VerifyVibeOrderResponseDto();

            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)
            {
                view.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                view.Message = "نتیجه Verify قبلی نامشخص است؛ بررسی عملیاتی لازم است.";
                return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
            }

            var claimedVerify = await _walletContext.Payment.Where(x => x.Id == payment.Id
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.PaymentStatusType,
                    (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
            if (claimedVerify != 1)
                return new VerifyVibeOrderResponseDto();

            // حتی Callback ناموفق باید با پاسخ Verify تعیین تکلیف شود؛ نتیجه نامعلوم Reserve را آزاد نمی‌کند.
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("X-API-Key",
                    CryptographyHelper.AesDecrypt(_vibeConfiguration.ApiKey));
                var response = await client.PostAsync(new Uri(_vibeConfiguration.VerifyUrl),
                    new StringContent(JsonConvert.SerializeObject(new { ref_id = refNumber }),
                        Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    view.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                    view.Message = "نتیجه تأیید درگاه نامشخص است؛ پرداخت در حال بررسی است.";
                    return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
                }
                var result = JsonConvert.DeserializeObject<VerifyVibeOrderResponseDto>(
                    await response.Content.ReadAsStringAsync());
                if (result == null)
                {
                    view.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                    view.Message = "پاسخ تأیید درگاه نامعتبر است؛ پرداخت در حال بررسی است.";
                    return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
                }
                if (!result.Status)
                {
                    var failedPayment = await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                        PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.Vibe,
                        "درگاه پرداخت را تایید نکرد", true);
                    if (failedPayment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                        return await FinishMixedCaptureAsync(payment, reservation, view);
                    var release = await _walletSettlement.ReleaseAsync(
                        new WalletReleaseRequestDto { ReservationId = reservation.Id });
                    if (!release.IsSuccess)
                    {
                        view.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                        view.Message = "تعیین تکلیف Reservation نیازمند بررسی است.";
                        return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
                    }
                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment, true);
                    view.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    view.Message = "درگاه پرداخت را تأیید نکرد.";
                    return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
                }

                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                    PaymentStatusTypesEnum.Paid, PaymentMethodTypesEnum.Vibe,
                    "پرداخت درگاه تایید شد", true);
                return await FinishMixedCaptureAsync(payment, reservation, view);
            }
            catch (Exception error)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException, nameof(VerifyMixedAsync),
                    "نتیجه Verify یا ثبت محلی Mixed نامشخص است", error);
                var paid = await _walletContext.Payment.AsNoTracking().AnyAsync(x =>
                    x.Id == payment.Id && x.ApplicationId == payment.ApplicationId
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid);
                if (paid)
                {
                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.IncompletePayment, true);
                    view.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                    view.Message = "پرداخت درگاه ثبت شد؛ Capture کیف پول باید بازیابی شود.";
                }
                else
                {
                    view.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                    view.Message = "نتیجه پرداخت در حال بررسی است.";
                }
                return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
            }
        }

        /// <summary>Capture تکرارپذیر را اجرا و تکمیل Order را به بدهی صفر محدود می‌کند.</summary>
        private async Task<VerifyVibeOrderResponseDto> FinishMixedCaptureAsync(
            Yashil.Core.Entities.Payment payment, Yashil.Core.Entities.WalletReservation reservation,
            VerifyPaymentViewModel view)
        {
            var capture = await _walletSettlement.CaptureAsync(new WalletCaptureRequestDto
            {
                ReservationId = reservation.Id,
                PaymentId = payment.Id,
                IdempotencyKey = $"WalletCapture:Payment:{payment.Id}"
            });
            if (!capture.IsSuccess)
            {
                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                    OrderStatusTypesEnum.IncompletePayment, true);
                view.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                view.Message = "درگاه پرداخت شد؛ ثبت سهم کیف پول در حال بازیابی است.";
                return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
            }
            var outstanding = await _orderService.CalcAmountForPayAsync((int)PaymentMethodTypesEnum.Vibe,
                payment.OrderId);
            await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                outstanding.Total == 0m ? OrderStatusTypesEnum.Complete
                    : OrderStatusTypesEnum.IncompletePayment, true);
            view.PaymentStatusType = PaymentStatusTypesEnum.Paid;
            view.Message = outstanding.Total == 0m
                ? "پرداخت درگاه و کیف پول تکمیل شد." : "پرداخت جزئی ثبت شد؛ مانده سفارش باقی است.";
            return new VerifyVibeOrderResponseDto(true) { verifyPaymentViewModel = view };
        }

        /// <summary>
        /// این متد جهت آماده کردن داده های مورد نیاز برای جزییات خرید از درگاه
        /// وایب کاربرد دارد و بر اساس سفارش این داده های را آماده می کند
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private VibeOrderData PrepareVibeOrderData(Order order)
        {
            var total = order.TotalInclSubsidy is > 0 ? order.TotalInclSubsidy : order.TotalExclSubsidy;
            var tax = order.TaxInclSubsidy is > 0 ? order.TaxInclSubsidy : order.TaxExclSubsidy;

            return new VibeOrderData()
            {
                GoodsAmount = (double)(total - tax),
                Tax = tax,
                OrderItems = order.OrderItem.Where(x => !x.InverseParent.Any()).Select(oi => new VibeOrderItem
                {
                    Id = oi.Id.ToString(),
                    Name = string.IsNullOrEmpty(oi.ProductSalesPlanning.Title)
                        ? oi.ProductSalesPlanning.Product.Title
                        : oi.ProductSalesPlanning.Title,
                    Price = (double)oi.BasePrice,
                    Quantity = oi.Quantity,
                    Discount = oi.DiscountAmountExclTax.HasValue ? (double)oi.DiscountAmountExclTax : 0


                }).ToList()
            };
        }

    }
}
