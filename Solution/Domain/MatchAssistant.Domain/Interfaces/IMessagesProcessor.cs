using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Interfaces
{
    public interface IMessagesProcessor
    {
        Task<Response> ProcessMessageAsync(ChatMessage message);
    }
}
