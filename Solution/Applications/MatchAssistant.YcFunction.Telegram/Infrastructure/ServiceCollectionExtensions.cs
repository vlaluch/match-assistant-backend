using MatchAssistant.Domain;
using MatchAssistant.Domain.Core.Interfaces;
using MatchAssistant.Domain.Handlers;
using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Persistence.Repositories.Ydb;
using MatchAssistant.Persistence.Repositories.Ydb.Repositories;
using MatchAssistant.YcFunction.Telegram.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace MatchAssistant.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {            
            services.AddSingleton<IDriverProvider, ConfigurationSettingsProvider>();
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(ConfigurationSettingsProvider.Token));
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IMessagesProcessor, MessagesProcessor>();

            services.AddTransient<IHandleCommand, GetParticipantsCountCommandHandler>();
            services.AddTransient<IHandleCommand, GetParticipantsListCommandHandler>();
            services.AddTransient<IHandleCommand, NewGameCommandHandler>();
            services.AddTransient<IHandleCommand, PingCommandHandler>();
            services.AddTransient<IHandleCommand, SetParticipantsGroupStateCommandHandler>();

            return services;
        }
    }
}
