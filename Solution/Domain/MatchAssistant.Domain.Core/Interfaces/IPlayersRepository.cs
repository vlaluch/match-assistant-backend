using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IPlayersRepository
    {
        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
