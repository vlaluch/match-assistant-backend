using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MatchAssistant.Web.Controllers
{
    [Route("chatMessages")]
    public class ChatMessagesController : Controller
    {
        private readonly ITelegramBotClient client;
        private readonly IMessagesProcessor messagesProcessor;

        public ChatMessagesController(
            ITelegramBotClient client,
            IMessagesProcessor messagesProcessor)
        {
            this.client = client;
            this.messagesProcessor = messagesProcessor;
        }

        [HttpPost]
        public async void Post([FromBody] Update update)
        {
            var message = update?.Message;

            if (message == null || message.Type != MessageType.Text)
                return;

            var response = messagesProcessor.ProcessMessage(message.ToChatMessage());

            var formattedResponse = TelegramCommandResponseFormatter.FormatCommandResponse(message.ToChatMessage(), response);

            if (!string.IsNullOrEmpty(formattedResponse))
            {
                await client.SendTextMessageAsync(message.Chat.Id, formattedResponse, ParseMode.Markdown);
            }
        }
    }
}