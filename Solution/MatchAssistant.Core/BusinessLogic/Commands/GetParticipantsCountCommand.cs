using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public class GetParticipantsCountCommand : BaseCommand
    {
        public GetParticipantsCountCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService)
            : base(commandMessage, participantsService)
        { }

        public override string Execute()
        {
            var participants = ParticipantsService.GetAllParticipantsForGame(Message.Chat.Name);
            return OutputFormatter.FormatCountResponse(participants);
        }
    }
}
