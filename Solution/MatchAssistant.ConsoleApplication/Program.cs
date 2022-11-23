using MatchAssistant.Core;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.MessagingGateways.Telegram;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MatchAssistant.ConsoleApplication
{
    public static class Program
    {
        private static IMessagesProcessor messagesProcessor;
        private static ITelegramBotClient client;
        private static ServiceProvider serviceProvider;

        public static void Main()
        {
            ConfigureApp();

            messagesProcessor = serviceProvider.GetService<IMessagesProcessor>();

            client = new TelegramBotClient(
                ApplicationSettingsManager.Token,
                CreateClient()
            );

            client.OnMessage += ClientOnMessageReceived;
            client.OnReceiveError += ClientOnReceiveError;

            client.StartReceiving(Array.Empty<UpdateType>());
            WriteMessage("started");

            Console.ReadLine();
            client.StopReceiving();
            WriteMessage("stopped");
        }

        private static void ConfigureApp()
        {
            var services = new ServiceCollection();
            services.AddDataAccessServices();
            services.AddBusinessLogicServices();
            services.AddConsoleAppServices();
            serviceProvider = services.BuildServiceProvider();
        }

        private static void ClientOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            var chatMessage = message.ToChatMessage();

            WriteMessage($"message received from {chatMessage.Chat.Name}");

            try
            {
                var response = messagesProcessor.ProcessMessage(chatMessage);
                var formattedResponse = TelegramCommandResponseFormatter.FormatCommandResponse(chatMessage, response);

                if (!string.IsNullOrEmpty(formattedResponse))
                {
                    client.SendTextMessageAsync(message.Chat.Id, formattedResponse, ParseMode.Markdown);
                }
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        private static HttpClient CreateClient()
        {
            var proxy = new WebProxy($"http://{ApplicationSettingsManager.ProxyAddress}:{ApplicationSettingsManager.ProxyPort}", false, Array.Empty<string>());

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = true
            };

            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }

        private static void ClientOnReceiveError(object sender, ReceiveErrorEventArgs errorEventArgs)
        {
            HandleError(errorEventArgs.ApiRequestException);
        }

        private static void HandleError(Exception exception)
        {
            Console.WriteLine($"{DateTime.Now:yyyy.MM.dd HH:mm} Error! {exception.Message}");
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy.MM.dd HH:mm} {message}");
        }
    }
}