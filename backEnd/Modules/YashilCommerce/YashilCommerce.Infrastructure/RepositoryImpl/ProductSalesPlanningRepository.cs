using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;
using static System.Net.Mime.MediaTypeNames;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductSalesPlanningRepository : GenericApplicationBasedRepository<ProductSalesPlanning, int>,
        IProductSalesPlanningRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public ProductSalesPlanningRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="representationId"></param>
        /// <param name="manageInventoryMethodType"></param>
        /// <param name="productAvailabilityRangeType"></param>
        /// <param name="deliveryDateType"></param>
        /// <param name="published">
        /// null:در نظر نگیر
        /// 0:false
        /// 1:true
        /// 2:آنهایی که خالی است
        /// </param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetByCustomFilter(int? productId, int? representationId,
            int? manageInventoryMethodType, int? productAvailabilityRangeType, int? deliveryDateType, int? published,
            int? confirmed)
        {
            var query = GetAll(true).AsNoTracking();
            if (productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (manageInventoryMethodType.HasValue)
            {
                query = query.Where(x => x.ManageInventoryMethodType == manageInventoryMethodType.Value);
            }

            if (productAvailabilityRangeType.HasValue)
            {
                query = query.Where(x => x.ProductAvailabilityRangeType == productAvailabilityRangeType.Value);
            }

            if (deliveryDateType.HasValue)
            {
                query = query.Where(x => x.DeliveryDateType == deliveryDateType.Value);
            }

            if (published.HasValue)
            {
                /*  null:در نظر نگیر
                    0:false
                    1:true
                    2:آنهایی که خالی است
                */
                if (published.Value == 0)
                {
                    query = query.Where(x => !x.Published);
                }

                if (published.Value == 1)
                {
                    query = query.Where(x => x.Published);
                }

                if (published.Value == 2)
                {
                    // query = query.Where(x => !x.Published.HasValue);
                }
            }

            if (confirmed.HasValue)
            {
                /*  null:در نظر نگیر
                    0:false
                    1:true
                    2:آنهایی که خالی است
                */
                if (confirmed.Value == 0)
                {
                    query = query.Where(x => x.Confirmed.HasValue && !x.Confirmed.Value);
                }

                if (confirmed.Value == 1)
                {
                    query = query.Where(x => x.Confirmed.HasValue && x.Confirmed.Value);
                }

                if (confirmed.Value == 2)
                {
                    query = query.Where(x => !x.Confirmed.HasValue);
                }
            }

            return query;
        }


        public IQueryable<ProductSalesPlanning> SearchProductSalesPlannings(
            int customerClubId,
            string searchTitle,
            int? applicationGroupId,
            int? applicationId,
            int? categoryId,
            List<int> company,
            int? representationId,
            int? cityId,
            int? categoryGroupId,
            bool loadSubCategories,
            bool isEmploymentCategoryGroup,
            bool showIfHasPrice = false)
        {
            if (company != null && company.Count == 0)
            {
                company = null;
            }

            string selectedCategoryCodePath = string.Empty;
            if (categoryId.HasValue)
            {
                selectedCategoryCodePath = _context.Category
                    .Where(x => x.Id == categoryId)
                    .Select(x => x.CodePath)
                    .FirstOrDefault();
            }

            var query = DbSet
            .Where(x =>
                    x.SaleConfiguration.SaleConfigurationAttribute
                        .Any(s => s.CustomerClubId == customerClubId && s.IsActive) &&

                    x.Product.Company.IsActive &&
                    (!showIfHasPrice || (x.Price.HasValue && x.Price != 0)));

            query = query.Where(GetProductSalePlanningIsActiveFilter(
                applicationId,
                customerClubId,
                categoryGroupId,
                isEmploymentCategoryGroup));

            //if (categoryGroupId.HasValue)
            //{
            //    query = isEmploymentCategoryGroup ?
            //        query.Where(x =>
            //            x.Product.ProductType == (int)ProductTypeEnum.Employment)
            //        : query.Where(x =>
            //            x.Product.DefaultCategory.CategoryGroup.Id == categoryGroupId.Value);
            //}

            if (company != null && company.Count > 0)
            {
                query = query.Where(x => company.Any(c => c == x.Product.CompanyId));
            }

            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                query = query.Where(x => x.Product.Title.Contains(searchTitle));
            }

            if (!string.IsNullOrWhiteSpace(selectedCategoryCodePath))
            {
                query = query.Where(x =>
                    x.Product.ProductCategory.Any(c =>
                        c.Category.CodePath.StartsWith(selectedCategoryCodePath)));
            }

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId.Value);
            }

            if (cityId.HasValue)
            {
                query = query.Where(x =>
                    !x.RepresentationId.HasValue ||
                    x.Representation.Address.CityId == cityId);
            }

            if (applicationGroupId.HasValue)
            {
                query = query.Where(x => x.Product.Application.ApplicationGroupId == applicationGroupId);
            }

            return query.AsNoTracking();
        }


        public IQueryable<Company> FindProductSalesPlanningsCompanies(int customerClubId, string searchTitle, int? applicationId,
            int? categoryId, int? representationId, int? cityId, bool loadSubCategories,
            bool showIfHasPrice = false)
        {
            var query = SearchProductSalesPlannings(customerClubId, searchTitle, null, applicationId,
                categoryId, null, representationId, cityId, null, loadSubCategories,
                showIfHasPrice);
            return _context.Company.Where(x => x.IsActive && query.Any(s => s.Product.CompanyId == x.Id));
        }


        public Task<int?> GetRepresentationIdByProductSalesPlanningIdAsync(int productSalesPlanningId,
            int applicationId)
        {
            return DbSet
                .Where(x => x.ApplicationId == applicationId && x.Id == productSalesPlanningId)
                .AsNoTracking()
                .Select(x => x.RepresentationId)
                .FirstOrDefaultAsync();
        }

        public override ProductSalesPlanning Get(object id, bool readOnly = false)
        {
            return DbSet.Include(x => x.Product)
                .Include(x => x.Product.ProductCategory)
                .Include(x => x.Product.Company)
                .Include(x => x.Representation.Address.City.Parent)
                .FirstOrDefault(x => x.Id == (int)id);
        }


        public IQueryable<ProductSalesPlanning> FindById(int id, int? applicationId = null)
        {
            return DbSet.Where(x => x.Id == id && (!applicationId.HasValue || x.ApplicationId == applicationId))
                .AsNoTracking();
        }

        public IQueryable<ProductSalesPlanning> GetByRepresentationFilter(
            int applicationId,
            int? companyId,
            int? categoryId,
            int? representationId,
            bool showOnlyActiveProductSalesPlannings,
            bool doGetProductSalePlanningByRepresentation,
            int customerClubId,
            bool? visibleIndividually = null,
            bool? doGetUnFactoredProductSalesPlannings = null,
            bool? showArchives = false,
            bool? filterByOrderItemRepresentationId = false,
            bool? includeHubRepresentationFilter = false)
        {
            string selectedCategoryCodePath;
            if (categoryId.HasValue)
            {
                selectedCategoryCodePath = _context.Category.Where(x => x.Id == categoryId)
                    .Select(x => x.CodePath)
                    .AsNoTracking().FirstOrDefault();
            }
            else
            {
                selectedCategoryCodePath = _context.Category
                    .Where(x => !x.ParentId.HasValue && x.ApplicationId == applicationId)
                    .Select(x => x.CodePath)
                    .AsNoTracking()
                    .FirstOrDefault();
            }

            IQueryable<ProductSalesPlanning> query = null;
            // در این حالت ما می خواهیم برنامه ریزی دوره با توجه به نمایندگی انتخابی دریافت شود
            //  یا برنامه ریزی دوره های که نمایندگی ندارند را دریافت کند
            if (doGetProductSalePlanningByRepresentation)
            {
                query = DbSet.Where(x => x.ApplicationId == applicationId &&
                                         (!x.RepresentationId.HasValue ||
                                             (
                     (representationId.HasValue && (!includeHubRepresentationFilter.HasValue || !includeHubRepresentationFilter.Value))
                                                     ?
                                                     x.RepresentationId == representationId
                                                     :
                       representationId.HasValue &&
                       x.Representation.Address.City.ParentId == _context.Representation
                           .Where(x => x.Id == representationId.Value)
                           .Select(x => x.Address.City.ParentId).FirstOrDefault()

                                             )
                                         )
                                         &&
                 x.Product.ProductCategory.Any(c
                     => c.Category.CodePath.StartsWith(selectedCategoryCodePath)));

                if (showArchives is false)
                {
                    query = query.Where(x => x.Deleted == false);
                }

                if (companyId.HasValue)
                {
                    query = query.Where(x => x.Product.CompanyId == companyId.Value);
                }
            }
            // در این حالت ما می خواهیم برنامه ریزی دوره با توجه به مرکز انتخابی دریافت شود
            else
            {
                query = DbSet.Where(x =>
                   x.ApplicationId == applicationId &&
                   (!companyId.HasValue || x.Product.CompanyId == companyId.Value) &&
                   x.Product.ProductCategory.Any(c =>
                       c.Category.CodePath.StartsWith(selectedCategoryCodePath)));
                if (showArchives is false)
                {
                    query = query.Where(x => x.Deleted == false);
                }

                if (representationId.HasValue)
                {
                    //در صورتی بر اساس شناسه نمایندگی در ایتم سفارش هم فیلتر می کند که
                    //فیلتر مروبطه را فعال کرده باشیم
                    query = query.Where(x => (!x.RepresentationId.HasValue ||
                                                                x.RepresentationId == representationId.Value) || (filterByOrderItemRepresentationId.HasValue && filterByOrderItemRepresentationId.Value &&
                                             x.RepresentationId.HasValue ||
                                             x.OrderItem.Any(orderItem => orderItem.RepresentationId == representationId.Value)));
                }
            }


            if (visibleIndividually != null)
            {
                query = query.Where(x => x.Product.VisibleIndividually == visibleIndividually.Value);
            }


            if (showOnlyActiveProductSalesPlannings)
            {
                query = query.Where(GetProductSalePlanningIsActiveFilter(
                    applicationId,
                    customerClubId,
                    null,
                    false));
            }

            if (doGetUnFactoredProductSalesPlannings != null && doGetUnFactoredProductSalesPlannings.Value)
            {
                query = query.Where(x => x.OrderItem
                    .Any(oi => oi.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete &&
                               !oi.Deleted &&
                                !_context.OrderItem.Any(ch => ch.ParentId == oi.Id) &&
                        !_context.FactorItem.Any(fi => fi.OrderItemId == oi.Id)));
            }
            return query.AsNoTracking();
        }

        public Expression<Func<ProductSalesPlanning, bool>> GetProductSalePlanningIsActiveFilter(
            int? applicationId,
            int customerClubId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup)
        {
            var todayInt = DateFunction.TodayShamsiInt();
            return x => (!applicationId.HasValue || x.ApplicationId == applicationId) &&
                        x.Published &&
                        (!x.RepresentationId.HasValue || (x.RepresentationId.HasValue && x.Representation.Confirmed)) &&
                        x.Confirmed.Value &&
                        !x.Deleted &&
                        (!x.EndDate.HasValue || x.EndDate.Value >= todayInt) &&
                        x.Product.VisibleIndividually &&
                        x.SaleConfiguration.IsActive &&
                        (!x.SaleConfiguration.LicenseExpireDate.HasValue ||
                         x.SaleConfiguration.LicenseExpireDate > todayInt) &&
                        x.SaleConfiguration.ToDate > todayInt &&
                        x.SaleConfiguration.SaleConfigurationAttribute.Any(sca =>
                            sca.CustomerClubId == customerClubId && sca.IsActive) &&
                        (!categoryGroupId.HasValue ||
                         (!isEmploymentCategoryGroup &&
                          x.Product.DefaultCategory.CategoryGroupId == categoryGroupId.Value) ||
                         (isEmploymentCategoryGroup && x.Product.ProductType == (int)ProductTypeEnum.Employment));

        }

        public override ValueTask<ProductSalesPlanning> GetAsync(object id, bool readOnly = true)
        {
            int key = (int)id;
            var productSalesPlanning = DbSet.FirstOrDefaultAsync(x => x.Id == key);
            return new ValueTask<ProductSalesPlanning>(productSalesPlanning);
        }

        public int? GetCourseId(int id)
        {
            return _context.ProductSalesPlanning.AsNoTracking().Where(x => x.Id == id).Select(x => x.Product.CourseId)
                .FirstOrDefault();
        }

        public Task UpdateProductSalesPlanningCalculatedPriceAsync(int? productSalesPlanningId)
        {
            object[] param = {
                new SqlParameter
                {
                    ParameterName = "@ApplicationId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = _userPrincipal.ApplicationId
                },

                new SqlParameter
                {
                    ParameterName = "@ProductSalesPlanningId",
                    SqlDbType = SqlDbType.Int,
                    IsNullable = true,
                    Direction = ParameterDirection.Input,
                    Value = (object)productSalesPlanningId ?? DBNull.Value
                }
            };
            return _context.Database.ExecuteSqlRawAsync("[shp].[UpdateProductCalculatedPrice] @ApplicationId,@ProductSalesPlanningId", param);
        }

        public Task<bool> CheckProductHasAnyProductSalesPlanningAsync(int productId)
        {
            return DbSet.AsNoTracking().AnyAsync(x => x.ProductId == productId);
        }

        public bool CheckExistsSalePlanning(int productId)
        {
            return DbSet.AsNoTracking().Count(x => x.ProductId == productId && !x.RepresentationId.HasValue) > 0;
        }

        public bool CheckRepresentationHasAnyProductSalesPlanning(int representationId)
        {
            return DbSet.Any(x => x.RepresentationId == representationId);
        }


        public IQueryable<ProductSalesPlanning> GetAllByApplicationId(int applicationId, bool readOnly)
        {
            var query = DbSet.Where(x => x.ApplicationId == applicationId);
            return readOnly ? query.AsNoTracking() : query;
        }

        /// <summary>
        /// برنامه ریزی محصول فعال را بر میگرداند
        /// درصورتی که برنامه ریزی محصول تاریخ پایان دوره داشته باشد و تایید شده باشد و تنظیم فروش فعال داشته باشد 
        /// آن را بر میگرداند
        /// </summary>
        /// <param name="id">شناسه برنامه ریزی محصول</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="customerClubId">باشکگاه مشتریان</param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetActiveProductSalePlanningById(int id, int? applicationId, int customerClubId)
        {
            var query = DbSet.Where(GetProductSalePlanningIsActiveFilter(
                applicationId,
                customerClubId,
                null,
                false));
            return query.Where(x => x.Id == id).AsNoTracking();
        }

        /// <summary>
        /// لیست ایتم های مربوط به گروه را بر میگرداند
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning>
            GetProductSalesPlanningAssignedToGroup(int groupId, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.ProductSalesPlanningGroupId == groupId);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// ایتم های گروهی با مقدار ایتم گروپ وارد شده را بر می گرداند
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        public Task<bool> ExistsAnyProductSalesPlanningWithThisItemGroupIdAsync(int itemGroupId)
        {
            return DbSet.AnyAsync(x => x.ProductSalesPlanningGroupId == itemGroupId);
        }

        /// <summary>
        /// لیست ایتم گروه های گروه بندی نشده
        /// </summary>
        /// <param name="itemGroupSubId"></param>
        /// <param name="readOnly"></param>
        public IQueryable<ProductSalesPlanning> GetNotGroupedProductSalesPlanning(int itemGroupSubId, bool readOnly = false)
        {
            var query = DbSet.Where(x => !x.ProductSalesPlanningGroupId.HasValue);
            if (readOnly)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        /// جهت ایتم گروپ های که تا کنون گروه بندی نشده اند به گروه نهایی و ثبت نهایی کردن آن گروه
        /// </summary>
        /// <param name="itemGroupSubId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Task<int> AssignItemGroupForProductSalesPlanningAsync(int itemGroupSubId, int groupId)
        {
            return GetNotGroupedProductSalesPlanning(itemGroupSubId)
                .UpdateAsync(x => new ProductSalesPlanning
                {
                    ProductSalesPlanningGroupId = groupId,

                });
        }

        public async Task<IQueryable<ProductSalesPlanning>> GetTeacherProductSalesPlanningsAsync(
            int applicationId,
            int teacherUserId,
            int? companyId,
            int? categoryId,
            int? representationId,
            bool hierarchical,
            bool showOnlyActiveProductSalesPlannings,
            int customerClubId,
            bool? visibleIndividually = null)
        {
            var userAdditionalProp = await _context.AdditionalUserProp.
                Where(x => x.UserId == teacherUserId).
                Select(x => new
                {
                    x.CompanyId,
                    x.RepresentationId,
                    x.RepresentationCategoryId
                })
                .FirstOrDefaultAsync();


            if (userAdditionalProp != null)
            {
                int? userAdditionalPropCategoryId = null;
                if (userAdditionalProp.RepresentationCategoryId.HasValue)
                {
                    userAdditionalPropCategoryId = await _context.RepresentationCategory
                       .Where(rc => rc.Id == userAdditionalProp.RepresentationCategoryId.Value)
                       .Select(x => x.CategoryId)
                       .FirstOrDefaultAsync();
                }
                companyId = userAdditionalProp.CompanyId ?? companyId;
                representationId = userAdditionalProp.RepresentationId ?? representationId;
                categoryId = userAdditionalProp.RepresentationCategoryId.HasValue ? userAdditionalPropCategoryId : categoryId;
                var query = GetByRepresentationFilter(
                    applicationId,
                    companyId,
                    categoryId,
                    representationId,
                    showOnlyActiveProductSalesPlannings,
                    false,
                    customerClubId,
                    visibleIndividually);
                query = query.Where(x =>
                    x.ProductSalesPlanningTeacher.Any(t => t.TeacherUserId == teacherUserId));
                return query.AsNoTracking();
            }

            return null;

        }

        /// <summary>
        /// بیشترین فروش محصولات مرکز را
        /// بر اساس تعداد درخواست شده در پارامتر بر میگرداند
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        public IQueryable<CompanyProductSalesPlanningDomainModel> GetBestSellers(int applicationId, int customerClubId)
        {
            return _context.CompanyProductSalesPlanningDomainModel
                .FromSqlRaw("EXECUTE [shp].[GetBestSellers] {0},{1}", applicationId, customerClubId);
        }

        /// <summary>
        ///محصولاتی که بیشترین تخفیف را دارند، بدون در نظر گرفتن مرکز بر می گرداند
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        public IQueryable<CompanyProductSalesPlanningDomainModel> GetFullOfDiscounts(int applicationId, int customerClubId)
        {
            return _context.CompanyProductSalesPlanningDomainModel
                .FromSqlRaw("EXECUTE [shp].[GetFullOfDiscounts] {0},{1}", applicationId, customerClubId);
        }

        /// <summary>
        /// جدید ترین محصولات مرکز را میگرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<CompanyProductSalesPlanningDomainModel> GetNewest(int applicationId, int customerClubId)
        {
            return _context.CompanyProductSalesPlanningDomainModel
                .FromSqlRaw("EXECUTE [shp].[GetNewest] {0},{1}", applicationId, customerClubId);
        }

        public Task<int> UpdateRemainingCapacity(
            int productSalesPlanningId, int applicationId)
        {
            object[] param = { new SqlParameter
            {
                ParameterName = "@ProductSalesPlanningId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                IsNullable = true,
                Value = productSalesPlanningId
            },
            new SqlParameter
                {
                ParameterName = "@ApplicationId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                IsNullable = true,
                Value = applicationId
            }};
            return _context.Database.ExecuteSqlRawAsync
                    ("EXEC [Shp].[UpdateProductSalesPlanningRemainingCapacity] @ProductSalesPlanningId,@ApplicationId", param);
        }

        public async Task<bool> HasDiscountPercentageChangedComparedToTheContractAsync(int productSalesPlanningId)
        {
            return await DbSet.AnyAsync(x =>
                 x.Id == productSalesPlanningId &&
                 x.SaleConfiguration.SaleConfigurationAttribute
                     .Any(a =>
                         a.CustomerClub.ProductSalesPlanningDiscount
                             .Any(d =>
                                 d.ProductSalesPlanningId == productSalesPlanningId && d.DiscountPercentage != a.DiscountPercentage))
                );
        }

        private (int? companyId, int? RepresentationId) GetCurrentUserCompanyAndRepresentation()
        {
            var res = _context.AdditionalUserProp
                .Where(x => x.UserId == _userPrincipal.Id)
                .Select(x => new { x.CompanyId, x.RepresentationId })
                .AsNoTracking().FirstOrDefault();
            return (res?.CompanyId, res?.RepresentationId);
        }

        public async Task<bool> CheckCurrentUserPurchasesByProductSalesPlanningIdAsync(int productSalesPlanningId)
        {
            return await _context.Order.Where(x => x.UserId == _userPrincipal.Id && x.OrderStatusType == (int)OrderStatusTypesEnum.Complete)
                .AnyAsync(o => o.OrderItem.Any(oi => oi.ProductSalesPlanning.Id == productSalesPlanningId));
        }

        /// <summary>
        /// دریافت برنامه ریزی های دوره فعال براساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetIsActiveProductSalesPlanningsByApplicationId(
            int applicationId,
             int customerClubId,
            bool readOnly = true)
        {
            return readOnly
                ? DbSet.Where(GetProductSalePlanningIsActiveFilter(applicationId, customerClubId, null, false))
                    .AsNoTracking()
                : DbSet.Where(GetProductSalePlanningIsActiveFilter(applicationId, customerClubId, null, false));

        }


        /// <summary>
        /// این متد برنامه ریزی دوره 
        /// productSalesPlanning
        /// مرتبط با 
        /// شناسه تگ های ارسالی را بر می گرداند
        /// </summary>
        /// <param name="tagIds">شناسه تگ ها</param>
        /// <returns>ProductSalesPlanning</returns>
        public IQueryable<ProductSalesPlanning> GetProductSalesPlanningRelatedToTagId(List<int> tagIds)
        {
            return (from productTag in _context.ProductTags
                    join productSalesPlanning in DbSet
                        on productTag.ProductId equals productSalesPlanning.ProductId
                    where tagIds.Contains(productTag.TagType)
                    select productSalesPlanning);
        }

        /// <summary>
        /// این متد جهت دریافت برنامه ریزی دوره بر اساس شناسه دسته بندی آزمون کاربرد دارد
        /// </summary>
        /// <param name="examCategoryId">شناسه دسته بندی آزمون</param>
        /// <returns>ProductSalesPlanning</returns>
        public IQueryable<ProductSalesPlanning> GetProductSalesPlanningByExamCategoryId(int examCategoryId)
        {
            return _context.ExamCategory.Where(examCategory => examCategory.Id == examCategoryId)
                .Select(x => x.ProductSalesPlanning);
        }

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public async Task<int> ArchiveAsync(int productSalesPlanningId)
        {
            return await DbSet.Where(x => x.Id == productSalesPlanningId).UpdateAsync(
                x => new()
                {
                    Deleted = true
                });
        }

        /// <summary>
        ///  می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با  
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public async Task<int> RestoreFromArchiveAsync(int productSalesPlanningId)
        {
            return await DbSet.Where(x => x.Id == productSalesPlanningId).UpdateAsync(
                x => new()
                {
                    Deleted = false
                });
        }

        public IQueryable<ProductSalesPlanning> FindBySaleConfigurationId(int saleConfigurationId, bool readOnly = true)
        {
            return GetAll(readOnly).Where(x => x.SaleConfigurationId == saleConfigurationId);
        }
    }
}