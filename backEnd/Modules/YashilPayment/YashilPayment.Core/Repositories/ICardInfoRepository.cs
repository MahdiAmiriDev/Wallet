			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface ICardInfoRepository : IGenericRepository<CardInfo,int>
    {
		          
        IQueryable<CardInfo> GetByCustomFilter( int? cardType, int? personId);

        IQueryable<CardInfo> GetCardInfoByCardTypeAndOrganization(int cardType, int organizationId);

        IQueryable<CardInfo> GetCardInfoByCardNumber(string cardNumber, string cvv2, int expireDate);

        void DoCardCharge(int ApplicationId, int CreateBy, int CreatorOrganizationId, int OrganizationId, int CardTypeId, int Amount, bool IsCredit);

        string AddCardInfo(int ApplicationId, int CreateBy, int CreatorOrganizationId, int OrganizationId, int CardTypeId, string Name, string Family, string Gender, string NationalCode, string MobileNumber, string CardNumber, string CVV2, string FirstPassword, string SecondPassword, int? ExpireDate, int CreditAmount, int CashAmount, byte[] Password, byte[] PasswordSalt);
    }
}      
 