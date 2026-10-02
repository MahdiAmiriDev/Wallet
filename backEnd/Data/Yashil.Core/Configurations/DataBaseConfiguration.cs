namespace Yashil.Core.Configurations
{
    public class DataBaseConfiguration : IDataBaseConfiguration
    {
        /// <summary>
        /// کلید رمز کردن داده ها در دیتا بیس
        /// </summary>
        public string EncryptedKey { get ; set ; }
    }
}
