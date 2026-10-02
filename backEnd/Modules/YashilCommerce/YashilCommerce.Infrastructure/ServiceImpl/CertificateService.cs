using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CertificateService : GenericService<Certificate, int>, ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public CertificateService(IUnitOfWork unitOfWork, ICertificateRepository certificateRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, certificateRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _certificateRepository = certificateRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<Certificate> GetByCustomFilter(string nationalCode)
        {
            return _certificateRepository.GetByCustomFilter(nationalCode);
        }

        /// <summary>
        /// اطلاعات گواهینامه را بر اساس شماره گواهینامه دریافت می کند
        /// </summary>
        /// <param name="certificateNumber">شماره گواهینامه</param>
        /// <returns></returns>
        public Task<Certificate> GetByNumberAsync(string certificateNumber)
        {
            return _certificateRepository.GetByNumberAsync(certificateNumber);
        }
    }

}
 