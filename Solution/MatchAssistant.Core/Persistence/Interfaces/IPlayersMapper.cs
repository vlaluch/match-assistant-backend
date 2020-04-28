using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.DataLayer.Interfaces
{
    public interface IPlayersMapper
    {
        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
