using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorRepository : GenericApplicationBasedRepository<Factor, int>, IFactorRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public FactorRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public override IQueryable<Factor> GetAll(bool readOnly = false, int? applicationId = null)
        {
            // دریافت همه
            var query = base.GetAll(readOnly);
            // دریافت اطلاعات کاربر لاگین شده
            var additionalUserProp = _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => new { x.CompanyId, x.RepresentationId })
                .AsNoTracking().FirstOrDefault();

            if (additionalUserProp?.CompanyId != null)
            {
                // فیلتر براساس مرکز و نمایندگی  کاربر
                // اگر نمایندگی نداشت پس باید همه آیتمهای مربوط به ان مرکز را نمایش دهد
                // در صورتی که نمایندگی داشت باید علاوه بر فیلتر مرکز فیلتر نمایندگی نیز لحاظ شود.
                return query.Where(x => x.CompanyId == additionalUserProp.CompanyId
                                        && ((additionalUserProp.RepresentationId.HasValue &&
                                             x.RepresentationId == additionalUserProp.RepresentationId.Value)
                                            || (!additionalUserProp.RepresentationId.HasValue &&
                                                !x.RepresentationId.HasValue)));
            }

            return query;
        }


        public IQueryable<Factor> GetByCustomFilterForDelete(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType, bool readOnly = false)
        {
            // دریافت همه بر اساس تایید شده ها
            var query = base.GetAll(readOnly)
                .Where(x => x.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed);
            // بررسی مقدار آیدی مالیات
            if (taxCategoryId.HasValue)
            {
                // فیتر بر اساس آیدی مالیات
                query = query.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }

            // بررسی وجود آیدی مرکز
            if (companyId.HasValue)
            {
                // فیلتر برایای آیدی مرکز
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            // بررسی وجود آیدی نمایندگی
            if (representationId.HasValue)
            {
                // فیلتر براساس آیدی نمایندگی
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            // بررسی وجود وضعیت فاکتور
            if (factorReviewStatusType.HasValue)
            {
                // فیلتر بر اساس وضعیت فاکتور
                query = query.Where(x => x.FactorReviewStatusType == factorReviewStatusType.Value);
            }

            return query;
        }

        public IQueryable<Factor> GetSupervisorFactorList(int? companyId, int? representationId, int? taxCategoryId,
            int? factorReviewStatusType)
        {
            var result = base.GetAll(true);
            // دریافت اطلاعات کاربر لاگین شده
            var additionalUserProp = _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => new { x.CompanyId, x.RepresentationId })
                .AsNoTracking().FirstOrDefault();
            // اگر کاربر لاگین شده دارای مرکز یا نمایندگی بود باید آین لحاظ شود در غیر این صورت باید آنچیزی که پاس داده شده لحاظ شود.
            if (additionalUserProp != null)
            {
                if (additionalUserProp.CompanyId.HasValue)
                {
                    companyId = additionalUserProp.CompanyId.Value;
                }

                if (additionalUserProp.RepresentationId.HasValue)
                {
                    representationId = additionalUserProp.RepresentationId.Value;
                }
            }

            if (companyId.HasValue)
            {
                result = result.Where(x => x.CompanyId == companyId.Value);
            }

            if (representationId.HasValue)
            {
                result = result.Where(x => x.RepresentationId == representationId.Value);
            }

            if (taxCategoryId.HasValue)
            {
                result = result.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }

            return result;
        }

        public async Task<bool> CreateFactorAsync(Factor factor, int[] productSalePlanningIds, string prefixFactorNumber, bool saveAfterAdd = false)
        {
            await _context.Database.ExecuteSqlRawAsync(
                $" [shp].[CreateFactor] {_userPrincipal.Id},N'{factor.Description}',N'{prefixFactorNumber}',N'{string.Join(",", productSalePlanningIds)}'");
            return true;
        }

        public bool CheckFactorGroupHasFactor(FactorGroup factorGroup)
        {
            return DbSet.Any(x => x.FactorGroupId == factorGroup.Id);
        }

        public IQueryable<Factor> GetByCustomFilterForFactorGroupList(int factorGroupId, int? taxCategoryId,
            int? companyId,
            int? representationId, int? factorReviewStatusType)
        {
            var query = base.GetAll(true);
            query = query.Where(x => x.FactorGroupId == factorGroupId);
            if (taxCategoryId.HasValue)
            {
                query = query.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (factorReviewStatusType.HasValue)
            {
                query = query.Where(x => x.FactorReviewStatusType == factorReviewStatusType.Value);
            }

            return query;
        }

        public IQueryable<Factor> GetFactorsAssignedToFactorGroupAsync(int factorGroupId)
        {
            return base.GetAll().Where(x =>
                x.FactorGroupId == factorGroupId &&
                x.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed);
        }

        public IQueryable<Factor> GetFactorsNotAssignedToFactorGroupAsync(int? categoryGroupId)
        {
            var query = base.GetAll().Where(x =>
                x.FactorGroupId == null && x.FactorReviewStatusType
                == (int)FactorReviewStatusTypeEnum.Confirmed);
            if (categoryGroupId.HasValue)
            {
                query = query.Where(x => x.CategoryGroupId == categoryGroupId);
            }
            return query;
        }

        public async Task AssignSelectedItemsToFactorGroup(List<int> selectedFactors, int groupId, bool assign)
        {
            if (!assign)
            {
                var factors = DbSet.Where(x => selectedFactors.Contains(x.Id) && x.FactorGroupId == groupId);
                await factors.UpdateAsync(x => new Factor { FactorGroupId = null });
            }
            else
            {
                var factors = DbSet.Where(x => selectedFactors.Contains(x.Id));
                await factors.UpdateAsync(x => new Factor { FactorGroupId = groupId });
            }
        }

        public IQueryable<FactorGroupItemReport> GetFactorGroupItemList(int factorGroupId)
        {
            return _context.FactorGroupItemReports.FromSqlRaw("EXECUTE [shp].[RptFactorGroupItem] {0}", factorGroupId);
        }

        public IQueryable<Factor> GetByCustomFilter(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType)
        {
            var query = GetAll(true);
            if (taxCategoryId.HasValue)
            {
                query = query.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (factorReviewStatusType.HasValue)
            {
                query = query.Where(x => x.FactorReviewStatusType == factorReviewStatusType.Value);
            }


            return query;
        }

        /// <summary>
        /// check existence any unconfirmed Factor
        ///  return true if any factorReviewStatus in initial reject processing
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public bool CheckExistenceUnconfirmedFactor(int companyId, int? representationId)
        {
            return DbSet.Any(x => x.CompanyId == companyId &&
                                  ((representationId.HasValue
                                    && x.RepresentationId == representationId.Value)
                                   || (!representationId.HasValue && !x.RepresentationId.HasValue))
                                  && (x.FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.Confirmed));
        }

        public Factor GetUserFactor(int factorId, bool readOnly)
        {
            return GetAll(true).FirstOrDefault(x => x.Id == factorId);
        }

        public IQueryable<Factor> GetGroupedFactors(int? companyId, int? representationId)
        {
            // دریافت همه بر اساس تایید شده ها
            var query = base.GetAll(true)
                .Where(x => x.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed
                            && x.FactorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed);
            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId);
            }

            return query;
        }

        public bool CheckExistenceFactorForSaleConfiguration(int saleConfigurationId)
        {
            return DbSet.Any(x => x.SaleConfigurationId == saleConfigurationId);
        }

        public int? GetUserFactorRequestCountInEra(DateTime startDate, DateTime endDate)
        {
            // دریافت اطلاعات کاربر لاگین شده
            var additionalUserProp = _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => new { x.CompanyId, x.RepresentationId })
                .AsNoTracking().FirstOrDefault();
            if (additionalUserProp == null)
            {
                return null;
            }

            return DbSet.Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate
                                                                && x.CompanyId == additionalUserProp.CompanyId
                                                                && ((additionalUserProp.RepresentationId.HasValue &&
                                                                     x.RepresentationId == additionalUserProp
                                                                         .RepresentationId.Value)
                                                                    || (!additionalUserProp.RepresentationId.HasValue &&
                                                                        !x.RepresentationId.HasValue)))
                .Select(x => x.IssueDate).Distinct().Count();
        }

        public Task<bool> CheckExistAnyFactorByPersonBankAccountIdAsync(int personBankaccountId)
        {
            return DbSet.AnyAsync(x => x.PersonBankAccountId == personBankaccountId);
        }

        /// <summary>
        /// تمامی تراکنش های پرداخت مربوط به فاکتور را نمایش میدهد 
        ///  و برای تراکنش های از نوع پرداخت حکمت  با دیتای  تراکنش های تایید شده حکمت که از طریق سی دی از شرکت اتکا گرفته میشود
        ///  مقایسه میشود و در صورت وجود نداشتن تراکنش حکمت در سی دی و مغایرت قیمت ، شماره حکمت ، و کد رهگیری پدیرنده
        ///  آن را به عنوان خروجی برمی گرداند
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="showContrast">فقط مغایرت را نمایش بدهد </param>
        /// <returns></returns>
        public IQueryable<PaymentComparisonWithHekmatApprovedTransactionDomainModel>
            GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(int factorId, bool showContrast)
        {
            var query =
                from f in _context.Factor.Where(x => x.Id == factorId)
                join fi in _context.FactorItem
                    on f.Id equals fi.FactorId
                join a in _context.Application
                    on f.ApplicationId equals a.Id
                join oi in _context.OrderItemPaymentFullInfoView.Where(c =>
                        c.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                    on fi.OrderItemId equals oi.OrderItemId
                join hat in _context.HekmatApprovedTransaction
                    on oi.InstallmentTrackingCode equals hat.InstallmentTrackingCode into empHat
                from pch in empHat.DefaultIfEmpty()
                select new PaymentComparisonWithHekmatApprovedTransactionDomainModel
                {
                    Id = oi.PaymentId,
                    InstallmentTrackingCode = oi.InstallmentTrackingCode,
                    HekmatInstallmentTrackingCode = pch == null ? null : pch.InstallmentTrackingCode,
                    CardNumber = oi.CardNumber,
                    HekmatCardNumber = pch == null ? null : pch.HekmatCardNo,
                    PaymentPaidDateInt = oi.PaidDateInt.Value,
                    HekmatTransactionDate = pch == null ? (int?)null : pch.TransactionDate.Value,
                    PaymentTotalInt = oi.PaymentTotalInt.Value,
                    HekmatPrice = pch == null ? (long?)null : pch.Price.Value,
                    InstallmentCount = oi.InstallmentCount.Value,
                    ApplicationTitle = a.Title,
                    CardNumberInt = oi.CardNumberInt.Value,
                    HekmatCardNumberInt = pch == null ? (long?)null : pch.HekmatCardNoInt.Value,
                    PaymentMethodType = oi.PaymentMethodType.Value,
                    PaymentMethodTitle = oi.PaymentMethodTitle,
                    CustomerRefNum = oi.CustomerRefNum,
                    PaymentId = oi.PaymentId,
                    NewPaymentAmount = oi.NewPaymentAmount

                };
            if (showContrast)
            {
                query = query.Where(x =>
                    (x.PaymentTotalInt != x.HekmatPrice ||
                    x.CardNumberInt != x.HekmatCardNumberInt ||
                    x.InstallmentTrackingCode != x.HekmatInstallmentTrackingCode)
                     && x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat
                    );
            }

            return query.AsNoTracking();
        }

        public IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView> GetFactorItemPaymentFullInfoViewList(
            int factorGroupId)
        {
            return _context.FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView.AsNoTracking()
                .Where(x => x.FactorGroupId == factorGroupId);
        }

        public FactorStatisticDomainModel GetFactorStatistic(int factorId)
        {
            var factor = DbSet.Where(x => x.Id == factorId);
            var factorPaymentStatistic = factor.Select(x => new FactorStatisticDomainModel()
            {
                Id = factorId,
                FactorCode = x.Code,
                FactorReviewStatusTypeTitle = x.FactorReviewStatusTypeNavigation.Title,
                TaxCategoryTitle = x.TaxCategory.Title,
                SaleConfigurationTitle = x.SaleConfiguration.Title,
                IssueDate = x.IssueDate,
                ProfitPercentage = x.ProfitPercentage,
                DiscountPercentage = x.DiscountPercentage,
                FactorInstallmentCount = x.FactorInstallmentCount,
                HasTaxCertificate = x.SaleConfiguration.HasTaxCertificate,
                FactorTaxPercentage = x.TaxCategory.Percentage,
                InsurancePercentage = x.FactorItem.FirstOrDefault().OrderItem.InsurancePercentage,
                OtherDeductionsPercentage = x.FactorItem.FirstOrDefault().OrderItem.OtherDeductionsPercentage,
                CompanyTitle = x.Company.Title,
                RepresentationTitle = x.Representation.Title,
                AccountNumber = x.PersonBankAccount.AccountNumber,
                ShebaNumber = x.PersonBankAccount.ShebaNumber,
                BranchName = x.PersonBankAccount.BranchName,
                BankName = x.PersonBankAccount.BankTypeNavigation.Title,
                BankAccountOwner = string.Concat
                (x.PersonBankAccount.Person.Name, " " + x.PersonBankAccount.Person.LastName)

            }).FirstOrDefault();
            if (factorPaymentStatistic != null)
            {
                factorPaymentStatistic.SumPaid = factor.SelectMany(f => f.FactorPayment).Sum(x => x.Price);
                factorPaymentStatistic.PaidFactorInstallmentCount =
                    factor.SelectMany(x => x.FactorPayment).Sum(fp => fp.InstallmentCount);


                var purchasePriceFromSupplier = from f in _context.Factor
                        .Where(f => f.Id == factorId)
                                                join fi in _context.FactorItem
                                                    on f.Id equals fi.FactorId
                                                join oi in _context.OrderItemFullInfoView
                                                    on fi.OrderItemId equals oi.OrderItemId
                                                select new { purchasePriceFromSupplier = oi.PurchasePriceFromSupplier ?? 0 };

                factorPaymentStatistic.CompanyPurchasePrice =
                    purchasePriceFromSupplier.Sum(x => x.purchasePriceFromSupplier);


                factorPaymentStatistic.SumFactorDeduction = factor.SelectMany(f => f.FactorDeduction).Sum(x => x.Price);
                factorPaymentStatistic.FactorDeductionCount = factor.SelectMany(f => f.FactorDeduction).Count();
                factorPaymentStatistic.AdditionalShippingCharge = factor.SelectMany(f => f.FactorItem)
                    .Sum(o => o.OrderItem.AdditionalShippingCharge.Value);
                factorPaymentStatistic.InsurancePrice = factor.SelectMany(f => f.FactorItem)
                    .Sum(o => o.OrderItem.InsurancePrice);
                factorPaymentStatistic.OtherDeductionsPrice = factor.SelectMany(f => f.FactorItem)
                    .Sum(o => o.OrderItem.OtherDeductionsPrice);



                return factorPaymentStatistic;
            }

            return null;
        }

        public bool CheckExistenceFactorForCompanyAndRepresentation(int companyId, int? representationId)
        {
            return DbSet.Any(x =>
                x.CompanyId == companyId &&
                ((representationId.HasValue && representationId.Value == x.RepresentationId) ||
                 (!representationId.HasValue && !x.RepresentationId.HasValue)));
        }

        public async Task<bool> IsExistDifferentCategoryGroupInSelectedFactorsAsync(List<int> factorIds)
        {
            var distinctCount = await DbSet.Where(x => factorIds.Contains(x.Id))
                .Select(x => x.CategoryGroupId).Distinct().CountAsync();
            return distinctCount > 1;
        }

        public IQueryable<Factor> FindById(int factorId, bool readOnly)
        {
            var query = DbSet.Where(x => x.Id == factorId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<Factor> GetByFactorGroupId(int factorGroupId)
        {
            return DbSet.Where(x => x.FactorGroupId.HasValue && x.FactorGroupId == factorGroupId);
        }
    }
}