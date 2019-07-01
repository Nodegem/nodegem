using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nodester.Common.Extensions;

namespace Nodester.Hubs
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
        
    }
}