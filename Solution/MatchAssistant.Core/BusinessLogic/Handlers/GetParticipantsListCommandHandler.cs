using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Handlers
{
    public class GetParticipantsListCommandHandler : IHandleCommand
    {
        private readonly IParticipantRepository participantMapper;
        private readonly IGameRepository gameMapper;

        public CommandType CommandType => CommandType.List;

        public GetParticipantsListCommandHandler(IParticipantRepository participantMapper, IGameRepository gameMapper)
        {
            this.participantMapper = participantMapper;
            this.gameMapper = gameMapper;
        }

        public Response Handle(Command command)
        {
            var participants = GetAllParticipantsForGame(command.Message.Chat.Name);
            return new Response(participants);
        }

        private IEnumerable<ParticipantsGroup> GetAllParticipantsForGame(string gameTitle)
        {
            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return participantMapper.GetAllParticipants(game.Id);
        }
    }
}
