namespace Reoria.Application.Interfaces
{
    public interface IAppConfigurationLoader
    {
        IAppConfigurationLoader AddJsonFilesFromDirectory(string directoryPath);
    }
}
