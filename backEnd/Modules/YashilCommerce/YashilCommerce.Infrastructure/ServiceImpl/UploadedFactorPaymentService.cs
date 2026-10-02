using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class UploadedFactorPaymentService : GenericService<UploadedFactorPayment, int>,
        IUploadedFactorPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadedFactorPaymentRepository _uploadedFactorPaymentRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAppDocumentService _appDocumentService;
        private readonly ISqlBulkCopyExcelToDb _oledbBasedRepository;
        private readonly IYashilLogger<UploadedFactorPayment> _logger;

        public UploadedFactorPaymentService(IUnitOfWork unitOfWork,
            IUploadedFactorPaymentRepository uploadedFactorPaymentRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher, IAppDocumentService appDocumentService, ISqlBulkCopyExcelToDb oledbBasedRepository, IYashilLogger<UploadedFactorPayment> logger)
            : base(unitOfWork, uploadedFactorPaymentRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _uploadedFactorPaymentRepository = uploadedFactorPaymentRepository;
            _eventPublisher = eventPublisher;
            _appDocumentService = appDocumentService;
            _oledbBasedRepository = oledbBasedRepository;
            _logger = logger;
        }

        public IQueryable<UploadedFactorPayment> GetByCustomFilter(int? appDocumentId)
        {
            return _uploadedFactorPaymentRepository.GetByCustomFilter(appDocumentId);
        }

        public Task<int> ChecksInvalidUploadedFactorPaymentFilesForLoggedInUserAsync()
        {
            return _uploadedFactorPaymentRepository.ChecksInvalidUploadedFactorPaymentFilesForLoggedInUser();
        }

        public void DeleteByUserId(int userId)
        {
            var allUploadedFactorPaymentsUploadedByUserId = _uploadedFactorPaymentRepository.GetAllUploadedFactorPaymentsUploadedByUser(userId);
            foreach (var uploadedFactorPayment in allUploadedFactorPaymentsUploadedByUserId)
            {
                _uploadedFactorPaymentRepository.Delete(uploadedFactorPayment);
            }
        }

        public Task<int> TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync()
        {
            return _uploadedFactorPaymentRepository
                .TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync();
        }

        public async Task<bool> BulkInsertFactorPaymentsDataAsync(IFormFile factorPaymentFile)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // (int)AppEntityEnum.FactorPayment 
                var saveAppDocumentResult = await _appDocumentService.SaveDocumentAsync(null, null, null, null, null, (int)AppEntityEnum.UploadedFactorPayment,
                    (int)DocumentTypeEnum.UploadFactorPayments,
                    null, (int)AppEntityEnum.FactorPayment, factorPaymentFile);
                var filePath = _appDocumentService.GetFilePath(saveAppDocumentResult.appDocument.Id);

                var factorPaymentColumns = new[]
                {
                    "FactorCode", "InstallmentCount", "Price", "PaymentDate", "BankAccountNumber"
                };
                await _oledbBasedRepository.BulkInsertFromExcelOrCsvAsync("shp.UploadedFactorPayment", filePath,
                    0, factorPaymentColumns, saveAppDocumentResult.appDocument.Id, 0);
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, "BulkInsertFactorPaymentsDataAsync",
                    $" بروز خطا در فرایند بارگذاری فایل اکسل پرداختی های فاکتور ", e);
                return false;
            }
        }
    }
}