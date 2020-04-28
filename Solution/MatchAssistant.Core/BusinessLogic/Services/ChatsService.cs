using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Services
{
    public class ChatsService : IChatsService
    {
        private readonly IChatMapper chatMapper;
        private readonly IUserMapper userMapper;

        public ChatsService(
            IChatMapper chatMapper,
            IUserMapper userMapper)
        {
            this.chatMapper = chatMapper;
            this.userMapper = userMapper;
        }

        public void CreateChat(GameChat chat)
        {
            chatMapper.Create(chat);
        }

        public void CreateUser(ChatUser user)
        {
            userMapper.Create(user);
        }

        public IEnumerable<ChatUser> GetChatUsers(long chatId)
        {
            return userMapper.GetChatUsers(chatId);
        }

        public void AddUserToChat(long chatId, int userId)
        {
            userMapper.AddToChat(chatId, userId);
        }
    }
}
