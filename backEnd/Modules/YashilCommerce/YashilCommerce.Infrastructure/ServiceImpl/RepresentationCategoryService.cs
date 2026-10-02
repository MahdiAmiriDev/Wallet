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
    public class RepresentationCategoryService : GenericService<RepresentationCategory, int>, IRepresentationCategoryService
    {
        private readonly IRepresentationCategoryRepository _representationCategoryRepository;

        public RepresentationCategoryService(IUnitOfWork unitOfWork, IRepresentationCategoryRepository representationCategoryRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, representationCategoryRepository, userPrincipal, eventPublisher)
        {
            _representationCategoryRepository = representationCategoryRepository;
        }

        public IQueryable<RepresentationCategory> GetByCustomFilter(int? representationId, int? categoryId)
        {
            return _representationCategoryRepository.GetByCustomFilter(representationId, categoryId);
        }

        public IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned)
        {
            return _representationCategoryRepository.GetCompanyCategory(id, onlyRootCategories, assigned);
        }
        /// <summary>
        /// دریافت دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId">شناسه نمایندگی</param>
        ///  <param name="readOnly">شناسه نمایندگی</param>
        /// <returns></returns>
        public IQueryable<RepresentationCategory> GetByRepresentationId(int representationId,bool readOnly)
        {
            return _representationCategoryRepository.GetByRepresentationId(representationId,readOnly);
        }

        public Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId)
        {
            return _representationCategoryRepository.CheckExistRepresentationCategoryByCategoryIdAsync(categoryId);
        }

        public Task DeleteByCategoryIdAsync(int categoryId)
        {
            return _representationCategoryRepository.DeleteByCategoryIdAsync(categoryId);
        }

        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public Task DeleteByRepresentationIdAsync(int representationId)
        {
            return _representationCategoryRepository.DeleteByRepresentationIdAsync(representationId);
        }
    }
}
