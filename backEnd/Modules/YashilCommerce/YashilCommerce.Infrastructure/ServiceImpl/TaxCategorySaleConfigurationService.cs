using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class TaxCategorySaleConfigurationService : GenericService<TaxCategorySaleConfiguration, int>, ITaxCategorySaleConfigurationService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly ITaxCategorySaleConfigurationRepository _taxCategorySaleConfigurationRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public TaxCategorySaleConfigurationService (IUnitOfWork unitOfWork, ITaxCategorySaleConfigurationRepository taxCategorySaleConfigurationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, taxCategorySaleConfigurationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_taxCategorySaleConfigurationRepository = taxCategorySaleConfigurationRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
         
        
        public IQueryable<TaxCategorySaleConfiguration> GetByCustomFilter( int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType)
        {
            return _taxCategorySaleConfigurationRepository.GetByCustomFilter(parentId,taxCategoryId,saleConfigurationId,factorReportType);
        }



        /// <summary>
        /// دریافت دسته بندی مالیاتی که قبلا برای تنظیم فروش مقداری درج نشده
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="taxCategoryId"></param>
        /// <returns></returns>
        public Task<bool> CheckAnyTaskCategorySaleConfigurationInsertedBefore(int taxCategoryId, int saleConfigurationId)
        {
            return GetAll(true)
                .Where(x => x.SaleConfigurationId == saleConfigurationId &&  x.TaxCategoryId == taxCategoryId)
                .AnyAsync();
        }

    }
}      
 