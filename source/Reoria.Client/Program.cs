using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reoria.Application;
using Reoria.Application.Configuration;
using Reoria.Client.FNA;

internal class Program
{
    private static void Main(string[] args)
    {
        var serilog = new SerilogBinder().AttachToStatic();

        var app = new ApplicationBuilder(args).ConfigureConfiguration((builder, configuration) =>
        {
            var configurationLoader = new ConfigurationLoader();

            configuration.AddConfiguration(configurationLoader.AddEnvironmentVariables().Build());
        }).ConfigureServices((context, services) =>
        {
            services.AddTransient<IClientService, ClientService>();
        }).AttachSerilog(serilog)
        .BuildApplication<IClientService>();
        
        app?.Run();
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