using Bridge.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.BackgroundServices;
using Nodester.Bridge.HubConnections;
using Nodester.Bridge.Services;
using Nodester.Common.Data.Interfaces;
using Nodester.Services;

namespace Nodester.Bridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                    }

                    logging.AddEventSourceLogger();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    AppState.Instance.Username = hostingContext.Configuration.GetValue<string>("username");
                    AppState.Instance.Password = hostingContext.Configuration.GetValue<string>("password");
                    
                    services.AddOptions();
                    services.Configure<AppConfig>(hostingContext.Configuration.GetSection("AppConfig"));

                    services.AddHttpClient<INodesterLoginService, NodesterLoginService>();
                    services.AddHttpClient<INodesterGraphService, NodesterGraphService>();
                    services.AddHttpClient<INodesterUserService, NodesterUserService>();

                    services.AddSingleton<IGraphHubConnection, GraphHubConnection>();

                    services.AddSingleton<ITerminalHubConnection, TerminalHubConnection>();
                    services.AddSingleton<ITerminalHubService>(
                        provider => provider.GetService<ITerminalHubConnection>());

                    services.AddSingleton<IBuildGraphService, GraphBuildService>();
                    services.AddSingleton<IBuildMacroService, MacroBuildService>();

                    services.AddServicesForBridge();
                    
                    services.AddHostedService<EngineService>();
                });
    }
}