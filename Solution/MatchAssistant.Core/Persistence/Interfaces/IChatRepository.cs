using MatchAssistant.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IChatRepository
    {
        Task<GameChat> GetChatByNameAsync(string name);

        Task CreateAsync(GameChat chat);
    }
}
