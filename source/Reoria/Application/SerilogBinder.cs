using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reoria.Application.Interfaces;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Reoria.Application
{
    public class SerilogBinder : ISerilogBinder
    {
        public readonly ILogger logger;

        public SerilogBinder()
        {
            logger = Build();
        }

        protected virtual IConfigurationBuilder LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ApplicationBuilder.GetEnvironment().ToLower()}.json", optional: true, reloadOnChange: true);
        }

        protected virtual LoggerConfiguration BuildLoggerConfiguration()
        {
            return new LoggerConfiguration()
                    .ReadFrom.Configuration(LoadConfiguration().Build())
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
        }

        protected virtual ILogger Build(bool updateStaticLogger = true)
        {
            if(updateStaticLogger)
            {
                Log.CloseAndFlush();

                return Log.Logger = BuildLoggerConfiguration().CreateLogger();
            }

            return BuildLoggerConfiguration().CreateLogger();
        }

        public virtual ISerilogBinder AttachToHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog(logger: logger, dispose: true);
                });
            });

            return this;
        }
    }
}
