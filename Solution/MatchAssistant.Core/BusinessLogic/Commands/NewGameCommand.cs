using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public class NewGameCommand : BaseCommand
    {
        public NewGameCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService)
            : base(commandMessage, participantsService)
        { }

        public override string Execute()
        {
            ParticipantsService.CreateNewGame(Message.Chat.Name);
            return string.Empty;
        }
    }
}
