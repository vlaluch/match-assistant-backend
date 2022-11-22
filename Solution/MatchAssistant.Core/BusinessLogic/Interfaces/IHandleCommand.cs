using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Interfaces
{
    public interface IHandleCommand
    {
        CommandType CommandType { get; }
        Response Handle(Command command);
    }
}
