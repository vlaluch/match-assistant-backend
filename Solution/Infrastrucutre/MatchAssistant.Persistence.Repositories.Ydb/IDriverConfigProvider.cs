using Ydb.Sdk;

namespace MatchAssistant.Persistence.Repositories.Ydb
{
    public interface IDriverConfigProvider
    {
        DriverConfig Config { get; }
    }
}
