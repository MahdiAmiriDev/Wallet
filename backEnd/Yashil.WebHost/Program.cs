using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Yashil.Common.SharedKernel.Helpers;

namespace Yashil.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {

                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .Build();
            // CreateWebHostBuilder(args).Build().Run();
            
            CreateHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot conf)
        {
            var columnOptionsSection = conf.GetSection("Serilog:ColumnOptions");
            var sinkOptionsSection = conf.GetSection("Serilog:SinkOptions");
            const string schemaName = "dbo";
            const string tableName = "LogEvents";
            var connectionString = CryptographyHelper.AesDecrypt(conf.GetConnectionString("YashilAppDB"), true);

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog((context, config) =>
                    {
                        config.ReadFrom.Configuration(context.Configuration).WriteTo.MSSqlServer(
                            connectionString,
                            new MSSqlServerSinkOptions
                            {
                                TableName = tableName,
                                SchemaName = schemaName,
                                AutoCreateSqlTable = true
                            },
                            sinkOptionsSection,
                             conf,
                            columnOptionsSection: columnOptionsSection);
                        Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                        Serilog.Debugging.SelfLog.Enable(Console.Error);
                    }
                );

        }
        /// <summary>
        /// options.AddServerHeader = false بخاطر اینکه ورژن IIS  را نشان ندهد
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
                Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                    .UseKestrel(options => options.AddServerHeader = false)
                    .UseStartup<Startup>().Build();


    }
}