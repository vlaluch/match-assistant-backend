using Ydb.Sdk;

namespace MatchAssistant.Core.Persistence.YDB
{
    public interface IDriverProvider
    {
        Driver Driver { get; }
    }
}
