using System;
using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class DependentProductSaleService : GenericService<DependentProductSale, int>, IDependentProductSaleService
    {
        private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IDependentProductSaleRepository _dependentProductSaleRepository;
        private readonly IUserPrincipal _userPrincipal;

        public DependentProductSaleService(IUnitOfWork unitOfWork,
            IDependentProductSaleRepository dependentProductSaleRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
            : base(unitOfWork, dependentProductSaleRepository, userPrincipal,eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _dependentProductSaleRepository = dependentProductSaleRepository;
            _userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }

        public IQueryable<DependentProductSale> GetByCustomFilter(int? mainProduct, int? relatedProduct)
        {
            return _dependentProductSaleRepository.GetByCustomFilter(mainProduct, relatedProduct);
        }

        public void UpdateMainProductDependentProducts(List<int> dependentProducts, in int mainProductId)
        {
            var dependentProductSales = _dependentProductSaleRepository.GetByCustomFilter(mainProductId, null);

            var forInsert = dependentProducts.Where(x => !dependentProductSales.Select(d => d.Id).Contains(x));
            var forDelete = dependentProductSales.Where(x => !dependentProducts.Contains(x.Id));
            foreach (var productCategory in forDelete)
            {
                _dependentProductSaleRepository.Delete(productCategory);
            }

            foreach (var dependentProductSalePlanning in forInsert)
            {
                _dependentProductSaleRepository.Add(new DependentProductSale()
                {
                    CreateBy = _userPrincipal.Id,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    RelatedProductSalePlanningId = dependentProductSalePlanning,
                    MainProductSalePlanningId = mainProductId
                });
            }
        }
    }
}