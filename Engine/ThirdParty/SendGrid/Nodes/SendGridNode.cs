using Nodester.Graph.Core;
using ThirdParty.Data.SendGrid;

namespace Nodester.ThirdParty.SendGrid.Nodes
{
    public abstract class SendGridNode : Node
    {
        protected readonly ISendGridService SendGridService;

        protected SendGridNode(ISendGridService sendGridService)
        {
            SendGridService = sendGridService;
        }
    }
}