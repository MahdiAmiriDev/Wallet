using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ItemGroupService : GenericService<ItemGroup, int>, IItemGroupService
    {
        private readonly IItemGroupRepository _itemGroupRepository;


        public ItemGroupService(IUnitOfWork unitOfWork, IItemGroupRepository itemGroupRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, itemGroupRepository, userPrincipal, eventPublisher)
        {
            _itemGroupRepository = itemGroupRepository;
        }

        public IQueryable<ItemGroup> GetByCustomFilter(int? parentId, int appEntityId, int? itemGroupTypeSubId)
        {
            return _itemGroupRepository.GetByCustomFilter(parentId, appEntityId, itemGroupTypeSubId);
        }

        public bool CheckItemGroupIsParent(int itemGroupId)
        {
            return _itemGroupRepository.CheckItemGroupIsParent(itemGroupId);
        }

        public async Task<string> ConfirmItemGroup(int id, bool saveAfterAddOrUpdate)
        {
            // دریافت گروه بندی آیتم
            var itemGroup = Get(id);
            if (itemGroup != null)
            {
                // وضعیت گروه بندی باید در حالت ثبت اولیه باشد
                // همچنین باید آیتمهای گروه در خود داشته باشد.
                if (itemGroup.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial)
                {
                    var itemGroupIsParent = CheckItemGroupIsParent(itemGroup.Id);
                    if ( !itemGroupIsParent)
                    {
                        itemGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                        itemGroup.ConfirmDate = DateTime.Now;
                        var result = await UpdateAsync(itemGroup, itemGroup.Id, saveAfterAddOrUpdate);
                        if (result != null)
                        {
                            return null;
                        }
                    }
                    return "کاربر گرامی این گروه نمیتواند تایید شود";
                }
                return "کاربر گرامی این گروه قبلا ثبت نهایی  شده است.";
            }
            return "کاربر گرامی گروه بندی مورد نظر وجود ندارد";
        }

        public IQueryable<ItemGroup> GetPaymentItemGroupThatCanBeParent(int appEntityId, int customerClubId)
        {
            return _itemGroupRepository.GetPaymentItemGroupThatCanBeParent(appEntityId, customerClubId);
        }

        public IQueryable<ItemGroup> GetRefundDetailItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId)
        {
            return _itemGroupRepository.GetRefundDetailItemGroupThatCanBeParent(appEntityId, itemGroupTypeSubId);
        }

        public IQueryable<ItemGroup> GetRefundDetailForPaymentItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId)
        {
            return _itemGroupRepository.GetRefundDetailForPaymentItemGroupThatCanBeParent(appEntityId, itemGroupTypeSubId);
        }

        public IQueryable<ItemGroup> GetCustomerClubPaymentInstallmentItemGroupThatCanBeParent(int appEntityId, int customerClubId)
        {
            return _itemGroupRepository.GetCustomerClubPaymentInstallmentItemGroupThatCanBeParent(appEntityId, customerClubId);
        }

        public IQueryable<ItemGroup> GetAdditionalUserPropItemGroupThatCanBeParent(int appEntityId,
            int? itemGroupTypeSubId)
        {
            return _itemGroupRepository.GetAdditionalUserPropItemGroupThatCanBeParent(appEntityId, itemGroupTypeSubId);
        }

        public IQueryable<ItemGroup> GetAdditionalUserPropItemGroupForNextGroup(int appEntityId, int? currentItemGroupId)
        {
            return _itemGroupRepository.GetAdditionalUserPropItemGroupForNextGroup(appEntityId,currentItemGroupId);
        }
    }
}