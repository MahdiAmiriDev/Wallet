using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorGroupRepository : GenericApplicationBasedRepository<FactorGroup, int>, IFactorGroupRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public FactorGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }

        public IQueryable<FactorGroup> GetByCustomFilter(int? parentId)
        {
            var query = GetAll(true);
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            return query;
        }

        public bool CheckFactorGroupIsParent(FactorGroup factorGroup)
        {
            return CheckFactorGroupIsParent(factorGroup.Id);
        }

        public bool CheckFactorGroupIsParent(int factorGroupId)
        {
            return DbSet.Any(x => x.ParentId == factorGroupId || (x.Id == factorGroupId && x.Factor.Count == 0));
        }

        public IQueryable<FactorGroup> GetForSelectParent()
        {
            return GetAll(true).Where(x =>
                x.Factor.Count == 0 && x.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial);
        }

        public bool CheckParentHasAnyFactorGroup(FactorGroup factorGroup)
        {
            return DbSet.Any(x => x.ParentId == factorGroup.Id);
        }

        public IQueryable<FactorGroupConvertReport> GetFactorGroupConvertReport(int factorGroupId)
        {
            return _context.FactorGroupConvertReports.FromSqlRaw("EXECUTE [shp].[RptFactorGroupConvert] {0}",
                factorGroupId);
        }

        public FactorGroup GetFactorGroupIfConfirm(int factorGroupId, bool readOnly)
        {
            return DbSet.FirstOrDefault(x =>
                x.Id == factorGroupId && x.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed);
        }

        public bool CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(int factorGroupId)
        {
            var query = GetFactorGroupConvertReport(factorGroupId).AsEnumerable();
            return !query
                .Any(x => x.CategoryOrTaxId == null || x.CategoryOrTaxId.Trim() == string.Empty
                     || x.MainCategoryOrTaxTitle == null || x.MainCategoryOrTaxTitle.Trim() == string.Empty
                     || x.UnitOfMeasurement == null || x.UnitOfMeasurement.Trim() == string.Empty);
        }

        public Task<int> FixMissedOrderItemPriceRightShareAsync()
        {
            return _context.Database.ExecuteSqlRawAsync("Shp.FixMissedOrderItemPriceRightShare");
        }

        public async Task<int> GetMissedOrderItemPriceRightShareCount()
        {
            var wrongCount = new SqlParameter
            {
                ParameterName = "@WrongCount",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC [Shp].[GetMissedOrderItemPriceRightShareCount] @WrongCount OUT", wrongCount);
            return int.Parse(wrongCount.Value.ToString());
        }

        public IQueryable<FactorGroup> FindById(int id, bool readOnly = true)
        {
            var applicationId = _userPrincipal.ApplicationId != 0 ?
                _userPrincipal.ApplicationId : _applicationConfiguration.ApplicationId;
            return GetAll(readOnly, applicationId).Where(x => x.Id == id);
        }

        public Task<int> AddFactorGroupItemAsync(int factorGroupId)
        {
            object[] param = {
                new SqlParameter
                {
                    ParameterName = "@factorGroupId",
                    SqlDbType =  SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = factorGroupId
                }
            };
            return _context.Database.ExecuteSqlRawAsync(" shp.AddFactorGroupItemByFactorGroupId @factorGroupId" , param);
        }
    }
}