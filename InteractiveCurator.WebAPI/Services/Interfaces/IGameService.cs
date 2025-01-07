using InteractiveCurator.WebAPI.DTOs;

namespace InteractiveCurator.WebAPI.Services.Interfaces
{
    public interface IGameService
    {
        Task<List<GameDto>> CurateGamesAsync(List<string> games);
        Task<GameDto> GetGameDetailsAsync(string gameId);
    }
}

