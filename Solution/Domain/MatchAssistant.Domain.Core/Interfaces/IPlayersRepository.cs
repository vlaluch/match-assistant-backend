using MatchAssistant.Domain.Contracts.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Domain.Contracts.Interfaces
{
    public interface IPlayersRepository
    {
        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
