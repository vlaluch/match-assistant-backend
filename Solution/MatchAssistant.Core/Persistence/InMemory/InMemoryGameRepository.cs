using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Persistence.InMemory
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> games;

        public InMemoryGameRepository()
        {
            games = new List<Game>();
        }

        public void AddGame(Game game)
        {
            games.Add(game);
        }

        public Game FindGameByTitleAndDate(string title, DateTime date)
        {
            return games.FirstOrDefault(game => game.Title == title && game.Date == date);
        }

        public Game GetLatestGameByTitle(string title)
        {
            return games.FirstOrDefault(game => game.Title == title);
        }
    }
}
