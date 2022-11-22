using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.DataLayer.Interfaces
{
    public interface IPlayersRepository
    {
        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
