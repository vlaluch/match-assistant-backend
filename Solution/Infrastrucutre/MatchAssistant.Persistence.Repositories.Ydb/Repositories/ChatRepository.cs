using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace MatchAssistant.Persistence.Repositories.Ydb.Repositories
{
    internal class ChatRepository : IChatRepository
    {
        private readonly IDriverProvider driverProvider;

        public ChatRepository(IDriverProvider driverProvider)
        {
            this.driverProvider = driverProvider;
        }

        public async Task CreateAsync(GameChat chat)
        {
            if (chat == null)
            {
                throw new ArgumentException($"{nameof(chat)} is null");
            }

            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {

                var yqlQuery = @"
DECLARE $id AS Int64;
DECLARE $name AS Utf8;

UPSERT INTO chats (id, name) VALUES ($id, $name);";

                var queryParams = new Dictionary<string, YdbValue> {
                    { "$id", YdbValue.MakeInt64(chat.Id) },
                    { "$name", YdbValue.MakeUtf8(chat.Name) }
                };

                return await session.ExecuteDataQuery(
                    query: yqlQuery,
                    txControl: TxControl.BeginSerializableRW().Commit(),
                    parameters: queryParams
                );
            });

            response.Status.EnsureSuccess();
        }

        public async Task<GameChat> GetChatByNameAsync(string name)
        {
            using var tableClient = new TableClient(driverProvider.Driver, new TableClientConfig());

            var response = await tableClient.SessionExec(async session =>
            {
                var yqlQuery = @"
DECLARE $name AS Utf8;

SELECT * FROM chats WHERE name = $name;
    ";

                var queryParams = new Dictionary<string, YdbValue> { { "$id", YdbValue.MakeUtf8(name) } };

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

            return new GameChat
            {
                Id = (long)row["id"],
                Name = (string)row["name"]
            };
        }
    }
}
