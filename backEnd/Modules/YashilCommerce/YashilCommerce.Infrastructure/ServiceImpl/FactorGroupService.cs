using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class FactorGroupService : GenericService<FactorGroup, int>, IFactorGroupService
    {
        private readonly IFactorGroupRepository _factorGroupRepository;
        private readonly IFactorItemService _factorItemService;
        private readonly IFactorService _factorService;

        public FactorGroupService(IUnitOfWork unitOfWork, IFactorGroupRepository factorGroupRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher, IFactorItemService factorItemService,
            IFactorService factorService)
            : base(unitOfWork, factorGroupRepository, userPrincipal, eventPublisher)
        {
            _factorGroupRepository = factorGroupRepository;
            _factorItemService = factorItemService;
            _factorService = factorService;
        }

        public IQueryable<FactorGroup> GetByCustomFilter(int? parentId)
        {
            return _factorGroupRepository.GetByCustomFilter(parentId);
        }

        public async Task<(bool isSuccessful, string message)> ConfirmFactorGroup(int factorGroupId,
            bool saveAfterAddOrUpdate = false)
        {
            // دریافت گروه بندی فاکتور
            var factorGroup = Get(factorGroupId, false);
            if (factorGroup != null)
            {
                // وضعیت گروه بندی فاکتور باید در حالت ثبت واولیه باشد
                // همچنین باید آیتمهای فاکتور در خود داشته باشد.
                if (factorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial)
                {
                    if (!CheckFactorGroupIsParent(factorGroup))
                    {
                        // بررسی میکنیم که برای گروه داده شده داده های مجاز وارد شده است یا خیر
                        // یعنی باید نام کالا، کد کالا و واحد سنجش مقدار داشته باشند.
                        var isValidDataForConvert =
                            CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(factorGroup.Id);
                        if (!isValidDataForConvert)
                        {
                            return (false, "کاربر گرامی ممکن است یک یا بیش از یکی از مقادیر نام کالا، کد کالا و یا واحد سنجش مقدار نداشته باشند، لطفا مقادیر مشخص شده را اصلاح کنید ");
                        }
                        _factorItemService.SetFactorGroupIdForDeletedFactoredOrderItems(factorGroup.Id, factorGroup.ApplicationId);
                        factorGroup.FactorGroupStatus = (int)FactorGroupStatusTypeEnum.Confirmed;
                        factorGroup.ConfirmDate = DateTime.Now;

                        var result = await UpdateAsync(factorGroup, factorGroup.Id, saveAfterAddOrUpdate);
                        if (result != null)
                        {
                            return (true, string.Empty);
                        }
                    }

                    return (false, "کاربر گرامی این گروه نمیتواند تایید شود");
                }

                return (false, "کاربر گرامی این گروه قبلا ثبت نهایی  شده است.");
            }

            return (false, "کاربر گرامی گروه بندی فاکتور مورد نظر وجود ندارد");
        }

        public bool CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(int factorGroupId)
        {
            return _factorGroupRepository.CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(factorGroupId);
        }

        public bool CheckFactorGroupIsParent(FactorGroup factorGroup)
        {
            return _factorGroupRepository.CheckFactorGroupIsParent(factorGroup);
        }

        public bool CheckFactorGroupIsParent(int factorGroupId)
        {
            return _factorGroupRepository.CheckFactorGroupIsParent(factorGroupId);
        }

        public IQueryable<FactorGroup> GetForSelectParent()
        {
            return _factorGroupRepository.GetForSelectParent();
        }

        public override Task<FactorGroup> AddAsync(FactorGroup t, bool saveAfterAdd = false, bool publishEvent = true)
        {
            t.FactorGroupStatus = (int)FactorGroupStatusTypeEnum.Initial;
            return base.AddAsync(t, saveAfterAdd);
        }

        public bool CheckParentHasAnyFactorGroup(FactorGroup factorGroup)
        {
            return _factorGroupRepository.CheckParentHasAnyFactorGroup(factorGroup);
        }

        public IQueryable<FactorGroupConvertReport> GetFactorGroupConvert(int factorGroupId)
        {
            return _factorGroupRepository.GetFactorGroupConvertReport(factorGroupId);
        }

        public FactorGroup GetFactorGroupIfConfirm(int factorGroupId, bool readOnly)
        {
            return _factorGroupRepository.GetFactorGroupIfConfirm(factorGroupId, readOnly);
        }

        public Task<int> FixMissedOrderItemPriceRightShareAsync()
        {
            return _factorGroupRepository.FixMissedOrderItemPriceRightShareAsync();
        }

        public async Task<bool> checkIsValidOrderItemPriceRightShareAsync()
        {
            var result = await _factorGroupRepository.GetMissedOrderItemPriceRightShareCount();
            return result == 0;
        }

        public IQueryable<FactorGroup> FindById(int id, bool readOnly = true)
        {
            return _factorGroupRepository.FindById(id, readOnly);
        }

        public async Task<int> RemovingFactorsFromFactorGroupAsync(int id)
        {
            return await _factorService
                .GetByFactorGroupId(id)
                .UpdateFromQueryAsync(f => new Factor
                {
                    FactorGroupId = null,
                    ModificationDate = DateTime.Now

                });
        }

        public async Task<(bool IsSucces, string Message)> ChangeFactorGroupStateToInitialAsync(int factorGroupId)
        {
            var factorGroup = await
                FindById(factorGroupId)
                    .FirstOrDefaultAsync();

            if (factorGroup == null)
            {
                return (false, "کاربر گرامی گروه بندی برای این رکورد یافت نشد.");
            }

            if (factorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial)
            {
                return (false, "کاربر گرامی گروه فاکتور در وضعیت ثبت اولیه می باشد.");
            }
            factorGroup.FactorGroupStatus = (int)FactorGroupStatusTypeEnum.Initial;
            factorGroup.ConfirmDate = null;
            await UpdateAsync(factorGroup, factorGroup.Id);
            await SaveChangeAsync();
            return (true, string.Empty);
        }

        public Task<int> AddFactorGroupItemAsync(int factorGroupId)
        {
           return _factorGroupRepository.AddFactorGroupItemAsync(factorGroupId);
        }
    }
}