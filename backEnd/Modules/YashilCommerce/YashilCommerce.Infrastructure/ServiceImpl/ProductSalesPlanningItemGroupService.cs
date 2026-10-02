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
using Yashil.Common.SharedKernel.Logging;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using Microsoft.Extensions.Logging;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductSalesPlanningItemGroupService : GenericService<ProductSalesPlanningItemGroup, int>, IProductSalesPlanningItemGroupService
    {
        private readonly IProductSalesPlanningItemGroupRepository _productSalesPlanningItemGroupRepository;
        private readonly IYashilLogger<ProductSalesPlanningItemGroup> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSalesPlanningItemGroupService(IUnitOfWork unitOfWork, IProductSalesPlanningItemGroupRepository productSalesPlanningItemGroupRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IYashilLogger<ProductSalesPlanningItemGroup> logger)
            : base(unitOfWork, productSalesPlanningItemGroupRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _productSalesPlanningItemGroupRepository = productSalesPlanningItemGroupRepository;
            _logger = logger;
        }

        public IQueryable<ProductSalesPlanningItemGroup> GetByCustomFilter(int? parentId, int? itemGroupId, int? productSalesPlanningId)
        {
            return _productSalesPlanningItemGroupRepository.GetByCustomFilter(parentId, itemGroupId, productSalesPlanningId);
        }

        public IQueryable<ProductSalesPlanningItemGroup> GetItemGroupByProductSalePlanningId(int productSalePlanningId, bool readOnly = false)
        {
            return _productSalesPlanningItemGroupRepository
                .GetItemGroupByProductSalePlanningId(productSalePlanningId, readOnly);


        }

        public async Task<(bool isSucces, string message)> AssignProductSalePlanningToItemGroupAsync
            (int productSalePlanningId, List<int> groupsId)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var groupsIdsNotAssignForProductSalesPlanning = _productSalesPlanningItemGroupRepository
                    .GetGroupsIdsNotAssignForProductSalesPlanning(productSalePlanningId, groupsId);


                await _productSalesPlanningItemGroupRepository
                    .DeleteProductSalePlanningItemGroupsNotExistsInGroupsIdAsync(productSalePlanningId, groupsId);

                foreach (var pspIg in groupsIdsNotAssignForProductSalesPlanning.Select(groupId =>
                           new ProductSalesPlanningItemGroup
                           {
                               ProductSalesPlanningId = productSalePlanningId,
                               ItemGroupId = groupId
                           }))
                {

                    await AddAsync(pspIg);
                }
                await SaveChangeAsync();
                await transaction.CommitAsync();
                return (true, string.Empty);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)Events.InsertException,
                        Events.GetEntityException.ToString()), nameof(ProductSalesPlanningItemGroup),
                    "خطا در درج آیتم های گروه برنامه ریزی دوره", e);
                return (false, "کاربر گرامی در اختصاص برنامه ریزی دوره به آیتم های گروه خطا رخ داده است");
            }
        }

        public Task<bool> CheckExistProductSalePlanningToItemGroupAsync(int productSalesPlanningId, int itemGroupId)
        {
            return _productSalesPlanningItemGroupRepository.CheckExistProductSalePlanningToItemGroupAsync(productSalesPlanningId, itemGroupId);
        }

        public Task<List<int>> GetItemGroupIdsAsync(int productSalesPlanningId)
        {
            return GetByCustomFilter(null, null, productSalesPlanningId)
                .Select(x => x.ItemGroupId)
                .ToListAsync();
        }

    }
}
