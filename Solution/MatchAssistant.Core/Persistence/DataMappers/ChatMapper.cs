using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;

namespace MatchAssistant.Core.Persistence.DataMappers
{
    public class ChatMapper : IChatMapper
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public ChatMapper(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public void Create(GameChat chat)
        {
            if (chat == null)
            {
                throw new ArgumentException($"{nameof(chat)} is null");
            }

            var sqlQuery = @"INSERT IGNORE INTO chats (Id, Name) VALUES (@Id, @Name)";
            var queryParams = new { chat.Id, chat.Name };
            dbConnectionProvider.Connection.Execute(sqlQuery, queryParams);
        }

        public GameChat GetChatByName(string name)
        {
            var sqlQuery = "SELECT * FROM chats WHERE Name = @Name";
            var queryParams = new { Name = name };
            return dbConnectionProvider.Connection.QueryFirstOrDefault<GameChat>(sqlQuery, queryParams);
        }
    }
}
