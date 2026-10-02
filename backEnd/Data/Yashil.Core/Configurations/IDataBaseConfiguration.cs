namespace Yashil.Core.Configurations
{
    public interface IDataBaseConfiguration
    {
        /// <summary>
        /// کلید رمز کردن داده ها در دیتا بیس
        /// </summary>
        string EncryptedKey { get; set; }
    }
}
