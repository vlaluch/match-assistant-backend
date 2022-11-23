using MatchAssistant.Domain.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Interfaces
{
    public interface IMessagesProcessor
    {
        Task<Response> ProcessMessageAsync(ChatMessage message);
    }
}
