using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace Nodester.WebApi.Services
{
    public interface ISendEmails
    {
        SendResponse Send(IFluentEmail email, CancellationToken? token = null);

         Task<SendResponse> SendEmailAsync(IFluentEmail email, string templateName, object data,
             CancellationToken? token = null);

        Task<SendResponse> SendEmailAsync(string subject, string toEmail, string templateName, object data,
            CancellationToken? token = null);

        Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null);
    }
}