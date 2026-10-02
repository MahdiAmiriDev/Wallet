using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class HekmatTransactionRepository : GenericRepository<HekmatTransaction, int>, IHekmatTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        public HekmatTransactionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
        }
        /// <summary>
        /// فایل تراکنش هایی را که منتظر تعیین وضعیت می باشند را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<HekmatTransactionCustomModel> GetHekmatTransactions()
        {
            return _context.HekmatTransactionSimpleModels.FromSqlRaw($"EXECUTE [shp].[GetHekmatTransactions]");
        }
        public async Task<string> UpdateHekmatTransactionsAsync(int userId)
        {
            try
            {

                var result = new SqlParameter
                {
                    ParameterName = "@Result",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = 300
                };

                var userIdParam = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = userId
                };

                await _context.Database.ExecuteSqlRawAsync($"EXEC [pay].[UpdateHekmatPaymentGatewayTransactionsFromCvsItemsUploaded] @userId,@Result OUT", userIdParam, result);
                return result.Value.ToString();


            }
            catch (Exception)
            {
                return "کاربر گرامی،عملیات به روز رسانی تراکنش های حکمت انجام نشده است.لطفا مجددا تلاش نمایید";
            }
        }
        /// <summary>
        /// نتیجه تراکنش های حکمت کاربر را که به طور کامل انجام نشده است حذف می کند 
        /// </summary>
        /// <param name="userId"></param>
        public Task<int> DeleteByUserIdAsync(int userId)
        {
            return _context.HekmatTransaction.Where(x => x.CreateBy == userId && !x.Done).DeleteFromQueryAsync();
        }
        /// <summary>
        /// کد ملی و شماره موبایل و شماره تلفن تراکنش های حکمت را به حالت رمز در می آورد و در 
        /// ستون های جدا ریخته و ستون های کد ملی و شماره موبایل و تلفن را به عدد 1 به روز رسانی می کتد
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns></returns>
        public async Task EncryptHekmatTransactionsAsync(string key)
        {
            var keyParam = new SqlParameter
            {
                ParameterName = "@key",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = key
            };
            await _context.Database.ExecuteSqlRawAsync($"[pay].[EncryptTransactionDetailByKey] @Key", keyParam);

        }
    }
}
