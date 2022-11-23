using MatchAssistant.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IParticipantRepository
    {
        Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(int gameId);

        Task<ParticipantsGroup> GetParticipantByNameAsync(int gameId, string participantName);

        Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, int latestGameId, int recentGamesLimit);

        Task AddParticipantAsync(int gameId, ParticipantsGroup participantsGroup);

        Task UpdateParticipantAsync(int gameId, ParticipantsGroup participantsGroup);
    }
}
