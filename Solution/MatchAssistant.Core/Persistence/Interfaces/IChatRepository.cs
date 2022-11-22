using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IChatRepository
    {
        GameChat GetChatByName(string name);

        void Create(GameChat chat);
    }
}
