using Microsoft.Extensions.DependencyInjection;
using Nodegem.Data.Models;
using Nodegem.Engine.Core;
using Nodegem.Engine.Core.Nodes;
using Nodegem.Engine.Integrations.Discord;
using Nodegem.Engine.Integrations.Reddit;
using Nodegem.Engine.Integrations.SendGrid;
using Nodegem.Engine.Integrations.Twilio;
using Nodegem.Services.Data;
using Nodegem.Services.Data.Repositories;
using Nodegem.Services.Repositories;

namespace Nodegem.Services
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Graph>, Repository<Graph>>();
            services.AddTransient<IMacroRepository, MacroRepository>();
            services.AddTransient<IGraphRepository, GraphRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();

            services.RegisterCommonServices();
        }

        public static void AddServicesForBridge(this IServiceCollection services)
        {
            services.AddHttpClient<INodeHttpClient, NodeHttpClient>();
            services.RegisterCommonServices();
        }

        private static void RegisterCommonServices(this IServiceCollection services)
        {
            services.RegisterMySelf();
            services.ApplyNodeServices();
            services.ApplyTwilioServices();
            services.ApplySendGridServices();
            services.ApplyDiscordServices();
            services.ApplyRedditService();
        }
    }
}