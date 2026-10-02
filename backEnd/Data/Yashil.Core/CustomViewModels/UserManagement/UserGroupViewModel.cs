namespace Yashil.Core.CustomViewModels.UserManagement
{
    /// <summary>
    /// دریافت گروه کاربری برای نمایش در فرم اختصاص کاربر به دوره
    /// </summary>
    public class UserGroupViewModel
    {
        /// <summary>
        /// شناسه گروه کاربری
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان گروه
        /// </summary>
        public string Title { get; set; }
    }
}
