using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{

    public class UploadedFactorInstallmentPaymentRepository : GenericRepository<UploadedFactorInstallmentPayment, int>, IUploadedFactorInstallmentPaymentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public UploadedFactorInstallmentPaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        public IQueryable<UploadedFactorInstallmentPayment> GetByCustomFilter(int? yearMonth, int? userId)
        {
            var query = GetAll(true);

            if (yearMonth.HasValue)
            {
                query = query.Where(x => x.YearMonth == yearMonth);
            }
            if (userId.HasValue)
            {
                query = query.Where(x => x.CreateBy == userId);
            }

            return query.Where(x => x.IsDone == null || x.IsDone == false);
        }
        /// <summary>
        /// حذف دیتای فایل آپلود شده مربوط به اقساط پرداخت شده فاکتور 
        /// که دارای دیتای نامعتبر است
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="yearMonth">سال و ماه</param>
        public void DeleteByYearMonth(int userId, int yearMonth)
        {
            GetAll(true).Where(x => 
                   x.CreateBy == userId &&
                   x.YearMonth == yearMonth &&
                   (x.IsDone == null || x.IsDone == false)).DeleteFromQuery();
        }

        /// <summary>
        /// اعتبار سنجی مقادیر فایل اکسل قبل از درج در جدول
        /// FactorPaymentInstallment
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="yearMonth">سال و ماه اقساط</param>
        /// <returns></returns>
        public async Task<bool> ValidateUploadedFactorInstallmentPayment(int userId, int yearMonth)
        {
            var hasErrorParameter = new SqlParameter
            {
                ParameterName = "@HasError",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [shp].[ValidateUploadedFactorInstallmentPayment] " +
                "@UserId = @p0, " +
                "@YearMonth = @p1, " +
                "@HasError = @HasError OUTPUT",
                userId,
                yearMonth,
                hasErrorParameter);

            return (bool)hasErrorParameter.Value;
        }

        public IQueryable<UploadedFactorInstallmentPaymentReferenceGroupDto> GetFactorPaymentInstallmentReferenceGroup(int userId, int yearMonth)
        {
            return GetAll(true)
                 .Where(x => x.YearMonth == yearMonth && x.CreateBy == userId)
                 .Join(
                     _context.FactorPaymentInstallment,
                     u => u.FactorPaymentInstallmentId,
                     f => f.Id,
                     (u, f) => new UploadedFactorInstallmentPaymentReferenceGroupDto
                     {
                         Id = u.Id,
                         YearMonth = u.YearMonth,
                         CreationDate = u.CreationDate,
                         PaymentReferenceId = f.PaymentReferenceId,
                         PaymentReferencePersianDateTime = f.PaymentReferencePersianDateTime,
                     });
        }

        public IQueryable<UploadedFactorInstallmentPaymentReferenceDto> GetFactorPaymentInstallmentReference(int userId,DateTime creationDate)
        {
            return GetAll(true)
                 .Where(x => x.CreationDate == creationDate && x.CreateBy == userId)
                 .Join(
                     _context.FactorPaymentInstallment,
                     u => u.FactorPaymentInstallmentId,
                     f => f.Id,
                     (u, f) => new UploadedFactorInstallmentPaymentReferenceDto
                     {
                         Id = f.Id,
                        CategoryGroupTitle = f.FactorGroupItem.Title,
                        CompanyOrRepresentationCode = f.FactorGroupItem.CompanyOrRepresentationCode,
                        CompanyOrRepresentationTitle = string.IsNullOrEmpty(f.FactorGroupItem.RepresentationTitle) ?
                            f.FactorGroupItem.CompanyTitle
                          : string.Concat(f.FactorGroupItem.CompanyTitle, " - ", f.FactorGroupItem.RepresentationTitle),
                        InstallmentAmount = f.InstallmentAmount.ToString(),
                        InstallmentNumber = f.InstallmentNumber.ToString()
                     });
        }
    }
}
