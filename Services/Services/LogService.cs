using System;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Common.Data.Interfaces;
using Nodester.Graph.Core.Data;
using Nodester.Services.Data;
using Nodester.Services.Data.Hubs;

namespace Nodester.Services
{
    public class LogService : ILogService
    {
        private readonly ITerminalHubService _terminalHub;

        public LogService(ITerminalHubService terminalHub)
        {
            _terminalHub = terminalHub;
        }

        public Task SendLogAsync(User user, string message)
        {
            Console.WriteLine($"Log: {message}");
            return _terminalHub.SendLogAsync(user, message);
        }

        public Task SendWarnLogAsync(User user, string message)
        {
            Console.WriteLine($"Warn: {message}");
            return _terminalHub.SendWarnLogAsync(user, message);
        }

        public Task SendErrorLogAsync(User user, string message)
        {
            Console.WriteLine($"Error: {message}");
            return _terminalHub.SendErrorLogAsync(user, message);
        }
    }
}