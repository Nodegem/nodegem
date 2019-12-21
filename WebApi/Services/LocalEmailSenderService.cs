using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Nodegem.Services.Data;

namespace Nodegem.WebApi.Services
{
    public class LocalEmailSenderService : ISendEmails
    {
        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }

        public Task<SendResponse> SendEmailAsync(IFluentEmail email, string templateName, object data, CancellationToken? token = null)
        {
            return Task.FromResult(new SendResponse());
        }

        public Task<SendResponse> SendEmailAsync(string subject, string toEmail, string templateName, object data, CancellationToken? token = null)
        {
            return Task.FromResult(new SendResponse());
        }

        public Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            return Task.FromResult(new SendResponse());
        }
    }
}