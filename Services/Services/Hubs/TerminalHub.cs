using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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

        public async Task Log(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveLog", message);
        }
        
        public async Task DebugLog(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveDebugLog", message);
        }
        
        public async Task WarnLog(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveWarnLog", message);
        }
        
        public async Task ErrorLog(User user, string message)
        {
            await Clients.Group(user.Id).SendAsync("ReceiveErrorLog", message);
        }
        
    }
}