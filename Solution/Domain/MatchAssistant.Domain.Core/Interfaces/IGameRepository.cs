using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetLatestGameByTitleAsync(string title);

        Task<Game> FindGameByTitleAndDateAsync(string title, DateTime date);

        Task AddGameAsync(Game game);
    }
}
