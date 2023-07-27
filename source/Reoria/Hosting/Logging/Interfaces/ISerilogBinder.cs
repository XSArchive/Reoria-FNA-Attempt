namespace Reoria.Hosting.Logging.Interfaces
{
    public interface ISerilogBinder : ILogBinder
    {
        ISerilogBinder AttachToStatic();
    }
}
