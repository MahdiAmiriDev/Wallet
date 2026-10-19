using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class YashilHekmatTerminalService : GenericService<YashilHekmatTerminal, int>, IYashilHekmatTerminalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYashilHekmatTerminalRepository _yashilHekmatTerminalRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public YashilHekmatTerminalService(IUnitOfWork unitOfWork, IYashilHekmatTerminalRepository yashilHekmatTerminalRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, yashilHekmatTerminalRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _yashilHekmatTerminalRepository = yashilHekmatTerminalRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<YashilHekmatTerminal> GetByCustomFilter(int? hekmatTerminalId, int? merchantId)
        {
            return _yashilHekmatTerminalRepository.GetByCustomFilter(hekmatTerminalId, merchantId);
        }

        public IQueryable<YashilHekmatTerminal> GetYashilHekmatTerminalByIdAndIp(int merchantId, int terminalId, string ipAddress, int applicationId)
        {
            return _yashilHekmatTerminalRepository.GetYashilHekmatTerminalByIdAndIp(merchantId, terminalId, ipAddress, applicationId);
        }
    }
}
