using Microsoft.Extensions.Configuration;
using Reoria.Application.Configuration.Interfaces;
using static Reoria.Application.AppEnvironment;

namespace Reoria.Application.Configuration
{
    public class ConfigurationLoader : ConfigurationBuilder, IConfigurationLoader
    {

        public ConfigurationLoader()
        {
            this.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ActiveEnvironment.ToLower()}.json", optional: true, reloadOnChange: true);
        }

        public virtual IConfigurationLoader AddJsonFilesFromDirectory(string directoryPath)
        {
            AddJsonFilesFromDirectory(directoryPath, "appsettings.*.json", Environments);
            AddJsonFilesFromDirectory(directoryPath, $"appsettings.*.{ActiveEnvironment.ToLower()}.json");

            return this;
        }

        protected virtual void AddJsonFilesFromDirectory(string directoryPath, string searchPattern, string[]? filters = null)
        {
            var appSettingsFiles = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
            foreach (var appSettingsFile in appSettingsFiles)
            {
                if (filters is null || !filters.Any(env => appSettingsFile.Contains(env)))
                {
                    this.AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true);
                }
            }
        }
    }
}
