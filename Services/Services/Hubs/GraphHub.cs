using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nodester.Common.Extensions;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Task = System.Threading.Tasks.Task;

namespace Nodester.Services.Hubs
{
    [Authorize]
    public class GraphHub : Hub
    {
        private static ConcurrentDictionary<Guid, List<BridgeInfo>> Bridges { get; } =
            new ConcurrentDictionary<Guid, List<BridgeInfo>>();

        private readonly ILogger<GraphHub> _logger;

        public GraphHub(ILogger<GraphHub> logger)
        {
            _logger = logger;
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
            if (Bridges.ContainsKey(userId))
            {
                Bridges[userId].Add(info);
            }
            else
            {
                Bridges.TryAdd(userId, new List<BridgeInfo> {info});
            }

            await Clients.Group(userId.ToString()).SendAsync("BridgeEstablishedAsync", info);
        }

        public async Task RemoveBridgeAsync()
        {
            var userId = Context.User.GetUserId();
            var connectionId = Context.ConnectionId;
            var (found, bridges) = await GetBridgesAsync();

            if (found)
            {
                bridges.RemoveAll(x => x.ConnectionId == connectionId);
                await Clients.Group(userId.ToString()).SendAsync("LostBridgeAsync");
            }
        }

        public async Task RequestBridgesAsync()
        {
            var (found, info) = await GetBridgesAsync();
            if (found)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("BridgeAsync", info);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("BridgeAsync", Enumerable.Empty<BridgeInfo>());
            }
        }

        public async Task RunGraphAsync(GraphDto graph, string connectionId)
        {
            var (found, bridges) = await GetBridgesAsync();
            if (found)
            {
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
            var (found, bridges) = await GetBridgesAsync();
            if (found)
            {
                if (bridges.Any(b => b.ConnectionId == connectionId))
                {
                    _logger.LogInformation(
                        $"User (Id: {macro.UserId}) executed macro (Id: {macro.Id}, Name: {macro.Name}) w/ input key {flowInputFieldKey}");
                    await Clients.Client(connectionId)
                        .SendAsync("RemoteExecuteMacroAsync", macro, flowInputFieldKey);
                }
            }
        }

        public async Task RelayExecutionErrorsAsync(ExecutionErrorData errorData)
        {
            var userId = Context.User.GetUserId();
            await Clients.Groups(userId.ToString()).SendAsync("ExecutionErrorAsync", errorData);
        }

        private async Task<(bool found, List<BridgeInfo> info)> GetBridgesAsync()
        {
            var userId = Context.User.GetUserId();
            var found = Bridges.TryGetValue(userId, out var info);
            return (found, info);
        }
    }
}