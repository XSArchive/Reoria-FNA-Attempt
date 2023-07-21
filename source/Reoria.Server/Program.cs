using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reoria.Application;

internal class Program
{
    private static void Main(string[] args)
    {
        var serilog = new SerilogBinder().AttachToStatic();

        var app = new ApplicationBuilder(args).ConfigureConfiguration((builder, configuration) =>
        {
            var appSettingsLoader = new AppConfigurationLoader();

            configuration.AddConfiguration(appSettingsLoader.Builder.AddEnvironmentVariables().Build());
        }).ConfigureServices((context, services) =>
        {
            services.AddTransient<IServerService, ServerService>();
        }).AttachSerilog(serilog)
        .BuildApplication<IServerService>();
        
        app?.Run();
    }

    private interface IServerService
    {
        void Run();
    }

    private class ServerService : IServerService
    {
        private readonly ILogger<ServerService> logger;
        private readonly IConfiguration configuration;

        public ServerService(ILogger<ServerService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public void Run()
        {
            logger.LogInformation($"Hello {configuration.GetValue<string>("username") ?? "Dave"}!");
        }
    }
}