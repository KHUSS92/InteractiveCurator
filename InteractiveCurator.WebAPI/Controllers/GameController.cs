using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCurator.WebAPI.Controllers
{
    public class GameController
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GamesController : ControllerBase
        {
            private readonly IGameService _gameService;

            public GamesController(IGameService gameService)
            {
                _gameService = gameService;
            }

            [HttpPost("curate")]
            public async Task<IActionResult> CurateGames([FromBody] CurateGamesRequestDto input)
            {
                if (input == null || input.Games == null || !input.Games.Any())
                    return BadRequest("The input list of games cannot be empty.");

                try
                {
                    var recommendations = await _gameService.CurateGamesAsync(input.Games);
                    return Ok(recommendations);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpGet("{gameId}")]
            public async Task<IActionResult> GetGameDetails(string gameId)
            {
                if (string.IsNullOrEmpty(gameId))
                    return BadRequest("Game ID cannot be null or empty.");

                try
                {
                    var gameDetails = await _gameService.GetGameDetailsAsync(gameId);
                    if (gameDetails == null)
                        return NotFound($"No details found for game ID: {gameId}");

                    return Ok(gameDetails);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}
