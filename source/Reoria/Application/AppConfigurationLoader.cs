using Microsoft.Extensions.Configuration;
using Reoria.Application.Interfaces;

namespace Reoria.Application
{
    public class AppConfigurationLoader : IAppConfigurationLoader
    {
        private static readonly string[] Environments = { "Development", "Staging", "Production" };

        private readonly IConfigurationBuilder configurationBuilder;

        public IConfigurationBuilder Builder => configurationBuilder;

        public AppConfigurationLoader()
        {
            configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ApplicationBuilder.GetEnvironment().ToLower()}.json", optional: true, reloadOnChange: true);
        }

        public IAppConfigurationLoader AddDirectory(string directoryPath)
        {
            var appSettingsFiles = Directory.GetFiles(directoryPath, "appsettings.*.json");
            foreach (var appSettingsFile in appSettingsFiles)
            {
                if (!Environments.Any(appSettingsFile.Contains))
                {
                    configurationBuilder.AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true);
                }
            }

            var appSettingsEnvironmentFiles = Directory.GetFiles(directoryPath, $"appsettings.*.{ApplicationBuilder.GetEnvironment().ToLower()}.json");
            foreach (var appSettingsFile in appSettingsEnvironmentFiles)
            {
                configurationBuilder.AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true);
            }

            return this;
        }
    }
}
