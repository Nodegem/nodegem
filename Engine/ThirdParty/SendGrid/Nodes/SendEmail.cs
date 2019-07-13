using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using ThirdParty.Data.SendGrid;

namespace Nodester.ThirdParty.SendGrid.Nodes
{
    [DefinedNode("Send Email")]
    [NodeNamespace("Third Party.SendGrid")]
    public class SendEmail : SendGridNode
    {
        public IFlowInputField In { get; private set; }

        public IFlowOutputField Out { get; private set; }

        [FieldAttributes("SendGrid API Key")] public IValueInputField ApiKey { get; private set; }

        [FieldAttributes("Email To")] public IValueInputField EmailTo { get; private set; }

        [FieldAttributes("Email From")] public IValueInputField EmailFrom { get; private set; }

        public IValueInputField Subject { get; private set; }

        [FieldAttributes("Plain Text Content")]
        public IValueInputField PlainTextContent { get; private set; }

        [FieldAttributes("HTML Content")] public IValueInputField HtmlContent { get; private set; }

        public SendEmail(ISendGridService sendGridService) : base(sendGridService)
        {
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

        private async Task<IFlowOutputField> SendOutEmail(IFlow flow)
        {
            var apiKey = await flow.GetValueAsync<string>(ApiKey);
            var fromEmail = await flow.GetValueAsync<string>(EmailFrom);
            var toEmail = await flow.GetValueAsync<string>(EmailTo);
            var subject = await flow.GetValueAsync<string>(Subject);
            var plainTextContent = await flow.GetValueAsync<string>(PlainTextContent);
            var htmlContent = await flow.GetValueAsync<string>(HtmlContent);
            await SendGridService.SendEmailAsync(apiKey, fromEmail, "", toEmail, "", subject, plainTextContent,
                htmlContent);

            return Out;
        }
    }
}