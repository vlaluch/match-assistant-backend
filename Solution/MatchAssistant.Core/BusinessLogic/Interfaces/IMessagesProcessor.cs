using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IMessagesProcessor
    {
        string ProcessMessage(ChatMessage message);
    }
}
