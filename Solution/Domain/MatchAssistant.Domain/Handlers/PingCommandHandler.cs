using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Contracts.Interfaces;
using MatchAssistant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Handlers
{
    public class PingCommandHandler : IHandleCommand
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;

        public CommandType CommandType => CommandType.Ping;

        public PingCommandHandler(IParticipantRepository participantRepository, IGameRepository gameRepository, IUserRepository userRepository)
        {
            this.participantRepository = participantRepository;
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
        }

        public async Task<Response> HandleAsync(Command command)
        {
            var filters = MessageParser.ParsePingFilters(MessageParser.GetCommandArgumentsFromMessage(command.Message));

            var recentGamesParticipants = await GetRecentGamesParticipantsAsync(command.Message.Chat.Name);
            var curentGameParticipants = await GetAllParticipantsForGameAsync(command.Message.Chat.Name);

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

            var users = await GetChatUsersAsync(command.Message.Chat.Id);
            var usersMap = users.ToDictionary(user => user.Name);

            var selectedUsers = usersMap.Where(user => participantsNames.Contains(user.Key)).Select(user => user.Value).ToArray();
            return new Response(selectedUsers);
        }

        private async Task<IEnumerable<ParticipantsGroup>> GetRecentGamesParticipantsAsync(string gameTitle)
        {
            var game = gameRepository.GetLatestGameByTitleAsync(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            const int recentGamesLimit = 3;

            return await participantRepository.GetRecentGamesParticipantsAsync(gameTitle, game.Id, recentGamesLimit);
        }

        private async Task<IEnumerable<ParticipantsGroup>> GetAllParticipantsForGameAsync(string gameTitle)
        {
            var game = gameRepository.GetLatestGameByTitleAsync(gameTitle);

            if (game == null)
            {
                return Array.Empty<ParticipantsGroup>();
            }

            return await participantRepository.GetAllParticipantsAsync(game.Id);
        }

        public async Task<IEnumerable<ChatUser>> GetChatUsersAsync(long chatId)
        {
            return await userRepository.GetChatUsersAsync(chatId);
        }
    }
}
