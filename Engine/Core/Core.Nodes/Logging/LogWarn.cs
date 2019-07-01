using Nodester.Common.Data.Interfaces;
using Nodester.Graph.Core.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode("Log Warn")]
    [NodeNamespace("Core.Logging")]
    public class LogWarn : BaseLog
    {
        public LogWarn(ILogService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.SendWarnLogAsync(Graph.User, message);
        }
    }
}