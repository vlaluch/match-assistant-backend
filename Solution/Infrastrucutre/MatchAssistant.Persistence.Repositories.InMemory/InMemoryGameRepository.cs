using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;

namespace MatchAssistant.Persistence.Repositories.InMemory
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> games;

        public InMemoryGameRepository()
        {
            games = new List<Game>();
        }

        public Task AddGameAsync(Game game)
        {
            games.Add(game);
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
