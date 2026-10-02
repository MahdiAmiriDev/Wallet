using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    
    public class UploadedCertificateRepository : GenericRepository<UploadedCertificate, int>, IUploadedCertificateRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public UploadedCertificateRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public Task<int> GetInvalidUploadedCertificateCountAsync(int userId)
        {
            return _context.Database.ExecuteSqlRawAsync($"[shp].[ValidateUploadedCertificate] {userId}");
        }

        public Task<int> AddCertificateFromUploadedCertificate(int userId)
        {
            return _context.Database.ExecuteSqlRawAsync($"[shp].[AddUploadedCertificatesToCertificate] {userId}");
        }

        public IQueryable<UploadedCertificate> GetByUserId(int userId, bool showInvalidItem)
        {
            var query = DbSet.Where(uc => uc.CreateBy == userId);

            if (showInvalidItem)
                query = query.Where(uc => uc.NationalCodeIsInvalid.Value ||
                                          uc.NationalCodeIsInvalid.Value ||
                                          uc.CourseTitleIsInvalid.Value ||
                                          uc.CourseDurationIsInvalid.Value ||
                                          uc.CourseTypeIsInvalid.Value ||
                                          uc.ScoreIsInvalid.Value ||
                                          uc.NameIsInvalid.Value ||
                                          uc.LastNameIsInvalid.Value ||
                                          uc.IssueDateIntIsInvalid.Value ||
                                          uc.NumberIsInvalid.Value
                );

            return query;
        }

        public Task DeleteByUserIdAsync(int userId)
        {
            return DbSet.Where(x => x.CreateBy == userId).DeleteFromQueryAsync();
        }
    }
}      
