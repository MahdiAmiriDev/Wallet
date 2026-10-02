namespace Yashil.Core.CustomViewModels
{
   public class AssignableListEditModel
    {
        public int[] SelectedItems { get; set; }
        public int GroupId { get; set; }
        public bool Assign { get; set; }
        /// <summary>
        /// در حالتی که از فرم ایجاد کلاس مجازی سریع استفاده می کنیم این پارامتر کارایی دارد
        /// </summary>
        public int? VirtualClassId { get; set; }
    }
}
