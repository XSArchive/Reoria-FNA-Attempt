using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reoria.Application;
using Reoria.Client.FNA;

internal class Program
{
    private static void Main(string[] args)
    {
        new ApplicationBuilder(args).ConfigureSerilog<SerilogBinder>()
        .ConfigureConfiguration((builder, configuration) =>
        {
            var appSettingsLoader = new AppConfigurationLoader();

            configuration.AddConfiguration(appSettingsLoader.Builder.AddEnvironmentVariables().Build());
        }).ConfigureServices((context, services) =>
        {
            services.AddTransient<IClientService, ClientService>();
        }).AttachSerilog()
        .BuildApplication<IClientService>()?.Run();
    }

    private interface IClientService
    {
        void Run();
    }

    private class ClientService : IClientService
    {
        private readonly ILogger<ClientService> logger;

        public ClientService(ILogger<ClientService> logger)
        {
            this.logger = logger;
        }

        public void Run()
        {
            logger.LogInformation("Starting FNA game instance...");
            using var game = new GameInstance();
            game.Run();
        }
    }
}