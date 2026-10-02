	using System.Linq;
    using Yashil.Common.Core.AppEvents;
    using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class YashilSchedulerService : GenericService<YashilScheduler,int>, IYashilSchedulerService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IYashilSchedulerRepository _yashilSchedulerRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public YashilSchedulerService (IUnitOfWork unitOfWork, IYashilSchedulerRepository yashilSchedulerRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, yashilSchedulerRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_yashilSchedulerRepository = yashilSchedulerRepository;
			_userPrincipal = userPrincipal;
        }
                 
        public IQueryable<YashilScheduler> GetByCustomFilter( int? occuranceType)
        {
            return _yashilSchedulerRepository.GetByCustomFilter(occuranceType);
        }
           

    }
}      
 