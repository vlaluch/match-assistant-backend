using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace MatchAssistant.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebInfrastructureServices(this IServiceCollection services)
        {
            var configurationSettingsProvider = new ConfigurationSettingsProvider();

            services.AddSingleton<IDbConnectionStringProvider>(configurationSettingsProvider);

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(configurationSettingsProvider.Token));
        }
    }
}
