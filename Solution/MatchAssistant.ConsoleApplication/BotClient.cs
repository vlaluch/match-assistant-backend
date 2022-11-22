using MatchAssistant.ConsoleApplication.Interfaces;
using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using System;
using System.Net;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MatchAssistant.ConsoleApplication
{
    public class BotClient : IBotClient
    {
        private readonly ITelegramBotClient client;
        private readonly IMessagesProcessor messagesProcessor;        
        private readonly IProxySettingsProvider proxySettingsProvider;

        public delegate void MessageReceivedHandler(ChatMessage message);
        public delegate void ErrorOccuredHandler(string errorMessage);

        public event MessageReceivedHandler OnMessageReceived;
        public event ErrorOccuredHandler OnErrorOccured;

        public BotClient(
            IMessagesProcessor messagesProcessor,
            IBotSettingsProvider botSettingsProvider,
            IProxySettingsProvider proxySettingsProvider)
        {
            this.messagesProcessor = messagesProcessor;            
            this.proxySettingsProvider = proxySettingsProvider;

            client = new TelegramBotClient(
                botSettingsProvider.Token,
                CreateClient()
            );

            client.OnMessage += ClientOnMessageReceived;
            client.OnReceiveError += ClientOnReceiveError;
        }

        public void Run()
        {
            client.StartReceiving(Array.Empty<UpdateType>());
        }

        public void Stop()
        {
            client.StopReceiving();
        }

        private HttpClient CreateClient()
        {
            var proxy = new WebProxy($"http://{proxySettingsProvider.ProxyAddress}:{proxySettingsProvider.ProxyPort}", false, Array.Empty<string>());

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = true
            };

            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }

        private void ClientOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            var messageInfo = message.ToChatMessage();

            OnMessageReceived?.Invoke(messageInfo);

            try
            {
                var response = messagesProcessor.ProcessMessage(messageInfo);
                if (!string.IsNullOrEmpty(response))
                {
                    client.SendTextMessageAsync(message.Chat.Id, response, ParseMode.Markdown);
                }
            }
            catch (Exception e)
            {
                OnErrorOccured?.Invoke(e.Message);
            }
        }

        private void ClientOnReceiveError(object sender, ReceiveErrorEventArgs errorEventArgs)
        {
            OnErrorOccured?.Invoke(errorEventArgs.ApiRequestException.Message);
        }
    }
}
