using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public class LogDebug : BaseLog
    {
        public LogDebug(ITerminalHubService logService) : base(logService)
        {
        }

        protected override async Task ExecuteLogAsync(string message, bool sendToClient)
        {
            await LogService.DebugLogAsync(Graph.User, Graph.Id.ToString(), message, Graph.DebugMode, sendToClient);
        }
    }
}