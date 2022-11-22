using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.BusinessLogic
{
    public class MessagesProcessor : IMessagesProcessor
    {
        private readonly IEnumerable<IHandleCommand> handlers;

        public MessagesProcessor(IEnumerable<IHandleCommand> handlers)
        {
            this.handlers = handlers;
        }

        public Response ProcessMessage(ChatMessage message)
        {
            if (message == null)
                return new Response();

            //chatsService.CreateChat(message.Chat);
            //chatsService.CreateUser(message.Author);
            //chatsService.AddUserToChat(message.Chat.Id, message.Author.Id);

            var commandType = MessageParser.GetCommandTypeFromMessage(message);

            if (commandType == CommandType.NotCommand)
                return new Response();

            var command = new Command(message, commandType);

            var handler = handlers.FirstOrDefault(h => h.CommandType == commandType);

            if (handler == null)
                throw new NotImplementedException("Unexpected command");

            return handler.Handle(command);
        }
    }
}
