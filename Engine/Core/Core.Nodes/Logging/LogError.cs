using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode("Log Error")]
    [NodeNamespace("Core.Logging")]
    public class LogError : BaseLog
    {
        public LogError(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.ErrorLogAsync(Graph.User, message);
        }
    }
}