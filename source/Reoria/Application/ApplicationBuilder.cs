using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reoria.Application.Interfaces;

namespace Reoria.Application
{
    public class ApplicationBuilder : IApplicationBuilder 
    {
        public static string GetEnvironment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        private readonly IHostBuilder builder;
        private IHost? host;
        private ISerilogBinder? serilog;

        public ApplicationBuilder(string[]? args)
        {
            builder = Host.CreateDefaultBuilder(args);
        }

        public IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            builder.ConfigureServices(configureDelegate);

            return this;
        }

        public IApplicationBuilder ConfigureConfiguration(Action<HostBuilderContext, Microsoft.Extensions.Configuration.IConfigurationBuilder> configureDelegate)
        {
            builder.ConfigureAppConfiguration(configureDelegate);

            return this;
        }

        public IApplicationBuilder AttachSerilog(ISerilogBinder serilog)
        {
            serilog.AttachToHost(builder);

            return this;
        }

        public TService? BuildApplication<TService>() where TService : class
        {
            host ??= builder.Build();

            return host?.Services.GetRequiredService<TService>();
        }
    }
}
