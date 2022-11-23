using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace MatchAssistant.Core.Persistence.YDB.Repositories
{
    internal class ParticipantRepository : IParticipantRepository
    {
        private readonly IDriverProvider driverProvider;

        public ParticipantRepository(IDriverProvider driverProvider)
        {
            this.driverProvider = driverProvider;
        }

        public async IEnumerable<ParticipantsGroup> GetAllParticipants(int gameId)
        {
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $game_id AS Int32;

SELECT * FROM game_participants WHERE game_id = $game_id;";

                var queryParams = new Dictionary<string, YdbValue> { { "$gameId", YdbValue.MakeInt32(gameId) } };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;
            var resultSet = queryResponse.Result.ResultSets[0];

            return resultSet.Rows.Select(row => new ParticipantsGroup
            {
                Name = (string)row["name"],
                Count = (int)row["count"],
                StateId = (int)row["state"]
            });
        }

        public async ParticipantsGroup GetParticipantByName(int gameId, string participantName)
        {
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $game_id AS Int32;
DECLARE $name AS Utf8;

SELECT * FROM game_participants WHERE game_id = $game_id and name = $name;";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$gameId", YdbValue.MakeInt32(gameId) },
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
            var resultSet = queryResponse.Result.ResultSets[0];

            var row = resultSet.Rows[0];

            return new ParticipantsGroup
            {
                Name = (string)row["name"],
                Count = (int)row["count"],
                StateId = (int)row["state"]
            };
        }

        public async IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            var gameIds = GetRecentGamesIds(gameTitle, latestGameId, recentGamesLimit);

            if (!gameIds.Any())
            {
                return Enumerable.Empty<ParticipantsGroup>();
            }

            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $title AS Utf8;
DECLARE $game_ids AS List;

SELECT participant.* 
FROM game_participants participant
JOIN games game ON game.id = participant.game_id
JOIN participant_states state ON state.id = participant.state_id 
WHERE state.name = 'Accepted' AND game.title = $title AND game.id IN $game_ids;"
                ;

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$title", YdbValue.MakeUtf8(gameTitle) },
                    { "$game_ids", YdbValue.MakeList(gameIds.Select(id => YdbValue.MakeInt32(id)).ToArray())}
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

            return resultSet.Rows.Select(row => new ParticipantsGroup
            {
                Name = (string)row["name"],
                Count = (int)row["count"],
                StateId = (int)row["state"]
            });
        }

        public async void AddParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $game_id AS Int32;
DECLARE $name AS Utf8;
DECLARE $state_id AS Int32;
DECLARE $count AS Int32;

UPSERT INTO game_participants (gameId, name, stateId, count) 
VALUES ($game_id, $name, $state_id, $count);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeInt32(gameId) },
                    { "$name", YdbValue.MakeUtf8(participantsGroup.Name) },
                    { "$state_id", YdbValue.MakeInt32(GetStateId(participantsGroup.State)) },
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

        public async void UpdateParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $game_id AS Int32;
DECLARE $name AS Utf8;
DECLARE $state_id AS Int32;
DECLARE $count AS Int32;

UPDATE game_participants
SET state_id = $state_id, count = $count
WHERE game_id = $game_id AND name = $name;";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeInt32(gameId) },
                    { "$name", YdbValue.MakeUtf8(participantsGroup.Name) },
                    { "$state_id", YdbValue.MakeInt32(GetStateId(participantsGroup.State)) },
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

        private async IEnumerable<int> GetRecentGamesIds(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @$"
DECLARE $game_id AS Int32;
DECLARE $title AS Utf8;

SELECT game.id 
FROM games game
WHERE game.title = $title AND game.id < $game_id
ORDER BY game.date DESC
LIMIT {recentGamesLimit};";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$game_id", YdbValue.MakeInt32(latestGameId) },
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
            var resultSet = queryResponse.Result.ResultSets[0];

            return resultSet.Rows.Select(row => new ParticipantsGroup
            {
                Name = (string)row["name"],
                Count = (int)row["count"],
                StateId = (int)row["state"]
            });
        }


        private int GetStateId(string state)
        {
            if (state == ParticipantState.Accepted)
            {
                return 1;
            }
            if (state == ParticipantState.Declined)
            {
                return 2;
            }
            if (state == ParticipantState.NotSured)
            {
                return 3;
            }

            return 0;
        }
    }
}
