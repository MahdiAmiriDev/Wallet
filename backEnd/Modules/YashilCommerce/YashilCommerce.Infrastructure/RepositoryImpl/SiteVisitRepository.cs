using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{

    public class SiteVisitRepository : GenericApplicationBasedRepository<SiteVisit, int>, ISiteVisitRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<SiteVisit> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public SiteVisitRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, IYashilLogger<SiteVisit> logger, ApplicationConfiguration applicationConfiguration) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }


        public IQueryable<SiteVisit> GetByCustomFilter(int? parentId)
        {
            var query = GetAll(true);
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            return query;
        }

        /// <summary>
        /// این متد جهت افزودن
        /// SiteVisit
        /// جدید کاربرد دارد
        /// </summary>
        /// <param name="url">آدرس صفحه </param>
        /// <param name="dateInt">تاریخ شمسی</param>
        /// <param name="device">دستگاه بازدید کننده</param>
        /// <returns></returns>
        public async Task AddSiteVisitAsync(string url, int dateInt, string device)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter()
                {
                    ParameterName = "@Url",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = url
                },
                new SqlParameter()
                {
                    ParameterName = "@DateInt",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = dateInt,
                },
                new SqlParameter()
                {
                    ParameterName = "@Device",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = device
                }
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXECUTE [shp].[AddSiteVisit] @Url,@DateInt,@Device", parameters);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(SiteVisit), "Error Occurred In Execute AddSiteVisit", e);
            }
        }


        /// <summary>
        /// این متد جهت دریافت تعداد بازدید های سایت
        /// مربوط به روز جاری کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCurrentDaySiteVisitCountAsync()
        {
            
            var toDayDateInt = DateFunction.TodayShamsiInt();

            return await _context.SiteVisit
                 .Where(siteVisit => siteVisit.DateInt == toDayDateInt)
                 .Select(siteVisit => siteVisit.VisitCount).SumAsync();
        }

        /// <summary>
        /// این متد برای دریافت آمار بازدید کل سایت کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetSiteVisitTotalCount()
        {
            return await _context.SiteVisit
                .Select(siteVisit => siteVisit.VisitCount).SumAsync();
        }

        /// <summary>
        /// دریافت
        /// SiteVisit
        /// براساس فیلتر تاریخ شمسی
        /// </summary>
        /// <param name="dateInt"></param>
        /// <returns></returns>
        public IQueryable<SiteVisit> GetByDateInt(int dateInt)
        {
            return  _context.SiteVisit
                .Where(siteVisit => siteVisit.DateInt == dateInt);
        }
    }
}
