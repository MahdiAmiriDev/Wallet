using YashilPayment.Core.DTO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using YashilPayment.Core.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using System.Linq;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using System.IO;
using Org.BouncyCastle.OpenSsl;


namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class IranKishPaymentService : IIranKishPaymentService
    {
        private readonly IConfiguration _configuration;
        private IranKishPaymentConfiguration _iranKishPaymentConfiguration;
        private readonly IHostEnvironment _environment;
        protected readonly ApplicationConfiguration ApplicationConfiguration;
        private readonly IYashilLogger<HybridPayment> _logger;

        public IranKishPaymentService(IConfiguration configuration, IHostEnvironment environment, ApplicationConfiguration applicationConfiguration, IYashilLogger<HybridPayment> logger)
        {
            _configuration = configuration;
            _environment = environment;
            ApplicationConfiguration = applicationConfiguration;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync(long amount, string resNumber)
        {
            try
            {

                if (amount <= 0 || string.IsNullOrEmpty(resNumber))
                    return string.Empty;

                BindIranKishPaymentConfigurationData();


                IranKisPaymentTokenRequestDto requestModel = new()
                {
                    AuthenticationEnvelopeDto = GetAuthenticationEnvelope(amount),
                    RequestDto = new RequestDto()
                    {
                        TerminalId = _iranKishPaymentConfiguration.DecryptedTerminalId,
                        AcceptorId = _iranKishPaymentConfiguration.DecryptedMerchantId,
                        Amount = amount,
                        RequestId = resNumber.Replace("-", ""),
                        RequestTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        TransactionType = "Purchase",
                        RevertUri = "https://localhost:2202"
                    }
                };

                var httpHandler = new HttpClientHandler();

                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                //برای عبور از پراکسی این قطعه کد را فعال می کنیم در حالت توسعه
                //نیاز است نام کاربری و رمز عبور مربوط به پراکسی شخص وارد شود
                if (_environment.IsDevelopment())
                {
                    var proxy = new WebProxy("http://192.168.55.2:8080");
                    proxy.Credentials = new NetworkCredential("ir94", "artin");
                    httpHandler.Proxy = proxy;
                    httpHandler.UseProxy = true;
                }

                var client = new HttpClient(httpHandler);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://ikc.shaparak.ir/api/v3/tokenization/make");

                var data = JsonSerializer.Serialize(requestModel);

                request.Content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var stringResponse = await response.Content.ReadAsStringAsync();

                var iranKishTokenDto = JsonSerializer.Deserialize<GenerateIranKishTokenResponseDto>(stringResponse);

                if (iranKishTokenDto == null || string.IsNullOrEmpty(iranKishTokenDto?.Result?.Token))
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, nameof(GenerateTokenAsync), stringResponse, null);

                    return string.Empty;
                }

                return iranKishTokenDto.Result.Token;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    nameof(GenerateTokenAsync), "error occurred in generating iranKishToken", e);
                return string.Empty;
            }

        }

        /// <summary>
        /// این متد جهت دریافت تنظیمات مربوط به دریگاه پرداخت ایران کیش کاربرد دارد
        /// اطلاعات را از
        /// appsetting
        /// می خواند به دلیل این که به صورت رمزنگاری شده نگه داری می شوند
        /// آن ها را به حالت اولیه برمیگردانیم و در یک
        /// feild
        /// نگه داری می کنیم تا در طول
        /// lifeTime
        /// درخواست از آن بخوانیم به جای این که هر بار این فرآیند را تکرار کنیم.
        /// </summary>
        /// <returns></returns>
        public void BindIranKishPaymentConfigurationData()
        {
            var iranKishPaymentConfiguration = _configuration.GetSection("IranKishPaymentConfiguration").Get<IranKishPaymentConfiguration>();

            if (iranKishPaymentConfiguration == null ||
                string.IsNullOrEmpty(iranKishPaymentConfiguration.Password) ||
                string.IsNullOrEmpty(iranKishPaymentConfiguration.MerchantId) ||
                string.IsNullOrEmpty(iranKishPaymentConfiguration.PublicKey) ||
                string.IsNullOrEmpty(iranKishPaymentConfiguration.TerminalId))
                throw new Exception("Invalid iranKishPaymentConfiguration in appsetting.json");

            _iranKishPaymentConfiguration = new IranKishPaymentConfiguration()
            {
                DecryptedMerchantId = CryptographyHelper.AesDecrypt(iranKishPaymentConfiguration.MerchantId),
                DecryptedPassword = CryptographyHelper.AesDecrypt(iranKishPaymentConfiguration.Password),
                DecryptedPublicKey = CryptographyHelper.AesDecrypt(iranKishPaymentConfiguration.PublicKey),
                DecryptedTerminalId = CryptographyHelper.AesDecrypt(iranKishPaymentConfiguration.TerminalId)
            };
        }

        /// <summary>
        /// دریافت تنظیمات درگاه ایران کیش
        /// </summary>
        /// <returns></returns>
        public IranKishPaymentConfiguration GetIranKishPaymentConfiguration()
        {
            BindIranKishPaymentConfigurationData();
            return _iranKishPaymentConfiguration;
        }


        /// <summary>
        /// این متد جهت دریافت پاکت احراز هویت که در ایجاد توکن
        /// برای درگاه پرداخت ایران کیش کاربرد دارد استفاده می شود 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private AuthenticationEnvelopeDto GetAuthenticationEnvelope(long amount)
        {
            var paymentInfo = _iranKishPaymentConfiguration.DecryptedTerminalId +
                _iranKishPaymentConfiguration.DecryptedPassword + amount.ToString().PadLeft(12, '0') + "00";

            var result = GetPaymentDataAndIv(paymentInfo);

            return new AuthenticationEnvelopeDto()
            {
                Data = result.data,
                Iv = result.iv
            };
        }

        public (string data, string iv) GetPaymentDataAndIv(string paymentInfoString)
        {
            using (Aes myAes = Aes.Create())
            {

                myAes.KeySize = 128;
                myAes.GenerateKey();
                myAes.GenerateIV();
                byte[] keyAes = myAes.Key;
                byte[] ivAes = myAes.IV;

                byte[] resultCoding = new byte[48];
                byte[] baseStringbyte = HexStringToByteArray(paymentInfoString);

                byte[] encrypted = EncryptStringToBytesAes(baseStringbyte, myAes.Key, myAes.IV);
                byte[] hsahash = new SHA256CryptoServiceProvider().ComputeHash(encrypted);

                resultCoding = CombinArray(keyAes, hsahash);

                var data = ByteArrayToHexString(RSAEncription(resultCoding, _iranKishPaymentConfiguration.DecryptedPublicKey));

                var iv = ByteArrayToHexString(ivAes);

                return (data, iv);
            };
        }


        public byte[] HexStringToByteArray(string hexString)
        {

            return Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(value: hexString.Substring(startIndex: x, length: 2), fromBase: 16))
                .ToArray();

        }


        public string ByteArrayToHexString(byte[] bytes)
        {

            return (bytes.Select(t => t.ToString(format: "X2")).Aggregate((a, b) => $"{a}{b}"));
        }

        private byte[] EncryptStringToBytesAes(byte[] plainText, byte[] Key, byte[] IV)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                // Create the streams used for encryption.

            }

        }

        private byte[] CombinArray(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Array.Copy(first, 0, bytes, 0, first.Length);
            Array.Copy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        private byte[] RSAEncription(byte[] aesCodingResult, string publicKey)
        {

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return encryptEngine.ProcessBlock(aesCodingResult, 0, aesCodingResult.Length);
        }


        public async Task<bool> VerifyMerchantTransactionAsync(string terminalId, string retrievalReferenceNumber, string systemTraceAuditNumber, string tokenIdentity)
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });

                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://ikc.shaparak.ir/api/v3/asanconfirmation/purchase")
                {
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        terminalId,
                        retrievalReferenceNumber,
                        systemTraceAuditNumber,
                        tokenIdentity
                    }), Encoding.UTF8, "application/json")
                });

                response.EnsureSuccessStatusCode();

                var verifyMerchantTransResponseModel =
                    JsonSerializer.Deserialize<VerifyIranKishMerchantTransactionResponse>(await response.Content.ReadAsStringAsync());


                if (verifyMerchantTransResponseModel is { Status: true, ResponseConde: "00" } && verifyMerchantTransResponseModel.Result.ResponseCode == "00")
                    return true;

                return false;

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, nameof(VerifyMerchantTransactionAsync),
                    "بروز خطا به هنگام تایید نتیجه تراکنش درگاه ایران کیش", e);

                return false;
            }
        }
    }
}
