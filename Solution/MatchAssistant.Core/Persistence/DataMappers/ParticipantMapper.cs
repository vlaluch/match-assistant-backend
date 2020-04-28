using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.DataMappers
{
    public class ParticipantMapper : IParticipantMapper
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public ParticipantMapper(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<ParticipantsGroup> GetAllParticipants(int gameId)
        {
            var sqlQuery = "SELECT * FROM game_participants WHERE GameId = @GameId";
            var queryParams = new { GameId = gameId };
            return dbConnectionProvider.Connection.Query<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public ParticipantsGroup GetParticipantByName(int gameId, string participantName)
        {
            var sqlQuery = "SELECT * FROM game_participants WHERE GameId = @GameId and Name = @Name";
            var queryParams = new { GameId = gameId, Name = participantName };
            return dbConnectionProvider.Connection.QueryFirstOrDefault<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            var gameIds = GetRecentGamesIds(gameTitle, latestGameId, recentGamesLimit);

            if (!gameIds.Any())
            {
                return Enumerable.Empty<ParticipantsGroup>();
            }

            var sqlQuery = $@"
SELECT participant.* FROM game_participants participant
JOIN games game ON game.Id = participant.GameId
WHERE game.Title = @Title AND game.Id IN @GameIds";
            var queryParams = new { Id = latestGameId, Title = gameTitle, GameIds = gameIds };
            return dbConnectionProvider.Connection.Query<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public void AddParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            var sqlQuery = @"
INSERT INTO game_participants (GameId, Name, StateId, Count) 
VALUES (@GameId, @Name, @StateId, @Count)";

            var queryParams = new
            {
                GameId = gameId,
                participantsGroup.Name,
                StateId = GetStateId(participantsGroup.State),
                participantsGroup.Count
            };

            dbConnectionProvider.Connection.Execute(sqlQuery, queryParams);
        }

        public void UpdateParticipant(int gameId, ParticipantsGroup participantsGroup)
        {
            if(participantsGroup == null)
            {
                throw new ArgumentException($"{nameof(participantsGroup)} is null");
            }

            var sqlQuery = @"
UPDATE game_participants 
SET StateId = @StateId, Count = @Count
WHERE GameId = @GameId AND Name = @Name";

            var queryParams = new
            {
                GameId = gameId,
                participantsGroup.Name,
                StateId = GetStateId(participantsGroup.State),
                participantsGroup.Count
            };

            dbConnectionProvider.Connection.Execute(sqlQuery, queryParams);
        }

        private IEnumerable<int> GetRecentGamesIds(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            var sqlQuery = $@"
SELECT game.Id 
FROM games game
WHERE game.Title = @Title AND game.Id < @Id
ORDER BY game.Date DESC
LIMIT {recentGamesLimit}";

            var queryParams = new { Id = latestGameId, Title = gameTitle };
            return dbConnectionProvider.Connection.Query<int>(sqlQuery, queryParams);
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
