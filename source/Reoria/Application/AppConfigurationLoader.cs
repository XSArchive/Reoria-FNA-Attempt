using Microsoft.Extensions.Configuration;
using Reoria.Application.Interfaces;

namespace Reoria.Application
{
    public class AppConfigurationLoader : IAppConfigurationLoader
    {
        protected static readonly string[] Environments = { "Development", "Staging", "Production" };

        private readonly IConfigurationBuilder configurationBuilder;

        public IConfigurationBuilder Builder => configurationBuilder;

        public AppConfigurationLoader()
        {
            configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ApplicationBuilder.GetEnvironment().ToLower()}.json", optional: true, reloadOnChange: true);
        }

        public virtual IAppConfigurationLoader AddJsonFilesFromDirectory(string directoryPath)
        {
            AddJsonFilesFromDirectory(directoryPath, "appsettings.*.json", Environments);
            AddJsonFilesFromDirectory(directoryPath, $"appsettings.*.{ApplicationBuilder.GetEnvironment().ToLower()}.json");

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
