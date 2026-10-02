using System.Collections.Generic;

namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// این مدل برای فرم 
    /// assignable
    /// مربوط به تخصیص دانشجو به کلاس مجازی کاربرد دارد
    /// چون در حالتی که از کلاس اسکای روم استفاده کردیم نیاز به حذف کاربران از اتاق مربوطه 
    /// در سامانه اسکای روم است
    /// </summary>
    public class AssignSelectedUsersToVirtualClassDto
    {
        /// <summary>
        /// شناسه کاربران
        /// </summary>
        public List<int> ProductSalesPlanningStudentIds { get; set; }

        /// <summary>
        /// تخصیص یا عدم تخصیص
        /// </summary>
        public bool Assign { get; set; }

        public int VirtualClassId { get; set; }
    }
}
