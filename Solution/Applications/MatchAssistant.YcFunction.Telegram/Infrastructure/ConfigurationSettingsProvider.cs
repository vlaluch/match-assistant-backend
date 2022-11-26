using MatchAssistant.Persistence.Repositories.Ydb;
using System;
using System.Threading.Tasks;
using Ydb.Sdk;
using Ydb.Sdk.Yc;

namespace MatchAssistant.YcFunction.Telegram.Infrastructure
{
    public class ConfigurationSettingsProvider : IDriverProvider
    {
        public static string Token => Environment.GetEnvironmentVariable("bot_token");

        public async Task<Driver> GetDriverAsync()
        {
            var metadataProvider = new MetadataProvider();
            await metadataProvider.Initialize();

            var config = new DriverConfig(
                endpoint: Environment.GetEnvironmentVariable("db_endpoint"),
                database: Environment.GetEnvironmentVariable("db_path"),
                credentials: metadataProvider
            );

            var driver = new Driver(config);
            await driver.Initialize();

            return driver;
        }
    }
}
