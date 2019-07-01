using System.Threading.Tasks;
using Nodester.ThirdParty.SendGrid.SendGrid.Data;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nodester.ThirdParty.SendGrid.SendGrid.Nodes
{
    public class SendGridService : ISendGridService
    {
        public async Task SendEmailAsync(string apiKey, string fromEmail, string fromName, string toEmail,
            string toName,
            string subject,
            string plainTextContent, string htmlContent)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };
            msg.AddTo(new EmailAddress(toEmail, toName));
            await new SendGridClient(apiKey).SendEmailAsync(msg);
        }
    }
}