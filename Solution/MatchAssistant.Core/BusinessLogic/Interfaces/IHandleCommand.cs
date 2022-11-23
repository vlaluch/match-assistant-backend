using MatchAssistant.Core.Entities;
using System.Threading.Tasks;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IHandleCommand
    {
        CommandType CommandType { get; }
        Task<Response> HandleAsync(Command command);
    }
}
