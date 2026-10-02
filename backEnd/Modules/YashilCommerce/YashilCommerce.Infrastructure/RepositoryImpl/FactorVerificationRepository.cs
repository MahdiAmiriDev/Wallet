using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorVerificationRepository : GenericApplicationBasedRepository<FactorVerification, int>,
        IFactorVerificationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IFactorVerificationConfigurationRepository _factorVerificationConfigurationRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public FactorVerificationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration,
            IFactorVerificationConfigurationRepository factorVerificationConfigurationRepository) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _factorVerificationConfigurationRepository = factorVerificationConfigurationRepository;
            _applicationConfiguration = applicationConfiguration;
        }

        public override IQueryable<FactorVerification> GetAll(bool readOnly = false, int? applicationId = null)
        {
            // دریافت اطلاعات کاربر لاگین شده
            var additionalUserProp = _context.AdditionalUserProp.AsNoTracking()
                .FirstOrDefault(x => x.UserId == _userPrincipal.Id);
            //TODO fix condition based on turn and state
            // فیلتر براساس اطلاعات کاربر لاگین شده
            if (additionalUserProp?.Company != null)
            {
                // دریافت همه بر اساس وضعیت فاکتور در حال بررسی و رد شده
                var query = base.GetAll(readOnly);
                query = query.Where(x => x.Factor.CompanyId == additionalUserProp.CompanyId);
                if (additionalUserProp.RepresentationId.HasValue)
                {
                    query = query.Where(x => x.Factor.RepresentationId == additionalUserProp.RepresentationId);
                }

                return query;
            }

            return null;
        }

        public IQueryable<FactorVerification> GetForConfirm()
        {
            var factorVerificationConfiguration = _factorVerificationConfigurationRepository
                .GetActiveFactorVerificationConfigurationIncludeRoles();

            var minVerificationOrder =
                factorVerificationConfiguration.FactorVerificationConfigurationRole.Min(x => x.VerificationOrder);

            var maxVerificationOrder =
                factorVerificationConfiguration.FactorVerificationConfigurationRole.Max(x => x.VerificationOrder);

            var countVerificationOrder = factorVerificationConfiguration.FactorVerificationConfigurationRole.Count();

            //لیست نقش های تایید کننده فاکتور برای کاربر لاگین شده
            var factorVerificationConfigurationRoles =
                factorVerificationConfiguration.FactorVerificationConfigurationRole.Where(x =>
                    _context.UserRole
                    .Where(ur => ur.UserId == _userPrincipal.Id && ur.ApplicationId == _userPrincipal.ApplicationId)
                    .Any(r => r.RoleId == x.RoleId && r.HasRole));

            IQueryable<FactorVerification> factorVerifications = DbSet.Where(x => 1 != 1);

            foreach (var factorVerificationConfigurationRole in factorVerificationConfigurationRoles)
            {
                factorVerifications = factorVerifications.Union(GetNotHistoryFactorVerificationsByFactorVerificationConfigurationId(
                        factorVerificationConfiguration.Id, factorVerificationConfigurationRole.VerificationOrder,
                        minVerificationOrder, maxVerificationOrder, countVerificationOrder));
            }

            return factorVerifications;
        }

        /// <summary>
        /// بررسی های  فاکتور به ازای تنظیم بررسی فعال  برنامه کاربر لاگین شده را بر اساس ترتیب مشخص شده می گرداند 
        /// قبلی ها همه تایید شده باشند و بعدی ها ثبت اولیه باشند
        /// </summary>
        /// <param name="factorVerificationConfigurationId"></param>
        /// <param name="verificationOrder">ترتیب مشخص شده</param>
        /// <param name="minVerificationOrder"></param>
        /// <param name="maxVerificationOrder"></param>
        /// <param name="countVerificationOrder"></param>
        /// <returns></returns>
        IQueryable<FactorVerification> GetNotHistoryFactorVerificationsByFactorVerificationConfigurationId(
            int factorVerificationConfigurationId, int verificationOrder, int minVerificationOrder,
            int maxVerificationOrder, int countVerificationOrder)
        {
            // باید بررسی های فاکتوری را بررسی کنیم که تاریخچه نباشند یا درواقع ishistory آنها 
            // false باشد
            var query = DbSet.Where(x => x.IsHistory == false);
            query = query.Where(x =>
               x.FactorVerificationConfigurationRole.FactorVerificationConfigurationId ==
               factorVerificationConfigurationId &&
               x.FactorVerificationConfigurationRole.VerificationOrder == verificationOrder &&
               (x.Factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Processing
                || x.Factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected) &&
               DbSet.Count(c => c.FactorId == x.FactorId && c.IsHistory == false &&
                                c.FactorVerificationConfigurationRole.VerificationOrder < verificationOrder &&
                                c.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Confirmed) +
               DbSet.Count(c => c.FactorId == x.FactorId && c.IsHistory == false &&
                                c.FactorVerificationConfigurationRole.VerificationOrder > verificationOrder &&
                                c.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Initial
               ) == countVerificationOrder - 1);
            return query;
        }

        public FactorVerification GetRejectedFactorVerification(int factorId, bool readOnly = false)
        {
            return DbSet.FirstOrDefault(x =>
                x.FactorId == factorId && x.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Rejected);
        }

        public IQueryable<FactorVerification> GetFactorVerificationsByFactorId(int factorId, bool withHistory = true, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.FactorId == factorId);
            if (!withHistory)
            {
                query = query.Where(x => x.IsHistory == false);
            }
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public Task<bool> CheckFactorHasAnyFactorVerificationAsync(int factorId)
        {
            return DbSet.AnyAsync(x => x.FactorId == factorId);
        }

        public Task<int> ArchiveOldFactorVerificationsAsync(int factorId, bool saveAfterAddOrUpdate)
        {
            return GetFactorVerificationsByFactorId(factorId, false).UpdateAsync(x => new FactorVerification() { IsHistory = true });
        }

        public Task<int> GetMaxFactorVerificationTryNumberAsync(int factorId)
        {
            return DbSet.Where(x => x.FactorId == factorId).OrderByDescending(x => x.TryNumber).Select(x => x.TryNumber).FirstOrDefaultAsync();
        }

    

        public IQueryable<FactorVerification> GetByCustomFilter(int? factorId, int? factorConfirmationType,
            int? factorVerificationConfigurationRoleId)
        {
            // فیلتر بر اساس آیدی فاکتور و وضعیت بررسی فاکتور
            var query = GetAll(true);
            if (factorId.HasValue)
            {
                query = query.Where(x => x.FactorId == factorId.Value);
            }

            if (factorConfirmationType.HasValue)
            {
                query = query.Where(x => x.FactorConfirmationType == factorConfirmationType.Value);
            }

            if (factorVerificationConfigurationRoleId.HasValue)
            {
                query = query.Where(x =>
                    x.FactorVerificationConfigurationRoleId == factorVerificationConfigurationRoleId.Value);
            }

            return query;
        }

        public bool CheckExistenceSubsequentRecord(int verificationOrder, int factorId)
        {
            return DbSet.Any(x => x.FactorId == factorId
                                  && x.FactorVerificationConfigurationRole.VerificationOrder > verificationOrder);
        }

        public IQueryable<FactorVerification> GetFactorVerificationsForFactorByCustomFilterForList(int factorId)
        {
            return DbSet.Where(x => x.FactorId == factorId);
        }

        public IQueryable<FactorVerification> GetFactorVerificationDescriptionForFactor(int factorId)
        {
            return DbSet.Where(x =>
                x.FactorId == factorId && x.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Rejected && !x.IsHistory);
        }

        /// <summary>
        /// در این متد بررسی های فاکتور قبلی و بعدی بر اساس ترتیب آنها یعنی 
        /// factorVerificationOrder چک میشود
        /// برای بررسی های فاکتور بعد باید همه آنها در وضعیت ثبت اولیه باشند
        /// و برای بررسیهای فاکتور قبلی باید همه آنها در وضعیت تایید شده باشند
        /// اگر هر یک از شرایط برقرار نباشد مقدار
        ///  False برمیگرداند
        /// </summary>
        /// <param name="factorVerification"></param>
        /// <returns>در صورتی که بررسی های فاکتور بعدی در حالت ثبت اولیه نباشند ویا بررسی های فاکتور قبلی در حالت تایید شده نباشند
        /// false
        /// و در غیر اینصورت
        /// true
        /// برمگیرداند</returns>
        public bool AllowUpdateFactorConfirmationType(FactorVerification factorVerification)
        {
            var userRoles = _context.UserRole.AsNoTracking().Where(x => x.UserId == _userPrincipal.Id && x.ApplicationId == _userPrincipal.ApplicationId && x.HasRole);
           
            if (DbSet.Any(x =>
                    x.Id == factorVerification.Id &&
                    !userRoles.Any(r => r.RoleId == x.FactorVerificationConfigurationRole.RoleId)
                    &&
                    ((x.FactorId == factorVerification.FactorId
                      && x.FactorVerificationConfigurationRole.VerificationOrder <
                      factorVerification.FactorVerificationConfigurationRole.VerificationOrder
                      && x.FactorConfirmationType != (int)FactorConfirmationTypeEnum.Confirmed && !x.IsHistory)
                     ||
                     (x.FactorId == factorVerification.FactorId
                      && x.FactorVerificationConfigurationRole.VerificationOrder >
                      factorVerification.FactorVerificationConfigurationRole.VerificationOrder
                      && x.FactorConfirmationType != (int)FactorConfirmationTypeEnum.Initial && !x.IsHistory))))
            {
                // یا کاربر نقش مورد نظر را ندارد
                // یا بررسی فاکتورهای بعدی در حالت ثبت اولیه نیستند
                // و یا بررسیهای فاکتور قبلی در حالت تایید شده نیستند
                return false;
            }

            if (DbSet.Any(x => x.FactorId == factorVerification.FactorId
                               && x.FactorVerificationConfigurationRole.FactorVerificationConfigurationId ==
                               factorVerification.FactorVerificationConfigurationRole.FactorVerificationConfigurationId
                               && x.FactorVerificationConfigurationRole.VerificationOrder > factorVerification
                                   .FactorVerificationConfigurationRole.VerificationOrder
                               && x.FactorConfirmationType != (int)FactorConfirmationTypeEnum.Initial && !x.IsHistory))
            {
                // کاربر هنگامی اجازه بروزرسانی را دارد که بررسی های فاکتور بعد در صورت وجود در حالت ثبت اولیه باشند
                // اگر اینطور نباشد اجازه بروزرسانی ندارد
                return false;
            }

            if (DbSet.Any(x => x.FactorId == factorVerification.FactorId
                               && x.FactorVerificationConfigurationRole.FactorVerificationConfigurationId ==
                               factorVerification.FactorVerificationConfigurationRole.FactorVerificationConfigurationId
                               && x.FactorVerificationConfigurationRole.VerificationOrder < factorVerification
                                   .FactorVerificationConfigurationRole.VerificationOrder
                               && x.FactorConfirmationType != (int)FactorConfirmationTypeEnum.Confirmed && !x.IsHistory))
            {
                // کاربر هنگامی اجازه بروزرسانی را دارد که بررسی های فاکتور قبل در صورت وجود در حالت تایید شده باشند
                // اگر اینطور نباشد اجازه بروزرسانی ندارد
                return false;
            }

            var factorReviewStatusType = _context.Factor.FirstOrDefault(x => x.Id == factorVerification.FactorId)
                ?.FactorReviewStatusType;
            if (factorReviewStatusType != null && (factorReviewStatusType == (int)FactorReviewStatusTypeEnum.Processing
                                                   || factorReviewStatusType ==
                                                   (int)FactorReviewStatusTypeEnum.Rejected))
            {
                return true;
            }

            return false;
        }


        public override FactorVerification Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.FactorVerificationConfigurationRole).FirstOrDefault(x => x.Id == (int)id);
        }

        public override async ValueTask<FactorVerification> GetAsync(object id, bool readOnly = true)
        {
            return await DbSet.Include(x => x.FactorVerificationConfigurationRole).FirstOrDefaultAsync(x => x.Id == (int)id);
        }
    }
}