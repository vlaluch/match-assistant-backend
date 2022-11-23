using MatchAssistant.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IMessagesProcessor
    {
        Task<Response> ProcessMessageAsync(ChatMessage message);
    }
}
