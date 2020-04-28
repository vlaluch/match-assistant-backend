using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public class SetParticipantsGroupStateCommand : GetParticipantsCountCommand
    {
        public SetParticipantsGroupStateCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService)
            : base(commandMessage, participantsService)
        { }

        public override string Execute()
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(Message);
            var hasUpdates = ParticipantsService.UpdateParticipantsGroupState(Message.Chat.Name, participantsGroup);

            if (hasUpdates)
            {
                return base.Execute();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
