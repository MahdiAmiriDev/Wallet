using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
    public interface IYashilHekmatTerminalService : IGenericService<YashilHekmatTerminal, int>
    {

        IQueryable<YashilHekmatTerminal> GetByCustomFilter(int? hekmatTerminalId, int? merchantId);


        IQueryable<YashilHekmatTerminal> GetYashilHekmatTerminalByIdAndIp(int merchantId, int terminalId, string ipAddress, int applicationId);
    }
}
