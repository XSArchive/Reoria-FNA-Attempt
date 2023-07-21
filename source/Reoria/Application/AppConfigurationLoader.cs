using Microsoft.Extensions.Configuration;
using Reoria.Application.Interfaces;
using static Reoria.Application.AppEnvironment;

namespace Reoria.Application
{
    public class AppConfigurationLoader : IAppConfigurationLoader
    {
        private readonly IConfigurationBuilder configurationBuilder;

        public IConfigurationBuilder Builder => configurationBuilder;

        public AppConfigurationLoader()
        {
            configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ActiveEnvironment.ToLower()}.json", optional: true, reloadOnChange: true);
        }

        public virtual IAppConfigurationLoader AddJsonFilesFromDirectory(string directoryPath)
        {
            AddJsonFilesFromDirectory(directoryPath, "appsettings.*.json", Environments);
            AddJsonFilesFromDirectory(directoryPath, $"appsettings.*.{ActiveEnvironment.ToLower()}.json");

            return this;
        }

        protected virtual void AddJsonFilesFromDirectory(string directoryPath, string searchPattern, string[]? filters = null)
        {
            var appSettingsFiles = Directory.GetFiles(directoryPath, searchPattern);
            foreach (var appSettingsFile in appSettingsFiles)
            {
                if ((filters is null) || !filters.Any(env => appSettingsFile.Contains(env)))
                {
                    configurationBuilder.AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true);
                }
            }
        }
    }
}
