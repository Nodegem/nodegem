using System.IO;
using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodegem.Data.Settings;
using Nodegem.Services.Data;
using Nodegem.WebApi.Services;

namespace Nodegem.WebApi.Extensions
{
    public static class EmailServiceBuilderExtensions
    {
        public static void AddEmailService(this IServiceCollection serviceCollection,
            MailConfigurationSettings mailSettings, string templateFolderName, bool sandboxMode = false)
        {
            serviceCollection
                .AddFluentEmail(mailSettings.FromEmail, mailSettings.FromName)
                .AddRazorRenderer()
                .AddInHouseEmailSenderService(mailSettings.ApiKey, templateFolderName, sandboxMode);
        }

        private static void AddInHouseEmailSenderService(this FluentEmailServicesBuilder builder,
            string sendGridApiKey, string templateFolderName, bool sandboxMode)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<ISendEmails>(x =>
                new EmailSenderService(x.GetService<IFluentEmailFactory>(),
                    $"{Directory.GetCurrentDirectory()}/{templateFolderName}", sendGridApiKey,
                    x.GetService<IOptions<AppSettings>>(), sandboxMode)));
        }
    }
}