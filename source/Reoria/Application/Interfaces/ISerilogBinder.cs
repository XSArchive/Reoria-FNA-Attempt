using Microsoft.Extensions.Hosting;

namespace Reoria.Application.Interfaces
{
    public interface ISerilogBinder
    {
        ISerilogBinder AttachToHost(IHostBuilder hostBuilder);
        ISerilogBinder AttachToStatic();
    }
}
