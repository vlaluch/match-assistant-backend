using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.BusinessLogic.Commands
{
    public class PingCommand : BaseCommand
    {
        private readonly IChatsService chatsService;

        public PingCommand(
            ChatMessage commandMessage,
            IParticipantsService participantsService,
            IChatsService chatsService)
            : base(commandMessage, participantsService)
        {
            this.chatsService = chatsService;
        }

        public override string Execute()
        {
            var filters = ParseFilters(MessageParser.GetCommandArgumentsFromMessage(Message));

            var recentGamesParticipants = ParticipantsService.GetRecentGamesParticipants(Message.Chat.Name);
            var curentGameParticipants = ParticipantsService.GetAllParticipantsForGame(Message.Chat.Name);

            var participantsNames = new List<string>();

            if (filters.Equals(PingFilters.None) || filters.Equals(PingFilters.All) || filters.HasFlag(PingFilters.Recent))
            {
                participantsNames = recentGamesParticipants.Select(participant => participant.Name).Distinct()
                .Except(curentGameParticipants.Select(participant => participant.Name).Distinct()).ToList();
            }

            if (filters.Equals(PingFilters.All) || filters.HasFlag(PingFilters.NotSured))
            {
                participantsNames.AddRange(
                    curentGameParticipants
                    .Where(participant => participant.State == ParticipantState.NotSured)
                    .Select(participant => participant.Name).Distinct());
            }

            var usersMap = (chatsService.GetChatUsers(Message.Chat.Id)).ToDictionary(user => user.Name);

            var selectedUsers = usersMap.Where(user => participantsNames.Contains(user.Key)).Select(user => user.Value).ToArray();

            if (!selectedUsers.Any())
            {
                return "У меня нет кандидатов. Можете попробобвать сами вспомнить еще кого-нибудь";
            }

            return string.Join(", ", selectedUsers.Select(user => $"[{user.Name.Split(' ')[0]}](tg://user?id={user.Id})"));
        }

        private PingFilters ParseFilters(IEnumerable<string> filterNames)
        {
            var filters = PingFilters.None;

            foreach (var filterName in filterNames)
            {
                var trimmedName = filterName.ToLower().Trim();

                if (trimmedName == "all" || trimmedName == "a")
                {
                    filters |= PingFilters.All;
                }
                else if (trimmedName == "recent" || trimmedName == "r")
                {
                    filters |= PingFilters.Recent;
                }
                else if (trimmedName == "notsured" || trimmedName == "ns")
                {
                    filters |= PingFilters.NotSured;
                }
            }

            return filters;
        }
    }
}
