using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IMessagesProcessor
    {
        Response ProcessMessage(ChatMessage message);
    }
}
