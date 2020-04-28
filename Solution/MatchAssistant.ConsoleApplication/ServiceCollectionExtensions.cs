using MatchAssistant.ConsoleApplication.Interfaces;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MatchAssistant.ConsoleApplication
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConsoleAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionStringProvider, ApplicationSettingsManager>();
            services.AddSingleton<IBotSettingsProvider, ApplicationSettingsManager>();
            services.AddSingleton<IProxySettingsProvider, ApplicationSettingsManager>();
            services.AddSingleton<IBotClient, BotClient>();
        }
    }
}
