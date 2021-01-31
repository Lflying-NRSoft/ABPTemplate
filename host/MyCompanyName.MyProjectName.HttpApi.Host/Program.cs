using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MyCompanyName.MyProjectName
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            //集成日志 Serilog
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.ffff}|{CorrelationId}|{Level:u5}|{Message:lj}-{RequestId}|{Exception}{NewLine}";
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
#else
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
#endif
                .Enrich.WithProperty("Application", "MyProjectName")
                .Enrich.FromLogContext()
                .WriteTo.Console()
#if DEBUG
                .WriteTo.File("Logs/logs.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate)
#else
                .WriteTo.Async(c => c.File(
                    "Logs/logs.txt",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 50_000_000, // 50M
                    rollOnFileSizeLimit: true,
                    buffered: true,
                    outputTemplate: outputTemplate))
#endif
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
