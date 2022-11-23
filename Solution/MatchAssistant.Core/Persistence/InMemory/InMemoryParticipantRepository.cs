﻿using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.InMemory
{
    public class InMemoryParticipantRepository : IParticipantRepository
    {
        private readonly Dictionary<int, List<ParticipantsGroup>> participants;

        public InMemoryParticipantRepository()
        {
            participants = new Dictionary<int, List<ParticipantsGroup>>();
        }

        public Task AddParticipantAsync(int gameId, ParticipantsGroup participantsGroup)
        {
            if (!participants.ContainsKey(gameId))
            {
                participants.Add(gameId, new List<ParticipantsGroup> { participantsGroup });
            }
            else
            {
                participants[gameId].Add(participantsGroup);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(int gameId)
        {
            if (!participants.ContainsKey(gameId))
            {
                return Task.FromResult(Enumerable.Empty<ParticipantsGroup>());
            }

            return Task.FromResult(participants[gameId].AsEnumerable());
        }

        public Task<ParticipantsGroup> GetParticipantByNameAsync(int gameId, string participantName)
        {
            if (!participants.ContainsKey(gameId))
            {
                return Task.FromResult<ParticipantsGroup>(null);
            }

            return Task.FromResult(participants[gameId].FirstOrDefault(x => x.Name == participantName));
        }

        public Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateParticipantAsync(int gameId, ParticipantsGroup participantsGroup)
        {
            var participant = participants[gameId].FirstOrDefault(x => x.Name == participantsGroup.Name);
            participant = participantsGroup;
            return Task.CompletedTask;
        }
    }
}