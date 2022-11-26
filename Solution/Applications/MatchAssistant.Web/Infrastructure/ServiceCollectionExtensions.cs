using MatchAssistant.Domain;
using MatchAssistant.Domain.Contracts.Interfaces;
using MatchAssistant.Domain.Handlers;
using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Persistence.Repositories.MySql;
using MatchAssistant.Persistence.Repositories.MySql.Repositories;
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

        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();

            services.AddTransient<IPlayersRepository, PlayersRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
        }

        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<ITeamsGenerator, TeamsGenerator>();
            services.AddTransient<IMessagesProcessor, MessagesProcessor>();

            services.AddTransient<IHandleCommand, GetParticipantsCountCommandHandler>();
            services.AddTransient<IHandleCommand, GetParticipantsListCommandHandler>();
            services.AddTransient<IHandleCommand, NewGameCommandHandler>();
            services.AddTransient<IHandleCommand, PingCommandHandler>();
            services.AddTransient<IHandleCommand, SetParticipantsGroupStateCommandHandler>();
        }
    }
}
