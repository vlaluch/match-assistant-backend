using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IChatsService
    {
        void CreateChat(GameChat chat);

        void CreateUser(ChatUser user);

        IEnumerable<ChatUser> GetChatUsers(long chatId);

        void AddUserToChat(long chatId, int userId);
    }
}
