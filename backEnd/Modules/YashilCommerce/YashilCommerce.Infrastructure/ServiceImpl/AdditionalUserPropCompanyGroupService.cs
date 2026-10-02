using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents; 
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class AdditionalUserPropCompanyGroupService : GenericService<AdditionalUserPropCompanyGroup, int>, IAdditionalUserPropCompanyGroupService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IAdditionalUserPropCompanyGroupRepository _additionalUserPropCompanyGroupRepository;
        private readonly IAdditionalUserPropRepository _additionalUserPropRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        private readonly IYashilLogger<UserApplication> _logger;

        public AdditionalUserPropCompanyGroupService(IUnitOfWork unitOfWork, IAdditionalUserPropCompanyGroupRepository additionalUserPropCompanyGroupRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IYashilLogger<UserApplication> logger, IAdditionalUserPropRepository additionalUserPropRepository)
            : base(unitOfWork, additionalUserPropCompanyGroupRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _additionalUserPropCompanyGroupRepository = additionalUserPropCompanyGroupRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _additionalUserPropRepository = additionalUserPropRepository;
        }


        public IQueryable<AdditionalUserPropCompanyGroup> GetByCustomFilter( int? additionalUserPropId, int? companyGroupId)
        {
            return _additionalUserPropCompanyGroupRepository.GetByCustomFilter(additionalUserPropId,companyGroupId);
        }
        /// <summary>
        /// این متد در فرم تخصیص کاربر به گروه مرکز کاربرد دارد
        /// گروه مرکزهایی را که به کاربر اختصاص داده نشده اند یا گروه مرکزهایی را که به کاربر اختصاص داده شده اند،را باتوجه به پارامتر اساین لیست میکند 
        /// اگر پارامتر اساین ترو باشد، با توجه به ای دی کاربر از جدول
        /// AdditionalUserPropCompanyGroup
        /// AssignedCompanyGroup
        /// ها را لیست میکند
        /// اگر پارامتر اساین فالس باشد
        /// از جدول AdditionalUserPropCompanyGroup 
        /// گروه مرکزهایی که به این کاربر اختصاص داده شده اند را سلکت کرده و و در جدول 
        /// CompanyGroup
        /// رکوردهایی که شامل لیست اپلیکیشن های بالا نیست را برمیگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assigned"></param>
        /// <returns></returns>
        public IQueryable<CompanyGroup> GetAdditionalUserPropCompanyGroups(int userId, bool assigned)
        {
            return _additionalUserPropCompanyGroupRepository.GetUserCompanyGroups(userId, assigned);
        }
        /// <summary>
        /// این متد برای تخصیص گروه مرکز به کاربر و خارج کردن تخصیص کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="companyGroupsId">لیست گروه های کاربر مورد نظر</param>
        /// <param name="userId">ای دی کاربر مورد نظر</param>
        /// <param name="assign">تخصیص داده شده یا نشده ، مقدار true یا false برمی گرداند</param>
        /// 
        /// <returns></returns>
        public async Task<bool> AssignSelectedCompanyGroupesToUser(List<int> companyGroupsId, int userId, bool assign)
        {
            var additionalUserProp = _additionalUserPropRepository.GetByUserId(userId,true);
            if (additionalUserProp == null) return false;
            await _additionalUserPropCompanyGroupRepository.AssignSelectedCompanyGroupesToUser(companyGroupsId, additionalUserProp.Id, assign);
            try
            {
                await SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.AccessDeniedGetEntityForEdit, Events.AccessDeniedGetEntityForEdit.ToString()), nameof(AdditionalUserPropCompanyGroup), "AssignSelectedApplicationsToUser Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntityForEdit} : {nameof(AdditionalUserPropCompanyGroup)}");
            }
        }

    }
}      
 