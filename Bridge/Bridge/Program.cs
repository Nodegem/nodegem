﻿using System;
using System.Threading.Tasks;
using Bridge.Data;
using McMaster.Extensions.CommandLineUtils;
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
        [Option(Description = "The environment the app runs in", ShortName = "e")]
        private string Environment { get; }

        [Option(Description = "Account username", ShortName = "u")]
        private string Username { get; }

        [Option(Description = "Account password", ShortName = "p")]
        private string Password { get; }

        public static void Main(string[] args)
        {
            try
            {
                CommandLineApplication.Execute<Program>(args);
            }
            catch (Exception)
            {
                System.Environment.Exit(1);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private async Task OnExecute()
        {
            var environment = Environment ?? "Development";

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                throw new ArgumentException("Username and password values are required.");
            }

            AppState.Instance.Username = Username;
            AppState.Instance.Password = Password;
            AppState.Instance.Environment = environment;

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.HostingEnvironment.EnvironmentName = environment;

                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config
                        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<AppConfig>(hostingContext.Configuration.GetSection("AppConfig"));

                    services.AddHttpClient<INodesterLoginService, NodesterLoginService>();
                    services.AddHttpClient<INodesterGraphService, NodesterGraphService>();
                    services.AddHttpClient<INodesterUserService, NodesterUserService>();

                    services.AddSingleton<IHostedService, EngineService>();
                    services.AddSingleton<IGraphHubConnection, GraphHubConnection>();

                    services.AddSingleton<ITerminalHubConnection, TerminalHubConnection>();
                    services.AddSingleton<ITerminalHubService>(
                        provider => provider.GetService<ITerminalHubConnection>());

                    services.AddSingleton<IBuildGraphService, GraphBuildService>();
                    services.AddSingleton<IBuildMacroService, MacroBuildService>();

                    services.AddServicesForBridge();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}