﻿using MatchAssistant.Domain;
using MatchAssistant.Domain.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Messaging.Telegram
{
    public class TelegramCommandResponseFormatter
    {
        public static string FormatCommandResponse(ChatMessage message, Response response)
        {
            var commandType = MessageParser.GetCommandTypeFromMessage(message);

            switch (commandType)
            {
                case CommandType.Count:
                case CommandType.SetState:
                    var participants = response.Payload as IEnumerable<ParticipantsGroup>;

                    if (participants == null || !participants.Any())
                    {
                        return string.Empty;
                    }

                    return OutputFormatter.FormatCountResponse(participants);
                case CommandType.List:
                    participants = response.Payload as IEnumerable<ParticipantsGroup>;
                    var filters = MessageParser.ParseListFilters(MessageParser.GetCommandArgumentsFromMessage(message));
                    return OutputFormatter.FormatListResponse(participants, filters);
                case CommandType.Ping:
                    var users = response.Payload as IEnumerable<ChatUser>;

                    if (!users.Any())
                    {
                        return "У меня нет кандидатов. Можете попробобвать сами вспомнить еще кого-нибудь";
                    }

                    return string.Join(", ", users.Select(user => $"[{user.Name.Split(' ')[0]}](tg://user?id={user.Id})"));

                default:
                    return string.Empty;
            }
        }
    }
}
