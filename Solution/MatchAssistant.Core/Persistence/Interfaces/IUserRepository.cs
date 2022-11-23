using MatchAssistant.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(ChatUser user);

        Task AddToChatAsync(long chatId, int userId);

        Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId);
    }
}
