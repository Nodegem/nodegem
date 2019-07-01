using Nodester.Graph.Core;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.ThirdParty.SendGrid.SendGrid.Data;

namespace Nodester.ThirdParty.SendGrid.SendGrid.Nodes.Nodes
{
    [DefinedNode("Send Email")]
    [NodeNamespace("Third Party.SendGrid")]
    public class SendEmail : Node
    {
        public FlowInput In { get; private set; }

        public FlowOutput Out { get; private set; }

        [FieldAttributes("SendGrid API Key")] public ValueInput ApiKey { get; private set; }

        [FieldAttributes("Email To")] public ValueInput EmailTo { get; private set; }

        [FieldAttributes("Email From")] public ValueInput EmailFrom { get; private set; }

        public ValueInput Subject { get; private set; }

        [FieldAttributes("Plain Text Content")]
        public ValueInput PlainTextContent { get; private set; }

        [FieldAttributes("HTML Content")] public ValueInput HtmlContent { get; private set; }

        private readonly ISendGridService _sendGridService;

        public SendEmail(ISendGridService sendGridService)
        {
            _sendGridService = sendGridService;
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), SendOutEmail);
            Out = AddFlowOutput(nameof(Out));

            ApiKey = AddValueInput<string>(nameof(ApiKey));
            EmailTo = AddValueInput<string>(nameof(EmailTo));
            EmailFrom = AddValueInput<string>(nameof(EmailFrom));
            Subject = AddValueInput<string>(nameof(Subject));
            PlainTextContent = AddValueInput<string>(nameof(PlainTextContent));
            HtmlContent = AddValueInput<string>(nameof(HtmlContent));
        }

        private IFlowOutputField SendOutEmail(IFlow flow)
        {
            var apiKey = flow.GetValue<string>(ApiKey);
            var fromEmail = flow.GetValue<string>(EmailFrom);
            var toEmail = flow.GetValue<string>(EmailTo);
            var subject = flow.GetValue<string>(Subject);
            var plainTextContent = flow.GetValue<string>(PlainTextContent);
            var htmlContent = flow.GetValue<string>(HtmlContent);
            _sendGridService.SendEmailAsync(apiKey, fromEmail, "", toEmail, "", subject, plainTextContent, htmlContent);

            return Out;
        }
    }
}