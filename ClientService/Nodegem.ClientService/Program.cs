using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Nodegem.ClientService.BackgroundServices;
using Nodegem.ClientService.Data;
using Nodegem.ClientService.HubConnections;
using Nodegem.ClientService.Services;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Services;
using Polly;

namespace Nodegem.ClientService
{
    class Options
    {
        [Option('e', "endpoint", Default = AppConstants.NodegemEndpoint)]
        public string Endpoint { get; set; }

        [Option('u', "username")] public string Username { get; set; }
        [Option('p', "password")] public string Password { get; set; }

        [Option("pingTime", Default = AppConstants.PingTime)]
        public int PingTime { get; set; }
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
                .WithParsed(o =>
                {
                    try
                    {
                        CreateHostBuilder(args, o).Build().Run();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                });
        }

        private static IHostBuilder CreateHostBuilder(string[] args, Options option)
        {
            var host = Host.CreateDefaultBuilder(args);

            return host
                .UseWindowsService()
                .UseSystemd()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        logging.AddEventLog(settings =>
                        {
                            settings.SourceName = "Nodegem.ClientService";
                            settings.LogName = "Nodegem";
                        });
                    }
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    AppState.Instance.Username = option.Username;
                    AppState.Instance.Password = option.Password;

                    services.AddOptions();
                    services.Configure<AppConfig>(options =>
                    {
                        options.Host = option.Endpoint;
                        options.PingTime = option.PingTime;
                    });

                    services.AddHttpClient<INodegemApiService, NodegemApiService>
                        (client =>
                        {
                            client.BaseAddress = new Uri($"{option.Endpoint}/api/");
                            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                        })
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