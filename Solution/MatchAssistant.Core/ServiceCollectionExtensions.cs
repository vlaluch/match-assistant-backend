using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.BusinessLogic.Handlers;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.DataLayer.Interfaces;
using MatchAssistant.Core.Persistence;
using MatchAssistant.Core.Persistence.DataMappers;
using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MatchAssistant.Core
{
    public static class ServiceCollectionExtensions
    {
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
