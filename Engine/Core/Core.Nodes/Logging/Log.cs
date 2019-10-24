using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data.Attributes;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public class Log : BaseLog
    {
        public Log(ITerminalHubService logService) : base(logService)
        {
        }

        protected override void ExecuteLog(string message, bool sendToClient)
        {
            LogService.LogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}