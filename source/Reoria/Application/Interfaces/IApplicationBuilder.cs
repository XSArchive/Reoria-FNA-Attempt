using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reoria.Application.Interfaces
{
    public interface IApplicationBuilder
    {
        TService? BuildApplication<TService>() where TService : class;
        IApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    }
}
