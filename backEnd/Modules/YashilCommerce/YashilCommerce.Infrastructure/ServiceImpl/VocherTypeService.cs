using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.RepositoryImpl;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class VocherTypeService : GenericService<VocherType,int>, IVocherTypeService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IVocherTypeRepository _vocherTypeRepository;
		private readonly IUserPrincipal _userPrincipal;
		private readonly IEventPublisher _eventPublisher;
        private readonly IVocherRepository _vocherRepository;

        public VocherTypeService(IUnitOfWork unitOfWork, IVocherTypeRepository vocherTypeRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher, IVocherRepository vocherRepository)
            : base(unitOfWork, vocherTypeRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _vocherTypeRepository = vocherTypeRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _vocherRepository = vocherRepository;
        }


        public IQueryable<VocherType> GetByCustomFilter(int? companyId,bool readOnly = false)
        {
            return _vocherTypeRepository.GetByCustomFilter(companyId,readOnly);

        }

        /// <summary>
        /// انواع ووچر های مربوط به یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public IQueryable<VocherType> GetVocherTypesByCompanyId(int companyId)
        {
            return _vocherTypeRepository.GetVocherTypesByCompanyId(companyId);
        }

        public Task<bool> IsExistVoucherTypeForCompanyAsync(int companyId)
        {
            return GetByCustomFilter(companyId,true).AnyAsync();
        }

        /// <summary>
        ///  لیست تمامی کوپن های مربوط به نوع مشخص شده را برمیگرداند 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        public IQueryable<Vocher> GetVochersList(int vocherTypeId)
        {
            return _vocherRepository.GetByCustomFilter(vocherTypeId);
        }

        /// <summary>
        /// حذف کوپن های تخصیص نیافته یک نوع مشخص 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        public async Task<int> DeleteNotAssignedVochersAsync(int vocherTypeId)
        {
            return await _vocherRepository.DeleteNotAssignedVochersAsync(vocherTypeId);
        }
    }
}      
 