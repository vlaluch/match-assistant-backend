using MatchAssistant.Domain.Contracts.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Contracts.Interfaces
{
    public interface IChatRepository
    {
        Task<GameChat> GetChatByNameAsync(string name);

        Task CreateAsync(GameChat chat);
    }
}
