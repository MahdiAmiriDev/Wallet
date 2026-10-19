using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class YashilHekmatTerminalSupplierService : GenericService<YashilHekmatTerminalSupplier, int>, IYashilHekmatTerminalSupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYashilHekmatTerminalSupplierRepository _yashilHekmatTerminalSupplierRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public YashilHekmatTerminalSupplierService(IUnitOfWork unitOfWork, IYashilHekmatTerminalSupplierRepository yashilHekmatTerminalSupplierRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, yashilHekmatTerminalSupplierRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _yashilHekmatTerminalSupplierRepository = yashilHekmatTerminalSupplierRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }


        public IQueryable<YashilHekmatTerminalSupplier> GetByCustomFilter(int? parentId, int? userId)
        {
            return _yashilHekmatTerminalSupplierRepository.GetByCustomFilter(parentId, userId);
        }

        /// <summary>
        /// دریافت تامین کنندگان مربوط به حکمت
        /// در صورتی کاربر ادمین باشد تمامی تامینن کننده ها را میتواند دریافت کند
        /// در غیر این صورت فقط تامین کننده مربط به کاربر جاری
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public IQueryable<YashilHekmatTerminalSupplier> GetForSelect(bool isAdmin)
        {
            return GetAll(true)
                .Where(x => isAdmin || x.UserId == _userPrincipal.Id);
        }

        /// <summary>
        /// این متد بررسی می کند که شناسه کاربری ارسالی به
        /// شناسه فروشنده ارسالی دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Task<bool> CheckUserHasAccessToHekmatTerminalSupplierAsync(int userId, int supplierId, int? yashilHekmatTerminalId = null)
        {
            return GetAll(true)
                .Where(x => (!yashilHekmatTerminalId.HasValue || x.YashilHekmatTerminalId == yashilHekmatTerminalId.Value))
                .AnyAsync(x => x.UserId == userId && x.SupplierId == supplierId);

        }
    }
}
