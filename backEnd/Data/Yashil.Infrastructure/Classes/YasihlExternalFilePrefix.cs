using System.Threading.Tasks;
using YashilUserManagement.Core.Services;

namespace Yashil.Infrastructure.Classes
{
    public static class YashilExternalFilePrefix
    {
        public static string YashilCssPrefix;
        public static string YashilJsPrefix;
        public static string YashilImageApiCallPrefix;
        public static string YashilGenericApiCallPrefix;
        public static string YashilImagePrefix;

        /// <summary>
        /// کلید پیشوند مربوط به استایل های سایت
        /// </summary>
        private static string SiteConfigurationPrefixCssKey = "Site.Configuration.Prefix.Css";
        /// <summary>
        /// کلید پیشوند مربوط به اسکریپت های سایت
        /// </summary>
        private static string SiteConfigurationPrefixJsKey = "Site.Configuration.Prefix.Js";
        /// <summary>
        /// کلید پیشوند مربوط به ای پی ای های تصاویر
        /// </summary>
        private static string SiteConfigurationPrefixImageApiCallKey = "Site.Configuration.Prefix.ImageApiCall";
        /// <summary>
        /// کلید پیشوند مربوط به ای پی ای های عمومی
        /// </summary>
        private static string SiteConfigurationPrefixGenericApiCallKey = "Site.Configuration.Prefix.GenericApiCall";
        /// <summary>
        /// کلید پیشوند مربوط به تصاویر
        /// </summary>
        private static string SiteConfigurationPrefixImageKey = "Site.Configuration.Prefix.Image";

        /// <summary>
        /// این متد کار دریافت آدرس پیشوند برای محتوای
        /// css
        /// js
        /// ImageApiCall
        /// GenericApiCall
        /// Image
        /// را انجام می دهد
        /// </summary>
        /// <param name="appConfigService"></param>
        /// <returns></returns>
        public static async Task InitializeAsync(IAppConfigService appConfigService)
        {
            YashilCssPrefix = await appConfigService.GetStringValueAsync(SiteConfigurationPrefixCssKey);
            YashilJsPrefix = await appConfigService.GetStringValueAsync(SiteConfigurationPrefixJsKey);
            YashilImageApiCallPrefix = await appConfigService.GetStringValueAsync(SiteConfigurationPrefixImageApiCallKey);
            YashilGenericApiCallPrefix = await appConfigService.GetStringValueAsync(SiteConfigurationPrefixGenericApiCallKey);
            YashilImagePrefix = await appConfigService.GetStringValueAsync(SiteConfigurationPrefixImageKey);
        }
    }
}
