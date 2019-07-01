using System;
using System.Threading.Tasks;
using Bridge.Data;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.Clients;
using Nodester.Bridge.HostedService;
using Nodester.Bridge.HubConnections;

namespace Nodester.Bridge
{
    public class Program
    {

        public static void Main(string[] args) 
            => CommandLineApplication.Execute<Program>(args);
        
        private const string EnvironmentKey = "ENVIRONMENT";
        
        [Option(Description = "The environment the app runs in", ShortName = "e")]
        public string Environment { get; }
        
        [Option(Description = "Account username", ShortName = "u")]
        public string Username { get; }
        
        [Option(Description = "Account password", ShortName = "p")]
        public string Password { get; }
        
        

        private async Task OnExecute()
        {
            var environment = Environment ?? System.Environment.GetEnvironmentVariable(EnvironmentKey);

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

                    services.AddSingleton<IHostedService, EngineService>();
                    services.AddSingleton<IGraphHubConnection, GraphHubConnection>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
