using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;
using MatchAssistant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Handlers
{
    public class GetParticipantsCountCommandHandler : IHandleCommand
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IGameRepository gameRepository;

        public CommandType CommandType => CommandType.Count;

        public GetParticipantsCountCommandHandler(IParticipantRepository participantRepository, IGameRepository gameRepository)
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
            var game = await gameRepository.GetLatestGameByTitleAsync(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return await participantRepository.GetAllParticipantsAsync(game.Id);
        }
    }
}
