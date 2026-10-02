using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using Yashil.Common.SharedKernel.Logging;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CategoryCustomerClubService : GenericService<CategoryCustomerClub, int>, ICategoryCustomerClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryCustomerClubRepository _categoryCustomerClubRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerClubService _customerClubService;
        private readonly IYashilLogger<CategoryCustomerClub> _logger;
        public CategoryCustomerClubService(IUnitOfWork unitOfWork, ICategoryCustomerClubRepository categoryCustomerClubRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, ICustomerClubService customerClubService, IYashilLogger<CategoryCustomerClub> logger)
            : base(unitOfWork, categoryCustomerClubRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _categoryCustomerClubRepository = categoryCustomerClubRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _customerClubService = customerClubService;
            _logger = logger;
        }
        public IQueryable<CategoryCustomerClub> GetByCustomFilter(int? customerClubId, int? categoryId, int? applicationId)
        {
            return _categoryCustomerClubRepository.GetByCustomFilter(customerClubId, categoryId, applicationId);
        }

        public async Task<string> CheckExistsCategoryCustomerClubAndAddIfNeededByCategoryIdAsync(int categoryId)
        {
            var existsAnyCategoryCustomerClub = await _categoryCustomerClubRepository.GetByCustomFilter(null, categoryId,null).AnyAsync(x => x.CategoryId == categoryId);

            if (existsAnyCategoryCustomerClub)
            {
                return string.Empty;
            }
            try
            {
                var customerClubs = await _customerClubService.GetAll(true).Select(x => new
                {
                    x.Id
                }).ToListAsync();

                List<Task<CategoryCustomerClub>> addedItems = new List<Task<CategoryCustomerClub>>();
                foreach (var customerClub in customerClubs)
                {
                    addedItems.Add(AddAsync(new CategoryCustomerClub
                    {

                        CustomerClubId = customerClub.Id,
                        CategoryId = categoryId,

                    }, false));
                }
                await Task.WhenAll(addedItems);
                await SaveChangeAsync();
                return string.Empty;

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.InsertException, Events.InsertException.ToString()), nameof(CategoryCustomerClub), "بروز خطا در هنگام درج categoryCustomerClub", e);
                return " کاربر گرامی عملیات درج در دسته بندی باشگاه مشتریان با خطا مواجه شد";
            }
        }

       

    }
}
