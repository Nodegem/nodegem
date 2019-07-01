using Nodester.Common.Data.Interfaces;
using Nodester.Graph.Core.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode("Log Error")]
    [NodeNamespace("Core.Logging")]
    public class LogError : BaseLog
    {
        public LogError(ILogService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.SendErrorLogAsync(Graph.User, message);
        }
    }
}