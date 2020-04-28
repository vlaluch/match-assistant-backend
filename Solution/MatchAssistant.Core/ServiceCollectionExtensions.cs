using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.BusinessLogic.Services;
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
            services.AddTransient<IPlayersMapper, PlayersMapper>();
            services.AddTransient<IGameMapper, GameMapper>();
            services.AddTransient<IChatMapper, ChatMapper>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IParticipantMapper, ParticipantMapper>();
        }

        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IParticipantsService, ParticipantsService>();
            services.AddTransient<IPlayersService, PlayersService>();
            services.AddTransient<IChatsService, ChatsService>();            
            services.AddTransient<ITeamsGenerator, TeamsGenerator>();
            services.AddTransient<IMessagesProcessor, MessagesProcessor>();
        }        
    }
}
