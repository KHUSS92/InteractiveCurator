using InteractiveCurator.WebAPI.Models;
using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.Services;
using InteractiveCurator.WebAPI.Services.Interfaces;

public class GameService : IGameService
{
    private readonly ISteamApiService _steamApiService;
    private readonly INeo4jRepository _neo4jRepository;

    public GameService(ISteamApiService steamApiService, INeo4jRepository neo4jRepository)
    {
        _steamApiService = steamApiService;
        _neo4jRepository = neo4jRepository;
    }

    public async Task ImportTopSellingGamesAsync()
    {
        var topGames = await _steamApiService.GetTopSellingGamesAsync(1000);

        foreach (var game in topGames)
        {
            await _neo4jRepository.AddGameAsync(game);
        }
    }

    public async Task<Game> GetOrFetchGameAsync(string appId)
    {
        var game = await _neo4jRepository.GetGameByAppIdAsync(appId);

        if (game == null)
        {
            game = await _steamApiService.GetGameDetailsAsync(appId);

            await _neo4jRepository.AddGameAsync(game);
        }

        return game;
    }

    public async Task<List<Game>> GetRecommendationsAsync(string appId)
    {
        var game = await _neo4jRepository.GetGameByAppIdAsync(appId);
        if (game == null)
        {
            // If not in the database, fetch and store it
            game = await GetOrFetchGameAsync(appId);
        }

        return await _neo4jRepository.GetRecommendationsForGameAsync(game);
    }
}
