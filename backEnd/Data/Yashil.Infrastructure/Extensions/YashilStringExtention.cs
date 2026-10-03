using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
using Quartz.Util;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;

namespace Yashil.Infrastructure.Extensions
{
    public static class YashilStringExtention
    {
        /// <summary>
        /// تعیین معتبر بودن کد ملی
        /// </summary>
        /// <param name="nationalCode">کد ملی وارد شده</param>
        /// <exception cref="System.Exception"></exception>
        public static bool IsValidNationalCode(this string nationalCode)
        {
            nationalCode = nationalCode.ToEnglishDigit();
            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.Length != 10)
                return false;

            //در صورتی که کد ملی ده رقم عددی نباشد
            if (!long.TryParse(nationalCode, out var nationalCodeInt))
                return false;
            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[]
            {
                "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666",
                "7777777777", "8888888888", "9999999999"
            };
            if (allDigitEqual.Contains(nationalCode)) return false;


            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
            var c = b % 11;

            return c < 2 && a == c || c >= 2 && 11 - c == a;
        }
        /// <summary>
        /// اعداد فارسی با انگلیسی جایگزین می کند
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToEnglishDigit(this string str)
        {
            var find = new string[] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            var replace = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            for (var i = 0; i < find.Length; i++)
            {
                str = str.Replace(find[i], replace[i]);
            }
            return str;
        }

        public static bool IsValidValidateHekmatCard(this string cardNumber)
        {
            cardNumber = cardNumber.ToEnglishDigit();
            if (string.IsNullOrEmpty(cardNumber)
              || cardNumber.Length != 16 || !cardNumber.StartsWith("08920") || cardNumber == "0892012345678912" || cardNumber == "0892090011111111")
            {
                return false;
            }
            var cardTotal = 0;
            var cardNumberArray = cardNumber.ToCharArray();
            for (var i = 0; i < 16; i += 1)
            {
                var c = int.Parse(cardNumberArray[i].ToString());
                if (i % 2 == 0)
                {
                    cardTotal += ((c * 2 > 9) ? (c * 2) - 9 : (c * 2));
                }
                else
                {
                    cardTotal += c;
                }
            }
            return cardTotal % 10 == 0;
        }
        /// <summary>
        /// بررسی معتبر بودن شماره موبایل
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsValidMobileNumber(this string number)
        {
            // return Regex.Match(number, @"^(\+98|0098|98|0)?9\d{9}$").Success;
            // در پروژه تنها موبایل هایی را قبول میکنیم که با 09 شروع شوند
            // زیرا برای ارسال پیامک ممکن است استفاده شود و به مشکل بخورد
            return Regex.Match(number, @"^09\d{9}$").Success;
        }
        /// <summary>
        /// چک کردن شماره شبا
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidSheba(this string input)
        {
            if (input.IsNullOrWhiteSpace())
            {
                return false;
            }
            input = input.Replace(" ", "").ToLower();
            //بررسی رشته وارد شده برای اینکه در فرمت شبا باشد
            var isSheba = Regex.IsMatch(input, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}",
                RegexOptions.Compiled);

            if (!isSheba)
                return false;
            //طول شماره شبا را چک میکند کمتر نباشد
            if (input.Length < 26)
                return false;
            input = input.ToLower();
            //بررسی اعتبار سنجی اصلی شبا
            ////ابتدا گرفتن چهار رقم اول شبا
            var get4FirstDigit = input.Substring(0, 4);
            ////جایگزین کردن عدد 18 و 27 به جای آی و آر
            var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
            ////حذف چهار رقم اول از رشته شبا
            var removedShebaFirst4Digit = input.Replace(get4FirstDigit, "");
            ////کانکت کردن شبای باقیمانده با جایگزین شده چهار رقم اول
            var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
            ////تبدیل کردن شبا به عدد  - دسیمال تا 28 رقم را نگه میدارد
            var finalLongData = Convert.ToDecimal(newSheba);
            ////تقسیم عدد نهایی به مقدار 97 - اگر باقیمانده برابر با عدد یک شود این رشته شبا صحیح خواهد بود
            var finalReminder = finalLongData % 97;
            if (finalReminder == 1)
            {
                return true;
            }
            return false;


        }
        /// <summary>
        /// چک کردن شماره کارت بانکی
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidCardNumber(this string cardNumber)
        {
            cardNumber = cardNumber.ToEnglishDigit();
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
            {
                return false;
            }
            var cardTotal = 0;
            var cardNumberArray = cardNumber.ToCharArray();
            for (var i = 0; i < 16; i += 1)
            {
                var c = int.Parse(cardNumberArray[i].ToString());
                if (i % 2 == 0)
                {
                    cardTotal += ((c * 2 > 9) ? (c * 2) - 9 : (c * 2));
                }
                else
                {
                    cardTotal += c;
                }
            }
            return cardTotal % 10 == 0;
        }

        /// <summary>
        /// بررسی میکند که ایمیل معتبر است یا خیر
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format"/>
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// بررسی می کند که شماره تلفن ثابت درست می باشد یا خیر
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsValidPhoneNumber(this string number)
        {
            return Regex.Match(number, @"^0[1-9]{2}[0-9]{8}$").Success;
        }
        /// <summary>
        /// بررسی می کند که آیا آی پی آدرس درست می باشد یا خیر ؟ 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsValidIpAddress(this string number)
        {
            return Regex.Match(number, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$").Success;
        }

        /// <summary>
        /// این متد برای تبدیل کردن string
        /// به نوع داده دلخواه که در به عنوان نوع جنریک
        /// توسط متد دریافت می شود کاربرد دارد
        /// در صورتی که در فرآیند انجام عملیات خطایی رخ دخد این متد مقدار
        /// Default(TResponse)
        /// را برمیگرداند
        /// اگر نوع
        /// Generic
        /// برابر String
        /// باشد تبدیلی انجام نمی شود
        /// </summary>
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResponse ConvertTo<TResponse>(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(TResponse);

            try
            {
                Type targetType = typeof(TResponse);

                if (targetType == typeof(string))
                    return (TResponse)(object)value;

                var converter = TypeDescriptor.GetConverter(targetType);

                if (converter.CanConvertFrom(typeof(string)))
                    return (TResponse)converter.ConvertFrom(value);

                throw new ArgumentException($"Error occurred in converting type of string value:{value} to {targetType}");

            }
            catch (Exception e)
            {
                return default(TResponse);
            }
        }

        /// <summary>
        /// بررسی می‌کند که مقدار ورودی شامل تگ‌های HTML یا محتوای قابل اجرای غیرمجاز نباشد.
        /// در صورتی که مقدار پس از Sanitization با مقدار اولیه متفاوت باشد، به این معنی است
        /// که محتوای HTML یا عناصر غیرمجاز در ورودی وجود داشته و مقدار false برگردانده می‌شود.
        /// </summary>
        /// <param name="input">رشته‌ای که باید از نظر وجود HTML یا محتوای غیرمجاز بررسی شود.</param>
        /// <returns>
        /// در صورت امن بودن ورودی مقدار true و در صورت وجود HTML یا محتوای غیرمجاز مقدار false برمی‌گرداند.
        /// </returns>
        public static bool IsSafeInput(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            var sanitizer = new HtmlSanitizer();

            var sanitized = sanitizer.Sanitize(input);

            return string.Equals(
                input,
                sanitized,
                StringComparison.Ordinal);
        }
    }
}
