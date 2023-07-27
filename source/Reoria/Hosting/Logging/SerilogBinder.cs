using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reoria.Hosting.Logging.Interfaces;
using Serilog;
using static Reoria.Hosting.AppEnvironment;
using ILogger = Serilog.ILogger;

namespace Reoria.Hosting.Logging
{
    public class SerilogBinder : ISerilogBinder
    {
        private readonly IConfigurationRoot configuration;
        private readonly ILogger logger;

        public SerilogBinder()
        {
            configuration = LoadConfiguration().Build();
            logger = BuildLoggerConfiguration(configuration).CreateLogger();
        }

        protected virtual IConfigurationBuilder LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ActiveEnvironment.ToLower()}.json", optional: true, reloadOnChange: true);
        }

        protected virtual LoggerConfiguration BuildLoggerConfiguration(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        }

        public virtual ILogBinder AttachToHost(IHostBuilder hostBuilder)
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

        public virtual ISerilogBinder AttachToStatic()
        {
            Log.CloseAndFlush();
            Log.Logger = logger;

            return this;
        }
    }
}
