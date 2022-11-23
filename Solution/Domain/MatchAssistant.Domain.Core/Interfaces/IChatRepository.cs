using MatchAssistant.Domain.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IChatRepository
    {
        Task<GameChat> GetChatByNameAsync(string name);

        Task CreateAsync(GameChat chat);
    }
}
