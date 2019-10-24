using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public class LogDebug : BaseLog
    {
        public LogDebug(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message, bool sendToClient)
        {
            LogService.DebugLogAsync(Graph.User, Graph.Id.ToString(), message, Graph.DebugMode, sendToClient);
        }
    }
}