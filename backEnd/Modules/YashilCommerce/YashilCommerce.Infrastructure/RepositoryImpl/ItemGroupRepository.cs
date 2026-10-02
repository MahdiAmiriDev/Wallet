using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ItemGroupRepository : GenericApplicationBasedRepository<ItemGroup, int>, IItemGroupRepository
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IUserPrincipal _userPrincipal;

        public ItemGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration) : base(context,
            userPrincipal)
        {
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }
        public IQueryable<ItemGroup> GetByCustomFilter(int? parentId, int appEntityId, int? itemGroupTypeSubId)
        {
            var query = GetAll(true);
            query = query.Where(x => x.AppEntityId == appEntityId);
            if (itemGroupTypeSubId.HasValue)
            {
                query = query.Where(x => x.ItemGroupSubId == itemGroupTypeSubId);
            }
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            return query;
        }

        public bool CheckItemGroupIsParent(int itemGroupId)
        {
            return DbSet.Any(x => x.ParentId == itemGroupId);
        }

        public IQueryable<ItemGroup> GetPaymentItemGroupThatCanBeParent(int appEntityId, int customerClubId)
        {
            var query = GetItemGroupsThatCanBeParents(appEntityId, customerClubId);
            query = query.Where(x => !x.Payment.Any());
            return query;
        }

        public IQueryable<ItemGroup> GetRefundDetailItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId)
        {
            var query = GetItemGroupsThatCanBeParents(appEntityId, itemGroupTypeSubId);
            query = query.Where(x => !x.RefundDetailRefundDetailGroup.Any());
            return query;

        }

        public IQueryable<ItemGroup> GetRefundDetailForPaymentItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId)
        {
            var query = GetItemGroupsThatCanBeParents(appEntityId, itemGroupTypeSubId);
            query = query.Where(x => !x.RefundDetailRefundDetailForPaymentGroup.Any());
            return query;
        }

        public IQueryable<ItemGroup> GetCustomerClubPaymentInstallmentItemGroupThatCanBeParent(int appEntityId, int customerClubId)
        {
            var query = GetItemGroupsThatCanBeParents(appEntityId, customerClubId);
            query = query.Where(x => !x.CustomerClubPaymentInstallment.Any());
            return query;
        }

        public IQueryable<ItemGroup> GetAdditionalUserPropItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId)
        {
            var query = GetItemGroupsThatCanBeParents(appEntityId, itemGroupTypeSubId);
            return query.Where(x => !x.AdditionalUserProp.Any());
        }

        
        private IQueryable<ItemGroup> GetItemGroupsThatCanBeParents(int appEntityId, int? itemGroupTypeSubId)
        {
            var applicationId = _userPrincipal.ApplicationId != 0
                ? _userPrincipal.ApplicationId
                : _applicationConfiguration.ApplicationId;
            return DbSet.Where(x =>
                x.AppEntityId == appEntityId &&
                x.ApplicationId == applicationId &&
                (!itemGroupTypeSubId.HasValue || x.ItemGroupSubId == itemGroupTypeSubId.Value));
        }

        public ItemGroup GetItemGroupIfConfirm(int groupId, bool readOnly = false)
        {
            IQueryable<ItemGroup> query = DbSet;
            if (readOnly)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefault(x => x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed);
        }

        public IQueryable<ItemGroup> GetAdditionalUserPropItemGroupForNextGroup(int appEntityId, int? currentItemGroupId)
        {
            return GetItemGroupForNextGroup(appEntityId, currentItemGroupId);
              
        }

        private IQueryable<ItemGroup> GetItemGroupForNextGroup(int appEntityId, int? currentItemGroupId)
        {
            var applicationId = _userPrincipal.ApplicationId != 0
                ? _userPrincipal.ApplicationId
                : _applicationConfiguration.ApplicationId;
            return DbSet.Where(x =>
                x.AppEntityId == appEntityId &&
                x.ApplicationId == applicationId &&
                (!currentItemGroupId.HasValue || x.Id != currentItemGroupId.Value));
        }
    }
}