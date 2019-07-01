using Microsoft.Extensions.DependencyInjection;
using Nodester.ThirdParty.SendGrid.SendGrid.Data;

namespace Nodester.ThirdParty.SendGrid.SendGrid.Nodes
{
    public static class ServiceExtensions
    {
        public static void ApplySendGridServices(this IServiceCollection services)
        {
            services.AddTransient<ISendGridService, SendGridService>();
        }
    }
}