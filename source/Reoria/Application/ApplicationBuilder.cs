using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reoria.Application.Interfaces;

namespace Reoria.Application
{
    public class ApplicationBuilder : IApplicationBuilder 
    {
        private readonly IHostBuilder builder;
        private IHost? host;

        public ApplicationBuilder(string[]? args)
        {
            builder = Host.CreateDefaultBuilder(args);
        }

        public IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            builder.ConfigureServices(configureDelegate);

            return this;
        }

        public TService? BuildApplication<TService>() where TService : class
        {
            host ??= builder.Build();

            return host?.Services.GetRequiredService<TService>();
        }
    }
}
