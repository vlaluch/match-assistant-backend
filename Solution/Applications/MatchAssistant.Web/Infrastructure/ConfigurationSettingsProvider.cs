using MatchAssistant.Persistence.Repositories.MySql;

namespace MatchAssistant.Web.Infrastructure
{
    public class ConfigurationSettingsProvider : IDbConnectionStringProvider
    {
        public string ConnectionString => Environment.GetEnvironmentVariable("DefaultConnection");

        public string Token => Environment.GetEnvironmentVariable("BotToken");
    }
}
