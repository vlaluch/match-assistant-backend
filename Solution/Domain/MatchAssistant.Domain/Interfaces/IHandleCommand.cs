using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Interfaces
{
    public interface IHandleCommand
    {
        CommandType CommandType { get; }
        Task<Response> HandleAsync(Command command);
    }
}
