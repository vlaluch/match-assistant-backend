namespace MatchAssistant.ConsoleApplication.Interfaces
{
    public interface IProxySettingsProvider
    {
        string ProxyAddress { get; }

        string ProxyPort { get; }
    }
}
