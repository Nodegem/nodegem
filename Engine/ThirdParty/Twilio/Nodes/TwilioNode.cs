using Nodester.Graph.Core;
using ThirdParty.Data.Twilio;

namespace Nodester.ThirdParty.Twilio.Nodes
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