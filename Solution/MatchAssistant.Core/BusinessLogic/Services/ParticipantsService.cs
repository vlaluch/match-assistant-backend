using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Services
{
    public class ParticipantsService : IParticipantsService
    {
        private readonly IGameMapper gameMapper;
        private readonly IParticipantMapper participantMapper;

        public ParticipantsService(
            IGameMapper gameMapper,
            IParticipantMapper participantMapper)
        {
            this.gameMapper = gameMapper;
            this.participantMapper = participantMapper;
        }

        public void CreateNewGame(string title)
        {
            var newGame = new Game(title, DateTime.Now.AddDays(1));
            gameMapper.AddGame(newGame);
        }

        public IEnumerable<ParticipantsGroup> GetAllParticipantsForGame(string gameTitle)
        {
            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return participantMapper.GetAllParticipants(game.Id);
        }

        public IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle)
        {
            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            const int recentGamesLimit = 3;

            return participantMapper.GetRecentGamesParticipants(gameTitle, game.Id, recentGamesLimit);
        }

        public bool UpdateParticipantsGroupState(string gameTitle, ParticipantsGroup participantsGroup)
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
    }
}
