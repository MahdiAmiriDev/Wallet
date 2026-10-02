using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;


namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class TaxCategoryService : GenericService<TaxCategory, int>, ITaxCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly ITaxCategoryRepository _taxCategoryRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ITaxCategorySaleConfigurationService _taxCategorySaleConfigurationService;

        public TaxCategoryService(IUnitOfWork unitOfWork, ITaxCategoryRepository taxCategoryRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, ITaxCategorySaleConfigurationService taxCategorySaleConfigurationService)
            : base(unitOfWork, taxCategoryRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _taxCategoryRepository = taxCategoryRepository;
            _userPrincipal = userPrincipal; _eventPublisher = eventPublisher;
            _taxCategorySaleConfigurationService = taxCategorySaleConfigurationService;
        }
        public IQueryable<TaxCategory> FindById(int id)
        {
            return _taxCategoryRepository.FindById(id);
        }

        /// <summary>
        /// این متد
        ///TaxCategory
        /// هایی را بر میگردااند که
        /// برای آن ها در
        /// taxCategorySaleConfiguration
        /// مقداری درج نشده است
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="taxCategorySaleConfigurationId"></param>
        /// <returns></returns>
        public IQueryable<TaxCategory> GetTaxCategoryNotUsedInTaxCategorySaleConfiguration(int saleConfigurationId , int? taxCategorySaleConfigurationId)
        {
            var taxCategoryIdsThatUsedInSaleConfiguration = _taxCategorySaleConfigurationService.GetAll(true)
                .Where(x => x.SaleConfigurationId == saleConfigurationId && 
                            (!taxCategorySaleConfigurationId.HasValue || x.Id != taxCategorySaleConfigurationId))
                .Select(x => x.TaxCategoryId);

            return GetAll(true)
                .Where(x => !taxCategoryIdsThatUsedInSaleConfiguration.Contains(x.Id));
        }








    }
}
