using Microsoft.Extensions.DependencyInjection;
using Nodegem.Engine.Integrations.Data.Mailgun;

namespace Nodegem.Engine.Integrations.Mailgun
{
    public static class ServiceExtensions
    {
        public static void ApplyMailgunServices(this IServiceCollection services)
        {
            services.AddTransient<IMailgunService, MailgunService>();
        }
    }
}