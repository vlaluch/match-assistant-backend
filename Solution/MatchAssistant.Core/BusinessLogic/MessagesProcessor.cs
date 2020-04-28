using MatchAssistant.Core.BusinessLogic.Commands;
using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using System;

namespace MatchAssistant.Core.BusinessLogic
{
    public class MessagesProcessor : IMessagesProcessor
    {
        private readonly IParticipantsService participantsService;
        private readonly IChatsService chatsService;

        public MessagesProcessor(
            IParticipantsService participantsService,
            IChatsService chatsService)
        {
            this.participantsService = participantsService;
            this.chatsService = chatsService;
        }

        public string ProcessMessage(ChatMessage message)
        {
            if (message == null)
            {
                return string.Empty;
            }

            chatsService.CreateChat(message.Chat);
            chatsService.CreateUser(message.Author);
            chatsService.AddUserToChat(message.Chat.Id, message.Author.Id);

            var commandType = MessageParser.GetCommandTypeFromMessage(message);

            if (commandType == CommandType.NotCommand)
            {
                return string.Empty;
            }

            var command = CreateCommand(commandType, message);
            return command.Execute();
        }

        private BaseCommand CreateCommand(CommandType commandType, ChatMessage message)
        {
            switch (commandType)
            {
                case CommandType.NewGame:
                    return new NewGameCommand(message, participantsService);
                case CommandType.SetState:
                    return new SetParticipantsGroupStateCommand(message, participantsService);
                case CommandType.Count:
                    return new GetParticipantsCountCommand(message, participantsService);
                case CommandType.List:
                    return new GetParticipantsListCommand(message, participantsService);
                case CommandType.Ping:
                    return new PingCommand(message, participantsService, chatsService);
                default:
                    throw new NotImplementedException("Unexpected command");
            }
        }
    }
}
