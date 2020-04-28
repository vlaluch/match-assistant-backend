using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public abstract class BaseCommand
    {
        protected BaseCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService)
        {
            Message = commandMessage;
            ParticipantsService = participantsService;
        }

        public abstract string Execute();

        protected ChatMessage Message { get; }

        protected IParticipantsService ParticipantsService { get; }
    }
}
