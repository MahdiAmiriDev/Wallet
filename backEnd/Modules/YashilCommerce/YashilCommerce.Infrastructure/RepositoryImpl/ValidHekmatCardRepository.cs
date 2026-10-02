			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ValidHekmatCardRepository : GenericApplicationBasedRepository<ValidHekmatCard,int>, IValidHekmatCardRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ValidHekmatCardRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
     public IQueryable<ValidHekmatCard> GetByCustomFilter( int? hekmatCardType)
        {
            var query= GetAll(true);
                         if ( hekmatCardType.HasValue)
                {
                    query = query.Where(x => x.HekmatCardType == hekmatCardType.Value);
                }
                  
          
            return query;
        }

        /// <summary>
        /// بررسی میکند شماره کارت در شماره کارت های معتبر موجود هست یا خیر
        /// </summary>
        /// <param name="cardNumber">شماره کرات</param>
        /// <returns></returns>
        public bool CheckExistCardNumber(string cardNumber)
        {

            return DbSet.Any(x => x.CardNumber == cardNumber);
        }



    }
}      
