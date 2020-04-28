using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IParticipantMapper
    {
        IEnumerable<ParticipantsGroup> GetAllParticipants(int gameId);

        ParticipantsGroup GetParticipantByName(int gameId, string participantName);

        IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle, int latestGameId, int recentGamesLimit);

        void AddParticipant(int gameId, ParticipantsGroup participantsGroup);

        void UpdateParticipant(int gameId, ParticipantsGroup participantsGroup);
    }
}
