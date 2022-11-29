using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Messaging.Telegram;
using MatchAssistant.Web.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MatchAssistant.YcFunction.Telegram
{
    public class Handler
    {
        private IMessagesProcessor messagesProcessor;
        private ITelegramBotClient client;
        private ServiceProvider serviceProvider;

        public async Task<Response> FunctionHandler(Request request)
        {
            Update update;

            try
            {
                update = JsonConvert.DeserializeObject<Update>(request.body);
            }
            catch
            {
                return new Response(200, "Not a tg message");
            }

            var message = update?.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return new Response(200, "Empty message");
            }

            ConfigureApp();

            messagesProcessor = serviceProvider.GetService<IMessagesProcessor>();
            client = serviceProvider.GetService<ITelegramBotClient>();

            var chatMessage = message.ToChatMessage();

            var response = await messagesProcessor.ProcessMessageAsync(chatMessage);

            var formattedResponse = TelegramCommandResponseFormatter.FormatCommandResponse(chatMessage, response);

            if (!string.IsNullOrEmpty(formattedResponse))
            {
                await client.SendTextMessageAsync(message.Chat.Id, formattedResponse, ParseMode.Markdown);
            }

            return new Response(200, "Success");
        }

        private void ConfigureApp()
        {
            serviceProvider = new ServiceCollection()
                .AddInfrastructureServices()
                .AddDomainServices()
                .BuildServiceProvider();
        }
    }
}