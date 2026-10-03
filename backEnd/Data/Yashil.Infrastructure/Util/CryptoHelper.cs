using System;
using System.Security.Cryptography;
using System.Text;

namespace Yashil.Infrastructure.Util
{
    public static class CryptoHelper
    {
        /// <summary>
        /// برای تایید کردن امضاء
        /// مقدار امضاء که به وسیله
        /// PrivateKey
        /// ساخته شده را با 
        /// Publickey
        /// بررسی می کند تا از ارسال اطلاعات توسظ سازنده 
        /// PrivateKey
        /// اطمینان حاصل کند
        /// </summary>
        /// <param name="data">دیتای ارسالی جهت بررسی و تایید امضا</param>
        /// <param name="sign">مقدار sign شده</param>
        /// <param name="publicKey">کلید عمومی</param>
        /// <returns></returns>
        public static bool VerifySignature(string data, byte[] sign, string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(publicKey.ToCharArray());
            var result = rsa.VerifyData(Encoding.UTF8.GetBytes(data), SHA256.Create(), sign);
            return result;
        }
        /// <summary>
        /// برای تایید کردن امضاء
        /// مقدار امضاء که به وسیله
        /// PrivateKey
        /// ساخته شده را با 
        /// Publickey
        /// بررسی می کند تا از ارسال اطلاعات توسظ سازنده 
        /// PrivateKey
        /// اطمینان حاصل کند
        /// </summary>
        /// <param name="data">دیتای ارسالی جهت بررسی و تایید امضا</param>
        /// <param name="sign">مقدار sign شده</param>
        /// <param name="publicKey">کلید عمومی</param>
        /// <returns></returns>
        public static bool VerifySignature(string data, string sign, string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(publicKey.ToCharArray());
            var result = rsa.VerifyData(Encoding.UTF8.GetBytes(data), SHA256.Create(), Convert.FromBase64String(sign));
            return result;
        }
    }
}
