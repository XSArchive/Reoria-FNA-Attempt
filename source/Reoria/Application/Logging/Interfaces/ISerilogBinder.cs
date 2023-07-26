namespace Reoria.Application.Logging.Interfaces
{
    public interface ISerilogBinder : ILogBinder
    {
        ISerilogBinder AttachToStatic();
    }
}
