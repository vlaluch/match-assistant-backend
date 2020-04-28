using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.BusinessLogic.Interfaces;
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
            IBotSettingsProvider botSettingsProvider,
            IMessagesProcessor messagesProcessor)
        {
            client = new TelegramBotClient(botSettingsProvider.Token);
            this.messagesProcessor = messagesProcessor;

        }

        [HttpPost]
        public async void Post([FromBody]Update update)
        {
            if (update == null)
            {
                return;
            }

            var message = update.Message;

            if (message.Type != MessageType.Text)
            {
                return;
            }

            var messageInfo = message.ToChatMessage();

            var response = messagesProcessor.ProcessMessage(messageInfo);
            if (!string.IsNullOrEmpty(response))
            {
                await client.SendTextMessageAsync(message.Chat.Id, response, ParseMode.Markdown);
            }
        }
    }
}