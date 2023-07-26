using Microsoft.Extensions.Hosting;

namespace Reoria.Application.Logging.Interfaces
{
    public interface ILogBinder
    {
        ILogBinder AttachToHost(IHostBuilder hostBuilder);
    }
}
