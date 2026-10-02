using System.Linq.Dynamic.Core;
using Yashil.Core.Entities;
using Yashil.Core.ServiceExtenders;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.ServicesExtenders
{
    public class UserServiceExtender : IUserServiceExtender
    {
        private readonly StoreConfiguration _storeConfiguration;
      
        private readonly IAdditionalUserPropRepository _additionalUserPropRepository;

        public UserServiceExtender(
            StoreConfiguration storeConfiguration,
            IAdditionalUserPropRepository additionalUserPropRepository)
        {
            _storeConfiguration = storeConfiguration;
            _additionalUserPropRepository = additionalUserPropRepository;
        }

        public bool ValidateExistUser(User user)
        {
            // چک میشود که پروژه محدود به اپیلیکیشن هست یا خیر
            if (_storeConfiguration.LimitedToCustomerClub)
            {
                // اگر محدود بود اطلاعات کاربر چک میشود
                var additionalUserProp = _additionalUserPropRepository.FindByUserId(user.Id).FirstOrDefault();
                // اگر اطلاعات کاربر مثل باشگاه مشتریان نداشت کاربر را ناموجود بحساب می آورد
                return additionalUserProp?.CustomerClubId != null;
            }
            return true;
        }
    }
}