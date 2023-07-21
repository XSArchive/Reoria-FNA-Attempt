using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reoria.Application.Interfaces
{
    public interface IApplicationBuilder
    {
        IApplicationBuilder AttachSerilog(ISerilogBinder serilog);
        TService? BuildApplication<TService>() where TService : class;
        IApplicationBuilder ConfigureConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
        IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    }
}
