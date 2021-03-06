﻿using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;

namespace MatchAssistant.Core.Persistence.DataMappers
{
    public class GameMapper : IGameMapper
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public GameMapper(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public void AddGame(Game game)
        {
            var sqlQuery = "INSERT INTO games (Title, Date) VALUES (@Title, @Date)";
            dbConnectionProvider.Connection.Execute(sqlQuery, game);
        }

        public Game FindGameByTitleAndDate(string title, DateTime date)
        {
            var sqlQuery = "SELECT * FROM games WHERE Title = @Title AND Date = @Date";
            var queryParams = new { Title = title, Date = date };
            return dbConnectionProvider.Connection.QueryFirstOrDefault<Game>(sqlQuery, queryParams);
        }

        public Game GetLatestGameByTitle(string title)
        {
            var sqlQuery = @"
SELECT * 
FROM games 
WHERE Title = @Title
ORDER BY Date DESC
LIMIT 1";

            var queryParams = new { Title = title };
            return dbConnectionProvider.Connection.QueryFirstOrDefault<Game>(sqlQuery, queryParams);
        }
    }
}
