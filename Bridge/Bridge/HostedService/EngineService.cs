using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nodester.Bridge.HostedService
{
    public class EngineService : BackgroundService
    {
        private AppConfig AppConfig { get; }
        private ILogger<EngineService> Logger { get; }
        private readonly IGraphHubConnection _graphConnection;
        private readonly INodesterLoginService _loginService;

        public EngineService(ILogger<EngineService> logger, IOptions<AppConfig> appConfig, IGraphHubConnection connection,
            INodesterLoginService loginService)
        {
            Logger = logger;
            AppConfig = appConfig.Value;
            _graphConnection = connection;
            _loginService = loginService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("sdasda");
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting...");

            Logger.LogInformation("Establishing Connection...");
            await EstablishConnection(cancellationToken);
            Logger.LogInformation("Connected!!");

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping...");
            await _graphConnection.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            Logger.LogInformation("Disposing Service...");
            _graphConnection.Dispose();
            base.Dispose();
        }

        private async Task RetrieveToken()
        {
            AppState.Instance.Token =
                await _loginService.GetAccessTokenAsync(AppState.Instance.Username, AppState.Instance.Password);
        }

        private async Task EstablishConnection(CancellationToken cancellationToken)
        {
            try
            {
                await RetrieveToken();
                await _graphConnection.StartAsync(cancellationToken);
            }
            catch (TimeoutException ex)
            {
                Logger.LogError($"Unable to reach service: {AppConfig.Host}", ex);
                throw;
            }
            
            catch (Exception ex)
            {
                Logger.LogError($"Something went wrong.", ex);
                throw;
            }
        }
    }
}