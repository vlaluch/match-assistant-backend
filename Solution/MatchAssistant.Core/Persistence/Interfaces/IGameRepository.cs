using MatchAssistant.Core.Entities;
using System;

namespace MatchAssistant.Core.Persistence.Interfaces
{
    public interface IGameRepository
    {
        Game GetLatestGameByTitle(string title);

        Game FindGameByTitleAndDate(string title, DateTime date);

        void AddGame(Game game);
    }
}
