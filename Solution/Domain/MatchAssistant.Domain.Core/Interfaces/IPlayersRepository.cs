using MatchAssistant.Domain.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IPlayersRepository
    {
        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
