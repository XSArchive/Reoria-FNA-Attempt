using Microsoft.Extensions.Configuration;

namespace Reoria.Hosting.Configuration.Interfaces
{
    public interface IConfigurationLoader : IConfigurationBuilder
    {
        IConfigurationLoader AddJsonFilesFromDirectory(string directoryPath);
    }
}
