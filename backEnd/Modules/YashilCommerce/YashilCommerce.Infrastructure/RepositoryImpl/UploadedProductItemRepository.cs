using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class UploadedProductItemRepository : GenericRepository<UploadedProductItem, int>, IUploadedProductItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public UploadedProductItemRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public Task<int> GetInvalidUploadedProductItemsCountAsync(
            int userId, int applicationId,
            char numberSeparator, int companyId)
        {

            return _context.Database.ExecuteSqlRawAsync
                ($"[shp].[ValidateUploadedProductItems]" +
                 $" {userId},{applicationId},N'{numberSeparator.ToString()}',{companyId}");
        }

        public Task DeleteByUserIdAsync(int userId)
        {
            return _context.UploadedProductItem.Where(x => x.CreateBy == userId).ExecuteDeleteAsync();
        }


        /// <summary>
        ///   رکورد های مربوط به کاربر لاگین کرده را بر می گرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="showInvalidItem">فقط رکورد های نامعتبر را نمایش دهد</param>
        /// <returns></returns>
        public IQueryable<UploadedProductItem> GetByUserId(int userId, bool showInvalidItem)
        {
            var query = DbSet.Where(x => x.CreateBy == userId);
            if (showInvalidItem)
            {
                query = query.Where(x =>
                    x.TestBasePriceIsInValid.Value
                    || x.BookBasePriceIsInValid.Value
                    || x.DeliveryCostIsInValid.Value
                    || x.TbaoCustomerClubDiscountPercentageIsInValid.Value
                    || x.OtherCustomerClubDiscountPercentageIsInValid.Value
                    || x.PublishedIsInValid.Value
                    || x.TypeIsInValid.Value
                    || x.ProductSalePlanningDisplaySubTitleIsInvalid.Value
                    || x.CategoryTitleIsInValid.Value
                    || x.ProductGroupIdForExamIsInvalid.Value
                    || x.ProductGroupIdForBookIsInvalid.Value
                    || x.ProductSalesPlanningIsInvalid.Value
                    || x.RepresentationIsInvalid.Value
                    || x.ProductSalesPlanningStudentUserNameIsInvalid.Value
                    || x.ProductSalesPlanningTeacherUserNameIsInvalid.Value
                    || x.ProductIdIsInvalid.Value);

            }

            return query;
        }
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاذی جدید تغییر می کند این متد مقدار
        ///  true
        ///  بر می گرداند در غیر اینصورت مقدار
        ///  false
        ///  بر می گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public Task<bool> CheckExistsProductsSoldAndRenamedInUploadedProductsAsync(int userId, int applicationId, int companyId)
        {
            var existsProductsSoldAndRenamed = GetProductsSoldAndRenamedInUploadedProducts(userId, applicationId, companyId).AnyAsync();
            return existsProductsSoldAndRenamed;

        }
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند .
        ///  آن محصولات را بر می  گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public IQueryable<UploadedProductItem> GetProductsSoldAndRenamedInUploadedProducts(int userId, int applicationId, int companyId)
        {

            var query = from up in _context.UploadedProductItem.Where(x => x.CreateBy == userId)
                        join p in _context.Product.Where(p => p.ApplicationId == applicationId
                        && p.CompanyId == companyId && p.VisibleIndividually)
                        on up.Code equals p.MainCode
                        where p.Title != up.Title || p.FullDescription != up.Description
                        join psp in _context.ProductSalesPlanning
                        on p.Id equals psp.ProductId
                        where _context.OrderItem.Any(oi =>
                        oi.ProductSalesPlanningId == psp.Id &&
                        (oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete
                              || oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment
                              || oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.Processing
                              || oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.UnsuccessfulPayment
                              || oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.Canceled)
                         )
                        select new UploadedProductItem
                        {
                            Id = up.Id,
                            Code = up.Code,
                            Title = up.Title,
                            Description = up.Description,
                            Type = up.Type
                        };

            return query;
        }


        public Task<int> GetInvalidUploadedSimpleProductItemsCountAsync(int userId, int companyId)
            => _context.Database.ExecuteSqlRawAsync(
                $"[shp].[ValidateSimpleCompanyConfirmedAndPublishedProducts] {companyId},{userId}");

            public Task<int> AddDataFromSimpleUploadedProductItemAsync(int applicationId, int userId, int companyId)
            => _context.Database.ExecuteSqlRawAsync(
                $"[shp].[AddDataFromSimpleUploadedProductItem] {companyId},{userId},{applicationId}");
    }
}
