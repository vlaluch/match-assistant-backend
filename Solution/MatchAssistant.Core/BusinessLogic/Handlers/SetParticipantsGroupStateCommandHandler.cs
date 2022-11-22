using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Handlers
{
    public class SetParticipantsGroupStateCommandHandler : IHandleCommand
    {
        private readonly IGameRepository gameMapper;
        private readonly IParticipantRepository participantMapper;

        public CommandType CommandType => CommandType.SetState;

        public SetParticipantsGroupStateCommandHandler(IGameRepository gameMapper, IParticipantRepository participantMapper)
        {
            this.gameMapper = gameMapper;
            this.participantMapper = participantMapper;
        }

        public Response Handle(Command command)
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(command.Message);
            var hasUpdates = UpdateParticipantsGroupState(command.Message.Chat.Name, participantsGroup);

            if (!hasUpdates) return new Response();

            var participants = GetAllParticipantsForGame(command.Message.Chat.Name);
            return new Response(participants);
        }

        private bool UpdateParticipantsGroupState(string gameTitle, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return false;
            }

            var existingGroup = participantMapper.GetParticipantByName(game.Id, participantsGroup.Name);

            if (existingGroup == null)
            {
                participantMapper.AddParticipant(game.Id, participantsGroup);
                return participantsGroup.State == ParticipantState.Accepted || participantsGroup.State == ParticipantState.NotSured;
            }
            else
            {
                return HandleUpdates(game.Id, existingGroup, participantsGroup);
            }
        }

        private bool HandleUpdates(int gameId, ParticipantsGroup existingGroup, ParticipantsGroup updatedGroup)
        {
            if (existingGroup.State != updatedGroup.State)
            {
                if (existingGroup.State == ParticipantState.Accepted && updatedGroup.State == ParticipantState.Declined)
                {
                    if (updatedGroup.Count >= existingGroup.Count)
                    {
                        existingGroup.State = ParticipantState.Declined;
                        existingGroup.Count = 1;
                    }
                    else
                    {
                        existingGroup.Count -= updatedGroup.Count;
                    }
                }
                else
                {
                    existingGroup.State = updatedGroup.State;
                    existingGroup.Count = updatedGroup.Count;
                }

                participantMapper.UpdateParticipant(gameId, existingGroup);
                return true;
            }
            else if (updatedGroup.State == ParticipantState.Accepted && updatedGroup.Count > 0 && !updatedGroup.IsSinglePerson)
            {
                existingGroup.Count += updatedGroup.Count;
                participantMapper.UpdateParticipant(gameId, existingGroup);
                return true;
            }

            return false;
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
