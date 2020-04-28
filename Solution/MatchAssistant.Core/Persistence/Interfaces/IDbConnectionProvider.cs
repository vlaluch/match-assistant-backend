using System.Data;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}
