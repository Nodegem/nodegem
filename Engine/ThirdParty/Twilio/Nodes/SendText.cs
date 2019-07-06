using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Fields.Graph;
using ThirdParty.Data.Twilio;

namespace Nodester.ThirdParty.Twilio.Nodes
{
    [DefinedNode("Send Text")]
    [NodeNamespace("Third Party.Twilio")]
    public class SendText : Node
    {
        public FlowInput In { get; private set; }

        public FlowOutput Out { get; private set; }

        [FieldAttributes("Account SID")] public ValueInput AccountSID { get; private set; }

        [FieldAttributes("Auth Token")] public ValueInput AuthToken { get; private set; }

        [FieldAttributes("To Phone #")] public ValueInput ToPhoneNumber { get; private set; }

        [FieldAttributes("From Phone #")] public ValueInput FromPhoneNumber { get; private set; }

        public ValueInput Message { get; private set; }

        private readonly ITwilioService _twilioService;

        public SendText(ITwilioService twilioService)
        {
            _twilioService = twilioService;
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), SendTextMessage);
            Out = AddFlowOutput(nameof(Out));

            AccountSID = AddValueInput<string>(nameof(AccountSID));
            AuthToken = AddValueInput<string>(nameof(AuthToken));
            ToPhoneNumber = AddValueInput<string>(nameof(ToPhoneNumber));
            FromPhoneNumber = AddValueInput<string>(nameof(FromPhoneNumber));
            Message = AddValueInput<string>(nameof(Message));
        }

        private IFlowOutputField SendTextMessage(IFlow flow)
        {
            var accountSid = flow.GetValue<string>(AccountSID);
            var authToken = flow.GetValue<string>(AuthToken);
            var toPhoneNumber = flow.GetValue<string>(ToPhoneNumber);
            var fromPhoneNumber = flow.GetValue<string>(FromPhoneNumber);
            var message = flow.GetValue<string>(Message);
            _twilioService.SendSMSAsync(accountSid, authToken, toPhoneNumber, fromPhoneNumber, message);
            return Out;
        }
    }
}