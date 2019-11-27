using Nodegem.Engine.Core;
using Nodegem.Engine.Integrations.Data.SendGrid;

namespace Nodegem.Engine.Integrations.SendGrid.Nodes
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