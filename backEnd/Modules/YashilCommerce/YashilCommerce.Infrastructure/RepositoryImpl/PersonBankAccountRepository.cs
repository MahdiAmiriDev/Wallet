using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class PersonBankAccountRepository : GenericApplicationBasedRepository<PersonBankAccount, int>,
        IPersonBankAccountRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public PersonBankAccountRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<PersonBankAccount> GetByCustomFilter(int? personId, int? bankType, int? representationId)
        {
            var query = GetAll(true).Where(x => !string.IsNullOrWhiteSpace(x.AccountNumber));
            if (personId.HasValue)
            {
                query = query.Where(x => x.PersonId == personId.Value);
            }

            if (bankType.HasValue)
            {
                query = query.Where(x => x.BankType == bankType.Value);
            }
            if (representationId.HasValue)
            {
                var representation = _context.Representation.FirstOrDefault(x => x.Id == representationId.Value);
                if (representation != null)
                {
                    query = query.Where(x => x.PersonId == representation.FounderId);
                }

            }
            return query;
        }

        public IQueryable<PersonBankAccount> GetByCardNumber(string cardNumber)
        {
            return DbSet.Where(x => x.CardNumber == cardNumber);
        }

        public IQueryable<PersonBankAccount> GetIncludePerson(int id)
        {
            return DbSet.Where(x => x.Id == id);
        }

        public IQueryable<PersonBankAccount> FindById(int id, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.Id == id);
            return readOnly ? query.AsNoTracking() : query;
        }
    }
}