using Microsoft.AspNetCore.Mvc;
using InteractiveCurator.WebAPI.Services.Interfaces;

namespace InteractiveCurator.WebAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("import-top-selling")]
        public async Task<IActionResult> ImportTopSellingGames()
        {
            await _gameService.ImportTopSellingGamesAsync();
            return Ok("Top-selling games imported successfully.");
        }

        [HttpGet("{appId}")]
        public async Task<IActionResult> GetGameDetails(string appId)
        {
            var game = await _gameService.GetOrFetchGameAsync(appId);
            if (game == null)
            {
                return NotFound($"No details found for game with App ID: {appId}");
            }

            return Ok(game);
        }

        [HttpGet("{appId}/recommendations")]
        public async Task<IActionResult> GetRecommendations(string appId)
        {
            var recommendations = await _gameService.GetRecommendationsAsync(appId);
            if (recommendations == null || recommendations.Count == 0)
            {
                return NotFound($"No recommendations found for game with App ID: {appId}");
            }

            return Ok(recommendations);
        }
    }
}
