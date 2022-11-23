using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.MessagingGateways.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MatchAssistant.Web.Controllers
{
    [Route("tgChatMessages")]
    public class TelegramChatMessagesController : Controller
    {
        private readonly ITelegramBotClient client;
        private readonly IMessagesProcessor messagesProcessor;

        public TelegramChatMessagesController(
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

            var chatMessage = message.ToChatMessage();

            var response = await messagesProcessor.ProcessMessageAsync(chatMessage);

            var formattedResponse = TelegramCommandResponseFormatter.FormatCommandResponse(chatMessage, response);

            if (!string.IsNullOrEmpty(formattedResponse))
            {
                await client.SendTextMessageAsync(message.Chat.Id, formattedResponse, ParseMode.Markdown);
            }
        }
    }
}