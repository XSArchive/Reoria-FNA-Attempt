using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reoria.Client.FNA;
using Reoria.Hosting;
using Reoria.Hosting.Configuration;
using Reoria.Hosting.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        using var app = new Application()
            .AssignConfigurationBuilder<ConfigurationLoader>()
            .AssignLogBinder<SerilogBinder>();

        app.OnConfigureServices += (context, services) =>
        {
            services.AddTransient<IClientService, ClientService>();
        };

        app.Start<IClientService>(args).Run();
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