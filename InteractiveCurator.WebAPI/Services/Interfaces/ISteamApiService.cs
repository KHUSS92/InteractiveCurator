using InteractiveCurator.WebAPI.DTOs;

namespace InteractiveCurator.WebAPI.Services
{
    public interface ISteamApiService
    {
        Task<GameDto> GetGameDetailsAsync(string gameId);
    }
}
