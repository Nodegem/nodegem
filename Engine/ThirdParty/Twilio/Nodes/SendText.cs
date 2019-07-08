using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Twilio;

namespace Nodester.ThirdParty.Twilio.Nodes
{
    [DefinedNode("Send Text")]
    [NodeNamespace("Third Party.Twilio")]
    public class SendText : TwilioNode
    {
        public IFlowInputField In { get; private set; }

        public IFlowOutputField Out { get; private set; }

        [FieldAttributes("Account SID")] public IValueInputField AccountSid { get; private set; }

        [FieldAttributes("Auth Token")] public IValueInputField AuthToken { get; private set; }

        [FieldAttributes("To Phone #")] public IValueInputField ToPhoneNumber { get; private set; }

        [FieldAttributes("From Phone #")] public IValueInputField FromPhoneNumber { get; private set; }

        public IValueInputField Message { get; private set; }

        public SendText(ITwilioService twilioService) : base(twilioService)
        {
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), SendTextMessage);
            Out = AddFlowOutput(nameof(Out));

            AccountSid = AddValueInput<string>(nameof(AccountSid));
            AuthToken = AddValueInput<string>(nameof(AuthToken));
            ToPhoneNumber = AddValueInput<string>(nameof(ToPhoneNumber));
            FromPhoneNumber = AddValueInput<string>(nameof(FromPhoneNumber));
            Message = AddValueInput<string>(nameof(Message));
        }

        private async Task<IFlowOutputField> SendTextMessage(IFlow flow)
        {
            var accountSid = flow.GetValue<string>(AccountSid);
            var authToken = flow.GetValue<string>(AuthToken);
            var toPhoneNumber = flow.GetValue<string>(ToPhoneNumber);
            var fromPhoneNumber = flow.GetValue<string>(FromPhoneNumber);
            var message = flow.GetValue<string>(Message);
            await TwilioService.SendSMSAsync(accountSid, authToken, toPhoneNumber, fromPhoneNumber, message);
            return Out;
        }
    }
}