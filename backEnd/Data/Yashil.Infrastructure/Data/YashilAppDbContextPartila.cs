using Microsoft.EntityFrameworkCore;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomDomainModels.YashilLight;
using Yashil.Core.Entities;

namespace Yashil.Infrastructure.Data
{
    public partial class YashilAppDbContext : DbContext
    {
        public virtual DbSet<CompanyProductSalesPlanningDomainModel> CompanyProductSalesPlanningDomainModel { get; set; }
        public virtual DbSet<CalcedOrderItemPrice> CalcedOrderItemPrices { get; set; }
        public virtual DbSet<FactorGroupItemReport> FactorGroupItemReports { get; set; }
        public virtual DbSet<FactorGroupConvertReport> FactorGroupConvertReports { get; set; }
        public virtual DbSet<CategoryCustomModel> CategorySimpleModels { get; set; }
        public virtual DbSet<InvalidFactorItemDomainModel> InvalidFactorItemDomainModels { get; set; }
        public virtual DbSet<QuestionCountByTypeAndDifficultyDomainModel> QuestionCountByTypeAndDifficultyDomainModels { get; set; }
        public virtual DbSet<AssignableQuestionsRandomlyByQuestionCountDomainModel> AssignableQuestionsRandomlyByQuestionCountDomainModels { get; set; }
        public virtual DbSet<ProductDetailDomainModel> ProductDetailDomainModels { get; set; }
        public virtual DbSet<ProductSimpleDomainModel> ProductSimpleDomainModels { get; set; }

        public virtual DbSet<HekmatTransactionCustomModel> HekmatTransactionSimpleModels { get; set; }
        public virtual DbSet<ProductSalesPlanningTeacherCustomDomainModel> ProductSalesPlanningTeacherCustomDomainModels { get; set; }
        public virtual DbSet<ResultMessage> ResultMessage { get; set; }
        public virtual DbSet<ExamQuestionAnswerDomainModel> ExamQuestionAnswerDomainModels { get; set; }
        public virtual DbSet<IsiBookCompleteOrderDomainModel> IsiBookCompleteOrderDomainModels { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalcedOrderItemPrice>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<FactorGroupItemReport>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<FactorGroupConvertReport>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<CategoryCustomModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<InvalidFactorItemDomainModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<HekmatTransactionCustomModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<QuestionCountByTypeAndDifficultyDomainModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<AssignableQuestionsRandomlyByQuestionCountDomainModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<ResultMessage>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<ProductRootCategoryView>(entity => { entity.HasKey(e => e.ProductId); });
            modelBuilder.Entity<ExamQuestionAnswerDomainModel>(entity => { entity.HasKey(e => e.ExamQuestionId); });
            modelBuilder.Entity<IsiBookCompleteOrderDomainModel>(entity => { entity.HasNoKey(); });
            modelBuilder.Entity<ProductSimpleDomainModel>(entity => { entity.HasNoKey(); });

            modelBuilder.Entity<Category>()
                .ToTable(tb => tb.HasTrigger("AfterInsertCategoryTrigger"));
            modelBuilder.Entity<Category>()
                .ToTable(tb => tb.HasTrigger("AfterUpdateCategoryTrigger"));
        }
    }
}
