using Dapper;
using MatchAssistant.Core.DataLayer.Interfaces;
using MatchAssistant.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.MySQL.Repositories
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public PlayersRepository(IDbConnectionProvider dbConnectionProvider)
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
