using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IPersonBankAccountRepository : IGenericRepository<PersonBankAccount, int>
    {

        IQueryable<PersonBankAccount> GetByCustomFilter(int? personId, int? bankType, int? representationId);


        IQueryable<PersonBankAccount> GetByCardNumber(string cardNumber);
        IQueryable<PersonBankAccount> GetIncludePerson(int id);
        IQueryable<PersonBankAccount> FindById(int id, bool readOnly = false);
    }
}
