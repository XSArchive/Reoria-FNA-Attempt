using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reoria.Hosting.Logging.Interfaces;

namespace Reoria.Hosting
{
    public class Application : IDisposable
    {
        private static T CreateService<T>() => Activator.CreateInstance<T>();

        public delegate void DisposeDelegate();
        public delegate void ConfigureStaticLoggingDelegate(ILogBinder binder);
        public delegate void ConfigureLoggingDelegate(ILoggingBuilder logging);
        public delegate void ConfigureConfigurationDelegate(IConfigurationBuilder configuration);
        public delegate void ConfigureServicesDelegate(HostBuilderContext context, IServiceCollection services);

        public DisposeDelegate? OnDisposeManaged;
        public DisposeDelegate? OnDisposeUnmanaged;
        public ConfigureStaticLoggingDelegate? OnConfigureStaticLogging;
        public ConfigureLoggingDelegate? OnConfigureLogging;
        public ConfigureConfigurationDelegate? OnConfigureConfiguration;
        public ConfigureServicesDelegate? OnConfigureServices;

        private readonly IHostBuilder builder;
        private readonly Lazy<IHost> host;

        private IConfigurationBuilder? configuration;
        private ILogBinder? binder;
        private bool disposedValue;

        public Application()
        {
            builder = new HostBuilder();
            host = new Lazy<IHost>(() => builder.Build());
        }

        ~Application()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Application AssignConfigurationBuilder<T>() where T : IConfigurationBuilder
        {
            configuration = CreateService<T>();

            return this;
        }

        public Application AssignLogBinder<T>() where T : ILogBinder
        {
            binder = CreateService<T>();

            return this;
        }

        public TService Start<TService>(params string[] args) where TService : notnull
        {
            if (binder?.GetType().GetMethod("AttachToStatic") != null)
            {
                binder?.GetType().GetMethod("AttachToStatic")?.Invoke(binder, null);
            }

            builder.ConfigureHostConfiguration(configuration =>
            {
                if (this.configuration != null)
                {
                    configuration.AddConfiguration(this.configuration.Build());
                    OnConfigureConfiguration?.Invoke(configuration);
                }
                else
                {
                    OnConfigureConfiguration?.Invoke(configuration);
                }
                configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                configuration.AddCommandLine(args);
            });

            builder.ConfigureServices((context, services) =>
            {
                if (binder != null)
                {
                    services.AddLogging(logging =>
                    {
                        logging.ClearProviders();
                        binder.AttachToHost(logging);
                    });
                }

                OnConfigureServices?.Invoke(context, services);
            });

            return host.Value.Services.GetRequiredService<TService>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDisposeManaged?.Invoke();
                }

                OnDisposeUnmanaged?.Invoke();
                disposedValue = true;
            }
        }
    }
}
