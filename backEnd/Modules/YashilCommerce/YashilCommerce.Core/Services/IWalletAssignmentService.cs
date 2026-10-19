using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletAssignmentService
    {
        /// <summary>Assignmentهای Program را برای مدیریت در Application جاری برمی‌گرداند.</summary>
        IQueryable<WalletAssignmentDto> GetAll(int walletProgramId);
        /// <summary>Assignment مشخص را فقط در Application جاری می‌خواند.</summary>
        Task<WalletAssignmentDto> GetAsync(int id);
        /// <summary>کاربران فعال Application را برای انتخاب Target برمی‌گرداند.</summary>
        IQueryable<WalletAssignmentUserDto> GetUsers();
        /// <summary>عضویت دستیِ صفرمانده را بدون Grant یا Ledger ایجاد می‌کند.</summary>
        Task<WalletAssignmentResultDto> CreateAsync(WalletAssignmentCreateDto request);
    }
}
