using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nodester.Common.Data;
using Nodester.Hubs;
using Nodester.Services.Data.Hubs;

namespace Nodester.Services.Hubs
{
    public class TerminalHubService : ITerminalHubService
    {
        private readonly IHubContext<TerminalHub> _terminalContext;

        public TerminalHubService(IHubContext<TerminalHub> terminalContext)
        {
            _terminalContext = terminalContext;
        }

        public Task SendLogAsync(User user, string message)
        {
            return _terminalContext.Clients.Group(user.Id).SendAsync("ReceiveLog", message);
        }

        public Task SendDebugLogAsync(User user, string message, bool isDebug)
        {
            return isDebug ? _terminalContext.Clients.Group(user.Id).SendAsync("ReceiveDebugLog", message) : Task.CompletedTask;
        }

        public Task SendErrorLogAsync(User user, string message)
        {
            return _terminalContext.Clients.Group(user.Id).SendAsync("ReceiveErrorLog", message);
        }

        public Task SendWarnLogAsync(User user, string message)
        {
            return _terminalContext.Clients.Group(user.Id).SendAsync("ReceiveWarnLog", message);
        }
    }
}