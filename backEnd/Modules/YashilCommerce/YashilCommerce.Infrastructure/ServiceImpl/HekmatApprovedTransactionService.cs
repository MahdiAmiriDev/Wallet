using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class HekmatApprovedTransactionService : GenericService<HekmatApprovedTransaction, int>, IHekmatApprovedTransactionService
    {
        private readonly IHekmatApprovedTransactionRepository _hekmatApprovedTransactionRepository;
        public HekmatApprovedTransactionService(IUnitOfWork unitOfWork, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IHekmatApprovedTransactionRepository hekmatApprovedTransactionRepository) : base(unitOfWork, hekmatApprovedTransactionRepository, userPrincipal, eventPublisher)
        {
            _hekmatApprovedTransactionRepository = hekmatApprovedTransactionRepository;
        }

        public async Task DeleteDuplicateHekmatApprovedTransactionsAsync()
        {
            await _hekmatApprovedTransactionRepository.DeleteDuplicateHekmatApprovedTransactionsAsync();
        }
        /// <summary>
        /// به روز رسانی تراکنش های حکمتی که اصلاح قیمت برای آنها رخ داده است
        /// </summary>
        /// <returns></returns>
        public async Task UpdateModifiedHekmatTransactionAsync()
        {
            await _hekmatApprovedTransactionRepository.UpdateModifiedHekmatTransactionAsync();
        }
    }
}
