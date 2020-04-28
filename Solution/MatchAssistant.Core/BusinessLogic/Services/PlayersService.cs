using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.DataLayer.Interfaces;
using MatchAssistant.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.BusinessLogic.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly IPlayersMapper playersMapper;

        public PlayersService(IPlayersMapper playersMapper)
        {
            this.playersMapper = playersMapper;
        }

        public IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names)
        {
            if (!names.Any())
            {
                return Array.Empty<Player>();
            }

            return playersMapper.GetPlayersByNames(names);
        }
    }
}
