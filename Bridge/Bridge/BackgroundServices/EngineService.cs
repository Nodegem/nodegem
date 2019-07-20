using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodester.Data;
using Nodester.Services;

namespace Nodester.Bridge.BackgroundServices
{
    public class EngineService : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<EngineService> _logger;
        private readonly IGraphHubConnection _graphConnection;
        private readonly INodesterLoginService _loginService;
        private readonly INodesterGraphService _graphService;
        private readonly IBuildGraphService _buildGraphService;
        private readonly IBuildMacroService _buildMacroService;
        private readonly ITerminalHubConnection _terminalHubConnection;

        private Coordinator _coordinator;

        public EngineService(
            ILogger<EngineService> logger,
            IOptions<AppConfig> appConfig,
            IGraphHubConnection connection,
            INodesterLoginService loginService,
            INodesterGraphService graphService,
            IServiceProvider provider,
            IBuildGraphService buildGraphService,
            IBuildMacroService buildMacroService,
            ITerminalHubConnection terminalHubConnection)
        {
            _logger = logger;
            _appConfig = appConfig.Value;
            _graphConnection = connection;
            _loginService = loginService;
            _graphService = graphService;
            _buildGraphService = buildGraphService;
            _terminalHubConnection = terminalHubConnection;
            _buildMacroService = buildMacroService;

            Initialize(provider);
        }

        private void Initialize(IServiceProvider provider)
        {
            try
            {
                if (string.IsNullOrEmpty(AppState.Instance.Username) ||
                    string.IsNullOrEmpty(AppState.Instance.Password))
                {
                    throw new ArgumentException();
                }

                NodeCache.CacheNodeData(provider);
            }
            catch (ArgumentException)
            {
                _logger.LogCritical("Must provide username and password for application to run.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error during node cache.");
                throw;
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var appId = AppState.Instance.DeviceIdentifier;
                _logger.LogInformation($"Starting bridge... (ID: {appId})");

                _logger.LogInformation("Establishing Connection...");
                await ConnectToServices(cancellationToken);
                _logger.LogInformation("Connected!!");

                _logger.LogInformation("Retrieving Graphs...");
                AppState.Instance.GraphLookUp = (await _graphService.GetGraphsAsync()).ToDictionary(k => k.Id, v => v);

                _coordinator = new Coordinator(_graphConnection, _buildGraphService, _buildMacroService);
                await _coordinator.InitializeAsync();

                await base.StartAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogCritical(ex, $"Unable to reach service: {_appConfig.Host}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Something went wrong");
                throw;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _coordinator.ExecuteRecurringGraphsAsync(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                //Ignored
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error during execution");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping...");
            await _graphConnection.StopAsync(cancellationToken);
            await _terminalHubConnection.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service...");
            _graphConnection?.Dispose();
            _terminalHubConnection?.Dispose();
            _coordinator?.Dispose();
            base.Dispose();
        }

        private async Task RetrieveToken()
        {
            var token = await _loginService.GetAccessTokenAsync(AppState.Instance.Username, AppState.Instance.Password);
            AppState.Instance.Token =
                new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
        }

        private async Task ConnectToServices(CancellationToken cancellationToken)
        {
            await RetrieveToken();

            var bridgeInfo = new BridgeInfo
            {
                DeviceIdentifier = AppState.Instance.DeviceIdentifier,
                DeviceName = Environment.MachineName,
                OperatingSystem = RuntimeInformation.OSDescription,
                ProcessorCount = Environment.ProcessorCount,
                UserId = AppState.Instance.UserId
            };

            AppState.Instance.Info = bridgeInfo;

            await _graphConnection.StartAsync(bridgeInfo, cancellationToken);
            await _terminalHubConnection.StartAsync(cancellationToken);
        }
    }
}