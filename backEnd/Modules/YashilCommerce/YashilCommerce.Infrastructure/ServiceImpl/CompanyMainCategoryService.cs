using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CompanyMainCategoryService : GenericService<CompanyMainCategory, int>, ICompanyMainCategoryService
    {
        private readonly ICompanyMainCategoryRepository _companyMainCategoryRepository;
        private readonly IRepresentationCategoryService _representationCategoryService;

        public CompanyMainCategoryService(IUnitOfWork unitOfWork,
            ICompanyMainCategoryRepository companyMainCategoryRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            IRepresentationCategoryService representationCategoryService)
            : base(unitOfWork, companyMainCategoryRepository, userPrincipal, eventPublisher)
        {
            _companyMainCategoryRepository = companyMainCategoryRepository;
            _representationCategoryService = representationCategoryService;
        }

        public async Task<bool> AssignSelectedItemsToCompany(List<int> mainCategories,
            int companyId, bool assign)
        {
            if (!assign)
            {
                var existCategoryInRepresentationCategory = await _representationCategoryService
                    .GetAll()
                    .AnyAsync(x => x.Representation.CompanyId == companyId &&
                                   mainCategories.Contains(x.CategoryId));
                if (existCategoryInRepresentationCategory)
                {
                    return false;
                }
            }
            await _companyMainCategoryRepository.AssignSelectedItemsToCompany(
                mainCategories, companyId, assign);


            try
            {
                await SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCompanyMainCategory(int? id)
        {
            return _companyMainCategoryRepository.GetMainCourseCategoriesByCompanyMainCategory(id);
        }
        public IQueryable<CompanyMainCategory> GetMainCategoryByRepresentationId(int id)
        {
            return _companyMainCategoryRepository.GetMainCategoryByRepresentationId(id);
        }

        public IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCategoryId(int categoryId, bool hierarchical = true)
        {
            return _companyMainCategoryRepository.GetMainCourseCategoriesByCategoryId(categoryId, hierarchical);
        }
        public IQueryable<CompanyMainCategory> GetByCustomFilter(int? companyId, int? mainCategoryId)
        {
            return _companyMainCategoryRepository.GetByCustomFilter(companyId, mainCategoryId);
        }

        public IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned)
        {
            return _companyMainCategoryRepository.GetCompanyCategory(id, onlyRootCategories, assigned);
        }
        /// <summary>
        /// بررسی می کند که آیا دسته بندی محصول از دسته بندی اصلی وجود دارد
        /// </summary>
        /// <param name="companyMainCategoryId">شناسه دسته بندی اصلی مرکز</param>
        /// <returns></returns>
        public Task<bool> CheckExistsProductCategoryOfMainCategoryAsync(int companyMainCategoryId)
        {
            return _companyMainCategoryRepository.CheckExistsProductCategoryOfMainCategoryAsync(companyMainCategoryId);
        }

        public Task<bool> IsExistCompanyMainCategoryAsync(int companyId)
        {
            return GetByCustomFilter(companyId, null).AnyAsync();
        }

        public Task<bool> CheckExistCompanyMainCategoryByCategoryIdAsync(int categoryId)
        {
            return _companyMainCategoryRepository.CheckExistCompanyMainCategoryByCategoryIdAsync(categoryId);
        }

        public IQueryable<CompanyMainCategory> FindById(int id, bool readOnly = true)
        {
            return _companyMainCategoryRepository.FindById(id, readOnly);
        }

        public Task DeleteByCategoryIdAsync(int categoryId)
        {
            return _companyMainCategoryRepository.DeleteByCategoryIdAsync(categoryId);
        }
        /// <summary>
        /// حذف بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Task DeleteByCompanyIdAsync(int companyId)
        {
            return _companyMainCategoryRepository.DeleteByCompanyIdAsync(companyId);
        }
    }
}