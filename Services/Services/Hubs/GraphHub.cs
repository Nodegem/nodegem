using System;
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

        public async Task RunGraph(GraphDto graph)
        {
            await Clients.Group(Context.User.GetUserId().ToString()).SendAsync("RemoteExecuteGraph", graph);
        }

        public async Task RunMacro(MacroDto macro, string flowInputFieldKey)
        {
            await Clients.Group(Context.User.GetUserId().ToString())
                .SendAsync("RemoteExecuteMacro", macro, flowInputFieldKey);
        }
    }
}