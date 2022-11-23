using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;
using MatchAssistant.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Handlers
{
    public class NewGameCommandHandler : IHandleCommand
    {
        private readonly IGameRepository gameRepository;

        public CommandType CommandType => CommandType.NewGame;

        public NewGameCommandHandler(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<Response> HandleAsync(Command command)
        {
            await CreateNewGameAsync(command.Message.Chat.Name);
            return new Response();
        }

        private async Task CreateNewGameAsync(string title)
        {
            var newGame = new Game(title, DateTime.Now.AddDays(1));
            await gameRepository.AddGameAsync(newGame);
        }
    }
}
