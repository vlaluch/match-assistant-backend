using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IParticipantsService
    {
        void CreateNewGame(string title);

        IEnumerable<ParticipantsGroup> GetAllParticipantsForGame(string gameTitle);

        bool UpdateParticipantsGroupState(string gameTitle, ParticipantsGroup participantsGroup);

        IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle);
    }
}
