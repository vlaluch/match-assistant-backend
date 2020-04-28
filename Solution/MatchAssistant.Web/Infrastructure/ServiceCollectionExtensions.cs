using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MatchAssistant.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionStringProvider, ConfigurationSettingsProvider>();
            services.AddSingleton<IBotSettingsProvider, ConfigurationSettingsProvider>();
        }
    }
}
