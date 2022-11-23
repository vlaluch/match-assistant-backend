using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IChatRepository
    {
        Task<GameChat> GetChatByNameAsync(string name);

        Task CreateAsync(GameChat chat);
    }
}
