using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;
using MatchAssistant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Handlers
{
    public class SetParticipantsGroupStateCommandHandler : IHandleCommand
    {
        private readonly IGameRepository gameRepository;
        private readonly IParticipantRepository participantRepository;

        public CommandType CommandType => CommandType.SetState;

        public SetParticipantsGroupStateCommandHandler(IGameRepository gameRepository, IParticipantRepository participantRepository)
        {
            this.gameRepository = gameRepository;
            this.participantRepository = participantRepository;
        }

        public async Task<Response> HandleAsync(Command command)
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(command.Message);
            var hasUpdates = await UpdateParticipantsGroupStateAsync(command.Message.Chat.Name, participantsGroup);

            if (!hasUpdates) return new Response();

            var participants = await GetAllParticipantsForGameAsync(command.Message.Chat.Name);
            return new Response(participants);
        }

        private async Task<bool> UpdateParticipantsGroupStateAsync(string gameTitle, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            var game = await gameRepository.GetLatestGameByTitleAsync(gameTitle);

            if (game == null)
            {
                return false;
            }

            var existingGroup = await participantRepository.GetParticipantByNameAsync(game.Id, participantsGroup.Name);

            if (existingGroup == null)
            {
                await participantRepository.AddParticipantAsync(game.Id, participantsGroup);
                return participantsGroup.State == ParticipantState.Accepted || participantsGroup.State == ParticipantState.NotSured;
            }
            else
            {
                return await HandleUpdatesAsync(game.Id, existingGroup, participantsGroup);
            }
        }

        private async Task<bool> HandleUpdatesAsync(int gameId, ParticipantsGroup existingGroup, ParticipantsGroup updatedGroup)
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

                await participantRepository.UpdateParticipantAsync(gameId, existingGroup);
                return true;
            }
            else if (updatedGroup.State == ParticipantState.Accepted && updatedGroup.Count > 0 && !updatedGroup.IsSinglePerson)
            {
                existingGroup.Count += updatedGroup.Count;
                await participantRepository.UpdateParticipantAsync(gameId, existingGroup);
                return true;
            }

            return false;
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
