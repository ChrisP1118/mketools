using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Compact;

namespace MkeAlerts.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.WriteTo.Console()
                //.WriteTo.File("logs/log.txt",
                //    rollingInterval: RollingInterval.Day,
                //    retainedFileCountLimit: 14,
                //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {SourceContext:l} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Logger(l => l
                    .MinimumLevel.Information()
                    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft.AspNetCore.Hosting"))
                    .WriteTo.Console()
                )
                .WriteTo.Logger(l => l
                    .MinimumLevel.Information()
                    .Filter.ByIncludingOnly(Matching.FromSource("MkeAlerts"))
                    .WriteTo.File("logs/MkeAlerts.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14
                    )
                )
                .WriteTo.Logger(l => l
                    .MinimumLevel.Information()
                    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft.AspNetCore.Hosting"))
                    .WriteTo.File("logs/Web.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14
                    )
                )
                .WriteTo.Logger(l => l
                    .MinimumLevel.Information()
                    .Filter.ByIncludingOnly(Matching.FromSource("MkeAlerts"))
                    .WriteTo.Seq("http://localhost:5341")
                    .WriteTo.File(
                        new RenderedCompactJsonFormatter(),
                        "logs/MkeAlerts.json",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14
                    )
                )
                .WriteTo.Logger(l => l
                    .MinimumLevel.Information()
                    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft.AspNetCore.Hosting"))
                    .WriteTo.Seq("http://localhost:5341")
                    .WriteTo.File(
                        new RenderedCompactJsonFormatter(),
                        "logs/Web.json",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14
                    )
                )
                .WriteTo.Logger(l => l
                    .MinimumLevel.Warning()
                    .WriteTo.Seq("http://localhost:5341")
                    .WriteTo.File(
                        new RenderedCompactJsonFormatter(),
                        "logs/Warnings.json",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14
                    )
                )
                //.WriteTo.Logger(l => l
                //    .MinimumLevel.Warning()
                //    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft.EntityFrameworkCore"))
                //    .WriteTo.Seq("http://localhost:5341")
                //    .WriteTo.File(
                //        new RenderedCompactJsonFormatter(),
                //        "logs/EF.json",
                //        rollingInterval: RollingInterval.Day,
                //        retainedFileCountLimit: 14
                //    )
                //)
                //.WriteTo.Logger(l => l
                //    .MinimumLevel.Warning()
                //    .Filter.ByIncludingOnly(Matching.FromSource("Hangfire"))
                //    .WriteTo.Seq("http://localhost:5341")
                //    .WriteTo.File(
                //        new RenderedCompactJsonFormatter(),
                //        "logs/Hangfire.json",
                //        rollingInterval: RollingInterval.Day,
                //        retainedFileCountLimit: 14
                //    )
                //)
                //<logger name=".*" minlevel="Warn" writeTo="EF" final="true" />
                //<logger name="Hangfire.*" minlevel="Info" writeTo="Hangfire" final="true" />
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                //logger.Debug("Starting app");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
