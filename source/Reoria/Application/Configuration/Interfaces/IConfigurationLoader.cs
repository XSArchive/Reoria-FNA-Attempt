using Microsoft.Extensions.Configuration;

namespace Reoria.Application.Configuration.Interfaces
{
    public interface IConfigurationLoader : IConfigurationBuilder
    {
        IConfigurationLoader AddJsonFilesFromDirectory(string directoryPath);
    }
}
