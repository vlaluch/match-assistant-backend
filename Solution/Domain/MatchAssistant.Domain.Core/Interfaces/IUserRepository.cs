using MatchAssistant.Domain.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(ChatUser user);

        Task AddToChatAsync(long chatId, int userId);

        Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId);
    }
}
