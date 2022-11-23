using System.Data;

namespace MatchAssistant.Persistence.Repositories.MySql
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}
