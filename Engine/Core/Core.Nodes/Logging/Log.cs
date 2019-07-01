using Nodester.Common.Data.Interfaces;
using Nodester.Graph.Core.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode]
    [NodeNamespace("Core.Logging")]
    public class Log : BaseLog
    {
        public Log(ILogService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message)
        {
            LogService.SendLogAsync(Graph.User, message);
        }
    }
}