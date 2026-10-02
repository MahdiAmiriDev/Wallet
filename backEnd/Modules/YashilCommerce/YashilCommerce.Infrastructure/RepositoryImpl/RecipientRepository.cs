using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class RecipientRepository : GenericApplicationBasedRepository<Recipient, int>, IRecipientRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public RecipientRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<Recipient> GetByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null)
        {
            var query = GetAll(true);
            if (userId.HasValue)
            {
                query = query.Where(x => x.CreateBy == userId.Value);
            }
            if (addressId.HasValue)
            {
                query = query.Where(x => x.AddressId == addressId.Value);
            }
            if (membershipType.HasValue)
            {
                query = query.Where(x => x.MembershipType == membershipType.Value);
            }

            if (!string.IsNullOrEmpty(nationalCode))
            {
                query = query.Where(x => x.NationalCode == nationalCode);
            }
            return query;
        }

        public int GetCountByCustomFilter(int? userId, int? addressId, int? membershipType, string nationalCode = null)
        {
            return GetByCustomFilter(userId, addressId, membershipType, nationalCode).Count();
        }

        public override void CustomiseEntityBeforeAdd(Recipient t)
        {
            if (t.Address != null)
            {
                SetApplicationBasedEntityDefaultPropValues(t.Address);
                SetEntityDefaultPropValues(t.Address);
            }
            base.CustomiseEntityBeforeAdd(t);
        }

        public override Recipient Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.Address).ThenInclude(x => x.City).FirstOrDefault(x => x.Id == (int)id);
        }

        public override ValueTask<Recipient> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var firstOrDefaultAsync = DbSet.Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<Recipient>(firstOrDefaultAsync);
        }

        public Address CloneRecipientAddress(int recipientId)
        {
            var address = _context.Recipient.FirstOrDefault(x => x.Id == recipientId)?.Address;
            if (address != null)
            {
                var values = _context.Entry(address).CurrentValues.Clone();
                Address newAddress = new Address();
                _context.Entry(newAddress).CurrentValues.SetValues(values);
                newAddress.Id = 0;
                return newAddress;
            }
            return null;
        }

        public IQueryable<Recipient> GetUserRecipients(int userId)
        {
            return DbSet.Where(x => x.CreateBy == userId);
        }
        public IQueryable<Recipient> GetUserRecipientsByApplicationId(int userId, int applicationId)
        {
            return DbSet.Where(x => x.CreateBy == userId && x.ApplicationId == applicationId);
        }
        public IQueryable<Recipient> GetIncludeAddress(int recipientId)
        {
            return DbSet.Include(x => x.Address).ThenInclude(x => x.City).Where(x => x.Id == recipientId);

        }
        public IQueryable<Recipient> GetUserRecipient(int recipientId, bool readOnly)
        {
            var userId = _userPrincipal.Id;
            return DbSet.Where(x => x.Id == recipientId && x.CreateBy == userId);
        }
    }
}
