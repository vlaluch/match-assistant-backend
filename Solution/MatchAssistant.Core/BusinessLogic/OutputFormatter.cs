using MatchAssistant.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.BusinessLogic
{
    public static class OutputFormatter
    {
        public static string FormatCountResponse(IEnumerable<ParticipantsGroup> participants)
        {
            if (participants == null)
            {
                return "0";
            }

            var suredParticipants = participants.Where(x => x.State == ParticipantState.Accepted).ToList();
            var notSuredParticipants = participants.Where(x => x.State == ParticipantState.NotSured).ToList();
            var suredCount = CountParticipants(suredParticipants);
            var notSuredCount = CountParticipants(notSuredParticipants);

            if (suredCount > 0 && notSuredCount > 0)
            {
                return $"{suredCount} и {notSuredCount}±";
            }
            if (suredCount > 0)
            {
                return suredCount.ToString();
            }
            if (notSuredCount > 0)
            {
                return $"{notSuredCount}±";
            }

            return "0";
        }

        public static string FormatListResponse(IEnumerable<ParticipantsGroup> participants, ListFilters filters)
        {
            if (participants == null || participants.Count() == 0)
            {
                return "Пока пусто. Жду ваших плюсов!";
            }

            var output = "";

            var acceptedParticipants = participants.Where(x => x.State == ParticipantState.Accepted).ToList();
            var notSuredParticipants = participants.Where(x => x.State == ParticipantState.NotSured).ToList();
            var declinedParticipants = participants.Where(x => x.State == ParticipantState.Declined).ToList();

            var outFilters = ListFilters.Accepted | ListFilters.NotSured;

            if (!filters.Equals(ListFilters.None))
            {
                outFilters = filters;
            }

            if (outFilters.HasFlag(ListFilters.Accepted) && acceptedParticipants.Count > 0)
            {
                if (notSuredParticipants.Count > 0)
                {
                    output += $"Пойдут:\r\n";
                }

                output += OutputParticipantsGroups(acceptedParticipants);
            }

            if (outFilters.HasFlag(ListFilters.NotSured) && notSuredParticipants.Count > 0)
            {
                output += $"Под вопросом:\r\n";
                output += OutputParticipantsGroups(notSuredParticipants);
            }

            if (outFilters.HasFlag(ListFilters.Declined) && declinedParticipants.Count > 0)
            {
                output += $"Не пойдут:\r\n";
                output += OutputParticipantsGroups(declinedParticipants);
            }

            return output;
        }

        private static string OutputParticipantsGroups(List<ParticipantsGroup> participantsGroups)
        {
            var output = "";
            var counter = 0;

            for (var i = 0; i < participantsGroups.Count; i++)
            {
                if (participantsGroups[i].Count > 1)
                {
                    output += $"{counter + 1}-{counter + participantsGroups[i].Count}. {participantsGroups[i].Name} - {participantsGroups[i].Count}\r\n";
                    counter += participantsGroups[i].Count;
                }
                else
                {
                    output += $"{counter + 1}. {participantsGroups[i].Name}\r\n";
                    counter++;
                }
            }

            return output;
        }

        private static int CountParticipants(IEnumerable<ParticipantsGroup> participantsGroups)
        {
            var count = 0;

            foreach (ParticipantsGroup participantsGroup in participantsGroups)
            {
                count += participantsGroup.Count;
            }

            return count;
        }
    }
}
