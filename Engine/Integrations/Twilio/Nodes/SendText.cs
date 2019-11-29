using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Twilio;

namespace Nodegem.Engine.Integrations.Twilio.Nodes
{
    [DefinedNode("C146334B-C5F0-4DD6-91C1-682D59D326E3")]
    [NodeNamespace("Integrations.Twilio")]
    public class SendText : TwilioNode
    {
        public IFlowInputField In { get; private set; }

        public IFlowOutputField Out { get; private set; }

        [FieldAttributes("Account SID", ValueType.Text)]
        public IValueInputField AccountSid { get; private set; }

        [FieldAttributes("Auth Token", ValueType.Text)]
        public IValueInputField AuthToken { get; private set; }

        [FieldAttributes("To Phone #", ValueType.PhoneNumber)]
        public IValueInputField ToPhoneNumber { get; private set; }

        [FieldAttributes("From Phone #", ValueType.PhoneNumber)]
        public IValueInputField FromPhoneNumber { get; private set; }

        [FieldAttributes(ValueType.TextArea)] public IValueInputField Message { get; private set; }

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
            var accountSid = await flow.GetValueAsync<string>(AccountSid);
            var authToken = await flow.GetValueAsync<string>(AuthToken);
            var toPhoneNumber = await flow.GetValueAsync<string>(ToPhoneNumber);
            var fromPhoneNumber = await flow.GetValueAsync<string>(FromPhoneNumber);
            var message = await flow.GetValueAsync<string>(Message);
            await TwilioService.SendSMSAsync(accountSid, authToken, toPhoneNumber, fromPhoneNumber, message);
            return Out;
        }
    }
}