using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CategoryGroupService : GenericService<CategoryGroup, int>, ICategoryGroupService
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public CategoryGroupService(
            IUnitOfWork unitOfWork,
            IAppConfigService appConfigService,
            ICategoryGroupRepository categoryGroupRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            IStaticCacheManager staticCacheManager, ApplicationConfiguration applicationConfiguration, IAdditionalUserPropService additionalUserPropService)
            : base(unitOfWork, categoryGroupRepository, userPrincipal, eventPublisher)
        {
            _categoryGroupRepository = categoryGroupRepository;
            _userPrincipal = userPrincipal;
            _staticCacheManager = staticCacheManager;
            _applicationConfiguration = applicationConfiguration;
            _additionalUserPropService = additionalUserPropService;
            _appConfigService = appConfigService;
        }

        /// <summary>
        /// این متد برای دریافت دسته بندی های گروه اپلیکیشن جاری کاربرد دارد
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryGroupInfoDto>> GetCurrentApplicationCategoryGroups()
        {


            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<CategoryGroup>.CategoryGroupKey,
                _applicationConfiguration.ApplicationId);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var employmentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(ApplicationConfigKey
                    .YashilCommerceCategoryGroupEmploymentCategoryGroupId, _applicationConfiguration.ApplicationId);
                var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                return await _categoryGroupRepository
                    .GetCategoryGroupByApplicationId(
                        employmentCategoryGroupId,
                        _applicationConfiguration.ApplicationId,
                        userAdditionalUserProp.CustomerClubId)
                    .OrderBy(c => c.DisplayOrder)
                    .Select(categoryGroup => new CategoryGroupInfoDto
                    {
                        Id = categoryGroup.Id,
                        Title = categoryGroup.Title

                    }).ToListAsync();

            });
        }

        public async Task<string> GetTitleAsync(int categoryGroupId)
        {
            return await _categoryGroupRepository.FindById(categoryGroupId,_applicationConfiguration.ApplicationId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();
        }
    }
}
