using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Interfaces
{
    public interface ITeamsGenerator
    {
        IEnumerable<Player>[] Generate(ICollection<Player> gameParticipants, TeamGenerationAlgorithm algorithm);
    }
}
