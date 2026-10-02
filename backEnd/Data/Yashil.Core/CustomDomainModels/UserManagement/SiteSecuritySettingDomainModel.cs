namespace Yashil.Core.CustomDomainModels.UserManagement
{
    public class SiteSecuritySettingDomainModel : IAppConfigs
    {
        /// <summary>
        /// تنظیم اینکه یک رمز عبور تا چند روز اعتبار داشته باشد و پس از آن منقضی شود.
        /// </summary>
        public int PasswordLifeTime { get; set; }
        /// <summary>
        /// تعداد دفعات مجازی که کاربر میتواند رمز عبور خود را
        /// اشتباه بزند و اگر بیش از این بزند کاربر برای مدت مشخصی قفل میشود. مدت قفل شدن در
        /// SiteConfigurationSecurityFailedPasswordLockOutMinutes
        /// تعیین میگردد. مقدار پیش فرض صفر به این معناست که کاربر میتواند
        /// بدون محدودیت رمز عبور خود را اشتباه بزند
        /// </summary>
        public int FailedPasswordAllowedAttempt { get; set; }
        /// <summary>
        /// میزان دقیقه قفل شدن کاربر پس از آنکه بعد از تعداد مشخصی رمز عبور خود را
        /// به اشتباه وارد کند. تعداد دفعات مشخصی که میتواند رمز عبور خود را به اشتباه وارد کند در
        /// SiteConfigurationSecurityFailedPasswordLockOutMinutes  مشخص میگردد مقدار پیش فرض صفر به این
        /// معناست که کاربر هیچوقت قفل نمیشود.
        /// </summary>
        public int FailedPasswordLockOutMinutes { get; set; }
        /// <summary>
        /// تعداد دفعات اشتباهی که کاربر رمز عبور خود را به اشتباه وارد کند و بعد از آن کاربر غیرفعال شود.
        /// مقدار پیش فرض صفر به این معناست که کاربر با زدن رمز عبور اشتباه  هیچوقت غیر فعال نمیشود.
        /// </summary>
        public int FailedPasswordCountDisableUser { get; set; }
        /// <summary>
        /// برای انکه کاربر رمز عبور خود را تغییر دهد نیاز است تا از رمز عبور های قبلی استفاده نکند.
        /// برای انکه مشخص کنیم از چه تعداد رمز عبور قبلی خود استفاده نکند با این پارامتر تنظیم میکنیم
        /// </summary>
        public int UnDuplicatedPasswordsNumber { get; set; }
        /// <summary>
        /// نمایش صفحه توافق نامه کاربرپس از لاگین کاربر
        /// </summary>
        public bool ShowUserAgreementConfirmationPageAfterLogin { get; set; }
        /// <summary>
        /// سیاست مربوط به رمز عبور که چگونه باشد در اینجا باید regular expression ست شود
        /// </summary>
        public string PasswordValidationRule { get; set; }
        /// <summary>
        /// پیام مربوط به ولیدیشن رمز عبور
        /// </summary>
        public string PasswordValidationRuleMessage { get; set; }
        /// <summary>
        /// مدت زمان پایان نشست
        /// </summary>
        public int SessionTimeOut { get; set; }
        /// <summary>
        /// حداکثر تعداد کاربران آنلاین
        /// </summary>
        public int SiteMaxUserCount { get; set; }
        /// <summary>
        /// کلمه عبور شامل حروف بزرگ باشد
        /// </summary>
        public bool PasswordRequireUppercase { get; set; }
        /// <summary>
        /// مقدار ریجکس حروف بزرگ
        /// </summary>
        public string PasswordUppercaseRegularExpressionValue { get; set; }
        /// <summary>
        ///  کلمه عبور شامل کوچک باشد
        /// </summary>
        public bool PasswordRequireLowercase { get; set; }
        /// <summary>
        /// مقدار ریجکس حروف کوچک
        /// </summary>
        public string PasswordLowercaseRegularExpressionValue { get; set; }
        /// <summary>
        ///  کلمه عبور شامل اعداد باشد
        /// </summary>
        public bool PasswordRequireDigit { get; set; }
        /// <summary>
        /// مقدار ریجکس اعداد
        /// </summary>
        public string PasswordDigitRegularExpressionValue { get; set; }
        /// <summary>
        ///  کلمه عبور شامل کاراکتر خاص باشد
        /// </summary>
        public bool PasswordRequireNonAlphanumeric { get; set; }
        /// <summary>
        /// مقدار ریجکس کاراکتر خاص
        /// </summary>
        public string PasswordNonAlphanumericRegularExpressionValue { get; set; }
        /// <summary>
        /// حداقل طول کاراکتر کلمه عبور
        /// </summary>
        public int PasswordMinLength { get; set; }
        /// <summary>
        /// اختلاف تعداد کاراکتر بین کلمه عبور جدید و قبلی
        /// </summary>
        public int NewCharacterCountDifferWithOldPassword { get; set; }
        /// <summary>
        /// مدت زمانی که کاربر در صورت عدم استفاده از سامانه لاک میشود.
        /// و به صفحه تایید رمز عبور میرود
        /// </summary>
        public int DurationOfUserLockout { get; set; }
    }
}
