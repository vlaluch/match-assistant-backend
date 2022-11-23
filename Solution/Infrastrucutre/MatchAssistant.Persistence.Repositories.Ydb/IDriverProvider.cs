using Ydb.Sdk;

namespace MatchAssistant.Persistence.Repositories.Ydb
{
    public interface IDriverProvider
    {
        Driver Driver { get; }
    }
}
