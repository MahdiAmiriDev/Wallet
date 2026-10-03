using Microsoft.AspNetCore.Mvc;
using Yashil.Infrastructure.Classes;

namespace Yashil.Infrastructure.Extensions
{
    /// <summary>
    /// این کلاس برای ایجاد
    /// Extenseion
    /// متد های جهت دریافت محتوای خارجی سایت مانند
    /// css , js , GenericApiCall ,...
    /// کاربرد دارد
    /// </summary>
    public static class YashilExternalFileHelper
    {
        /// <summary>
        /// این متد جهت دریافت فایل های
        /// css
        /// سایت کاربرد دارد
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="fileName">نام فایل</param>
        /// <param name="prefix">پیشوند</param>
        /// <returns>string</returns>
        public static string YashilCss(this IUrlHelper urlHelper, string fileName, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
                return GenerateUrl(fileName, YashilExternalFilePrefix.YashilCssPrefix);


            return prefix + "/" + fileName;
        }

        /// <summary>
        /// این متد جهت دریافت فایل های
        /// JS
        /// سایت کاربرد دارد
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="fileName">نام فایل</param>
        /// <param name="prefix">پیشوند</param>
        /// <returns>string</returns>
        public static string YashilJs(this IUrlHelper urlHelper, string fileName, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
                return GenerateUrl(fileName, YashilExternalFilePrefix.YashilJsPrefix);

            return prefix + "/" + fileName;
        }

        /// <summary>
        /// این متد برای دریافت
        /// api
        /// های عمومی سایت کاربرد دارد
        /// <param name="urlHelper"></param>
        /// <param name="fileName">نام فایل</param>
        /// <param name="prefix">پیشوند</param>
        /// <returns>string</returns>
        public static string YashilGenericApiCall(this IUrlHelper urlHelper, string fileName, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
                return GenerateUrl(fileName, YashilExternalFilePrefix.YashilGenericApiCallPrefix);


            return prefix + "/" + fileName;
        }

        /// <summary>
        /// این متد جهت دریافت تصاویر
        /// api call
        /// کاربرد دارد
        /// <param name="urlHelper"></param>
        /// <param name="fileName">نام فایل</param>
        /// <param name="prefix">پیشوند</param>
        /// <returns>string</returns>
        public static string YashilImageApiCall(this IUrlHelper urlHelper, string fileName, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
                return GenerateUrl(fileName, YashilExternalFilePrefix.YashilImageApiCallPrefix);


            return prefix + "/" + fileName;
        }

        /// <summary>
        /// این متد جهت دریافت فایل های تصویری کاربرد دارد
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="fileName">نام فایل</param>
        /// <param name="prefix">پیشوند</param>
        /// <returns>string</returns>
        public static string YashilImage(this IUrlHelper urlHelper, string fileName, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
                return GenerateUrl(fileName, YashilExternalFilePrefix.YashilImagePrefix);


            return prefix + "/" + fileName;
        }

        /// <summary>
        /// این متد جهت ساخت آدرس دریافت فایل کاربرد دارد
        /// </summary>
        /// <param name="fileName">نام فایل دریافتی</param>
        /// <param name="rootUrl">آدرس فایل</param>
        /// <returns>string</returns>
        private static string GenerateUrl(string fileName, string rootUrl)
        {
            return rootUrl + "/" + fileName;
        }

    }
}
