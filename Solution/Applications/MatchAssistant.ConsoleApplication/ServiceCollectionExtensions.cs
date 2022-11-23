using MatchAssistant.Domain;
using MatchAssistant.Domain.Core.Interfaces;
using MatchAssistant.Domain.Handlers;
using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Persistence.Repositories.MySql;
using MatchAssistant.Persistence.Repositories.MySql.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MatchAssistant.ConsoleApplication
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConsoleAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionStringProvider, ApplicationSettingsManager>();
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
