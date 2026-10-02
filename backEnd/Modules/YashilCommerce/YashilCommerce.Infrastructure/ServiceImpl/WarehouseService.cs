using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WarehouseService : GenericService<Warehouse, int>, IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUserPrincipal _userPrincipal;

        public WarehouseService(IUnitOfWork unitOfWork, IWarehouseRepository warehouseRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, warehouseRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _warehouseRepository = warehouseRepository;
            _userPrincipal = userPrincipal; _eventPublisher = eventPublisher;
        }


    }
}
