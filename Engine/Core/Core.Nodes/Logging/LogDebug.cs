using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode("Log Debug")]
    [NodeNamespace("Core.Logging")]
    public class LogDebug : BaseLog
    {
        public LogDebug(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.DebugLogAsync(Graph.User, message, Graph.DebugMode);
        }
    }
}