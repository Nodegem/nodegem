using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;

namespace Nodegem.Services.Hubs
{
    [Authorize]
    public class TerminalHub : Hub
    {
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

        public async Task LogAsync(User user, string graphId, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", graphId, message, "log");
        }

        public async Task WarnLogAsync(User user, string graphId, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", graphId, message, "warn");
        }

        public async Task ErrorLogAsync(User user, string graphId, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", graphId, message, "error");
        }
    }
}