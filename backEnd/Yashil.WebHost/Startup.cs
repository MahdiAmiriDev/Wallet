using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Module;
using Yashil.Common.SharedKernel.Web;
using Yashil.Common.SharedKernel.Web.ModelBinders;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.ControllersExtenders;
using Yashil.Infrastructure;
using Yashil.Infrastructure.Data;
using Yashil.Infrastructure.Extensions;
using Yashil.Runtime.Classes;
using Yashil.Runtime.RequestFormatter;
using Yashil.WebHost.Extensions;
using Yashil.WebHost.Filters;
using Yashil.WebHost.Observability.Monitoring;
using YashilUserManagement.Core.Classes;
using YashilUserManagement.Core.Services;
using YashilUserManagement.Web.Areas.UserMngAdminApi.ControllersDefaultExtenders;

namespace Yashil.WebHost;

public class Startup
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _configuration;
    private readonly IOnlineUser _onlineUser;


    public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _onlineUser = new OnlineUser();
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        foreach (var module in GlobalConfiguration.Modules)
        {
            var moduleInitializerType = module.Assembly.GetTypes()
                .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
            if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
            {
                var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                moduleInitializer?.OnStartup(configuration, hostingEnvironment);
            }
        }
        CommonHelper.DefaultFileProvider = new YashilFileProvider(hostingEnvironment);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        var meters = new MetricReporter();
        services.AddSingleton<MetricReporter>();

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });

        // If you're using XML comments, uncomment the following line
        // c.IncludeXmlComments(System.AppDomain.CurrentDomain.GetAssemblies()
        //     .SelectMany(assembly => assembly.GetTypes())
        //     .Where(type => type.IsPublic && typeof(ApiController).IsAssignableFrom(type))
        //     .SelectMany(t => t.GetMethods())
        //     .Where(m => m.GetCustomAttribute<ApiExplorerRouteAttributes>() != null));
        //});

        //services.AddOpenTelemetryMetrics(opts => opts
        //    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Anosito"))
        //    .AddMeter(meters.MetricName)
        //    .AddAspNetCoreInstrumentation()
        //    .AddRuntimeInstrumentation()
        //    .AddPrometheusExporter());




        #region Configure Forwarded Headers
        // Forwarded Headers Middleware should run before other middleware. 
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        #endregion

        services.AddScoped(typeof(IYashilLogger<>), typeof(YashilLogger<>));

        services.AddGrpc();

        // implementation of 
        // IDistributedCache
        var applicationConfiguration = _configuration.GetSection("ApplicationConfiguration").Get<ApplicationConfiguration>();
        services.AddSingleton(applicationConfiguration);

        //باشد true برابر با state ابتدا بررسی می کند که 
        //برابر با مک آدرس ماشین هست یا نه appsetting سپس بررسی می کند که آیا مک آدرس قرارداده شده در
        // درصورتی که یکی نباشد از ادامه اجرای برنامه جلوگیری می کند
        if (!_hostingEnvironment.IsDevelopment())
        {
            try
            {
                var envState = _configuration.GetValue<string>("EnvKey:state");
                if (CryptographyHelper.AesDecrypt(envState) == "true")
                {
                    var envKey = CryptographyHelper.AesDecrypt(_configuration.GetValue<string>("EnvKey:value"));
                    if (!YashilDeviceManagingExtention.ValidateMacAddress(envKey))
                    {
                        throw new Exception("You are not authorized to use this software!");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        // بررسی تنظیمات appsettings.json
        services.AddSingleton(_onlineUser);
        // پارامتر های مربوط به تنظیمات ارسال پیام از کانفیگ خوانده میشود 
        #region Register Message Configuration

        var registerMessageConfiguration = _configuration.GetSection("RegisterMessageConfiguration").Get<RegisterMessageConfiguration>();
        services.AddSingleton<IRegisterMessageConfiguration>(registerMessageConfiguration);

        #endregion

        #region Sms Service Configuration
        var smsServiceConfiguration = _configuration.GetSection("SmsServiceConfiguration").Get<SmsServiceConfiguration>();
        smsServiceConfiguration.UserName = CryptographyHelper.AesDecrypt(smsServiceConfiguration.UserName);
        smsServiceConfiguration.Password = CryptographyHelper.AesDecrypt(smsServiceConfiguration.Password);
        services.AddSingleton<ISmsServiceConfiguration>(smsServiceConfiguration);
        #endregion
        #region VerifySms Service Configuration
        var verifySmsServiceConfiguration = _configuration.GetSection("VerifySmsServiceConfiguration").Get<VerifySmsServiceConfiguration>();

        if (applicationConfiguration.SmsBased && (string.IsNullOrWhiteSpace(verifySmsServiceConfiguration.UserApiKey) ||
                                                  string.IsNullOrWhiteSpace(verifySmsServiceConfiguration.SecretKey) ||
                                                  verifySmsServiceConfiguration.TemplateId == 0))
        {
            throw new Exception("تنظیمات پیامک احراز هویت به درستی ست نشده است");
        }

        verifySmsServiceConfiguration.UserApiKey = CryptographyHelper.AesDecrypt(verifySmsServiceConfiguration.UserApiKey);
        verifySmsServiceConfiguration.SecretKey = CryptographyHelper.AesDecrypt(verifySmsServiceConfiguration.SecretKey);
        services.AddSingleton<IVerifySmsServiceConfiguration>(verifySmsServiceConfiguration);
        #endregion
        #region Sata Service 
        var sataServiceConfiguration = _configuration.GetSection("SataSubsidyApprovalConfiguration").Get<SataSubsidyApprovalConfiguration>();
        sataServiceConfiguration.UserName = CryptographyHelper.AesDecrypt(sataServiceConfiguration.UserName);
        sataServiceConfiguration.Password = CryptographyHelper.AesDecrypt(sataServiceConfiguration.Password);
        services.AddSingleton<ISataSubsidyApprovalConfiguration>(sataServiceConfiguration);
        #endregion

        #region HekmatPayment Configuration

        // read hekmat payment configuration from appsettings
        var hekmatPaymentConfiguration = _configuration.GetSection("HekmatPaymentConfiguration").Get<HekmatPaymentConfiguration>();

        if (hekmatPaymentConfiguration != null)
        {
            hekmatPaymentConfiguration.MerchantId =
                CryptographyHelper.AesDecrypt(hekmatPaymentConfiguration.MerchantId);
            hekmatPaymentConfiguration.TerminalId =
                CryptographyHelper.AesDecrypt(hekmatPaymentConfiguration.TerminalId);
            hekmatPaymentConfiguration.PrivateKey =
                CryptographyHelper.AesDecrypt(hekmatPaymentConfiguration.PrivateKey);

            services.AddSingleton<IHekmatPaymentConfiguration>(hekmatPaymentConfiguration);
        }
        // read hekmat payment configuration from appsettings
        var dmsConfiguration = _configuration.GetSection("DmsConfiguration").Get<DmsConfiguration>();
        services.AddSingleton(dmsConfiguration);


        #endregion

        #region Hekmat Online Payment

        var hekmatOnlinePaymentConfiguration = _configuration.GetSection("HekmatOnlinePaymentConfiguration")
            .Get<HekmatOnlinePaymentConfiguration>();

        if (hekmatOnlinePaymentConfiguration != null)
        {
            hekmatOnlinePaymentConfiguration.MerchantId =
                CryptographyHelper.AesDecrypt(hekmatOnlinePaymentConfiguration.MerchantId);

            hekmatOnlinePaymentConfiguration.TerminalId =
                CryptographyHelper.AesDecrypt(hekmatOnlinePaymentConfiguration.TerminalId);

            hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey =
                CryptographyHelper.AesDecrypt(hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey);

            hekmatOnlinePaymentConfiguration.SitePrivateKey =
                CryptographyHelper.AesDecrypt(hekmatOnlinePaymentConfiguration.SitePrivateKey);

            hekmatOnlinePaymentConfiguration.Password =
                CryptographyHelper.AesDecrypt(hekmatOnlinePaymentConfiguration.Password);


            services.AddSingleton<IHekmatOnlinePaymentConfiguration>(hekmatOnlinePaymentConfiguration);
        }
        #endregion

        #region  AnsarPayment Configuration
        // read ansar payment configuration from appsettings
        var ansarPaymentConfiguration = _configuration.GetSection("AnsarPaymentConfiguration")
            .Get<AnsarPaymentConfiguration>();

        if (ansarPaymentConfiguration != null)
        {
            ansarPaymentConfiguration.MID = CryptographyHelper.AesDecrypt(ansarPaymentConfiguration.MID);
            ansarPaymentConfiguration.TerminalId = CryptographyHelper.AesDecrypt(ansarPaymentConfiguration.TerminalId);
            ansarPaymentConfiguration.UserName = CryptographyHelper.AesDecrypt(ansarPaymentConfiguration.UserName);
            ansarPaymentConfiguration.Password = CryptographyHelper.AesDecrypt(ansarPaymentConfiguration.Password);

            services.AddSingleton<IAnsarPaymentConfiguration>(ansarPaymentConfiguration);
        }
        #endregion
        // read saman payment configuration from appsettings.json
        #region SamanPayment Configuration Sep IPg
        var samanPaymentConfiguration =
            _configuration.GetSection("SamanPaymentConfiguration").Get<SamanPaymentConfiguration>();


        if (samanPaymentConfiguration != null)
        {
            samanPaymentConfiguration.TerminalId = CryptographyHelper.AesDecrypt
                (samanPaymentConfiguration.TerminalId);
            samanPaymentConfiguration.CallBackUrl = samanPaymentConfiguration.CallBackUrl;
            samanPaymentConfiguration.GenerateTokenUrl = samanPaymentConfiguration.GenerateTokenUrl;
            samanPaymentConfiguration.RedirectToIpgUrl = samanPaymentConfiguration.RedirectToIpgUrl;
            samanPaymentConfiguration.VerifyTransactionUrl = samanPaymentConfiguration.VerifyTransactionUrl;
            samanPaymentConfiguration.ReverseTransactionUrl = samanPaymentConfiguration.ReverseTransactionUrl;


            services.AddSingleton<ISamanPaymentConfiguration>(samanPaymentConfiguration);
        }
        #endregion

        #region SaminPayment Configuration IPg  درگاه پرداخت ثمین

        var saminPaymentConfiguration =
            _configuration.GetSection("SaminPaymentConfiguration")
                .Get<SaminPaymentConfiguration>();


        if (saminPaymentConfiguration != null)
        {
            saminPaymentConfiguration.TerminalCode = CryptographyHelper.AesDecrypt
                (saminPaymentConfiguration.TerminalCode);

            saminPaymentConfiguration.MerchantCode = CryptographyHelper.AesDecrypt
                (saminPaymentConfiguration.MerchantCode);

            saminPaymentConfiguration.MerchantPassword = CryptographyHelper.AesDecrypt
                (saminPaymentConfiguration.MerchantPassword);

            saminPaymentConfiguration.CallBackUrl = saminPaymentConfiguration.CallBackUrl;
            saminPaymentConfiguration.CallBackPath = saminPaymentConfiguration.CallBackPath;
            saminPaymentConfiguration.GenerateTokenUrl = saminPaymentConfiguration.GenerateTokenUrl;
            saminPaymentConfiguration.RedirectToIpgUrl = saminPaymentConfiguration.RedirectToIpgUrl;
            saminPaymentConfiguration.VerifyTransactionUrl = saminPaymentConfiguration.VerifyTransactionUrl;
            saminPaymentConfiguration.InquireTransactionUrl = saminPaymentConfiguration.InquireTransactionUrl;


            services.AddSingleton<ISaminPaymentConfiguration>(saminPaymentConfiguration);
        }

        #endregion

        #region  SaderatPayment Configuration
        // read saderat payment configuration from appsettings
        var saderatPaymentConfiguration = _configuration.GetSection("SaderatPaymentConfiguration").Get<SaderatPaymentConfiguration>();

        if (saderatPaymentConfiguration != null)
        {
            saderatPaymentConfiguration.MID = CryptographyHelper.AesDecrypt(saderatPaymentConfiguration.MID);

            saderatPaymentConfiguration.TerminalId = CryptographyHelper.AesDecrypt(saderatPaymentConfiguration.TerminalId);

            saderatPaymentConfiguration.CallBackUrl = saderatPaymentConfiguration.CallBackUrl;

            services.AddSingleton<ISaderatPaymentConfiguration>(saderatPaymentConfiguration);
        }
        #endregion

        #region SetAspose License
        string licenseLocation = Path.Combine(_hostingEnvironment.WebRootPath, $"aspose-license{Path.DirectorySeparatorChar}Aspose-License.txt");
        License license = new License();
        license.SetLicense(licenseLocation);

        #endregion
        // پارمتر های مربوط به تنظیمات دیتابیس از کانفیگ خوانده شود
        #region Database Configuration

        // read hekmat payment configuration from appsettings
        var dataBaseConfiguration = _configuration.GetSection("DataBaseConfiguration").Get<DataBaseConfiguration>();
        if (dataBaseConfiguration != null)
        {
            dataBaseConfiguration.EncryptedKey = CryptographyHelper.AesDecrypt(dataBaseConfiguration.EncryptedKey);
            services.AddSingleton<IDataBaseConfiguration>(dataBaseConfiguration);
        }

        #endregion


        #region Humax API Configuration
        var humaxApiConfiguration = _configuration.GetSection("HumaxAPIConfiguration").Get<HumaxApiConfiguration>();
        if (humaxApiConfiguration != null)
        {
            humaxApiConfiguration.PartnerName = CryptographyHelper.AesDecrypt(humaxApiConfiguration.PartnerName);
            humaxApiConfiguration.APIKey = CryptographyHelper.AesDecrypt(humaxApiConfiguration.APIKey);
            humaxApiConfiguration.Url = humaxApiConfiguration.Url;
            services.AddSingleton<IHumaxApiConfiguration>(humaxApiConfiguration);
        }
        #endregion

        #region Sms Templates Configuration
        var smsTemplatesConfiguration = _configuration.GetSection("SmsTemplatesConfiguration")
            .Get<SmsTemplatesConfiguration>();
        if (smsTemplatesConfiguration != null)
        {
            services.AddSingleton<ISmsTemplatesConfiguration>(smsTemplatesConfiguration);
        }
        #endregion

        #region  Add Http Handler
        services.AddHttpClient("Payment")
            .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = environment.IsDevelopment()
                        ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        : null
                };
            });


        #endregion

        var allowedOrigins = _configuration.GetSection("AllowedOrigins:Origins").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    if (allowedOrigins != null)
                        builder.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });
        });
        //var appSettingsSection = _configuration.GetSection("AppSettings");
        //services.Configure<AppSettings>(appSettingsSection);
        var yashilSecret = Environment.GetEnvironmentVariable("YashilSecret");
        if (string.IsNullOrEmpty(yashilSecret))
        {
            throw new Exception("YashilSecret Not Exists in EnvironmentVariable");
        }
        var yashilAESIv = Environment.GetEnvironmentVariable("YashilAESIv");
        if (string.IsNullOrEmpty(yashilAESIv))
        {
            throw new Exception("yashilAESIv Not Exists in EnvironmentVariable");
        }
        var yashilAESKey = Environment.GetEnvironmentVariable("YashilAESKey");
        if (string.IsNullOrEmpty(yashilAESKey))
        {
            throw new Exception("YashilAESKey Not Exists in EnvironmentVariable");
        }
        AppSettings appSettings = new AppSettings
        {
            Secret = yashilSecret,
            AESIv = yashilAESIv,
            AESKey = yashilAESKey
        };
        services.AddSingleton(appSettings);
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            x.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.Principal != null)
                    {
                        var strUserId = context.Principal.Identity?.Name;
                        if (!string.IsNullOrEmpty(strUserId))
                        {
                            var userId = Convert.ToInt32(strUserId);
                            if (!_onlineUser.Users.ContainsKey(userId))
                                context.Fail("Failed additional validation");
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });
        services.AddControllersWithViews(x => x.InputFormatters.Insert(0, new RawRequestBodyFormatter())).AddRazorRuntimeCompilation();


        GlobalConfiguration.WebRootPath = _hostingEnvironment.WebRootPath;
        GlobalConfiguration.ContentRootPath = _hostingEnvironment.ContentRootPath;

        services.AddModules(_hostingEnvironment.ContentRootPath);
        services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander()); });



        var connectionString = CryptographyHelper.AesDecrypt(_configuration.GetConnectionString("YashilAppDB"));

        services.AddDbContext<YashilAppDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout(600);
                }));
        services.AddScoped<IUnitOfWork, UnitOfWork<YashilAppDbContext>>();
        services.AddScoped<IUserControllerExtender, UserControllerDefaultExtender>();

        services.AddScoped<IYashilFileProvider, YashilFileProvider>();

        var typeFinder = new WebAppTypeFinder();
        //register type finder
        services.AddSingleton<ITypeFinder>(typeFinder);

        var orderedMapperProfiles = new List<IOrderedMapperProfile>();
        foreach (var module in GlobalConfiguration.Modules)
        {
            var moduleInitializerType = module.Assembly.GetTypes()
                .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
            if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
            {
                var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                services.AddSingleton(typeof(IModuleInitializer), moduleInitializer ?? throw new InvalidOperationException());
                moduleInitializer.ConfigureServices(services, _configuration, typeFinder);
            }

            var orderedMapperProfile = module.Assembly.GetTypes()
                .FirstOrDefault(t => typeof(IOrderedMapperProfile).IsAssignableFrom(t));
            if (orderedMapperProfile != null)
            {
                orderedMapperProfiles.Add((IOrderedMapperProfile)Activator.CreateInstance(orderedMapperProfile));
            }
        }

        var mappingConfig = new MapperConfiguration(cfg =>
        {
            foreach (var instance in orderedMapperProfiles)
            {
                cfg.AddProfile(instance.GetType());
            }
        });

        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);

        //register CustomResultFilter
        // this filter add some Header in response 
        services.AddSingleton<AddHeaderFilter>();

        services.AddResponseCaching();
        services.AddRazorPages().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
            .AddMvcOptions(o =>
            {
                o.Filters.AddService(typeof(AddHeaderFilter));
                o.EnableEndpointRouting = false;
                o.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
                o.Filters.Add(new RequestSizeLimitAttribute(long.MaxValue));
            }).AddRazorRuntimeCompilation()
            // برای کانفیگ کردن خطاهایی که بصورت api ارسال میشود از این اپشن استفاده میکنیم
            // این تنظیم به منظور تمهیدات امنیتی لحاظ شده است

            .ConfigureApiBehaviorOptions(options =>
            {
                //InvalidModelStateResponseFactory is a Func delegate  
                // and used to customize the error response.    
                //It is exposed as property of ApiBehaviorOptions class  
                // that is used to configure api behaviour.    
                if (!_hostingEnvironment.IsDevelopment())
                {
                    options.InvalidModelStateResponseFactory = CustomErrorResponse;
                }
            });
        services.Configure<FormOptions>(options =>
        {
            // Set the limit to 400 MB
            options.MultipartBodyLengthLimit = 2194304000;
        });
        var serviceProvider = services.BuildServiceProvider();
        var applicationService = serviceProvider.GetService<IApplicationService>();
        applicationService?.ValidationApplicationConfiguration();

        /*
      * برای آنکه sessionTiimeout
      * قابل تنظیم باشد آنرا از appconfig میخوانیم
      * و چون در yashilSpl این مقدار را نیاز داریم در اینجا مقدار آنرا در صورت وجود
      * در applicationConfigurationنیز مقدار میدهیم
      * باید توجه داشت که سرویس  applicationConfiguration بصورت
      * singleton است
      * پس هنگامیکه مقدار sessionTimeout در appconfig
      * تغییر کرد باید در داخل سرویس applicationSesvice نیز تغییر کند
      */
        var appConfigService = serviceProvider.GetService<IAppConfigService>();
        var sessionTimeOut = appConfigService?.GetIntValueAsync(ApplicationConfigKey.SiteConfigurationSecuritySessionTimeOut).Result;
        if (sessionTimeOut != null)
        {
            applicationConfiguration.SessionTimeOut = sessionTimeOut.Value;
        }
        var siteMaxUserCount = appConfigService?.GetIntValueAsync(ApplicationConfigKey.SiteConfigurationSecuritySiteMaxUserCount).Result;
        if (siteMaxUserCount != null)
        {
            applicationConfiguration.SiteMaxUserCount = siteMaxUserCount.Value;
        }

        var newCharacterCountDifferWithOldPassword = appConfigService?.GetIntValueAsync(ApplicationConfigKey.SiteConfigurationSecurityNewCharacterCountDifferWithOldPassword).Result;
        if (newCharacterCountDifferWithOldPassword != null)
        {
            applicationConfiguration.NewCharacterCountDifferWithOldPassword = newCharacterCountDifferWithOldPassword.Value;
        }

        if (applicationConfiguration.EnableUserLockFeature)
        {
            var durationOfUserLockout = appConfigService?.GetIntValueAsync(ApplicationConfigKey.SiteConfigurationSecurityInactiveLockUserSeconds).Result;
            if (durationOfUserLockout != null)
            {
                applicationConfiguration.DurationOfUserLockout = durationOfUserLockout.Value;
            }
        }
    }

    // Below method extracts model state errors and assigns to the properties of Custom class.    
    /// <summary>
    /// برای مخفی کردن خطاهایی که بصورت Api ارسال میشوند این متد نوشته شده است
    /// </summary>
    /// <param name="actionContext"></param>
    /// <returns></returns>
    private BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
    {
        //BadRequestObjectResult is class found Microsoft.AspNetCore.Mvc and is inherited from ObjectResult.    
        //Rest code is linq.    
        // لیست خطاها را دریافت می کنیم
        // بجای متن خطا متن خطای کانفیگ شده را ارسال می کنیم

        var messageList = actionContext.ModelState
            .Where(modelError => modelError.Value != null && modelError.Value.Errors.Count > 0)
            .Select(modelError => new
            {
                //ErrorField = modelError.Key,
                //ErrorDescription = modelError.Value.Errors.FirstOrDefault().ErrorMessage
                ErrorField = modelError.Key,
                ErrorDescription = "لطفا ورودی را چک کنید"
            }).ToList();

        var res = new BadRequestObjectResult(messageList);

        var loginService =
            (IYashilLogger<object>)actionContext.HttpContext.RequestServices.GetService(typeof(IYashilLogger<object>))!;
        {
            var message = string.Join(" | ", actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var controllerName = actionContext.RouteData.Values["controller"]?.ToString()?.Replace("Controller", "");
            loginService.InsertLog(LogLevel.Error,
                new EventId((int)Events.InvalidParamsUpdate,
                    Events.InvalidParamsUpdate.ToString()),
                controllerName,
                message,
                new Exception(message));
        }
        return res;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            //app.UseSwagger();
            //app.UseSwaggerUI();

            //app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            //{
            //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //    options.RoutePrefix = string.Empty;
            //});
        }
        else
        {
            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                nonApiApp => nonApiApp.UseExceptionHandler("/Error"));
        }
        app.UseForwardedHeaders();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        var contentDirectoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Contents");
        if (!Directory.Exists(contentDirectoryPath))
        {
            Directory.CreateDirectory(contentDirectoryPath);
        }
        var staticFileOption = new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(contentDirectoryPath),
        };
        app.UseStaticFiles(staticFileOption);

        //برای اضافه کردن متریک های مربوط به در خواست ها
        //<<<<<-----در نمایش ویدیو ها خطا داشت و فعلا کامنت شد---->>>>>>>
        // app.UseMiddleware<ResponseMetricMiddleware>();


        //app.UseOpenTelemetryPrometheusScrapingEndpoint();




        app.UseRouting();

        app.UseDevExpressControls();
        app.UseCors("CorsPolicy");
        //UseCors must be called before UseResponseCaching when using CORS middleware.

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Value != null && !context.Request.Path.Value.StartsWith("/assets/") && !context.Request.Path.Value.EndsWith(".js"))
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    var jwToken = context.Request.Cookies["JwtToken"];
                    if (!string.IsNullOrEmpty(jwToken))
                    {
                        context.Request.Headers.Add("Authorization", "Bearer " + jwToken);
                    }
                }
            }
            await next();
        });

        //redirect to login page if no authorized
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                response.StatusCode == (int)HttpStatusCode.Forbidden)
                if (!context.HttpContext.Request.Path.ToString().StartsWith("/api"))
                {
                    try
                    {
                        response.Redirect("/user/login");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCaching();
        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        app.UseEndpoints(endpoints =>
        {
            BindGrpcServices(endpoints);
            EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard", "yashilDashboard");
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501
            // spa.Options.SourcePath = "ClientApp";
            //spa.Options.DefaultPage = "";
            //if (env.IsDevelopment())
            //{
            //    spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);
            //    spa.UseAngularCliServer(npmScript: "start");
            //}
        });
    }

    private void BindGrpcServices(IEndpointRouteBuilder endpoints)
    {
        foreach (var module in GlobalConfiguration.Modules)
        {
            var grpcServiceInitializerType = module.Assembly.GetTypes()
                .FirstOrDefault(t => typeof(IGrpcServiceInitializer).IsAssignableFrom(t));
            if (grpcServiceInitializerType != null && grpcServiceInitializerType != typeof(IModuleInitializer))
            {
                var grpcServiceInitializerInstance = Activator.CreateInstance(grpcServiceInitializerType);
                if (grpcServiceInitializerInstance != null)
                {
                    var grpcServiceInitializer = (IGrpcServiceInitializer)grpcServiceInitializerInstance;
                    grpcServiceInitializer.BindGrpcServices(endpoints);
                }

            }
        }

        foreach (var module in GlobalConfiguration.Modules)
        {
            var viewModels = module.Assembly.GetTypes()
                .FirstOrDefault(t => typeof(IBaseViewModel).IsAssignableFrom(t));

            //if (viewModels != null && viewModels.has)
            //{
            //    var grpcServiceInitializerInstance = Activator.CreateInstance(viewModels);
            //    if (grpcServiceInitializerInstance != null)
            //    {
            //        var grpcServiceInitializer = (IGrpcServiceInitializer)grpcServiceInitializerInstance;
            //        grpcServiceInitializer.BindGrpcServices(endpoints);
            //    }

            //}
        }

    }
 
}