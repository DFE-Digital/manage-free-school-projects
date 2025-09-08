using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Dfe.ManageFreeSchoolProjects;

public static class Program
{
   public static void Main(string[] args)
   {
      Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
         .Enrich.FromLogContext()
         .WriteTo.Console(new RenderedCompactJsonFormatter())
         .CreateLogger();

      Log.Information("Starting web host");
      CreateHostBuilder(args).Build().Run();
   }

   public static IHostBuilder CreateHostBuilder(string[] args)
   {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, configuration) => configuration.AddEnvironmentVariables())
            .UseSerilog((hostContext, configureLogger) =>
            {
                var connectionString = hostContext.Configuration["ApplicationInsights:ConnectionString"];
                configureLogger.WriteTo.ApplicationInsights(TelemetryConfiguration.CreateFromConfiguration(connectionString), TelemetryConverter.Traces);
            })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseKestrel(options =>
                {
                   options.AddServerHeader = false;
                });
             });
   }
}
