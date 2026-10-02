using Aspose.Cells;
using AutoMapper;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Core.Interfaces.Services;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Dto;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorPaymentInstallmentService : GenericService<FactorPaymentInstallment, int>, IFactorPaymentInstallmentService
    {
        private readonly IFactorPaymentInstallmentRepository _factorPaymentInstallmentRepository;
        private readonly IMapper _mapper;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IPersianDateMonthService _persianDateMonthService;
        private readonly IExcelBuilder _excelBuilder;
        private readonly IActivityLogService _activityLogService;


        public FactorPaymentInstallmentService(IUnitOfWork unitOfWork, IFactorPaymentInstallmentRepository factorPaymentInstallmentRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IMapper mapper, IPersianDateMonthService persianDateMonthService, IExcelBuilder excelBuilder, IActivityLogService activityLogService)
            : base(unitOfWork, factorPaymentInstallmentRepository, userPrincipal, eventPublisher)
        {
            _factorPaymentInstallmentRepository = factorPaymentInstallmentRepository;
            _userPrincipal = userPrincipal;
            _mapper = mapper;
            _persianDateMonthService = persianDateMonthService;
            _excelBuilder = excelBuilder;
            _activityLogService = activityLogService;
        }


        public IQueryable<FactorPaymentInstallment> GetByCustomFilter(int? factorGroupItemId, int? installmentPaymentStatusType)
        {
            return _factorPaymentInstallmentRepository.GetByCustomFilter(factorGroupItemId, installmentPaymentStatusType);
        }

        public IQueryable<FactorPaymentInstallmentInfoDto> GetInstallmentMonthYearList()
        {
            return _factorPaymentInstallmentRepository.GetInstallmentMonthYearList();
        }

        public IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallment(int? yearMonth, int? categoryGroupId, int? installmentPaymentStatusType)
        {
            return _factorPaymentInstallmentRepository.GetFactorPaymentInstallment(yearMonth, categoryGroupId, installmentPaymentStatusType);
        }

        public Task<IReadOnlyList<FactorPaymentInstallmentPerMonthDto>> GetPaymentListPerMonthAsync(int yearMonth, List<int> ids)
        {
            return _factorPaymentInstallmentRepository.GetPaymentListPerMonthAsync(yearMonth, ids);
        }
        public async Task<MemoryStream> ExportFactorPaymentInstallmentAsync(int yearMonth, List<int> ids)
        {
            var factorPaymentInstallmentLis = await
                GetPaymentListPerMonthAsync(yearMonth, ids);

            if (!factorPaymentInstallmentLis.Any())
                return null;

            return await CopyFactorPaymentInstallmentsToWorkBookAsync(factorPaymentInstallmentLis, yearMonth);

        }
        private async Task<MemoryStream>
            CopyFactorPaymentInstallmentsToWorkBookAsync(
                IReadOnlyList<FactorPaymentInstallmentPerMonthDto> source,
                int yearMonth)
        {
            var persianDateInfo =
                await _persianDateMonthService
                    .FindByYearMonth(yearMonth)
                    .Select(x => new
                    {
                        monthName = x.MonthNumberNavigation.Title,
                        YearName = x.YearNumber
                    })
                    .FirstOrDefaultAsync();
            // هر تعداد Header که خواستی
            _excelBuilder.AddTitle(0, 0, 1, yearMonth.ToString(), true);
            _excelBuilder.AddTitle(0, 1, 4, "");

            _excelBuilder.AddTitle(
                0,
                5,
                4,
                $"ماه : {persianDateInfo?.monthName}"
                );

            _excelBuilder.AddTitle(
                0,
                9,
                5,
                $"سال : {persianDateInfo?.YearName}");

            _excelBuilder.AddTitle(0, 14, 3, "");

            var factorPaymentInstallmentInfoInstance = new FactorPaymentInstallmentInfo();

            _excelBuilder.AddHeaders(
                factorPaymentInstallmentInfoInstance.FactorPaymentInstallmentColumns
                    .Select(x => x.Value.Name)
                    .ToList());

            _excelBuilder.ImportData(
                source,
                factorPaymentInstallmentInfoInstance.FactorPaymentInstallmentColumns
                    .Select(x => x.Key)
                    .ToArray(),
                new[]
                {
                    nameof(FactorPaymentInstallmentPerMonthDto.AccountNumber),
                    nameof(FactorPaymentInstallmentPerMonthDto.ShebaNumber)
                });

            return (MemoryStream)_excelBuilder.Build();
        }
        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        public async Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId)
        {
            return await _factorPaymentInstallmentRepository.UpdateFactorInstallmentPaymentFromUploadedFileAsync(userId);
        }
        /// <summary>
        /// ثبت گروهی شناسه مرجع اقساط پرداختی
        /// </summary>
        /// <param name="registerPaymentInstallmentReference">اطلاعات مرجع پرداخت</param>
        /// <returns></returns>
        public async Task<int> BulkInsertPaymentReferencesAsync(RegisterPaymentInstallmentReferencParamsDto registerPaymentInstallmentReference)
        {
            var dateTimeNow = DateTime.Now;

            var query = _factorPaymentInstallmentRepository
                             .GetFactorPaymentInstallmentForRegisterReference(
                             registerPaymentInstallmentReference.YearMonth,
                             null,
                             (int)InstallmentPaymentStatusTypeEnum.Paying,
                             registerPaymentInstallmentReference.CreationDate
                             );

            var previousRecordContent = await query.ToListAsync();

            var updatedCount = await query.ExecuteUpdateAsync(x => x
                .SetProperty(
                    c => c.PaymentReferenceId,
                    registerPaymentInstallmentReference.PaymentReferenceId)
                .SetProperty(
                    c => c.PaymentReferencePersianDateTime,
                    registerPaymentInstallmentReference.PaymentReferenceDate)
                .SetProperty(
                c => c.InstallmentPaymentStatusType,
                  (int)InstallmentPaymentStatusTypeEnum.Paid)
                .SetProperty(
                    c => c.ModificationDate,
                    dateTimeNow));

            if (updatedCount > 0)
            {
                await _activityLogService.AddActivityLogAsync(
                    new AddActivityLogDto
                    {
                        Description = "ثبت گروهی شناسه مرجع اقساط پرداختی",
                        ApplicationId = _userPrincipal.ApplicationId,
                        AppActionTitle = nameof(BulkInsertPaymentReferencesAsync),
                        PreviousRecordContent =
                            JsonConvert.SerializeObject(previousRecordContent),
                        ResourceTitle = nameof(FactorPaymentInstallment),
                        UserId = _userPrincipal.Id
                    });
            }
            return updatedCount;
        }
    }

    public class FactorPaymentInstallmentInfo
    {
        public enum FactorPaymentInstallmentColumnName
        {
            Id,
            CompanyOrRepresentationCode,
            CompanyOrRepresentationTitle,
            FactorCode,
            NumberOfInstallmentsPaid,
            SumAmountPaymentToSupplier,
            InstallmentAmount,
            RefundAmount,
            PaidAmount,
            RemainingPaymentToSupplier,
            TotalPaymentToSupplier,
            InstallmentNumber,
            NumberOfInstallmentsRemaining,
            AccountNumber,
            ShebaNumber,
            AccountOwner,
            BankName,

        }

        private static FactorPaymentInstallmentInfo _factorPaymentInstallmentInfo;

        public static char NumberGroupSeparator = ',';



        public static FactorPaymentInstallmentInfo Instance =>
            _factorPaymentInstallmentInfo ??= new FactorPaymentInstallmentInfo();
        /// <summary>
        /// جزئیات هر ستون
        /// </summary>
        public class FactorPaymentInstallmentColumnDetail
        {
            /// <summary>
            /// نام ستون
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// اجازه بارگذاری 
            /// </summary>
            public bool AllowUpload { get; set; }
        }
        public List<KeyValuePair<string, FactorPaymentInstallmentColumnDetail>> FactorPaymentInstallmentColumns { get; }

        public FactorPaymentInstallmentInfo()
        {

            FactorPaymentInstallmentColumns =
                new List<KeyValuePair<string, FactorPaymentInstallmentColumnDetail>>
                {
                    new(FactorPaymentInstallmentColumnName.Id.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "شناسه ", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.CompanyOrRepresentationCode.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "کد آموزشگاه", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.CompanyOrRepresentationTitle.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "موسسه", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.FactorCode.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "شماره فاکتور", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.NumberOfInstallmentsPaid.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "تعداد اقساط پرداختی", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.SumAmountPaymentToSupplier.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "مبلغ فاکتور", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.InstallmentAmount.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "مبلغ یک قسط", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.RefundAmount.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "عودت وجه", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.PaidAmount.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "میلغ پرداختی", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.RemainingPaymentToSupplier.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "مانده مبلغ ", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.TotalPaymentToSupplier.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "مجموع پرداختی", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.InstallmentNumber.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "شماره قسط", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.NumberOfInstallmentsRemaining.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "مانده قسط", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.AccountNumber.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "شماره حساب", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.ShebaNumber.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "شماره شبا", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.AccountOwner.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "نام صاحب حساب", AllowUpload = true }),
                    new(FactorPaymentInstallmentColumnName.BankName.ToString(),
                        new FactorPaymentInstallmentColumnDetail { Name = "نام بانک", AllowUpload = true })
                };
        }

        public int GetIndexOfProductItemColumn(FactorPaymentInstallmentColumnName columnName)
        {
            return FactorPaymentInstallmentColumns.IndexOf(FactorPaymentInstallmentColumns.FirstOrDefault(x => x.Key == columnName.ToString()));
        }

        public static string yearMonthPosition = "A1";

    }
}
