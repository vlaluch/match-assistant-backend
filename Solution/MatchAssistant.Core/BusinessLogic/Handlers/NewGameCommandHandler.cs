using MatchAssistant.Core.BusinessLogic.Interfaces;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;

namespace MatchAssistant.Core.BusinessLogic.Handlers
{
    public class NewGameCommandHandler : IHandleCommand
    {
        private readonly IGameRepository gameMapper;

        public CommandType CommandType => CommandType.NewGame;

        public NewGameCommandHandler(IGameRepository gameMapper)
        {
            this.gameMapper = gameMapper;
        }

        public Response Handle(Command command)
        {
            CreateNewGame(command.Message.Chat.Name);
            return new Response();
        }

        private void CreateNewGame(string title)
        {
            var newGame = new Game(title, DateTime.Now.AddDays(1));
            gameMapper.AddGame(newGame);
        }
    }
}
