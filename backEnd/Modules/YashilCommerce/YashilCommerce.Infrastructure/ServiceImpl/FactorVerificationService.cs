using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorVerificationService : GenericService<FactorVerification, int>, IFactorVerificationService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IEventPublisher _eventPublisher;
        private readonly IFactorVerificationRepository _factorVerificationRepository;

        public FactorVerificationService(IUnitOfWork unitOfWork, IFactorVerificationRepository factorVerificationRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, factorVerificationRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _factorVerificationRepository = factorVerificationRepository;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<FactorVerification> GetByCustomFilter(int? factorId, int? factorConfirmationType, int? factorVerificationConfigurationRoleId)
        {
            return _factorVerificationRepository.GetByCustomFilter(factorId, factorConfirmationType, factorVerificationConfigurationRoleId);
        }

        public bool AllowUpdateFactorConfirmationType(FactorVerification factorVerification)
        {
            // در صورت نقش مجاز داشتن نوبت بررسی فاکتور نیز چک میشود
            return _factorVerificationRepository.AllowUpdateFactorConfirmationType(factorVerification);
        }

        public bool CheckExistenceSubsequentRecord(int verificationOrder, int factorId)
        {
            return _factorVerificationRepository.CheckExistenceSubsequentRecord(verificationOrder,
                factorId);
        }

        public IQueryable<FactorVerification> GetFactorVerificationsForFactorByCustomFilterForList(int factorId)
        {

            return _factorVerificationRepository.GetFactorVerificationsForFactorByCustomFilterForList(factorId);
        }

        public IQueryable<FactorVerification> GetFactorVerificationDescriptionForFactor(int factorId)
        {
            return _factorVerificationRepository.GetFactorVerificationDescriptionForFactor(factorId);
        }

        public IQueryable<FactorVerification> GetForConfirm()
        {
            return _factorVerificationRepository.GetForConfirm();
        }

        public FactorVerification GetRejectedFactorVerification(int factorId, bool readOnly = false)
        {
            return _factorVerificationRepository.GetRejectedFactorVerification(factorId, readOnly);
        }

        public IQueryable<FactorVerification> GetFactorVerificationsByFactorId(int factorId, bool withHistory = true, bool readOnly = false)
        {
            return _factorVerificationRepository.GetFactorVerificationsByFactorId(factorId, withHistory, readOnly);
        }

        public Task<bool> CheckFactorHasAnyFactorVerificationAsync(int factorId)
        {
            return _factorVerificationRepository.CheckFactorHasAnyFactorVerificationAsync(factorId);
        }

        public Task<int> ArchiveOldFactorVerificationsAsync(int factorId, bool saveAfterAddOrUpdate)
        {
            return _factorVerificationRepository.ArchiveOldFactorVerificationsAsync(factorId, saveAfterAddOrUpdate);
        }

        public Task<int> GetMaxFactorVerificationTryNumberAsync(int factorId)
        {
            return _factorVerificationRepository.GetMaxFactorVerificationTryNumberAsync(factorId);
        }

        public async Task<FactorVerification> ChangeFactorLastVerifierConfirmTypeToInitialAsync(int factorId)
        {
            var factorVerification = await
                GetFactorVerificationsByFactorId(factorId)
                .Select(x => x)
                .OrderByDescending(c => c.FactorVerificationConfigurationRole.VerificationOrder)
                .Take(1)
                .FirstOrDefaultAsync();
            if (factorVerification != null &&
                factorVerification.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Confirmed)
            {
                factorVerification.FactorConfirmationType = (int)FactorConfirmationTypeEnum.Initial;
                factorVerification.ModificationDate = DateTime.Now;
                factorVerification.ValidationDate = null;
                factorVerification.VerifyBy = null;
               return await UpdateAsync(factorVerification, factorVerification.Id);
            }
            return null;
        }
    }
}
