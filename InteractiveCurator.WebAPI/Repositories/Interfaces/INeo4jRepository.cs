using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Repositories
{
    public interface INeo4jRepository
    {
        Task AddGameAsync(Game game);
        Task<Game?> GetGameByAppIdAsync(string appId);
        Task<List<Game>> GetRecommendationsForGameAsync(Game game);
    }
}
