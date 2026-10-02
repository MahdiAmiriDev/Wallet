using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;
using YashilCommerce.Infrastructure.ServiceImpl;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{

    public class AdditionalUserPropCompanyGroupRepository : GenericApplicationBasedRepository<AdditionalUserPropCompanyGroup, int>, IAdditionalUserPropCompanyGroupRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICompanyGroupRepository _companyGroupRepository;
        private readonly IAdditionalUserPropRepository _additionalUserPropRepository;


        public AdditionalUserPropCompanyGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ICompanyGroupRepository companyGroupRepository, IAdditionalUserPropRepository additionalUserPropRepository) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _companyGroupRepository = companyGroupRepository;
            _additionalUserPropRepository = additionalUserPropRepository;
        }


        public IQueryable<AdditionalUserPropCompanyGroup> GetByCustomFilter(int? additionalUserPropId, int? companyGroupId)
        {
            var query = GetAll(true);
            if (additionalUserPropId.HasValue)
            {
                query = query.Where(x => x.AdditionalUserPropId == additionalUserPropId.Value);
            }

            if (companyGroupId.HasValue)
            {
                query = query.Where(x => x.CompanyGroupId == companyGroupId.Value);
            }

            return query;
        }
        ///<summary>
        /// این متد در فرم تخصیص کاربر به گروه مرکز کاربرد دارد
        /// گروه مرکزهایی را که به کاربر اختصاص داده نشده اند یا گروه مرکزهایی را که به کاربر اختصاص داده شده اند،را باتوجه به پارامتر اساین لیست میکند 
        /// اگر پارامتر اساین ترو باشد، با توجه به ای دی کاربر از جدول
        /// AdditionalUserPropCompanyGroup
        /// AssignedCompanyGroup
        /// ها را لیست میکند
        /// اگر پارامتر اساین فالس باشد
        /// از جدول AdditionalUserPropCompanyGroup 
        /// گروه مرکزهایی که به این کاربر اختصاص داده شده اند را سلکت کرده و و در جدول 
        /// CompanyGroup
        /// رکوردهایی که شامل لیست اپلیکیشن های بالا نیست را برمیگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assigned"></param>
        /// <returns></returns>
        public IQueryable<CompanyGroup> GetUserCompanyGroups(int userId, bool assigned)
        {
            var companyGroups = _companyGroupRepository.GetAll(true)
                                .Where(x => x.ApplicationId == _userPrincipal.ApplicationId);

            if (assigned)
            {
                // گروه‌هایی که کاربر به آنها اختصاص دارد
                var assignedCompanyGroupIds = DbSet
                    .Where(x => x.AdditionalUserProp.UserId == userId)
                    .Select(x => x.CompanyGroup.Id)
                    .Distinct();

                return companyGroups.Where(x => assignedCompanyGroupIds.Contains(x.Id));
            }
            else
            {
                // گروه‌هایی که کاربر به آنها اختصاص ندارد
                var assignedCompanyGroupIds = DbSet
                    .Where(x => x.AdditionalUserProp.UserId == userId)
                    .Select(x => x.CompanyGroup.Id)
                    .Distinct();

                return companyGroups.Where(x => !assignedCompanyGroupIds.Contains(x.Id));
            }

        }

        /// <summary>
        /// این متد برای تخصیص گروه مرکز به کاربر و خارج کردن تخصیص کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="companyGroupsId">لیست گروه های کاربر مورد نظر</param>
        /// <param name="userId">ای دی کاربر مورد نظر</param>
        /// <param name="assign">تخصیص داده شده یا نشده ، مقدار true یا false برمی گرداند</param>
        /// 
        /// <returns></returns>
        public async Task AssignSelectedCompanyGroupesToUser(List<int> companyGroupsId, int additionalUserPropId, bool assign)
        {
            if (!assign)
            {
                DbSet.RemoveRange(DbSet.Where(x => companyGroupsId.Contains(x.CompanyGroupId) && x.AdditionalUserPropId == additionalUserPropId));
            }
            else
            {
                foreach (var companyMainCategory in companyGroupsId.Select(companyGroupId =>
                    new AdditionalUserPropCompanyGroup
                    {
                        CreateBy = _userPrincipal.Id,
                        CreationDate = DateTime.Now,
                        CreatorOrganizationId = _userPrincipal.OrganizationId,
                        CompanyGroupId = companyGroupId,
                        AdditionalUserPropId = additionalUserPropId
                    }))
                {
                    await AddAsync(companyMainCategory);
                }
            }
        }


    }
}      
