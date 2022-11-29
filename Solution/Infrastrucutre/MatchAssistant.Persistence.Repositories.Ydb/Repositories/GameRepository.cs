using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace MatchAssistant.Persistence.Repositories.Ydb.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IDriverProvider driverProvider;

        public GameRepository(IDriverProvider driverProvider)
        {
            this.driverProvider = driverProvider;
        }

        public async Task AddGameAsync(Game game)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $id AS Utf8;
DECLARE $title AS Utf8;
DECLARE $date AS Datetime;

UPSERT INTO games (game_id, title, game_date) VALUES ($id, $title, $date);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$id", YdbValue.MakeUtf8(Guid.NewGuid().ToString()) },
                    { "$title", YdbValue.MakeUtf8(game.Title) },
                    { "$date", YdbValue.MakeDatetime(game.Date) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        public async Task<Game> FindGameByTitleAndDateAsync(string title, DateTime date)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $title AS Utf8;
DECLARE $date AS Datetime;

SELECT * FROM games WHERE title = $title AND game_date = $date;
    ";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$title", YdbValue.MakeUtf8(title) },
                    { "$date", YdbValue.MakeDatetime(date) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;

            if (!queryResponse.Result.ResultSets.Any() || !queryResponse.Result.ResultSets[0].Rows.Any())
            {
                return null;
            }

            return MapToGame(queryResponse.Result.ResultSets[0].Rows[0]);
        }

        public async Task<Game> GetLatestGameByTitleAsync(string title)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $title AS Utf8;

SELECT * 
FROM games 
WHERE title = $title
ORDER BY game_date DESC
LIMIT 1;";

                var queryParams = new Dictionary<string, YdbValue> { { "$title", YdbValue.MakeUtf8(title) } };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;

            if (!queryResponse.Result.ResultSets.Any() || !queryResponse.Result.ResultSets[0].Rows.Any())
            {
                return null;
            }

            return MapToGame(queryResponse.Result.ResultSets[0].Rows[0]);
        }

        private Game MapToGame(ResultSet.Row row)
        {
            var gameDate = (DateTime?)row["game_date"];

            return new Game((string)row["game_id"], (string)row["title"], gameDate.Value);
        }
    }
}
