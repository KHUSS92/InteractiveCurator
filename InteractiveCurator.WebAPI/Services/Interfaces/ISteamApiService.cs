using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services
{
    public interface ISteamApiService
    {
        Task<List<Game>> GetTopSellingGamesAsync(int limit);
        Task<Game> GetGameDetailsAsync(string appId);
    }
}
