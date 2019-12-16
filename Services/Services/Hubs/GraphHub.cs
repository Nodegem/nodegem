using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nodegem.Common;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Common.Extensions;
using Nodegem.Services.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Nodegem.Services.Hubs
{
    [Authorize]
    public class GraphHub : Hub
    {
        public const int ExpirationTimeInMinutes = 60;
        public static TimeSpan DefaultExpiration;

        private readonly IDistributedCache _cache;
        private readonly ILogger<GraphHub> _logger;

        public GraphHub(ILogger<GraphHub> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
            DefaultExpiration = TimeSpan.FromMinutes(ExpirationTimeInMinutes);
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task ClientConnectAsync()
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                clientData.WebClientConnectionIds.AddOrUpdate(Context.ConnectionId);
                await UpdateClientDataAsync(clientData);
            }
            else
            {
                await UpdateClientDataAsync(new ClientData
                {
                    Bridges = new List<BridgeInfo>(),
                    WebClientConnectionIds = new List<string> {Context.ConnectionId}
                });
            }
        }

        public async Task ClientDisconnectAsync()
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                clientData.WebClientConnectionIds.RemoveAll(x => x == Context.ConnectionId);

                if (!clientData.WebClientConnectionIds.Any())
                {
                    await Clients.Clients(clientData.Bridges.Select(x => x.GraphHubConnectionId).ToList())
                        .SendAsync("DisposeListenersAsync");
                }

                await UpdateClientDataAsync(clientData);
            }
        }

        public async Task EstablishBridgeAsync(BridgeInfo info)
        {
            var userId = Context.User.GetUserId();
            info.GraphHubConnectionId = Context.ConnectionId;

            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);

                // Just a ping
                if (clientData.Bridges.Any(x =>
                    x.DeviceIdentifier == info.DeviceIdentifier && x.GraphHubConnectionId == info.GraphHubConnectionId))
                {
                    return;
                }

                _logger.LogInformation($"Establishing bridge. Device: {info.DeviceName} ({info.OperatingSystem})");
                clientData.Bridges.AddOrUpdate(info, x => x.DeviceIdentifier == info.DeviceIdentifier);
                await UpdateClientDataAsync(clientData);
                await Clients.Clients(clientData.WebClientConnectionIds).SendAsync("BridgeEstablishedAsync", info);
            }
            else
            {
                await UpdateClientDataAsync(new ClientData
                {
                    Bridges = new List<BridgeInfo> {info},
                    WebClientConnectionIds = new List<string>()
                });
            }
        }

        public async Task RemoveBridgeAsync()
        {
            var userId = Context.User.GetUserId();
            var connectionId = Context.ConnectionId;

            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                if (clientData.ContainsConnectionId(connectionId))
                {
                    clientData.Bridges.RemoveAll(x => x.GraphHubConnectionId == connectionId);
                    await UpdateClientDataAsync(clientData);
                    await Clients.Clients(clientData.WebClientConnectionIds).SendAsync("LostBridgeAsync", connectionId);
                }
            }
        }

        public async Task RequestBridgesAsync()
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                await Clients.Client(Context.ConnectionId).SendAsync("RequestedBridgesAsync",
                    clientData?.Bridges);
            }
            else
            {
                await Clients.Client(Context.ConnectionId)
                    .SendAsync("RequestedBridgesAsync", Enumerable.Empty<BridgeInfo>());
            }
        }

        public async Task RunGraphAsync(GraphDto graph, string connectionId)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                if (clientData.ContainsConnectionId(connectionId))
                {
                    _logger.LogInformation(
                        $"User (Id: {graph.UserId}) executed graph (Id: {graph.Id}, Name: {graph.Name})");
                    await Clients.Client(connectionId).SendAsync("RemoteExecuteGraphAsync", graph);
                }
            }
        }

        public async Task RunMacroAsync(MacroDto macro, string connectionId, string flowInputFieldKey)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                if (clientData.ContainsConnectionId(connectionId))
                {
                    _logger.LogInformation(
                        $"User (Id: {macro.UserId}) executed macro (Id: {macro.Id}, Name: {macro.Name}) w/ input key {flowInputFieldKey}");
                    await Clients.Client(connectionId)
                        .SendAsync("RemoteExecuteMacroAsync", macro, flowInputFieldKey);
                }
            }
        }

        public async Task OnGraphErrorAsync(ExecutionErrorData errorData)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                await Clients.Clients(clientData.WebClientConnectionIds).SendAsync("GraphErrorAsync", errorData);
            }
        }

        public async Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var clientData = await _cache.GetAsync<ClientData>(userId);
                await Clients.Clients(clientData.WebClientConnectionIds).SendAsync("GraphCompletedAsync", errorData);
            }
        }

        private async Task UpdateClientDataAsync(ClientData clientData)
        {
            await _cache.SetAsync(Context.User.GetUserId(), clientData, DefaultExpiration);
        }


    }
}