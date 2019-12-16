using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Mailgun;

namespace Nodegem.Engine.Integrations.Mailgun.Nodes
{
    [DefinedNode("E77CDB80-8745-4629-BAFF-0AC5FC502DBD")]
    [NodeNamespace("Integrations.Mailgun")]
    public class SendEmail : Node
    {
        
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField RequestUri { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField ApiKey { set; get; }

        [FieldAttributes(ValueType.Text)]
        public IValueInputField To { get; set; }

        [FieldAttributes(ValueType.Text)]
        public IValueInputField From { get; set; }

        [FieldAttributes(ValueType.Text)]
        public IValueInputField Subject { get; set; }
        
        [FieldAttributes(ValueType.TextArea)]
        public IValueInputField Message { get; set; }

        private readonly IMailgunService _mailgunService;
        
        public SendEmail(IMailgunService mailgunService)
        {
            _mailgunService = mailgunService;
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), SendEmailAsync);
            Out = AddFlowOutput(nameof(Out));
            RequestUri = AddValueInput<string>(nameof(RequestUri));
            ApiKey = AddValueInput<string>(nameof(ApiKey));
            To = AddValueInput<string>(nameof(To));
            From = AddValueInput<string>(nameof(From));
            Subject = AddValueInput<string>(nameof(Subject));
            Message = AddValueInput<string>(nameof(Message));
        }

        private async Task<IFlowOutputField> SendEmailAsync(IFlow flow)
        {
            var requestUri = await flow.GetValueAsync<string>(RequestUri);
            var apiKey = await flow.GetValueAsync<string>(ApiKey);
            var to = await flow.GetValueAsync<string>(To);
            var from = await flow.GetValueAsync<string>(From);
            var subject = await flow.GetValueAsync<string>(Subject);
            var message = await flow.GetValueAsync<string>(Message);

            await _mailgunService.SendEmailAsync(apiKey, requestUri, to, from, subject, message);
            return Out;
        }
        
    }
}