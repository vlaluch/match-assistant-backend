using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;

namespace MatchAssistant.Persistence.Repositories.InMemory
{
    public class InMemoryParticipantRepository : IParticipantRepository
    {
        private readonly Dictionary<string, List<ParticipantsGroup>> participants;

        public InMemoryParticipantRepository()
        {
            participants = new Dictionary<string, List<ParticipantsGroup>>();
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
            return Task.CompletedTask;
        }
    }
}
