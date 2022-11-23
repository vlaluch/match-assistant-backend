using Ydb.Sdk;

namespace MatchAssistant.Core.Persistence.YDB
{
    public interface IDriverConfigProvider
    {
        DriverConfig Config { get; }
    }
}
