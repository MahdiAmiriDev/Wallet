namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.Slider
{
    public class SliderSimpleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }
        public string ModificationDate { get; set; }
        /// <summary>
        /// آدرس url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// شناسه
        /// object
        /// مربوط به گروه دسته بندی
        /// </summary>
        public long? CategoryGroupImageObjectId { get; set; }
    }
}
