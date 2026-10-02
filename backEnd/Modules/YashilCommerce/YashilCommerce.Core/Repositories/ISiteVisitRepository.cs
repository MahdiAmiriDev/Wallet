using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ISiteVisitRepository : IGenericRepository<SiteVisit,int>
    {
        IQueryable<SiteVisit> GetByCustomFilter( int? parentId);


        /// <summary>
        /// این متد جهت افزودن
        /// SiteVisit
        /// جدید کاربرد دارد
        /// </summary>
        /// <param name="url">آدرس صفحه</param>
        /// <param name="dateInt">تاریخ شمسی</param>
        /// <param name="device">دستگاه بازدید کننده</param>
        /// <returns></returns>
        Task AddSiteVisitAsync(string url, int dateInt, string device);


        /// <summary>
        /// این متد جهت دریافت تعداد بازدید های سایت
        /// مربوط به روز جاری کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<int> GetCurrentDaySiteVisitCountAsync();

        /// <summary>
        /// این متد برای دریافت آمار بازدید کل سایت کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<int> GetSiteVisitTotalCount();

        /// <summary>
        /// دریافت
        /// SiteVisit
        /// براساس فیلتر تاریخ شمسی
        /// </summary>
        /// <param name="dateInt">تاریخ شمسی</param>
        /// <returns></returns>
        IQueryable<SiteVisit> GetByDateInt(int dateInt);
    }
}      
 