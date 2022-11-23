﻿using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.MySQL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public GameRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public async Task AddGameAsync(Game game)
        {
            var sqlQuery = "INSERT INTO games (Title, Date) VALUES (@Title, @Date)";
            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, game);
        }

        public async Task<Game> FindGameByTitleAndDateAsync(string title, DateTime date)
        {
            var sqlQuery = "SELECT * FROM games WHERE Title = @Title AND Date = @Date";
            var queryParams = new { Title = title, Date = date };
            return await dbConnectionProvider.Connection.QueryFirstOrDefaultAsync<Game>(sqlQuery, queryParams);
        }

        public async Task<Game> GetLatestGameByTitleAsync(string title)
        {
            var sqlQuery = @"
SELECT * 
FROM games 
WHERE Title = @Title
ORDER BY Date DESC
LIMIT 1";

            var queryParams = new { Title = title };
            return await dbConnectionProvider.Connection.QueryFirstOrDefaultAsync<Game>(sqlQuery, queryParams);
        }
    }
}