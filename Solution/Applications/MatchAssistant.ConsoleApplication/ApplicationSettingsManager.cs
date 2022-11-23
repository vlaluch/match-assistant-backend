using MatchAssistant.Persistence.Repositories.MySql;
using Microsoft.Extensions.Configuration;

namespace MatchAssistant.ConsoleApplication
{
    public class ApplicationSettingsManager : IDbConnectionStringProvider
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

        public static string Token => Configuration["token"];

        public static string ProxyAddress => Configuration["proxySettings:address"];

        public static string ProxyPort => Configuration["proxySettings:port"];

        public string ConnectionString => Configuration.GetConnectionString("DefaultConnection");
    }
}