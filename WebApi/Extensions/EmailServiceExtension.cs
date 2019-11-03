using System.IO;
using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nodester.WebApi.Services;
using Nodester.WebApi.Settings;

namespace Digiop.Shared.Extensions
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
                    x.GetService<ILogger<EmailSenderService>>(), sandboxMode)));
        }
    }
}