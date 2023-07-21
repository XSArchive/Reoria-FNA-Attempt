using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reoria.Application.Interfaces;

namespace Reoria.Application
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly IHostBuilder builder;
        private readonly Lazy<IHost> host;

        public ApplicationBuilder(string[] args)
        {
            builder = new HostBuilder().ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                config.AddCommandLine(args);
            });

            host = new Lazy<IHost>(() => builder.Build());
        }

        public IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            builder.ConfigureServices(configureDelegate);
            return this;
        }

        public IApplicationBuilder ConfigureConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
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
            return host.Value.Services.GetRequiredService<TService>();
        }
    }
}
