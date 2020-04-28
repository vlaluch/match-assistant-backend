using MatchAssistant.ConsoleApplication.Interfaces;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MatchAssistant.ConsoleApplication
{
    public class ApplicationSettingsManager : IDbConnectionStringProvider, IBotSettingsProvider, IProxySettingsProvider
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    _configuration = builder.Build();
                }

                return _configuration;
            }
        }

        public string Token => Configuration["token"];

        public string ProxyAddress => Configuration["proxySettings:address"];

        public string ProxyPort => Configuration["proxySettings:port"];

        public string ConnectionString => Configuration.GetConnectionString("DefaultConnection");
    }
}