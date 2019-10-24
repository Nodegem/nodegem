using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public class LogWarn : BaseLog
    {
        public LogWarn(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message, bool sendToClient)
        {
            LogService.WarnLogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}