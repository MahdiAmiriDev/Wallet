using Yashil.Common.SharedKernel.Module;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.RepositoryImpl;
using YashilPayment.Infrastructure.ServiceImpl;
using Yashil.Common.Core.Interfaces;
using Yashil.Core;
using YashilPayment.Web.GrpcServices;
using Yashil.Common.Core.Classes;

namespace YashilPayment.Web
{
    public class ModuleInitializer : IModuleInitializer, IGrpcServiceInitializer
    {
        public void OnStartup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder)
        {
            services.AddScoped<ICardInfoRepository, CardInfoRepository>();
            services.AddScoped<ICardInfoService, CardInfoService>();
            services.AddScoped<ICreditOrCashPaymentRepository, CreditOrCashPaymentRepository>();
            services.AddScoped<ICreditOrCashPaymentService, CreditOrCashPaymentService>();
            services.AddScoped<IInstallmentRepository, InstallmentRepository>();
            services.AddScoped<IInstallmentService, InstallmentService>();
            services.AddScoped<ICardAmountHistoryRepository, CardAmountHistoryRepository>();
            services.AddScoped<ICardAmountHistoryService, CardAmountHistoryService>();
            services.AddScoped<IMerchantInfoRepository, MerchantInfoRepository>();
            services.AddScoped<IMerchantInfoService, MerchantInfoService>();
            services.AddScoped<IMerchantTerminalInfoRepository, MerchantTerminalInfoRepository>();
            services.AddScoped<IMerchantTerminalInfoService, MerchantTerminalInfoService>();

            services.AddScoped<IHekmatPaymentGatewayTransactionRepository, HekmatPaymentGatewayTransactionRepository>();
            services.AddScoped<IHekmatPaymentGatewayTransactionService, HekmatPaymentGatewayTransactionService>();
            services.AddScoped<IHekmatTerminalRepository, HekmatTerminalRepository>();
            services.AddScoped<IHekmatTerminalService, HekmatTerminalService>();
            services.AddScoped<IYashilHekmatTerminalRepository, YashilHekmatTerminalRepository>();
            services.AddScoped<IYashilHekmatTerminalService, YashilHekmatTerminalService>();
            services.AddScoped<IHybridPaymentRepository, HybridPaymentRepository>();
            services.AddScoped<IHybridPaymentService, HybridPaymentService>();
            services.AddScoped<IHybridPaymentRequestRepository, HybridPaymentRequestRepository>();
            services.AddScoped<IHybridPaymentRequestService, HybridPaymentRequestService>();
            
            
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();


            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestRepository, HekmatPaymentGatewayTransactionBulkRequestRepository>();
            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestService, HekmatPaymentGatewayTransactionBulkRequestService>();

            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestCategoryRepository, HekmatPaymentGatewayTransactionBulkRequestCategoryRepository>();
            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestCategoryService, HekmatPaymentGatewayTransactionBulkRequestCategoryService>();

            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestDetailRepository, HekmatPaymentGatewayTransactionBulkRequestDetailRepository>();
            services.AddScoped<IHekmatPaymentGatewayTransactionBulkRequestDetailService, HekmatPaymentGatewayTransactionBulkRequestDetailService>();
            services.AddScoped<IRefundPaymentTransactionItemRequestRepository, RefundPaymentTransactionItemRequestRepository>();
            services.AddScoped<IRefundPaymentTransactionItemRequestService, RefundPaymentTransactionItemRequestService>();

            services.AddScoped<IYashilHekmatTerminalSupplierRepository, YashilHekmatTerminalSupplierRepository>();
            services.AddScoped<IYashilHekmatTerminalSupplierService, YashilHekmatTerminalSupplierService>();
            services.AddScoped<IPaymentTransactionItemRefundRepository, PaymentTransactionItemRefundRepository>();
            services.AddScoped<IPaymentTransactionItemRefundService, PaymentTransactionItemRefundService>();

            services.AddScoped<IPaymentTransactionItemRefundRepository, PaymentTransactionItemRefundRepository>();
            services.AddScoped<IPaymentTransactionItemRefundService, PaymentTransactionItemRefundService>();
            services.AddScoped<IIranKishPaymentService,IranKishPaymentService>();
            services.AddScoped<IVibeService, VibeService>();
            services.AddScoped<IHekmatOnlinePaymentGatewayService, HekmatOnlinePaymentGatewayService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void BindGrpcServices(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGrpcService<HekmatIpgService>().EnableGrpcWeb();
        }
        public List<YashilCustomResource> GetCustomResources()
        {
            return null;
        }
        public YashilModuleInformation GetModuleInformation()
        {
            return new YashilModuleInformation() { DatabaseSchemaName = "pay" };
        }
    }
}