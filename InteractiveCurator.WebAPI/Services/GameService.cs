using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Services.Interfaces;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly INeo4jRepository _neo4jRepository;
        private readonly ISteamApiService _steamApiService;

        public GameService(ILogger<GameService> logger, INeo4jRepository neo4jRepository, ISteamApiService steamApiService)
        {
            _logger = logger;
            _neo4jRepository = neo4jRepository;
            _steamApiService = steamApiService;
        }

        public async Task<List<GameDto>> CurateGamesAsync(List<string> games)
        {
            _logger.LogInformation("Starting game curation for input: {@Games}", games);

            // Handle null or empty input
            if (games == null || !games.Any())
            {
                _logger.LogWarning("No games provided for curation.");
                return new List<GameDto>();
            }

            try
            {
                var gameDetails = new List<Game>();
                foreach (var game in games)
                {
                    var details = await _steamApiService.GetGameDetailsAsync(game);
                    if (details != null)
                    {
                        _logger.LogInformation("Fetched details for game: {Game}", game);
                        gameDetails.Add(new Game
                        {
                            Name = details.Name,
                            SteamId = details.SteamId
                        });
                    }
                    else
                    {
                        _logger.LogWarning("No details found for game: {Game}", game);
                    }
                }

                var recommendations = await _neo4jRepository.GetRecommendedGamesAsync(gameDetails);

                _logger.LogInformation("Generated recommendations: {@Recommendations}", recommendations);

                // Convert Game to GameDto for the API response
                return recommendations.Select(r => new GameDto
                {
                    Name = r.Name,
                    SteamId = r.SteamId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while curating games");
                throw;
            }
        }

        public async Task<GameDto> GetGameDetailsAsync(string gameId)
        {
            _logger.LogInformation("Fetching details for game ID: {GameId}", gameId);

            try
            {
                var gameDetails = await _steamApiService.GetGameDetailsAsync(gameId);

                if (gameDetails != null)
                {
                    _logger.LogInformation("Successfully fetched details for game ID: {GameId}", gameId);
                }
                else
                {
                    _logger.LogWarning("No details found for game ID: {GameId}", gameId);
                }

                return gameDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching game details for ID: {GameId}", gameId);
                throw;
            }
        }
    }
}
