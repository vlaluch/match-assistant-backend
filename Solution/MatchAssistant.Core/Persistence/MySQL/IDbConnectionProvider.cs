using System.Data;

namespace MatchAssistant.Core.Persistence.MySQL
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}
