using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.InMemory
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<int, ChatUser> users;
        private readonly Dictionary<long, List<int>> chatUsers;

        public InMemoryUserRepository()
        {
            users = new Dictionary<int, ChatUser>();
            chatUsers = new Dictionary<long, List<int>>();
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
        }

        public void Create(ChatUser user)
        {
            if (!users.ContainsKey(user.Id))
            {
                users.Add(user.Id, user);
            }            
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
