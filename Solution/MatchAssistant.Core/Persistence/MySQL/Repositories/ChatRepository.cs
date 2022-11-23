using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.MySQL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public ChatRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public async Task CreateAsync(GameChat chat)
        {
            if (chat == null)
            {
                throw new ArgumentException($"{nameof(chat)} is null");
            }

            var sqlQuery = @"INSERT IGNORE INTO chats (Id, Name) VALUES (@Id, @Name)";
            var queryParams = new { chat.Id, chat.Name };
            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, queryParams);
        }

        public async Task<GameChat> GetChatByNameAsync(string name)
        {
            var sqlQuery = "SELECT * FROM chats WHERE Name = @Name";
            var queryParams = new { Name = name };
            return await dbConnectionProvider.Connection.QueryFirstOrDefaultAsync<GameChat>(sqlQuery, queryParams);
        }
    }
}
