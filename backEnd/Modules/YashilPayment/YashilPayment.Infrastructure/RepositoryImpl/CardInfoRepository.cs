using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class CardInfoRepository : GenericApplicationBasedRepository<CardInfo, int>, ICardInfoRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CardInfoRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<CardInfo> GetByCustomFilter(int? cardType, int? personId)
        {
            var query = GetAll(true);
            if (cardType.HasValue)
            {
                query = query.Where(x => x.CardType == cardType.Value);
            }
            if (personId.HasValue)
            {
                query = query.Where(x => x.PersonId == personId.Value);
            }
            return query;
        }
        public IQueryable<CardInfo> GetCardInfoByCardTypeAndOrganization(int cardType, int organizationId)
        {
            return DbSet.Where(x => x.CardType == cardType && x.Person.User.Any(u => u.OrganizationId == organizationId));
        }

        public IQueryable<CardInfo> GetCardInfoByCardNumber(string cardNumber, string cvv2, int expireDate)
        {
            return DbSet.Where(x => x.CardNumber == cardNumber && x.Cvv2 == cvv2 && x.ExpireDate == expireDate);
        }

        //تابع مربوط به افزایش شارژ بن ریالی یا اعتبار گروهی را در پایگاه داده فراخوانی می نماید
        public void DoCardCharge(int ApplicationId, int CreateBy, int CreatorOrganizationId, int OrganizationId, int CardTypeId, int Amount, bool IsCredit)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@ApplicationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = ApplicationId
                        },
                        new SqlParameter() {
                            ParameterName = "@CreateBy",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = CreateBy
                        },
                        new SqlParameter() {
                            ParameterName = "@CreatorOrganizationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = CreatorOrganizationId
                        },
                        new SqlParameter() {
                            ParameterName = "@OrganizationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = OrganizationId
                        },
                        new SqlParameter() {
                            ParameterName = "@CardTypeId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = CardTypeId
                        },
                        new SqlParameter() {
                            ParameterName = "@Amount",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Amount
                        },
                        new SqlParameter() {
                            ParameterName = "@IsCredit",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = IsCredit == true ? 1 : 0
                        }};
            _context.Database.ExecuteSqlRaw($" [pay].[DoCardCharge] @ApplicationId, @CreateBy, @CreatorOrganizationId, @OrganizationId, @CardTypeId, @Amount, @IsCredit", param);
        }


        //تابع مربوط به ذخیره سازی کارت را در پایگاه داده فراخوانی می نماید
        public string AddCardInfo(int applicationId, int createBy, int creatorOrganizationId, int organizationId,
            int cardTypeId, string name, string family, string gender, string nationalCode,
            string mobileNumber, string cardNumber, string cvv2, string firstPassword,
            string secondPassword, int? expireDate,
            int creditAmount, int cashAmount, byte[] password, byte[] passwordSalt)
        {
            SqlParameter result = null;
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@ApplicationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = applicationId
                        },
                        new SqlParameter() {
                            ParameterName = "@CreateBy",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = createBy
                        },
                        new SqlParameter() {
                            ParameterName = "@CreatorOrganizationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = creatorOrganizationId
                        },
                        new SqlParameter() {
                            ParameterName = "@OrganizationId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = organizationId
                        },
                        new SqlParameter() {
                            ParameterName = "@CardTypeId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = cardTypeId
                        },
                        new SqlParameter() {
                            ParameterName = "@Name",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = name
                        },
                        new SqlParameter() {
                            ParameterName = "@Family",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = family
                        },
                        new SqlParameter() {
                            ParameterName = "@Gender",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(gender)
                        },
                        new SqlParameter() {
                            ParameterName = "@NationalCode",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = nationalCode
                        },
                        new SqlParameter() {
                            ParameterName = "@MobileNumber",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(mobileNumber)
                        },
                        new SqlParameter() {
                            ParameterName = "@CardNumber",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = cardNumber
                        },
                        new SqlParameter() {
                            ParameterName = "@CVV2",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(cvv2)
                        },
                        new SqlParameter() {
                            ParameterName = "@FirstPassword",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = firstPassword
                        },
                        new SqlParameter() {
                            ParameterName = "@SecondPassword",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(secondPassword)
                        },
                        new SqlParameter() {
                            ParameterName = "@ExpireDate",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(expireDate)
                        },
                        new SqlParameter() {
                            ParameterName = "@CreditAmount",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(creditAmount)
                        },
                        new SqlParameter() {
                            ParameterName = "@CashAmount",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            IsNullable = true,
                            Value = CheckForDbNull(cashAmount)
                        },
                        new SqlParameter() {
                            ParameterName = "@Password",
                            SqlDbType =  System.Data.SqlDbType.VarBinary,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = password
                        },
                        new SqlParameter() {
                            ParameterName = "@PasswordSalt",
                            SqlDbType =  System.Data.SqlDbType.VarBinary,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = passwordSalt
                        },
                        result = new SqlParameter() {
                            ParameterName = "@Result",
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Output,
                            Size = 4000
                        }
        };
            _context.Database.ExecuteSqlRaw($" [pay].[AddCardInfo] @ApplicationId, @CreateBy, @CreatorOrganizationId, @OrganizationId, @CardTypeId, @Name , @Family, @Gender, @NationalCode, @MobileNumber, @CardNumber, @CVV2, @FirstPassword, @SecondPassword, @ExpireDate, @CreditAmount, @CashAmount, @Password, @PasswordSalt, @Result OUTPUT", param);
            string message = (string)result.Value;
            return message;
        }

        public object CheckForDbNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

    }
}
