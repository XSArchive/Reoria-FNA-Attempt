using Microsoft.Extensions.Logging;

namespace Reoria.Hosting.Logging.Interfaces
{
    public interface ILogBinder
    {
        ILogBinder AttachToHost(ILoggingBuilder loggingBuilder);
    }
}
