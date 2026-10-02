
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IValidHekmatCardService : IGenericService<ValidHekmatCard,int>
    {
                 
        IQueryable<ValidHekmatCard> GetByCustomFilter( int? hekmatCardType);
        /// <summary>
        /// بررسی میکند شماره کارت در شماره کارت های معتبر موجود هست یا خیر
        /// </summary>
        /// <param name="cardNumber">شماره کرات</param>
        /// <returns></returns>
         bool CheckExistCardNumber(string cardNumber);



    }
}      
 