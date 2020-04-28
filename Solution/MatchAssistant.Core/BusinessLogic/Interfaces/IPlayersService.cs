using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IPlayersService
    {
        IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names);
    }
}
