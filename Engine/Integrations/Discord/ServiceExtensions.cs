using Microsoft.Extensions.DependencyInjection;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord
{
    public static class ServiceExtensions
    {
        
        public static void ApplyDiscordServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscordService, DiscordService>();
        }
        
    }
}