using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                    var baseUrl = hostingContext.Configuration.GetSection("AppConfig").GetValue<string>("Host");
                    services.Configure<AppConfig>(hostingContext.Configuration.GetSection("AppConfig"));

                    services.AddHttpClient<INodesterApiService, NodesterApiService>
                        (client =>
                        {
                            client.BaseAddress = new Uri($"{baseUrl}/api/");
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