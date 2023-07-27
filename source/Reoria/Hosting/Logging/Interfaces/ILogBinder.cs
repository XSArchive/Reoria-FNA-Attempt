using Microsoft.Extensions.Hosting;

namespace Reoria.Hosting.Logging.Interfaces
{
    public interface ILogBinder
    {
        ILogBinder AttachToHost(IHostBuilder hostBuilder);
    }
}
