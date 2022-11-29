using MatchAssistant.Domain.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Contracts.Interfaces
{
    public interface IParticipantRepository
    {
        Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(string gameId);

        Task<ParticipantsGroup> GetParticipantByNameAsync(string gameId, string participantName);

        Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, string latestGameId, int recentGamesLimit);

        Task AddParticipantAsync(string gameId, ParticipantsGroup participantsGroup);

        Task UpdateParticipantAsync(string gameId, ParticipantsGroup participantsGroup);
    }
}
