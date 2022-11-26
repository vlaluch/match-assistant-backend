using Dapper;
using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;

namespace MatchAssistant.Persistence.Repositories.MySql.Repositories
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
