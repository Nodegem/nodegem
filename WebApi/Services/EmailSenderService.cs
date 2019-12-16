using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Nodegem.Services.Data;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridAttachment = SendGrid.Helpers.Mail.Attachment;

namespace Nodegem.WebApi.Services
{
    public class EmailSenderService : ISendEmails
    {
        private readonly string _templateDirectory;
        private readonly string _sendGridApiKey;
        private readonly bool _sandboxMode;
        private readonly IFluentEmailFactory _emailFactory;
        private readonly ILogger<EmailSenderService> _logger;


        public EmailSenderService(IFluentEmailFactory emailFactory, string templateDirectory, string sendGridApiKey, ILogger<EmailSenderService> logger, bool sandboxMode = false)
        {
            _templateDirectory = templateDirectory;
            _sendGridApiKey = sendGridApiKey;
            _sandboxMode = sandboxMode;
            _emailFactory = emailFactory;
            _logger = logger;
        }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }

        public async Task<SendResponse> SendEmailAsync(IFluentEmail email, string templateName, object data,
            CancellationToken? token = null)
        {
            templateName = !templateName.EndsWith("cshtml") ? $"{templateName}.cshtml" : templateName;
            var combinedPath = Path.Combine(_templateDirectory, templateName);
            if (!File.Exists(combinedPath)) throw new ArgumentException($"{combinedPath} does not exist");
            email = email.UsingTemplateFromFile(combinedPath, data);
            return await SendAsync(email, token);
        }

        public async Task<SendResponse> SendEmailAsync(string subject, string toEmail, string templateName, object data,
            CancellationToken? token = null)
        {
            var email = _emailFactory.Create().To(toEmail).Subject(subject);
            return await SendEmailAsync(email, templateName, data, token);
        }

        public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            var sendGridClient = new SendGridClient(_sendGridApiKey);

            var mailMessage = new SendGridMessage();
            mailMessage.SetSandBoxMode(_sandboxMode);


            mailMessage.SetFrom(ConvertAddress(email.Data.FromAddress));


            if (email.Data.ToAddresses.Any(a => !string.IsNullOrWhiteSpace(a.EmailAddress)))
                mailMessage.AddTos(email.Data.ToAddresses.Select(ConvertAddress).ToList());

            if (email.Data.CcAddresses.Any(a => !string.IsNullOrWhiteSpace(a.EmailAddress)))
                mailMessage.AddCcs(email.Data.CcAddresses.Select(ConvertAddress).ToList());

            if (email.Data.BccAddresses.Any(a => !string.IsNullOrWhiteSpace(a.EmailAddress)))
                mailMessage.AddBccs(email.Data.BccAddresses.Select(ConvertAddress).ToList());

            mailMessage.SetSubject(email.Data.Subject);

            if (email.Data.Headers.Any())
            {
                mailMessage.AddHeaders(email.Data.Headers);
            }

            if (email.Data.IsHtml)
            {
                mailMessage.HtmlContent = email.Data.Body;
            }
            else
            {
                mailMessage.PlainTextContent = email.Data.Body;
            }

            switch (email.Data.Priority)
            {
                case Priority.High:
                    // https://stackoverflow.com/questions/23230250/set-email-priority-with-sendgrid-api
                    mailMessage.AddHeader("Priority", "Urgent");
                    mailMessage.AddHeader("Importance", "High");
                    // https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmail/2bb19f1b-b35e-4966-b1cb-1afd044e83ab
                    mailMessage.AddHeader("X-Priority", "1");
                    mailMessage.AddHeader("X-MSMail-Priority", "High");
                    break;

                case Priority.Normal:
                    // Do not set anything.
                    // Leave default values. It means Normal Priority.
                    break;

                case Priority.Low:
                    // https://stackoverflow.com/questions/23230250/set-email-priority-with-sendgrid-api
                    mailMessage.AddHeader("Priority", "Non-Urgent");
                    mailMessage.AddHeader("Importance", "Low");
                    // https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmail/2bb19f1b-b35e-4966-b1cb-1afd044e83ab
                    mailMessage.AddHeader("X-Priority", "5");
                    mailMessage.AddHeader("X-MSMail-Priority", "Low");
                    break;
            }

            if (!string.IsNullOrEmpty(email.Data.PlaintextAlternativeBody))
            {
                mailMessage.PlainTextContent = email.Data.PlaintextAlternativeBody;
            }

            if (email.Data.Attachments.Any())
            {
                foreach (var attachment in email.Data.Attachments)
                {
                    var sendGridAttachment = await ConvertAttachment(attachment);
                    mailMessage.AddAttachment(sendGridAttachment.Filename, sendGridAttachment.Content,
                        sendGridAttachment.Type, sendGridAttachment.Disposition, sendGridAttachment.ContentId);
                }
            }

            var sendGridResponse = await sendGridClient.SendEmailAsync(mailMessage, token.GetValueOrDefault());

            var sendResponse = new SendResponse();

            if (IsHttpSuccess((int)sendGridResponse.StatusCode)) return sendResponse;

            sendResponse.ErrorMessages.Add($"{sendGridResponse.StatusCode}");
            var messageBodyDictionary = await sendGridResponse.DeserializeResponseBodyAsync(sendGridResponse.Body);

            if (messageBodyDictionary.ContainsKey("errors"))
            {
                var errors = messageBodyDictionary["errors"];

                foreach (var error in errors)
                {
                    sendResponse.ErrorMessages.Add($"{error}");
                }
            }

            return sendResponse;
        }

        private static EmailAddress ConvertAddress(Address address) =>
            new EmailAddress(address.EmailAddress, address.Name);

        private static async Task<SendGridAttachment>
            ConvertAttachment(FluentEmail.Core.Models.Attachment attachment) => new SendGridAttachment
            {
                Content = await GetAttachmentBase64String(attachment.Data),
                Filename = attachment.Filename,
                Type = attachment.ContentType
            };

        private static async Task<string> GetAttachmentBase64String(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private static bool IsHttpSuccess(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }
    }
}