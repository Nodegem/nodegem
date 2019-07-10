using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nodester.Common.Extensions;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Services.Hubs
{
    [Authorize]
    public class GraphHub : Hub
    {
        private static ConcurrentDictionary<Guid, BridgeInfo> Bridges { get; } =
            new ConcurrentDictionary<Guid, BridgeInfo>();

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

        public void EstablishBridge(string deviceName)
        {
            var userId = Context.User.GetUserId();
            _logger.LogInformation($"Establishing bridge. Device: {deviceName}");
            var bridgeInfo = new BridgeInfo
            {
                ConnectionId = Context.ConnectionId,
                DeviceName = deviceName,
                UserId = userId
            };
            Bridges.TryAdd(userId, bridgeInfo);
        }
        
        public async Task RemoveBridge()
        {
            var userId = Context.User.GetUserId();
            var removed = Bridges.TryRemove(userId, out var info);
            if (!removed) return;
            _logger.LogInformation($"Removing bridge. Device: {info.DeviceName}");
            await Clients.Group(userId.ToString()).SendAsync("LostBridge");
        }

        public async Task IsBridgeEstablished()
        {
            var userId = Context.User.GetUserId();
            var found = Bridges.TryGetValue(Context.User.GetUserId(), out var info);
            if (found)
            {
                await Clients.Group(userId.ToString()).SendAsync("BridgeInfo", info);
            }
        }

        public async Task RunGraphAsync(GraphDto graph)
        {
            _logger.LogInformation($"User (Id: {graph.UserId}) executed graph (Id: {graph.Id}, Name: {graph.Name})");
            await Clients.Group(Context.User.GetUserId().ToString()).SendAsync("RemoteExecuteGraph", graph);
        }

        public async Task RunMacroAsync(MacroDto macro, string flowInputFieldKey)
        {
            _logger.LogInformation(
                $"User (Id: {macro.UserId}) executed macro (Id: {macro.Id}, Name: {macro.Name}) w/ input key {flowInputFieldKey}");
            await Clients.Group(Context.User.GetUserId().ToString())
                .SendAsync("RemoteExecuteMacro", macro, flowInputFieldKey);
        }

        private struct BridgeInfo
        {
            public string DeviceName { get; set; }
            public string ConnectionId { get; set; }
            public Guid UserId { get; set; }
        }
    }
}