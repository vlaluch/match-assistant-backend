using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MatchAssistant.ConsoleApplication
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConsoleAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionStringProvider, ApplicationSettingsManager>();
        }
    }
}
