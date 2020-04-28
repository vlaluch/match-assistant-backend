using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IUserMapper
    {
        void Create(ChatUser user);

        void AddToChat(long chatId, int userId);

        IEnumerable<ChatUser> GetChatUsers(long chatId);
    }
}
