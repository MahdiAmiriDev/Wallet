	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ValidHekmatCardService : GenericService<ValidHekmatCard,int>, IValidHekmatCardService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IValidHekmatCardRepository _validHekmatCardRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ValidHekmatCardService (IUnitOfWork unitOfWork, IValidHekmatCardRepository validHekmatCardRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, validHekmatCardRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_validHekmatCardRepository = validHekmatCardRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ValidHekmatCard> GetByCustomFilter( int? hekmatCardType)
        {
            return _validHekmatCardRepository.GetByCustomFilter(hekmatCardType);
        }
        /// <summary>
        /// بررسی میکند شماره کارت در شماره کارت های معتبر موجود هست یا خیر
        /// </summary>
        /// <param name="cardNumber">شماره کرات</param>
        /// <returns></returns>
        public bool CheckExistCardNumber(string cardNumber)
        {

            return _validHekmatCardRepository.CheckExistCardNumber(cardNumber);
        }

    }
}      
 