using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Persistence.Repositories.JsonFiles
{
    public class FileParticipantRepository : IParticipantRepository
    {
        private Dictionary<int, List<ParticipantsGroup>> participants;
        private readonly JsonStorage<Dictionary<int, List<ParticipantsGroup>>> storage;

        public FileParticipantRepository(JsonStorage<Dictionary<int, List<ParticipantsGroup>>> storage)
        {
            this.storage = storage;

            participants = new Dictionary<int, List<ParticipantsGroup>>();

            var storedParticipants = storage.Load();

            if (storedParticipants != null)
            {
                foreach (var participantInfo in storedParticipants)
                {
                    participants.Add(participantInfo.Key, participantInfo.Value);
                }
            }
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

            storage.Save(participants);
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

            storage.Save(participants);
            return Task.CompletedTask;
        }
    }
}
