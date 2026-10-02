using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Common.Infrastructure.JobImp;
using Yashil.Common.SharedKernel.Module;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.ControllersExtenders;
using Yashil.Core.Entities;
using Yashil.Core.Interfaces.Services;
using Yashil.Core.ServiceExtenders;
using Yashil.Infrastructure;
using Yashil.Infrastructure.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.ObjectFactory;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.Implementations;
using YashilCommerce.Infrastructure.JobImpl;
using YashilCommerce.Infrastructure.RepositoryImpl;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Infrastructure.ServicesExtenders;
using YashilCommerce.Web.Areas.ECommerce.CachedData;
using YashilCommerce.Web.Areas.ECommerce.DataService;
using YashilCommerce.Web.Areas.ECommerce.ObjectFactory;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ControllersExtenders;
using YashilUserManagement.Core.Repositories;
using YashilUserManagement.Core.Services;
using YashilUserManagement.Infrastructure.RepositoryImpl;
using YashilUserManagement.Infrastructure.ServiceImpl;

namespace YashilCommerce.Web
{
    public partial class ModuleInitializer : IModuleInitializer
    {
        partial void ConfigureCustomServices(IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder)
        {

            


            _configuration = configuration;
            var yashilCommerceCustomMessage = _configuration.GetSection("YashilCommerceCustomMessage").Get<YashilCommerceCustomMessage>();
            services.AddSingleton(yashilCommerceCustomMessage);

            // تنظیمات فروسگاه
            var storeConfiguration = _configuration.GetSection("StoreConfiguration").Get<StoreConfiguration>();
            services.AddSingleton(storeConfiguration);

            services.AddScoped<IHekmatService, Infrastructure.ServiceImpl.HekmatService>();

            services.AddScoped<ISqlBulkCopyExcelToDb, SqlBulkCopyExcelToDb>();

            services.AddScoped<IAddressService, AddressService>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            //:TODO Modify Or Remove Or Register Default => By Application Configuration
            services.AddScoped<IUserControllerExtender, UserControllerExtender>();
            services.AddScoped<IUserServiceExtender, UserServiceExtender>();


            services.AddScoped<ICustomEcommerceRepository, CustomEcommerceRepository>();


            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add SendMessageOnOrderStateChangeJob job
            // اگر در تنظیمات فعال باشد زمابندی را فعال میکنیم و سرویس را رجیستر میکنیم
            if (storeConfiguration.SendMessageOnOrderStateChange)
            {
                services.AddSingleton<SendMessageOnOrderStateChangeJob>();
                services.AddSingleton(new JobSchedule(
                    jobType: typeof(SendMessageOnOrderStateChangeJob),
                    cronExpression: "0 0/2 * 1/1 * ? *")); // run every 1 minuts
            }
            // Add SendMessageOnOrderStateChangeJob job
            // اگر در تنظیمات فعال باشد زمابندی را فعال میکنیم و سرویس را رجیستر میکنیم
            if (storeConfiguration.UpdateExpiredOrderStatus)
            {
                services.AddSingleton<UpdateExpiredOrderStatusJob>();
                services.AddSingleton(new JobSchedule(
                    jobType: typeof(UpdateExpiredOrderStatusJob),
                    cronExpression: "0 0/1 * 1/1 * ? *")); // run every 45 seconds
            }

            //تنطیم یک جاب برای اجرای ریست کردن تعداد بازدید روزانه سایت 
            //هر شب ساعت 23
            services.AddSingleton<ResetSiteVisitJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(ResetSiteVisitJob),
                cronExpression: "0 0 23 ? * * *"));

            // تنظیم جاب برای چک کردن سقف یارانه استفاده شده
            // هر 12 ساعت
            services.AddSingleton<UpdateSubsidyStatusBySubsidyConsumption>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(UpdateSubsidyStatusBySubsidyConsumption),
                cronExpression: "0 0 */12 ? * *"));


            services.AddScoped<IProductSalePlanningFactory, ProductSalePlanningFactoryViewComponent>();
            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = criteria != null && type.IsGenericType &&
                                  ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);
            }

            services.AddScoped<IItemGroup<Payment>, PaymentGroup>();

            services.AddScoped<IAdditionalUserPropService, AdditionalUserPropService>();

            services.AddScoped<CacheDataService>();
            services.AddScoped<PublicDataService>();

            services.AddScoped<IExcelBuilder, AsposeExcelBuilder>();
        }
    }
}