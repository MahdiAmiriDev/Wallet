using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{

    public class FactorPaymentInstallmentRepository : GenericApplicationBasedRepository<FactorPaymentInstallment, int>, IFactorPaymentInstallmentRepository
    {
        private readonly YashilAppDbContext _context;
        public FactorPaymentInstallmentRepository(
            YashilAppDbContext context,
            IUserPrincipal userPrincipal)
            : base(context, userPrincipal)
        {
            _context = context;
        }


        public IQueryable<FactorPaymentInstallment> GetByCustomFilter(int? factorGroupItemId, int? installmentPaymentStatusType)
        {
            var query = GetAll(true);

            if (factorGroupItemId.HasValue)
            {
                query = query.Where(x => x.FactorGroupItemId == factorGroupItemId.Value);
            }

            if (installmentPaymentStatusType.HasValue)
            {
                query = query.Where(x => x.InstallmentPaymentStatusType == installmentPaymentStatusType.Value);
            }

            return query;
        }

        public IQueryable<FactorPaymentInstallmentInfoDto> GetInstallmentMonthYearList()
        {
            var query = _context.PersianDateMonth
                .Where(x => x.FactorPaymentInstallment.Any())
                .Select(x => new FactorPaymentInstallmentInfoDto
                {
                    Id = x.YearMonth,
                    MonthName = x.MonthNumberNavigation.Title,
                    Year = x.YearNumber,
                    AllowUploadFactorPaymentInstallment = x.FactorPaymentInstallment
                        .Any(fpi => fpi.InstallmentPaymentStatusType != (int)InstallmentPaymentStatusTypeEnum.Paid)
                })
                .Distinct();

            return query;
        }

        public IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallment(
            int? yearMonth,
            int? categoryGroupId,
            int? installmentPaymentStatusType,
            bool readOnly = true)
        {
            var query = GetAll(readOnly)
                   .Where(x => x.YearMonth == yearMonth);

            if (categoryGroupId.HasValue)
            {
                query = query.Where(x => x.FactorGroupItem.Factor.CategoryGroupId == categoryGroupId.Value);
            }
            if (installmentPaymentStatusType.HasValue)
            {
                query = query.Where(x => x.InstallmentPaymentStatusType == installmentPaymentStatusType);
            }
            return query;

        }

        public IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallmentForRegisterReference(
           int? yearMonth,
           int? categoryGroupId,
           int? installmentPaymentStatusType,
           DateTime creationData)
        {
            var query =  GetFactorPaymentInstallment(yearMonth, categoryGroupId, installmentPaymentStatusType);
            return query.Where(x => 
                _context.UploadedFactorInstallmentPayment.Any(
                    u => u.CreationDate == creationData &&
                    u.FactorPaymentInstallmentId == x.Id)
                );
        }

        public async Task<IReadOnlyList<FactorPaymentInstallmentPerMonthDto>> GetPaymentListPerMonthAsync(int yearMonth, List<int> ids, bool readOnly = true)
        {
            return await GetAll(readOnly).Where(
                    x => x.YearMonth == yearMonth
                    && ids.Contains(x.Id)
                    )
                .Select(x => new FactorPaymentInstallmentPerMonthDto
                {
                    Id = x.Id,
                    CompanyOrRepresentationCode = x.FactorGroupItem.CompanyOrRepresentationCode,
                    RepresentationTitle = x.FactorGroupItem.RepresentationTitle,
                    CompanyTitle = x.FactorGroupItem.CompanyTitle,
                    FactorCode = x.FactorGroupItem.FactorCode,
                    NumberOfInstallmentsPaid = x.FactorGroupItem.NumberOfInstallmentsPaid ?? 0,
                    SumAmountPaymentToSupplier = x.SumAmountPaymentToSupplier.ToString(CultureInfo.InvariantCulture),
                    InstallmentAmount = x.InstallmentAmount.ToString(CultureInfo.InvariantCulture),
                    RefundAmount = x.RefundAmount,
                    PaidAmount = x.PaidAmount,
                    RemainingPaymentToSupplier = x.RemainingPaymentToSupplier ?? 0,
                    TotalPaymentToSupplier = x.TotalPaymentToSupplier,
                    InstallmentNumber = x.InstallmentNumber.ToString(),
                    NumberOfInstallmentsRemaining = Convert.ToString(x.FactorGroupItem.FactorInstallmentCount - x.InstallmentNumber),
                    AccountNumber = x.FactorGroupItem.AccountNumber,
                    ShebaNumber = x.FactorGroupItem.ShebaNumber,
                    AccountOwner = string.Concat(x.FactorGroupItem.PersonBankAccountName, " ", x.FactorGroupItem.PersonBankAccountLastName),
                    BankName = x.FactorGroupItem.BankTypeTitle
                }).ToListAsync();
        }
        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        public async Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId)
        {
            var updatedCountParameter = new SqlParameter
            {
                ParameterName = "@UpdatedCount",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC shp.UpdateFactorPaymentInstallmentFromUploadedFile @UserId, @UpdatedCount OUTPUT",
                new SqlParameter("@UserId", userId),
                updatedCountParameter);

            return (int)updatedCountParameter.Value;
        }

    }
}
