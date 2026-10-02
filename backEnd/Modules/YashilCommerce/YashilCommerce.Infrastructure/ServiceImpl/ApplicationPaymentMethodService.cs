using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Interfaces;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ApplicationPaymentMethodService : GenericService<ApplicationPaymentMethod, int>,
        IApplicationPaymentMethodService
    {
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICustomerClubPaymentMethodTypeService _customerClubPaymentMethodTypeService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ICommonBaseDataService _commonBaseDataService;
        private readonly IApplicationPaymentMethodRepository _applicationPaymentMethodRepository;

        public ApplicationPaymentMethodService(IUnitOfWork unitOfWork,
            IApplicationPaymentMethodRepository applicationPaymentMethodRepository,
            IUserPrincipal userPrincipal,
            ICustomerClubPaymentMethodTypeService customerClubPaymentMethodTypeService,
            IEventPublisher eventPublisher,
            IAdditionalUserPropService additionalUserPropService, ICommonBaseDataService commonBaseDataService)
            : base(unitOfWork, applicationPaymentMethodRepository, userPrincipal, eventPublisher)
        {
            _applicationPaymentMethodRepository = applicationPaymentMethodRepository;
            _userPrincipal = userPrincipal;
            _customerClubPaymentMethodTypeService = customerClubPaymentMethodTypeService;
            _additionalUserPropService = additionalUserPropService;
            _commonBaseDataService = commonBaseDataService;
        }

        public async Task<IQueryable<PaymentMethodType>> GetApplicationPaymentMethodsAsync(int applicationId, bool readOnly)
        {
            if (_userPrincipal.Id != 0)
            {
                var additionalUserProp =
                    await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                return _customerClubPaymentMethodTypeService.GetByCustomerClubId(additionalUserProp.CustomerClubId);

            }

            return null;
        }

        public IQueryable<ApplicationPaymentMethod> GetOnlineAndHekmatPaymentMethodsForSelect()
        {

            return GetByCustomFilter(null).Where(x =>
                x.PaymentMethodType == (int)PaymentMethodTypesEnum.Ansar ||
                x.PaymentMethodType == (int)PaymentMethodTypesEnum.Saman ||
                x.PaymentMethodType == (int)PaymentMethodTypesEnum.Samin ||
                x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat);
        }

        /// <summary>
        /// روش های پرداخت حکمت و ناجا را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<PaymentMethodType> GetNajaAndHekmatPaymentMethodsForSelect()
        {
            var query = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.PaymentMethodType.ToString());
            return query.Where(x =>
                x.Id == (int)PaymentMethodTypesEnum.Wallet || x.Id == (int)PaymentMethodTypesEnum.Hekmat).Select(x =>
                new PaymentMethodType
                {
                    Id = x.Id,
                    Title = x.Title
                });

        }

        public IQueryable<ApplicationPaymentMethod> GetByCustomFilter(int? paymentMethodType)
        {
            return _applicationPaymentMethodRepository.GetByCustomFilter(paymentMethodType);
        }

    }
}
