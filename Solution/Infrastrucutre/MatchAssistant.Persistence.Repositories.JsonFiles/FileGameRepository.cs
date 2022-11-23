using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;

namespace MatchAssistant.Persistence.Repositories.JsonFiles
{
    public class FileGameRepository : IGameRepository
    {
        private readonly List<Game> games;
        private readonly JsonStorage<List<Game>> storage;

        public FileGameRepository(JsonStorage<List<Game>> storage)
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
