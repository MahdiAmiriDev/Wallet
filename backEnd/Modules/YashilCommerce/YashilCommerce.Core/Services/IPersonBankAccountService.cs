using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IPersonBankAccountService : IGenericService<PersonBankAccount, int>
    {

        IQueryable<PersonBankAccount> GetByCustomFilter(int? personId, int? bankType, int? representationId);


        Task<PersonBankAccountCustomDomainModel> GetByCardNumberAsync(string cardNumber);
        IQueryable<PersonBankAccount> GetIncludePerson(int id);

        /// <summary>
        /// دریافت اطلاعات حساب بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه اطلاعات حساب </param>
        /// <param name="readOnly">فقط خواندنی</param>
        /// <returns></returns>
        IQueryable<PersonBankAccount> FindById(int id, bool readOnly = false);
    }
}
