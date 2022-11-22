using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Persistence.Interfaces;
using System;

namespace MatchAssistant.Web.Infrastructure
{
    public class ConfigurationSettingsProvider : IDbConnectionStringProvider, IBotSettingsProvider
    {
        public string ConnectionString => Environment.GetEnvironmentVariable("DefaultConnection");

        public string Token => Environment.GetEnvironmentVariable("BotToken");
    }
}
