using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface ITeamsGenerator
    {
        IEnumerable<Player>[] Generate(ICollection<Player> gameParticipants, TeamGenerationAlgorithm algorithm);
    }
}
