using MatchAssistant.Domain.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Domain
{
    public static class MessageParser
    {
        private static readonly string[] _increaseCountSymbols = new[] { "+", };
        private static readonly string[] _decreaseCountSymbols = new[] { "-" };
        private static readonly string[] _notSureSymbols = new[] { "+-", "+/-", "+\\-", "±" };

        public static CommandType GetCommandTypeFromMessage(ChatMessage message)
        {

            if (IsSetStateMessage(message.Text))
            {
                return CommandType.SetState;
            }
            if (IsNewGameCommand(message.Text))
            {
                return CommandType.NewGame;
            }
            if (IsCountCommand(message.Text))
            {
                return CommandType.Count;
            }
            if (IsListCommand(message.Text))
            {
                return CommandType.List;
            }
            if (IsPingCommand(message.Text))
            {
                return CommandType.Ping;
            }

            return CommandType.NotCommand;
        }

        public static IEnumerable<string> GetCommandArgumentsFromMessage(ChatMessage message)
        {
            var messageParts = message.Text.TrimStart().Split(' ');
            return messageParts.Where(part => part.StartsWith("-")).Select(part => part.TrimStart('-')).ToArray();
        }

        public static ParticipantsGroup GetParticipantsGroupFromMessage(ChatMessage message)
        {
            var participantsGroup = new ParticipantsGroup();

            var countSymbols = _notSureSymbols.Concat(_increaseCountSymbols).Concat(_decreaseCountSymbols);
            var messageText = message.Text;

            foreach (var symbol in countSymbols)
            {
                messageText = messageText.Replace(symbol, "");
            }

            var messageParts = messageText.TrimStart().Split(' ');

            if (IsSimpleCountMessage(message.Text))
            {
                participantsGroup.Name = message.Author.Name;
                participantsGroup.Count = 1;
                participantsGroup.IsSinglePerson = true;
            }
            else if (char.IsDigit(messageParts[0][0]))
            {
                participantsGroup.Count = int.Parse(messageParts[0]);
                participantsGroup.IsSinglePerson = false;

                if (messageParts.Length > 1)
                {
                    var participantName = messageParts[1];
                    for (var i = 2; i < messageParts.Length; i++)
                    {
                        participantName += " " + messageParts[i];
                    }
                    participantsGroup.Name = participantName;
                }
                else
                {
                    participantsGroup.Name = "вместе с " + message.Author.Name;
                }
            }
            else
            {
                participantsGroup.Name = string.Join(" ", messageParts);
                participantsGroup.Count = 1;
                participantsGroup.IsSinglePerson = false;
            }

            participantsGroup.State = GetStateFromMessage(message.Text);

            return participantsGroup;
        }

        public static PingFilters ParsePingFilters(IEnumerable<string> filterNames)
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

        public static ListFilters ParseListFilters(IEnumerable<string> filterNames)
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

        private static string GetStateFromMessage(string messageText)
        {
            if (IsNotSureMessage(messageText))
            {
                return ParticipantState.NotSured;
            }

            if (IsIncreaseCountMessage(messageText))
            {
                return ParticipantState.Accepted;
            }

            if (IsDecreaseCountMessage(messageText))
            {
                return ParticipantState.Declined;
            }

            return ParticipantState.Unknown;
        }

        private static bool IsSetStateMessage(string messageText)
        {
            return IsNotSureMessage(messageText) || IsIncreaseCountMessage(messageText) || IsDecreaseCountMessage(messageText);
        }

        private static bool IsNotSureMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _notSureSymbols);
        }

        private static bool IsSimpleCountMessage(string messageText)
        {
            return MessageEqualsAnyOfSymbols(messageText, _increaseCountSymbols) ||
                MessageEqualsAnyOfSymbols(messageText, _decreaseCountSymbols) ||
                MessageEqualsAnyOfSymbols(messageText, _notSureSymbols);
        }

        private static bool IsIncreaseCountMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _increaseCountSymbols);
        }

        private static bool IsDecreaseCountMessage(string messageText)
        {
            return MessageStartsWithAnyOfSymbols(messageText, _decreaseCountSymbols);
        }

        private static bool MessageStartsWithAnyOfSymbols(string messageText, string[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (messageText.StartsWith(symbol))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MessageEqualsAnyOfSymbols(string messageText, string[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (messageText.ToLower().Trim() == symbol)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsCountCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/count" || messageText.Trim().ToLower() == "/c";
        }

        private static bool IsListCommand(string messageText)
        {
            var commandText = messageText.Trim().Split(' ')[0];
            return commandText == "/list" || commandText == "/l";
        }

        private static bool IsNewGameCommand(string messageText)
        {
            return messageText.Trim().ToLower() == "/new" || messageText.Trim().ToLower() == "/n" || messageText.Trim().ToLower() == "на завтра";
        }

        private static bool IsPingCommand(string messageText)
        {
            var commandText = messageText.Trim().Split(' ')[0];
            return commandText == "/ping" || commandText == "/p";
        }
    }
}
