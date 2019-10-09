using System;
using System.Runtime.InteropServices;
using Bridge.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.BackgroundServices;
using Nodester.Bridge.HubConnections;
using Nodester.Bridge.Services;
using Nodester.Common.Data.Interfaces;
using Nodester.Common.Extensions;
using Nodester.Services;
using Polly;

namespace Nodester.Bridge
{
    public class Program
    {
        private static TimeSpan[] Retries =
        {
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60)
        };
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        private static string Environment { get; set; }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
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
                .ConfigureServices((hostingContext, services) =>
                {
                    AppState.Instance.Username = hostingContext.Configuration.GetValue<string>("username");
                    AppState.Instance.Password = hostingContext.Configuration.GetValue<string>("password");

                    services.AddOptions();
                    services.Configure<AppConfig>(hostingContext.Configuration.GetSection("AppConfig"));

                    services.AddHttpClient<INodesterLoginService, NodesterLoginService>()
                        .AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(Retries));
                    services.AddHttpClient<INodesterGraphService, NodesterGraphService>()
                        .AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(Retries));
                    services.AddHttpClient<INodesterUserService, NodesterUserService>()
                        .AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(Retries));

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
}
