using Microsoft.Extensions.DependencyInjection;
using Nodegem.Engine.Integrations.Data.SendGrid;

namespace Nodegem.Engine.Integrations.SendGrid
{
    public static class ServiceExtensions
    {
        public static void ApplySendGridServices(this IServiceCollection services)
        {
            services.AddTransient<ISendGridService, SendGridService>();
        }
    }
}