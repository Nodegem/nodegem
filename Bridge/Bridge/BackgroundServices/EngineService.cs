using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        public EngineService(ILogger<EngineService> logger, IOptions<AppConfig> appConfig,
            IGraphHubConnection connection,
            INodesterLoginService loginService, INodesterGraphService graphService,
            IServiceProvider provider, IBuildGraphService buildGraphService,
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

        private static void Initialize(IServiceProvider provider)
        {
            NodeCache.CacheNodeData(provider, "Nodester");            
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting...");

            _logger.LogInformation("Establishing Connection...");
            await ConnectToServices(cancellationToken);
            _logger.LogInformation("Connected!!");

            _logger.LogInformation("Retrieving Graphs...");
            AppState.Instance.Graphs = await _graphService.GetGraphsAsync();

            _coordinator = new Coordinator(_graphConnection, _buildGraphService, _buildMacroService);

            await base.StartAsync(cancellationToken);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
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
            _graphConnection.Dispose();
            _terminalHubConnection.Dispose();
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
            try
            {
                await RetrieveToken();
                await _graphConnection.StartAsync(cancellationToken);
                await _terminalHubConnection.StartAsync(cancellationToken);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError($"Unable to reach service: {_appConfig.Host}", ex);
                throw;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong.", ex);
                throw;
            }
        }
    }
}