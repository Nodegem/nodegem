using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nodester.Common.Data;
using Nodester.Common.Extensions;

namespace Nodester.Services.Hubs
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

        public async Task LogAsync(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", message, "log");
        }
        
        public async Task DebugLogAsync(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", message, "debug");
        }
        
        public async Task WarnLogAsync(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", message, "warn");
        }
        
        public async Task ErrorLogAsync(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLogAsync", message, "error");
        }
        
    }
}