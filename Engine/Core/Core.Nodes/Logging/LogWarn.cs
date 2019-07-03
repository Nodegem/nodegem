using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode("Log Warn")]
    [NodeNamespace("Core.Logging")]
    public class LogWarn : BaseLog
    {
        public LogWarn(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.WarnLogAsync(Graph.User, message);
        }
    }
}