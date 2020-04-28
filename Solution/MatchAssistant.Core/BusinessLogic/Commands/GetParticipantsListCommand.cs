using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public class GetParticipantsListCommand : BaseCommand
    {
        public GetParticipantsListCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService)
            : base(commandMessage, participantsService)
        { }

        public override string Execute()
        {
            var participants = ParticipantsService.GetAllParticipantsForGame(Message.Chat.Name);
            var filters = ParseFilters(MessageParser.GetCommandArgumentsFromMessage(Message));
            return OutputFormatter.FormatListResponse(participants, filters);
        }

        private ListFilters ParseFilters(IEnumerable<string> filterNames)
        {
            var filters = ListFilters.None;

            foreach (var filterName in filterNames)
            {
                var trimmedName = filterName.ToLower().Trim();

                if (trimmedName == "all" || trimmedName == "a")
                {
                    filters |= ListFilters.All;
                }
                else if (trimmedName == "accepted" || trimmedName == "ac")
                {
                    filters |= ListFilters.Accepted;
                }
                else if (trimmedName == "declined" || trimmedName == "d")
                {
                    filters |= ListFilters.Declined;
                }
                else if (trimmedName == "notsured" || trimmedName == "ns")
                {
                    filters |= ListFilters.NotSured;
                }
            }

            return filters;
        }
    }
}
