using Nodegem.Engine.Core;
using Nodegem.Engine.Integrations.Data.Twilio;

namespace Nodegem.Engine.Integrations.Twilio.Nodes
{
    public abstract class TwilioNode : Node
    {
        protected ITwilioService TwilioService { get; }

        public TwilioNode(ITwilioService twilioService)
        {
            TwilioService = twilioService;
        }

    }
}