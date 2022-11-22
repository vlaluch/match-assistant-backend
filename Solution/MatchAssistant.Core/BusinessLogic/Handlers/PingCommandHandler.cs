using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.BusinessLogic.Handlers
{
    public class PingCommandHandler : IHandleCommand
    {
        private readonly IParticipantRepository participantMapper;
        private readonly IGameRepository gameMapper;
        private readonly IUserRepository userMapper;

        public CommandType CommandType => CommandType.Ping;

        public PingCommandHandler(IParticipantRepository participantMapper, IGameRepository gameMapper, IUserRepository userMapper)
        {
            this.participantMapper = participantMapper;
            this.gameMapper = gameMapper;
            this.userMapper = userMapper;
        }

        public Response Handle(Command command)
        {
            var filters = MessageParser.ParsePingFilters(MessageParser.GetCommandArgumentsFromMessage(command.Message));

            var recentGamesParticipants = GetRecentGamesParticipants(command.Message.Chat.Name);
            var curentGameParticipants = GetAllParticipantsForGame(command.Message.Chat.Name);

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

            var usersMap = GetChatUsers(command.Message.Chat.Id).ToDictionary(user => user.Name);

            var selectedUsers = usersMap.Where(user => participantsNames.Contains(user.Key)).Select(user => user.Value).ToArray();
            return new Response(selectedUsers);
        }

        private IEnumerable<ParticipantsGroup> GetRecentGamesParticipants(string gameTitle)
        {
            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            const int recentGamesLimit = 3;

            return participantMapper.GetRecentGamesParticipants(gameTitle, game.Id, recentGamesLimit);
        }

        private IEnumerable<ParticipantsGroup> GetAllParticipantsForGame(string gameTitle)
        {
            var game = gameMapper.GetLatestGameByTitle(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return participantMapper.GetAllParticipants(game.Id);
        }

        public IEnumerable<ChatUser> GetChatUsers(long chatId)
        {
            return userMapper.GetChatUsers(chatId);
        }
    }
}
