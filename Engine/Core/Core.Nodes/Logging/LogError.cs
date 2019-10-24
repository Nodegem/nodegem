using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public class LogError : BaseLog
    {
        public LogError(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message, bool sendToClient)
        {
            LogService.ErrorLogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}