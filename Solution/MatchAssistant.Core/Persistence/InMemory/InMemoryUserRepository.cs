﻿using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task AddToChatAsync(long chatId, int userId)
        {
            if (!chatUsers.ContainsKey(chatId))
            {
                chatUsers.Add(chatId, new List<int> { userId });
            }
            else
            {
                chatUsers[chatId].Add(userId);
            }
            return Task.CompletedTask;
        }

        public Task CreateAsync(ChatUser user)
        {
            if (!users.ContainsKey(user.Id))
            {
                users.Add(user.Id, user);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId)
        {
            if (!chatUsers.ContainsKey(chatId))
            {
                return Task.FromResult(Enumerable.Empty<ChatUser>());
            }

            var usersIds = chatUsers[chatId];
            return Task.FromResult(usersIds.Select(userId => users[userId]));
        }
    }
}