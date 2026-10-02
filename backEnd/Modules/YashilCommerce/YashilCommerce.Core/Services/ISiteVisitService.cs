using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Core.Services
{
	public interface ISiteVisitService : IGenericService<SiteVisit, int>
    {
        IQueryable<SiteVisit> GetByCustomFilter( int? parentId);


        /// <summary>
        /// این متد جهت افزودن
        /// SiteVisit
        /// جدید کاربرد دارد
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        Task AddSiteVisitAsync(string url,string userAgent);

        /// <summary>
        /// این متد جهت دریافت
        /// SiteVisit
        /// بر اساس فیلتر تاریخ شمسی کاربرد دارد
        /// </summary>
        /// <param name="dateInt">تاریخ شمسی</param>
        /// <returns></returns>
        IQueryable<SiteVisit> GetByDateInt(int dateInt);

        /// <summary>
        /// این متد جهت دریافت تعداد بازدید سایت کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<int> GetCurrentDaySiteVisitCountAsync();

        /// <summary>
        /// این متد جهت دریافت آمار بازدید کل سایت کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalSiteVisitCountAsync();

        /// <summary>
        /// دریافت آمار بازدید کل سایت در روز قبل
        /// </summary>
        /// <returns></returns>
        Task<int> GetPreviousDayTotalSiteVisitCountAsync();


        /// <summary>
        /// این متد جهت ریست کردن مقدار تعداد بازدید های متغییر
        /// استاتیک پس پایان هر روز کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public Task ResetStaticSiteVisitCount();
    }
}
