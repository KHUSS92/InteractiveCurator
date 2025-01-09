using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services.Interfaces
{
    public interface IGameService
    {
        Task ImportTopSellingGamesAsync();
        Task<Game> GetOrFetchGameAsync(string appId);
        Task<List<Game>> GetRecommendationsAsync(string appId);
    }
}

