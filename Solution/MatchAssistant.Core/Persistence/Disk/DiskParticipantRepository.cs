using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.Disk
{
    public class DiskParticipantRepository : IParticipantRepository
    {
        private Dictionary<int, List<ParticipantsGroup>> participants;
        private readonly JsonStorage<Dictionary<int, List<ParticipantsGroup>>> storage;

        public DiskParticipantRepository()
        {
            participants = new Dictionary<int, List<ParticipantsGroup>>();
        }

        public DiskParticipantRepository(JsonStorage<Dictionary<int, List<ParticipantsGroup>>> storage)
        {
            this.storage = storage;

            participants = new Dictionary<int, List<ParticipantsGroup>>();

            var storedParticipants = storage.Load();

            if (storedParticipants != null)
            {
                foreach(var participantInfo in storedParticipants)
                {
                    participants.Add(participantInfo.Key, participantInfo.Value);
                }
            }
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

            storage.Save(participants);
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

            storage.Save(participants);
        }
    }
}
