using MatchAssistant.Domain.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace MatchAssistant.Domain.Contracts.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetLatestGameByTitleAsync(string title);

        Task<Game> FindGameByTitleAndDateAsync(string title, DateTime date);

        Task AddGameAsync(Game game);
    }
}
