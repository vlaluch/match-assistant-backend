namespace MatchAssistant.Persistence.Repositories.MySql
{
    public interface IDbConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}
