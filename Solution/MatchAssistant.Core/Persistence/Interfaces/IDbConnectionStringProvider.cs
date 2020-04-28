namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IDbConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}
