using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Tests.Infrastructure.Mappers
{
    public class InMemoryGameMapper : IGameMapper
    {
        private readonly List<Game> games;

        public InMemoryGameMapper()
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
