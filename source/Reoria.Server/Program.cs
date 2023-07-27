using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reoria.Framework.Serialization;
using Reoria.Framework.Serialization.Extensions;
using Reoria.Framework.Serialization.Interfaces;
using Reoria.Hosting;
using Reoria.Hosting.Configuration;
using Reoria.Hosting.Logging;

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
            services.AddTransient(typeof(IJsonSerializer<>), typeof(JsonSerializer<>));
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
        private readonly IJsonSerializer<TestJsonClass> serializer;

        public ServerService(ILogger<ServerService> logger, IConfiguration configuration, IJsonSerializer<TestJsonClass> serializer)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.serializer = serializer;
        }

        public void Run()
        {
            logger.LogInformation("Hello {username}!", configuration.GetValue<string>("username") ?? "Dave");

            if(!File.Exists("test.json"))
            {
                serializer.SerializeToFile(new TestJsonClass(), "test.json");
            }

            var test = serializer.DeserializeFromFile("test.json");
            while(true)
            {
                test.CurrentDateTime = DateTime.Now;
                serializer.SerializeToFile(test, "test.json");

                Thread.Sleep(100);
            }
        }
    }

    public class TestJsonClass
    {
        public Guid Guid { get; set; }
        public DateTime CurrentDateTime { get; set; }

        public TestJsonClass()
        {
            Guid = Guid.NewGuid();
            CurrentDateTime = DateTime.Now;
        }
    }
}