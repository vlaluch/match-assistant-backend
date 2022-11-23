using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.Disk
{
    public class DiskGameRepository : IGameRepository
    {
        private readonly List<Game> games;
        private readonly JsonStorage<List<Game>> storage;

        public DiskGameRepository(JsonStorage<List<Game>> storage)
        {
            this.storage = storage;

            games = new List<Game>();

            var storedGames = storage.Load();

            if (storedGames != null)
            {
                games.AddRange(storedGames);
            }
        }

        public Task AddGameAsync(Game game)
        {
            games.Add(game);
            storage.Save(games);
            return Task.CompletedTask;
        }

        public Task<Game> FindGameByTitleAndDateAsync(string title, DateTime date)
        {
            return Task.FromResult(games.FirstOrDefault(game => game.Title == title && game.Date == date));
        }

        public Task<Game> GetLatestGameByTitleAsync(string title)
        {
            return Task.FromResult(games.FirstOrDefault(game => game.Title == title));
        }
    }
}
