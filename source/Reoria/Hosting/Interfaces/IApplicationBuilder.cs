using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reoria.Hosting.Logging.Interfaces;

namespace Reoria.Hosting.Interfaces
{
    public interface IApplicationBuilder
    {
        IApplicationBuilder AttachSerilog(ISerilogBinder serilog);
        TService? BuildApplication<TService>() where TService : class;
        IApplicationBuilder ConfigureConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
        IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    }
}
