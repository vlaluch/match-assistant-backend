using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.MySQL.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public ParticipantRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsAsync(int gameId)
        {
            var sqlQuery = "SELECT * FROM game_participants WHERE GameId = @GameId";
            var queryParams = new { GameId = gameId };
            return await dbConnectionProvider.Connection.QueryAsync<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public async Task<ParticipantsGroup> GetParticipantByNameAsync(int gameId, string participantName)
        {
            var sqlQuery = "SELECT * FROM game_participants WHERE GameId = @GameId and Name = @Name";
            var queryParams = new { GameId = gameId, Name = participantName };
            return await dbConnectionProvider.Connection.QueryFirstOrDefaultAsync<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public async Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            var gameIds = await GetRecentGamesIds(gameTitle, latestGameId, recentGamesLimit);

            if (!gameIds.Any())
            {
                return Enumerable.Empty<ParticipantsGroup>();
            }

            var sqlQuery = $@"
SELECT participant.* FROM game_participants participant
JOIN games game ON game.Id = participant.GameId
JOIN participant_states state ON state.Id = participant.StateId 
WHERE state.Name = 'Accepted' AND game.Title = @Title AND game.Id IN @GameIds";
            var queryParams = new { Id = latestGameId, Title = gameTitle, GameIds = gameIds };
            return await dbConnectionProvider.Connection.QueryAsync<ParticipantsGroup>(sqlQuery, queryParams);
        }

        public async Task AddParticipantAsync(int gameId, ParticipantsGroup participantsGroup)
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

            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, queryParams);
        }

        public async Task UpdateParticipantAsync(int gameId, ParticipantsGroup participantsGroup)
        {
            if (participantsGroup == null)
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

            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, queryParams);
        }

        private async Task<IEnumerable<int>> GetRecentGamesIds(string gameTitle, int latestGameId, int recentGamesLimit)
        {
            var sqlQuery = $@"
SELECT game.Id 
FROM games game
WHERE game.Title = @Title AND game.Id < @Id
ORDER BY game.Date DESC
LIMIT {recentGamesLimit}";

            var queryParams = new { Id = latestGameId, Title = gameTitle };
            return await dbConnectionProvider.Connection.QueryAsync<int>(sqlQuery, queryParams);
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
