﻿using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;

namespace MatchAssistant.Persistence.Repositories.JsonFiles
{
    public class FileParticipantRepository : IParticipantRepository
    {
        private Dictionary<string, List<ParticipantsGroup>> participants;
        private readonly JsonStorage<Dictionary<string, List<ParticipantsGroup>>> storage;

        public FileParticipantRepository(JsonStorage<Dictionary<string, List<ParticipantsGroup>>> storage)
        {
            this.storage = storage;

            participants = new Dictionary<string, List<ParticipantsGroup>>();

            var storedParticipants = storage.Load();

            if (storedParticipants != null)
            {
                foreach (var participantInfo in storedParticipants)
                {
                    participants.Add(participantInfo.Key, participantInfo.Value);
                }
            }
        }

        public Task AddParticipantAsync(string gameId, ParticipantsGroup participantsGroup)
        {
            if (!participants.ContainsKey(gameId))
            {
                participants.Add(gameId, new List<ParticipantsGroup> { participantsGroup });
            }
            else
            {
                participants[gameId].Add(participantsGroup);
            }

            storage.Save(participants);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(string gameId)
        {
            if (!participants.ContainsKey(gameId))
            {
                return Task.FromResult(Enumerable.Empty<ParticipantsGroup>());
            }

            return Task.FromResult(participants[gameId].AsEnumerable());
        }

        public Task<ParticipantsGroup> GetParticipantByNameAsync(string gameId, string participantName)
        {
            if (!participants.ContainsKey(gameId))
            {
                return Task.FromResult<ParticipantsGroup>(null);
            }

            return Task.FromResult(participants[gameId].FirstOrDefault(x => x.Name == participantName));
        }

        public Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, string latestGameId, int recentGamesLimit)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateParticipantAsync(string gameId, ParticipantsGroup participantsGroup)
        {
            var participant = participants[gameId].FirstOrDefault(x => x.Name == participantsGroup.Name);
            participant = participantsGroup;

            storage.Save(participants);
            return Task.CompletedTask;
        }
    }
}
