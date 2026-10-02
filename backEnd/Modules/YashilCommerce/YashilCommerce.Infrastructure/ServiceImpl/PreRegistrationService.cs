	using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

    namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class PreRegistrationService : GenericService<PreRegistration,int>, IPreRegistrationService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IPreRegistrationRepository _preRegistrationRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly ApplicationConfiguration _applicationConfiguration;
		public PreRegistrationService (IUnitOfWork unitOfWork, IPreRegistrationRepository preRegistrationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher, IProductSalesPlanningService productSalesPlanningService, ApplicationConfiguration applicationConfiguration)
			: base(unitOfWork, preRegistrationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_preRegistrationRepository = preRegistrationRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
             _productSalesPlanningService = productSalesPlanningService;
             _applicationConfiguration = applicationConfiguration;
        }
                 
        public IQueryable<PreRegistration> GetByCustomFilter( int? userId, int? productSalesPlanningId)
        {
            return _preRegistrationRepository.GetByCustomFilter(userId, productSalesPlanningId);
        }

        public Task<bool> CheckUserIsPreRegisteredAsync(int productSalesPlanningId, int userId)
        {
            return GetAll(true).AnyAsync(x => x.ProductSalesPlanningId == productSalesPlanningId
                                              && x.UserId == userId);
        }

        public async Task<string> PublicUserPreRegisterAsync(int productSalesPlanningId, int userId , int nationalCode)
        {
            var isValidPsp = await _productSalesPlanningService.CheckExistsProductSalePlanningInApplicationAsync(
                productSalesPlanningId, _applicationConfiguration.ApplicationId);
            if (!isValidPsp)
            {
               return  "کابر گرامی محصول انتخاب شده قابل فروش نمی باشد";
            }
            var isUserRegisteredBefore=  await CheckUserIsPreRegisteredAsync(productSalesPlanningId, userId);
            if (isUserRegisteredBefore)
            {
                return "کاربر گرامی نام شما قبلا در پیش ثبت نام درج شده است";
            }

            var result =await AddAsync(new PreRegistration
            {
                ProductSalesPlanningId = productSalesPlanningId,
                UserId = userId,
                ApplicationId = _applicationConfiguration.ApplicationId,
                CreatorOrganizationId = _userPrincipal.OrganizationId,
                Title = nationalCode.ToString()
            }, true);
            return string.Empty;
        }
    }
}      
 