using static MatchAssistant.ConsoleApplication.BotClient;

namespace MatchAssistant.ConsoleApplication.Interfaces
{
    public interface IBotClient
    {
        void Run();
        void Stop();
        event MessageReceivedHandler OnMessageReceived;
        event ErrorOccuredHandler OnErrorOccured;
    }
}
