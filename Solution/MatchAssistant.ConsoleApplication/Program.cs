using MatchAssistant.ConsoleApplication.Interfaces;
using MatchAssistant.Core;
using MatchAssistant.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MatchAssistant.ConsoleApplication
{
    public static class Program
    {
        private static IBotClient bot;
        private static ServiceProvider serviceProvider;

        public static void Main()
        {
            ConfigureApp();

            bot = serviceProvider.GetService<IBotClient>();

            bot.OnMessageReceived += BotOnMessageReceived;
            bot.OnErrorOccured += BotOnErrorOccured;

            bot.Run();
            WriteMessage("started");

            Console.ReadLine();
            bot.Stop();
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

        private static void BotOnMessageReceived(ChatMessage message)
        {
            WriteMessage($"message received from {message.Chat.Name}");
        }

        private static void BotOnErrorOccured(string errorMessage)
        {
            WriteMessage("oops, error occured");
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy.MM.dd HH:mm} {message}");
        }
    }
}