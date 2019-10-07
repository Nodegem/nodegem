using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nodester.Common.Extensions;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Nodester.WebApi.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Nodester.Services.Hubs
{
    [Authorize]
    public class GraphHub : Hub
    {
        private const int ExpirationTimeInMinutes = 60;

        private readonly IDistributedCache _cache;
        private readonly ILogger<GraphHub> _logger;

        public GraphHub(ILogger<GraphHub> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
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

        public async Task EstablishBridgeAsync(BridgeInfo info)
        {
            var userId = Context.User.GetUserId();
            info.ConnectionId = Context.ConnectionId;
            _logger.LogInformation($"Establishing bridge. Device: {info.DeviceName} ({info.OperatingSystem})");

            if (await _cache.ContainsKeyAsync(userId))
            {
                var bridges = await _cache.GetAsync<List<BridgeInfo>>(userId);
                bridges.AddOrUpdate(info, x => x.DeviceIdentifier == info.DeviceIdentifier);
                await _cache.SetAsync(userId, bridges, TimeSpan.FromSeconds(ExpirationTimeInMinutes * 60));
            }
            else
            {
                await _cache.SetAsync(userId, new List<BridgeInfo> {info},
                    TimeSpan.FromSeconds(ExpirationTimeInMinutes * 60));
            }

            await Clients.Group(userId.ToString()).SendAsync("BridgeEstablishedAsync", info);
        }

        public async Task RemoveBridgeAsync()
        {
            var userId = Context.User.GetUserId();
            var connectionId = Context.ConnectionId;

            if (await _cache.ContainsKeyAsync(userId))
            {
                var bridges = await _cache.GetAsync<List<BridgeInfo>>(userId);
                bridges.RemoveAll(x => x.ConnectionId == connectionId);
                await _cache.SetAsync(userId, bridges, TimeSpan.FromSeconds(ExpirationTimeInMinutes * 60));
                await Clients.Group(userId.ToString()).SendAsync("LostBridgeAsync");
            }
        }

        public async Task RequestBridgesAsync()
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("BridgeAsync",
                    await _cache.GetAsync<IEnumerable<BridgeInfo>>(userId));
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("BridgeAsync", Enumerable.Empty<BridgeInfo>());
            }
        }

        public async Task RunGraphAsync(GraphDto graph, string connectionId)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var bridges = await _cache.GetAsync<IEnumerable<BridgeInfo>>(userId);
                if (bridges.Any(b => b.ConnectionId == connectionId))
                {
                    _logger.LogInformation(
                        $"User (Id: {graph.UserId}) executed graph (Id: {graph.Id}, Name: {graph.Name})");
                    await Clients.Client(connectionId).SendAsync("RemoteExecuteGraphAsync", graph);
                }
            }
        }

        public async Task RunMacroAsync(MacroDto macro, string flowInputFieldKey, string connectionId)
        {
            var userId = Context.User.GetUserId();
            if (await _cache.ContainsKeyAsync(userId))
            {
                var bridges = await _cache.GetAsync<IEnumerable<BridgeInfo>>(userId);
                if (bridges.Any(b => b.ConnectionId == connectionId))
                {
                    _logger.LogInformation(
                        $"User (Id: {macro.UserId}) executed macro (Id: {macro.Id}, Name: {macro.Name}) w/ input key {flowInputFieldKey}");
                    await Clients.Client(connectionId)
                        .SendAsync("RemoteExecuteMacroAsync", macro, flowInputFieldKey);
                }
            }
        }

        public async Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null)
        {
            var userId = Context.User.GetUserId();
            await Clients.Groups(userId.ToString()).SendAsync("GraphCompletedAsync", errorData);
        }
    }
}