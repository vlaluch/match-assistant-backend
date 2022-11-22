using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Tests.Infrastructure.Mappers
{
    public class InMemoryParticipantMapper : IParticipantRepository
    {
        private readonly Dictionary<int, List<ParticipantsGroup>> participants;

        public InMemoryParticipantMapper()
        {
            participants = new Dictionary<int, List<ParticipantsGroup>>();
        }

        public void AddParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            if (!participants.ContainsKey(gameId))
            {
                participants.Add(gameId, new List<ParticipantsGroup> { participantsGroup });
            }
            else
            {
                participants[gameId].Add(participantsGroup);
            }
        }

        public IEnumerable<ParticipantsGroup> GetAllParticipants(int gameId)
        {
            if (!participants.ContainsKey(gameId))
            {
                return Enumerable.Empty<ParticipantsGroup>();
            }

            return participants[gameId];
        }

        public ParticipantsGroup GetParticipantByName(int gameId, string participantName)
        {
            if (!participants.ContainsKey(gameId))
            {
                return null;
            }

            return participants[gameId].FirstOrDefault(x => x.Name == participantName);
        }

        public IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            var participant = participants[gameId].FirstOrDefault(x => x.Name == participantsGroup.Name);
            participant = participantsGroup;
        }
    }
}
