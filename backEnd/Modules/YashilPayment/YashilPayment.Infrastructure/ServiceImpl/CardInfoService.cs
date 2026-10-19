using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class CardInfoService : GenericService<CardInfo,int>, ICardInfoService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ICardInfoRepository _cardInfoRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public CardInfoService (IUnitOfWork unitOfWork, ICardInfoRepository cardInfoRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, cardInfoRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_cardInfoRepository = cardInfoRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }        

        public IQueryable<CardInfo> GetByCustomFilter( int? cardType, int? personId)
        {
            return _cardInfoRepository.GetByCustomFilter(cardType,personId);
        }

        public List<CardInfo> GetCardInfoByCardTypeAndOrganization(int cardType, int organizationId)
        {
            return  _cardInfoRepository.GetCardInfoByCardTypeAndOrganization(cardType, organizationId).ToList();
        }

        public CardInfo GetCardInfoByCardNumber(string cardNumber, string cvv2, int expireDate)
        {
            return _cardInfoRepository.GetCardInfoByCardNumber(cardNumber, cvv2, expireDate).SingleOrDefault();
        }

        public void DoCardCharge(int ApplicationId, int CreateBy, int CreatorOrganizationId, int OrganizationId, int CardTypeId, int Amount, bool IsCredit)
        {
            _cardInfoRepository.DoCardCharge(ApplicationId, CreateBy, CreatorOrganizationId, OrganizationId, CardTypeId, Amount, IsCredit);
            //await SaveChangeAsync();
        }

        public string AddCardInfo(int ApplicationId, int CreateBy, int CreatorOrganizationId, int OrganizationId, int CardTypeId, string Name, string Family, string Gender, string NationalCode, string MobileNumber, string CardNumber, string CVV2, string FirstPassword, string SecondPassword, int? ExpireDate, int CreditAmount, int CashAmount, byte[] Password, byte[] PasswordSalt)
        {
            string message = _cardInfoRepository.AddCardInfo(ApplicationId, CreateBy, CreatorOrganizationId, OrganizationId, CardTypeId, Name, Family, Gender, NationalCode, MobileNumber, CardNumber, CVV2, FirstPassword, SecondPassword, ExpireDate, CreditAmount, CashAmount, Password, PasswordSalt);
            return message;
            //await SaveChangeAsync();
        }

    }
}      
 