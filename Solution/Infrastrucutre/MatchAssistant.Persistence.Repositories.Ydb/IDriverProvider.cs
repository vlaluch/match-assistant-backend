using System.Threading.Tasks;
using Ydb.Sdk;

namespace MatchAssistant.Persistence.Repositories.Ydb
{
    public interface IDriverProvider
    {
        Task<Driver> GetDriverAsync();
    }
}
