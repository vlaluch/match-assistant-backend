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
    public class UserRepository : IUserRepository
    {
        private readonly IDriverProvider driverProvider;

        public UserRepository(IDriverProvider driverProvider)
        {
            this.driverProvider = driverProvider;
        }

        public async Task CreateAsync(ChatUser user)
        {
            if (user == null)
            {
                throw new ArgumentException($"{nameof(user)} is null");
            }

            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $id AS Int32;
DECLARE $name AS Utf8;
DECLARE $user_name AS Utf8;

UPSERT INTO users (id, name, user_name) VALUES ($id, $name, $user_name);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$id", YdbValue.MakeInt32(user.Id) },
                    { "$name", YdbValue.MakeUtf8(user.Name) },
                    { "$user_name", YdbValue.MakeUtf8(user.UserName) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        public async Task AddToChatAsync(long chatId, int userId)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $user_id AS Int64;
DECLARE $chat_id AS Int64;

UPSERT INTO users_chats (user_id, chat_id) VALUES ($user_id, $chat_id);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$userId", YdbValue.MakeInt64(userId) },
                    { "$chatId", YdbValue.MakeInt64(chatId) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        public async Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId)
        {
            using var driver = await driverProvider.GetDriverAsync();
            using var tableClient = new TableClient(driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $chat_id AS Int64;

SELECT user.* 
FROM users user
JOIN users_chats user_chat ON user.id = user_chat.user_id
WHERE user_chat.chat_id = @$chat_id;";

                var queryParams = new Dictionary<string, YdbValue> { { "$chat_id", YdbValue.MakeInt64(chatId) } };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams

                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (ExecuteDataQueryResponse)response;
            var resultSet = queryResponse.Result.ResultSets[0];

            if (resultSet.Rows.Count == 0)
            {
                return null;
            }

            var row = resultSet.Rows[0];

            return resultSet.Rows.Select(row => new ChatUser
            {
                Id = (int)row["id"],
                Name = (string)row["name"],
                UserName = (string)row["user_name"]
            });
        }        
    }
}
