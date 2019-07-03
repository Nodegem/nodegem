using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode]
    [NodeNamespace("Core.Logging")]
    public class Log : BaseLog
    {
        public Log(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.LogAsync(Graph.User, message);
        }
    }
}