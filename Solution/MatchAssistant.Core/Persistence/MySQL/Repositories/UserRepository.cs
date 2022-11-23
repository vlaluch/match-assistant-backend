using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.MySQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public UserRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public async Task CreateAsync(ChatUser user)
        {
            if (user == null)
            {
                throw new ArgumentException($"{nameof(user)} is null");
            }

            var sqlQuery = @"
INSERT IGNORE INTO users (Id, Name, UserName) 
VALUES (@Id, @Name, @UserName)";

            var queryParams = new
            {
                user.Name,
                user.Id,
                user.UserName
            };

            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, queryParams);
        }

        public async Task AddToChatAsync(long chatId, int userId)
        {
            var sqlQuery = @"
INSERT IGNORE INTO users_chats (UserId, ChatId) 
VALUES (@UserId, @ChatId)";

            var queryParams = new
            {
                ChatId = chatId,
                UserId = userId
            };

            await dbConnectionProvider.Connection.ExecuteAsync(sqlQuery, queryParams);
        }

        public async Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId)
        {
            var sqlQuery = @"
SELECT user.* 
FROM users user
JOIN users_chats userChat ON user.Id = userChat.UserId
WHERE userChat.ChatId = @ChatId";

            var queryParams = new { ChatId = chatId };
            return await dbConnectionProvider.Connection.QueryAsync<ChatUser>(sqlQuery, queryParams);
        }
    }
}
