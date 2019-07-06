using Microsoft.Extensions.DependencyInjection;
using ThirdParty.Data.SendGrid;

namespace Nodester.ThirdParty.SendGrid
{
    public static class ServiceExtensions
    {
        public static void ApplySendGridServices(this IServiceCollection services)
        {
            services.AddTransient<ISendGridService, SendGridService>();
        }
    }
}