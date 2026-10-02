using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class VocherService : GenericService<Vocher, int>, IVocherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVocherRepository _vocherRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IYashilLogger<Product> _logger;


        public VocherService(IUnitOfWork unitOfWork, IVocherRepository vocherRepository,
            ApplicationConfiguration applicationConfiguration,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IYashilLogger<Product> logger)
            : base(unitOfWork, vocherRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _vocherRepository = vocherRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        public void AssignVocherForOrderItems(int applicationId)
        {
            _vocherRepository.AssignVoucherForOrderItems(applicationId);
        }


        public IQueryable<Vocher> GetByCustomFilter(int? vocherTypeId)
        {
            return _vocherRepository.GetByCustomFilter(vocherTypeId);
        }
        /// <summary>
        /// دریافت ووچر بر اساس آیتم سفارش
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        public Vocher GetByOrderItemId(int orderItemId)
        {
            return _vocherRepository.GetByOrderItemId(orderItemId);
        }

        public Vocher GetNotUsedVocherByOrderItemId(int OrderItemId)
        {
            return _vocherRepository.GetNotUsedVoucherByOrderItemId(OrderItemId);
        }

        /// <summary>
        /// دریافت ووچر استفاده نشده بر اساس نوع ووچر
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        public Vocher GetNotUsedVocherByVocherTypeId(int vocherTypeId)
        {
            return _vocherRepository.GetNotUsedVoucherByVoucherTypeId(vocherTypeId, _applicationConfiguration.ApplicationId);
        }
        /// <summary>
        /// درج کد کوپن رمز شده از روی جدول
        /// UploadedVoucher
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        public Task<int> AddVoucherFromUploadedVoucher(int applicationId, int userId, int companyId,
            int voucherTypeId)
        {
            try
            {

                var res = _vocherRepository.AddVoucherFromUploadedVoucher(applicationId, userId, companyId, voucherTypeId);
                _eventPublisher.EntityInsertedAsync<Vocher, int>(null);
                return res;

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(1, Events.Insert.ToString()), nameof(Product), "خطا در اجرای sp AddDataFromUploadedProductItem", e);
                return null;
            }
        }
    }
}
