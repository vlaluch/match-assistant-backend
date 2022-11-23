﻿using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;
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
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $title AS Utf8;
DECLARE $date AS Datetime;

UPSERT INTO games (title, date) VALUES ($title, $date);";

                var queryParams = new Dictionary<string, YdbValue> {
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
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $title AS Utf8;
DECLARE $date AS Datetime;

SELECT * FROM games WHERE title = $title AND date = $date;
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
            var resultSet = queryResponse.Result.ResultSets[0];

            if (resultSet.Rows.Count == 0)
            {
                return null;
            }

            var row = resultSet.Rows[0];

            return new Game((string)row["title"], (DateTime)row["date"]);
        }

        public async Task<Game> GetLatestGameByTitleAsync(string title)
        {
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $title AS Utf8;

SELECT * 
FROM games 
WHERE title = $title
ORDER BY date DESC
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
            var resultSet = queryResponse.Result.ResultSets[0];

            if (resultSet.Rows.Count == 0)
            {
                return null;
            }

            var row = resultSet.Rows[0];

            return new Game((string)row["title"], (DateTime)row["date"]);
        }
    }
}