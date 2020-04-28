using Dapper;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;

namespace MatchAssistant.Core.Persistence.DataMappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public UserMapper(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public void Create(ChatUser user)
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

            dbConnectionProvider.Connection.Execute(sqlQuery, queryParams);
        }

        public void AddToChat(long chatId, int userId)
        {
            var sqlQuery = @"
INSERT IGNORE INTO users_chats (UserId, ChatId) 
VALUES (@UserId, @ChatId)";

            var queryParams = new
            {
                ChatId = chatId,
                UserId = userId
            };

            dbConnectionProvider.Connection.Execute(sqlQuery, queryParams);
        }

        public IEnumerable<ChatUser> GetChatUsers(long chatId)
        {
            var sqlQuery = @"
SELECT user.* 
FROM users user
JOIN users_chats userChat ON user.Id = userChat.UserId
WHERE userChat.ChatId = @ChatId";

            var queryParams = new { ChatId = chatId };
            return dbConnectionProvider.Connection.Query<ChatUser>(sqlQuery, queryParams);
        }
    }
}
