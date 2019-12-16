using Microsoft.Extensions.DependencyInjection;
using Nodegem.Engine.Integrations.Data.Reddit;

namespace Nodegem.Engine.Integrations.Reddit
{
    public static class ServiceExtensions
    {
        
        public static void ApplyRedditService(this IServiceCollection services)
        {
            services.AddScoped<IRedditService, RedditService>();
        }
        
    }
}