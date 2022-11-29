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
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IDriverProvider driverProvider;

        public ParticipantRepository(IDriverProvider driverProvider)
        {
            this.driverProvider = driverProvider;
        }

        public async Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(string gameId)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $game_id AS Utf8;

SELECT * FROM game_participants WHERE game_id = $game_id;";

                var queryParams = new Dictionary<string, YdbValue> { { "$game_id", YdbValue.MakeUtf8(gameId) } };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;

            return queryResponse.Result.ResultSets[0].Rows.Select(MapToParticipantGroup);
        }

        public async Task<ParticipantsGroup> GetParticipantByNameAsync(string gameId, string participantName)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $game_id AS Utf8;
DECLARE $name AS Utf8;

SELECT * FROM game_participants WHERE game_id = $game_id and participant_name = $name;";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeUtf8(gameId) },
                    { "$name", YdbValue.MakeUtf8(participantName) }
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

            return MapToParticipantGroup(queryResponse.Result.ResultSets[0].Rows[0]);
        }

        public async Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, string latestGameId, int recentGamesLimit)
        {
            var gameIds = await GetRecentGamesIds(gameTitle, latestGameId, recentGamesLimit);

            if (!gameIds.Any())
            {
                return Enumerable.Empty<ParticipantsGroup>();
            }

            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = $@"
DECLARE $title AS Utf8;
DECLARE $game_ids AS List;

SELECT participant.* 
FROM game_participants participant
JOIN games game ON game.game_id = participant.game_id
WHERE participant.state = '{ParticipantState.Accepted}' AND game.title = $title AND game.game_id IN $game_ids;"
                ;

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$title", YdbValue.MakeUtf8(gameTitle) },
                    { "$game_ids", YdbValue.MakeList(gameIds.Select(id => YdbValue.MakeUtf8(id)).ToArray())}
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;

            return queryResponse.Result.ResultSets[0].Rows.Select(MapToParticipantGroup);
        }

        public async Task AddParticipantAsync(string gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $game_id AS Utf8;
DECLARE $name AS Utf8;
DECLARE $state AS Utf8;
DECLARE $count AS Int32;

UPSERT INTO game_participants (game_id, participant_name, state, participant_count) 
VALUES ($game_id, $name, $state, $count);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeUtf8(gameId) },
                    { "$name", YdbValue.MakeUtf8(participantsGroup.Name) },
                    { "$state", YdbValue.MakeUtf8(participantsGroup.State) },
                    { "$count", YdbValue.MakeInt32(participantsGroup.Count) },
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        public async Task UpdateParticipantAsync(string gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $game_id AS Utf8;
DECLARE $name AS Utf8;
DECLARE $state AS Utf8;
DECLARE $count AS Int32;

UPDATE game_participants
SET state = $state, participant_count = $count
WHERE game_id = $game_id AND participant_name = $name;";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeUtf8(gameId) },
                    { "$name", YdbValue.MakeUtf8(participantsGroup.Name) },
                    { "$state", YdbValue.MakeUtf8(participantsGroup.State) },
                    { "$count", YdbValue.MakeInt32(participantsGroup.Count) },
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        private async Task<IEnumerable<string>> GetRecentGamesIds(string gameTitle, string latestGameId, int recentGamesLimit)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @$"
DECLARE $game_id AS Utf8;
DECLARE $title AS Utf8;

SELECT game.game_id 
FROM games game
WHERE game.title = $title AND game.game_id < $game_id
ORDER BY game.game_date DESC
LIMIT {recentGamesLimit};";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeUtf8(latestGameId) },
                    { "$title", YdbValue.MakeUtf8(gameTitle) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;

            return queryResponse.Result.ResultSets[0].Rows.Select(row => (string)row["game_id"]);
        }

        private ParticipantsGroup MapToParticipantGroup(ResultSet.Row row)
        {
            var count = (int?)row["participant_count"];

            return new ParticipantsGroup
            {
                Name = (string)row["participant_name"],
                Count = count.Value,
                State = (string)row["state"]
            };
        }
    }
}
