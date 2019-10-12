using System;
using System.Runtime.InteropServices;
using Bridge.Data;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.BackgroundServices;
using Nodester.Bridge.HubConnections;
using Nodester.Bridge.Services;
using Nodester.Common.Data.Interfaces;
using Nodester.Services;
using Polly;

namespace Nodester.Bridge
{
    class Options
    {
        [Option('e', "environment")] public string Environment { get; set; }
        [Option('u', "username")] public string Username { get; set; }
        [Option('p', "password")] public string Password { get; set; }
    }

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
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => { CreateHostBuilder(args, o).Build().Run(); });
        }

        private static IHostBuilder CreateHostBuilder(string[] args, Options option)
        {
            var host = Host.CreateDefaultBuilder(args);

            if (!string.IsNullOrEmpty(option.Environment))
            {
                host.UseEnvironment(option.Environment);
            }

            return host
                .UseWindowsService()
                .UseSystemd()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        logging.AddEventLog();
                    }
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    AppState.Instance.Username = option.Username;
                    AppState.Instance.Password = option.Password;

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