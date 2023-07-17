using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reoria.Application;

internal class Program
{
    private static void Main(string[] args)
    {
        new ApplicationBuilder(args).ConfigureServices((context, services) =>
        {
            services.AddTransient<IServerService, ServerService>();
        }).BuildApplication<IServerService>()?.Run();
    }

    private interface IServerService
    {
        void Run();
    }

    private class ServerService : IServerService
    {
        private readonly IConfiguration configuration;

        public ServerService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Run()
        {
            Console.WriteLine($"Hello {configuration.GetValue<string>("username") ?? "Dave"}!");
        }
    }
}