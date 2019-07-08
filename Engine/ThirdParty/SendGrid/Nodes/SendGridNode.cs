using Nodester.Graph.Core;
using ThirdParty.Data.SendGrid;

namespace Nodester.ThirdParty.SendGrid.Nodes
{
    public abstract class SendGridNode : Node
    {
        protected readonly ISendGridService SendGridService;

        public SendGridNode(ISendGridService sendGridService)
        {
            SendGridService = sendGridService;
        }
    }
}