using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IChatMapper
    {
        GameChat GetChatByName(string name);

        void Create(GameChat chat);
    }
}
