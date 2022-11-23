using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.Disk
{
    public class DiskUserRepository : IUserRepository
    {
        private readonly Dictionary<int, ChatUser> users;
        private readonly Dictionary<long, List<int>> chatUsers;
        private readonly JsonStorage<Dictionary<int, ChatUser>> usersStorage;
        private readonly JsonStorage<Dictionary<long, List<int>>> chatUsersStorage;

        public DiskUserRepository(JsonStorage<Dictionary<int, ChatUser>> usersStorage, JsonStorage<Dictionary<long, List<int>>> chatUsersStorage)
        {
            this.usersStorage = usersStorage;
            this.chatUsersStorage = chatUsersStorage;

            users = new Dictionary<int, ChatUser>();

            var storedUsers = usersStorage.Load();

            if (storedUsers != null)
            {
                foreach (var userInfo in storedUsers)
                {
                    users.Add(userInfo.Key, userInfo.Value);
                }
            }

            chatUsers = new Dictionary<long, List<int>>();

            var storedChatUsers = chatUsersStorage.Load();

            if (storedChatUsers != null)
            {
                foreach (var chatUserInfo in storedChatUsers)
                {
                    chatUsers.Add(chatUserInfo.Key, chatUserInfo.Value);
                }
            }
        }

        public void AddToChat(long chatId, int userId)
        {
            if (!chatUsers.ContainsKey(chatId))
            {
                chatUsers.Add(chatId, new List<int> { userId });
            }
            else
            {
                chatUsers[chatId].Add(userId);
            }

            chatUsersStorage.Save(chatUsers);
        }

        public void Create(ChatUser user)
        {
            if (!users.ContainsKey(user.Id))
            {
                users.Add(user.Id, user);
            }

            usersStorage.Save(users);
        }

        public IEnumerable<ChatUser> GetChatUsers(long chatId)
        {
            if (!chatUsers.ContainsKey(chatId))
            {
                return Enumerable.Empty<ChatUser>();
            }

            var usersIds = chatUsers[chatId];
            return usersIds.Select(userId => users[userId]);
        }
    }
}
