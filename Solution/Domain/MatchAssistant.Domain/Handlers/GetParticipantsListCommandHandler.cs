using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;

namespace MatchAssistant.Domain.Handlers
{
    public class GetParticipantsListCommandHandler : IHandleCommand
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IGameRepository gameRepository;

        public CommandType CommandType => CommandType.List;

        public GetParticipantsListCommandHandler(IParticipantRepository participantRepository, IGameRepository gameRepository)
        {
            this.participantRepository = participantRepository;
            this.gameRepository = gameRepository;
        }

        public async Task<Response> HandleAsync(Command command)
        {
            var participants = await GetAllParticipantsForGameAsync(command.Message.Chat.Name);
            return new Response(participants);
        }

        private async Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsForGameAsync(string gameTitle)
        {
            var game = gameRepository.GetLatestGameByTitleAsync(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return await participantRepository.GetAllParticipantsAsync(game.Id);
        }
    }
}
