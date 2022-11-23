using MatchAssistant.Domain.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Interfaces
{
    public interface IHandleCommand
    {
        CommandType CommandType { get; }
        Task<Response> HandleAsync(Command command);
    }
}
