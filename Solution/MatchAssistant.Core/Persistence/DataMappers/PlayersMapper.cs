using Dapper;
using MatchAssistant.Core.DataLayer.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.DataMappers
{
    public class PlayersMapper : IPlayersMapper
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public PlayersMapper(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return dbConnectionProvider.Connection.Query<Player>("SELECT * FROM players");
        }

        public IEnumerable<Player> GetPlayersByNames(IEnumerable<string> names)
        {
            var sqlQuery = "SELECT * FROM players WHERE Name IN @Names";
            var queryParams = new { Names = names.ToArray() };
            return dbConnectionProvider.Connection.Query<Player>(sqlQuery, queryParams);
        }
    }
}
