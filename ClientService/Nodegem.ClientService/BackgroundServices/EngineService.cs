using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodegem.ClientService.Data;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Services;
using Timer = System.Timers.Timer;

namespace Nodegem.ClientService.BackgroundServices
{
    public class EngineService : BackgroundService
    {
        private const int PingTimeInMinutes = 3;

        private readonly AppConfig _appConfig;
        private readonly ILogger<EngineService> _logger;
        private readonly IGraphHubConnection _graphConnection;
        private readonly INodegemApiService _apiService;
        private readonly IBuildGraphService _buildGraphService;
        private readonly IBuildMacroService _buildMacroService;
        private readonly ITerminalHubConnection _terminalHubConnection;
        private readonly ILoggerFactory _loggerFactory;

        private Coordinator _coordinator;
        private Timer _timer;

        public EngineService(
            ILogger<EngineService> logger,
            IOptions<AppConfig> appConfig,
            IGraphHubConnection connection,
            INodegemApiService apiService,
            IServiceProvider provider,
            IBuildGraphService buildGraphService,
            IBuildMacroService buildMacroService,
            ITerminalHubConnection terminalHubConnection,
            ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _appConfig = appConfig.Value;
            _apiService = apiService;
            _graphConnection = connection;
            _buildGraphService = buildGraphService;
            _terminalHubConnection = terminalHubConnection;
            _buildMacroService = buildMacroService;
            _loggerFactory = loggerFactory;

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

                using var scopedProvider = provider.CreateScope();
                NodeCache.CacheNodeData(scopedProvider.ServiceProvider);
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
                _logger.LogInformation($"Starting bridge... (ID: {AppState.Instance.Identifier})");

                _logger.LogInformation("Establishing Connection...");
                await ConnectToServices(cancellationToken);
                _logger.LogInformation("Connected!!");

                _logger.LogInformation("Retrieving Graphs...");
                var graphs = await _apiService.GraphService.GetGraphsAsync();
                AppState.Instance.GraphLookUp = graphs.ToDictionary(k => k.Id, v => v);

                _coordinator = new Coordinator(_graphConnection, _buildGraphService, _buildMacroService, _loggerFactory.CreateLogger<Coordinator>());
                await _coordinator.InitializeAsync();

                _timer = new Timer(_appConfig.PingTime)
                {
                    AutoReset = true,
                };

                _timer.Elapsed += async (sender, e) =>
                {
                    try
                    {
                        await _graphConnection.UpdateBridgeAsync(CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while pinging server");
                    }
                };

                _timer.Start();

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
            _timer.Stop();
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service...");
            _graphConnection?.Dispose();
            _terminalHubConnection?.Dispose();
            _coordinator?.Dispose();
            _timer?.Dispose();
            base.Dispose();
        }

        private async Task RetrieveToken()
        {
            var token = await _apiService.LoginService.GetAccessTokenAsync(AppState.Instance.Username,
                AppState.Instance.Password);
            AppState.Instance.Token =
                new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
        }

        private async Task ConnectToServices(CancellationToken cancellationToken)
        {
            await RetrieveToken();

            await _graphConnection.StartAsync(cancellationToken);
            await _terminalHubConnection.StartAsync(cancellationToken);
        }
    }
}