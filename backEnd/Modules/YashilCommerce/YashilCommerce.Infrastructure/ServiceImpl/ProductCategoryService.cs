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

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductCategoryService : GenericService<ProductCategory, int>, IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IUnitOfWork unitOfWork,
            IProductCategoryRepository productCategoryRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, productCategoryRepository, userPrincipal, eventPublisher)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public IQueryable<ProductCategory> GetByCustomFilter(int? categoryId, int? productId = null)
        {
            return _productCategoryRepository.GetByCustomFilter(categoryId, productId);
        }

        public void Update(ICollection<ProductCategory> productCategoryForUpdate, int productId)
        {
            var productCategories = _productCategoryRepository.GetByCustomFilter(null, productId,false);
            var forInsert = productCategoryForUpdate.Where(x => !productCategories.Select(d => d.Id).Contains(x.Id));
            var forDelete = productCategories.Where(x => !productCategoryForUpdate.Select(d => d.Id).Contains(x.Id));
            foreach (var productCategory in forDelete)
            {
                _productCategoryRepository.Delete(productCategory.Id);
            }

            foreach (var productCategory in forInsert)
            {
                _productCategoryRepository.Add(productCategory);
            }
        }

        public async Task RemoveProductCategoriesByProductId(int productId, bool saveAfterAdd = false)
        {
            _productCategoryRepository.RemoveProductCategoriesByProductId(productId, saveAfterAdd);
            if (saveAfterAdd)
            {
               await SaveChangeAsync();
            }
        }

        public IQueryable<ProductCategory> GetProductCategories(int productId)
        {
            return _productCategoryRepository.GetProductCategories(productId);
        }
    }
}