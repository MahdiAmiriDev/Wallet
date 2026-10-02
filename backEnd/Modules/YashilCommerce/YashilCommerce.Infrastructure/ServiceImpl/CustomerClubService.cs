using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilCommerce.Core.DTO;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CustomerClubService : GenericService<CustomerClub, int>, ICustomerClubService
    {
        private readonly ICustomerClubRepository _customerClubRepository;
        private readonly ICustomerClubCardService _customerClubCardService;
        private readonly ISaleConfigurationAttributeService _saleConfigurationAttributeService;
        private readonly IAppConfigService _appConfigService;

        public CustomerClubService(IUnitOfWork unitOfWork, 
            ICustomerClubRepository customerClubRepository,
            IUserPrincipal userPrincipal,
            ICustomerClubCardService customerClubCardService, 
            IEventPublisher eventPublisher,
            ISaleConfigurationAttributeService saleConfigurationAttributeService,
            IAppConfigService appConfigService)
            : base(unitOfWork, customerClubRepository, userPrincipal, eventPublisher)
        {
            _customerClubRepository = customerClubRepository;
            _customerClubCardService = customerClubCardService;
            _saleConfigurationAttributeService = saleConfigurationAttributeService;
            _appConfigService = appConfigService;
        }
        /// <summary>
        /// Get CustomerClub By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        public CustomerClub GetByCardNumber(string cardNumber)
        {
            return _customerClubRepository.GetByCardNumber(cardNumber);
        }

        public async Task<bool> VerifyCustomerAsync(CustomerClubVerifyParameter customerClubVerifyParameter)
        {
            if (customerClubVerifyParameter.CustomerClubId.HasValue)
            {
                var customerClub = _customerClubRepository.Get(customerClubVerifyParameter.CustomerClubId);
                if (customerClub != null)
                {
                    if (customerClub.ServiceBased)
                    {
                        // باشگاه مشتریان کارافرین میباشد
                        if (customerClub.Code == "Karafarin")
                        {
                            if (customerClub.HasMobileNumber && customerClub.HasNationalCode)
                            {
                                return await CheckExistKarafarinCustomerClub(customerClubVerifyParameter.MobileNumber, customerClubVerifyParameter.NationalCode, customerClub.ServiceUrl);
                            }
                        }

                    }
                    if (customerClub.IsLocalClub)
                    {
                        return _customerClubCardService.CheckExistenceByFilter(customerClubVerifyParameter);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// موجود بودن کاربر در باشگاه مشتریان را بررسی میکند
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="nationalCode"></param>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistKarafarinCustomerClub(string mobile, string nationalCode, string serviceUrl)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            foreach (var requestParameter in request.Parameters)
            {
                request.RemoveParameter(requestParameter);
            }
            //  request.AddParameter("application/json", JsonConvert.SerializeObject(new { mobile = mobile, national_code = nationalCode }), ParameterType.RequestBody);
            var param = JsonConvert.SerializeObject(new { mobile, national_code = nationalCode });
            request.AddStringBody(param, ContentType.Json);
            var response = await client.ExecuteAsync(request);
            var result = JsonConvert.DeserializeObject<RestClubResultParams>(response.Content);
            var status = result.Status;
            if (status == 200)
                return true;
            return false;
        }

        public PayWithWalletDomainModel GetWalletPaymentInfoByCardNumber(string cardNumber)
        {
            var customerClubCard = _customerClubCardService.GetByCardNumber(cardNumber);
            var payWithWalletDomainModel = customerClubCard.Select(x => new PayWithWalletDomainModel()
            {
                CustomerClubId = x.CustomerClubId,
                CustomerClubCardId = x.Id,
                IsPayWithWallet = x.CustomerClub.PayWithWallet
            }).FirstOrDefault();

            if (payWithWalletDomainModel == null)
            {
                payWithWalletDomainModel = new PayWithWalletDomainModel();
            }
            return payWithWalletDomainModel;
        }
        /// <summary>
        /// باشگاه مشتریان مربوط به هر اپلیکیشنی را برمی گیرداند
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <returns></returns>
        public IQueryable<CustomerClub> GetAllByApplicationId(int applicationId)
        {
            return _customerClubRepository.GetAllByApplicationId(applicationId);
        }
        public IQueryable<CustomerClub> FindById(int id, bool readOnly = false)
        {
            return _customerClubRepository.FindById(id, readOnly);
        }

        public IQueryable<CustomerClub> GetAllCustomerClubHasActiveContract(int applicationId, int productSalesPlanningId)
        {
            return _customerClubRepository.GetAllCustomerClubHasActiveContract(applicationId, productSalesPlanningId);
        }

        /// <summary>
        /// دریافت اطلاعات مربوط به قیمت بر اساس تنظیم فروش و شناسه باشگاه مشتریان
        /// اگر برای باشگاه مشتریان ویژگی تنظیم وجود داشت
        /// پارامتر های قیمت مربوط به آن ویژگی تنظیم را بر می گرداند
        /// در غیر اینصورت پارامتر های قیمت مربوط به تنظیم فروش را بر می گرداند  
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="customerClubId"></param>
        /// <returns></returns>
        private async Task<SaleConfigurationPriceParametersDto> GetSaleConfigurationPriceParams
            (int saleConfigurationId,
             int? customerClubId)
        {
            if (customerClubId.HasValue)
            {
                customerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubPublicCustomerClubId);
            }

            var customerClubSaleConfiguration = await
                    _saleConfigurationAttributeService.GetByCustomFilter(null, null
                            , saleConfigurationId, customerClubId)
                        .FirstOrDefaultAsync();
            if (customerClubSaleConfiguration != null)
            {
                return new SaleConfigurationPriceParametersDto
                {
                    DiscountPercentage = customerClubSaleConfiguration.DiscountPercentage,
                    ProfitPercentage = customerClubSaleConfiguration.ProfitPercentage,
                    OtherDeductionsPercentage = customerClubSaleConfiguration.OtherDeductionsPercentage,
                    WagePercentage = customerClubSaleConfiguration.WagePercentage,
                    SaleConfigurationHaveAttribute = true

                };
            }

            return null;

        }
    }
}
